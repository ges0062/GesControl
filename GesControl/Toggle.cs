using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_自定义控件
{
    //指定默认事件（双击控件进入）
    [DefaultEvent("MouseDown_G")]

    public partial class Toggle : UserControl
    {
        public Toggle()
        {
            InitializeComponent();

            //设置控件样式
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); //忽略窗口消息减少闪烁
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            //添加鼠标点击事件处理
            this.MouseDown += Toggle_MouseDown;

        }

        #region 属性——选中、背景颜色、文本、样式、滑块颜色

        private Font displayFont = new Font("Segoe UI", 12);
        [Browsable(true)]
        [Category("布局_G")]
        [Description("字体格式")]
        public Font DisplayFont
        {
            get { return displayFont; }
            set
            {
                if (value != null)
                {
                    displayFont = value;
                    this.Invalidate(); // Trigger redraw
                }
            }
        }

        private bool _checked = false;
        [Browsable(true)]  //说明（需放在属性前边）：是否选中
        [Category("布局_G")]
        [Description("是否选中")]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                this.Invalidate();

                //激活触发事件
                this.MouseDown_G?.Invoke(this, null);

            }
        }

        private Color falseColor = Color.FromArgb(180, 180, 180);
        [Browsable(true)]  //说明：未选中背景色
        [Category("布局_G")]
        [Description("未选中背景色")]
        public Color FalseColor
        {
            get { return falseColor; }
            set { falseColor = value; this.Invalidate(); }
        }

        private Color trueColor = Color.FromArgb(73, 119, 232);
        [Browsable(true)]  //说明：选中背景色
        [Category("布局_G")]
        [Description("选中背景色")]
        public Color TrueColor
        {
            get { return trueColor; }
            set { trueColor = value; this.Invalidate(); }
        }

        private string falseText = "关闭";
        [Browsable(true)]  //说明：文本关闭
        [Category("布局_G")]
        [Description("文本关闭")]
        public string FalseText
        {
            get { return falseText; }
            set { falseText = value; this.Invalidate(); }
        }

        private string trueText = "打开";
        [Browsable(true)]  //说明：文本打开
        [Category("布局_G")]
        [Description("文本打开")]
        public string TrueText
        {
            get { return trueText; }
            set { trueText = value; this.Invalidate(); }
        }

        //样式切换
        public enum SwType
        {
            Ellipse,    //椭圆
            Rectangle,  //矩形
        }

        private SwType switchType = SwType.Ellipse;
        [Browsable(true)]  //说明：切换样式
        [Category("布局_G")]
        [Description("切换样式")]
        public SwType SwitchType
        {
            get { return switchType; }
            set { switchType = value; this.Invalidate(); }
        }

        private Color sliderColor = Color.White; //Color.White
        [Browsable(true)]  //说明：滑块颜色
        [Category("布局_G")]
        [Description("滑块颜色")]
        public Color SliderColor
        {
            get { return sliderColor; }
            set { sliderColor = value; this.Invalidate(); }
        }

        #endregion

        #region 画布——矩形、椭圆、滑块、文本

        private Graphics graphics;
        private int width;
        private int height;

        //矩形绘制
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //画布质量
            graphics = e.Graphics;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.width = this.Width;
            this.height = this.Height;

            if (this.switchType == SwType.Rectangle)  //字段选择为矩形时
            {
                //填充色
                Color fillColor = this._checked ? trueColor : falseColor;

                //带四角圆弧的矩形
                GraphicsPath path = new GraphicsPath();
                int diameter = 10;  //默认圆弧直径
                //左上角圆弧：起始坐标，宽，高，开始角度，扫描角度
                path.AddArc(0, 0, diameter, diameter, 180f, 90f);
                path.AddArc(this.width - diameter, 0, diameter, diameter, 270f, 90f);  //右上角
                path.AddArc(this.width - diameter, this.height - diameter, diameter, diameter, 0f, 90f);  //右下角
                path.AddArc(0, this.height - diameter, diameter, diameter, 90f, 90f);  //左下角
                graphics.FillPath(new SolidBrush(fillColor), path);  //填充色

                //文本
                string strText = this._checked ? trueText : falseText;

                //滑块(true\false 两种形态)
                if (_checked)
                {
                    //绘制滑块
                    path = new GraphicsPath();
                    int sliderwidth = this.height - 4;
                    path.AddArc(this.width - sliderwidth - 2, 2, diameter, diameter, 180f, 90f);
                    path.AddArc(this.width - diameter - 2, 2, diameter, diameter, 270f, 90f);
                    path.AddArc(this.width - diameter - 2, this.height - diameter - 2, diameter, diameter, 0f, 90f);
                    path.AddArc(this.width - sliderwidth - 2, this.height - diameter - 2, diameter, diameter, 90f, 90f);
                    graphics.FillPath(new SolidBrush(sliderColor), path);

                    //绘制文本
                    Rectangle rec = new Rectangle(0, 0, this.width - sliderwidth - 2, this.height);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    graphics.DrawString(strText, DisplayFont, new SolidBrush(sliderColor), rec, sf);

                }
                else
                {
                    //绘制滑块
                    path = new GraphicsPath();
                    int sliderwidth = this.height - 4;
                    path.AddArc(2, 2, diameter, diameter, 180f, 90f);
                    path.AddArc(sliderwidth - diameter + 2, 2, diameter, diameter, 270f, 90f);
                    path.AddArc(sliderwidth - diameter + 2, sliderwidth - diameter + 2, diameter, diameter, 0f, 90f);
                    path.AddArc(2, sliderwidth - diameter + 2, diameter, diameter, 90f, 90f);
                    graphics.FillPath(new SolidBrush(sliderColor), path);

                    //绘制文本
                    Rectangle rec = new Rectangle(sliderwidth + 2, 0, this.width - sliderwidth - 2, this.height);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    graphics.DrawString(strText, DisplayFont, new SolidBrush(sliderColor), rec, sf);
                }
            }
            else if (this.switchType == SwType.Ellipse)  //字段选择为椭圆时
            {
                //填充色
                Color fillColor = this._checked ? trueColor : falseColor;

                //带四角圆弧的椭圆形
                GraphicsPath path = new GraphicsPath();

                path.AddArc(1, 1, this.height - 2, this.height - 2, 90f, 180f);
                path.AddArc(this.width - (this.height - 2) - 1, 1, this.height - 2, this.height - 2, 270f, 180f);
                graphics.FillPath(new SolidBrush(fillColor), path);  //填充色

                //文本
                string strText = this._checked ? TrueText : falseText;

                //滑块(true\false 两种形态)
                if (_checked)
                {
                    //绘制滑块
                    int ciclewidth = this.height - 6;
                    graphics.FillEllipse(new SolidBrush(sliderColor), new Rectangle(this.width - ciclewidth - 3, 3, ciclewidth, ciclewidth));

                    //绘制文本
                    Rectangle rec = new Rectangle(0, 0, this.width - ciclewidth - 3, this.height);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    graphics.DrawString(strText, DisplayFont, new SolidBrush(sliderColor), rec, sf);
                }
                else
                {
                    //绘制滑块
                    int ciclewidth = this.height - 6;

                    graphics.FillEllipse(new SolidBrush(sliderColor), new Rectangle(3, 3, ciclewidth, ciclewidth));

                    //绘制文本
                    Rectangle rec = new Rectangle(ciclewidth + 3, 0, this.width - ciclewidth - 3, this.height);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    graphics.DrawString(strText, DisplayFont, new SolidBrush(sliderColor), rec, sf);
                }
            }
        }

        #endregion

        #region 事件——鼠标按下进入属性激活触发事件

        [Browsable(true)]
        [Category("操作_G")]
        [Description("点击进入事件")]
        public event EventHandler MouseDown_G;   //鼠标点击事件声明
        //初始化添加事件处理后自动生成此方法（无需在控件的属性中双击）
        //2024.9.1（发现一个错误，当应用该控件时，From中点击无效；经过两天的查询（断点查询）发现问题在Form的设计器中的Enable为False
        //原因：控件做了没有及时生成导致）
        //2024.9.7（发现一个错误，点击控件，变量也变，就是不绘制，变量的取反得用属性（不能用字段））
        private void Toggle_MouseDown(object sender, MouseEventArgs e)
        {
            DialogResult dr = MessageBox.Show("二次确认操作？", "提示您", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Checked = !Checked;
            }
            else return;
        }

        #endregion


    }
}
