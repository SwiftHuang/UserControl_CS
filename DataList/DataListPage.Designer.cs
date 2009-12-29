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
            this.toolChkSelectAll = new hwj.UserControls.ToolStrip.ToolStripCheckedBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
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
            // toolChkSelectAll
            // 
            this.toolChkSelectAll.AccessibleDescription = null;
            resources.ApplyResources(this.toolChkSelectAll, "toolChkSelectAll");
            this.toolChkSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.toolChkSelectAll.BackgroundImage = null;
            // 
            // toolChkSelectAll
            // 
            this.toolChkSelectAll.CheckBox.AccessibleDescription = null;
            resources.ApplyResources(this.toolChkSelectAll.CheckBox, "toolChkSelectAll");
            this.toolChkSelectAll.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this.toolChkSelectAll.CheckBox.BackgroundImage = null;
            this.toolChkSelectAll.CheckBox.Font = null;
            this.toolChkSelectAll.CheckBox.Name = "toolStripCheckedBox1";
            this.toolChkSelectAll.CheckBox.UseVisualStyleBackColor = false;
            this.toolChkSelectAll.Name = "toolChkSelectAll";
            this.toolChkSelectAll.CheckedChanged += new System.EventHandler(this.toolChkSelectAll_CheckedChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = null;
            this.toolStrip1.AccessibleName = null;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackgroundImage = null;
            this.toolStrip1.Font = null;
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
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AccessibleDescription = null;
            this.toolStripSeparator3.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolSeparatorSelAll
            // 
            this.toolSeparatorSelAll.AccessibleDescription = null;
            this.toolSeparatorSelAll.AccessibleName = null;
            resources.ApplyResources(this.toolSeparatorSelAll, "toolSeparatorSelAll");
            this.toolSeparatorSelAll.Name = "toolSeparatorSelAll";
            // 
            // toolBtnFirst
            // 
            this.toolBtnFirst.AccessibleDescription = null;
            this.toolBtnFirst.AccessibleName = null;
            resources.ApplyResources(this.toolBtnFirst, "toolBtnFirst");
            this.toolBtnFirst.BackgroundImage = null;
            this.toolBtnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnFirst.Name = "toolBtnFirst";
            this.toolBtnFirst.Click += new System.EventHandler(this.toolBtnFirst_Click);
            // 
            // toolLblTotal
            // 
            this.toolLblTotal.AccessibleDescription = null;
            this.toolLblTotal.AccessibleName = null;
            this.toolLblTotal.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.toolLblTotal, "toolLblTotal");
            this.toolLblTotal.BackgroundImage = null;
            this.toolLblTotal.Name = "toolLblTotal";
            // 
            // toolBtnPrev
            // 
            this.toolBtnPrev.AccessibleDescription = null;
            this.toolBtnPrev.AccessibleName = null;
            resources.ApplyResources(this.toolBtnPrev, "toolBtnPrev");
            this.toolBtnPrev.BackgroundImage = null;
            this.toolBtnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnPrev.Name = "toolBtnPrev";
            this.toolBtnPrev.Click += new System.EventHandler(this.toolBtnPrev_Click);
            // 
            // toolTxtIndex
            // 
            this.toolTxtIndex.AccessibleDescription = null;
            this.toolTxtIndex.AccessibleName = null;
            resources.ApplyResources(this.toolTxtIndex, "toolTxtIndex");
            this.toolTxtIndex.Name = "toolTxtIndex";
            this.toolTxtIndex.Validated += new System.EventHandler(this.toolTxtIndex_Validated);
            this.toolTxtIndex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolTxtIndex_KeyPress);
            // 
            // toolLblPage
            // 
            this.toolLblPage.AccessibleDescription = null;
            this.toolLblPage.AccessibleName = null;
            resources.ApplyResources(this.toolLblPage, "toolLblPage");
            this.toolLblPage.BackgroundImage = null;
            this.toolLblPage.Name = "toolLblPage";
            // 
            // toolBtnNext
            // 
            this.toolBtnNext.AccessibleDescription = null;
            this.toolBtnNext.AccessibleName = null;
            resources.ApplyResources(this.toolBtnNext, "toolBtnNext");
            this.toolBtnNext.BackgroundImage = null;
            this.toolBtnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnNext.Name = "toolBtnNext";
            this.toolBtnNext.Click += new System.EventHandler(this.toolBtnNext_Click);
            // 
            // toolBtnLast
            // 
            this.toolBtnLast.AccessibleDescription = null;
            this.toolBtnLast.AccessibleName = null;
            resources.ApplyResources(this.toolBtnLast, "toolBtnLast");
            this.toolBtnLast.BackgroundImage = null;
            this.toolBtnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnLast.Name = "toolBtnLast";
            this.toolBtnLast.Click += new System.EventHandler(this.toolBtnLast_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AccessibleDescription = null;
            this.toolStripLabel1.AccessibleName = null;
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            this.toolStripLabel1.BackgroundImage = null;
            this.toolStripLabel1.Name = "toolStripLabel1";
            // 
            // toolCboPageSize
            // 
            this.toolCboPageSize.AccessibleDescription = null;
            this.toolCboPageSize.AccessibleName = null;
            this.toolCboPageSize.AutoCompleteCustomSource.AddRange(new string[] {
            resources.GetString("toolCboPageSize.AutoCompleteCustomSource"),
            resources.GetString("toolCboPageSize.AutoCompleteCustomSource1"),
            resources.GetString("toolCboPageSize.AutoCompleteCustomSource2"),
            resources.GetString("toolCboPageSize.AutoCompleteCustomSource3")});
            resources.ApplyResources(this.toolCboPageSize, "toolCboPageSize");
            this.toolCboPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolCboPageSize.DropDownWidth = 50;
            this.toolCboPageSize.Items.AddRange(new object[] {
            resources.GetString("toolCboPageSize.Items"),
            resources.GetString("toolCboPageSize.Items1"),
            resources.GetString("toolCboPageSize.Items2"),
            resources.GetString("toolCboPageSize.Items3")});
            this.toolCboPageSize.Name = "toolCboPageSize";
            this.toolCboPageSize.SelectedIndexChanged += new System.EventHandler(this.toolCboPageSize_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AccessibleDescription = null;
            this.toolStripLabel2.AccessibleName = null;
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            this.toolStripLabel2.BackgroundImage = null;
            this.toolStripLabel2.Name = "toolStripLabel2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // DataListPage
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.toolStrip1);
            this.Font = null;
            this.Name = "DataListPage";
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
