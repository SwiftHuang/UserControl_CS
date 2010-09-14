using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace hwj.UserControls.Other
{
    public partial class MSSQLConnSetting : UserControl
    {
        #region Property
        //private string _connectionString;
        [DesignOnly(true), Browsable(false)]
        public string ConnectionString
        {
            get
            {
                return GetConnectionString();
                //return _connectionString;
            }
            //set
            //{
            //    _connectionString = value;
            //    SetConnectionString(_connectionString);
            //}
        }
        [DesignOnly(true), Browsable(false)]
        public SqlConnectionStringBuilder SqlConnStringBuilder { get; set; }
        #endregion

        public MSSQLConnSetting()
        {
            InitializeComponent();

            if (this.DesignMode)
                return;

            cboServerType.SelectedIndex = 0;
            cboVerificationType.SelectedIndex = 0;
        }

        #region Event Function
        private void cboDatabase_Click(object sender, EventArgs e)
        {
            btnRefreshDB.PerformClick();
        }
        private void cboVerificationType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboVerificationType.SelectedIndex == 0)
            {
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
            }
            else
            {
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
            }
        }
        private void btnRefreshDB_Click(object sender, EventArgs e)
        {
            if ((txtUser.Enabled && string.IsNullOrEmpty(txtUser.Text)) || (txtPassword.Enabled && string.IsNullOrEmpty(txtPassword.Text)) || string.IsNullOrEmpty(txtDataSource.Text))
            {
                MessageBox.Show(Properties.Resources.RequiredInfo, "Connection Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!string.IsNullOrEmpty(ConnectionString))
            {
                SetDatabaseDataSource(ConnectionString);
            }
        }
        #endregion

        #region Publice Function
        public void SetConnectionString(string connectionString)
        {
            if (DesignMode || string.IsNullOrEmpty(connectionString))
                return;

            try
            {
                SqlConnStringBuilder = new SqlConnectionStringBuilder(connectionString);

                if (SqlConnStringBuilder != null)
                {
                    SetDatabaseDataSource(connectionString);

                    txtDataSource.Text = SqlConnStringBuilder.DataSource;
                    cboDatabase.SelectedValue = SqlConnStringBuilder.InitialCatalog;

                    if (SqlConnStringBuilder.IntegratedSecurity == false)
                    {
                        cboVerificationType.SelectedIndex = 0;

                        txtUser.Enabled = true;
                        txtPassword.Enabled = true;
                        txtUser.Text = SqlConnStringBuilder.UserID;
                        txtPassword.Text = SqlConnStringBuilder.Password;
                    }
                    else
                    {
                        cboVerificationType.SelectedIndex = 1;

                        txtUser.Enabled = false;
                        txtPassword.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Connection Setting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Private Function
        private string GetConnectionString()
        {
            try
            {
                string db = string.Empty;
                if (cboDatabase.SelectedValue != null)
                    db = cboDatabase.SelectedValue.ToString();

                SqlConnStringBuilder = hwj.CommonLibrary.Function.ConnectionString.GetMSSQLConnectionStringBuilder(txtDataSource.Text, db,
                    txtUser.Enabled ? txtUser.Text : string.Empty, txtPassword.Enabled ? txtPassword.Text : string.Empty, cboVerificationType.SelectedIndex == 1);

                if (SqlConnStringBuilder != null)
                    return SqlConnStringBuilder.ToString();
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Connection Setting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }
        }
        private bool SetDatabaseDataSource(string ConnectionString)
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        DataTable tb1 = conn.GetSchema("Databases");
                        DataView dv = tb1.DefaultView;
                        dv.Sort = "database_name";
                        cboDatabase.DisplayMember = "database_name";
                        cboDatabase.ValueMember = "database_name";
                        cboDatabase.DataSource = dv;

                        if (!string.IsNullOrEmpty(conn.ServerVersion))
                        {
                            if (conn.ServerVersion.StartsWith("08"))
                            {
                                cboServerType.SelectedIndex = 0;
                            }
                            else if (conn.ServerVersion.StartsWith("09"))
                            {
                                cboServerType.SelectedIndex = 1;
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Connection Setting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
