using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using hwj.UserControls.Interface;
using System.Data;

namespace hwj.UserControls.Suggest.View
{
    public partial class SuggestBoxView : UserControl, IEnterEqualTab
    {
        private int selectIndex = 0;
        private bool IsShowed
        {
            get
            {
                if (tsDropDown != null && tsDropDown.Visible)
                    return true;
                else
                    return false;
            }
        }
        private ToolStripDropDown tsDropDown = null;
        private ToolStripControlHost tsCH = null;
        private SuggestView ListControl = null;
        private int RecordCount
        {
            get
            {
                if (ListControl != null && ListControl.DataList != null)
                    return ListControl.DataList.Count;
                else
                    return 0;
            }
        }
        protected Function.Verify.RequiredHandle RequiredHandle { get; set; }

        #region Event Object
        public delegate void SelectedValueHandler(SuggestValue e);
        public event SelectedValueHandler OnSelected;
        public event EventHandler SelectedValueChanged;
        public event EventHandler OnFocus;
        public event EventHandler DoubleClick;
        public event EventHandler ButtonClick;
        public event KeyEventHandler KeyDown;
        protected delegate void DataBindingHandler(DataBindingArg e);
        protected event DataBindingHandler DataBinding;
        #endregion

        #region Property
        public const string ValueFlag = "[Sug_Value]";
        public string FilterString { get; set; }
        public string ValueMember { get; set; }
        public string FirstMember { get; set; }
        public string SecondMember { get; set; }

        private bool _ReadOnly = false;
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                txtValue.ReadOnly = ReadOnly;
                btnSelect.Visible = !ReadOnly;
            }
        }

        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }

        [DefaultValue(false)]
        public bool IsRequired
        {
            get { return txtValue.IsRequired; }
            set { txtValue.IsRequired = value; }
        }

        [Browsable(false)]
        public System.Drawing.Color OldBackColor { get; set; }

        [DefaultValue(typeof(SystemColors), "Window")]
        public new Color BackColor
        {
            get { return txtValue.BackColor; }
            set { txtValue.BackColor = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]//Hidden = 0
        public DataTable DataSource { get; set; }

        public bool SecondColumnMode
        {
            get { return ListControl.SecondColumnMode; }
            set { ListControl.SecondColumnMode = value; }
        }
        public string PrimaryColumnHeaderName
        {
            get { return ListControl.PrimaryColumnName; }
            set { ListControl.PrimaryColumnName = value; }
        }
        public string SecondColumnHeaderName
        {
            get { return ListControl.SecondColumnName; }
            set { ListControl.SecondColumnName = value; }
        }
        private string _SelectedValue = string.Empty;
        [DesignOnly(true)]
        public string SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.txtValue.Text) && string.IsNullOrEmpty(_SelectedValue))
                    return EmptyValue;
                else
                    return _SelectedValue;
            }
            set
            {
                if (DataSource != null)
                {
                    _SelectedText = GetMatchText(value);
                    txtValue.Text = _SelectedText;
                }
                SetSelectedValue(value);
            }
        }
        private string _SelectedText = string.Empty;
        [DesignOnly(true)]
        public string SelectedText
        {
            get { return _SelectedText; }
            set
            {
                if (DataSource != null)
                    SetSelectedValue(GetMatchValue(value));
                SetSelectedText(value);
            }
        }

        public override string Text
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }
        [DefaultValue(true), Browsable(true)]
        public bool ButtonVisible
        {
            get { return btnSelect.Visible; }
            set { btnSelect.Visible = value; }
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
        public int MaxLength
        {
            get { return txtValue.MaxLength; }
            set { txtValue.MaxLength = value; }
        }
        public enum DisplayMemberType
        {
            Primary,
            Second,
        }
        [DefaultValue(DisplayMemberType.Primary)]
        public DisplayMemberType DisplayMember { get; set; }
        /// <summary>
        /// 返回最大记录数
        /// </summary>
        [Description("返回最大记录数"), DefaultValue(0)]
        public int MaxRecords { get; set; }

        public enum SuggextBoxStyle
        {
            DropDownList,
            Suggest,
        }
        /// <summary>
        /// 获取或设置SuggestBox Dropdown的显示方式
        /// </summary>
        [DefaultValue(SuggextBoxStyle.Suggest), Description("获取或设置SuggestBox Dropdown的显示方式")]
        public SuggextBoxStyle DropDownStyle { get; set; }

        [DefaultValue(true)]
        public bool ShowToolTip { get; set; }

        public int ListWidth { get; set; }
        public Image Image
        {
            get { return btnSelect.Image; }
            set { btnSelect.Image = value; }
        }

        [Description("当值改变时,同时赋值给指定的控件")]
        public SuggestBoxView SetValueToControl { get; set; }

        /// <summary>
        /// 获取或设置触发搜索的字符最小长度
        /// </summary>
        [Description("获取或设置触发搜索的字符最小长度"), DefaultValue(0)]
        public int SearchMinLength { get; set; }
        #endregion

        public SuggestBoxView()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            InitializeComponent();
            DropDownStyle = SuggextBoxStyle.Suggest;

            txtValue.EnterEqualTab = false;
            _ReadOnly = false;
            ShowToolTip = true;

            OldBackColor = this.txtValue.OldBackColor;
            IsRequired = false;
            EnterEqualTab = true;
            SetValueToControl = null;

            if (!DesignMode)
            {
                this.Disposed += new EventHandler(SuggestBox_Disposed);
                DataSource = new DataTable();

                txtValue.LostFocus += new EventHandler(txtValue_LostFocus);

                ListControl = new SuggestView();
                ListControl.SelectedValue += new SuggestView.SelectedValueHandler(lstCtrl_SelectedValue);

                tsCH = new ToolStripControlHost(ListControl);
                tsCH.Padding = new Padding(0);
                tsCH.Dock = DockStyle.Fill;

                tsDropDown = new ToolStripDropDown();
                tsDropDown.Padding = new Padding(0);
                tsDropDown.DropShadowEnabled = true;
                tsDropDown.Items.Add(tsCH);
            }

        }

        protected override void OnCreateControl()
        {
            #region Set Width
            Size MinSize = new Size(this.Width, 150);
            tsDropDown.MinimumSize = MinSize;
            ListControl.MinimumSize = MinSize;

            if (ListWidth != 0)
            {
                tsDropDown.Width = ListWidth;
                ListControl.Width = ListWidth;
            }
            #endregion

            //btnSelect.Visible = ButtonVisible;
            txtValue.MaxLength = MaxLength;
            this.txtValue.BackColor = SystemColors.Window;

            base.OnCreateControl();
            if (Image == null)
                Image = Properties.Resources.page_search;
            if (DropDownStyle == SuggextBoxStyle.DropDownList)
            {
                txtValue.ReadOnly = true;
            }
            else
            {
                txtValue.ReadOnly = false;
                if (IsRequired)
                    this.txtValue.BackColor = Common.RequiredBackColor;
                txtValue.ReadOnly = ReadOnly;
                btnSelect.Visible = !ReadOnly;
            }
            txtValue.SetRequiredStatus();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab && IsShowed)
            {
                ListControl.SetSelectedValue(selectIndex);
                selectIndex = 0;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Events
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (ButtonClick == null)
            {
                if (OnFocus != null)
                    OnFocus(sender, e);
                if (DropDownStyle == SuggextBoxStyle.Suggest)
                    Clear();
                ShowList(sender, e);
            }
            else
                ButtonClick(sender, e);
        }
        private void lstCtrl_SelectedValue(SuggestValue e)
        {
            if (e != null)
            {
                if (DisplayMember == DisplayMemberType.Primary)
                    _SelectedText = e.PrimaryValue;
                else if (DisplayMember == DisplayMemberType.Second)
                    _SelectedText = e.SecondValue;
                this.txtValue.Text = this.SelectedText;
                SetSelectedValue(e.Key);
            }
            else
            {
                _SelectedText = string.Empty;
                this.txtValue.Text = this.SelectedText;
                SetSelectedValue(EmptyValue);
            }

            if (SetValueToControl != null)
            {
                SetValueToControl.SetSelectedValue(SelectedValue);
                SetValueToControl.SetSelectedText(SelectedText);
            }

            CloseList(true);

            if (OnSelected != null)
                OnSelected(e);
        }
        private void ParentForm_Move(object sender, EventArgs e)
        {
            CloseList(true);
        }
        void SuggestBox_Disposed(object sender, EventArgs e)
        {
            Common.HideToolTip();
            if (tsDropDown != null && tsDropDown.IsHandleCreated)
            {
                tsDropDown.Dispose();
            }
        }
        #region Text Control
        private void txtValue_Enter(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            if (DropDownStyle == SuggextBoxStyle.DropDownList)
                txtValue.SelectAll();
            if (OnFocus != null)
                OnFocus(sender, e);
        }
        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (DesignMode)
                return;
            this.Cursor = Cursors.AppStarting;
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (IsShowed)
                    {
                        ListControl.SetSelectedValue(selectIndex);
                        selectIndex = 0;
                        return;
                    }
                    else
                    {
                        if (EnterEqualTab)
                            SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down && IsShowed)
                {
                    if (selectIndex < RecordCount - 1)
                        selectIndex++;
                    ListControl.SelectIndex(selectIndex);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Up && IsShowed)
                {
                    if (selectIndex > 0)
                        selectIndex--;
                    ListControl.SelectIndex(selectIndex);
                    e.Handled = true;
                }
                if (KeyDown != null)
                    KeyDown(sender, e);
            }
            catch
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void txtValue_LostFocus(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CloseList(false);
        }
        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            if (!(txtValue.Focused || IsShowed) || ReadOnly) return;
            this.Cursor = Cursors.AppStarting;
            try
            {
                if (txtValue.ValueChangedHandle != null)
                    txtValue.ValueChangedHandle.IsChanged = true;
                if (txtValue.Text.Length >= SearchMinLength)
                {
                    if (!string.IsNullOrEmpty(txtValue.Text))
                        ShowList(sender, e);
                }
                else if (ShowToolTip)
                {
                    Common.ShowToolTipInfo(this, string.Format(Properties.Resources.InvalidInputString, SearchMinLength));
                }
            }
            catch (Exception ex)
            {
                Common.ShowToolTipError(this, ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }
        private void txtValue_Validating(object sender, CancelEventArgs e)
        {
            if (DesignMode)
                return;
            if (ReadOnly) return;
            if (_SelectedText != this.txtValue.Text)
                Clear();
            if (IsRequired)
            {
                if (string.IsNullOrEmpty(this.Text))
                    this.BackColor = Common.RequiredBackColor;
                else
                    this.BackColor = OldBackColor;
            }
        }
        private void txtValue_DoubleClick(object sender, EventArgs e)
        {
            if (ReadOnly) return;
            this.Cursor = Cursors.AppStarting;
            try
            {
                if (DoubleClick != null)
                    DoubleClick(sender, e);
            }
            catch
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void txtValue_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            if (DropDownStyle == SuggextBoxStyle.DropDownList)
                ShowList(sender, e);
        }
        #endregion
        #endregion

        #region Public Functions
        private void DataBind()
        {
            if (DataBinding != null)
            {
                DataBindingArg e = new DataBindingArg();
                DataBinding(e);
                if (e.FilterDataSource != null)
                {
                    ListControl.DataList = e.FilterDataSource.DefaultView;
                }
            }
            else
            {
                if (DataSource != null)
                {
                    if (txtValue.Text == string.Empty)
                        ListControl.DataList = DataSource.DefaultView;
                    else if (DropDownStyle == SuggextBoxStyle.Suggest)
                        ListControl.DataList = SearchValue(txtValue.Text);
                    else
                        ListControl.DataList = DataSource.DefaultView;
                }
            }
            ListControl.DataBind();
        }
        public void Clear()
        {
            _SelectedText = string.Empty;
            txtValue.Text = string.Empty;
            SetSelectedValue(EmptyValue);
        }
        public new void Focus()
        {
            txtValue.Focus();
        }
        /// <summary>
        /// 设置SelectedValue,不触发任何处理
        /// </summary>
        /// <param name="value"></param>
        public void SetSelectedValue(string value)
        {
            if (_SelectedValue != value && SelectedValueChanged != null)
            {
                _SelectedValue = value;
                SelectedValueChanged(this, new EventArgs());
            }
            _SelectedValue = value;
        }
        /// <summary>
        /// 设置SelectedText,不触发任何处理
        /// </summary>
        /// <param name="text"></param>
        public void SetSelectedText(string text)
        {
            _SelectedText = text;
            txtValue.Text = text;
        }
        #endregion

        #region Private Functions
        private void ShowList(object sender, EventArgs e)
        {
            if (ReadOnly) return;
            DataBind();
            selectIndex = 0;
            if (RecordCount > 0)
            {
                if (tsDropDown != null)
                {
                    if (tsDropDown.AutoClose)
                        tsDropDown.AutoClose = false;

                    if (this.ParentForm != null)
                    {
                        this.ParentForm.Move -= new EventHandler(ParentForm_Move);
                        this.ParentForm.Move += new EventHandler(ParentForm_Move);
                    }

                    ListControl.IsEnterList = false;
                    ListControl.Width = tsDropDown.Width;
                    tsDropDown.Show(this, 0, this.Height);
                    Focus();
                }
            }
            else
            {
                CloseList(true);
                if (ShowToolTip)
                    Common.ShowToolTipInfo(this, Properties.Resources.NoRecord);
            }
        }
        private void CloseList(bool nocheck)
        {
            if (tsDropDown == null)
                return;

            if (nocheck)
                tsDropDown.Close();
            else if (!ListControl.IsEnterList)
                tsDropDown.Close();

            if (DropDownStyle == SuggextBoxStyle.DropDownList)
                txtValue.SelectAll();
        }
        private DataView SearchValue(string value)
        {
            DataView dv = new DataView();
            if (DataSource != null)
            {
                dv = DataSource.DefaultView;
                dv.RowFilter = FilterString.Replace(ValueFlag, value);
            }
            return dv;
        }
        private string GetMatchText(string value)
        {
            DataView dv = GetDataView();
            if (dv != null)
            {
                string filterStr = string.Format("{0}='{1}'", ValueMember, value);
                dv.RowFilter = filterStr;
                if (dv.Count > 0)
                {
                    string member = string.Empty;
                    if (DisplayMember == DisplayMemberType.Primary)
                        member = FirstMember;
                    else if (DisplayMember == DisplayMemberType.Second)
                        member = SecondMember;

                    object obj = dv[0][member];
                    if (obj != null)
                        return obj.ToString();
                }
            }
            return string.Empty;
        }
        private string GetMatchValue(string text)
        {
            DataView dv = GetDataView();
            if (dv != null)
            {
                string member = string.Empty;
                if (DisplayMember == DisplayMemberType.Primary)
                    member = FirstMember;
                else if (DisplayMember == DisplayMemberType.Second)
                    member = SecondMember;

                string filterStr = string.Format("{0}='{1}'", member, text);
                dv.RowFilter = filterStr;
                if (dv.Count > 0)
                {
                    object obj = dv[0][ValueMember];
                    if (obj != null)
                        return obj.ToString();
                }
            }

            return string.Empty;
        }
        private DataView GetDataView()
        {
            if (DataSource != null)
            {
                DataView dv = new DataView();
                dv = DataSource.DefaultView;
                return dv;
            }
            else
            {
                return null;
            }

        }
        #endregion

    }
}
