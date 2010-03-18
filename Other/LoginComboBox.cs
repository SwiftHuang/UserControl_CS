using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace hwj.UserControls.Other
{
    public partial class LoginComboBox : CommonControls.xComboBox
    {
        #region Property
        public string FileName { get; set; }
        //public bool KeepRecord { get; set; }
        ContextMenu menu = new ContextMenu();
        #endregion
        MenuItem menuItemRemoveAll = new MenuItem("Remove All");


        public LoginComboBox()
        {
            menu.Popup += new EventHandler(menu_Popup);
            menuItemRemoveAll.Click += new EventHandler(menuItemRemoveAll_Click);
            InitializeComponent();
            AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
        }

        void menuItemRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveALL();
        }

        void menu_Popup(object sender, EventArgs e)
        {
            //MenuItem menuItem2 = new MenuItem("Remove");
            menu.MenuItems.Clear();
            if (menu.SourceControl == this)
            {
                menu.MenuItems.Add(menuItemRemoveAll);
                //menu.MenuItems.Add(menuItem2);
            }
        }

        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                this.ContextMenu = menu;
                CheckFileName();
                this.DataSource = GetList();
            }
            base.OnCreateControl();
        }

        public bool AddText()
        {
            List<string> list = GetList();

            if (list == null)
                list = new List<string>();
            else
                list.Add(this.Text);

            return UpdateFile(list);
        }
        public bool RemoveALL()
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
                this.DataSource = list;
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
                    strList.Add(sr.ReadLine());
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
    }
}
