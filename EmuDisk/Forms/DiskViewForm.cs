using System;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace EmuDisk
{
    public partial class DiskViewForm : Form
    {
        #region Private Fields

        private IDiskFormat diskFormat;
        private ListViewColumnSorter lvwColumnSorter;
        private int currentDirectory;

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

        public void DiskViewForm_Load(object sender, EventArgs e)
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
            else if (DiskFormat is RSDosFormat)
            {
                listView.Columns.Add("Filename", 145);
                listView.Columns.Add("Ext", 70);
                listView.Columns.Add("Size");
                listView.Columns.Add("Attr");
            }
            else if (DiskFormat is DragonDosFormat)
            {
                listView.Columns.Add("Filename", 145);
                listView.Columns.Add("Ext", 70);
                listView.Columns.Add("Size");
            }

            currentDirectory = 0;
            UpdateView();
        }

        public void DiskViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.diskFormat.DiskImage.Close();
        }

        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.ForeColor = SystemColors.WindowText;
                treeView.SelectedNode.BackColor = SystemColors.Window;
            }
            e.Node.ForeColor = SystemColors.Window;
            e.Node.BackColor = SystemColors.Highlight;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            currentDirectory = (int)e.Node.Tag;
            UpdateListView();
        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (((e.State & TreeNodeStates.Selected) != 0) && (!treeView.Focused))
            {
                e.Node.ForeColor = SystemColors.Window;
                e.Node.BackColor = SystemColors.Highlight;
            }
            else
                e.DrawDefault = true;
        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
                ColumnSortType sorttype = ColumnSortType.Alphanumeric;

                if (DiskFormat is OS9Format)
                {
                    switch (e.Column)
                    {
                        case 1:
                            sorttype = ColumnSortType.Numeric;
                            break;
                        case 3:
                        case 4:
                            sorttype = ColumnSortType.Date;
                            break;
                        default:
                            sorttype = ColumnSortType.Alphanumeric;
                            break;
                    }
                }
                else if (DiskFormat is RSDosFormat || DiskFormat is DragonDosFormat)
                {
                    if (e.Column == 2)
                        sorttype = ColumnSortType.Numeric;
                    else
                        sorttype = ColumnSortType.Alphanumeric;
                }

                lvwColumnSorter.SortType = sorttype;
            }

            // Perform the sort with these new sort options.
            this.listView.Sort();
        }

        #endregion

        #region Private Methods

        private void UpdateView()
        {
            if (DiskFormat is RSDosFormat || DiskFormat is DragonDosFormat)
            {
                treeView.Visible = false;
                splitContainer1.Panel1Collapsed = true;

                UpdateListView();
            }
            else
            {
                treeView.Visible = true;
                splitContainer1.Panel1Collapsed = false;

                UpdateTreeView();
            }
        }

        private void UpdateListView()
        {
            listView.Items.Clear();
            VirtualDirectory dir = DiskFormat.GetDirectory(currentDirectory);
            foreach (VirtualFile file in dir)
            {
                if (!file.IsDirectory)
                {
                    ListViewItem li;
                    switch (DiskFormat.DiskFormat)
                    {
                        case DiskFormatTypes.OS9Format:
                            li = new ListViewItem(new string[] { file.Filename, file.Filesize.ToString(), OS9AttrToString(file.Attr), file.Created.ToShortDateString(), file.Modified.ToShortDateString() });
                            listView.Items.Add(li);
                            break;
                        case DiskFormatTypes.RSDOSFormat:
                            li = new ListViewItem(new string[] { file.Filename, file.Extension, file.Filesize.ToString(), RSAttrToString(file.FileType, file.ASCII) });
                            listView.Items.Add(li);
                            break;
                        case DiskFormatTypes.DragonDosFormat:
                            li = new ListViewItem(new string[] { file.Filename, file.Extension, file.Filesize.ToString() });
                            listView.Items.Add(li);
                            break;
                    }
                }
            }
        }

        private void UpdateTreeView()
        {
            treeView.Nodes.Clear();
            TreeNode tn = new TreeNode();
            tn.Text = "Root";
            tn.Tag = 0;
            treeView.Nodes.Add(tn);

            VirtualDirectory dir = DiskFormat.GetDirectory(currentDirectory);
            ParseTree(tn, dir);
            tn.Expand();
            treeView.SelectedNode = tn;
        }

        private void ParseTree(TreeNode tn, VirtualDirectory dir)
        {
            foreach (VirtualFile file in dir)
            {
                if (file.IsDirectory)
                {
                    TreeNode t = new TreeNode();
                    t.Text = file.Filename;
                    t.Tag = file.LSN;
                    tn.Nodes.Add(t);

                    VirtualDirectory d = DiskFormat.GetDirectory(file.LSN);
                    ParseTree(t, d);
                }
            }
        }

        private string OS9AttrToString(int attr)
        {
            string b = "DSEWRewr";
            StringBuilder sb = new StringBuilder();

            for (int i=0; i<8; i++)
            {
                int a = attr;
                a >>= 7 - (i % 8);
                if ((a & 1) > 0)
                    sb.Append(b[i]);
                else
                    sb.Append("-");
            }
            return sb.ToString();
        }
        
        private string RSAttrToString(RSDosFileTypes type, bool ascii)
        {
            string a = string.Format("{0} {1}", (int)type, ascii ? "A" : "B");
            return a;
        }

        #endregion

    }
}
