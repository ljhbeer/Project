using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBQuery
{
    public partial class FormShow : Form
    {
        public FormShow(List<string> tablesname)
        {
            if(tablesname.Count==0) return;
            InitializeComponent();
            rbss = new List<RadioButton>();
            rbcs = new List<RadioButton>();         
            if (tablesname.Count > 0)
            {
                for (int i = 0; i < tablesname.Count; i++)
                {
                    TextBox tb = new TextBox();
                    RadioButton rbs = new RadioButton();
                    RadioButton rbc = new RadioButton();
                    FlowLayoutPanel fp = new FlowLayoutPanel();
                    rbcs.Add(rbc);
                    tb.Text = tablesname[i];                              
                    tb.Size = new System.Drawing.Size(170, 21);
                    tb.ReadOnly = true;
                    tb.TextAlign = HorizontalAlignment.Left;
                    tb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
                    rbs.Text = "结构";
                    rbc.Text = "结构和内容";
                    rbs.Checked = true;
                    fp.Size = new Size(440,25);
                    fp.Controls.Add(tb);
                    fp.Controls.Add(rbc);
                    fp.Controls.Add(rbs);
                    flowLayoutPanel1.Controls.Add(fp);
                }
            }
        }
        public List<bool> GetResult()
        {
            List<bool> ret = new List<bool>();
            foreach (RadioButton r in rbcs)
                if (r.Checked)
                    ret.Add(true);
                else
                    ret.Add(false);
            return ret;
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        List<RadioButton> rbss;
        List<RadioButton> rbcs;
    }
}
