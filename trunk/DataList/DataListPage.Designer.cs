namespace hwj.UserControls.DataList
{
    partial class DataListPage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataListPage));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolChkSelectAll = new hwj.UserControls.ToolStrip.ToolStripCheckedBox();
            this.toolSeparatorSelAll = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnFirst = new System.Windows.Forms.ToolStripButton();
            this.toolLblTotal = new System.Windows.Forms.ToolStripLabel();
            this.toolBtnPrev = new System.Windows.Forms.ToolStripButton();
            this.toolTxtIndex = new System.Windows.Forms.ToolStripTextBox();
            this.toolLblPage = new System.Windows.Forms.ToolStripLabel();
            this.toolBtnNext = new System.Windows.Forms.ToolStripButton();
            this.toolBtnLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolCboPageSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3,
            this.toolChkSelectAll,
            this.toolSeparatorSelAll,
            this.toolBtnFirst,
            this.toolLblTotal,
            this.toolBtnPrev,
            this.toolTxtIndex,
            this.toolLblPage,
            this.toolBtnNext,
            this.toolBtnLast,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.toolCboPageSize,
            this.toolStripLabel2,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(629, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolChkSelectAll
            // 
            this.toolChkSelectAll.BackColor = System.Drawing.Color.Transparent;
            // 
            // toolChkSelectAll
            // 
            this.toolChkSelectAll.CheckBox.AccessibleName = "toolChkSelectAll";
            this.toolChkSelectAll.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this.toolChkSelectAll.CheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolChkSelectAll.CheckBox.Location = new System.Drawing.Point(6, 1);
            this.toolChkSelectAll.CheckBox.Name = "toolStripCheckedBox1";
            this.toolChkSelectAll.CheckBox.Size = new System.Drawing.Size(72, 22);
            this.toolChkSelectAll.CheckBox.TabIndex = 2;
            this.toolChkSelectAll.CheckBox.Text = "全选(页)";
            this.toolChkSelectAll.CheckBox.UseVisualStyleBackColor = false;
            this.toolChkSelectAll.Name = "toolChkSelectAll";
            this.toolChkSelectAll.Size = new System.Drawing.Size(72, 22);
            this.toolChkSelectAll.Text = "全选(页)";
            this.toolChkSelectAll.ToolTipText = "全选当前页";
            this.toolChkSelectAll.CheckedChanged += new System.EventHandler(this.toolChkSelectAll_CheckedChanged);
            // 
            // toolSeparatorSelAll
            // 
            this.toolSeparatorSelAll.Name = "toolSeparatorSelAll";
            this.toolSeparatorSelAll.Size = new System.Drawing.Size(6, 25);
            // 
            // toolBtnFirst
            // 
            this.toolBtnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnFirst.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnFirst.Image")));
            this.toolBtnFirst.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolBtnFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnFirst.Name = "toolBtnFirst";
            this.toolBtnFirst.Size = new System.Drawing.Size(23, 22);
            this.toolBtnFirst.Text = "最前";
            this.toolBtnFirst.Click += new System.EventHandler(this.toolBtnFirst_Click);
            // 
            // toolLblTotal
            // 
            this.toolLblTotal.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolLblTotal.Name = "toolLblTotal";
            this.toolLblTotal.Size = new System.Drawing.Size(77, 22);
            this.toolLblTotal.Text = "共1000条记录";
            // 
            // toolBtnPrev
            // 
            this.toolBtnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnPrev.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnPrev.Image")));
            this.toolBtnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnPrev.Name = "toolBtnPrev";
            this.toolBtnPrev.Size = new System.Drawing.Size(23, 22);
            this.toolBtnPrev.Text = "前一页";
            this.toolBtnPrev.Click += new System.EventHandler(this.toolBtnPrev_Click);
            // 
            // toolTxtIndex
            // 
            this.toolTxtIndex.MaxLength = 5;
            this.toolTxtIndex.Name = "toolTxtIndex";
            this.toolTxtIndex.Size = new System.Drawing.Size(40, 25);
            this.toolTxtIndex.Text = "1";
            this.toolTxtIndex.ToolTipText = "当前页数";
            this.toolTxtIndex.Validated += new System.EventHandler(this.toolTxtIndex_Validated);
            this.toolTxtIndex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolTxtIndex_KeyPress);
            // 
            // toolLblPage
            // 
            this.toolLblPage.Name = "toolLblPage";
            this.toolLblPage.Size = new System.Drawing.Size(17, 22);
            this.toolLblPage.Text = "/1";
            this.toolLblPage.ToolTipText = "总页数";
            // 
            // toolBtnNext
            // 
            this.toolBtnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnNext.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnNext.Image")));
            this.toolBtnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnNext.Name = "toolBtnNext";
            this.toolBtnNext.Size = new System.Drawing.Size(23, 22);
            this.toolBtnNext.Text = "后一页";
            this.toolBtnNext.Click += new System.EventHandler(this.toolBtnNext_Click);
            // 
            // toolBtnLast
            // 
            this.toolBtnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnLast.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnLast.Image")));
            this.toolBtnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolBtnLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnLast.Name = "toolBtnLast";
            this.toolBtnLast.Size = new System.Drawing.Size(23, 22);
            this.toolBtnLast.Text = "最后";
            this.toolBtnLast.Click += new System.EventHandler(this.toolBtnLast_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(29, 22);
            this.toolStripLabel1.Text = "每页";
            // 
            // toolCboPageSize
            // 
            this.toolCboPageSize.AutoCompleteCustomSource.AddRange(new string[] {
            "50",
            "100",
            "500",
            "1000"});
            this.toolCboPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolCboPageSize.DropDownWidth = 50;
            this.toolCboPageSize.Items.AddRange(new object[] {
            "50",
            "100",
            "500",
            "1000"});
            this.toolCboPageSize.Name = "toolCboPageSize";
            this.toolCboPageSize.Size = new System.Drawing.Size(75, 25);
            this.toolCboPageSize.SelectedIndexChanged += new System.EventHandler(this.toolCboPageSize_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(29, 22);
            this.toolStripLabel2.Text = "记录";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // DataListPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "DataListPage";
            this.Size = new System.Drawing.Size(629, 25);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolBtnFirst;
        private System.Windows.Forms.ToolStripLabel toolLblTotal;
        private System.Windows.Forms.ToolStripButton toolBtnPrev;
        private System.Windows.Forms.ToolStripButton toolBtnNext;
        private System.Windows.Forms.ToolStripButton toolBtnLast;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox toolTxtIndex;
        private System.Windows.Forms.ToolStripComboBox toolCboPageSize;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolLblPage;
        private hwj.UserControls.ToolStrip.ToolStripCheckedBox toolChkSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolSeparatorSelAll;
    }
}
