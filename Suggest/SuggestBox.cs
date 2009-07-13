using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace hwj.UserControls.Suggest
{
    public partial class SuggestBox : UserControl, CommonControls.ICommonControls
    {
        private System.Drawing.Color oldBackColor;
        private int selectIndex = 0;
        private bool textChange = false;
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

        #region Event Object
        public delegate void SelectedValueHandler(SuggestValue e);
        public event SelectedValueHandler OnSelected;
        public event EventHandler SelectedValueChnaged;
        public event EventHandler OnFocus;
        public event EventHandler DataBinding;
        public event EventHandler DoubleClick;
        #endregion

        #region Property
        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }
        [DefaultValue(false)]
        public bool IsRequired { get; set; }
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
                if (string.IsNullOrEmpty(this.txtValue.Text))
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
                _SelectedText = value;
                txtValue.Text = value;
            }
        }
        public override string Text
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }
        [DefaultValue(true), Browsable(true)]
        public bool ButtonVisible { get; set; }
        private string _emptyValue = string.Empty;
        /// <summary>
        /// ���TextBox.TextΪ��ʱ,SelectedValue��ֵ.
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
        /// ��������¼��
        /// </summary>
        [Description("��������¼��"), DefaultValue(0)]
        public int MaxRecords { get; set; }

        public enum SuggextBoxStyle
        {
            DropDownList,
            Suggest,
        }
        /// <summary>
        /// ��ȡ������SuggestBox Dropdown����ʾ��ʽ
        /// </summary>
        [DefaultValue(SuggextBoxStyle.Suggest), Description("��ȡ������SuggestBox Dropdown����ʾ��ʽ")]
        public SuggextBoxStyle DropDownStyle { get; set; }

        #endregion

        public SuggestBox()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            InitializeComponent();
            DropDownStyle = SuggextBoxStyle.Suggest;
            ButtonVisible = true;
            txtValue.EnterEqualTab = false;
            oldBackColor = this.txtValue.BackColor;

            IsRequired = false;
            EnterEqualTab = true;

            if (!DesignMode)
            {
                DataList = new SuggestList();
                Size MinSize = new Size(100, 150);

                txtValue.LostFocus += new EventHandler(txtValue_LostFocus);

                ListControl = new SuggestView();
                ListControl.MinimumSize = MinSize;
                ListControl.SelectedValue += new SuggestView.SelectedValueHandler(lstCtrl_SelectedValue);

                tsCH = new ToolStripControlHost(ListControl);
                tsCH.Padding = new Padding(0);
                tsCH.Dock = DockStyle.Fill;

                tsDropDown = new ToolStripDropDown();
                tsDropDown.Padding = new Padding(0);
                tsDropDown.DropShadowEnabled = true;
                tsDropDown.Items.Add(tsCH);
                tsDropDown.AutoClose = false;
            }
        }
        protected override void OnCreateControl()
        {
            tsDropDown.Width = this.Width;
            btnSelect.Visible = ButtonVisible;
            txtValue.MaxLength = MaxLength;
            this.txtValue.BackColor = SystemColors.Window;

            if (DropDownStyle == SuggextBoxStyle.DropDownList)
            {
                txtValue.ReadOnly = true;
                //txtValue.Enabled = false;
            }
            else
            {
                txtValue.ReadOnly = false;
                if (IsRequired)
                    this.txtValue.BackColor = Common.RequiredBackColor;
            }
            base.OnCreateControl();
        }

        #region Events
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (OnFocus != null)
                OnFocus(sender, e);
            ShowList(sender, e);
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

            CloseList(true);

            if (OnSelected != null)
                OnSelected(e);
        }
        private void ParentForm_Move(object sender, EventArgs e)
        {
            CloseList(true);
        }
        #region Text Control
        private void txtValue_Enter(object sender, EventArgs e)
        {
            if (OnFocus != null)
                OnFocus(sender, e);
        }
        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
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
            CloseList(false);
        }
        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.AppStarting;
                VerifyInfo.ValueIsChanged = true;
                textChange = true;
                ShowList(sender, e);
            }
            catch (Exception ex)
            {
                Common.ShowToolTipError(this, ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
        }
        private void txtValue_Validating(object sender, CancelEventArgs e)
        {
            if (_SelectedText != this.txtValue.Text)
                Clear();
            if (IsRequired)
            {
                if (string.IsNullOrEmpty(this.Text))
                    this.BackColor = Common.RequiredBackColor;
                else
                    this.BackColor = oldBackColor;
            }
        }
        private void txtValue_DoubleClick(object sender, EventArgs e)
        {
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
            if (DropDownStyle == SuggextBoxStyle.DropDownList)
                ShowList(sender, e);
        }
        #endregion
        #endregion

        #region Public Functions
        private void DataBind(object sender, EventArgs e)
        {
            if (DataBinding != null)
            {
                DataBinding(sender, e);
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
            //SetFootText();
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
        #endregion

        #region Private Functions
        private void ShowList(object sender, EventArgs e)
        {
            DataBind(sender, e);
            textChange = false;
            selectIndex = 0;
            if (RecordCount > 0)
            {
                if (tsDropDown != null)
                {
                    if (this.ParentForm != null)
                        this.ParentForm.Move += new EventHandler(ParentForm_Move);
                    ListControl.IsEnterList = false;
                    ListControl.Width = tsDropDown.Width;
                    tsDropDown.Show(this, 0, this.Height);
                    Focus();
                }
            }
            else
            {
                CloseList(true);
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
                if (DisplayMember == DisplayMemberType.Primary && s.PrimaryValue == text)
                    return s.Key;
                else if (DisplayMember == DisplayMemberType.Second && s.SecondValue == text)
                    return s.Key;
            }
            return string.Empty;
        }
        private void SetSelectedValue(string value)
        {
            if (_SelectedValue != value && SelectedValueChnaged != null)
                SelectedValueChnaged(this, new EventArgs());
            _SelectedValue = value;
        }
        //private void SetFootText()
        //{
        //    ListControl.FootInfoText = string.Format(Properties.Resources.ReturnRecord, RecordCount);
        //}
        #endregion

    }
}
