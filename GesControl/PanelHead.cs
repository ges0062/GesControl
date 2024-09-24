using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp_自定义控件
{
    public partial class PanelHead : Panel
    {
        public PanelHead()
        {
            InitializeComponent();

            //字体对齐
            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;
            this.Size = new System.Drawing.Size(300, 150);
        }

        public PanelHead(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;
        }

        #region  属性
        private Color colorTitle = Color.White;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("标题文本颜色")]
        public Color ColorTitle
        {
            get { return colorTitle; }
            set { colorTitle = value; this.Invalidate(); }
        }

        private Color colorBack = Color.LimeGreen;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("背景颜色")]
        public Color ColorBack
        {
            get { return colorBack; }
            set { colorBack = value; this.Invalidate(); }
        }

        private Color colorBorder = Color.Gray;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("边框颜色")]
        public Color ColorBorder
        {
            get { return colorBorder; }
            set { colorBorder = value; this.Invalidate(); }
        }

        private Font titleFont = new Font("微软雅黑", 12);
        [Browsable(true)]
        [Category("布局_G")]
        [Description("标题字体")]
        public Font TitleFont
        {
            get { return titleFont; }
            set
            {
                titleFont = value;
                this.Invalidate();
            }
        }

        private int titleHeight = 30;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("抬头高度")]
        public int TitleHeight
        {
            get { return titleHeight; }
            set
            {
                titleHeight = value;
                this.Invalidate();
            }
        }

        private string titleText = "标题文本";
        [Browsable(true)]
        [Category("布局_G")]
        [Description("标题文本")]
        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;
                this.Invalidate();
            }
        }
        #endregion  

        #region  重绘
        private Graphics g;   //GPI绘图
        private StringFormat sf;  //字体格式
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //画布质量
            g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //画外边框
            g.DrawRectangle(new Pen(this.colorBorder), new Rectangle(0, 0, this.Width - 1, this.Height - 1));

            //填充抬头矩形
            RectangleF rec = new RectangleF(0.5f, 0.5f, this.Width - 2, this.titleHeight);
            g.FillRectangle(new SolidBrush(this.colorBack), rec);

            //文本绘制
            g.DrawString(this.titleText, this.titleFont, new SolidBrush(this.colorTitle), rec, sf);
        }

        #endregion
    }
}
