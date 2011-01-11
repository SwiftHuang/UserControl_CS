using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace hwj.UserControls.Suggest.View
{
    internal partial class SuggestView : UserControl
    {
        public static readonly string colKeyName = "_colKeyName";
        public static readonly string colFirstName = "_colFirstName";
        public static readonly string colSecondName = "_colSecondName";
        private bool _isAlreadyCreateColumns = false;
        public delegate void SelectedValueHandler(SelectedItem e);
        public event SelectedValueHandler SelectedValue;

        public SuggestView()
        {
            InitializeComponent();
            dgList.AutoGenerateColumns = false;
            SecondColumnMode = false;
        }

        #region Property
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]//Hidden = 0
        internal bool IsEnterList = false;
        public DataView DataList { get; set; }
        private bool _SecondColumnMode = false;
        public bool SecondColumnMode
        {
            get { return _SecondColumnMode; }
            set
            {
                if (_SecondColumnMode != value)
                {
                    _isAlreadyCreateColumns = false;
                    DataList = new DataView();
                }
                _SecondColumnMode = value;
            }
        }

        public string FirstColumnName { get; set; }
        public string SecondColumnName { get; set; }

        public string KeyColumnDataPropertyName { get; set; }
        public string FirstColumnDataPropertyName { get; set; }
        public string SecondColumnDataPropertyName { get; set; }

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

                if (string.IsNullOrEmpty(FirstColumnName) && string.IsNullOrEmpty(SecondColumnName))
                    dgList.ColumnHeadersVisible = false;
                else
                    dgList.ColumnHeadersVisible = true;

                dgList.Columns.Clear();
                DataGridViewTextBoxColumn colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colValue.DataPropertyName = KeyColumnDataPropertyName;
                colValue.Frozen = true;
                colValue.HeaderText = "Value";
                colValue.Name = colKeyName;
                colValue.Visible = false;
                colValue.ReadOnly = true;
                dgList.Columns.Add(colValue);
                dgList.Columns[0].Visible = false;

                DataGridViewTextBoxColumn colPri = new System.Windows.Forms.DataGridViewTextBoxColumn();
                colPri.DataPropertyName = FirstColumnDataPropertyName;
                colPri.HeaderText = FirstColumnName;
                colPri.Name = colFirstName;
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
                    colSec.DataPropertyName = SecondColumnDataPropertyName;
                    colSec.Frozen = false;
                    colSec.HeaderText = SecondColumnName;
                    colSec.Name = colSecondName;
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

            DataGridViewRow r = dgList.Rows[rowIndex];
            if (r != null)
            {
                SelectedItem e = new SelectedItem();
                e.SelectedIndex = rowIndex;
                e.Item = DataList[e.SelectedIndex];

                e.Key = r.Cells[colKeyName].Value.ToString();
                e.FirstColumnValue = r.Cells[colFirstName].Value.ToString();
                if (SecondColumnMode && dgList.Columns[colSecondName] != null)
                    e.SecondColumnValue = r.Cells[colSecondName].Value.ToString();
                if (SelectedValue != null)
                    SelectedValue(e);
            }
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
