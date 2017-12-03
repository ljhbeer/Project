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
    public partial class FormShow : Form
    {
        public FormShow()
        {
            InitializeComponent();
        }
        internal void SetImg(Image cropimg)
        {
            pictureBox1.Image = cropimg;
        }
    }
}
