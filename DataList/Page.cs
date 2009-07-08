using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hwj.UserControls.DataList
{
    public partial class Page : UserControl
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
        #endregion

        public delegate void PageIndexChangedHandler(int pageIndex, int pageSize);
        public event PageIndexChangedHandler PageIndexChanged;

        public Page()
        {
            InitializeComponent();
            toolCboPageSize.SelectedItem = "500";
            toolTxtIndex.Text = "1";
            RecordCount = 0;
            PageNum = 1;
        }

        #region Page Change
        private void toolBtnFirst_Click(object sender, EventArgs e)
        {
            PageIndex = 1;
            PerformPageChange(PageIndex, PageSize);
        }

        private void toolBtnPrev_Click(object sender, EventArgs e)
        {
            PageIndex--;
            PerformPageChange(PageIndex, PageSize);
        }

        private void toolBtnNext_Click(object sender, EventArgs e)
        {
            PageIndex++;
            PerformPageChange(PageIndex, PageSize);
        }

        private void toolBtnLast_Click(object sender, EventArgs e)
        {
            PageIndex = PageNum;
            PerformPageChange(PageIndex, PageSize);
        }

        private void toolTxtIndex_Validated(object sender, EventArgs e)
        {
            if (PageIndex > PageNum)
                PageIndex = PageNum;
            PerformPageChange(PageIndex, PageSize);
        }

        private void toolCboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageIndex = 1;
            PerformPageChange(PageIndex, PageSize);
        }
        #endregion

        public void PerformPageChange(int pageIndex, int pageSize)
        {
            if (!HandlePageChange)
            {
                HandlePageChange = true;
                return;
            }
            PageIndex = pageIndex;
            PageSize = pageSize;
            if (PageIndexChanged != null)
                PageIndexChanged(PageIndex, PageSize);
            RefreshStatus();
        }
        private void RefreshStatus()
        {
            if (PageSize != 0)
                PageNum = RecordCount / PageSize + (RecordCount % PageSize > 0 ? 1 : 0);
            if (PageNum == 0)
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
        }

        private void toolTxtIndex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)))
                e.Handled = true;
        }
    }
}
