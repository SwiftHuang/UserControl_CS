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
            this.btnShow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mTxtValue
            // 
            this.mTxtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mTxtValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mTxtValue.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.mTxtValue.Location = new System.Drawing.Point(20, 3);
            this.mTxtValue.MinimumSize = new System.Drawing.Size(0, 15);
            this.mTxtValue.Name = "mTxtValue";
            this.mTxtValue.Size = new System.Drawing.Size(92, 15);
            this.mTxtValue.TabIndex = 0;
            // 
            // chkBox
            // 
            this.chkBox.AutoSize = true;
            this.chkBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkBox.Location = new System.Drawing.Point(0, 0);
            this.chkBox.Name = "chkBox";
            this.chkBox.Padding = new System.Windows.Forms.Padding(2, 1, 0, 0);
            this.chkBox.Size = new System.Drawing.Size(17, 19);
            this.chkBox.TabIndex = 1;
            this.chkBox.UseVisualStyleBackColor = true;
            // 
            // btnShow
            // 
            this.btnShow.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnShow.Location = new System.Drawing.Point(114, 0);
            this.btnShow.Margin = new System.Windows.Forms.Padding(1);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(19, 19);
            this.btnShow.TabIndex = 2;
            this.btnShow.Text = "button1";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // MaskedDateTimePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.mTxtValue);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.chkBox);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MinimumSize = new System.Drawing.Size(50, 21);
            this.Name = "MaskedDateTimePicker";
            this.Size = new System.Drawing.Size(133, 19);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox mTxtValue;
        private System.Windows.Forms.CheckBox chkBox;
        private System.Windows.Forms.Button btnShow;
    }
}
