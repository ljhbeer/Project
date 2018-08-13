using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EncyptTools;

namespace SoftRegTools
{
    public partial class FormRegShow : Form
    {
        public FormRegShow(RegHelper regHelper)
        {           
            InitializeComponent();
            _reghelper = regHelper;
            textBoxReadMe.Text = @"注册购买后您将得到
1、无任何功能和次数限制的完全版本
2、免费升级和相应的技术支持以及购买其他产品的优惠

[晶华教师个人阅卷系统]  本软件为收费软件
  首次注册：免费/半年
  推广期间：半价优惠，即100元/年
  注册费用：200元/年

你可以通过淘宝商城，支付宝，微信转账的方式进行购买
软件注册服务电话：15827880192
注册服务邮箱：ljh_beer@163.com
在线购买方式1： 晶华软件淘宝店（淘宝网-暂未开通）
在线购买方式2： 添加本人的支付宝，或者微信进行购买";
            textBoxJG.Text = @"警告: 本计算机程序受著作权法和国际公约的保护，未经授权擅自复制或散布本程序的部分或全部，将承受严厉的民事和刑事处罚，对已知的违反者将给予法律范围内的全面制裁。";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxMachineCode.Text = SoftRegisterTools.getMachineNum();
            if (_reghelper.CheckReged())
            {
                textBoxRegstate.Text = "已注册，有效期为" + _reghelper.ExpirDate.ToShortDateString();
                textBoxRegCodeHash.Visible = false;
                buttonReg.Visible = false;
                buttonClose.Text = "确定";
            }
            else
            {
                textBoxRegstate.Text = "没有注册或者已过期，首次注册半年内免费哦";
            }
        }
       
        private void buttonReg_Click(object sender, EventArgs e)
        {            
            if (_reghelper.TryRegHash(textBoxRegCodeHash.Text))
            {
                if (!ScanTemplate.global.LocReghelper.TryRegHash(textBoxRegCodeHash.Text))
                {
                    MessageBox.Show("逻辑错误");
                }
                textBoxRegstate.Text = "你已经成功注册，有效期为" + _reghelper.ExpirDate.ToShortDateString();
                MessageBox.Show("你已经成功注册，有效期为" + _reghelper.ExpirDate.ToShortDateString());
                buttonClose.Text = "确定";
                buttonReg.Visible = false;
            }
            else
            {
                MessageBox.Show("注册失败，原因是" + _reghelper.ExceptMsg);
            }  
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private RegHelper _reghelper;
    }
}
