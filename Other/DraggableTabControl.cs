using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace hwj.UserControls.Other
{
    [ToolboxBitmap(typeof(DraggableTabControl))]

    public class DraggableTabControl : System.Windows.Forms.TabControl
    {
        private System.ComponentModel.Container components = null;
        /// <summary>
        /// 
        /// </summary>
        public TabPage beDraged;
        //private TabPage lastPage;
        /// <summary>
        /// 
        /// </summary>
        public DragDropEffects effect;

        /// <summary>
        /// 
        /// </summary>
        public DraggableTabControl()
        {
            InitializeComponent();
            AllowDrop = true;

        }


        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
        #endregion

        protected override void OnDragOver(System.Windows.Forms.DragEventArgs e)
        {
            base.OnDragOver(e);

            Point pt = new Point(e.X, e.Y);
            pt = PointToClient(pt);

            TabPage hover_tab = GetTabPageByTab(pt);

            if (hover_tab == null)
            {
                //if (!((UserControl)beDraged.Controls[0]).CanFloating)
                //{
                //    e.Effect = DragDropEffects.None;
                //    effect = DragDropEffects.None;
                //}
                //else
                if (e.Data.GetDataPresent(typeof(TabPage)))
                {
                    e.Effect = DragDropEffects.Copy;
                    effect = DragDropEffects.Copy;
                }
            }
            else if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                e.Effect = DragDropEffects.Move;
                effect = DragDropEffects.Move;
            }

        }
        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
            effect = DragDropEffects.None;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Clicks == 1)
                base.OnMouseDown(e);
            else if (e.Clicks == 2)
                base.OnMouseDoubleClick(e);


            Point pt = new Point(e.X, e.Y);
            beDraged = GetTabPageByTab(pt);

            if (e.Button == MouseButtons.Left)
            {
                if (beDraged != null)
                {
                    DoDragDrop(beDraged, DragDropEffects.All);
                }
                effect = DragDropEffects.None;
            }
        }
        protected override void OnDragDrop(DragEventArgs e)
        {
        
            
            #region Sort
            Point pt = new Point(e.X, e.Y);
            pt = PointToClient(pt);

            TabPage hover_tab = GetTabPageByTab(pt);

            if (hover_tab != null)
            {
                if (e.Data.GetDataPresent(typeof(TabPage)))
                {
                    TabPage drag_tab = (TabPage)e.Data.GetData(typeof(TabPage));


                    int item_drag_index = FindIndex(drag_tab);
                    int drop_location_index = FindIndex(hover_tab);

                    if (item_drag_index != drop_location_index)
                    {
                        this.TabPages.Remove(drag_tab);
                        this.TabPages.Insert(drop_location_index, drag_tab);
                        SelectedTab = drag_tab;
                    }
                }
            }
            #endregion

            base.OnDragDrop(e);
            //#region NewForm
            //ShowNewForm(new Form(), new Point(e.X, e.Y));

            //#endregion
        }

        //protected override void OnSelecting(TabControlCancelEventArgs e)
        //{
        //    if (lastPage != null && lastPage.Controls.Count > 0)
        //    {
        //        BaseEditControl lastBC = lastPage.Controls[0] as BaseEditControl;

        //        if (lastBC != null)
        //        {
        //            TabPage selected = Program.mainFrm.tabMain.SelectedTab;
        //            if (!lastBC.CheckLeave && selected.Controls.Count > 0)
        //            {
        //                Control curControls = selected.Controls[0];
        //                if ((curControls is BaseEditControl && !(curControls as BaseEditControl).FirstLoad || curControls is BaseControl))
        //                {
        //                    e.Cancel = true;
        //                    lastBC.CheckLeave = true;
        //                    return;
        //                }
        //            }
        //        }
        //    }
        //    base.OnSelecting(e);
        //    lastPage = e.TabPage;
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Public Function
        public void ShowNewForm(Form frm, Point? pt)
        {
            if (beDraged != null)
            {
                frm.Size = new Size(beDraged.Width + 10, beDraged.Height + 40);
                if (pt != null)
                {
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = new Point(pt.Value.X - 30, pt.Value.Y - 10);
                }
                else
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                }
                frm.Text = beDraged.Text;
                //frm.toolTipText = beDraged.ToolTipText;
                Controls.Remove(beDraged);
                beDraged.Controls[0].Dock = DockStyle.Fill;
                //(beDraged.Controls[0] as Acct.UI.Base.BaseControl).IncludeTabPage = false;
                frm.Controls.Add(beDraged.Controls[0]);
                beDraged.Dispose();
                frm.Show();
                //Program.mainFrm.formList.Add(frm);
                effect = DragDropEffects.None;
            }
        }
        public TabPage GetTabPageByTab(Point pt)
        {
            TabPage tp = null;

            for (int i = 0; i < TabPages.Count; i++)
            {
                if (GetTabRect(i).Contains(pt))
                {
                    tp = TabPages[i];
                    break;
                }
            }

            return tp;
        }
        #endregion

        #region Private Function
        private int FindIndex(TabPage page)
        {
            for (int i = 0; i < TabPages.Count; i++)
            {
                if (TabPages[i] == page)
                    return i;
            }

            return -1;
        }
        #endregion

    }
}
