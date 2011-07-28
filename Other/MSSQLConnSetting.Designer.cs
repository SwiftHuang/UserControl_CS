namespace hwj.UserControls.Other
{
    partial class MSSQLConnSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MSSQLConnSetting));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnTestDB = new hwj.UserControls.CommonControls.xButton();
            this.cboDatabase = new hwj.UserControls.CommonControls.xComboBox();
            this.txtDataSource = new hwj.UserControls.CommonControls.xTextBox();
            this.cboServerType = new hwj.UserControls.CommonControls.xComboBox();
            this.cboVerificationType = new hwj.UserControls.CommonControls.xComboBox();
            this.txtUser = new hwj.UserControls.CommonControls.xTextBox();
            this.txtPassword = new hwj.UserControls.CommonControls.xTextBox();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AccessibleDescription = null;
            this.tableLayoutPanel2.AccessibleName = null;
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.BackgroundImage = null;
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.btnTestDB, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.cboDatabase, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtDataSource, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cboServerType, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cboVerificationType, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtUser, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtPassword, 1, 4);
            this.tableLayoutPanel2.Font = null;
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Font = null;
            this.label5.Name = "label5";
            // 
            // label6
            // 
            this.label6.AccessibleDescription = null;
            this.label6.AccessibleName = null;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Font = null;
            this.label6.Name = "label6";
            // 
            // label7
            // 
            this.label7.AccessibleDescription = null;
            this.label7.AccessibleName = null;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Font = null;
            this.label7.Name = "label7";
            // 
            // label8
            // 
            this.label8.AccessibleDescription = null;
            this.label8.AccessibleName = null;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Font = null;
            this.label8.Name = "label8";
            // 
            // label9
            // 
            this.label9.AccessibleDescription = null;
            this.label9.AccessibleName = null;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Font = null;
            this.label9.Name = "label9";
            // 
            // btnTestDB
            // 
            this.btnTestDB.AccessibleDescription = null;
            this.btnTestDB.AccessibleName = null;
            resources.ApplyResources(this.btnTestDB, "btnTestDB");
            this.btnTestDB.BackgroundImage = null;
            this.btnTestDB.CursorFromClick = System.Windows.Forms.Cursors.WaitCursor;
            this.btnTestDB.Font = null;
            this.btnTestDB.Name = "btnTestDB";
            this.btnTestDB.UseVisualStyleBackColor = true;
            this.btnTestDB.Click += new System.EventHandler(this.btnRefreshDB_Click);
            // 
            // cboDatabase
            // 
            this.cboDatabase.AccessibleDescription = null;
            this.cboDatabase.AccessibleName = null;
            resources.ApplyResources(this.cboDatabase, "cboDatabase");
            this.cboDatabase.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDatabase.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDatabase.BackColor = System.Drawing.SystemColors.Window;
            this.cboDatabase.BackgroundImage = null;
            this.cboDatabase.Font = null;
            this.cboDatabase.FormattingEnabled = true;
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.OldBackColor = System.Drawing.SystemColors.Window;
            this.cboDatabase.DropDown += new System.EventHandler(this.cboDatabase_DropDown);
            this.cboDatabase.Click += new System.EventHandler(this.cboDatabase_Click);
            // 
            // txtDataSource
            // 
            this.txtDataSource.AccessibleDescription = null;
            this.txtDataSource.AccessibleName = null;
            resources.ApplyResources(this.txtDataSource, "txtDataSource");
            this.txtDataSource.BackColor = System.Drawing.SystemColors.Window;
            this.txtDataSource.BackgroundImage = null;
            this.tableLayoutPanel2.SetColumnSpan(this.txtDataSource, 2);
            this.txtDataSource.Font = null;
            this.txtDataSource.Format = null;
            this.txtDataSource.IsRequired = true;
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.OldBackColor = System.Drawing.SystemColors.Window;
            this.txtDataSource.RequiredHandle = null;
            this.txtDataSource.SetValueToControl = null;
            this.txtDataSource.TextIsChanged = false;
            // 
            // cboServerType
            // 
            this.cboServerType.AccessibleDescription = null;
            this.cboServerType.AccessibleName = null;
            resources.ApplyResources(this.cboServerType, "cboServerType");
            this.cboServerType.BackColor = System.Drawing.SystemColors.Window;
            this.cboServerType.BackgroundImage = null;
            this.tableLayoutPanel2.SetColumnSpan(this.cboServerType, 2);
            this.cboServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboServerType.Font = null;
            this.cboServerType.FormattingEnabled = true;
            this.cboServerType.IsRequired = true;
            this.cboServerType.Items.AddRange(new object[] {
            resources.GetString("cboServerType.Items"),
            resources.GetString("cboServerType.Items1")});
            this.cboServerType.Name = "cboServerType";
            this.cboServerType.OldBackColor = System.Drawing.SystemColors.Window;
            // 
            // cboVerificationType
            // 
            this.cboVerificationType.AccessibleDescription = null;
            this.cboVerificationType.AccessibleName = null;
            resources.ApplyResources(this.cboVerificationType, "cboVerificationType");
            this.cboVerificationType.BackColor = System.Drawing.SystemColors.Window;
            this.cboVerificationType.BackgroundImage = null;
            this.tableLayoutPanel2.SetColumnSpan(this.cboVerificationType, 2);
            this.cboVerificationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVerificationType.Font = null;
            this.cboVerificationType.FormattingEnabled = true;
            this.cboVerificationType.IsRequired = true;
            this.cboVerificationType.Items.AddRange(new object[] {
            resources.GetString("cboVerificationType.Items"),
            resources.GetString("cboVerificationType.Items1")});
            this.cboVerificationType.Name = "cboVerificationType";
            this.cboVerificationType.OldBackColor = System.Drawing.SystemColors.Window;
            this.cboVerificationType.SelectedValueChanged += new System.EventHandler(this.cboVerificationType_SelectedValueChanged);
            // 
            // txtUser
            // 
            this.txtUser.AccessibleDescription = null;
            this.txtUser.AccessibleName = null;
            resources.ApplyResources(this.txtUser, "txtUser");
            this.txtUser.BackColor = System.Drawing.SystemColors.Window;
            this.txtUser.BackgroundImage = null;
            this.tableLayoutPanel2.SetColumnSpan(this.txtUser, 2);
            this.txtUser.Font = null;
            this.txtUser.Format = null;
            this.txtUser.IsRequired = true;
            this.txtUser.Name = "txtUser";
            this.txtUser.OldBackColor = System.Drawing.SystemColors.Window;
            this.txtUser.RequiredHandle = null;
            this.txtUser.SetValueToControl = null;
            this.txtUser.TextIsChanged = false;
            // 
            // txtPassword
            // 
            this.txtPassword.AccessibleDescription = null;
            this.txtPassword.AccessibleName = null;
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.BackColor = System.Drawing.SystemColors.Window;
            this.txtPassword.BackgroundImage = null;
            this.tableLayoutPanel2.SetColumnSpan(this.txtPassword, 2);
            this.txtPassword.Font = null;
            this.txtPassword.Format = null;
            this.txtPassword.IsRequired = true;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.OldBackColor = System.Drawing.SystemColors.Window;
            this.txtPassword.RequiredHandle = null;
            this.txtPassword.SetValueToControl = null;
            this.txtPassword.TextIsChanged = false;
            // 
            // MSSQLConnSetting
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = null;
            this.Name = "MSSQLConnSetting";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private hwj.UserControls.CommonControls.xButton btnTestDB;
        private hwj.UserControls.CommonControls.xComboBox cboDatabase;
        private hwj.UserControls.CommonControls.xTextBox txtDataSource;
        private hwj.UserControls.CommonControls.xComboBox cboServerType;
        private hwj.UserControls.CommonControls.xComboBox cboVerificationType;
        private hwj.UserControls.CommonControls.xTextBox txtUser;
        private hwj.UserControls.CommonControls.xTextBox txtPassword;
    }
}
