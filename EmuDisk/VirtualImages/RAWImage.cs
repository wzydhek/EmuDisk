namespace EmuDisk
{
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
