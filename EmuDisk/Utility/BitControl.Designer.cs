namespace EmuDisk
{
    partial class BitControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblBit = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.pnBitsEditor = new System.Windows.Forms.Panel();
            this.pnBitsHeader = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblBit
            // 
            this.lblBit.AutoSize = true;
            this.lblBit.Location = new System.Drawing.Point(0, 2);
            this.lblBit.Name = "lblBit";
            this.lblBit.Size = new System.Drawing.Size(19, 13);
            this.lblBit.TabIndex = 0;
            this.lblBit.Text = "Bit";
            this.lblBit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(0, 19);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(34, 13);
            this.lblValue.TabIndex = 1;
            this.lblValue.Text = "Value";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnBitsEditor
            // 
            this.pnBitsEditor.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnBitsEditor.Location = new System.Drawing.Point(68, 19);
            this.pnBitsEditor.Margin = new System.Windows.Forms.Padding(0);
            this.pnBitsEditor.Name = "pnBitsEditor";
            this.pnBitsEditor.Padding = new System.Windows.Forms.Padding(1);
            this.pnBitsEditor.Size = new System.Drawing.Size(117, 18);
            this.pnBitsEditor.TabIndex = 2;
            // 
            // pnBitsHeader
            // 
            this.pnBitsHeader.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnBitsHeader.Location = new System.Drawing.Point(68, 1);
            this.pnBitsHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnBitsHeader.Name = "pnBitsHeader";
            this.pnBitsHeader.Padding = new System.Windows.Forms.Padding(1);
            this.pnBitsHeader.Size = new System.Drawing.Size(117, 18);
            this.pnBitsHeader.TabIndex = 3;
            // 
            // BitControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnBitsHeader);
            this.Controls.Add(this.pnBitsEditor);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.lblBit);
            this.Name = "BitControl";
            this.Size = new System.Drawing.Size(215, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBit;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Panel pnBitsEditor;
        private System.Windows.Forms.Panel pnBitsHeader;
    }
}
