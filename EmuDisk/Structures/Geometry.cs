namespace EmuDisk
{
    public class Geometry
    {
        #region Public Properties

        public int Partitons { get; set; }

        public int Tracks { get; set; }

        public int Heads { get; set; }

        public int Sectors { get; set; }

        public int SectorSize { get; set; }

        public bool WriteProtect { get; set; }

        #endregion

        #region Constructors

        public Geometry(int partitions, int tracks, int heads, int sectors, int sectorsize, bool writeprotect)
        {
            this.Partitons = partitions;
            this.Tracks = tracks;
            this.Heads = heads;
            this.Sectors = sectors;
            this.SectorSize = sectorsize;
            this.WriteProtect = writeprotect;
        }

        #endregion
    }
}
