using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp_自定义控件
{
    //指定默认事件（双击控件进入）
    [DefaultEvent("TextShowClick")]
    public partial class TextShow : UserControl
    {
        public TextShow()
        {
            InitializeComponent();
        }

        #region Fields 变量名称、变量值、单位、字体、控件刻度
        //[Browsable(true)]
        //[Category("布局_G")]
        //[Description("变量名称")]
        //public String VarName { get; set; }


        private Font textFont = new Font("Segoe UI Variable Display", 15, FontStyle.Bold);
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
                    this.lbl_Value.Font = this.lbl_Unit.Font = textFont;
                }
            }
        }

        private Color textColor = Color.Blue;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("文本颜色")]
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                this.lbl_Value.ForeColor = this.lbl_Unit.ForeColor = textColor;
            }
        }

        private string varValue = "1.0E-5";
        [Browsable(true)]
        [Category("布局_G")]
        [Description("变量值")]
        public string VarValue
        {
            get { return varValue; }
            set
            {
                varValue = value;
                this.lbl_Value.Text = varValue;
            }
        }

        private string unit = "Pa";
        [Browsable(true)]
        [Category("布局_G")]
        [Description("单位")]
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                this.lbl_Unit.Text = unit;
            }
        }

        private float textScale = 0.6f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("控件刻度")]
        public float TextScale
        {
            get { return textScale; }
            set
            {
                textScale = value;
                this.tableLayoutPanel1.ColumnStyles[0].Width = textScale * this.Width;
                this.tableLayoutPanel1.ColumnStyles[1].Width = this.Width - textScale * this.Width;
            }
        }

        #endregion

        #region Event 鼠标双击控件后进入自定义事件
        //创建委托—事件
        public delegate void BtnClickDelegate(object sender, EventArgs e);
        [Browsable(true)]
        [Category("操作_G")]
        [Description("文本双击触发事件")]
        public event BtnClickDelegate TextShowClick;

        private void Lbl_Value_DoubleClick(object sender, EventArgs e)
        {
            TextShowClick?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}
