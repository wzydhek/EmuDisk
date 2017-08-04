using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace EmuDisk
{
    /// <summary>
    /// Physical Floppy Disk Support
    /// </summary>
    internal class PhysicalDisk : IDiskImage
    {
        #region Private Fields

        /// <summary>
        /// Physical Drive Names
        /// </summary>
        private string[] drives = new string[] { "A:", "B:" };

        /// <summary>
        /// Logical drive number
        /// </summary>
        private int drivenum;

        /// <summary>
        /// Handle to opened physical drive
        /// </summary>
        private SafeFileHandle handle;

        /// <summary>
        /// Physical Tracks
        /// </summary>
        private int tracks = 35;

        /// <summary>
        /// Physical Heads
        /// </summary>
        private int heads = 1;

        /// <summary>
        /// Physical Sectors
        /// </summary>
        private int sectors = 18;

        /// <summary>
        /// Physical Sector Size
        /// </summary>
        private int sectorsize = 256;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDisk"/> class
        /// </summary>
        /// <param name="drive">Drive Number</param>
        public PhysicalDisk(int drive)
        {
            this.drivenum = drive;
            this.OpenDisk(drive);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the FDRAWCMD.SYS driver is installed and meets the minimum version
        /// </summary>
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

        /// <summary>
        /// Gets Disk Image Type
        /// </summary>
        public DiskImageTypes ImageType
        {
            get
            {
                return DiskImageTypes.PhysicalDisk;
            }
        }

        #endregion

        #region Public IDiskImage Properties

        /// <summary>
        /// Gets Physical Drive Name
        /// </summary>
        public string Filename
        {
            get { return string.Format(MainForm.ResourceManager.GetString("PhysicalDisk_DriveName", MainForm.CultureInfo), this.drives[this.drivenum]); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the disk image is partitioned (PartitionedVHDImage)
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether the data on the disk image is valid for the given disk image class
        /// </summary>
        public bool IsValidImage
        {
            get
            {
                return this.handle != null && !this.handle.IsInvalid;
            }
        }

        /// <summary>
        /// Gets or sets size of disk image's header
        /// </summary>
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

        /// <summary>
        /// Gets the number of cylinders
        /// </summary>
        public int PhysicalTracks
        {
            get
            {
                return this.tracks;
            }
        }

        /// <summary>
        /// Gets or sets the number of sides
        /// </summary>
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

        /// <summary>
        /// Gets the number of sectors per track
        /// </summary>
        public int PhysicalSectors
        {
            get
            {
                return this.sectors;
            }
        }

        /// <summary>
        /// Gets the size of a sector in bytes
        /// </summary>
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

        public virtual Geometry DiskGeometry
        {
            get
            {
                return new Geometry(Partitions, PhysicalTracks, PhysicalHeads, PhysicalSectors, PhysicalSectorSize, false);
            }
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

        /// <summary>
        /// Opens and creates a handle to a Physical Drive
        /// </summary>
        /// <param name="drive">Drive Number</param>
        private void OpenDisk(int drive)
        {

        }

        #endregion
    }
}
