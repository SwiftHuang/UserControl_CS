namespace hwj.UserControls.Other
{
    partial class MaskedDateTimePicker
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
            this.mTxtValue = new System.Windows.Forms.MaskedTextBox();
            this.chkBox = new System.Windows.Forms.CheckBox();
            this.pBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mTxtValue
            // 
            this.mTxtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mTxtValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mTxtValue.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.mTxtValue.Location = new System.Drawing.Point(3, 3);
            this.mTxtValue.Name = "mTxtValue";
            this.mTxtValue.Size = new System.Drawing.Size(66, 14);
            this.mTxtValue.TabIndex = 0;
            // 
            // chkBox
            // 
            this.chkBox.AutoSize = true;
            this.chkBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkBox.Location = new System.Drawing.Point(0, 0);
            this.chkBox.Name = "chkBox";
            this.chkBox.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.chkBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkBox.Size = new System.Drawing.Size(16, 19);
            this.chkBox.TabIndex = 1;
            this.chkBox.TabStop = false;
            this.chkBox.UseVisualStyleBackColor = true;
            // 
            // pBox
            // 
            this.pBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.pBox.Image = global::hwj.UserControls.Properties.Resources.calendar;
            this.pBox.Location = new System.Drawing.Point(71, 0);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(19, 19);
            this.pBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBox.TabIndex = 2;
            this.pBox.TabStop = false;
            this.pBox.Click += new System.EventHandler(this.pBox_Click);
            // 
            // MaskedDateTimePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pBox);
            this.Controls.Add(this.chkBox);
            this.Controls.Add(this.mTxtValue);
            this.MinimumSize = new System.Drawing.Size(50, 21);
            this.Name = "MaskedDateTimePicker";
            this.Size = new System.Drawing.Size(90, 19);
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox mTxtValue;
        private System.Windows.Forms.CheckBox chkBox;
        private System.Windows.Forms.PictureBox pBox;
    }
}
