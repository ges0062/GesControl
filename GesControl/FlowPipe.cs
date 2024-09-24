using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp_自定义控件
{
    public partial class FlowPipe : UserControl
    {
        public FlowPipe()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); //忽略窗口消息减少闪烁
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            //流动条流动速度（刷新速度）
            this.myTimer = new Timer();
            myTimer.Interval = 50;
            this.myTimer.Tick += MyTimer_Tick; ;

        }

        #region 定时循环
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            this.startOffset = this.startOffset - this.moveSpeed;

            if (this.startOffset > this.flowLength + this.flowLengthGap || this.startOffset < (this.flowLength + this.flowLengthGap) * (-1))
            { this.startOffset = 0; }
            this.Invalidate();
        }

        #endregion

        #region 属性

        //管道左右两端的转向（上、下、左、右、无）
        public enum PipeTurn
        {
            Up = 1,
            Down,
            Left,
            Right,
            None
        }

        //管道样式（水平，垂直）
        public enum PipeStyle
        {
            Horizontal = 1,
            Vertical,
        }

        private PipeTurn pipeTurnLeft = PipeTurn.None;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("左管道转向")]
        public PipeTurn PipeTurnLeft
        {
            get { return pipeTurnLeft; }
            set
            {
                this.pipeTurnLeft = value;
                this.Invalidate();
            }
        }

        private PipeTurn pipeTurnRight = PipeTurn.None;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("右管道转向")]
        public PipeTurn PipeTurnRight
        {
            get { return pipeTurnRight; }
            set
            {
                this.pipeTurnRight = value;
                this.Invalidate();
            }
        }

        private PipeStyle pipeStyle = PipeStyle.Horizontal;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("管道类型")]
        public PipeStyle PipeStyleHV
        {
            get { return pipeStyle; }
            set
            {
                this.pipeStyle = value;
                this.Invalidate();
            }
        }

        private bool isActive = false;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("是否激活")]
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                this.isActive = value;
                this.myTimer.Enabled = value;
                this.Invalidate();
            }
        }

        private float moveSpeed = 0.3f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("流动条速度，负数为反向")]
        public float MoveSpeed
        {
            get { return moveSpeed; }
            set
            {
                this.moveSpeed = value;
                this.Invalidate();
            }
        }


        private int flowLength = 5;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("流动条长度")]
        public int FlowLength
        {
            get { return flowLength; }
            set
            {
                this.flowLength = value;
                this.Invalidate();
            }
        }


        private int flowWidth = 5;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("流动条宽度")]
        public int FlowWidth
        {
            get { return flowWidth; }
            set
            {
                this.flowWidth = value;
                this.Invalidate();
            }
        }

        private int flowLengthGap = 5;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("流动条间隙")]
        public int FlowLengthGap
        {
            get { return flowLengthGap; }
            set
            {
                this.flowLengthGap = value;
                this.Invalidate();
            }
        }


        private Color flowColor = Color.DodgerBlue;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("流动条颜色")]
        public Color FlowColor
        {
            get { return flowColor; }
            set
            {
                this.flowColor = value;
                this.Invalidate();
            }
        }

        private Color pipeColorBorder = Color.DimGray;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("管道边线颜色")]
        public Color PipeColorBorder
        {
            get { return pipeColorBorder; }
            set
            {
                this.pipeColorBorder = value;
                this.Invalidate();
            }
        }

        private Color pipeColorCenter = Color.LightGray;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("管道中心颜色")]
        public Color PipeColorCenter
        {
            get { return pipeColorCenter; }
            set
            {
                this.pipeColorCenter = value;
                this.Invalidate();
            }
        }

        private Color pipeColorEdge = Color.DimGray;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("管道边沿颜色")]
        public Color PipeColorEdge
        {
            get { return pipeColorEdge; }
            set
            {
                this.pipeColorEdge = value;
                this.p = new Pen(value, 1.0f);
                this.Invalidate();
            }
        }

        #endregion

        #region 画布

        //字段
        private Graphics g;   //GPI绘图
        private Pen p;  //画笔
        private float startOffset = 0; //短划线起始位置
        private Timer myTimer;

        protected override void OnPaint(PaintEventArgs e)
        {
            //画布质量
            g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            ColorBlend colorBlend = new ColorBlend();  //渐变色设置

            //渐近线比例、颜色
            colorBlend.Positions = new float[] { 0.0f, 0.5f, 1.0f };
            colorBlend.Colors = new Color[] { this.pipeColorEdge, this.pipeColorCenter, this.pipeColorEdge };

            //画笔初始化
            this.p = new Pen(this.pipeColorBorder, 1.0f);

            //管道绘制（水平）
            if (this.pipeStyle == PipeStyle.Horizontal)
            {
                //矩形画刷
                LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, this.Height), pipeColorEdge, pipeColorEdge);
                linearGradientBrush.InterpolationColors = colorBlend;

                //绘制左部分
                switch (this.pipeTurnLeft)
                {
                    case PipeTurn.Up:
                        this.PaintEllipse(g, colorBlend, p, new Rectangle(0, this.Height * (-1)-1, this.Height * 2, this.Height * 2), 90.0f, 90.0f);
                        break;
                    case PipeTurn.Down:
                        this.PaintEllipse(g, colorBlend, p, new Rectangle(0, 0, this.Height * 2, this.Height * 2), 180.0f, 90.0f);
                        break;
                    default:
                        this.PaintRectangle(g, linearGradientBrush, p, new Rectangle(-1, 0, this.Height+1, this.Height));
                        break;
                }

                //绘制右部分
                switch (this.pipeTurnRight)
                {
                    case PipeTurn.Up:
                        this.PaintEllipse(g, colorBlend, p, new Rectangle(this.Width - this.Height * 2, this.Height * (-1)-1, this.Height * 2, this.Height * 2), 0.0f, 90.0f);
                        break;
                    case PipeTurn.Down:
                        this.PaintEllipse(g, colorBlend, p, new Rectangle(this.Width - this.Height * 2, 0, this.Height * 2, this.Height * 2), 270.0f, 90.0f);
                        break;
                    default:
                        this.PaintRectangle(g, linearGradientBrush, p, new Rectangle(this.Width - this.Height, 0, this.Height, this.Height));
                        break;
                }

                //绘制中间
                if (this.Width > this.Height * 2)
                {
                    this.PaintRectangle(g, linearGradientBrush, p, new Rectangle(this.Height - 1, 0, this.Width - this.Height * 2 + 2, this.Height));
                }

                //绘制流动条
                if (isActive)
                {
                    //流动条路径
                    GraphicsPath path = new GraphicsPath();

                    //虚线路径—左边
                    switch (this.pipeTurnLeft)
                    {
                        case PipeTurn.Up:
                            path.AddArc(new Rectangle(this.Height / 2, this.Height / 2 * (-1) -1, this.Height, this.Height), 181.0f, -91.0f);
                            break;
                        case PipeTurn.Down:
                            path.AddArc(new Rectangle(this.Height / 2, this.Height / 2, this.Height, this.Height), 180.0f, 90.0f);
                            break;
                        default:
                            path.AddLine(-1, this.Height / 2, this.Height+1, this.Height / 2);
                            break;
                    }

                    //虚线路径—中间
                    if (this.Width > this.Height * 2)
                    {
                        path.AddLine(this.Height, this.Height / 2, this.Width - this.Height -1, this.Height / 2);
                    }

                    //虚线路径—右边
                    switch (this.pipeTurnRight)
                    {
                        case PipeTurn.Up:
                            path.AddArc(new Rectangle(this.Width - 1 - this.Height * 3 / 2, -this.Height / 2-1 , this.Height, this.Height), 88f, -91.0f);
                            break;
                        case PipeTurn.Down:
                            path.AddArc(new Rectangle(this.Width - 1 - this.Height * 3 / 2, this.Height / 2, this.Height, this.Height), 270.0f, 90.0f);
                            break;
                        default:
                            path.AddLine(this.Width - this.Height, this.Height / 2, this.Width , this.Height / 2);
                            break;
                    }

                    //画虚线，关键用笔和路径来
                    Pen pen = new Pen(this.flowColor, this.flowWidth);
                    pen.DashStyle = DashStyle.Custom;
                    pen.DashPattern = new float[]
                    {
                        flowLength,flowLengthGap
                    };
                    pen.DashOffset = this.startOffset;
                    g.DrawPath(pen, path);
                }
            }
            //管道绘制（垂直）
            else
            {
                //矩形画刷
                LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), pipeColorEdge, pipeColorEdge);
                linearGradientBrush2.InterpolationColors = colorBlend;

                //绘制上部分
                switch (this.pipeTurnLeft)
                {
                    case PipeTurn.Left:
                        this.PaintEllipse(g, colorBlend, p, new Rectangle(-this.Width-1, 0, this.Width * 2, this.Width * 2), 270.0f, 90.0f);
                        break;
                    case PipeTurn.Right:
                        this.PaintEllipse(g, colorBlend, p, new Rectangle(0, 0, this.Width * 2, this.Width * 2), 180.0f, 90.0f);
                        break;
                    default:
                        this.PaintRectangle(g, linearGradientBrush2, p, new Rectangle(0, -1, this.Width, this.Width+1));
                        break;
                }

                //绘制下部分
                switch (this.pipeTurnRight)
                {
                    case PipeTurn.Left:
                        this.PaintEllipse(g, colorBlend, p, new Rectangle(-this.Width-1, this.Height - this.Width * 2, this.Width * 2, this.Width * 2), 0.0f, 90.0f);
                        break;
                    case PipeTurn.Right:
                        this.PaintEllipse(g, colorBlend, p, new Rectangle(0, this.Height - this.Width * 2, this.Width * 2, this.Width * 2), 90f, 90.0f);
                        break;
                    default:
                        this.PaintRectangle(g, linearGradientBrush2, p, new Rectangle(0, this.Height - this.Width, this.Width, this.Width));
                        break;
                }

                //绘制中间
                if (this.Height > this.Width * 2)
                {
                    this.PaintRectangle(g, linearGradientBrush2, p, new Rectangle(0, this.Width - 1, this.Width, this.Height - this.Width * 2 + 2));
                }

                //绘制流动条
                if (isActive)
                {
                    //流动条路径
                    GraphicsPath path = new GraphicsPath();

                    //虚线路径—上边
                    switch (this.pipeTurnLeft)
                    {
                        case PipeTurn.Left:
                            path.AddArc(new Rectangle(-this.Width / 2-1, this.Width / 2, this.Width, this.Width), 270.0f, 88.0f);
                            break;
                        case PipeTurn.Right:
                            path.AddArc(new Rectangle(this.Width / 2, this.Width / 2, this.Width, this.Width), 271.0f, -90.0f);
                            break;
                        default:
                            path.AddLine(this.Width / 2, -1, this.Width / 2, this.Width);
                            break;
                    }

                    //虚线路径—中间
                    if (this.Height > this.Width * 2)
                    {
                        path.AddLine(this.Width / 2, this.Width, this.Width / 2, this.Height - this.Width-1 );
                    }

                    //虚线路径—下边
                    switch (this.pipeTurnRight)
                    {
                        case PipeTurn.Left:
                            path.AddArc(new Rectangle(-this.Width / 2-1, this.Height - this.Width * 3 / 2-1, this.Width, this.Width), 0f, 91.0f);
                            break;
                        case PipeTurn.Right:
                            path.AddArc(new Rectangle(this.Width / 2, this.Height  - this.Width * 3 / 2, this.Width, this.Width), 180.0f, -90.0f);
                            break;
                        default:
                            path.AddLine(this.Width / 2, this.Height - this.Width, this.Width / 2, this.Height );
                            break;
                    }

                    //画虚线，关键用笔和路径来
                    Pen pen = new Pen(this.flowColor, this.flowWidth);
                    pen.DashStyle = DashStyle.Custom;
                    pen.DashPattern = new float[]
                    {
                        flowLength,flowLengthGap
                    };
                    pen.DashOffset = this.startOffset;
                    g.DrawPath(pen, path);
                }
            }
        }

        #endregion

        #region 画管道的方法
        /// <summary>
        /// 画渐变色半圆的方法
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="colorBlend"></param>
        /// <param name="p"></param>
        /// <param name="rect"></param>
        /// <param name="startAngle"></param>
        /// <param name="sweepAngle"></param>
        private void PaintEllipse(Graphics g, ColorBlend colorBlend, Pen p, Rectangle rect, float startAngle, float sweepAngle)
        {
            //第一步：创建GPI路径
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(rect);

            //第二步：渐变色填充
            PathGradientBrush brush = new PathGradientBrush(path);
            brush.CenterPoint = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            brush.InterpolationColors = colorBlend;

            //第三步：绘制管道
            g.FillPie(brush, rect, startAngle, sweepAngle);

            //第四步：绘制边线
            g.DrawArc(p, rect, startAngle, sweepAngle);
        }

        /// <summary>
        /// 画渐变色矩形的方法
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="brush">画刷</param>
        /// <param name="pen">笔</param>
        /// <param name="rectangle">矩形</param>
        private void PaintRectangle(Graphics g, Brush brush, Pen pen, Rectangle rectangle)
        {
            //填充矩形
            g.FillRectangle(brush, rectangle);

            switch (this.pipeStyle)
            {
                case PipeStyle.Horizontal:
                    g.DrawLine(pen, rectangle.X, rectangle.Y, rectangle.X + rectangle.Width, rectangle.Y);
                    g.DrawLine(pen, rectangle.X, rectangle.Y + rectangle.Height - 1, rectangle.X + rectangle.Width, rectangle.Height);
                    break;
                case PipeStyle.Vertical:
                    g.DrawLine(pen, rectangle.X, rectangle.Y, rectangle.X, rectangle.Y + rectangle.Height);
                    g.DrawLine(pen, rectangle.X + rectangle.Width - 1, rectangle.Y, rectangle.X + rectangle.Width - 1, rectangle.Height);
                    break;
                default:
                    break;
            }
        }

        #endregion


    }
}
