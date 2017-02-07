using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;

namespace hwj.UserControls.Other
{
    public partial class MSSQLConnSetting : UserControl
    {
        #region Property

        [DesignOnly(true), Browsable(false)]
        public string ConnectionString
        {
            get
            {
                return GetConnectionString(false, 0);
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

        #endregion Property

        public MSSQLConnSetting()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;

            InitializeComponent();

            if (this.DesignMode)
                return;

            cboServerType.SelectedIndex = 0;
            cboVerificationType.SelectedIndex = 0;
        }

        #region Event Function

        private void cboDatabase_Click(object sender, EventArgs e)
        {
        }

        private void cboDatabase_DropDown(object sender, EventArgs e)
        {
            SetDatabaseList(cboDatabase.Text);
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
                MessageBox.Show(Properties.Resources.RequiredInfo, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string error = string.Empty;
            if (IsValidConnectionString(true, out error))
            {
                MessageBox.Show(Properties.Resources.TestConnectionSucceeded, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(error, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion Event Function

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
                    //SetDatabaseDataSource(connectionString);

                    txtDataSource.Text = SqlConnStringBuilder.DataSource;

                    cboDatabase.Text = SqlConnStringBuilder.InitialCatalog;

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
                MessageBox.Show(ex.Message, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public bool IsValidConnectionString()
        {
            string error = string.Empty;
            return IsValidConnectionString(out error);
        }

        public bool IsValidConnectionString(out string error)
        {
            return IsValidConnectionString(false, out error);
        }

        private bool IsValidConnectionString(bool ignoreDatabase, out string error)
        {
            error = string.Empty;
            string connStr = GetConnectionString(ignoreDatabase, 3);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    DataTable tb1 = conn.GetSchema("Databases");
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return true;
        }

        #endregion Publice Function

        #region Private Function

        private string GetConnectionString(bool ignoreDatabase, int timeOut)
        {
            try
            {
                string db = string.Empty;
                if (!ignoreDatabase)
                {
                    db = cboDatabase.Text;
                }

                SqlConnStringBuilder = hwj.CommonLibrary.Function.ConnectionString.GetMSSQLConnectionStringBuilder(txtDataSource.Text, db,
                    txtUser.Enabled ? txtUser.Text : string.Empty, txtPassword.Enabled ? txtPassword.Text : string.Empty, cboVerificationType.SelectedIndex == 1);

                if (timeOut > 0)
                {
                    SqlConnStringBuilder.ConnectTimeout = timeOut;
                }

                if (SqlConnStringBuilder != null)
                    return SqlConnStringBuilder.ToString();
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }
        }

        private bool SetDatabaseList(string currentDatabase)
        {
            string connStr = GetConnectionString(true, 3);
            if (!string.IsNullOrEmpty(connStr))
            {
                using (SqlConnection conn = new SqlConnection(connStr))
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
                        cboDatabase.SelectedValue = currentDatabase;

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
                        MessageBox.Show(ex.Message, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        #endregion Private Function
    }
}