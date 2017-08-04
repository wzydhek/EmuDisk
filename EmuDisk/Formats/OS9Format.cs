using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuDisk
{
    /// <summary>
    /// OS9 virtual disk support
    /// </summary>
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
            this.BaseDiskLabel = this.lsn0.VolumeName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the disk's Logical Sector 0
        /// </summary>
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
                return this.GetFreeClusers(bitmap) * this.LogicalSectorSize * this.lsn0.ClusterSize;
            }
        }

        public override int TotalSpace
        {
            get
            {
                return lsn0.TotalSectors * this.LogicalSectorSize;
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return "OS9";
        }

        #endregion

        #endregion

        #region Private Methods

        private byte[] ReadLSN(int lsn)
        {
            if (lsn == 0)
                return DiskImage.ReadSector(0, 0, 1);

            int track = lsn / (LogicalSectors * LogicalHeads);
            int head = (lsn / LogicalSectors) % LogicalHeads;
            int sector = (lsn % LogicalSectors) + 1;

            return DiskImage.ReadSector(track, head, sector);
        }

        private byte[] ReadLSNs(int lsn, int count)
        {
            byte[] buffer = new byte[count * this.LogicalSectorSize];

            for (int i = 0, bufferOffset = 0; i < count; i++, bufferOffset += this.LogicalSectorSize)
            {
                Array.Copy(this.ReadLSN(lsn + i), 0, buffer, bufferOffset, this.LogicalSectorSize);
            }

            return buffer;
        }

        private void WriteLSN(int lsn, byte[] data)
        {
            if (lsn == 0)
                DiskImage.WriteSector(0, 0, 1, data);
            else
            {
                int track = lsn / (LogicalSectors * LogicalHeads);
                int head = (lsn / LogicalSectors) % LogicalHeads;
                int sector = (lsn % LogicalSectors) + 1;

                DiskImage.WriteSector(track, head, sector, data);
            }
        }

        private void WriteLSNs(int lsn, int count, byte[] data)
        {
            byte[] buffer = new byte[this.LogicalSectorSize];

            for (int i = 0, bufferOffset = 0; i < count; i++, bufferOffset += this.LogicalSectorSize)
            {
                Array.Copy(data, bufferOffset, buffer, 0, this.LogicalSectorSize);
                WriteLSN(lsn + i, buffer);
            }
        }

        private bool ValidateOS9()
        {
            this.lsn0 = new LSN0(this.ReadLSN(0));

            if (this.lsn0.DiskFormat == 0x82)
            {
                this.LogicalHeads = 1;
            }
            else
            {
                this.LogicalHeads = ((this.lsn0.DiskFormat & 0x01) > 0) ? 2 : 1;
            }

            this.LogicalSectors = this.lsn0.SectorsPerTrack;
            this.LogicalSectorSize = this.lsn0.SectorSize;
            if (this.LogicalSectorSize == 0)
            {
                this.LogicalSectorSize = 256;
            }

            if (this.lsn0.PathDescriptor.SegmentAllocationSize == 0)
            {
                this.lsn0.PathDescriptor.SegmentAllocationSize = 8;
            }

            if (this.lsn0.TotalSectors == 0 || this.LogicalSectors == 0)
            {
                return false;
            }

            this.LogicalTracks = this.lsn0.TotalSectors / this.LogicalSectors / this.LogicalHeads;

            int bitmapLSNs = (this.lsn0.MapBytes + (this.LogicalSectorSize - 1)) / this.LogicalSectorSize;
            if (1 + bitmapLSNs > this.lsn0.RootDirectory)
            {
                return false;
            }

            if (this.LogicalSectors != this.lsn0.TrackSize)
            {
                return false;
            }

            if (this.LogicalSectors == 0)
            {
                return false;
            }

            if (this.lsn0.TotalSectors % this.LogicalSectors > 0)
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

            if (this.lsn0.TotalSectors != this.LogicalTracks * this.LogicalHeads * this.LogicalSectors)
            {
                return false;
            }

            return true;
        }

        private byte[] GetBitmap()
        {
            byte[] bitmap = new byte[this.lsn0.MapBytes].Initialize(0xff);
            int bitmapSectors = (this.lsn0.MapBytes + (this.LogicalSectorSize - 1)) / this.LogicalSectorSize;
            return this.ReadLSNs(1, bitmapSectors);
        }

        private int GetFreeClusers(byte[] bitmap)
        {
            int freeLSNs = 0;

            for (int i = 0; i < this.lsn0.TotalSectors; i++)
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
