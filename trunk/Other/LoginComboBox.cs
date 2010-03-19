using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace hwj.UserControls.Other
{
    public partial class LoginComboBox : CommonControls.xComboBox
    {
        ContextMenu menu = new ContextMenu();
        MenuItem menuItemClearHistory = new MenuItem(Properties.Resources.ClearHistory);
        MenuItem menuItemOpenFile = new MenuItem(Properties.Resources.OpenFile);

        #region Property
        public string FileName { get; set; }
        public bool DefaultDisplayLastRecord { get; set; }
        #endregion

        public LoginComboBox()
        {

            DefaultDisplayLastRecord = true;
            menu.Popup += new EventHandler(menu_Popup);
            menuItemClearHistory.Click += new EventHandler(menuItemClearHistory_Click);
            menuItemOpenFile.Click += new EventHandler(menuItemOpenFile_Click);

            InitializeComponent();

            AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
        }

        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                this.ContextMenu = menu;
                CheckFileName();
                RefreshList(GetList());
            }
            base.OnCreateControl();
        }

        #region Event Function
        void menuItemClearHistory_Click(object sender, EventArgs e)
        {
            ClearHistory();
        }
        void menuItemOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(FileName))
                    System.Diagnostics.Process.Start(FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Properties.Resources.OpenFileException, ex.Message), Properties.Resources.ErrorInfo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        void menu_Popup(object sender, EventArgs e)
        {
            //MenuItem menuItem2 = new MenuItem("Remove");
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            menu.MenuItems.Clear();

            menuItemClearHistory.Text = Properties.Resources.ClearHistory;
            menuItemOpenFile.Text = Properties.Resources.OpenFile;

            if (menu.SourceControl == this)
            {
                menu.MenuItems.Add(menuItemClearHistory);
                if (!string.IsNullOrEmpty(FileName))
                    menu.MenuItems.Add(menuItemOpenFile);
                //menu.MenuItems.Add(menuItem2);
            }
        }
        #endregion

        #region Public Function
        public bool AddText()
        {
            List<string> list = GetList();

            if (list == null)
                list = new List<string>();
            else
            {
                List<string> delList = new List<string>();
                foreach (string s in list)
                {
                    if (s.ToUpper().Trim() == this.Text.ToUpper().Trim())
                        delList.Add(s);
                }
                foreach (string s in delList)
                {
                    list.Remove(s);
                }
                list.Insert(0, this.Text);
            }

            return UpdateFile(list);
        }
        public bool ClearHistory()
        {
            return UpdateFile(null);
        }
        public bool RemoveAt(string text)
        {
            List<string> list = GetList();

            if (list != null && !string.IsNullOrEmpty(text))
            {
                list.Remove(text);
                return UpdateFile(list);
                this.Text = string.Empty;
            }
            return true;
        }
        #endregion

        #region Private Function
        private bool UpdateFile(List<string> list)
        {
            if (!DesignMode)
            {
                CheckFileName();
                using (StreamWriter sw = new StreamWriter(FileName, false, System.Text.Encoding.UTF8))
                {
                    if (list == null)
                        sw.Write(string.Empty);
                    else
                    {
                        foreach (string s in list)
                        {
                            sw.WriteLine(s);
                        }
                    }
                }
                RefreshList(list);
            }
            return true;
        }
        private List<string> GetList()
        {
            List<string> strList = new List<string>();

            CheckFileName();
            using (StreamReader sr = new StreamReader(FileName, System.Text.Encoding.UTF8))
            {
                while (sr.Peek() >= 0)
                {
                    string tmpStr = sr.ReadLine();
                    if (!string.IsNullOrEmpty(tmpStr))
                        strList.Add(tmpStr);
                }
            }
            return strList;
        }
        private void CheckFileName()
        {
            if (!DesignMode && !File.Exists(FileName))
            {
                string directory = Path.GetDirectoryName(FileName);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                else
                    using (File.Create(FileName))
                    {
                    }

            }
        }
        private void RefreshList(List<string> list)
        {
            this.DataSource = list;
            if (list != null && list.Count > 0 && DefaultDisplayLastRecord)
                this.SelectedIndex = 0;
        }
        #endregion
    }
}
