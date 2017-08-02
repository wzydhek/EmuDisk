using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace EmuDisk
{
    /// <summary>
    /// VHD Disk Image support
    /// </summary>
    internal class VHDImage : DiskImageBase, IDiskImage
    {
        #region Private Properties

        private const int rsdospart = 161280;

        private int partitions = 0;
        private int currentPartition = 0;
        private int firstPartitonSize = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VHDImage"/> class
        /// </summary>
        /// <param name="filename">Disk image filename</param>
        public VHDImage(string filename) : base(filename)
        {
            this.GetDiskInfo();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Disk Image Type
        /// </summary>
        public override DiskImageTypes ImageType
        {
            get
            {
                return DiskImageTypes.VHDImage;
            }
        }

        public override bool IsPartitioned
        {
            get
            {
                return (partitions > 0);
            }
        }

        public override int Partitions {
            get
            {
                return partitions;
            }
            set
            {
                partitions = value;
            }
        }

        #endregion

        #region Public Methods

        public override void SetPartition(int partition)
        {
            if (IsPartitioned)
            {
                this.currentPartition = partition;
            }
        }

        public override byte[] ReadSector(int track, int head, int sector)
        {
            int offset = CalculateOffset(track, head, sector);
            return this.ReadBytes(offset, this.PhysicalSectorSize);
        }

        public override void WriteSector(int track, int head, int sector, byte[] data)
        {
            int offset = CalculateOffset(track, head, sector);
            this.WriteBytes(offset, data);
        }

        public override void CreateDisk(string filename, int tracks, int heads, int sectors, int sectorsize, byte filldata)
        {
            int size = firstPartitonSize + (rsdospart * partitions);

            if (this.baseStream != null)
            {
                this.baseStream.Close();
                this.baseStream = null;
            }

            this.filename = filename;

            try
            {
                this.baseStream = File.Open(this.filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                physicalTracks = tracks;
                physicalHeads = heads;
                physicalSectors = sectors;
                physicalSectorSize = sectorsize;

                this.baseStream.Write(new byte[firstPartitonSize], 0, firstPartitonSize);
                for (int i = 0; i < Partitions; i++)
                    this.baseStream.Write(new byte[rsdospart], 0, rsdospart);

            }
            catch (IOException)
            {
                MessageBox.Show(string.Format(MainForm.ResourceManager.GetString("DiskImageBase_FileOpenError", MainForm.CultureInfo), this.filename), MainForm.ResourceManager.GetString("DiskImageBase_FileOpenErrorCaption", MainForm.CultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.filename = string.Empty;
            }
        }

        public override string ToString()
        {
            return "VHD";
        }

        #endregion

        #region Private Methods

        private void GetDiskInfo()
        {
            this.physicalTracks = 35;
            this.physicalHeads = 1;
            this.physicalSectors = 18;
            this.physicalSectorSize = 256;
            this.partitions = 0;
            this.currentPartition = 0;
            this.firstPartitonSize = 0;

            this.headerLength = (int)this.Length % 256;
            if (this.HeaderLength != 0)
                goto NotValid;

            LSN0 lsn0 = new LSN0(this.ReadSector(0, 0, 1));
            int totalSectors = lsn0.TotalSectors;
            if (((this.Length - (totalSectors * this.PhysicalSectorSize)) % rsdospart) == 0)
            {
                firstPartitonSize = totalSectors * this.PhysicalSectorSize;
                partitions = (((int)this.Length - firstPartitonSize) / rsdospart) + 1;
            }
            else
            {
                if (this.Length % rsdospart != 0)
                    goto NotValid;
                partitions = (int)this.Length / rsdospart;
            }

            this.isValidImage = true;
            return;

        NotValid:
            this.isValidImage = false;
            this.Close();

        }

        private int CalculateOffset(int track, int head, int sector)
        {
            int offset = 0;
            if (currentPartition != 0)
            {
                if (firstPartitonSize != 0)
                {
                    offset += firstPartitonSize;
                    offset += (currentPartition - 1) * rsdospart;
                }
                else
                {
                    offset += currentPartition * rsdospart;
                }
            }

            offset += (track * this.PhysicalHeads * this.PhysicalSectors * this.PhysicalSectorSize) + (head * this.PhysicalSectors * this.PhysicalSectorSize) + ((sector - 1) * this.PhysicalSectorSize);
            return offset;
        }

        #endregion
    }
}
