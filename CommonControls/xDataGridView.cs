using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace hwj.UserControls.CommonControls
{
    public class xDataGridView : DataGridView
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

        protected override void InitLayout()
        {
            base.InitLayout();
        }
        protected override void OnCreateControl()
        {
            if (!DesignMode) CreateRowNum();
            CreateTotal();
            if (!DesignMode) AddRows();
            base.OnCreateControl();
        }
        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (this.Rows[e.RowIndex].Cells[ColName].Value == null)
                this.Rows[e.RowIndex].Cells[ColName].Value = e.RowIndex + 1;
            base.OnRowPostPaint(e);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            if (this[e.ColumnIndex, e.RowIndex].ReadOnly)
                SendKeys.Send("{Tab}");
            base.OnCellEnter(e);
        }
        protected override void OnScroll(ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                foreach (DataGridViewColumn c in this.Columns)
                {
                    if (c.Index != 0)
                    {
                        Label l = this.Controls[LblName + c.Index] as Label;
                        if (l != null)
                            l.Location = new System.Drawing.Point(l.Location.X - (e.NewValue - e.OldValue), l.Location.Y);
                    }
                }
            }
            base.OnScroll(e);
        }
        protected override void OnCellValidated(DataGridViewCellEventArgs e)
        {
            if (!this.CurrentCell.ReadOnly && IsSumColumn(this.Columns[e.ColumnIndex]))
            {
                decimal d = 0;
                string tmp = string.Empty;
                foreach (DataGridViewRow r in this.Rows)
                {
                    if (r.Cells[e.ColumnIndex].Value != null)
                    {
                        tmp = r.Cells[e.ColumnIndex].Value.ToString();
                        if (CommonLibrary.Utility.NumberHelper.IsNumeric(tmp))
                            d += decimal.Parse(tmp);
                    }
                }
                this.Controls[LblName + e.ColumnIndex].Text = d.ToString(this.Columns[e.ColumnIndex].DefaultCellStyle.Format);
            }
            base.OnCellValidated(e);
        }
        protected override void OnColumnStateChanged(DataGridViewColumnStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.ReadOnly && e.Column.ReadOnly && e.Column.Name != ColName)
                e.Column.DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            base.OnColumnStateChanged(e);
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
        private void CreateTotal()
        {
            if (!FooterVisible) return;
            int i = 1;
            int top = this.Height - this.RowTemplate.Height;
            if (this.HorizontalScrollBar.Visible)
                top = top - this.HorizontalScrollBar.Size.Height;


            foreach (DataGridViewColumn c in this.Columns)
            {
                Label l = new Label();
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
                l.Name = LblName + c.Index;
                l.BorderStyle = BorderStyle.Fixed3D;

                l.Height = this.RowTemplate.Height - 4;
                l.AutoSize = false;

                l.Size = this.Rows[0].Cells[c.Index].Size;
                l.Location = new System.Drawing.Point(i, top);

                this.Controls.Add(l);
                i += c.Width;
            }
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
