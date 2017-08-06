using System;

namespace EmuDisk
{
    public class OS9Format : BaseDiskFormat, IDiskFormat
    {
        #region Private Fields

        private LSN0 lsn0;

        #endregion

        #region Constructors

        public OS9Format(IDiskImage diskImage)
        {
            this.DiskImage = diskImage;
            this.ValidateOS9();
            this.BaseDiskLabel = this.lsn0.DD_NAM;
        }

        #endregion

        #region Public Properties

        public LSN0 Lsn0
        {
            get
            {
                return this.lsn0;
            }
        }

        #endregion

        #region IDiskFormat Members

        #region Public Properties

        public DiskFormatTypes DiskFormat
        {
            get
            {
                return DiskFormatTypes.OS9Format;
            }
        }

        public bool IsValidFormat
        {
            get
            {
                return this.ValidateOS9();
            }
        }

        public override int FreeSpace
        {
            get
            {
                byte[] bitmap = this.GetBitmap();
                return this.GetFreeClusers(bitmap) * this.LogicalSectorSize * this.lsn0.DD_BIT;
            }
        }

        public override int TotalSpace
        {
            get
            {
                return lsn0.DD_TOT * this.LogicalSectorSize;
            }
        }

        public override string VolumeLabel
        {
            get
            {
                return lsn0.DD_NAM;
            }
            set
            {
                lsn0.DD_NAM = value;
                WriteLSN(0, lsn0.Bytes);
            }
        }

        #endregion

        #region Public Methods

        public override VirtualDirectory GetDirectory(int index)
        {
            VirtualDirectory dir = new VirtualDirectory();
            if (index == 0)
                index = lsn0.DD_DIR;

            OS9FileDescriptor dirdesc = new OS9FileDescriptor(ReadLSN(index));
            int dirsize = dirdesc.FD_SIZ;

            for (int s = 0; s < 48; s++)
            {
                int dsize = 0;
                OS9FileSegment seg = dirdesc.FD_SEG[s];
                if (seg.Sectors == 0)
                    break;

                for (int i=0; i<seg.Sectors; i++)
                {
                    byte[] sector = ReadLSN(seg.LSN + i);

                    for (int j=0; j<8; j++)
                    {
                        dsize += 0x20;
                        if (dsize > dirsize)
                        {
                            i = seg.Sectors;
                            s = 48;
                            break;
                        }
                        OS9DirectoryEntry entry = new OS9DirectoryEntry(sector.Subset(j * 32, 32));
                        //if (entry.EoD)
                        //{
                        //    i = seg.Sectors;
                        //    s = 48;
                        //    break;
                        //}
                        if (entry.Deleted)
                            continue;

                        if (entry.Filename == "." || entry.Filename == "..")
                            continue;

                        OS9FileDescriptor filedesc = new OS9FileDescriptor(ReadLSN(entry.LSN));
                        int siz = filedesc.FD_SIZ;

                        VirtualFile file = new VirtualFile();
                        file.Filename = entry.Filename;
                        file.Filesize = filedesc.FD_SIZ;
                        file.Attr = filedesc.FD_ATT;
                        file.Created = filedesc.FD_CREAT;
                        file.Modified = filedesc.FD_DAT;
                        file.LSN = entry.LSN;
                        file.ParentLSN = index;

                        dir.Add(file);
                    }
                }
            }

            return dir;
        }

        public override string ToString()
        {
            return "OS9";
        }

        #endregion

        #endregion

        #region Private Methods

        private bool ValidateOS9()
        {
            this.lsn0 = new LSN0(this.ReadLSN(0));

            if (this.lsn0.DD_FMT == 0x82)
            {
                this.LogicalHeads = 1;
            }
            else
            {
                this.LogicalHeads = ((this.lsn0.DD_FMT & 0x01) > 0) ? 2 : 1;
            }

            this.LogicalSectors = this.lsn0.DD_SPT;
            this.LogicalSectorSize = this.lsn0.DD_LSNSize;
            if (this.LogicalSectorSize == 0)
            {
                this.LogicalSectorSize = 256;
            }

            if (this.lsn0.DD_TOT == 0 || this.LogicalSectors == 0)
            {
                return false;
            }

            this.LogicalTracks = this.lsn0.DD_TOT / this.LogicalSectors / this.LogicalHeads;

            int bitmapLSNs = (this.lsn0.DD_MAP + (this.LogicalSectorSize - 1)) / this.LogicalSectorSize;
            if (1 + bitmapLSNs > this.lsn0.DD_DIR)
            {
                return false;
            }

            if (this.LogicalSectors != this.lsn0.DD_TKS)
            {
                return false;
            }

            if (this.LogicalSectors == 0)
            {
                return false;
            }

            if (this.lsn0.DD_TOT % this.LogicalSectors > 0)
            {
                return false;
            }

            byte[] bitmap = this.GetBitmap();

            for (int i = 0; i < bitmapLSNs; i++)
            {
                byte b = bitmap[i / 8];
                byte mask = (byte)(1 << (7 - (i % 8)));
                if ((b & mask) == 0)
                {
                    return false;
                }
            }

            if (this.lsn0.DD_TOT != this.LogicalTracks * this.LogicalHeads * this.LogicalSectors)
            {
                return false;
            }

            return true;
        }

        private byte[] GetBitmap()
        {
            byte[] bitmap = new byte[this.lsn0.DD_MAP].Initialize(0xff);
            int bitmapSectors = (this.lsn0.DD_MAP + (this.LogicalSectorSize - 1)) / this.LogicalSectorSize;
            return this.ReadLSNs(1, bitmapSectors);
        }

        private int GetFreeClusers(byte[] bitmap)
        {
            int freeLSNs = 0;

            for (int i = 0; i < this.lsn0.DD_TOT; i++)
            {
                byte b = bitmap[i / 8];
                b >>= 7 - (i % 8);
                freeLSNs += ~b & 1;
            }

            return freeLSNs;
        }

        #endregion
    }
}
