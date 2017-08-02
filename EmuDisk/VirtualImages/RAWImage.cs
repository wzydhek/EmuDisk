using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuDisk
{
    /// <summary>
    /// RAW Disk Image support
    /// </summary>
    internal class RAWImage : DiskImageBase, IDiskImage
    {
        #region Constructors

        public RAWImage(string filename) : base(filename)
        {
            this.GetDiskInfo();
        }

        public RAWImage(string filename, int tracks, int heads, int sectors, int sectorsize, byte filldata)
        {
            this.CreateDisk(filename, tracks, heads, sectors, sectorsize, filldata);
            this.GetDiskInfo();
        }

        #endregion

        #region Public Properties

        public override DiskImageTypes ImageType
        {
            get
            {
                return DiskImageTypes.RAWImage;
            }
        }

        #endregion

        #region Private Methods

        private void GetDiskInfo()
        {
            this.physicalTracks = 35;
            this.physicalHeads = 1;
            this.physicalSectors = 18;
            this.physicalSectorSize = 256;

            this.headerLength = (int)this.Length % 256;
            if (this.HeaderLength != 0)
                goto NotValid;

            //this.Seek(0);
            //LSN0 lsn0 = new LSN0(this.ReadSector(0, 0, 1));

            //this.physicalSectors = lsn0.SectorsPerTrack;
            //this.physicalHeads = ((lsn0.DiskFormat & 0x01) > 0) ? 2 : 1;
            //if (this.PhysicalHeads != 1 && this.physicalHeads != 2)
            //    goto NotValid;
            //this.physicalTracks = lsn0.TotalSectors / this.physicalSectors / this.physicalHeads;
            //if (lsn0.TotalSectors != this.PhysicalTracks * this.PhysicalSectors * this.PhysicalHeads)
            //    goto NotValid;

            //if (this.Length != lsn0.TotalSectors * this.PhysicalSectorSize)
            //    goto NotValid;

            this.isValidImage = true;
            return;

        NotValid:
            this.isValidImage = false;
            this.Close();

        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return "RAW";
        }

        #endregion
    }
}
