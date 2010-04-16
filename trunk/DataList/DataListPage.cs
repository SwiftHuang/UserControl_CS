using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace hwj.UserControls.DataList
{
    [DefaultEvent("PageIndexChangedHandler")]
    public partial class DataListPage : UserControl
    {
        #region Property
        private const string pagefmt = "/{0}";
        private bool HandlePageChange = false;
        /// <summary>
        /// 获取或设置当前页数
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (hwj.CommonLibrary.Object.NumberHelper.IsInt(toolTxtIndex.Text))
                    return int.Parse(toolTxtIndex.Text);
                else
                    return 1;
            }
            set
            {
                if (value == 0) value = 1;
                toolTxtIndex.Text = value.ToString();
                RefreshStatus();
            }
        }
        /// <summary>
        /// 获取或设置每页显示的记录数
        /// </summary>
        public int PageSize
        {
            get { return int.Parse(toolCboPageSize.Text); }
            set
            {
                value = value < 1 ? 1 : value;
                if (!toolCboPageSize.Items.Contains(value.ToString()))
                {
                    HandlePageChange = false;
                    toolCboPageSize.Items.Add(value.ToString());
                }
                toolCboPageSize.Text = value.ToString();
            }
        }
        private int _RecordCount = 0;
        /// <summary>
        /// 记录总数
        /// </summary>
        public int RecordCount
        {
            get { return _RecordCount; }
            set
            {
                _RecordCount = value;
                toolLblTotal.Text = string.Format(Properties.Resources.PageRecordCount, value);
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageNum { get; private set; }

        #region Select All(CheckBox)
        private bool _SelectAllVisible = false;
        /// <summary>
        /// 显示全选CheckBox
        /// </summary>
        [Category("Select All(CheckBox)"), Description("显示全选CheckBox"), DefaultValue(false)]
        public bool SelectAllVisible
        {
            get { return _SelectAllVisible; }
            set
            {
                _SelectAllVisible = value;
                toolChkSelectAll.Visible = value;
                toolSeparatorSelAll.Visible = value;
            }
        }
        /// <summary>
        /// 设置CheckBox列
        /// </summary>
        [Category("Select All(CheckBox)"), Description("设置CheckBox列")]
        public DataGridViewColumn CheckBoxColumn { get; set; }

        [Category("Select All(CheckBox)")]
        public bool SelectChecked
        {
            get { return toolChkSelectAll.CheckBox.Checked; }
            set
            {
                if (toolChkSelectAll != null)
                    toolChkSelectAll.CheckBox.Checked = value;
            }
        }

        [Description("当该属性有值时PageIndexChanged事件无效,请使用xDataGridViewd的DataBinding事件")]
        public xDataGridView DataGridView { get; set; }

        #endregion
        #endregion

        public delegate void PageIndexChangedHandler(PagingEventArgs e);
        public event PageIndexChangedHandler PageIndexChanged;

        public DataListPage()
        {
            this.Enabled = false;
            this.Dock = DockStyle.Bottom;
            InitializeComponent();
            SelectChecked = false;
            toolCboPageSize.SelectedItem = "100";
            SelectAllVisible = false;
            CheckBoxColumn = null;
            toolTxtIndex.Text = "1";
            RecordCount = 0;
            PageNum = 1;

        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DataGridView != null)
                DataGridView.DataListPage = this;
        }

        #region Page Changed
        private void toolBtnFirst_Click(object sender, EventArgs e)
        {
            PageIndex = 1;
            PerformPageChanged(PageIndex, PageSize);
        }

        private void toolBtnPrev_Click(object sender, EventArgs e)
        {
            PageIndex--;
            PerformPageChanged(PageIndex, PageSize);
        }

        private void toolBtnNext_Click(object sender, EventArgs e)
        {
            PageIndex++;
            PerformPageChanged(PageIndex, PageSize);
        }

        private void toolBtnLast_Click(object sender, EventArgs e)
        {
            PageIndex = PageNum;
            PerformPageChanged(PageIndex, PageSize);
        }

        private void toolTxtIndex_Validated(object sender, EventArgs e)
        {
            if (PageIndex > PageNum)
                PageIndex = PageNum;
            PerformPageChanged(PageIndex, PageSize);
        }

        private void toolCboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignMode) return;
            PageIndex = 1;
            PerformPageChanged(PageIndex, PageSize);
        }
        #endregion

        #region Public Function
        /// <summary>
        /// 执行分页事件
        /// </summary>
        /// <param name="pageIndex">页数</param>
        public void PerformPageChanged(int pageIndex)
        {
            PerformPageChanged(pageIndex, PageSize);
        }
        /// <summary>
        /// 执行分页事件
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页记录数</param>
        public void PerformPageChanged(int pageIndex, int pageSize)
        {
            PerformPageChanged(pageIndex, pageSize, null, ListSortDirection.Ascending);
        }
        /// <summary>
        /// 执行分页事件
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="sortName"></param>
        /// <param name="sort"></param>
        public void PerformPageChanged(int pageIndex, int pageSize, string sortName, ListSortDirection sort)
        {
            if (!HandlePageChange)
            {
                HandlePageChange = true;
                return;
            }
            PageIndex = pageIndex;
            PageSize = pageSize;
            if (PageIndexChanged != null || DataGridView != null)
            {
                PagingEventArgs e = new PagingEventArgs();
                e.PageIndex = pageIndex;
                e.PageSize = pageSize;
                e.SortName = sortName;
                e.Sort = sort;
                if (DataGridView != null)
                {
                    if (DataGridView.PagingArgs != null)
                    {
                        e.SortName = DataGridView.PagingArgs.SortName;
                        e.Sort = DataGridView.PagingArgs.Sort;
                    }
                    DataGridView.OnDataBinding(e);
                }
                else
                    PageIndexChanged(e);
            }
            RefreshStatus();
        }
        #endregion

        #region Private Function
        private void RefreshStatus()
        {
            if (DesignMode) return;
            this.Enabled = true;
            //toolChkSelectAll.CheckBox.Checked = Checked;
            if (PageSize != 0)
                PageNum = RecordCount / PageSize + (RecordCount % PageSize > 0 ? 1 : 0);
            if (PageNum == 0 || RecordCount == 0)
                PageNum = 1;
            toolLblPage.Text = string.Format(pagefmt, PageNum);
            if (PageIndex <= 1)
            {
                toolBtnFirst.Enabled = false;
                toolBtnPrev.Enabled = false;
            }
            else
            {
                toolBtnFirst.Enabled = true;
                toolBtnPrev.Enabled = true;
            }
            if (PageIndex >= PageNum)
            {
                toolBtnNext.Enabled = false;
                toolBtnLast.Enabled = false;
            }
            else
            {
                toolBtnNext.Enabled = true;
                toolBtnLast.Enabled = true;
            }
            toolChkSelectAll_CheckedChanged(null, null);
        }
        private void toolTxtIndex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)))
                e.Handled = true;
        }
        private void toolChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectAllVisible && toolChkSelectAll != null && CheckBoxColumn != null && CheckBoxColumn.DataGridView != null && CheckBoxColumn.DataGridView.Rows.Count > 0)
            {
                foreach (DataGridViewRow r in CheckBoxColumn.DataGridView.Rows)
                {
                    if (!r.Cells[CheckBoxColumn.Name].ReadOnly)
                        r.Cells[CheckBoxColumn.Name].Value = toolChkSelectAll.CheckBox.Checked;
                }
            }
        }
        #endregion

    }
}
