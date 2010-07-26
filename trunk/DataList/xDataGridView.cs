using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using hwj.UserControls.Interface;

namespace hwj.UserControls.DataList
{
    public class xDataGridView : System.Windows.Forms.DataGridView
    {
        #region Property
        /// <summary>
        /// 显示行序列
        /// </summary>
        [Description("显示行序列")]
        public bool RowSeqVisible { get; set; }
        /// <summary>
        /// 显示行尾
        /// </summary>
        [Description("显示行尾")]
        public bool RowFooterVisible { get; set; }
        /// <summary>
        /// 获取或设置需要求和的列名
        /// </summary>
        [Browsable(false), Description("获取或设置需要求和的列名")]
        public List<string> SumColumnName { get; set; }
        /// <summary>
        /// 获取或设置显示行数
        /// </summary>
        [Description("获取或设置显示行数")]
        public int DisplayRows { get; set; }
        /// <summary>
        /// 设置引发hwj.UserControls.ValueChanged事件的对象
        /// </summary>
        [DefaultValue(null), Description("设置引发hwj.UserControls.ValueChanged事件的对象"), Browsable(false)]
        protected Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }

        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }

        private int PageSize = 0;
        [Browsable(false)]
        public PagingEventArgs PagingArgs { get; private set; }
        [Browsable(false)]
        public DataListPage DataListPage { get; set; }
        #endregion

        public xDataGridView()
        {
            SumColumnName = new List<string>();
            RowHeadersVisible = false;
            RowFooterVisible = false;
            DisplayRows = 0;
            BackgroundColor = SystemColors.Window;
            ValueChangedHandle = Common.ValueChanged;
            EnterEqualTab = true;
        }

        public delegate void RowFooterValueChangedHandler(DataGridViewColumn column, string value);
        public event RowFooterValueChangedHandler RowFooterValueChanged;
        public delegate void DataBindingHandler(PagingEventArgs e);
        /// <summary>
        /// 为DataGridView绑定数据
        /// </summary>
        public event DataBindingHandler DataBinding;

        #region Protected Function

        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                CreateRowSeq();
                AddRows();
            }
            CreateTotal();
            base.OnCreateControl();
        }
        protected override void OnScroll(ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                foreach (DataGridViewColumn c in this.Columns)
                {
                    if (c.Index != 0 && !c.Frozen)
                    {
                        Label l = this.Controls[LblName + c.DisplayIndex] as Label;
                        if (l != null)
                            l.Location = new System.Drawing.Point(l.Location.X - (e.NewValue - e.OldValue), l.Location.Y);
                    }
                }
            }
            base.OnScroll(e);
        }
        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            base.OnDataBindingComplete(e);
            CreateTotal();
            RefreshRowFooter();
            //if (this.Rows.Count == 0)
            //{
            //    this.DataSource = null;
            //    AddRows();
            //}
        }
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
        }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (RowSeqVisible && this.Rows[e.RowIndex].Cells[ColSeqName].Value == null)
                this.Rows[e.RowIndex].Cells[ColSeqName].Value = e.RowIndex + 1;
            base.OnRowPostPaint(e);
        }
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            RefreshRowSeq();
        }

        private bool PressEnter = false;
        //private bool isSuggestBoxCell = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (EnterEqualTab && keyData == Keys.Enter)
            {
                //isSuggestBoxCell = false;
                //if (!CurrentCell.ReadOnly && CurrentCell is xDataGridViewTextBoxCell)
                //{
                //    xDataGridViewTextBoxCell c = CurrentCell as xDataGridViewTextBoxCell;
                //    isSuggestBoxCell = c.IsSuggestBoxCell;
                //    return base.ProcessCmdKey(ref msg, keyData);
                //}

                PressEnter = true;
                SendKeys.Send("{Tab}");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        //protected override bool ProcessDialogKey(Keys keyData)
        //{
        //    if (isSuggestBoxCell && keyData == Keys.Enter)
        //        return false;
        //    else
        //        return base.ProcessDialogKey(keyData);
        //}

        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
            {
                DataGridViewCell cell = this[e.ColumnIndex, e.RowIndex];
                if (PressEnter && cell.ReadOnly)
                    SendKeys.Send("{Tab}");
                else
                    PressEnter = false;
            }
            base.OnCellEnter(e);
        }
        protected override void OnCellValidated(DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != -1 && IsSumColumn(this.Columns[e.ColumnIndex]))
                CalculateTotal(this.Columns[e.ColumnIndex]);
            base.OnCellValidated(e);
        }
        protected override void OnCellStateChanged(DataGridViewCellStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.ReadOnly)
            {
                if (e.Cell.ReadOnly)
                    e.Cell.Style.BackColor = Common.DataGridViewCellReadonlyBackColor;
                else
                    e.Cell.Style.BackColor = this.DefaultCellStyle.BackColor;
            }
            base.OnCellStateChanged(e);
        }
        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            if (this.EditingControl != null && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            base.OnCellValueChanged(e);
        }

        protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
        {
            if (!DesignMode)
            {
                if (isCreateTotal)
                    CreateTotal();
            }
            base.OnColumnWidthChanged(e);
        }
        protected override void OnColumnStateChanged(DataGridViewColumnStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.ReadOnly && e.Column.Name != ColSeqName)
            {
                if (e.Column.ReadOnly)
                    e.Column.DefaultCellStyle.BackColor = Common.DataGridViewCellReadonlyBackColor;
                else
                    e.Column.DefaultCellStyle.BackColor = this.DefaultCellStyle.BackColor;
            }
            base.OnColumnStateChanged(e);
        }

        private DataGridViewColumn oldSortColumn = null;
        protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnColumnHeaderMouseClick(e);
            if (DataBinding != null && this.Rows.Count > 0)
            {
                DataGridViewColumn col = this.Columns[e.ColumnIndex];
                PagingEventArgs pageArg = new PagingEventArgs();
                if (col.SortMode == DataGridViewColumnSortMode.Programmatic)
                {
                    if (oldSortColumn != null)
                    {
                        if (oldSortColumn.Name == col.Name && col.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                        {
                            col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                            pageArg.Sort = ListSortDirection.Descending;
                        }
                        else
                        {
                            col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                            pageArg.Sort = ListSortDirection.Ascending;
                        }
                    }
                    else
                    {
                        col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                        pageArg.Sort = ListSortDirection.Ascending;
                    }
                    oldSortColumn = col;

                    pageArg.SortName = col.DataPropertyName;
                    pageArg.PageIndex = 1;
                    pageArg.PageSize = PageSize;
                    OnDataBinding(pageArg);
                }
            }
        }
        #endregion

        #region Public Function
        /// <summary>
        /// 刷新行尾
        /// </summary>
        public void RefreshRowFooter()
        {
            if (SumColumnName != null && SumColumnName.Count > 0)
            {
                foreach (string c in SumColumnName)
                {
                    CalculateTotal(this.Columns[c]);
                }
            }
        }
        /// <summary>
        /// 获取行尾对应的列值
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetFooterValue(DataGridViewColumn column)
        {
            return this.Controls[LblName + column.DisplayIndex].Text;
        }

        public void OnDataBinding()
        {
            OnDataBinding(null);
        }
        public void OnDataBinding(int pageSize)
        {
            PageSize = pageSize;
            OnDataBinding(null);
        }
        public void OnDataBinding(PagingEventArgs e)
        {
            if (DataBinding != null)
            {
                if (e == null)
                {
                    oldSortColumn = null;
                    e = new PagingEventArgs();
                    e.PageIndex = 1;
                    e.PageSize = PageSize;
                    e.Sort = ListSortDirection.Ascending;
                    e.SortName = string.Empty;
                }
                else
                    PageSize = e.PageSize;
                PagingArgs = e;
                if (DataListPage != null)
                    DataListPage.PageIndex = e.PageIndex;
                DataBinding(e);
                if (oldSortColumn != null)
                    oldSortColumn.HeaderCell.SortGlyphDirection = e.Sort == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
            }
        }
        #endregion

        #region Private Function
        private const string ColSeqName = "__colRowSeq";
        private void CreateRowSeq()
        {
            if (RowSeqVisible && this.Columns[ColSeqName] == null)
            {
                this.RowHeadersVisible = false;
                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.SelectionBackColor = ColumnHeadersDefaultCellStyle.SelectionBackColor;
                cellStyle.BackColor = ColumnHeadersDefaultCellStyle.BackColor;
                cellStyle.SelectionForeColor = ColumnHeadersDefaultCellStyle.SelectionForeColor;
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataGridViewTextBoxColumn colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colNum.Width = 40;
                colNum.Frozen = true;
                colNum.Visible = true;
                colNum.ReadOnly = true;
                colNum.Name = ColSeqName;
                colNum.HeaderText = "";
                colNum.DefaultCellStyle = cellStyle;
                colNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                colNum.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.Columns.Insert(0, colNum);
            }
        }
        private void RefreshRowSeq()
        {
            if (!DesignMode && RowSeqVisible)
            {
                foreach (DataGridViewRow r in this.Rows)
                {
                    if (r.Index >= 0)
                        r.Cells[ColSeqName].Value = r.Index + 1;
                }
            }
        }
        private const string LblName = "lblTot_";
        private bool isCreateTotal = false;
        private void CreateTotal()
        {
            if (!RowFooterVisible || this.Rows.Count == 0) return;
            int i = 1;
            int top = this.Height - this.RowTemplate.Height;
            isCreateTotal = true;
            if (this.HorizontalScrollBar.Visible)
                top = top - this.HorizontalScrollBar.Size.Height;

            for (int x = 0; x <= this.DisplayedColumnCount(false); x++)
            {
                foreach (DataGridViewColumn c in this.Columns)
                {
                    if (c.DisplayIndex == x)
                    {
                        Label l = this.Controls[LblName + c.DisplayIndex] as Label;
                        if (l == null)
                        {
                            l = new Label();

                            l.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                            if (c.Index != 0)
                            {
                                l.BackColor = System.Drawing.SystemColors.Info;
                                if (IsSumColumn(c))
                                {
                                    l.Text = decimal.Parse("0").ToString(c.DefaultCellStyle.Format);
                                    if (c.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
                                        l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                    else if (c.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                                        l.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                }
                            }
                            else
                            {
                                l.Text = "合计";
                                l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                            }
                            l.Name = LblName + c.DisplayIndex;
                            l.BorderStyle = BorderStyle.Fixed3D;
                            l.AutoSize = false;
                            this.Controls.Add(l);
                        }
                        l.Size = this.Rows[0].Cells[c.Index].Size;
                        l.Location = new System.Drawing.Point(i, top);
                        i += c.Width;
                    }
                }
            }
        }
        private void CalculateTotal(DataGridViewColumn column)
        {
            decimal d = 0;
            string tmp = string.Empty;
            foreach (DataGridViewRow r in this.Rows)
            {
                if (r.Cells[column.Index].Value != null)
                {
                    tmp = r.Cells[column.Index].Value.ToString().Replace(",", "");
                    if (CommonLibrary.Object.NumberHelper.IsNumeric(tmp))
                        d += decimal.Parse(tmp);
                }
            }
            this.Controls[LblName + column.DisplayIndex].Text = d.ToString(column.DefaultCellStyle.Format);
            if (RowFooterValueChanged != null)
                RowFooterValueChanged(column, GetFooterValue(column));
        }
        private bool IsSumColumn(DataGridViewColumn col)
        {
            if (SumColumnName != null && SumColumnName.Count > 0)
            {
                foreach (string c in SumColumnName)
                {
                    if (c == col.Name)
                        return true;
                }
            }
            return false;
        }
        private void AddRows()
        {
            bool allowAddRows = this.AllowUserToAddRows;
            if (!allowAddRows)
                this.AllowUserToAddRows = true;
            for (int i = 1; i < DisplayRows; i++)
            {
                this.Rows.AddCopy(0);
            }
            this.AllowUserToAddRows = allowAddRows;
        }
        #endregion
    }
}
