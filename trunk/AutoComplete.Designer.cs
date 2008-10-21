namespace Vinson.UserControls
{
    partial class AutoComplete
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
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelect.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSelect.Location = new System.Drawing.Point(176, 0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(24, 21);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "O";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtValue
            // 
            this.txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValue.Location = new System.Drawing.Point(0, 0);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(176, 21);
            this.txtValue.TabIndex = 1;
            this.txtValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValue_KeyDown);
            this.txtValue.Enter += new System.EventHandler(this.txtValue_Enter);
            // 
            // AutoComplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.btnSelect);
            this.Name = "AutoComplete";
            this.Size = new System.Drawing.Size(200, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtValue;
    }
}
