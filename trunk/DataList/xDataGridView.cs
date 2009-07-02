using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace hwj.UserControls.DataList
{
    public class xDataGridView : System.Windows.Forms.DataGridView
    {
        #region Property
        public bool RowNum { get; set; }
        public bool FooterVisible { get; set; }
        public List<string> SumColumnName { get; set; }
        public int DisplayRows { get; set; }
        #endregion

        public xDataGridView()
        {
            SumColumnName = new List<string>();
            RowHeadersVisible = false;
            FooterVisible = false;
            DisplayRows = 0;
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
        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                CreateRowNum();
                AddRows();
            }
            CreateTotal();
            base.OnCreateControl();
        }
        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (RowNum && this.Rows[e.RowIndex].Cells[ColName].Value == null)
                this.Rows[e.RowIndex].Cells[ColName].Value = e.RowIndex + 1;
            base.OnRowPostPaint(e);
        }
        private bool PressEnter = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && !CurrentCell.ReadOnly)
            {
                PressEnter = true;
                this.ProcessTabKey(keyData);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = this[e.ColumnIndex, e.RowIndex];
            if (!cell.ReadOnly)
                PressEnter = false;
            if (PressEnter && cell.ReadOnly)
                SendKeys.Send("{Tab}");
            base.OnCellEnter(e);
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
        protected override void OnCellValidated(DataGridViewCellEventArgs e)
        {
            if (IsSumColumn(this.Columns[e.ColumnIndex]))
                CalculateTotal(this.Columns[e.ColumnIndex]);
            base.OnCellValidated(e);
        }
        protected override void OnColumnStateChanged(DataGridViewColumnStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.ReadOnly && e.Column.ReadOnly && e.Column.Name != ColName)
                e.Column.DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            base.OnColumnStateChanged(e);
        }
        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            base.OnDataBindingComplete(e);
            CreateTotal();
            foreach (string c in SumColumnName)
            {
                CalculateTotal(this.Columns[c]);
            }
        }
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
        }

        private const string ColName = "_colRowNum";
        private void CreateRowNum()
        {
            if (RowNum && this.Columns[ColName] == null)
            {
                this.RowHeadersVisible = false;
                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.SelectionBackColor = System.Drawing.SystemColors.Control;
                cellStyle.BackColor = System.Drawing.SystemColors.Control;
                cellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataGridViewTextBoxColumn colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colNum.Width = 40;
                colNum.Frozen = true;
                colNum.Visible = true;
                colNum.ReadOnly = true;
                colNum.Name = ColName;
                colNum.HeaderText = "";
                colNum.DefaultCellStyle = cellStyle;
                colNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                this.Columns.Insert(0, colNum);
            }
        }
        private const string LblName = "lblTot_";
        private bool isCreateTotal = false;
        private void CreateTotal()
        {
            if (!FooterVisible) return;
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
        private void CalculateTotal(DataGridViewColumn cloumn)
        {
            decimal d = 0;
            string tmp = string.Empty;
            foreach (DataGridViewRow r in this.Rows)
            {
                if (r.Cells[cloumn.Index].Value != null)
                {
                    tmp = r.Cells[cloumn.Index].Value.ToString().Replace(",", "");
                    if (CommonLibrary.Utility.NumberHelper.IsNumeric(tmp))
                        d += decimal.Parse(tmp);
                }
            }
            this.Controls[LblName + cloumn.DisplayIndex].Text = d.ToString(cloumn.DefaultCellStyle.Format);
        }
        private bool IsSumColumn(DataGridViewColumn col)
        {
            foreach (string s in SumColumnName)
            {
                if (s == col.Name)
                    return true;
            }
            return false;
        }
        private void AddRows()
        {
            for (int i = 1; i < DisplayRows; i++)
            {
                this.Rows.AddCopy(0);
            }
        }
    }
}
