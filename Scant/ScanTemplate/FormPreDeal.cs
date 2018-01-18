using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MovetoCTL;
using ARTemplate;
using System.Drawing.Imaging;
using Tools;

namespace ScanTemplate
{
    public partial class FormPreDeal : Form
    {
        public FormPreDeal( ScanConfig _sc)
        {
            InitializeComponent();
            this._sc = _sc;
            _activedir = null;
            _fs = null;
            _ActivePath = "";
            _ScanDataMode = false;
            zoombox = new ZoomBox();
        }        
        public  void Clear()
        {
            if (_fs != null)
            {
                _fs.Close();
                _fs = null;
            }
        }
        
        private void FormPreDeal_Load(object sender, EventArgs e)
        {
            if (_sc != null)
            {
                listBoxUnScanDir.Items.Clear();
                listBoxUnScanDir.Items.AddRange(_sc.Unscans.Unscans.ToArray());
                listBoxScantData.Items.Clear();
                listBoxScantData.Items.AddRange(_sc.Scandatas.Scandatas.ToArray());
            }
        }
        private void listBoxScantData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxScantData.SelectedIndex == -1) return;
            _activescandata = (ScanData)listBoxScantData.SelectedItem;
            _ScanDataMode = true;
            _ActivePath = _activescandata.Imgpath;
            //List<string> nameList = _activedir.ImgList();
            List<string> nameList = _activescandata.ImgList;
            if (nameList.Count > 0)
            {
                //string str = string.Join("\r\n", nameList);
                nameList = nameList.Select(r => r.Substring(_activescandata.Imgpath.Length)).ToList();
                listBoxfilename.Items.Clear();
                listBoxfilename.Items.AddRange(nameList.ToArray());
                listBoxNewfilename.Items.Clear();
            }
        }
        private void listBoxUnScanDir_SelectedIndexChanged(object sender, EventArgs e)
        {
             if (listBoxUnScanDir.SelectedIndex == -1) return;
            _activedir = (UnScan)listBoxUnScanDir.SelectedItem;
            _ActivePath = _activedir.FullPath;
            _ScanDataMode = false;

            List<string> nameList = _activedir.ImgList();
            if (nameList.Count > 0)
            {
                //string str = string.Join("\r\n", nameList);
                nameList = nameList.Select(r => r.Substring(  _ActivePath.Length)).ToList();
                listBoxfilename.Items.Clear();
                listBoxfilename.Items.AddRange(nameList.ToArray());
                listBoxNewfilename.Items.Clear();
            }
        }
        private void listBoxfilename_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxfilename.SelectedIndex == -1) return;
            if (_ActivePath == "" || !Directory.Exists(_ActivePath)) return;             
            string filename = _ActivePath + listBoxfilename.SelectedItem.ToString();
            if (_fs != null)
            {
                _fs.Close();
            }
            _fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap src = (Bitmap)System.Drawing.Image.FromStream(_fs);
            SetPictureImage(src );
            _fs.Close();
            _fs = null;
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
                File.Move(_ActivePath   + srclst[i],_ActivePath   + dstlst[i]);
            }
            // Refresh
            if (_ScanDataMode)
                listBoxScantData.SelectedItem = _activescandata;
            else
                listBoxUnScanDir.SelectedItem = _activedir;
        }
        private List<string> GetListBoxNameList(ListBox listBox)
        {
            List<string> lst = new List<string>();
            for (int i = 0; i < listBox.Items.Count; i++)
                lst.Add(listBox.Items[i].ToString());
            return lst;
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }
        private void pictureBox1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            double f = 0.0;
            if (numberOfTextLinesToMove > 0)
            {
                for (int i = 0; i < numberOfTextLinesToMove; i++)
                {
                    f += 0.05;
                }
                Zoomrat(f + 1, e.Location);
            }
            else if (numberOfTextLinesToMove < 0)
            {
                for (int i = 0; i > numberOfTextLinesToMove; i--)
                {
                    f -= 0.05;
                }
                Zoomrat(f + 1, e.Location);
            }
        }       
        private void Zoomrat(double rat, Point e)
        {
            Bitmap bitmap_show = (Bitmap)pictureBox1.Image;
            Point L = pictureBox1.Location;
            Point S = panel1.AutoScrollPosition;
            int w = (int)(pictureBox1.Width * rat);
            int h = w * bitmap_show.Height / bitmap_show.Width;
            L.Offset((int)(e.X * (rat - 1)), (int)(e.Y * (rat - 1)));
            pictureBox1.SetBounds(S.X, S.Y, w, h);
            zoombox.UpdateBoxScale(pictureBox1);

            S.Offset((int)(e.X * (1 - rat)), (int)(e.Y * (1 - rat)));
            panel1.Invalidate();
            panel1.AutoScrollPosition = new Point(-S.X, -S.Y);
        }
       
        private void SetPictureImage(Bitmap image)
        {
            pictureBox1.Image = ConvertFormat.Convert(image, PixelFormat.Format1bppIndexed, false);
            Bitmap bitmap_show = (Bitmap)pictureBox1.Image;
            crop_startpoint.X = crop_startpoint.Y = 0;
            int width = pictureBox1.Height * bitmap_show.Width / bitmap_show.Height;
            int height = pictureBox1.Width * bitmap_show.Height / bitmap_show.Width;
            if (pictureBox1.Height > height)
                pictureBox1.Height = height;
            if (pictureBox1.Width > width)
                pictureBox1.Width = width;
            // pictureBox1.Image = bitmap_show;
            zoombox.UpdateBoxScale(pictureBox1);
            _OriginWith = zoombox.ImageWith(pictureBox1);
            pictureBox1.Invalidate();
        }
        
        private ScanConfig _sc;
        private UnScan _activedir;
        private ScanData _activescandata;
        private FileStream _fs;
        private string _ActivePath;
        private bool _ScanDataMode;

        private ZoomBox zoombox;
        private Point crop_startpoint;
        private double _OriginWith;

        private void buttonPredealImage_Click(object sender, EventArgs e)
        {
            if (!_ScanDataMode)
            {
                this.Hide();
                FormPreDealImage f = new FormPreDealImage(_ActivePath, GetListBoxNameList(listBoxfilename));
                f.ShowDialog();
                this.Show();
            }
        }

    }
}
