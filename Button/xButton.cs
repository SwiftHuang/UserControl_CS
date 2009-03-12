using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
namespace hwj.UserControls
{
    public class xButton : Button
    {

        public xButton()
        {
            
        }
        #region Property
        public Cursor CursorFromClick { get; set; }
        #endregion
        private Cursor _CurrCursor;
        protected override void OnClick(EventArgs e)
        {
            _CurrCursor = this.Cursor;
            this.Cursor = CursorFromClick;
            try
            {
                base.OnClick(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Cursor = _CurrCursor;
            }
        }
    }
}
