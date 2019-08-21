using hwj.UserControls.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace hwj.UserControls.Suggest
{
    public partial class SuggestBox : UserControl, IEnterEqualTab, IValueChanged
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

        private List<SelectedValueHandler> OnSelecteds = new List<SelectedValueHandler>();
        private List<EventHandler> SelectedValueChangeds = new List<EventHandler>();
        private List<EventHandler> OnFocuss = new List<EventHandler>();
        private List<EventHandler> DataBindingLst = new List<EventHandler>();
        private List<EventHandler> DoubleClicks = new List<EventHandler>();
        private List<EventHandler> ButtonClicks = new List<EventHandler>();
        private List<KeyEventHandler> KeyDowns = new List<KeyEventHandler>();

        private event SelectedValueHandler _OnSelected;

        public event SelectedValueHandler OnSelected
        {
            add { _OnSelected += value; OnSelecteds.Add(value); }
            remove { _OnSelected -= value; OnSelecteds.Remove(value); }
        }

        private event EventHandler _SelectedValueChanged;

        public event EventHandler SelectedValueChanged
        {
            add { _SelectedValueChanged += value; SelectedValueChangeds.Add(value); }
            remove { _SelectedValueChanged -= value; SelectedValueChangeds.Remove(value); }
        }

        private event EventHandler _OnFocus;

        public event EventHandler OnFocus
        {
            add { _OnFocus += value; OnFocuss.Add(value); }
            remove { _OnFocus -= value; OnFocuss.Remove(value); }
        }

        private event EventHandler _DataBinding;

        public event EventHandler DataBinding
        {
            add { _DataBinding += value; DataBindingLst.Add(value); }
            remove { _DataBinding -= value; DataBindingLst.Remove(value); }
        }

        private event EventHandler _DoubleClick;

        public event EventHandler DoubleClick
        {
            add { _DoubleClick += value; DoubleClicks.Add(value); }
            remove { _DoubleClick -= value; DoubleClicks.Remove(value); }
        }

        private event EventHandler _ButtonClick;

        public event EventHandler ButtonClick
        {
            add { _ButtonClick += value; ButtonClicks.Add(value); }
            remove { _ButtonClick -= value; ButtonClicks.Remove(value); }
        }

        private event KeyEventHandler _KeyDown;

        public event KeyEventHandler KeyDown
        {
            add { _KeyDown += value; KeyDowns.Add(value); }
            remove { _KeyDown -= value; KeyDowns.Remove(value); }
        }

        #endregion Event Object

        #region Property

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
        public SuggestList DataList { get; set; }

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

        [BrowsableAttribute(false)]
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
                if (DataList != null)
                {
                    _SelectedText = GetMatchText(value);
                    txtValue.Text = _SelectedText;
                }
                SetSelectedValue(value);
            }
        }

        private string _SelectedText = string.Empty;

        [BrowsableAttribute(false)]
        public string SelectedText
        {
            get { return _SelectedText; }
            set
            {
                if (DataList != null)
                    SetSelectedValue(GetMatchValue(value));
                SetSelectedText(value);
            }
        }

        [BrowsableAttribute(false)]
        public object SelectedItem { get; set; }

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
        public SuggestBox SetValueToControl { get; set; }

        /// <summary>
        /// 获取或设置触发搜索的字符最小长度
        /// </summary>
        [Description("获取或设置触发搜索的字符最小长度"), DefaultValue(0)]
        public int SearchMinLength { get; set; }

        #endregion Property

        public SuggestBox()
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
                DataList = new SuggestList();

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

        public void RemoveAllEvents()
        {
            foreach (SelectedValueHandler eh in OnSelecteds)
            {
                _OnSelected -= eh;
            }
            OnSelecteds.Clear();
            foreach (EventHandler eh in SelectedValueChangeds)
            {
                _SelectedValueChanged -= eh;
            }
            SelectedValueChangeds.Clear();
            foreach (EventHandler eh in OnFocuss)
            {
                _OnFocus -= eh;
            }
            OnFocuss.Clear();
            foreach (EventHandler eh in DataBindingLst)
            {
                _DataBinding -= eh;
            }
            DataBindingLst.Clear();
            foreach (EventHandler eh in DoubleClicks)
            {
                _DoubleClick -= eh;
            }
            DoubleClicks.Clear();
            foreach (EventHandler eh in ButtonClicks)
            {
                _ButtonClick -= eh;
            }
            ButtonClicks.Clear();
            foreach (KeyEventHandler eh in KeyDowns)
            {
                _KeyDown -= eh;
            }
            KeyDowns.Clear();
        }

        protected override void OnCreateControl()
        {
            SetListBoxWidth();
            //#region Set Width
            //Size MinSize = new Size(this.Width, 150);
            //tsDropDown.MinimumSize = MinSize;
            //ListControl.MinimumSize = MinSize;

            //if (ListWidth != 0)
            //{
            //    tsDropDown.Width = ListWidth;
            //    ListControl.Width = ListWidth;
            //}
            //#endregion

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

        protected override void Dispose(bool disposing)
        {
            RemoveAllEvents();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Events

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (ButtonClicks.Count == 0)
            {
                if (OnFocuss.Count != 0)
                    _OnFocus(sender, e);
                if (DropDownStyle == SuggextBoxStyle.Suggest)
                    Clear();
                ShowList(sender, e);
            }
            else
                _ButtonClick(sender, e);
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
                SelectedItem = e.Item;
                SetSelectedValue(e.Key);
            }
            else
            {
                _SelectedText = string.Empty;
                SelectedItem = null;
                this.txtValue.Text = this.SelectedText;
                SetSelectedValue(EmptyValue);
            }

            if (SetValueToControl != null)
            {
                SetValueToControl.SetSelectedValue(SelectedValue);
                SetValueToControl.SetSelectedText(SelectedText);
            }

            CloseList(true);

            if (OnSelecteds.Count != 0)
                _OnSelected(e);
        }

        private void ParentForm_Move(object sender, EventArgs e)
        {
            CloseList(true);
        }

        private void SuggestBox_Disposed(object sender, EventArgs e)
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
            if (OnFocuss.Count != 0)
                _OnFocus(sender, e);
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
                if (KeyDowns.Count != 0)
                    _KeyDown(sender, e);
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
                if (txtValue.ValueChangedEnabled && txtValue.ValueChangedHandle != null)
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
                if (DoubleClicks.Count != 0)
                    _DoubleClick(sender, e);
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

        #endregion Text Control

        #endregion Events

        #region Public Functions

        private void DataBind(object sender, EventArgs e)
        {
            if (DataBindingLst.Count != 0)
            {
                _DataBinding(sender, e);
                ListControl.DataList = DataList;
            }
            else
            {
                if (txtValue.Text == string.Empty)
                    ListControl.DataList = DataList;
                else if (DropDownStyle == SuggextBoxStyle.Suggest)
                    ListControl.DataList = SearchValue(txtValue.Text);
                else
                    ListControl.DataList = DataList;
            }
            ListControl.DataBind();
        }

        public void Clear()
        {
            _SelectedText = string.Empty;
            txtValue.Text = string.Empty;
            SelectedItem = null;
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
            if (_SelectedValue != value && SelectedValueChangeds.Count != 0)
            {
                _SelectedValue = value;
                _SelectedValueChanged(this, new EventArgs());
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

        #endregion Public Functions

        #region Private Functions

        private void SetListBoxWidth()
        {
            Size MinSize = new Size(this.Width, 150);
            tsDropDown.MinimumSize = MinSize;
            ListControl.MinimumSize = MinSize;
            tsDropDown.Size = MinSize;
            ListControl.Size = MinSize;
            if (ListWidth != 0)
            {
                tsDropDown.Width = ListWidth;
                ListControl.Width = ListWidth;
            }
        }

        private void ShowList(object sender, EventArgs e)
        {
            if (ReadOnly) return;

            SetListBoxWidth();

            DataBind(sender, e);
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

        private SuggestList SearchValue(string value)
        {
            string v = value.ToUpper();
            SuggestList lst = new SuggestList();
            foreach (SuggestValue s in DataList)
            {
                if (s.PrimaryValue.ToUpper().IndexOf(v) != -1 || (SecondColumnMode && s.SecondValue.ToUpper().IndexOf(v) != -1))
                    lst.Add(s);
                if (MaxRecords != 0 && lst.Count == MaxRecords)
                    break;
            }
            return lst;
        }

        private string GetMatchText(string value)
        {
            foreach (SuggestValue s in DataList)
            {
                if (s.Key == value)
                {
                    SelectedItem = s.Item;
                    if (DisplayMember == DisplayMemberType.Primary)
                        return s.PrimaryValue;
                    else if (DisplayMember == DisplayMemberType.Second)
                        return s.SecondValue;
                }
            }
            return string.Empty;
        }

        private string GetMatchValue(string text)
        {
            foreach (SuggestValue s in DataList)
            {
                SelectedItem = s.Item;
                if (DisplayMember == DisplayMemberType.Primary && s.PrimaryValue == text)
                    return s.Key;
                else if (DisplayMember == DisplayMemberType.Second && s.SecondValue == text)
                    return s.Key;
            }
            return string.Empty;
        }

        #endregion Private Functions

        #region IValueChanged Members

        /// <summary>
        /// 获取或设置ValueChanged事件的IsChanged属性
        /// </summary>
        [DefaultValue(true), Description("获取或设置ValueChanged事件的IsChanged属性")]
        public bool ValueChangedEnabled
        {
            get
            {
                if (txtValue != null)
                    return txtValue.ValueChangedEnabled;
                else
                    return true;
            }
            set
            {
                txtValue.ValueChangedEnabled = value;
            }
        }

        #endregion IValueChanged Members
    }
}