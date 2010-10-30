using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace hwj.UserControls.CommonControls
{
    public class xButton : Button
    {

        public xButton()
        {

        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            CursorFromClick = Cursors.WaitCursor;
        }
        #region Property
        [DefaultValue(typeof(Cursors), "WaitCursor")]
        public Cursor CursorFromClick { get; set; }
        #endregion
        private Cursor _CurrCursor;
        protected override void OnClick(EventArgs e)
        {
            if (e != EventArgs.Empty)
            {
                _CurrCursor = this.Cursor;
                this.Cursor = CursorFromClick;
            }
            try
            {
                base.OnClick(e);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (e != EventArgs.Empty)
                {
                    this.Cursor = _CurrCursor;
                }
            }
        }
    }
}
