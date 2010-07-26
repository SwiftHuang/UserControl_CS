using System;
using System.ComponentModel;
using System.Windows.Forms;
using hwj.UserControls.CommonControls;

namespace hwj.UserControls.DataList
{
    public class xDataGridViewTextBoxColumn : DataGridViewColumn
    {
        public int MaxInputLength { get; set; }
        internal ContentType ColumnContentType { get; set; }

        public xDataGridViewTextBoxColumn()
            : base(new xDataGridViewTextBoxCell())
        {
            MaxInputLength = 32767;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a Numberic Cell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(xDataGridViewTextBoxCell)))
                {
                    throw new InvalidCastException("Must be a xDataGridViewTextBoxCell");
                }
                base.CellTemplate = value;
            }
        }
        public override object Clone()
        {
            xDataGridViewTextBoxColumn col = (xDataGridViewTextBoxColumn)base.Clone();
            col.MaxInputLength = this.MaxInputLength;
            return col;
        }
    }

    public class xDataGridViewTextBoxCell : DataGridViewTextBoxCell
    {
        public ContentType ContentType { get; set; }
        private string Format = string.Empty;
        private DataGridViewContentAlignment Alignment = DataGridViewContentAlignment.NotSet;

        public xDataGridViewTextBoxCell()
            : base()
        {
            ContentType = ContentType.None;
            //Format = string.Empty;
            //Alignment = DataGridViewContentAlignment.NotSet;
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);

            xDataGridViewTextBoxCellEdittingControl ctl = DataGridView.EditingControl as xDataGridViewTextBoxCellEdittingControl;

            if (ctl != null)
            {
                if (this.DataGridView != null && this.ColumnIndex >= 0)
                {
                    xDataGridViewTextBoxColumn col = DataGridView.Columns[ColumnIndex] as xDataGridViewTextBoxColumn;
                    ctl.MaxLength = col.MaxInputLength;
                    if (ContentType != ContentType.None)
                    {
                        ctl.ContentType = ContentType;
                        Format = this.Style.Format;
                        Alignment = this.Style.Alignment;
                    }
                    else
                    {
                        ctl.ContentType = col.ColumnContentType;
                        Format = col.DefaultCellStyle.Format;
                        Alignment = col.DefaultCellStyle.Alignment;
                    }
                }

                if (ctl.ContentType == ContentType.Integer || ctl.ContentType == ContentType.Numberic)
                {
                    if (string.IsNullOrEmpty(Format))
                    {
                        ctl.Format = Common.Format_Numberic;
                        this.Style.Format = ctl.Format;
                    }
                    else
                        ctl.Format = Format;


                    if (Alignment == DataGridViewContentAlignment.NotSet)
                    {
                        this.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    else if (Alignment == DataGridViewContentAlignment.BottomCenter || Alignment == DataGridViewContentAlignment.MiddleCenter || Alignment == DataGridViewContentAlignment.TopCenter)
                        ctl.TextAlign = HorizontalAlignment.Center;
                    else if (Alignment == DataGridViewContentAlignment.BottomLeft || Alignment == DataGridViewContentAlignment.MiddleLeft || Alignment == DataGridViewContentAlignment.TopLeft)
                        ctl.TextAlign = HorizontalAlignment.Left;
                    else
                        ctl.TextAlign = HorizontalAlignment.Right;
                }

                if (this.Value is String)
                    ctl.Text = this.Value.ToString();
            }


        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing contol that CalendarCell uses.
                return typeof(xDataGridViewTextBoxCellEdittingControl);
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

    class xDataGridViewTextBoxCellEdittingControl : xTextBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public xDataGridViewTextBoxCellEdittingControl()
        {
            //this.Format = DateTimePickerFormat.Short;
            //this.Format = Common.Format_Numberic;
            //this.ContentType = ContentType.Numberic;
            this.ShowContentError = false;
            this.EnterEqualTab = false;
            //this.MaxLength = 18;
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get
            {
                if (this.ContentType == ContentType.Integer || this.ContentType == ContentType.Numberic)
                {
                    decimal v = 0;
                    if (!string.IsNullOrEmpty(this.Text) && decimal.TryParse(this.Text, out v))
                    {
                        if (v == 0)
                            return string.Empty;
                        else
                            return v.ToString(this.Format);
                    }
                    else
                        return string.Empty;
                }
                else
                {
                    return this.Text;
                }
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
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public int EditingControlRowIndex
        {
            get { return rowIndex; }
            set { rowIndex = value; }
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
            get { return false; }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get { return dataGridView; }
            set { dataGridView = value; }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public bool EditingControlValueChanged
        {
            get { return valueChanged; }
            set { valueChanged = value; }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
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

}
