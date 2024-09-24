using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_自定义控件
{
    public partial class DashBoard : UserControl
    {
        public DashBoard()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

        }

        #region 属性【外环颜色、角度、间隙；刻度颜色、占比、字体；文本字体；量程；当前值】

        private Graphics g;    //画布
        private Pen p;         //笔-绘制线条、曲线
        private SolidBrush sb; //笔（填充）-填充矩形、路径
        private int width;
        private int height;


        private Color colorCircle1 = Color.FromArgb(33, 80, 33);
        [Browsable(true)]
        [Category("布局_G")]
        [Description("外环颜色1")]
        public Color ColorCircle1
        {
            get { return colorCircle1; }
            set { colorCircle1 = value; this.Invalidate(); }
        }

        private Color colorCircle2 = Color.FromArgb(22, 128, 22);
        [Browsable(true)]
        [Category("布局_G")]
        [Description("外环颜色2")]
        public Color ColorCircle2
        {
            get { return colorCircle2; }
            set { colorCircle2 = value; this.Invalidate(); }
        }

        private Color colorCircle3 = Color.FromArgb(20, 181, 20);
        [Browsable(true)]
        [Category("布局_G")]
        [Description("外环颜色3")]
        public Color ColorCircle3
        {
            get { return colorCircle3; }
            set { colorCircle3 = value; this.Invalidate(); }
        }

        private Color pointColor = Color.Green;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("指针颜色")]
        public Color PointColor
        {
            get { return pointColor; }
            set { pointColor = value; this.Invalidate(); }
        }

        private Color scaleColor = Color.Black;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("刻度颜色")]
        public Color ScaleColor
        {
            get { return scaleColor; }
            set { scaleColor = value; this.Invalidate(); }
        }

        private float scaleProportion = 0.8f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("刻度圆占外圆的比例0-1：越大越紧挨")]
        public float ScaleProportion
        {
            get { return scaleProportion; }
            set
            {
                if (value > 1.0f || value < 0.0f) return;
                scaleProportion = value; this.Invalidate();
            }
        }

        private Font scaleFont = new Font(new FontFamily("微软雅黑"), 10.0f);
        [Browsable(true)]
        [Category("布局_G")]
        [Description("字体格式")]
        public Font ScaleFont
        {
            get { return scaleFont; }
            set
            {
                if (value != null)
                {
                    scaleFont = value;
                    this.Invalidate();
                }
            }
        }

        private string textPrefix = "实际温度：";
        [Browsable(true)]  //说明：文本关闭
        [Category("布局_G")]
        [Description("文本前缀")]
        public string TextPrefix
        {
            get { return textPrefix; }
            set { textPrefix = value; this.Invalidate(); }
        }

        private string textUnit = "℃";
        [Browsable(true)]  //说明：文本关闭
        [Category("布局_G")]
        [Description("文本单位")]
        public string TextUnit
        {
            get { return textUnit; }
            set { textUnit = value; this.Invalidate(); }
        }

        private Font textFont = new Font("Segoe UI", 10.5f);
        [Browsable(true)]
        [Category("布局_G")]
        [Description("字体格式")]
        public Font TextFont
        {
            get { return textFont; }
            set
            {
                if (value != null)
                {
                    textFont = value;
                    this.Invalidate();
                }
            }
        }

        private Color textColor = Color.Black;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("指针颜色")]
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; this.Invalidate(); }
        }

        private float textProportion = 0.88f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("文本显示高度占比0-1；越小越靠上")]
        public float TextProportion
        {
            get { return textProportion; }
            set
            {
                if (value > 1.0f || value < 0.0f) return;
                textProportion = value; this.Invalidate();
            }
        }

        private bool textShow = true;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("是否显示文本")]
        public bool TextShow
        {
            get { return textShow; }

            set
            {
                textShow = value; this.Invalidate();
            }
        }

        private float currentValue = 100.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("实时值显示")]
        public float CurrentValue
        {
            get { return currentValue; }

            set
            {
                if (value < 0.0f || value > range) return;
                currentValue = value; this.Invalidate();
            }
        }

        private int outThickness = 5;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("外环画笔的宽度")]
        public int OutThickness
        {
            get { return outThickness; }

            set
            {
                if (value <= 0) return;
                outThickness = value; this.Invalidate();
            }
        }

        private float range = 180.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("量程")]
        public float Range
        {
            get { return range; }

            set
            {
                if (value < 0.0f) return;
                range = value; this.Invalidate();
            }
        }

        private float centerRadius = 6.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("中心半径")]
        public float CenterRadius
        {
            get { return centerRadius; }

            set
            {
                if (value <= 0.0f) return;
                centerRadius = value; this.Invalidate();
            }
        }

        private float gapAngle = 2.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("间隙角度")]
        public float GapAngle
        {
            get { return gapAngle; }

            set
            {
                if (value <= 0.0f) return;
                gapAngle = value; this.Invalidate();
            }
        }

        #endregion wai 

        #region 重绘【画外圆、画刻度、画指针、画中心、画文本】
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //画布质量
            g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.width = this.Width;
            this.height = this.Height;

            //特殊情况处理
            if (this.width <= 20 || this.height <= 20) return;
            if (this.height < this.width * 0.5f) return;

            // 画外环-定义角度、坐标、宽度、高度
            float angle = (180.0f - gapAngle * 2) / 3;
            RectangleF rec = new RectangleF(10, 10, this.width - 20, this.width - 20);

            //第一个圆弧
            p = new Pen(colorCircle3, outThickness);
            g.DrawArc(p, rec, -180.0f, angle);

            //第二个圆弧
            p = new Pen(colorCircle2, outThickness);
            g.DrawArc(p, rec, -180.0f + angle + gapAngle, angle);

            //第三个圆弧
            p = new Pen(colorCircle1, outThickness);
            g.DrawArc(p, rec, -180.0f + angle * 2.0f + gapAngle + 2.0f, angle);

            //画刻度
            g.TranslateTransform(this.width * 0.5f, this.width * 0.5f);

            for (int i = 0; i < 4; i++)
            {
                float actualAngle = -180.0f + 60.0f * i;
                double x1 = Math.Cos(actualAngle * Math.PI / 180);
                double y1 = Math.Sin(actualAngle * Math.PI / 180);
                float x = Convert.ToSingle(this.width * scaleProportion * 0.5f * x1);
                float y = Convert.ToSingle(this.width * scaleProportion * 0.5f * y1);

                StringFormat sf = new StringFormat();

                if (i > 1)
                {
                    x = x - 60;
                    sf.Alignment = StringAlignment.Far;
                }
                else
                {
                    sf.Alignment = StringAlignment.Near;
                }

                //刻度的坐标，宽，高
                rec = new RectangleF(x, y, 60, 20);
                sb = new SolidBrush(scaleColor);

                if (range % 6 == 0)
                {
                    g.DrawString((range / 3 * i).ToString(), scaleFont, sb, rec, sf);
                }
                else
                {
                    g.DrawString((range / 3 * i).ToString("f1"), scaleFont, sb, rec, sf);
                }
            }

            //画内圆
            g.FillEllipse(new SolidBrush(pointColor), new RectangleF(-centerRadius, -centerRadius, centerRadius * 2.0f, centerRadius * 2.0f));

            //画指针
            p = new Pen(pointColor, 3.0f);  //定义指针颜色、宽度
            float sweepAngle = currentValue / range * 180.0f; //划过的角度
            float z = this.width * 0.5f * scaleProportion - outThickness * 0.5f - 20.0f;  //指针长度
            g.RotateTransform(90.0f); //默认开始角度
            g.RotateTransform(sweepAngle);

            //画线
            g.DrawLine(p, new PointF(0, 0), new PointF(0, z));

            //文本标签
            if (textShow)
            {
                g.RotateTransform(-sweepAngle);
                g.RotateTransform(-90.0f);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                rec = new RectangleF(this.width * (-0.5f), this.height * textProportion - 0.5f * this.width, this.width, this.height * (1.0f - this.scaleProportion));
                //string val = TextPrefix + currentValue.ToString() + "" + textUnit + "（" + (currentValue / range * 100.0f).ToString("f0") + "%" + "）";
                string val = TextPrefix + currentValue.ToString() + "" + textUnit ;

                //文本
                g.DrawString(val, textFont, new SolidBrush(textColor), rec, sf);

            }
        }


        #endregion
    }
}
