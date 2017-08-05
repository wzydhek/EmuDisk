using System;
using System.Windows.Forms;

namespace EmuDisk
{
    public partial class DiskViewForm : Form
    {
        #region Private Fields

        private IDiskFormat diskFormat;
        private ListViewColumnSorter lvwColumnSorter;

        #endregion

        #region Constructors

        public DiskViewForm(IDiskFormat diskFormat)
        {
            this.InitializeComponent();
            this.diskFormat = diskFormat;

            lvwColumnSorter = new ListViewColumnSorter();
            this.listView.ListViewItemSorter = lvwColumnSorter;
        }

        #endregion

        #region Public Properties

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
            this.Text = this.diskFormat.DiskImage.Filename;
            Geometry g = this.diskFormat.DiskImage.DiskGeometry;

            // Show Image and Format types in status bar
            toolStripStatusLabel1.Text = this.diskFormat.DiskImage.ToString();
            toolStripStatusLabel2.Text = this.diskFormat.ToString();
            toolStripStatusLabel3.Text = string.Format("{0}P:{1} T:{2} H:{3} S:{4} SS:{5}", g.WriteProtect ? "[RO] " : "", g.Partitons, g.Tracks, g.Heads, g.Sectors, g.SectorSize);
            toolStripStatusLabel4.Text = string.Format("{0} of {1} bytes free", diskFormat.FreeSpace, diskFormat.TotalSpace);

            this.Text = diskFormat.DiskImage.Filename;
            if (!string.IsNullOrEmpty(diskFormat.DiskImage.DiskLabel))
                this.Text += " {" + diskFormat.DiskImage.DiskLabel + "}";
            if (!string.IsNullOrEmpty(diskFormat.VolumeLabel))
                this.Text += " [" + diskFormat.VolumeLabel + "]";

            treeView.Nodes.Clear();
            listView.Items.Clear();

            if (diskFormat is OS9Format)
            {
                listView.Columns.Add("Filename", 145);
                listView.Columns.Add("Size");
                listView.Columns.Add("Attr", 70);
                listView.Columns.Add("Created", 70);
                listView.Columns.Add("Modified", 105);
                this.Width = 760;
            }
            else
            {
                listView.Columns.Add("Filename", 145);
                listView.Columns.Add("Size");
                listView.Columns.Add("Attr");
            }


        }

        private void DiskViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.diskFormat.DiskImage.Close();
        }

        #endregion

    }
}
