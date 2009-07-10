using System;
using System.Windows.Forms;
using hwj.UserControls.CommonControls;

namespace hwj.UserControls.DataList
{
    public class xDataGridViewNumbericColumn : DataGridViewColumn
    {
        public xDataGridViewNumbericColumn()
            : base(new xDataGridViewNumbericCell())
        {
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.DefaultCellStyle.Format = Common.Format_Numberic;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(xDataGridViewNumbericCell)))
                {
                    throw new InvalidCastException("Must be a NumbericCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class xDataGridViewNumbericCell : DataGridViewTextBoxCell
    {

        public xDataGridViewNumbericCell()
            : base()
        {
            this.MaxInputLength = 15;
            this.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.Style.Format = Common.Format_Numberic;
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            xDataGridViewNumbericCellEdittingControl ctl = DataGridView.EditingControl as xDataGridViewNumbericCellEdittingControl;
            if (this.Value is String)
                ctl.Text = this.Value.ToString();
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing contol that CalendarCell uses.
                return typeof(xDataGridViewNumbericCellEdittingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.
                return typeof(String);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                return String.Empty;
            }
        }
    }

    class xDataGridViewNumbericCellEdittingControl : xTextBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public xDataGridViewNumbericCellEdittingControl()
        {
            //this.Format = DateTimePickerFormat.Short;
            this.Format = Common.Format_Numberic;
            this.ContentType = ContentType.Numberic;
            this.ShowContentError = false;
            this.EnterEqualTab = false;
            this.MaxLength = 18;
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Text) && this.IsNumberic())
                {
                    decimal v = decimal.Parse(this.Text);
                    if (v == 0)
                        return string.Empty;
                    else
                        return v.ToString(this.Format);
                }
                else
                    return string.Empty;
            }
            set
            {
                if (value is String)
                {
                    this.Text = value.ToString();
                }
            }
        }

        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            //this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            //this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
        // method.
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
        // method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        // Implements the IDataGridViewEditingControl
        // .RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(e);
        }
    }

    //public class xDataGridViewNumbericCellColumn : DataGridViewColumn
    //{
    //    public xDataGridViewNumbericCellColumn()
    //        : base(new xDataGridViewNumbericCell())
    //    {

    //    }
    //    public override DataGridViewCell CellTemplate
    //    {
    //        get
    //        {
    //            return base.CellTemplate;
    //        }
    //        set
    //        {
    //            if (value != null && !value.GetType().IsAssignableFrom(typeof(xDataGridViewNumbericCell)))
    //            {
    //                throw new InvalidCastException("Must be a xDataGridViewNumbericCell");
    //            }
    //            base.CellTemplate = value;
    //        }
    //    }
    //}
    //public class xDataGridViewNumbericCell : DataGridViewCell
    //{
    //    public xDataGridViewNumbericCell()
    //        : base()
    //    {

    //    }

    //    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    //    {
    //        base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
    //        xDataGridViewNumbericCellEdittingControl ctl = DataGridView.EditingControl as xDataGridViewNumbericCellEdittingControl;
    //        ctl.Text = (string)this.Value;
    //    }
    //    public override Type EditType
    //    {
    //        get
    //        {
    //            return typeof(xDataGridViewNumbericCellEdittingControl);
    //        }
    //    }
    //    public override Type ValueType
    //    {
    //        get
    //        {
    //            return typeof(String);
    //        }
    //    }
    //    public override object DefaultNewRowValue
    //    {
    //        get
    //        {
    //            return string.Empty;
    //        }
    //    }
    //    //protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
    //    //{
    //    //    if (this.DataGridView == null)
    //    //    {
    //    //        return new Size(-1, -1);
    //    //    }

    //    //    Size preferredSize = base.GetPreferredSize(
    //    //    graphics, cellStyle, rowIndex, constraintSize);
    //    //    if (constraintSize.Width == 0)
    //    //    {
    //    //        const int ButtonsWidth = 16; // 上/下按钮的宽度。 
    //    //        const int ButtonMargin = 8;  // 文字和按钮之间的一些空白像素。
    //    //        preferredSize.Width += ButtonsWidth + ButtonMargin;
    //    //    }
    //    //    return preferredSize;
    //    //}

    //    //private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
    //    //{
    //    //    // 在编辑控件的左边和右边加1填充像素
    //    //    editingControlBounds.X += 1;
    //    //    editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);

    //    //    // 调整编辑控件的垂直位置
    //    //    int preferredHeight = cellStyle.Font.Height + 3;
    //    //    if (preferredHeight < editingControlBounds.Height)
    //    //    {
    //    //        switch (cellStyle.Alignment)
    //    //        {
    //    //            case DataGridViewContentAlignment.MiddleLeft:
    //    //            case DataGridViewContentAlignment.MiddleCenter:
    //    //            case DataGridViewContentAlignment.MiddleRight:
    //    //                editingControlBounds.Y += (editingControlBounds.Height - preferredHeight) / 2;
    //    //                break;
    //    //            case DataGridViewContentAlignment.BottomLeft:
    //    //            case DataGridViewContentAlignment.BottomCenter:
    //    //            case DataGridViewContentAlignment.BottomRight:
    //    //                editingControlBounds.Y +=
    //    //                editingControlBounds.Height - preferredHeight;
    //    //                break;
    //    //        }
    //    //    }
    //    //    return editingControlBounds;
    //    //}

    //    //public override void PositionEditingControl(bool setLocation,
    //    //    bool setSize,
    //    //    Rectangle cellBounds,
    //    //    Rectangle cellClip,
    //    //    DataGridViewCellStyle cellStyle,
    //    //    bool singleVerticalBorderAdded,
    //    //    bool singleHorizontalBorderAdded,
    //    //    bool isFirstDisplayedColumn,
    //    //    bool isFirstDisplayedRow
    //    //)
    //    //{
    //    //    Rectangle editingControlBounds = PositionEditingPanel(cellBounds,
    //    //    cellClip,
    //    //    cellStyle,
    //    //    singleVerticalBorderAdded,
    //    //    singleHorizontalBorderAdded,
    //    //    isFirstDisplayedColumn,
    //    //    isFirstDisplayedRow);
    //    //    editingControlBounds = GetAdjustedEditingControlBounds(
    //    //    editingControlBounds, cellStyle);
    //    //    this.DataGridView.EditingControl.Location = new Point(
    //    //    editingControlBounds.X, editingControlBounds.Y);
    //    //    this.DataGridView.EditingControl.Size = new Size(
    //    //    editingControlBounds.Width, editingControlBounds.Height);
    //    //}

    //    //public override void DetachEditingControl()
    //    //{
    //    //    DataGridView dataGridView = this.DataGridView;
    //    //    if (dataGridView == null || dataGridView.EditingControl == null)
    //    //    {
    //    //        throw new InvalidOperationException("Cell is detached or its grid has no editing control.");
    //    //    }

    //    //    xTextBox box = dataGridView.EditingControl as xTextBox;
    //    //    if (box != null)
    //    //    {
    //    //        // 编辑控件被回收。事实上，当一个DataGridViewNumericUpDownCell在
    //    //        // 另一个DataGridViewNumericUpDownCell之后得到编辑行为时，
    //    //        // 因为性能原因，该编辑控件将再次使用（避免不必要的控件析构和创建）。 
    //    //        // 下面，NumericUpDown控件中文本框的undo缓冲区被清除，避免编辑任务之间的干扰。
    //    //        box.ClearUndo();
    //    //    }

    //    //    base.DetachEditingControl();
    //    //}
    //}

    //class xDataGridViewNumbericCellEdittingControl : TextBox, IDataGridViewEditingControl
    //{
    //    DataGridView dataGridView;
    //    private bool valueChanged = false;
    //    int rowIndex;

    //    public xDataGridViewNumbericCellEdittingControl()
    //    {
    //        //this.Format = "###,##0.00";
    //        //this.ContentType = ContentType.Numberic;
    //        //this.ContentCheck = false;
    //        //this.BorderStyle = BorderStyle.FixedSingle;
    //    }
    //    protected override void OnTextChanged(EventArgs e)
    //    {
    //        valueChanged = true;
    //        this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
    //        base.OnTextChanged(e);
    //    }

    //    #region IDataGridViewEditingControl Members

    //    public object EditingControlFormattedValue
    //    {
    //        get { return this.Text; }
    //        set
    //        {
    //            if (value is String)
    //            {
    //                this.Text = value.ToString();
    //            }
    //        }
    //    }

    //    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
    //    {
    //        return EditingControlFormattedValue;
    //    }

    //    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    //    {
    //        //this.Font = dataGridViewCellStyle.Font;
    //        //this.ForeColor = dataGridViewCellStyle.ForeColor;
    //        //this.BackColor = dataGridViewCellStyle.BackColor;
    //    }

    //    public DataGridView EditingControlDataGridView
    //    {
    //        get { return dataGridView; }
    //        set { dataGridView = value; }
    //    }

    //    public int EditingControlRowIndex
    //    {
    //        get { return rowIndex; }
    //        set { rowIndex = value; }
    //    }

    //    public bool EditingControlValueChanged
    //    {
    //        get { return valueChanged; }
    //        set { valueChanged = value; }
    //    }

    //    public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
    //    {
    //        return !dataGridViewWantsInputKey;
    //    }

    //    public Cursor EditingPanelCursor
    //    {
    //        get { return base.Cursor; }
    //    }

    //    public void PrepareEditingControlForEdit(bool selectAll)
    //    {

    //    }

    //    public bool RepositionEditingControlOnValueChange
    //    {
    //        get { return false; }
    //    }

    //    #endregion
    //}
}
