using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmuDisk
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseDiskFormat
    {
        #region Private Properties

        /// <summary>
        /// Contains the disk's volume label
        /// </summary>
        private string diskLabel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the underlying disk image
        /// </summary>
        public IDiskImage DiskImage { get; set; }

        /// <summary>
        /// Gets or sets the volume label
        /// </summary>
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

        /// <summary>
        /// Gets or sets the logical number of cylinders
        /// </summary>
        public int LogicalTracks { get; set; }

        /// <summary>
        /// Gets or sets the logical number of sides
        /// </summary>
        public int LogicalHeads { get; set; }

        /// <summary>
        /// Gets or sets the logical number of sectors
        /// </summary>
        public int LogicalSectors { get; set; }

        /// <summary>
        /// Gets or sets the logical size of sectors in bytes
        /// </summary>
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
