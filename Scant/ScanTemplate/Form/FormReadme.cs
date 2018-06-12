using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScanTemplate
{
    public partial class FormReadme : Form
    {
        public FormReadme()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void SetText(string s)
        {
            textBox1.Text = s;
        }
    }
}
