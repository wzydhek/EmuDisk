using System;
using System.Text;

namespace EmuDisk
{
    internal class RSDosFormat : BaseDiskFormat, IDiskFormat
    {
        #region Constructors

        public RSDosFormat(IDiskImage diskImage)
        {
            this.DiskImage = diskImage;

            byte[] sector = this.DiskImage.ReadSector(17, 0, 17);
            int e = 0;
            for (int i = 0; i < sector.Length; i++)
            {
                if (sector[i] == 0 || sector[i] == 0xff)
                {
                    e = i;
                    break;
                }
            }

            if (e > 0)
            {
                this.BaseDiskLabel = Encoding.ASCII.GetString(sector.Subset(0, e)).Trim();
            }

            this.LogicalTracks = 35;
            this.LogicalHeads = 1;
            this.LogicalSectors = 18;
            this.LogicalSectorSize = 256;

            if (!(this.DiskImage.IsPartitioned))
            {
                this.LogicalTracks = this.DiskImage.PhysicalTracks;
                this.LogicalHeads = this.DiskImage.PhysicalHeads;
                this.LogicalSectors = this.DiskImage.PhysicalSectors;
                this.LogicalSectorSize = this.DiskImage.PhysicalSectorSize;
            }
        }

        #endregion

        #region IDiskFormat Members

        #region Public Properties

        public DiskFormatTypes DiskFormat
        {
            get
            {
                return DiskFormatTypes.RSDOSFormat;
            }
        }

        public bool IsValidFormat
        {
            get
            {
                return this.ValidateRSDOS();
            }
        }

        public override int FreeSpace
        {
            get
            {
                byte[] granuleMap = this.DiskImage.ReadSector(17, 0, 2);

                int granuleCount = ((this.LogicalTracks * this.LogicalHeads) - 1) * 2;
                if (granuleCount > 0x79)
                {
                    granuleCount = 0x79;
                }

                int freeGranuleCount = 0;
                for (int i = 0; i < granuleCount; i++)
                {
                    if (granuleMap[i] == 0xff)
                    {
                        freeGranuleCount++;
                    }
                }

                return freeGranuleCount * 9 * this.LogicalSectorSize;
            }
        }

        public override int TotalSpace
        {
            get
            {
                int granuleCount = ((this.LogicalTracks * this.LogicalHeads) - 1) * 2;
                if (granuleCount > 0x79)
                {
                    granuleCount = 0x79;
                }

                return granuleCount * 9 * this.LogicalSectorSize;
            }
        }

        public override string VolumeLabel
        {
            get
            {
                byte[] sector = ReadSector(17, 0, 17);
                StringBuilder sb = new StringBuilder();
                for (int i=0; i<sector.Length; i++)
                {
                    if (sector[i] == 0xff || sector[i] == 0)
                        break;
                    sb.Append(Encoding.ASCII.GetString(new byte[] { sector[i] }));
                }

                string name = sb.ToString();
                if (string.IsNullOrEmpty(name))
                    return null;
                return name;
            }
            set
            {
                if (value.Length > 255)
                    value = value.Substring(0, 255);
                byte[] namebytes = Encoding.ASCII.GetBytes(value);
                byte[] sector = new byte[DiskImage.PhysicalSectorSize].Initialize(0xFF);
                Array.Copy(namebytes, 0, sector, 0, namebytes.Length);
                sector[namebytes.Length] = 0x00;
                WriteSector(17, 0, 17, sector);
            }
        }

        #endregion

        #region Public Methods

        public override VirtualDirectory GetDirectory(int index)
        {
            VirtualDirectory dir = new VirtualDirectory();

            for (int s = 3; s < 19; s++)
            {
                byte[] buffer = ReadSector(17, 0, s);
                for (int i=0; i<8; i++)
                {
                    RSDosDirectoryEntry entry = new RSDosDirectoryEntry(buffer.Subset(i * 32, 32));
                    if (entry.Deleted)
                        continue;
                    if (entry.EoD)
                    {
                        s = 18;
                        break;
                    }

                    VirtualFile file = new VirtualFile();
                    file.Filename = entry.Filename;
                    file.Extension = entry.Extension;
                    file.Filesize = GetFileSize(entry.FirstGranule, entry.LastSectorBytes);
                    file.FileType = entry.FileType;
                    file.ASCII = entry.ASCII;

                    dir.Add(file);
                }
            }

            return dir;
        }

        public override string ToString()
        {
            return "RSDOS";
        }

        #endregion

        #endregion

        #region Private Methods

        private bool ValidateRSDOS()
        {
            byte[] granuleMap = this.DiskImage.ReadSector(17, 0, 2);
            int granuleCount = ((this.LogicalTracks * this.LogicalHeads) - 1) * 2;
            if (granuleCount > 0x79)
            {
                granuleCount = 0x79;
            }

            // Test for valid granule map
            for (int i = 0; i < granuleCount; i++)
            {
                if (granuleMap[i] != 0xff && (granuleMap[i] > granuleCount) && (granuleMap[i] > 0x80 && ((granuleMap[i] & 0x0f) < 0 || (granuleMap[i] & 0x0f) > 9)))
                {
                    return false;
                }
            }

            // Test for valid directory entries
            byte[] directorySectors = this.ReadSectors(17, 0, 3, 8);
            for (int i = 0; i < directorySectors.Length; i += 16)
            {
                if (directorySectors[i] == 0xff)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (directorySectors[i + j] != 0xff)
                        {
                            return false;
                        }
                    }
                }

                if (directorySectors[i] != 0xff && (directorySectors[i + 11] > 0x03 || (directorySectors[i + 12] != 00 && directorySectors[i + 12] != 0xff)))
                {
                    return false;
                }

                if (directorySectors[i] != 0xff && directorySectors[i] != 0xff && (!directorySectors.Subset(i, 11).IsASCII(false)))
                {
                    return false;
                }
            }

            bool directoryEmpty = true;
            for (int i = 0; i < 256; i++)
            {
                if (directorySectors[i] != 0)
                {
                    directoryEmpty = false;
                    break;
                }
            }

            return !directoryEmpty;
        }

        private int GetFileSize(int granule, int lastbytes)
        {
            int size = 0;
            byte[] granmap = ReadSector(17, 0, 2);
            int g = granmap[granule];
            while ((g & 0xC0) != 0xC0)
            {
                size += 9 * 256;
                granule = g;
                g = granmap[granule];
            }
            if (g > 0xC0)
            {
                g &= 0x0F;
                size += (g - 1) * 256;
            }

            size += lastbytes;
            return size;
        }

        #endregion
    }
}
