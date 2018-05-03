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
    public partial class FormChooseResult : Form
    {
        private string _result;
        public FormChooseResult()
        {
            InitializeComponent();
            _result = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _result = "exresult";
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            _result = "eximage";
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            _result = "exresultfx";
            this.Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            _result = "exother";
            this.Close();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            _result = "exonlyoption";
            this.Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            _result = "exonlyoptionimages";
            this.Close();
        }
        public string Result { get { return _result; } }

    }
}
