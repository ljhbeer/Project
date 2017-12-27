using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ScanTemplate
{
    public partial class FormChoose : Form
    {
        public FormChoose()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormM().ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormYJ.FormYJTools().ShowDialog();
            this.Close();
        }

        private void FormChoose_Load(object sender, EventArgs e)
        {

            if (File.Exists("tagcfg.ini"))
            {
                string s = File.ReadAllText("tagcfg.ini");
                try
                {
                    global.tag = Convert.ToInt32(s,2);
                }
                catch
                {
                }
            }
        }
    }

}
