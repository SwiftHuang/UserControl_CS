﻿using System;
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
                // Ensure that the cell used for the template is a Numberic Cell.
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
            //this.Format = Common.Format_Numberic;
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
