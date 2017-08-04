using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmuDisk
{
    /// <summary>
    /// RS Dos virtual disk support
    /// </summary>
    internal class RSDosFormat : BaseDiskFormat, IDiskFormat
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RSDosFormat"/> class.
        /// </summary>
        /// <param name="diskImage">Underlying virtual disk image class</param>
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

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return "RSDOS";
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Read a series of sectors from the disk
        /// </summary>
        /// <param name="track">Beginning cylinder</param>
        /// <param name="head">Beginning side</param>
        /// <param name="sector">Beginning sector</param>
        /// <param name="sectorCount">Number of consecutive sectors to read</param>
        /// <returns>Array of bytes read from the disk</returns>
        private byte[] ReadSectors(int track, int head, int sector, int sectorCount)
        {
            byte[] buffer = new byte[sectorCount * this.LogicalSectorSize];

            for (int i = 0; i < sectorCount; i++)
            {
                Array.Copy(this.DiskImage.ReadSector(track, head, sector), 0, buffer, i * this.LogicalSectorSize, this.LogicalSectorSize);
                sector++;
                if (sector > this.LogicalSectors)
                {
                    sector = 1;
                    head++;
                    if (head > this.LogicalHeads - 1)
                    {
                        head = 0;
                        track++;
                        if (track > this.LogicalTracks - 1)
                        {
                            track = 0;
                        }
                    }
                }
            }

            return buffer;
        }

        /// <summary>
        /// Validate the disk's data is a valid RS Dos format
        /// </summary>
        /// <returns>A value specifying the validation was successful or not</returns>
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

        #endregion
    }
}
