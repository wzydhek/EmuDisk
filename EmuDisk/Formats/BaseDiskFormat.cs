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

        public new virtual string ToString()
        {
            return "Unknown";
        }

        #endregion
    }
}
