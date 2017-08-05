using Microsoft.Win32.SafeHandles;

namespace EmuDisk
{
    internal class PhysicalDisk : IDiskImage
    {
        #region Private Fields

        private string[] drives = new string[] { "A:", "B:" };
        private int drivenum;
        private SafeFileHandle handle;
        private int tracks = 35;
        private int heads = 1;
        private int sectors = 18;
        private int sectorsize = 256;

        #endregion

        #region Constructors

        public PhysicalDisk(int drive)
        {
            this.drivenum = drive;
            this.OpenDisk(drive);
        }

        #endregion

        #region Public Properties

        public static bool DriverInstalled
        {
            get
            {
                //uint dwRet = 0;
                //int version = 0;
                //uint paramSize = (uint)Marshal.SizeOf(version);
                //IntPtr param = Marshal.AllocHGlobal((int)paramSize);

                //SafeFileHandle handle = NativeMethods.CreateFile(@"\\.\fdrawcmd", NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, 0, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
                //NativeMethods.DeviceIoControl(handle, IOCTLFDRAWCMDGETVERSION, IntPtr.Zero, 0, param, paramSize, ref dwRet, IntPtr.Zero);

                //version = Marshal.ReadInt32(param);
                //Marshal.FreeHGlobal(param);
                //param = IntPtr.Zero;

                //if (version < FDRAWCMDVERSION)
                //{
                //    return false;
                //}

                //return true;

                return false;
            }
        }

        public DiskImageTypes ImageType
        {
            get
            {
                return DiskImageTypes.PhysicalDisk;
            }
        }

        #endregion

        #region Public IDiskImage Properties

        public string Filename
        {
            get { return string.Format(MainForm.ResourceManager.GetString("PhysicalDisk_DriveName", MainForm.CultureInfo), this.drives[this.drivenum]); }
        }

        public bool IsPartitioned
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        public bool IsValidImage
        {
            get
            {
                return this.handle != null && !this.handle.IsInvalid;
            }
        }

        public int HeaderLength
        {
            get
            {
                return 0;
            }

            set
            {
            }
        }

        public int PhysicalTracks
        {
            get
            {
                return this.tracks;
            }
        }

        public int PhysicalHeads
        {
            get
            {
                return this.heads;
            }

            set
            {
            }
        }

        public int PhysicalSectors
        {
            get
            {
                return this.sectors;
            }
        }

        public int PhysicalSectorSize
        {
            get
            {
                return this.sectorsize;
            }
        }

        public int Partitions
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        public Geometry DiskGeometry
        {
            get
            {
                return new Geometry(Partitions, PhysicalTracks, PhysicalHeads, PhysicalSectors, PhysicalSectorSize, false);
            }
        }

        public string DiskLabel
        {
            get { return null; }
            set { }
        }

        #endregion

        #region Public IDiskImage Methods

        public void SetPartition(int partition)
        {

        }

        public void Close()
        {
            if (this.handle != null)
            {
                NativeMethods.CloseHandle(this.handle);
            }

            this.handle = null;
        }

        public byte[] ReadBytes(int offset, int length)
        {
            return null;
        }

        public byte[] ReadSector(int track, int head, int sector)
        {
            return null;
        }

        public void WriteBytes(int offset, byte[] data)
        {

        }

        public void WriteSector(int track, int head, int sector, byte[] data)
        {

        }

        public void FormatSector(int track, int head, int sector, byte filldata)
        {

        }

        public void CreateDisk(string filename, int tracks, int heads, int sectors, int sectorsize, byte filldata)
        {

        }

        public override string ToString()
        {
            return "Floppy";
        }

        #endregion

        #region Private Methods

        private void OpenDisk(int drive)
        {

        }

        #endregion
    }
}
