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
    public partial class FormPreDeal : Form
    {
        private ScanConfig _sc;
        private UnScan _activedir;
        public FormPreDeal( ScanConfig _sc)
        {
            InitializeComponent();
            this._sc = _sc;
            _activedir = null;
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
            _activedir = (UnScan)listBoxUnScanDir.SelectedItem;

            List<string> nameList = _activedir.ImgList();
            if (nameList.Count > 0)
            {
                //string str = string.Join("\r\n", nameList);
                nameList = nameList.Select(r => r.Substring(_activedir.FullPath.Length)).ToList();
                listBoxfilename.Items.Clear();
                listBoxfilename.Items.AddRange(nameList.ToArray());
                listBoxNewfilename.Items.Clear();
            }
        }

        private void listBoxfilename_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxfilename.SelectedIndex == -1 || _activedir==null) return;
            string filename =_activedir.FullPath+ listBoxfilename.SelectedItem.ToString();
            pictureBox1.Image = Bitmap.FromFile(filename);

        }
        private void buttonModifyNewFilename_Click(object sender, EventArgs e)
        {
            string src = textBoxfilenamereplacesrc.Text;
            string dst = textBoxfilenamereplacedst.Text;
            List<string> srclst = GetListBoxNameList(listBoxNewfilename);
            if(  srclst.Count != listBoxfilename.Items.Count)
                srclst = GetListBoxNameList(listBoxfilename);
            listBoxNewfilename.Items.Clear();
            listBoxNewfilename.Items.AddRange(
                srclst.Select(r => r.Replace(src, dst)).ToArray());
        }
        private void buttonReName_Click(object sender, EventArgs e)
        {
            List<string> srclst = GetListBoxNameList(listBoxfilename);
            List<string> dstlst = GetListBoxNameList(listBoxNewfilename);
            if (srclst.Count != dstlst.Count || _activedir== null)
            {
                MessageBox.Show("不能重命名");
                return;
            }
            for (int i = 0; i < srclst.Count; i++)
            {
                File.Move(_activedir.FullPath   + srclst[i], _activedir.FullPath   + dstlst[i]);
            }
            // Refresh
            listBoxUnScanDir.SelectedItem = _activedir;
        }
        public List<string> GetListBoxNameList(ListBox listBox)
        {
            List<string> lst = new List<string>();
            for (int i = 0; i < listBox.Items.Count; i++)
                lst.Add(listBox.Items[i].ToString());
            return lst;
        }

    }
}
