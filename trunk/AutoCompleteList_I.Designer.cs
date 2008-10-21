namespace Vinson.UserControls
{
    partial class AutoCompleteList_I
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblFootInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTimeInfo = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgList)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgList
            // 
            this.dgList.AllowUserToAddRows = false;
            this.dgList.AllowUserToDeleteRows = false;
            this.dgList.AllowUserToResizeColumns = false;
            this.dgList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            this.dgList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgList.Location = new System.Drawing.Point(0, 0);
            this.dgList.MultiSelect = false;
            this.dgList.Name = "dgList";
            this.dgList.ReadOnly = true;
            this.dgList.RowHeadersVisible = false;
            this.dgList.RowHeadersWidth = 20;
            this.dgList.RowTemplate.Height = 21;
            this.dgList.RowTemplate.ReadOnly = true;
            this.dgList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgList.Size = new System.Drawing.Size(211, 184);
            this.dgList.TabIndex = 0;
            this.dgList.TabStop = false;
            this.dgList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgList_CellClick);
            this.dgList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgList_KeyDown);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFootInfo,
            this.toolStripStatusLabel2,
            this.lblTimeInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 182);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(211, 20);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblFootInfo
            // 
            this.lblFootInfo.Name = "lblFootInfo";
            this.lblFootInfo.Size = new System.Drawing.Size(59, 15);
            this.lblFootInfo.Text = "lblFootInfo";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(78, 15);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // lblTimeInfo
            // 
            this.lblTimeInfo.Name = "lblTimeInfo";
            this.lblTimeInfo.Size = new System.Drawing.Size(59, 15);
            this.lblTimeInfo.Text = "lblTimeInfo";
            // 
            // AutoCompleteList_I
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dgList);
            this.Name = "AutoCompleteList_I";
            this.Size = new System.Drawing.Size(211, 202);
            ((System.ComponentModel.ISupportInitialize)(this.dgList)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.ToolStripStatusLabel lblFootInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblTimeInfo;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}
