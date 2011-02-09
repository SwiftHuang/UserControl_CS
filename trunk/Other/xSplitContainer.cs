using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace hwj.UserControls.Other
{
    public enum SplitterDirection
    {
        None,
        Right,
        Left,
        Up,
        Bottom
    }

    public partial class xSplitContainer : SplitContainer
    {
        #region Property

        //string DirectionStr = "-";
        private int splitterLine = 0;
        private Rectangle splitterButtonRectangle = new Rectangle(0, 0, 10, 10);
        private Brush splitterButtonBrush = new LinearGradientBrush(new Rectangle(0, 0, 50, 50), Color.GreenYellow, Color.YellowGreen, LinearGradientMode.ForwardDiagonal);
        private Brush splitterButtonBrushHover = new LinearGradientBrush(new Rectangle(0, 0, 50, 50), Color.GreenYellow, Color.YellowGreen, LinearGradientMode.ForwardDiagonal);

        private int splitterButtonHeight = 60;
        [Description("获取或设置分隔按钮的高度"), Category("Extension"), DefaultValue(60)]
        public int SplitterButtonHeight
        {
            get { return splitterButtonHeight; }
            set { splitterButtonHeight = value; }
        }

        private SplitterDirection splitterDirection = SplitterDirection.Left;
        [Description("获取或设置分栏方向"), Category("Extension"), DefaultValue(typeof(SplitterDirection), "Left")]
        public SplitterDirection SplitterDirection
        {
            get { return splitterDirection; }
            set
            {
                switch (value)
                {
                    case SplitterDirection.Right:
                        this.Orientation = Orientation.Vertical;
                        this.FixedPanel = FixedPanel.Panel2;
                        break;
                    case SplitterDirection.Left:
                        this.Orientation = Orientation.Vertical;
                        this.FixedPanel = FixedPanel.Panel1;
                        break;
                    case SplitterDirection.Up:
                        this.Orientation = Orientation.Horizontal;
                        this.FixedPanel = FixedPanel.Panel1;
                        break;
                    case SplitterDirection.Bottom:
                        this.Orientation = Orientation.Horizontal;
                        this.FixedPanel = FixedPanel.Panel2;
                        break;
                }
                splitterDirection = value;
                //setDirectionString();
            }
        }

        private int splitterMinValue = 0;
        [Description("获取或设置分栏面板的最小宽度或高度"), Category("Extension"), DefaultValue(0)]
        public int SplitterMinValue
        {
            get { return splitterMinValue; }
            set { splitterMinValue = value; }
        }

        private bool splitterCollapesd = false;
        [Description("获取或设置收缩分栏"), Category("Extension"), DefaultValue(false)]
        public bool SplitterCollapesd
        {
            get
            {
                return splitterCollapesd;
            }
            set
            {
                if (this.SplitterCollapesd == value)
                {
                    return;
                }
                splitterCollapesd = value;
                if (value)
                {
                    this.splitterLine = this.SplitterDistance;
                    switch (this.splitterDirection)
                    {
                        case SplitterDirection.Right:
                            this.SplitterDistance = this.Width - this.SplitterWidth;
                            break;
                        case SplitterDirection.Bottom:
                            this.SplitterDistance = this.Height - this.SplitterWidth;
                            break;
                        case SplitterDirection.Left:
                        case SplitterDirection.Up:
                            this.SplitterDistance = 0;
                            break;
                    }
                    this.IsSplitterFixed = true;
                }
                else
                {
                    this.SplitterDistance = this.splitterLine;
                    this.IsSplitterFixed = false;
                }
                //setDirectionString();

                Panel1.Refresh();
                Panel2.Refresh();
                this.Refresh();

            }
        }

        private Image splitterButtonBackGround;
        [Description("获取或设置分栏按钮的背景图片"), Category("Extension")]
        public Image SplitterButtonBackGround
        {
            get { return splitterButtonBackGround; }
            set { splitterButtonBackGround = value; }
        }

        private Image splitterButtonBackGroundHover;
        [Description("获取或设置鼠标停留在分栏按钮时的图片"), Category("Extension")]
        public Image SplitterButtonBackGroundHover
        {
            get { return splitterButtonBackGroundHover; }
            set { splitterButtonBackGroundHover = value; }
        }

        private bool splitterButtonMouseHover = false;
        private bool SplitterButtonMouseHover
        {
            get { return splitterButtonMouseHover; }
            set
            {
                if (value == splitterButtonMouseHover)
                {
                    return;
                }
                if (value)
                {
                    if (this.splitterButtonBackGroundHover == null)
                    {
                        this.splitterButtonBrushHover.Dispose();
                        this.splitterButtonBrushHover = new LinearGradientBrush(this.splitterButtonRectangle, Color.Red, Color.Yellow, LinearGradientMode.ForwardDiagonal);
                    }
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    if (this.splitterButtonBackGround == null)
                    {
                        this.splitterButtonBrush.Dispose();
                        this.splitterButtonBrush = new LinearGradientBrush(this.splitterButtonRectangle, Color.GreenYellow, Color.YellowGreen, LinearGradientMode.BackwardDiagonal);
                    }
                    this.Cursor = Cursors.Default;
                }
                splitterButtonMouseHover = value;
                this.Invalidate(this.splitterButtonRectangle);
            }
        }

        #endregion

        public xSplitContainer()
        {
            InitializeComponent();
            SetSplitterButtonRectangle();
            this.SetStyle(ControlStyles.Selectable, false);
            this.SplitterMoved += new SplitterEventHandler(xSplitContainer_SplitterMoved);
        }

        #region Private Function
        private void SetSplitterButtonRectangle()
        {
            if (this.Orientation == Orientation.Vertical)
            {
                this.splitterButtonRectangle = new Rectangle(SplitterRectangle.X, SplitterRectangle.Height / 2 - splitterButtonHeight / 2, this.SplitterRectangle.Width, splitterButtonHeight);
            }
            else
            {
                this.splitterButtonRectangle = new Rectangle(SplitterRectangle.Width / 2 - splitterButtonHeight / 2, SplitterRectangle.Y, splitterButtonHeight, this.SplitterWidth);
            }

        }
        #endregion

        #region Event Function

        private void MSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            switch (this.splitterDirection)
            {
                case SplitterDirection.Right:
                    if (this.SplitterCollapesd == false && this.SplitterDistance > this.Width - this.splitterMinValue)
                    {
                        this.SplitterDistance = this.Width - this.splitterMinValue;
                    }
                    break;
                case SplitterDirection.Left:
                    if (this.SplitterCollapesd == false && this.SplitterDistance < this.splitterMinValue)
                    {
                        this.SplitterDistance = this.splitterMinValue;
                    }
                    break;
                case SplitterDirection.Bottom:
                    if (this.SplitterCollapesd == false && this.SplitterDistance > this.Height - this.splitterMinValue)
                    {
                        this.SplitterDistance = this.Height - this.splitterMinValue;
                    }
                    break;
                case SplitterDirection.Up:
                    if (this.SplitterDistance > this.splitterMinValue)
                    {
                        this.SplitterDistance = this.splitterMinValue;
                    }
                    break;
            }
            SetSplitterButtonRectangle();
            this.Invalidate(this.SplitterRectangle);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SetSplitterButtonRectangle();
            if (this.splitterButtonMouseHover)
            {
                e.Graphics.FillRectangle(this.splitterButtonBrushHover, this.splitterButtonRectangle);
                if (this.splitterButtonBackGroundHover != null)
                {
                    e.Graphics.DrawImage(this.SplitterButtonBackGroundHover, this.splitterButtonRectangle);
                }
            }
            else
            {
                e.Graphics.FillRectangle(this.splitterButtonBrush, this.splitterButtonRectangle);
                if (this.splitterButtonBackGround != null)
                {
                    e.Graphics.DrawImage(this.SplitterButtonBackGround, this.splitterButtonRectangle);
                }
            }

            base.OnPaint(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (this.splitterButtonMouseHover)
            {
                base.OnMouseClick(e);
                this.SplitterCollapesd = !this.SplitterCollapesd;

                if (this.SplitterButtonBackGround != null)
                {
                    Image img1 = this.SplitterButtonBackGround;
                    img1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    this.SplitterButtonBackGround = img1;
                }
                if (this.SplitterButtonBackGroundHover != null)
                {
                    Image img2 = this.SplitterButtonBackGroundHover;
                    img2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    this.SplitterButtonBackGroundHover = img2;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.X > this.splitterButtonRectangle.X && e.X < this.splitterButtonRectangle.Width + this.splitterButtonRectangle.X && e.Y > this.splitterButtonRectangle.Y && e.Y < this.splitterButtonRectangle.Y + this.splitterButtonRectangle.Height)
            {
                this.SplitterButtonMouseHover = true;
            }
            else
            {
                this.SplitterButtonMouseHover = false;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.SplitterButtonMouseHover = false;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.splitterButtonMouseHover == false)
            {
                base.OnMouseDown(e);
            }
        }
        /// <summary>
        /// 非常格硬来的事情
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.Panel2.Focus();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Refresh();
        }
        void xSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.Refresh();
        }
        #endregion
    }
}
