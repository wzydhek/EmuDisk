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

        #endregion

        #region Form Events

        private void DiskViewForm_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = this.diskFormat.DiskImage.ToString();
            toolStripStatusLabel2.Text = this.diskFormat.ToString();
        }

        private void DiskViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.diskFormat.DiskImage.Close();
        }

        #endregion

    }
}
