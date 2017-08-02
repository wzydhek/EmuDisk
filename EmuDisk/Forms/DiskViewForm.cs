using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmuDisk
{
    /// <summary>
    /// Form used to show the disk's directories and files.
    /// </summary>
    public partial class DiskViewForm : Form
    {
        #region Private Fields

        /// <summary>
        /// Contains the Disk Format instance
        /// </summary>
        private IDiskFormat diskFormat;

        #endregion

        #region Constructors

        public DiskViewForm(IDiskFormat diskFormat)
        {
            this.InitializeComponent();
            this.diskFormat = diskFormat;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the disk's format
        /// </summary>
        public IDiskFormat DiskFormat
        {
            get
            {
                return this.diskFormat;
            }

            set
            {
                this.diskFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the Partition Menu Items for this instance
        /// </summary>
        public ToolStripMenuItem PartitionItems { get; set; }

        #endregion
    }
}
