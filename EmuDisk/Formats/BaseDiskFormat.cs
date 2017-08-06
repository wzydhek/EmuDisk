using System;

namespace EmuDisk
{
    public class BaseDiskFormat
    {
        #region Private Properties

        private string diskLabel;

        #endregion

        #region Public Properties

        public IDiskImage DiskImage { get; set; }

        internal string BaseDiskLabel
        {
            get
            {
                return this.diskLabel;
            }

            set
            {
                this.diskLabel = value;
            }
        }

        public int LogicalTracks { get; set; }

        public int LogicalHeads { get; set; }

        public int LogicalSectors { get; set; }

        public int LogicalSectorSize { get; set; }

        public virtual int FreeSpace
        {
            get { return 0; }
        }

        public virtual int TotalSpace
        {
            get { return 0; }
        }

        public virtual string VolumeLabel
        {
            get { return null; }
            set { }
        }

        #endregion

        #region Public Methods

        public virtual VirtualDirectory GetDirectory(int index)
        {
            return new VirtualDirectory();
        }

        public virtual byte[] GetFile(VirtualFile file)
        {
            return null;
        }

        public byte[] ReadSector(int track, int head, int sector)
        {
            return DiskImage.ReadSector(track, head, sector);
        }

        public byte[] ReadSectors(int track, int head, int sector, int sectorCount)
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

        public void WriteSector(int track, int head, int sector, byte[] data)
        {
            DiskImage.WriteSector(track, head, sector, data);
        }

        public void WriteSectors(int track, int head, int sector, byte[] data)
        {
            int ptr = 0;

            while (ptr < data.Length)
            {
                WriteSector(track, head, sector, data.Subset(ptr, this.LogicalSectorSize));
                ptr += this.LogicalSectorSize;

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
        }

        public byte[] ReadLSN(int lsn)
        {
            if (lsn == 0)
                return DiskImage.ReadSector(0, 0, 1);

            int track = lsn / (LogicalSectors * LogicalHeads);
            int head = (lsn / LogicalSectors) % LogicalHeads;
            int sector = (lsn % LogicalSectors) + 1;

            return DiskImage.ReadSector(track, head, sector);
        }

        public byte[] ReadLSNs(int lsn, int count)
        {
            byte[] buffer = new byte[count * this.LogicalSectorSize];

            for (int i = 0, bufferOffset = 0; i < count; i++, bufferOffset += this.LogicalSectorSize)
            {
                Array.Copy(this.ReadLSN(lsn + i), 0, buffer, bufferOffset, this.LogicalSectorSize);
            }

            return buffer;
        }

        public void WriteLSN(int lsn, byte[] data)
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

        public void WriteLSNs(int lsn, int count, byte[] data)
        {
            byte[] buffer = new byte[this.LogicalSectorSize];

            for (int i = 0, bufferOffset = 0; i < count; i++, bufferOffset += this.LogicalSectorSize)
            {
                Array.Copy(data, bufferOffset, buffer, 0, this.LogicalSectorSize);
                WriteLSN(lsn + i, buffer);
            }
        }

        public new virtual string ToString()
        {
            return "Unknown";
        }

        #endregion
    }
}
