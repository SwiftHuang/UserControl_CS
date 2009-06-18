using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace hwj.UserControls
{
    public partial class AutoComplete : UserControl
    {
        private const string ListInfoFormat1 = "查:{0}条";
        private const string ListInfoFormat2 = "共:{0}条";
        private const string NoFoundRecord = "当前不存在你所需要的记录";
        private const string AutoCompleteInfo = "按回车可以进行模糊查找,查找过程中请注意区分英文大小写";

        private bool _isDisplayInfo = false;
        private ToolStripDropDown tsDropDown = null;
        private ToolStripControlHost tsCH = null;
        private AutoCompleteList_I lstCtrl = null;
        private ToolTip toolTip1 = new ToolTip();
        //private DateTime StartTime = DateTime.Now;
        //private DateTime EndTime = DateTime.Now;

        public delegate void SelectedValueHandler(AutoCompleteValue e);
        public event SelectedValueHandler OnSelected;
        public event EventHandler OnFocus;

        #region Property
        private List<AutoCompleteValue> _dataList = new List<AutoCompleteValue>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]//Hidden = 0
        public List<AutoCompleteValue> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }
        public bool SecondColumnMode
        {
            get { return lstCtrl.SecondColumnMode; }
            set { lstCtrl.SecondColumnMode = value; }
        }
        public string PrimaryColumnHeaderName
        {
            get { return lstCtrl.PrimaryColumnName; }
            set { lstCtrl.PrimaryColumnName = value; }
        }
        public string SecondColumnHeaderName
        {
            get { return lstCtrl.SecondColumnName; }
            set { lstCtrl.SecondColumnName = value; }
        }
        private string _SelectedValue = string.Empty;
        [BrowsableAttribute(false)]
        public string SelectedValue
        {
            get
            {
                if (this.txtValue.Text == string.Empty)
                    return EmptyValue;
                else
                    return _SelectedValue;
            }
            set { _SelectedValue = value; }
        }
        private string _SelectedText = string.Empty;
        [BrowsableAttribute(false)]
        public string SelectedText
        {
            get { return _SelectedText; }
            set { _SelectedText = value; }
        }
        public override string Text
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }
        private int _TotalRecordCount = 0;
        public int TotalRecoudCount
        { get { return _TotalRecordCount; } }
        private int _SearchRecordCount = 0;
        public int SearchRecordCount
        { get { return _SearchRecordCount; } }
        private bool _ButtonVisible = true;
        public bool ButtonVisible
        {
            get { return _ButtonVisible; }
            set { _ButtonVisible = value; }
        }
        private string _emptyValue = string.Empty;
        /// <summary>
        /// 如果TextBox.Text为空时,SelectedValue的值.
        /// </summary>
        public string EmptyValue
        {
            get { return _emptyValue; }
            set { _emptyValue = value; }
        }
        public int MaxLength { get; set; }
        #endregion

        public AutoComplete()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                Size MinSize = new Size(100, 150);
                lstCtrl = new AutoCompleteList_I();
                lstCtrl.MinimumSize = MinSize;
                //lstCtrl.MaximumSize = MaxSize;
                lstCtrl.SelectedValue += new AutoCompleteList_I.SelectedValueHandler(lstCtrl_SelectedValue);

                tsCH = new ToolStripControlHost(lstCtrl);
                tsCH.Padding = new Padding(0);
                tsCH.Dock = DockStyle.Fill;

                tsDropDown = new ToolStripDropDown();
                //tsDropDown.BackColor = System.Drawing.Color.LightBlue;
                tsDropDown.Padding = new Padding(0);
                tsDropDown.DropShadowEnabled = true;
                tsDropDown.Items.Add(tsCH);
            }
        }
        private void AutoComplete_Load(object sender, EventArgs e)
        {
            tsDropDown.Width = this.Width;
            btnSelect.Visible = ButtonVisible;
            txtValue.MaxLength = MaxLength;
        }

        #region Events
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (OnFocus != null)
                OnFocus(sender, e);
            else
                DataBind();

            ShowList();
        }
        private void txtValue_Enter(object sender, EventArgs e)
        {
            if (!_isDisplayInfo)
            {
                _isDisplayInfo = true;
                toolTip1.ToolTipIcon = ToolTipIcon.Info;
                toolTip1.ToolTipTitle = "提示信息";
                toolTip1.Show(AutoCompleteInfo, this.txtValue, this.txtValue.Location.X, this.txtValue.Location.Y + this.txtValue.Height, 3000);
            }
            if (OnFocus != null)
                OnFocus(sender, e);
        }
        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtValue.Text == string.Empty)
                    DataBind();
                else
                    SearchPrimaryColumn(txtValue.Text);

                ShowList();
            }
        }
        private void lstCtrl_SelectedValue(AutoCompleteValue e)
        {
            if (e != null)
            {
                this.SelectedValue = e.Key;
                this.SelectedText = e.PrimaryValue;
            }
            else
            {
                this.SelectedValue = string.Empty;
                this.SelectedText = string.Empty;
            }
            this.txtValue.Text = this.SelectedText;
            CloseList();

            if (OnSelected != null)
                OnSelected(e);
        }
        #endregion

        #region Public Functions
        public void DataBind()
        {
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            try
            {
                //StartTime = DateTime.Now;
                _TotalRecordCount = DataList.Count;
                _SearchRecordCount = _TotalRecordCount;
                lstCtrl.DataList = DataList;
                lstCtrl.DataBind();
                SetFootText();
            }
            catch (Exception e)
            { throw e; }
            finally { this.Cursor = System.Windows.Forms.Cursors.Default; }
        }
        public void Clear()
        {
            SelectedText = string.Empty;
            SelectedValue = EmptyValue;
            txtValue.Text = string.Empty;
        }
        public new void Focus()
        {
            txtValue.Focus();
        }
        #endregion

        #region Private Functions
        private void ShowList()
        {
            if (TotalRecoudCount != 0 && SearchRecordCount != 0)
            {
                if (tsDropDown != null)
                {
                    lstCtrl.Width = tsDropDown.Width;
                    //lstCtrl.Height = tsDropDown.Height;
                    tsDropDown.Show(this, 0, this.Height);
                    lstCtrl.SetListFocus();
                }
            }
            else
            {
                toolTip1.ToolTipIcon = ToolTipIcon.None;
                toolTip1.Show(NoFoundRecord, this.txtValue, this.txtValue.Location.X, this.txtValue.Location.Y + this.txtValue.Height, 5000);
            }
        }
        private void CloseList()
        {
            tsDropDown.Close();
        }
        private void SearchDataBind(List<AutoCompleteValue> lst)
        {
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            try
            {
                _SearchRecordCount = lst.Count;
                lstCtrl.DataList = lst;
                lstCtrl.DataBind();
                SetFootText();
            }
            catch (Exception e)
            { throw e; }
            finally { this.Cursor = System.Windows.Forms.Cursors.Default; }
        }
        private void SearchPrimaryColumn(string value)
        {
            //StartTime = DateTime.Now;
            AutoCompleteCommon.SearchValue = value;
            SearchDataBind(DataList.FindAll(AutoCompleteCommon.SearchPrimaryValue));
        }
        private void SetFootText()
        {
            //EndTime = DateTime.Now;
            //TimeSpan ts = EndTime - StartTime;
            lstCtrl.FootText1 = string.Format(ListInfoFormat1, SearchRecordCount);
            lstCtrl.FootText2 = string.Format(ListInfoFormat2, TotalRecoudCount);
            //lstCtrl.FootTimeText = (ts.TotalMilliseconds / 1000).ToString("N");
        }
        #endregion
    }
}
