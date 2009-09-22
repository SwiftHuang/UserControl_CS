namespace hwj.UserControls.Suggest
{
    partial class SuggestBox
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
            this.txtValue = new hwj.UserControls.CommonControls.xTextBox();
            this.btnSelect = new hwj.UserControls.CommonControls.xButton();
            this.SuspendLayout();
            // 
            // txtValue
            // 
            this.txtValue.BackColor = System.Drawing.SystemColors.Window;
            this.txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValue.Format = null;
            this.txtValue.Location = new System.Drawing.Point(0, 0);
            this.txtValue.Name = "txtValue";
            this.txtValue.OldBackColor = System.Drawing.SystemColors.Window;
            this.txtValue.Size = new System.Drawing.Size(97, 21);
            this.txtValue.TabIndex = 0;
            this.txtValue.TextIsChanged = false;
            this.txtValue.DoubleClick += new System.EventHandler(this.txtValue_DoubleClick);
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            this.txtValue.Click += new System.EventHandler(this.txtValue_Click);
            this.txtValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValue_KeyDown);
            this.txtValue.Enter += new System.EventHandler(this.txtValue_Enter);
            this.txtValue.Validating += new System.ComponentModel.CancelEventHandler(this.txtValue_Validating);
            // 
            // btnSelect
            // 
            this.btnSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelect.CursorFromClick = System.Windows.Forms.Cursors.WaitCursor;
            this.btnSelect.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSelect.Image = global::hwj.UserControls.Properties.Resources.page_search;
            this.btnSelect.Location = new System.Drawing.Point(97, 0);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelect.MaximumSize = new System.Drawing.Size(23, 21);
            this.btnSelect.MinimumSize = new System.Drawing.Size(23, 21);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(23, 21);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.TabStop = false;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // SuggestBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.btnSelect);
            this.MinimumSize = new System.Drawing.Size(0, 21);
            this.Name = "SuggestBox";
            this.Size = new System.Drawing.Size(120, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CommonControls.xButton btnSelect;
        private CommonControls.xTextBox txtValue;
    }
}
