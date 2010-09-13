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
        private string _connectionString;
        [DesignOnly(true)]
        public string ConnectionString
        {
            get
            {
                GetConnectionString();
                return _connectionString;
            }
            set
            {
                _connectionString = value;
                InitConnectionString(_connectionString);
            }
        }

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

        #region Private Function
        private void GetConnectionString()
        {
            try
            {
                string db = string.Empty;
                if (cboDatabase.SelectedValue != null)
                    db = cboDatabase.SelectedValue.ToString();

                _connectionString = hwj.CommonLibrary.Object.StringHelper.GetMSSQLConnectionString(txtDataSource.Text, db,
                    txtUser.Enabled ? txtUser.Text : string.Empty, txtPassword.Enabled ? txtPassword.Text : string.Empty, cboVerificationType.SelectedIndex == 1);
            }
            catch
            {
                _connectionString = string.Empty;
            }
        }
        private void InitConnectionString(string connectionString)
        {
            if (DesignMode)
                return;
            if (string.IsNullOrEmpty(connectionString))
                return;
            try
            {
                SqlConnectionStringBuilder sqlConn = new SqlConnectionStringBuilder(connectionString);

                if (sqlConn != null)
                {
                    SetDatabaseDataSource(connectionString);

                    txtDataSource.Text = sqlConn.DataSource;
                    cboDatabase.SelectedValue = sqlConn.InitialCatalog;
                    txtUser.Text = sqlConn.UserID;
                    txtPassword.Text = sqlConn.Password;

                    if (!string.IsNullOrEmpty(sqlConn.Password))
                    {
                        cboVerificationType.SelectedIndex = 0;
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        if (!string.IsNullOrEmpty(connection.ServerVersion))
                        {
                            if (connection.ServerVersion.StartsWith("08"))
                            {
                                cboServerType.SelectedIndex = 0;
                            }
                            else if (connection.ServerVersion.StartsWith("09"))
                            {
                                cboServerType.SelectedIndex = 1;
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //UI.Common.MsgInfo(ex.Message);
                throw ex;
            }
        }
        private void SetDatabaseDataSource(string ConnectionString)
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        DataTable tb1 = connection.GetSchema("Databases");
                        DataView dv = tb1.DefaultView;
                        dv.Sort = "database_name";
                        cboDatabase.DisplayMember = "database_name";
                        cboDatabase.ValueMember = "database_name";
                        cboDatabase.DataSource = dv;
                    }
                    catch (Exception ex)
                    {
                        //UI.Common.MsgInfo(ex.Message);
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        #endregion
    }
}
