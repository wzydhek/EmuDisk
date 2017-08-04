namespace EmuDisk
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilePhysicalDrive = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilePhysicalDriveA = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilePhysicalDriveB = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileRecentFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDisk = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDiskInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDiskFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDiskChangeDiskName = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDiskChangeVolumename = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDiskSectorEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDiskBootstrap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowCloseWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuWindowCascade = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowTileHorizontal = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowTileVertical = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuDisk,
            this.mnuWindow,
            this.mnuOptions,
            this.mnuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileOpen,
            this.mnuFilePhysicalDrive,
            this.mnuFileNew,
            this.mnuFileRecentFiles,
            this.mnuFileClose,
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Name = "mnuFileOpen";
            this.mnuFileOpen.Size = new System.Drawing.Size(147, 22);
            this.mnuFileOpen.Text = "O&pen";
            this.mnuFileOpen.Click += new System.EventHandler(this.mnuFileOpen_Click);
            // 
            // mnuFilePhysicalDrive
            // 
            this.mnuFilePhysicalDrive.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFilePhysicalDriveA,
            this.mnuFilePhysicalDriveB});
            this.mnuFilePhysicalDrive.Name = "mnuFilePhysicalDrive";
            this.mnuFilePhysicalDrive.Size = new System.Drawing.Size(147, 22);
            this.mnuFilePhysicalDrive.Text = "Physical &Drive";
            // 
            // mnuFilePhysicalDriveA
            // 
            this.mnuFilePhysicalDriveA.Name = "mnuFilePhysicalDriveA";
            this.mnuFilePhysicalDriveA.Size = new System.Drawing.Size(115, 22);
            this.mnuFilePhysicalDriveA.Text = "Drive &A:";
            this.mnuFilePhysicalDriveA.Click += new System.EventHandler(this.mnuFilePhysicalDriveA_Click);
            // 
            // mnuFilePhysicalDriveB
            // 
            this.mnuFilePhysicalDriveB.Name = "mnuFilePhysicalDriveB";
            this.mnuFilePhysicalDriveB.Size = new System.Drawing.Size(115, 22);
            this.mnuFilePhysicalDriveB.Text = "Drive &B:";
            this.mnuFilePhysicalDriveB.Click += new System.EventHandler(this.mnuFilePhysicalDriveB_Click);
            // 
            // mnuFileNew
            // 
            this.mnuFileNew.Name = "mnuFileNew";
            this.mnuFileNew.Size = new System.Drawing.Size(147, 22);
            this.mnuFileNew.Text = "&New";
            this.mnuFileNew.Click += new System.EventHandler(this.mnuFileNew_Click);
            // 
            // mnuFileRecentFiles
            // 
            this.mnuFileRecentFiles.Name = "mnuFileRecentFiles";
            this.mnuFileRecentFiles.Size = new System.Drawing.Size(147, 22);
            this.mnuFileRecentFiles.Text = "Recent Files";
            // 
            // mnuFileClose
            // 
            this.mnuFileClose.Name = "mnuFileClose";
            this.mnuFileClose.Size = new System.Drawing.Size(147, 22);
            this.mnuFileClose.Text = "&Close";
            this.mnuFileClose.Click += new System.EventHandler(this.mnuFileClose_Click);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(147, 22);
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuDisk
            // 
            this.mnuDisk.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDiskInfo,
            this.mnuDiskFormat,
            this.mnuDiskChangeDiskName,
            this.mnuDiskChangeVolumename,
            this.mnuDiskSectorEditor,
            this.mnuDiskBootstrap});
            this.mnuDisk.Name = "mnuDisk";
            this.mnuDisk.Size = new System.Drawing.Size(41, 20);
            this.mnuDisk.Text = "&Disk";
            // 
            // mnuDiskInfo
            // 
            this.mnuDiskInfo.Name = "mnuDiskInfo";
            this.mnuDiskInfo.Size = new System.Drawing.Size(193, 22);
            this.mnuDiskInfo.Text = "&Info";
            this.mnuDiskInfo.Click += new System.EventHandler(this.mnuDiskInfo_Click);
            // 
            // mnuDiskFormat
            // 
            this.mnuDiskFormat.Name = "mnuDiskFormat";
            this.mnuDiskFormat.Size = new System.Drawing.Size(193, 22);
            this.mnuDiskFormat.Text = "For&mat";
            this.mnuDiskFormat.Click += new System.EventHandler(this.mnuDiskFormat_Click);
            // 
            // mnuDiskChangeDiskName
            // 
            this.mnuDiskChangeDiskName.Name = "mnuDiskChangeDiskName";
            this.mnuDiskChangeDiskName.Size = new System.Drawing.Size(193, 22);
            this.mnuDiskChangeDiskName.Text = "Change &Disk Name";
            this.mnuDiskChangeDiskName.Click += new System.EventHandler(this.mnuDiskChangeDiskName_Click);
            // 
            // mnuDiskChangeVolumename
            // 
            this.mnuDiskChangeVolumename.Name = "mnuDiskChangeVolumename";
            this.mnuDiskChangeVolumename.Size = new System.Drawing.Size(193, 22);
            this.mnuDiskChangeVolumename.Text = "Change &Volume Name";
            this.mnuDiskChangeVolumename.Click += new System.EventHandler(this.mnuDiskChangeVolumename_Click);
            // 
            // mnuDiskSectorEditor
            // 
            this.mnuDiskSectorEditor.Name = "mnuDiskSectorEditor";
            this.mnuDiskSectorEditor.Size = new System.Drawing.Size(193, 22);
            this.mnuDiskSectorEditor.Text = "&Sector Editor";
            this.mnuDiskSectorEditor.Click += new System.EventHandler(this.mnuDiskSectorEditor_Click);
            // 
            // mnuDiskBootstrap
            // 
            this.mnuDiskBootstrap.Name = "mnuDiskBootstrap";
            this.mnuDiskBootstrap.Size = new System.Drawing.Size(193, 22);
            this.mnuDiskBootstrap.Text = "&Bootstrap";
            this.mnuDiskBootstrap.Click += new System.EventHandler(this.mnuDiskBootstrap_Click);
            // 
            // mnuWindow
            // 
            this.mnuWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWindowCloseWindow,
            this.toolStripSeparator1,
            this.mnuWindowCascade,
            this.mnuWindowTileHorizontal,
            this.mnuWindowTileVertical});
            this.mnuWindow.Name = "mnuWindow";
            this.mnuWindow.Size = new System.Drawing.Size(63, 20);
            this.mnuWindow.Text = "&Window";
            // 
            // mnuWindowCloseWindow
            // 
            this.mnuWindowCloseWindow.Name = "mnuWindowCloseWindow";
            this.mnuWindowCloseWindow.Size = new System.Drawing.Size(151, 22);
            this.mnuWindowCloseWindow.Text = "C&lose Window";
            this.mnuWindowCloseWindow.Click += new System.EventHandler(this.mnuWindowCloseWindow_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(148, 6);
            // 
            // mnuWindowCascade
            // 
            this.mnuWindowCascade.Name = "mnuWindowCascade";
            this.mnuWindowCascade.Size = new System.Drawing.Size(151, 22);
            this.mnuWindowCascade.Text = "&Cascade";
            this.mnuWindowCascade.Click += new System.EventHandler(this.mnuWindowCascade_Click);
            // 
            // mnuWindowTileHorizontal
            // 
            this.mnuWindowTileHorizontal.Name = "mnuWindowTileHorizontal";
            this.mnuWindowTileHorizontal.Size = new System.Drawing.Size(151, 22);
            this.mnuWindowTileHorizontal.Text = "Tile Hori&zontal";
            this.mnuWindowTileHorizontal.Click += new System.EventHandler(this.mnuWindowTileHorizontal_Click);
            // 
            // mnuWindowTileVertical
            // 
            this.mnuWindowTileVertical.Name = "mnuWindowTileVertical";
            this.mnuWindowTileVertical.Size = new System.Drawing.Size(151, 22);
            this.mnuWindowTileVertical.Text = "Tile &Vertical";
            this.mnuWindowTileVertical.Click += new System.EventHandler(this.mnuWindowTileVertical_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new System.Drawing.Size(61, 20);
            this.mnuOptions.Text = "&Options";
            this.mnuOptions.Click += new System.EventHandler(this.mnuOptions_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            this.mnuHelpAbout.Size = new System.Drawing.Size(107, 22);
            this.mnuHelpAbout.Text = "&About";
            this.mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "EmuDisk";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuDisk;
        private System.Windows.Forms.ToolStripMenuItem mnuWindow;
        private System.Windows.Forms.ToolStripMenuItem mnuOptions;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuFilePhysicalDrive;
        private System.Windows.Forms.ToolStripMenuItem mnuFilePhysicalDriveA;
        private System.Windows.Forms.ToolStripMenuItem mnuFilePhysicalDriveB;
        private System.Windows.Forms.ToolStripMenuItem mnuFileNew;
        private System.Windows.Forms.ToolStripMenuItem mnuFileRecentFiles;
        private System.Windows.Forms.ToolStripMenuItem mnuFileClose;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuDiskInfo;
        private System.Windows.Forms.ToolStripMenuItem mnuDiskFormat;
        private System.Windows.Forms.ToolStripMenuItem mnuDiskChangeDiskName;
        private System.Windows.Forms.ToolStripMenuItem mnuDiskSectorEditor;
        private System.Windows.Forms.ToolStripMenuItem mnuDiskBootstrap;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowCloseWindow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowCascade;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowTileHorizontal;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowTileVertical;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem mnuDiskChangeVolumename;
    }
}

