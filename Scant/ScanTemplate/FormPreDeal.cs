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
    public partial class FormPreDeal : Form
    {
        private ScanConfig _sc;
        public FormPreDeal( ScanConfig _sc)
        {
            InitializeComponent();
            this._sc = _sc;
        }
        public  void Clear()
        {
           ;
        }

        private void FormPreDeal_Load(object sender, EventArgs e)
        {
            if (_sc != null)
            {
                listBoxUnScanDir.Items.Clear();
                listBoxUnScanDir.Items.AddRange(_sc.Unscans.Unscans.ToArray());
            }
        }

        private void listBoxUnScanDir_SelectedIndexChanged(object sender, EventArgs e)
        {
             if (listBoxUnScanDir.SelectedIndex == -1) return;
            UnScan dir = (UnScan)listBoxUnScanDir.SelectedItem;
            List<string> nameList = dir.ImgList();
            if (nameList.Count > 0)
            {
                //string str = string.Join("\r\n", nameList);
                listBoxfilename.Items.Clear();
                listBoxfilename.Items.AddRange(nameList.ToArray());
            }
        }

        private void listBoxfilename_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxfilename.SelectedIndex == -1) return;
            pictureBox1.Image = Bitmap.FromFile(listBoxfilename.SelectedItem.ToString());

        }

    }
}
