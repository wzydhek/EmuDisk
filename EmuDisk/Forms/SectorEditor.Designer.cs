namespace EmuDisk
{
    partial class SectorEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SectorEditor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSaveChanges = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditCopyHex = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewEncoding = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewBits = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.hexBox1 = new EmuDisk.HexBox();
            this.bitControl1 = new EmuDisk.BitControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numSector = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numSide = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numTrack = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.fileSizeToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.bitToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.copyToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyHexToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.encodingToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrack)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuView,
            this.mnuTools});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(662, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileSaveChanges,
            this.toolStripSeparator1,
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuFileSaveChanges
            // 
            this.mnuFileSaveChanges.Enabled = false;
            this.mnuFileSaveChanges.Name = "mnuFileSaveChanges";
            this.mnuFileSaveChanges.Size = new System.Drawing.Size(147, 22);
            this.mnuFileSaveChanges.Text = "&Save Changes";
            this.mnuFileSaveChanges.Click += new System.EventHandler(this.mnuFileSaveChanges_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(144, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(147, 22);
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditCut,
            this.mnuEditCopy,
            this.toolStripSeparator2,
            this.mnuEditCopyHex,
            this.toolStripSeparator3,
            this.mnuEditSelectAll});
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(39, 20);
            this.mnuEdit.Text = "&Edit";
            // 
            // mnuEditCut
            // 
            this.mnuEditCut.Name = "mnuEditCut";
            this.mnuEditCut.ShortcutKeyDisplayString = "";
            this.mnuEditCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.mnuEditCut.Size = new System.Drawing.Size(199, 22);
            this.mnuEditCut.Text = "Cu&t";
            this.mnuEditCut.Click += new System.EventHandler(this.mnuEditCut_Click);
            // 
            // mnuEditCopy
            // 
            this.mnuEditCopy.Name = "mnuEditCopy";
            this.mnuEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuEditCopy.Size = new System.Drawing.Size(199, 22);
            this.mnuEditCopy.Text = "&Copy";
            this.mnuEditCopy.Click += new System.EventHandler(this.mnuEditCopy_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(196, 6);
            // 
            // mnuEditCopyHex
            // 
            this.mnuEditCopyHex.Name = "mnuEditCopyHex";
            this.mnuEditCopyHex.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.mnuEditCopyHex.Size = new System.Drawing.Size(199, 22);
            this.mnuEditCopyHex.Text = "Copy &Hex";
            this.mnuEditCopyHex.Click += new System.EventHandler(this.mnuEditCopyHex_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(196, 6);
            // 
            // mnuEditSelectAll
            // 
            this.mnuEditSelectAll.Name = "mnuEditSelectAll";
            this.mnuEditSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mnuEditSelectAll.Size = new System.Drawing.Size(199, 22);
            this.mnuEditSelectAll.Text = "Select &All";
            this.mnuEditSelectAll.Click += new System.EventHandler(this.mnuEditSelectAll_Click);
            // 
            // mnuView
            // 
            this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewEncoding,
            this.mnuViewBits});
            this.mnuView.Name = "mnuView";
            this.mnuView.Size = new System.Drawing.Size(44, 20);
            this.mnuView.Text = "&View";
            // 
            // mnuViewEncoding
            // 
            this.mnuViewEncoding.Name = "mnuViewEncoding";
            this.mnuViewEncoding.Size = new System.Drawing.Size(124, 22);
            this.mnuViewEncoding.Text = "&Encoding";
            // 
            // mnuViewBits
            // 
            this.mnuViewBits.CheckOnClick = true;
            this.mnuViewBits.Name = "mnuViewBits";
            this.mnuViewBits.Size = new System.Drawing.Size(124, 22);
            this.mnuViewBits.Text = "&Bits";
            this.mnuViewBits.Click += new System.EventHandler(this.mnuViewBits_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuToolsOptions});
            this.mnuTools.Name = "mnuTools";
            this.mnuTools.Size = new System.Drawing.Size(47, 20);
            this.mnuTools.Text = "&Tools";
            // 
            // mnuToolsOptions
            // 
            this.mnuToolsOptions.Name = "mnuToolsOptions";
            this.mnuToolsOptions.Size = new System.Drawing.Size(125, 22);
            this.mnuToolsOptions.Text = "&Options...";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.hexBox1);
            this.panel1.Controls.Add(this.bitControl1);
            this.panel1.Location = new System.Drawing.Point(10, 49);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 324);
            this.panel1.TabIndex = 3;
            // 
            // hexBox1
            // 
            this.hexBox1.ColumnInfoVisible = true;
            this.hexBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox1.InfoForeColor = System.Drawing.Color.Gray;
            this.hexBox1.LineInfoVisible = true;
            this.hexBox1.Location = new System.Drawing.Point(0, 0);
            this.hexBox1.Name = "hexBox1";
            this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox1.Size = new System.Drawing.Size(640, 288);
            this.hexBox1.StringViewVisible = true;
            this.hexBox1.TabIndex = 0;
            this.hexBox1.UseFixedBytesPerLine = true;
            this.hexBox1.VScrollBarVisible = true;
            this.hexBox1.SelectionStartChanged += new System.EventHandler(this.hexBox1_SelectionStartChanged);
            this.hexBox1.SelectionLengthChanged += new System.EventHandler(this.hexBox1_SelectionLengthChanged);
            this.hexBox1.CurrentLineChanged += new System.EventHandler(this.Position_Changed);
            this.hexBox1.CurrentPositionInLineChanged += new System.EventHandler(this.Position_Changed);
            this.hexBox1.Copied += new System.EventHandler(this.hexBox1_Copied);
            this.hexBox1.CopiedHex += new System.EventHandler(this.hexBox1_CopiedHex);
            this.hexBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hexBox1_KeyDown);
            // 
            // bitControl1
            // 
            this.bitControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bitControl1.Location = new System.Drawing.Point(0, 288);
            this.bitControl1.Name = "bitControl1";
            this.bitControl1.Size = new System.Drawing.Size(640, 36);
            this.bitControl1.TabIndex = 0;
            this.bitControl1.Visible = false;
            this.bitControl1.BitChanged += new System.EventHandler(this.bitControl1_BitChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.numSector);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.numSide);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.numTrack);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(10, 383);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(640, 23);
            this.panel2.TabIndex = 4;
            // 
            // numSector
            // 
            this.numSector.Location = new System.Drawing.Point(315, 2);
            this.numSector.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numSector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSector.Name = "numSector";
            this.numSector.Size = new System.Drawing.Size(63, 20);
            this.numSector.TabIndex = 11;
            this.numSector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSector.ValueChanged += new System.EventHandler(this.numSector_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(271, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Sector:";
            // 
            // numSide
            // 
            this.numSide.Location = new System.Drawing.Point(182, 2);
            this.numSide.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numSide.Name = "numSide";
            this.numSide.Size = new System.Drawing.Size(52, 20);
            this.numSide.TabIndex = 9;
            this.numSide.ValueChanged += new System.EventHandler(this.numSide_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Side:";
            // 
            // numTrack
            // 
            this.numTrack.Location = new System.Drawing.Point(63, 2);
            this.numTrack.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numTrack.Name = "numTrack";
            this.numTrack.Size = new System.Drawing.Size(53, 20);
            this.numTrack.TabIndex = 7;
            this.numTrack.ValueChanged += new System.EventHandler(this.numTrack_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Track:";
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.fileSizeToolStripStatusLabel,
            this.bitToolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 413);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip.Size = new System.Drawing.Size(662, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 5;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.toolStripStatusLabel.Size = new System.Drawing.Size(7, 17);
            // 
            // fileSizeToolStripStatusLabel
            // 
            this.fileSizeToolStripStatusLabel.Name = "fileSizeToolStripStatusLabel";
            this.fileSizeToolStripStatusLabel.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fileSizeToolStripStatusLabel.Size = new System.Drawing.Size(8, 17);
            // 
            // bitToolStripStatusLabel
            // 
            this.bitToolStripStatusLabel.Name = "bitToolStripStatusLabel";
            this.bitToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStrip
            // 
            this.toolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip.AutoSize = false;
            this.toolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripSplitButton,
            this.encodingToolStripComboBox});
            this.toolStrip.Location = new System.Drawing.Point(10, 25);
            this.toolStrip.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(640, 22);
            this.toolStrip.TabIndex = 6;
            this.toolStrip.Text = "toolStrip1";
            // 
            // copyToolStripSplitButton
            // 
            this.copyToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem1,
            this.copyHexToolStripMenuItem1});
            this.copyToolStripSplitButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.copyToolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripSplitButton.Image")));
            this.copyToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripSplitButton.Name = "copyToolStripSplitButton";
            this.copyToolStripSplitButton.Size = new System.Drawing.Size(32, 19);
            this.copyToolStripSplitButton.Text = "Copy";
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem1.Text = "Copy";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.mnuEditCopy_Click);
            // 
            // copyHexToolStripMenuItem1
            // 
            this.copyHexToolStripMenuItem1.Name = "copyHexToolStripMenuItem1";
            this.copyHexToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.copyHexToolStripMenuItem1.Text = "Copy Hex";
            this.copyHexToolStripMenuItem1.Click += new System.EventHandler(this.mnuEditCopyHex_Click);
            // 
            // encodingToolStripComboBox
            // 
            this.encodingToolStripComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.encodingToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encodingToolStripComboBox.DropDownWidth = 180;
            this.encodingToolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encodingToolStripComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.encodingToolStripComboBox.Name = "encodingToolStripComboBox";
            this.encodingToolStripComboBox.Size = new System.Drawing.Size(180, 22);
            this.encodingToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.encodingToolStripComboBox_SelectedIndexChanged);
            // 
            // SectorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 435);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SectorEditor";
            this.ShowIcon = false;
            this.Text = "Sector Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SectorEditor_FormClosing);
            this.Load += new System.EventHandler(this.SectorEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrack)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileSaveChanges;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuEditCut;
        private System.Windows.Forms.ToolStripMenuItem mnuEditCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuEditCopyHex;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuEditSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private System.Windows.Forms.ToolStripMenuItem mnuViewEncoding;
        private System.Windows.Forms.ToolStripMenuItem mnuViewBits;
        private System.Windows.Forms.ToolStripMenuItem mnuTools;
        private System.Windows.Forms.ToolStripMenuItem mnuToolsOptions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown numSector;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numSide;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numTrack;
        private System.Windows.Forms.Label label1;
        private BitControl bitControl1;
        private HexBox hexBox1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel fileSizeToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel bitToolStripStatusLabel;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripSplitButton copyToolStripSplitButton;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyHexToolStripMenuItem1;
        private System.Windows.Forms.ToolStripComboBox encodingToolStripComboBox;
    }
}