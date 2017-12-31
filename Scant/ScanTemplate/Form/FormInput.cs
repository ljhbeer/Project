using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARTemplate
{
    public partial class FormInput : Form
    {
        private string keyname;
        public string StrValue { get; set; }
        public int IntValue { get; set; }

        public FormInput(string keyname)
        {
            this.keyname = keyname;
            InitializeComponent();
            label2.Visible = textBox2.Visible = false;
            if (keyname == "考试名称")
            {
                label1.Text = "请输入本次考试名称";
                textBox1.Text = "";
            }
            else if (keyname == "校对")
            {
                label1.Visible = textBox1.Visible = false;
                comboBox1.Visible = true;
                comboBox1.Location = textBox1.Location;
                if(comboBox1.Items.Count>0)
                comboBox1.SelectedIndex = 0;
            }
            else if (keyname == "选择题")
            {
                label1.Text = "请输入选择题的个数";
                textBox1.Text = "5";
            }
            else if (keyname == "非选择题")
            {
                label1.Text = "请输入每个空的分值，必须大于0,也可以以后统一输入";
                textBox1.Text = "2";
            }
            else if (keyname == "自定义")
            {
                label1.Text = "请输入自定义名称";
                textBox1.Text = "座位号";
                label2.Text = "输入选项数";
                textBox2.Text = "10";
                label2.Visible = textBox2.Visible = true; ;
            }
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (keyname == "考试名称")
            {
                StrValue =  textBox1.Text;
            }
            else if (keyname == "校对")
            {
                if (comboBox1.SelectedIndex == -1)
                    StrValue = "";
                else 
                    StrValue = comboBox1.SelectedItem.ToString();
            }
            else if (keyname == "选择题" || keyname == "非选择题")
            {
                try
                {
                    IntValue = Convert.ToInt32(textBox1.Text);
                }
                catch
                {
                    IntValue = -1;
                }
            }
            else if (keyname == "自定义")
            {
                try
                {
                    StrValue = textBox1.Text.Trim();
                    IntValue = Convert.ToInt32(textBox2.Text);
                }
                catch
                {
                    IntValue = -2;
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        private void FormInput_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            //textBox1.SelectAll();
        }
    }
}
