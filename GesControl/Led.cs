using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp_自定义控件
{
    public partial class Led : UserControl
    {
        public Led()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            //闪烁频率
            this.myTimer = new Timer();
            myTimer.Interval = this.flickerFre;
            this.myTimer.Tick += MyTimer_Tick;
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            if (this.flickerVis == true)
            {
                //显隐控件
                this.Visible=!this.Visible;
                this.blink=false;
            }
            else
            {
                //内圆闪烁标志
                this.blink = !this.blink;
            }
            this.Invalidate();
        }

        #region 属性-外环与边界间隙、外环宽度、内圆与外环的间隙、内圆、颜色

        private Timer myTimer;  //定义一个计时器
        private bool blink;  //闪烁标志位

        private Color zcolor1 = Color.Gray;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("颜色1")]
        public Color ZColor1
        {
            get { return zcolor1; }
            set { zcolor1 = value; this.Invalidate(); }
        }

        private Color zcolor2 = Color.LimeGreen;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("颜色2")]
        public Color ZColor2
        {
            get { return zcolor2; }
            set { zcolor2 = value; this.Invalidate(); }
        }

        private Color zcolor3 = Color.Red;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("颜色3")]
        public Color ZColor3
        {
            get { return zcolor3; }
            set { zcolor3 = value; this.Invalidate(); }
        }

        private Color zcolor4 = Color.DarkGoldenrod;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("颜色1")]
        public Color ZColor4
        {
            get { return zcolor4; }
            set { zcolor4 = value; this.Invalidate(); }
        }

        private Color zcolor5 = Color.Blue;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("颜色5")]
        public Color ZColor5
        {
            get { return zcolor5; }
            set { zcolor5 = value; this.Invalidate(); }
        }

        private Color zcolor6 = Color.Transparent;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("闪烁颜色为透明")]
        public Color ZColor6
        {
            get { return zcolor6; }
            set { zcolor6 = value; this.Invalidate(); }
        }

        private float gapIn = 8.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("内圆与边界的间隙，越小越离谱")]
        public float GapIn
        {
            get { return gapIn; }
            set
            {
                if (value <= 0 || value <= gapOut) return;
                gapIn = value; this.Invalidate();
            }
        }

        private float gapOut = 3.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("指示灯与边界的间隙，越大越离谱")]
        public float GapOut
        {
            get { return gapOut; }
            set
            {
                if (value <= 0 || value > 0.1 * this.Width || value >= this.gapIn) return;
                gapOut = value; this.Invalidate();
            }
        }

        private float outWidth = 4.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("外环的宽度")]
        public float OutWidth
        {
            get { return outWidth; }
            set
            {
                if (value <= 0 || value < 0.1 * this.Width) return;
                outWidth = value; this.Invalidate();
            }
        }

        private int curValue = 0;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("当前值")]
        public int CurValue
        {
            get { return curValue; }
            set
            {
                if (value < 0 || value > 4) return;
                curValue = value; this.Invalidate();
            }
        }

        private bool flickerAct = false;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("是否闪烁")]
        public bool FlickerAct
        {
            get { return flickerAct; }
            set
            {
                if (value == true)
                {
                    myTimer.Interval = this.flickerFre;
                    this.myTimer.Start();
                }
                else
                {
                    this.myTimer.Stop();
                    this.blink = false;
                    this.Visible = true;
                }
                flickerAct = value; this.Invalidate();
            }
        }

        private bool flickerVis = false;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("是否带外环一起闪烁")]
        public bool FlickerVis
        {
            get { return flickerVis; }
            set
            {
                flickerVis = value; this.Invalidate();
            }
        }

        private int flickerFre = 500;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("闪烁频率ms")]
        public int FlickerFre
        {
            get { return flickerFre; }
            set
            {
                this.flickerFre = value;
                this.Invalidate();
            }
        }

        #endregion

        #region 重绘
        private Graphics g;
        private Pen p;
        private SolidBrush sb;
        private int width;
        private int height;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //画布质量
            g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.width = this.Width;  //当后期需要更改width只需更改这里就可以
            this.height = this.Height;
            this.height = this.width;

            //特殊情况处理
            if (gapIn > 0.5f * this.width || gapIn > this.height * 0.5f) return;

            Color getCurColor = GetCurColor();

            //绘制外环（DrawEllipse-用笔画椭圆）
            p = new Pen(getCurColor, outWidth);
            RectangleF rec = new RectangleF(this.gapOut, this.gapOut, this.width - 2 * this.gapOut, this.height - 2 * gapOut);
            g.DrawEllipse(p, rec);

            //绘制内圆（FillEllipse-填充椭圆内部）
            if (this.flickerAct == true)
            {
                if (this.blink == true)
                {
                    sb = new SolidBrush(zcolor6);
                }
                else
                {
                    sb = new SolidBrush(getCurColor);
                }
            }
            else
            {
                sb = new SolidBrush(getCurColor);
            }
            rec = new RectangleF(gapIn, gapIn, this.width - 2 * this.gapIn, this.height - 2 * gapIn);
            g.FillEllipse(sb, rec);
        }
        #endregion

        #region 根据值获取当前颜色
        private Color GetCurColor()
        {
            List<Color> colors = new List<Color>();
            colors.Add(zcolor1);
            colors.Add(zcolor2);
            colors.Add(zcolor3);
            colors.Add(zcolor4);
            colors.Add(zcolor5);
            return colors[curValue];
        }
        #endregion
    }
}
