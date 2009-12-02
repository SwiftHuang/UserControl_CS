using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace hwj.UserControls.Suggest
{
    internal partial class SuggestView : UserControl
    {
        private bool _isAlreadyCreateColumns = false;
        public delegate void SelectedValueHandler(SuggestValue e);
        public event SelectedValueHandler SelectedValue;

        public SuggestView()
        {
            InitializeComponent();
            dgList.AutoGenerateColumns = false;
            SecondColumnMode = false;
        }

        #region Property
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]//Hidden = 0
        public bool IsEnterList = false;
        public SuggestList DataList { get; set; }
        private bool _SecondColumnMode = false;
        public bool SecondColumnMode
        {
            get { return _SecondColumnMode; }
            set
            {
                if (_SecondColumnMode != value)
                {
                    _isAlreadyCreateColumns = false;
                    DataList = new SuggestList();
                }
                _SecondColumnMode = value;
            }
        }
        public string PrimaryColumnName { get; set; }
        public string SecondColumnName { get; set; }
        #endregion

        #region Public Functions
        public void DataBind()
        {
            CreateColumns();
            dgList.DataSource = null;
            dgList.DataSource = DataList;
        }
        public void SelectIndex(int index)
        {
            if (index >= dgList.Rows.Count)
                index--;
            dgList.Rows[index].Selected = true;
            dgList.CurrentCell = dgList.Rows[index].Cells[1];
        }
        #endregion

        #region Private Functions
        private void CreateColumns()
        {
            if (!_isAlreadyCreateColumns)
            {
                _isAlreadyCreateColumns = true;

                if (string.IsNullOrEmpty(PrimaryColumnName) && string.IsNullOrEmpty(SecondColumnName))
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
                dgList.Columns[0].Visible = false;

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
        public void SetSelectedValue(int rowIndex)
        {
            if (rowIndex == -1)
                return;
            SuggestValue v = new SuggestValue();
            v.Key = dgList.Rows[rowIndex].Cells["colValue"].Value.ToString();
            v.PrimaryValue = dgList.Rows[rowIndex].Cells["colPri"].Value.ToString();
            if (SecondColumnMode && dgList.Columns["colSec"] != null)
                v.SecondValue = dgList.Rows[rowIndex].Cells["colSec"].Value.ToString();

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
        void dgList_MouseLeave(object sender, EventArgs e)
        {
            IsEnterList = false;
        }
        void dgList_MouseEnter(object sender, EventArgs e)
        {
            IsEnterList = true;
        }
        private void dgList_MouseMove(object sender, MouseEventArgs e)
        {
            IsEnterList = true;
        }
        #endregion


    }
}
