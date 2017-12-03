using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Camera
{
    public partial class ShowForm : Form
    {
        public ShowForm()
        {
            InitializeComponent();
        }
        public void ShowImg(Image img){
            pictureBox1.Image=img;
            ShowDialog();
        }
    }
}
