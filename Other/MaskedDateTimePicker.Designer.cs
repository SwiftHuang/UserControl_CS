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
            this.btnShow = new System.Windows.Forms.Button();
            this.chkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // mTxtValue
            // 
            this.mTxtValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTxtValue.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.mTxtValue.Location = new System.Drawing.Point(0, 0);
            this.mTxtValue.Name = "mTxtValue";
            this.mTxtValue.Size = new System.Drawing.Size(121, 21);
            this.mTxtValue.TabIndex = 0;
            this.mTxtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(100, 0);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(20, 20);
            this.btnShow.TabIndex = 1;
            this.btnShow.TabStop = false;
            this.btnShow.Text = "button1";
            this.btnShow.UseVisualStyleBackColor = true;
            // 
            // chkBox
            // 
            this.chkBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBox.AutoSize = true;
            this.chkBox.Location = new System.Drawing.Point(3, 3);
            this.chkBox.Name = "chkBox";
            this.chkBox.Size = new System.Drawing.Size(15, 14);
            this.chkBox.TabIndex = 2;
            this.chkBox.UseVisualStyleBackColor = true;
            // 
            // MaskedDateTimePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.chkBox);
            this.Controls.Add(this.mTxtValue);
            this.Name = "MaskedDateTimePicker";
            this.Size = new System.Drawing.Size(121, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox mTxtValue;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.CheckBox chkBox;
    }
}
