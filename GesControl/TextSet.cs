using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp_自定义控件
{
    public partial class TextSet : UserControl
    {
        public TextSet()
        {
            InitializeComponent();
            this.txt_Value.ReadOnly = true;
        }

        #region 属性  字体、标签、值、单位

        private Font textFont = new Font("微软雅黑", 12);
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
                    this.lbl_Title.Font = this.lbl_Unit.Font = this.txt_Value.Font = textFont;
                }
            }
        }

        private Color textColor = Color.Black;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("文本颜色")]
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                this.lbl_Title.ForeColor = this.lbl_Unit.ForeColor = this.txt_Value.ForeColor = textColor;
            }
        }

        private float textScale = 0.37f;
        [Browsable(true)]
        [Category("布局_G")]
        [Description("控件刻度")]
        public float TextScale
        {
            get { return textScale; }
            set
            {
                textScale = value;
                this.tableLayoutPanel1.ColumnStyles[0].Width = (this.Width - textScale * this.Width) * 0.75f;
                this.tableLayoutPanel1.ColumnStyles[1].Width = textScale * this.Width;
                this.tableLayoutPanel1.ColumnStyles[2].Width = (this.Width - textScale * this.Width) * 0.25f;
            }
        }

        private string varTitle = "变量名称";
        [Browsable(true)]
        [Category("布局_G")]
        [Description("变量名称")]
        public string VarTitle
        {
            get { return varTitle; }
            set
            {
                varTitle = value;
                this.lbl_Title.Text = varTitle;
            }
        }

        private string varValue = "21.50";
        [Browsable(true)]
        [Category("布局_G")]
        [Description("输入值")]
        public string VarValue
        {
            get { return varValue; }
            set
            {
                varValue = value;
                this.txt_Value.Text = varValue;
            }
        }

        private string varUnit = "℃";
        [Browsable(true)]
        [Category("布局_G")]
        [Description("单位")]
        public string VarUnit
        {
            get { return varUnit; }
            set
            {
                varUnit = value;
                this.lbl_Unit.Text = varUnit;
            }
        }

        #endregion

        #region  输入使能事件

        //正在输入标志位
        public bool IsSetting { get; set; }

        private void txt_Value_Enter(object sender, EventArgs e)
        {
            IsSetting = true;
            this.txt_Value.ReadOnly = false;
        }

        private void txt_Value_Leave(object sender, EventArgs e)
        {
            IsSetting = false;
            this.txt_Value.ReadOnly = true;
        }

        //添加输入完成事件
        public event EventHandler SettingChanged;

        private void txt_Value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //技巧：输入完成移动焦点~输入框变灰
                this.lbl_Title.Focus();

                //激活触发事件
                SettingChanged?.Invoke(this, e);
            }
        }

        #endregion

    }
}