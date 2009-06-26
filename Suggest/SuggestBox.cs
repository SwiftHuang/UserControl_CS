using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace hwj.UserControls.Suggest
{
    public partial class SuggestBox : UserControl
    {
        private int selectIndex = 0;
        private bool textChange = false;
        private bool IsShowed = false;
        private ToolStripDropDown tsDropDown = null;
        private ToolStripControlHost tsCH = null;
        private SuggestView ListControl = null;
        private ToolTip toolTip = new ToolTip();

        public delegate void SelectedValueHandler(SuggestValue e);
        public event SelectedValueHandler OnSelected;
        public event EventHandler OnFocus;
        public event EventHandler DataBinding;

        #region Property
        public bool EnterEqualTab { get; set; }
        public bool IsRequired { get; set; }
        public Color BackColor
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
        public int RecordCount
        {
            get
            {
                if (ListControl != null && ListControl.DataList != null)
                    return ListControl.DataList.Count;
                else
                    return 0;
            }
        }
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

        public SuggestBox()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            InitializeComponent();
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
                tsDropDown.Closed += new ToolStripDropDownClosedEventHandler(tsDropDown_Closed);
                tsDropDown.Padding = new Padding(0);
                tsDropDown.DropShadowEnabled = true;
                tsDropDown.Items.Add(tsCH);
                tsDropDown.AutoClose = false;
            }
        }
        private void SuggestBox_Load(object sender, EventArgs e)
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
                DataBind(sender, e);
            ShowList();
        }
        private void lstCtrl_SelectedValue(SuggestValue e)
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
            CloseList(true);

            if (OnSelected != null)
                OnSelected(e);
        }
        void tsDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            IsShowed = false;
        }
        void ParentForm_Move(object sender, EventArgs e)
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
                    if (!textChange && IsShowed)
                    {
                        ListControl.SetSelectedValue(selectIndex);
                        selectIndex = 0;
                        if (EnterEqualTab)
                            SendKeys.Send("{Tab}");
                        return;
                    }
                    DataBind(sender, e);
                    textChange = false;
                    ShowList();
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (selectIndex > 0)
                        selectIndex--;
                    ListControl.SelectIndex(selectIndex);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (selectIndex < RecordCount - 1)
                        selectIndex++;
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
            textChange = true;
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
                else
                    ListControl.DataList = SearchPrimaryColumn(txtValue.Text);
            }
            ListControl.DataBind();
            SetFootText();
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
            selectIndex = 0;
            if (RecordCount > 0)
            {
                if (tsDropDown != null)
                {
                    if (this.ParentForm != null)
                        this.ParentForm.Move += new EventHandler(ParentForm_Move);
                    IsShowed = true;
                    ListControl.IsEnterList = false;
                    ListControl.Width = tsDropDown.Width;
                    tsDropDown.Show(this, 0, this.Height);
                    Focus();
                }
            }
            else
            {
                CloseList(true);
                Common.ShowToolTipInfo(toolTip, this, Properties.Resources.NoRecord);
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
        }
        private SuggestList SearchPrimaryColumn(string value)
        {
            SuggestList lst = new SuggestList();
            foreach (SuggestValue s in DataList)
            {
                if (s.Key.IndexOf(value) != -1)
                    lst.Add(s);
            }
            return lst;
        }
        private void SetFootText()
        {
            ListControl.FootInfoText = string.Format(Properties.Resources.ReturnRecord, RecordCount);
        }
        #endregion
    }
}
