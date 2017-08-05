using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;

namespace EmuDisk
{
    /// <summary>
    /// Main Application Form
    /// </summary>
    public partial class MainForm : Form, IMRUClient
    {
        #region Private Fields

        /// <summary>
        /// Registry path to store application settings
        /// </summary>
        private const string RegistryPath = "Software\\Zydhek\\EmuDisk";

        /// <summary>
        /// Current Culture Information
        /// </summary>
        private static CultureInfo cultureInfo;

        /// <summary>
        /// Resource Manager used to get localized strings
        /// </summary>
        private static ResourceManager resourceManager;

        /// <summary>
        /// True if FDRAWCMD.SYS driver is installed
        /// </summary>
        private static bool driverInstalled = PhysicalDisk.DriverInstalled;

        /// <summary>
        /// MRU List manager
        /// </summary>
        private MRUManager mruManager;

        #endregion

        #region Constructors

        public MainForm()
        {
            resourceManager = new ResourceManager("EmuDisk.Resource.Resource", typeof(MainForm).Assembly);
            this.InitializeComponent();
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if (cultureInfo == null)
            {
                this.SetLanguage("es");
            }

            this.mruManager = new MRUManager();
            this.mruManager.Initialize(this, this.mnuFileRecentFiles, RegistryPath);
            this.mnuFilePhysicalDrive.Enabled = driverInstalled;
            this.UpdateMenu();
        }

        public MainForm(string[] args) : this()
        {
            foreach (string arg in args)
            {
                this.OpenDiskView(arg);
            }

            this.UpdateMenu();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current Culture Information
        /// </summary>
        public static CultureInfo CultureInfo
        {
            get
            {
                return cultureInfo;
            }
        }

        /// <summary>
        /// Gets the Resource Manager used to get localized strings
        /// </summary>
        public static ResourceManager ResourceManager
        {
            get
            {
                return resourceManager;
            }
        }

        #endregion

        #region Public Methods

        public static DialogResult InputBox(string title, string promptText, int maxLength, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = resourceManager.GetString("Button_OK", cultureInfo);
            buttonCancel.Text = resourceManager.GetString("Button_Cancel", cultureInfo);
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            textBox.MaxLength = maxLength;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        /// <summary>
        /// Most Recently Used menu item handler
        /// </summary>
        /// <param name="fileName">Selected filename to open</param>
        public void OpenMRUFile(string fileName)
        {
            this.OpenDiskView(fileName);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Open virtual disk and display it in a DiskViewForm instance
        /// </summary>
        /// <param name="filename">Filename to open</param>
        internal void OpenDiskView(string filename)
        {
            IDiskImage diskimage = null;
            IDiskFormat diskformat = null;

            if (!File.Exists(filename))
            {
                this.mruManager.Remove(filename);
                return;
            }

            switch (Path.GetExtension(filename).ToUpper())
            {
                case ".OS9":
                    diskimage = new RAWImage(filename);
                    break;
                case ".JVC":
                    diskimage = new JVCImage(filename);
                    break;
                case ".VDK":
                    diskimage = new VDKImage(filename);
                    break;
                case ".DMK":
                    diskimage = new DMKImage(filename);
                    break;
                case ".VHD":
                    diskimage = new VHDImage(filename);
                    break;
                case ".DSK":
                    diskimage = new JVCImage(filename);
                    if (!diskimage.IsValidImage)
                    {
                        diskimage.Close();
                        diskimage = new DMKImage(filename);
                    }

                    break;
            }

            if (diskimage == null || !diskimage.IsValidImage)
            {
                diskimage.Close();
                this.mruManager.Remove(filename);
                MessageBox.Show(string.Format(resourceManager.GetString("MainForm_NotValidDiskImage", cultureInfo), filename), resourceManager.GetString("MainForm_NotValidDiskImageCaption", cultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            diskimage.SetPartition(0);

            diskformat = new OS9Format(diskimage);
            if (diskformat == null || !diskformat.IsValidFormat)
            {
                diskformat = new DragonDosFormat(diskimage);
                if (diskformat == null || !diskformat.IsValidFormat)
                {
                    diskformat = new RSDosFormat(diskimage);
                }
            }

            if (!diskformat.IsValidFormat || !diskimage.IsValidImage)
            {
                diskformat = null;
                diskimage.Close();
            }
            else
            {
                DiskViewForm diskviewform = new DiskViewForm(diskformat);
                diskviewform.Text = string.Format("EMUDisk - {0}", diskimage.Filename);
                diskviewform.MdiParent = this;
                diskviewform.Activated += new EventHandler(this.DiskViewForm_Activated);
                diskviewform.Disposed += new EventHandler(this.DiskViewForm_Disposed);
                diskviewform.Show();

                this.mruManager.Add(filename);
            }
        }

        #endregion

        #region Window Events

        /// <summary>
        /// Form's Load event handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Load event arguments</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.UpdateMenu();
        }

        /// <summary>
        /// Form's Drag Enter event handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Drag event arguments</param>
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Form's Drag Drop event handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Drag event arguments</param>
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        this.OpenDiskView(file);
                    }
                }
            }
        }

        #endregion

        #region Menu Events

        #region File menu Events

        /// <summary>
        /// Open File menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CoCo Virtual Disk Files (*.dsk;*.dmk;*.os9;*.vdk;*.vhd)|*.dsk;*.dmk;*.os9;*.vdk;*.vhd";

            DialogResult dr = ofd.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                string strFileName = ofd.FileName;
                this.OpenDiskView(strFileName);
            }

            this.UpdateMenu();
        }

        /// <summary>
        /// Open Drive A: menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuFilePhysicalDriveA_Click(object sender, EventArgs e)
        {
            PhysicalDisk disk = null;
            try
            {
                disk = new PhysicalDisk(0);
            }
            catch (DriveNotFoundException)
            {
                MessageBox.Show(string.Format(resourceManager.GetString("MainForm_DriveNotFoundError", cultureInfo), "A:"), resourceManager.GetString("MainForm_OpenDiskErrorCaption", cultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (disk != null)
                {
                    disk.Close();
                }

                return;
            }
            catch (DiskNotPresentException)
            {
                MessageBox.Show(string.Format(resourceManager.GetString("MainForm_DiskNotPresentError", cultureInfo), "A:"), resourceManager.GetString("MainForm_OpenDiskErrorCaption", cultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (disk != null)
                {
                    disk.Close();
                }

                return;
            }
            catch (DiskFormatException)
            {
                MessageBox.Show(resourceManager.GetString("PhysicalDisk_DiskFormatError", cultureInfo), resourceManager.GetString("MainForm_OpenDiskErrorCaption", cultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (disk != null)
                {
                    disk.Close();
                }

                return;
            }
            catch
            {
                MessageBox.Show(resourceManager.GetString("MainForm_OpenDiskError", cultureInfo), resourceManager.GetString("MainForm_OpenDiskErrorCaption", cultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (disk != null)
                {
                    disk.Close();
                }

                return;
            }
        }

        /// <summary>
        /// Open Drive B: menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuFilePhysicalDriveB_Click(object sender, EventArgs e)
        {
            PhysicalDisk disk = null;
            try
            {
                disk = new PhysicalDisk(1);
            }
            catch (DriveNotFoundException)
            {
                MessageBox.Show(string.Format(resourceManager.GetString("MainForm_DriveNotFoundError", cultureInfo), "B:"), resourceManager.GetString("MainForm_OpenDiskErrorCaption", cultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (disk != null)
                {
                    disk.Close();
                }

                return;
            }
            catch (DiskNotPresentException)
            {
                MessageBox.Show(string.Format(resourceManager.GetString("MainForm_DiskNotPresentError", cultureInfo), "B:"), resourceManager.GetString("MainForm_OpenDiskErrorCaption", cultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (disk != null)
                {
                    disk.Close();
                }

                return;
            }
            catch (DiskFormatException)
            {
                MessageBox.Show(resourceManager.GetString("PhysicalDisk_DiskFormatError", cultureInfo), resourceManager.GetString("MainForm_OpenDiskErrorCaption", cultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (disk != null)
                {
                    disk.Close();
                }

                return;
            }
            catch
            {
                MessageBox.Show(resourceManager.GetString("MainForm_OpenDiskError", cultureInfo), resourceManager.GetString("MainForm_OpenDiskErrorCaption", cultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (disk != null)
                {
                    disk.Close();
                }

                return;
            }
        }

        /// <summary>
        /// Create New File menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuFileNew_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Close menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuFileClose_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                this.ActiveMdiChild.Close();
            }

            this.UpdateMenu();
        }

        /// <summary>
        /// Exit Application menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length != 0)
            {
                foreach (Form form in this.MdiChildren)
                {
                    form.Close();
                    form.Dispose();
                }
            }

            Environment.Exit(0);
        }

        #endregion

        #region Disk menu Events

        private void mnuDiskInfo_Click(object sender, EventArgs e)
        {

        }

        private void mnuDiskFormat_Click(object sender, EventArgs e)
        {

        }

        private void mnuDiskChangeDiskName_Click(object sender, EventArgs e)
        {
            DiskViewForm form = (DiskViewForm)this.ActiveMdiChild;
            string diskname = form.DiskFormat.DiskImage.DiskLabel;

            DialogResult dr = InputBox("Disk Rename", "Disk Name", 32, ref diskname);
            if (dr == DialogResult.OK)
            {
                if (diskname == "")
                    diskname = null;
                form.DiskFormat.DiskImage.DiskLabel = diskname;

                form.Text = form.DiskFormat.DiskImage.Filename;
                if (!string.IsNullOrEmpty(form.DiskFormat.DiskImage.DiskLabel))
                    this.Text += " {" + form.DiskFormat.DiskImage.DiskLabel + "}";
                if (!string.IsNullOrEmpty(form.DiskFormat.VolumeLabel))
                    this.Text += " [" + form.DiskFormat.VolumeLabel + "]";
            }
        }

        private void mnuDiskChangeVolumename_Click(object sender, EventArgs e)
        {
            DiskViewForm form = (DiskViewForm)this.ActiveMdiChild;
            string volumename = form.DiskFormat.VolumeLabel;

            DialogResult dr = InputBox("Volume Rename", "Volume Name", 32, ref volumename);
            if (dr == DialogResult.OK)
            {
                if (volumename == "")
                    volumename = null;
                form.DiskFormat.VolumeLabel = volumename;

                form.Text = form.DiskFormat.DiskImage.Filename;
                if (!string.IsNullOrEmpty(form.DiskFormat.DiskImage.DiskLabel))
                    form.Text += " {" + form.DiskFormat.DiskImage.DiskLabel + "}";
                if (!string.IsNullOrEmpty(form.DiskFormat.VolumeLabel))
                    form.Text += " [" + form.DiskFormat.VolumeLabel + "]";
            }
        }

        private void mnuDiskSectorEditor_Click(object sender, EventArgs e)
        {
            DiskViewForm dv = (DiskViewForm)this.ActiveMdiChild;
            SectorEditor sv = new SectorEditor(dv.DiskFormat.DiskImage);
            sv.ShowDialog();
            if (sv.SectorChanged)
            {

            }
        }

        private void mnuDiskBootstrap_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Window menu Events

        /// <summary>
        /// Close Window menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuWindowCloseWindow_Click(object sender, EventArgs e)
        {
            this.mnuFileClose_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// Cascade Windows menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuWindowCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        /// <summary>
        /// Tile Windows Horizontally menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuWindowTileHorizontal_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        /// <summary>
        /// Tile Windows Vertically menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuWindowTileVertical_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        #endregion

        #region Options menu Events

        /// <summary>
        /// Options menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuOptions_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Help menu Events

        /// <summary>
        /// About menu item handler
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog();
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets new language
        /// </summary>
        /// <param name="language">Specified language to change to</param>
        private void SetLanguage(string language)
        {
            cultureInfo = CultureInfo.CreateSpecificCulture(language);

            foreach (ToolStripItem item in this.menuStrip1.Items)
            {
                if (item is ToolStripMenuItem)
                {
                    this.SetMenuText((ToolStripMenuItem)item);
                }
            }
        }

        /// <summary>
        /// Update menu text when language has been changed
        /// </summary>
        /// <param name="item">Menu item to update</param>
        private void SetMenuText(ToolStripMenuItem item)
        {
            string localizedText;
            try
            {
                localizedText = resourceManager.GetString("MainForm_" + item.Name, cultureInfo);
                if (!string.IsNullOrEmpty(localizedText))
                {
                    item.Text = localizedText;
                }
            }
            catch
            {
            }

            if (item.DropDownItems.Count != 0)
            {
                foreach (ToolStripItem subitem in item.DropDownItems)
                {
                    if (subitem is ToolStripMenuItem)
                    {
                        this.SetMenuText((ToolStripMenuItem)subitem);
                    }
                }
            }
        }

        /// <summary>
        /// Update the menus
        /// </summary>
        private void UpdateMenu()
        {
            if (this.MdiChildren.Length == 0)
            {
                this.mnuFileClose.Enabled = false;
                this.mnuDisk.Enabled = false;
                this.mnuWindow.Enabled = false;
            }
            else
            {
                this.mnuFileClose.Enabled = true;
                this.mnuDisk.Enabled = true;
                this.mnuWindow.Enabled = true;
                this.mnuDiskBootstrap.Enabled = false;
                this.mnuDiskChangeDiskName.Enabled = false;
                this.mnuDiskChangeVolumename.Enabled = false;

                DiskViewForm form = (DiskViewForm)this.ActiveMdiChild;
                if (form != null)
                {
                    if (form.DiskFormat.DiskFormat == DiskFormatTypes.OS9Format)
                    {
                        OS9Format diskformat = (OS9Format)form.DiskFormat;
                        if (diskformat.Lsn0.BootStrap != 0)
                        {
                            this.mnuDiskBootstrap.Enabled = true;
                        }
                    }

                    if (form.DiskFormat.DiskImage is VDKImage)
                        this.mnuDiskChangeDiskName.Enabled = true;

                    if (form.DiskFormat is OS9Format || form.DiskFormat is RSDosFormat)
                        this.mnuDiskChangeVolumename.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Cleanup after disposing an instance of the DiskViewForm
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void DiskViewForm_Disposed(object sender, EventArgs e)
        {
            this.UpdateMenu();
        }

        /// <summary>
        /// Update menu when a DiskViewForm is activated
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void DiskViewForm_Activated(object sender, EventArgs e)
        {
            DiskViewForm form = (DiskViewForm)this.ActiveMdiChild;
            IDiskImage di = form.DiskFormat.DiskImage;
            this.UpdateMenu();
        }

        #endregion

    }
}
