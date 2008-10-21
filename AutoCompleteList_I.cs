using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Vinson.UserControls
{
    public partial class AutoCompleteList_I : UserControl
    {
        private bool _isAlreadyCreateColumns = false;
        public delegate void SelectedValueHandler(AutoCompleteValue e);
        public event SelectedValueHandler SelectedValue;

        public AutoCompleteList_I()
        {
            InitializeComponent();
            dgList.AutoGenerateColumns = false;
        }

        #region Property
        private List<AutoCompleteValue> _dataList = new List<AutoCompleteValue>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]//Hidden = 0
        public List<AutoCompleteValue> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }
        private bool _secondColumn = false;
        public bool SecondColumnMode
        {
            get { return _secondColumn; }
            set { _secondColumn = value; }
        }
        private string _priColumnName = string.Empty;
        public string PrimaryColumnName
        {
            get { return _priColumnName; }
            set { _priColumnName = value; }
        }
        private string _secColumnName = string.Empty;
        public string SecondColumnName
        {
            get { return _secColumnName; }
            set { _secColumnName = value; }
        }
        public string FootText
        {
            get { return this.lblFootInfo.Text; }
            set { lblFootInfo.Text = value; }
        }
        public string FootTimeText
        {
            get { return lblTimeInfo.Text; }
            set { lblTimeInfo.Text = value; }
        }
        #endregion

        #region Public Functions
        public void DataBind()
        {
            CreateColumns();
            dgList.DataSource = _dataList;
        }
        public void SetListFocus()
        { dgList.Focus(); }
        #endregion

        #region Private Functions
        private void CreateColumns()
        {
            if (!_isAlreadyCreateColumns)
            {
                _isAlreadyCreateColumns = true;

                if (PrimaryColumnName == string.Empty && SecondColumnName == string.Empty)
                    dgList.ColumnHeadersVisible = false;
                else
                    dgList.ColumnHeadersVisible = true;

                dgList.Columns.Clear();
                DataGridViewTextBoxColumn colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colValue.DataPropertyName = "Key";
                colValue.Frozen = true;
                colValue.HeaderText = "Key";
                colValue.Name = "colValue";
                colValue.Visible = false;
                colValue.ReadOnly = true;
                dgList.Columns.Add(colValue);

                DataGridViewTextBoxColumn colPri = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colPri.DataPropertyName = "PrimaryValue";
                colPri.HeaderText = PrimaryColumnName;
                colPri.Name = "colPri";
                colPri.ReadOnly = true;
                if (SecondColumnMode)
                {
                    colPri.Frozen = true;
                    colPri.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    colPri.Frozen = false;
                    colPri.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                dgList.Columns.Add(colPri);

                if (SecondColumnMode)
                {
                    DataGridViewTextBoxColumn colSec = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    colSec.DataPropertyName = "SecondValue";
                    colSec.Frozen = false;
                    colSec.HeaderText = SecondColumnName;
                    colSec.Name = "colSec";
                    colSec.ReadOnly = true;
                    colSec.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgList.Columns.Add(colSec);
                }
            }
        }
        private void SetSelectedValue(int row)
        {
            if (row == -1)
                return;
            AutoCompleteValue v = new AutoCompleteValue();
            v.Key = dgList.Rows[row].Cells["colValue"].Value.ToString();
            v.PrimaryValue = dgList.Rows[row].Cells["colPri"].Value.ToString();
            if (SecondColumnMode)
                v.SecondValue = dgList.Rows[row].Cells["colSec"].Value.ToString();

            if (SelectedValue != null)
                SelectedValue(v);
        }
        #endregion

        #region Events
        private void dgList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetSelectedValue(e.RowIndex);
        }
        private void dgList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dgList.CurrentRow != null)
                    SetSelectedValue(dgList.CurrentRow.Index);
            }
        }
        #endregion
    }
}
