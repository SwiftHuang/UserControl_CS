using System;
using System.ComponentModel;
using System.Data.Common;
using System.Threading;
using System.Windows.Forms;

namespace hwj.UserControls.Other
{
    public partial class CommonConnSetting : UserControl
    {
        //"Server=127.0.0.1;Port=5432;User Id=postgres;Password=113502;Database=T1;";
        public delegate void ConnectionTest(string connStr);

        #region Property

        [DesignOnly(true), Browsable(false)]
        public string ConnectionString
        {
            get
            {
                return GetConnectionString(0);
                //return _connectionString;
            }
            //set
            //{
            //    _connectionString = value;
            //    SetConnectionString(_connectionString);
            //}
        }

        /// <summary>
        ///
        /// </summary>
        [DesignOnly(true), Browsable(false)]
        public DbConnectionStringBuilder DbConnStringBuilder { get; set; }

        public string ServerKey { get; set; }
        public string UserKey { get; set; }
        public string PortKey { get; set; }
        public string PasswordKey { get; set; }
        public string DatabaseKey { get; set; }
        public string TimeoutKey { get; set; }

        #endregion Property

        public CommonConnSetting()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;

            InitializeComponent();

            if (this.DesignMode)
                return;
        }

        #region Event Function
        //暂时不能实现
        //private void btnTestDB_Click(object sender, EventArgs e)
        //{
        //    if ((txtUser.Enabled && string.IsNullOrEmpty(txtUser.Text)) || (txtPassword.Enabled && string.IsNullOrEmpty(txtPassword.Text)) || string.IsNullOrEmpty(txtServer.Text))
        //    {
        //        MessageBox.Show(Properties.Resources.RequiredInfo, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    string error = string.Empty;
        //    string connStr = ConnectionString;
        //    if (IsValidConnectionString(new ConnectionTest(null), out error))
        //    {
        //        MessageBox.Show(Properties.Resources.TestConnectionSucceeded, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    else
        //    {
        //        MessageBox.Show(error, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

        #endregion Event Function

        #region Public Function

        public void SetConnectionString(DbConnectionStringBuilder dbConnectionStringBuilder)
        {
            if (DesignMode || dbConnectionStringBuilder == null)
                return;

            try
            {
                DbConnStringBuilder = dbConnectionStringBuilder;

                if (DbConnStringBuilder != null)
                {
                    txtServer.Text = GetValue(DbConnStringBuilder, ServerKey);
                    txtPort.Text = GetValue(DbConnStringBuilder, PortKey);
                    txtDatabase.Text = GetValue(DbConnStringBuilder, DatabaseKey);
                    txtUser.Text = GetValue(DbConnStringBuilder, UserKey);
                    txtPassword.Text = GetValue(DbConnStringBuilder, PasswordKey);
                    txtTimeout.Text = GetValue(DbConnStringBuilder, TimeoutKey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public bool IsValidConnectionString(ConnectionTest connectionTest, out string error)
        {
            error = string.Empty;
            string connStr = GetConnectionString(10);
            try
            {
                connectionTest(connStr);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }

        #endregion Public Function

        #region Private Function

        private string GetValue(DbConnectionStringBuilder dbConnectionStringBuilder, string key)
        {
            if (dbConnectionStringBuilder != null && dbConnectionStringBuilder[key] != null)
            {
                return dbConnectionStringBuilder[key].ToString();
            }
            return string.Empty;
        }

        private string GetConnectionString(int timeOut)
        {
            if (this.DesignMode)
            {
                return string.Empty;
            }
            try
            {
                string db = string.Empty;

                if (DbConnStringBuilder == null)
                {
                    DbConnStringBuilder = new DbConnectionStringBuilder();
                }
                DbConnStringBuilder[ServerKey] = txtServer.Text;
                DbConnStringBuilder[PortKey] = txtPort.Text;
                DbConnStringBuilder[UserKey] = txtUser.Text;
                DbConnStringBuilder[PasswordKey] = txtPassword.Text;
                DbConnStringBuilder[DatabaseKey] = txtDatabase.Text;

                if (timeOut > 0)
                {
                    DbConnStringBuilder[TimeoutKey] = timeOut;
                }

                if (DbConnStringBuilder != null)
                {
                    return DbConnStringBuilder.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }
        }

        private void btnTestDB_Click(object sender, EventArgs e)
        {

        }

        //private bool SetDatabaseList(string currentDatabase)
        //{
        //    string connStr = GetConnectionString(true, 3);
        //    if (!string.IsNullOrEmpty(connStr))
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            try
        //            {
        //                conn.Open();
        //                DataTable tb1 = conn.GetSchema("Databases");
        //                DataView dv = tb1.DefaultView;
        //                dv.Sort = "database_name";
        //                cboDatabase.DisplayMember = "database_name";
        //                cboDatabase.ValueMember = "database_name";
        //                cboDatabase.DataSource = dv;
        //                cboDatabase.SelectedValue = currentDatabase;

        //                if (!string.IsNullOrEmpty(conn.ServerVersion))
        //                {
        //                    if (conn.ServerVersion.StartsWith("08"))
        //                    {
        //                        cboServerType.SelectedIndex = 0;
        //                    }
        //                    else if (conn.ServerVersion.StartsWith("09"))
        //                    {
        //                        cboServerType.SelectedIndex = 1;
        //                    }
        //                }
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, Properties.Resources.ConnectionSetting, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return false;
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }
        //        }
        //    }
        //    return true;
        //}

        #endregion Private Function
    }
}