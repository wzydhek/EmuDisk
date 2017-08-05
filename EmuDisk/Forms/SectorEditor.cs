using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace EmuDisk
{
    public partial class SectorEditor : Form
    {
        #region Private Properties

        private IDiskImage disk;
        private byte[] data;
        public bool SectorChanged;

        #endregion

        #region Constructors

        public SectorEditor(IDiskImage Disk)
        {
            InitializeComponent();
            this.toolStrip.Renderer.RenderToolStripBorder += new ToolStripRenderEventHandler(Renderer_RenderToolStripBorder);

            disk = Disk;
            data = Disk.ReadSector((int)numTrack.Value, (int)numSide.Value, (int)numSector.Value);
            FixedLengthByteProvider dbp = new FixedLengthByteProvider(data);
            dbp.Changed += new EventHandler(dbp_Changed);
            hexBox1.ByteProvider = dbp;
            //hexBox1.ReadOnly = true;
            numTrack.Maximum = disk.PhysicalTracks - 1;
            numSide.Maximum = disk.PhysicalHeads - 1;
            numSector.Maximum = disk.PhysicalSectors;
            Init();

            hexBox1.MouseWheel += hexBox1_MouseWheel;
        }

        #endregion

        #region Form Events

        private void SectorEditor_Load(object sender, EventArgs e)
        {
            hexBox1.Focus();
        }

        private void SectorEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mnuFileSaveChanges.Enabled)
            {
                DialogResult dr = MessageBox.Show("Data has changed, save changes?", "Data Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (dr)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        data = ((FixedLengthByteProvider)hexBox1.ByteProvider).ByteArray;
                        disk.WriteSector((int)numTrack.Value, (int)numSide.Value, (int)numSector.Value, data);
                        break;
                }
            }
        }

        private void mnuFileSaveChanges_Click(object sender, EventArgs e)
        {
            data = ((FixedLengthByteProvider)hexBox1.ByteProvider).ByteArray;
            disk.WriteSector((int)numTrack.Value, (int)numSide.Value, (int)numSector.Value, data);
            SectorChanged = true;
            mnuFileSaveChanges.Enabled = false;
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuEditCut_Click(object sender, EventArgs e)
        {

        }

        private void mnuEditCopy_Click(object sender, EventArgs e)
        {
            this.hexBox1.Copy();
        }

        private void mnuEditCopyHex_Click(object sender, EventArgs e)
        {
            this.hexBox1.CopyHex();
        }

        private void mnuEditSelectAll_Click(object sender, EventArgs e)
        {
            this.hexBox1.SelectAll();
        }

        void encodingMenuItem_Clicked(object sender, EventArgs e)
        {
            var converter = ((ToolStripMenuItem)sender).Tag;
            encodingToolStripComboBox.SelectedItem = converter;
        }

        private void mnuViewBits_Click(object sender, EventArgs e)
        {
            UpdateBitControlVisibility();
        }

        private void encodingToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            hexBox1.ByteCharConverter = encodingToolStripComboBox.SelectedItem as IByteCharConverter;

            foreach (ToolStripMenuItem encodingMenuItem in mnuViewEncoding.DropDownItems)
                encodingMenuItem.Checked = (encodingMenuItem.Tag == hexBox1.ByteCharConverter);
        }

        private void dbp_Changed(object sender, EventArgs e)
        {
            mnuFileSaveChanges.Enabled = true;
        }

        private void hexBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            // This may seem backwards, but I think it makes more sense logically.
            // Scroll the wheel up to navigate to the previous sector.
            // Scroll the wheel down to navigate to the next sector.
            if (e.Delta > 0)
            {
                PreviousSector();
            }
            else if (e.Delta < 0)
            {
                NextSector();
            }
        }

        private void hexBox1_Copied(object sender, EventArgs e)
        {
            ManageAbilityForCopyAndPaste();
        }

        private void hexBox1_CopiedHex(object sender, EventArgs e)
        {
            ManageAbilityForCopyAndPaste();
        }

        private void hexBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Alt && !e.Control && !e.Shift && e.KeyCode == Keys.PageDown)
            {
                NextSector();
                e.Handled = true;
            }
            else if (!e.Alt && !e.Control && !e.Shift && e.KeyCode == Keys.PageUp)
            {
                PreviousSector();
                e.Handled = true;
            }
            else if (!e.Alt && e.Control && !e.Shift && e.KeyCode == Keys.Home)
            {
                numSector.Value = numSector.Minimum;
                numSide.Value = numSide.Minimum;
                numTrack.Value = numTrack.Minimum;
            }
            else if (!e.Alt && e.Control && !e.Shift && e.KeyCode == Keys.End)
            {
                numSector.Value = numSector.Maximum;
                numSide.Value = numSide.Maximum;
                numTrack.Value = numTrack.Maximum;
            }
        }

        private void hexBox1_SelectionLengthChanged(object sender, EventArgs e)
        {
            ManageAbilityForCopyAndPaste();
        }

        private void hexBox1_SelectionStartChanged(object sender, EventArgs e)
        {
            ManageAbilityForCopyAndPaste();
        }

        private void Position_Changed(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = string.Format(CultureInfo.CurrentCulture, "Ln {0}    Col {1}",
                hexBox1.CurrentLine, hexBox1.CurrentPositionInLine);

            string bitPresentation = string.Empty;

            byte? currentByte = hexBox1.ByteProvider != null && hexBox1.ByteProvider.Length > hexBox1.SelectionStart
                ? hexBox1.ByteProvider.ReadByte(hexBox1.SelectionStart)
                : (byte?)null;

            BitInfo bitInfo = currentByte != null ? new BitInfo((byte)currentByte, hexBox1.SelectionStart) : null;

            if (bitInfo != null)
            {
                bitPresentation = string.Format(CultureInfo.CurrentCulture, "Bits of Byte {0}: {1}"
                    , hexBox1.SelectionStart
                    , bitInfo.ToString()
                    );
            }

            this.bitToolStripStatusLabel.Text = bitPresentation;

            this.bitControl1.BitInfo = bitInfo;
        }

        private void bitControl1_BitChanged(object sender, EventArgs e)
        {
            hexBox1.ByteProvider.WriteByte(bitControl1.BitInfo.Position, bitControl1.BitInfo.Value);
            hexBox1.Invalidate();
        }

        private void numTrack_ValueChanged(object sender, EventArgs e)
        {
            ChangeSector();
        }

        private void numSide_ValueChanged(object sender, EventArgs e)
        {
            ChangeSector();
        }

        private void numSector_ValueChanged(object sender, EventArgs e)
        {
            ChangeSector();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Removes the border on the right of the tool strip
        /// </summary>
        /// <param name="sender">the renderer</param>
        /// <param name="e">the event args</param>
        void Renderer_RenderToolStripBorder(object sender, ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip.GetType() != typeof(ToolStrip))
                return;

            e.Graphics.DrawLine(new Pen(new SolidBrush(SystemColors.Control)), new Point(toolStrip.Width - 1, 0),
                new Point(toolStrip.Width - 1, toolStrip.Height));
        }

        void Init()
        {
            var defConverter = new DefaultByteCharConverter();
            ToolStripMenuItem miDefault = new ToolStripMenuItem();
            miDefault.Text = defConverter.ToString();
            miDefault.Tag = defConverter;
            miDefault.Click += new EventHandler(encodingMenuItem_Clicked);

            var ebcdicConverter = new EbcdicByteCharProvider();
            ToolStripMenuItem miEbcdic = new ToolStripMenuItem();
            miEbcdic.Text = ebcdicConverter.ToString();
            miEbcdic.Tag = ebcdicConverter;
            miEbcdic.Click += new EventHandler(encodingMenuItem_Clicked);

            encodingToolStripComboBox.Items.Add(defConverter);
            encodingToolStripComboBox.Items.Add(ebcdicConverter);

            mnuViewEncoding.DropDownItems.Add(miDefault);
            mnuViewEncoding.DropDownItems.Add(miEbcdic);
            encodingToolStripComboBox.SelectedIndex = 0;
        }

        private void NextSector()
        {
            if (numSector.Value < numSector.Maximum)
            {
                numSector.Value++;
            }
            else
            {
                numSector.Value = numSector.Minimum;

                if (numSide.Value < numSide.Maximum)
                {
                    numSide.Value++;
                }
                else
                {
                    numSide.Value = numSide.Minimum;

                    if (numTrack.Value < numTrack.Maximum)
                    {
                        numTrack.Value++;
                    }
                    else
                    {
                        numTrack.Value = numTrack.Minimum;
                    }
                }
            }
        }

        private void PreviousSector()
        {
            if (numSector.Value > numSector.Minimum)
            {
                numSector.Value--;
            }
            else
            {
                numSector.Value = numSector.Maximum;

                if (numSide.Value > numSide.Minimum)
                {
                    numSide.Value--;
                }
                else
                {
                    numSide.Value = numSide.Maximum;

                    if (numTrack.Value > numTrack.Minimum)
                    {
                        numTrack.Value--;
                    }
                    else
                    {
                        numTrack.Value = numTrack.Maximum;
                    }
                }
            }
        }

        void ManageAbilityForCopyAndPaste()
        {
            mnuEditCopyHex.Enabled =
                copyToolStripSplitButton.Enabled = mnuEditCopy.Enabled = hexBox1.CanCopy();

            /* cutToolStripButton.Enabled = */
            //cutToolStripMenuItem.Enabled = hexBox1.CanCut();
            /* pasteToolStripSplitButton.Enabled = */
            //pasteToolStripMenuItem.Enabled = hexBox1.CanPaste();
            //pasteHexToolStripMenuItem.Enabled = /* pasteHexToolStripMenuItem1.Enabled = */ hexBox1.CanPasteHex();
        }

        void ChangeSector()
        {
            data = disk.ReadSector((int)numTrack.Value, (int)numSide.Value, (int)numSector.Value);
            DynamicByteProvider dbp = new DynamicByteProvider(data);
            dbp.Changed += new EventHandler(dbp_Changed);
            hexBox1.ByteProvider = dbp;
        }

        void UpdateBitControlVisibility()
        {
            //if (this.bitControl1.Visible == bitsToolStripMenuItem.Checked)
            //{
            //    return;
            //}
            if (mnuViewBits.Checked)
            {
                //hexBox.Height -= bitControl1.Height;
                bitControl1.Visible = true;
            }
            else
            {
                //hexBox.Height += bitControl1.Height;
                bitControl1.Visible = false;
            }
        }

        #endregion
    }
}
