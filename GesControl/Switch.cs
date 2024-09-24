using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_自定义控件
{
    [DefaultEvent("MouseDown_G")]
    public partial class Switch : UserControl
    {
        public Switch()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            //添加鼠标点击事件处理
            this.MouseDown += Switch_MouseDown; ;
        }

        #region 属性
        private float cirOutWidth = 3.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("外环宽度")]
        public float CirOutWidth
        {
            get { return cirOutWidth; }
            set
            {
                if (value < 0) return;
                cirOutWidth = value; this.Invalidate();
            }
        }

        private float cirOutGap = 15.0f;  
        [Browsable(true)]
        [Category("布局_G")]
        [Description("外环与边界的间隙")]
        public float CirOutGap
        {
            get { return cirOutGap; }
            set
            {
                if (value < 0) return;
                cirOutGap = value; this.Invalidate();
            }
        }

        private float cirInGap = 19.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("内圆与边界的间隙")]
        public float CirInGap
        {
            get { return cirInGap; }
            set
            {
                if (value < 0) return;
                cirInGap = value; this.Invalidate();
            }
        }

        private Color cirInColor = Color.DimGray;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("内圆颜色")]
        public Color CirInColor
        {
            get { return cirInColor; }
            set { cirInColor = value; this.Invalidate(); }
        }

        private Color togColor = Color.Black;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("开关颜色")]
        public Color TogColor
        {
            get { return togColor; }
            set { togColor = value; this.Invalidate(); }
        }

        private float togWidth = 15.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("开关宽度")]
        public float TogWidth
        {
            get { return togWidth; }
            set
            {
                if (value < 0) return;
                togWidth = value; this.Invalidate();
            }
        }

        private float togGap = 6.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("开关与边界的间隙")]
        public float TogGap
        {
            get { return togGap; }
            set
            {
                if (value < 0) return;
                togGap = value; this.Invalidate();
            }
        }

        private Color togForeColor = Color.FromArgb(255, 128, 0);
        [Browsable(true)]
        [Category("布局_G")]
        [Description("开关圆点颜色")]
        public Color TogForeColor
        {
            get { return togForeColor; }
            set { togForeColor = value; this.Invalidate(); }
        }

        private float togForeHeight = 20.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("开关圆点颜色")]
        public float TogForeHeight
        {
            get { return togForeHeight; }
            set
            {
                if (value < 0) return;
                togForeHeight = value; this.Invalidate();
            }
        }

        private float togForeGap = 4.0f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("开关圆点与开关的间隙")]
        public float TogForeGap
        {
            get { return togForeGap; }
            set
            {
                if (value < 0) return;
                togForeGap = value; this.Invalidate();
            }
        }

        private bool switchStatus = false;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("开关状态")]
        public bool SwitchStatus
        {
            get { return switchStatus; }
            set
            {
                switchStatus = value; this.Invalidate();

                //激活触发事件
                this.MouseDown_G?.Invoke(this, null);
            }
        }

        private string textLeft = "手动";
        [Browsable(true)]
        [Category("布局_G")]
        [Description("标签文本左边")]
        public string TextLeft
        {
            get { return textLeft; }
            set
            {
                textLeft = value; this.Invalidate();
            }
        }

        private string textRight = "自动";
        [Browsable(true)]
        [Category("布局_G")]
        [Description("标签文本右边")]
        public string TextRight
        {
            get { return textRight; }
            set
            {
                textRight = value; this.Invalidate();
            }
        }

        private bool textVisible = true;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("标签文本是否显示")]
        public bool TextVisible
        {
            get { return textVisible; }
            set
            {
                textVisible = value; this.Invalidate();
            }
        }

        private Color textColor = Color.Black;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("标签文本颜色")]
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; this.Invalidate(); }
        }

        private Font textFont = new Font("微软雅黑", 12);
        [Browsable(true)]
        [Category("布局_G")]
        [Description("标签文本字体")]
        public Font TextFont
        {
            get { return textFont; }
            set
            {
                textFont = value;
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
        private StringFormat sf;


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
            // this.height = this.width;

            //特殊情况处理
            if (cirInGap > 0.5f * this.width || cirInGap > 0.5f * this.height) return;
            if (cirOutGap > 0.5f * this.width || cirOutGap > 0.5f * this.height) return;

            //更改绘图间隙
            if (textVisible)
            {
                this.cirOutGap = 27.0f;
                this.cirInGap = 31.0f;
                this.cirOutWidth = 3.0f;
                this.togWidth = 10.0f;
                this.togGap = 20.0f;
                this.togForeHeight = 15.0f;
                this.togForeGap = 3.0f;
            }else
            {
                this.cirOutGap = 15.0f;
                this.cirInGap = 19.0f;
                this.cirOutWidth = 3.0f;
                this.togWidth = 15.0f;
                this.togGap = 6.0f;
                this.togForeHeight = 20.0f;
                this.togForeGap = 4.0f;
            }

            //获取原点
            Point centerPoint = GetCenterPoint();

            //绘制外环—(Pen)-DrawEllipse
            p = new Pen(this.cirInColor, this.cirOutWidth);
            RectangleF rec = new RectangleF(this.cirOutGap, this.cirOutGap, (centerPoint.X - this.cirOutGap) * 2, (centerPoint.X - this.cirOutGap) * 2);
            g.DrawEllipse(p, rec);

            //填充内圆—(SolidBrush)-FillEllipse
            sb = new SolidBrush(this.cirInColor);
            rec = new RectangleF(this.cirInGap, this.cirInGap, (centerPoint.X - this.cirInGap) * 2, (centerPoint.X - this.cirInGap) * 2);
            g.FillEllipse(sb, rec);

            if (textVisible)
            {
                //绘制文本
                rec = new RectangleF(this.width * 0.05f, 1, this.width, 20);
                g.DrawString(this.textLeft, this.textFont, new SolidBrush(this.textColor), rec, sf);
                rec = new RectangleF(this.width * 0.63f, 1, this.width, 20);
                g.DrawString(this.textRight, this.textFont, new SolidBrush(this.textColor), rec, sf);
            }

            //更改坐标系原点
            g.TranslateTransform(centerPoint.X, centerPoint.Y);

            //旋转指定角度
            if (switchStatus)
            {
                g.RotateTransform(36.0f);
            }
            else
            {
                g.RotateTransform(-36.0f);
            }

            //填充矩形开关
            rec = new RectangleF(-this.togWidth * 0.5f, this.togGap - centerPoint.Y, togWidth, (centerPoint.Y - togGap) * 2);
            g.FillRectangle(new SolidBrush(this.togColor), rec);

            //填充开关圆点
            rec = new RectangleF(-this.togWidth * 0.5f + togForeGap, this.togGap - centerPoint.Y + togForeGap, togWidth - 2 * togForeGap, togForeHeight);
            g.FillEllipse(new SolidBrush(this.togForeColor), rec);
        }

        #endregion

        #region 获取中心原点
        private Point GetCenterPoint()
        {
            if (this.height > this.width)
            {
                return new Point(this.width / 2, this.width / 2);
            }
            else
            {
                return new Point(this.height / 2, this.height / 2);
            }
        }
        #endregion

        #region 添加事件
        [Browsable(true)]
        [Category("操作_G")]
        [Description("双击进入事件")]
        public event EventHandler MouseDown_G;   //事件声明

        //更改属性
        //点击控件，变量也变，就是不绘制，变量的取反得用属性（不能用字段Private）-只有属性public更改才会触发事件）
        private void Switch_MouseDown(object sender, MouseEventArgs e)
        {
            //DialogResult dr = MessageBox.Show("二次确认操作？", "提示您", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (dr == DialogResult.OK)
            //{
                SwitchStatus = !SwitchStatus;
            //}
            //else return;
        }
        #endregion
    }

}
