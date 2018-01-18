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
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            zoombox = new ZoomBox();
            _ListFeature = new List<Rectangle>();
        }        
        public  void Clear()
        {
            if (_fs != null)
            {
                _fs.Close();
                _fs = null;
            }
           ;
        }
        private void Reset()
        {
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            _OriginWith = pictureBox1.Width;
            zoombox.Reset();
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
            SetImage(src );
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
            //if (m_act == Act.ZoomMouse)
                pictureBox1.Focus();
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            if (MT != null && MT.Selection.Width > 3)
            {
                CompleteSelection(true);
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                if (m_Imgselection.Width > 0 && m_Imgselection.Height > 0)
                {
                    Pen pen = Pens.Red;
                    Rectangle r = zoombox.ImgToBoxSelection(m_Imgselection);
                    e.Graphics.DrawRectangle(pen, r);

                    foreach (Rectangle fr in _ListFeature)
                    {
                        Rectangle nr = zoombox.ImgToBoxSelection(fr);
                        e.Graphics.DrawRectangle(pen, nr);
                    }
                    textBoxOut.AppendText(".");
                }
            }
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
        private void CompleteSelection(bool bcomplete)
        {
            if (bcomplete)
            {
                //ShowMessage("Complete: " + m_act);
                m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                MT.StartDraw(false);
                //switch (m_act) { }
            }
            pictureBox1.Invalidate();
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
        private void ReSetPictureBoxImage()
        {
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
            _ListFeature.Clear();
            m_Imgselection.Width = 0;
        }
        private void SetImage(Bitmap image)
        {
            //image.Save("F:\\setimage.tif");
            pictureBox1.Image = image;
            ReSetPictureBoxImage();
            MT = new MovetoCTL.MovetoTracker(pictureBox1);
            MT.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            MT.StartDraw(false);
        }
        private void buttonSelectionBlack_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)pictureBox1.Image;
            if (src != null && m_Imgselection.Width > 0 && m_Imgselection.Height > 0)
            {
                Bitmap rgb = ConvertFormat.ConvertToRGB(src);
              
                using (Graphics g = Graphics.FromImage(rgb))
                {
                    g.FillRectangle(Brushes.Black,m_Imgselection);
                }
                pictureBox1.Image = rgb;
            }
        }
        private void buttonOutRectWhite_Click(object sender, EventArgs e)
        {
            Bitmap src =(Bitmap) pictureBox1.Image;
            if (src != null && m_Imgselection.Width > 0 && m_Imgselection.Height > 0)
            {
                Bitmap rgb = ConvertFormat.ConvertToRGB(src);
                List<Rectangle> yuji = GetYuji(new Rectangle(0, 0, src.Width, src.Height), m_Imgselection);
                if(yuji.Count>0)
                using (Graphics g = Graphics.FromImage(rgb ))
                {
                    g.FillRectangles(Brushes.White, yuji.ToArray());
                }
                pictureBox1.Image = rgb;
            }
        }
        private List<Rectangle> GetYuji(Rectangle A, Rectangle B)
        {
            List<Rectangle> lst = new List<Rectangle>();
            Rectangle Bi = B;
            Bi.Intersect(A);
            if (Bi.Width == 0 || Bi.Height == 0)
                lst.Add(A);
            else
            {
                //T
                if (A.Top < Bi.Top)
                    lst.Add(new Rectangle(A.X, A.Y, A.Width, Bi.Top - A.Top));
                //B
                if(A.Bottom > Bi.Bottom)
                    lst.Add( new Rectangle( A.X,Bi.Bottom,A.Width, A.Bottom-Bi.Bottom));
                //L
                if(Bi.X> A.X)
                    lst.Add( new Rectangle( A.X,Bi.Top,Bi.X - A.X+1,Bi.Height));
                //R
                if(A.Right > Bi.Right)
                    lst.Add( new Rectangle( B.Right,B.Top, A.Right-Bi.Right, Bi.Height));
            }
            return lst;
        }
        private void buttonCutbySelection_Click(object sender, EventArgs e)
        {
            if (m_Imgselection.Width > 0 && m_Imgselection.Height > 0 && pictureBox1.Image != null
                && m_Imgselection.Width < pictureBox1.Image.Width && m_Imgselection.Height < pictureBox1.Image.Height)
            {
                Bitmap src = (Bitmap)pictureBox1.Image;
                SetImage(src.Clone(m_Imgselection, pictureBox1.Image.PixelFormat));
                m_Imgselection.Location = new Point(0, 0);
            }
        }
        private void buttonSaveActiveImage_Click(object sender, EventArgs e)
        {
            if (  m_Imgselection.Width == 0 || m_Imgselection.Height == 0) return;
            if (listBoxfilename.SelectedIndex == -1  || _ActivePath=="" ||!Directory.Exists(_ActivePath)) return;
            string filename =_ActivePath + listBoxfilename.SelectedItem.ToString();

            Bitmap rgb = (Bitmap)pictureBox1.Image;
            rgb = ConvertFormat.Convert(rgb, PixelFormat.Format1bppIndexed, false);
            if (_fs != null)
            {
                _fs.Close();
            }
            rgb.Save(filename);
            //
            _fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap src = (Bitmap)System.Drawing.Image.FromStream(_fs);
            SetImage(src);
        }
        private void buttonApplyAll_Click(object sender, EventArgs e)
        {
            if (listBoxfilename.SelectedIndex == -1 || _ActivePath == "" || !Directory.Exists(_ActivePath)
                || m_Imgselection.Width==0 || m_Imgselection.Height==0 ) return;
       
            List<string> srclst = GetListBoxNameList(listBoxfilename);
            
            System.IO.FileStream fs1 = new System.IO.FileStream(_ActivePath+srclst[0], System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap  src = (Bitmap)System.Drawing.Image.FromStream(fs1);
            List<Rectangle> yuji = GetYuji(new Rectangle(0, 0, src.Width, src.Height), m_Imgselection);
            fs1.Close();

            string msg = "";
            if (yuji.Count > 0)
            foreach( string s in srclst)
            {
                    string ss =_ActivePath + s;
                    try
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(ss, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        Bitmap orgsrc = (Bitmap)System.Drawing.Image.FromStream(fs);
                        yuji = GetYuji(new Rectangle(0, 0,orgsrc.Width, orgsrc.Height), m_Imgselection);
                        Bitmap rgb = ConvertFormat.ConvertToRGB(orgsrc);
                        using (Graphics g = Graphics.FromImage(rgb))
                        {
                            g.FillRectangles(Brushes.White, yuji.ToArray());
                        }
                        rgb = ConvertFormat.Convert(rgb, src.PixelFormat, false);
                        fs.Close();
                        rgb.Save(ss);
                    }
                    catch
                    {
                        msg += s + "\r\n";
                    }
            }
            if (msg != "")
                MessageBox.Show(msg);
        }
        private void buttonApplyCutToAllImage_Click(object sender, EventArgs e)
        {
            if (listBoxfilename.SelectedIndex == -1 || _activedir == null || _ActivePath == "" || !Directory.Exists(_ActivePath)
                || m_Imgselection.Width == 0 || m_Imgselection.Height == 0) return;

            List<string> srclst = GetListBoxNameList(listBoxfilename);

            System.IO.FileStream fs1 = new System.IO.FileStream(_ActivePath + srclst[0], System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap src = (Bitmap)System.Drawing.Image.FromStream(fs1);
            List<Rectangle> yuji = GetYuji(new Rectangle(0, 0, src.Width, src.Height), m_Imgselection);
            fs1.Close();

            if (yuji.Count > 0)
                foreach (string s in srclst)
                {
                    string ss =_ActivePath + s;
                    System.IO.FileStream fs = new System.IO.FileStream(ss, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    Bitmap orgsrc = (Bitmap)System.Drawing.Image.FromStream(fs);

                    Bitmap rgb = orgsrc.Clone(m_Imgselection, orgsrc.PixelFormat);
                    ss =_ActivePath+"\\img" + s;
                    rgb.Save(ss);
                    fs.Close();
                }
        }
        private float GetAngle()
        {
            float angle = 0;
            try
            {
                angle = (float)Convert.ToDouble(textBoxAngle.Text);
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
            return angle;
        }
        private void buttonAngle_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image = Tools.BitmapRotateTools.KiRotate((Bitmap)pictureBox1.Image, GetAngle(), Color.White);
            pictureBox1.Image = Tools.BitmapRotateTools.Rotate((Bitmap)pictureBox1.Image, GetAngle());
        }
        private void buttonTo2bpp_Click(object sender, EventArgs e)
        {
            Bitmap bin = ConvertFormat.Convert((Bitmap)pictureBox1.Image, PixelFormat.Format1bppIndexed, false);
            pictureBox1.Image = bin;
        }
        private void buttonSetSeletion_Click(object sender, EventArgs e)
        {
            if (MT != null)
                MT.StartDraw(true);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null || MT==null || m_Imgselection.Width==0 || m_Imgselection.Height==0) return;
            Bitmap _src = (Bitmap)pictureBox1.Image;
            _src = _src.Clone(m_Imgselection, _src.PixelFormat);
            try
            {
                DetectData dd = DetectImageTools.DetectImg(_src);
                dd = DetectImageTools.DetectCorrect.ReDetectCorrectImg(_src, dd);

                if (dd.CorrectRect.Width > 0)
                {
                    InitListFeature(dd);
                    List<Point> list = ListFeatureToPoints(dd);
                    _angle = new AutoAngle(list);
                    textBoxOut.AppendText("_angle" + _angle.Angle1);

                    double angle = -_angle.Angle1 * 180 / Math.PI;
                    textBoxAngle.Text = angle.ToString();
                    pictureBox1.Refresh();
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show("检测失败"+ ee.Message);
            }
        }

        private void InitListFeature(DetectData dd)
        {
            _ListFeature = dd.ListFeature.Select(r => { r.Offset(dd.CorrectRect.Location); r.Offset(m_Imgselection.Location); return r; }).ToList();
            Rectangle rr = dd.CorrectRect;
            rr.Offset(m_Imgselection.Location);
            _ListFeature.Add(rr);
        }
        private List<Point> ListFeatureToPoints(DetectData dd)
        {
            List<Point> list = dd.ListFeature.Select(r => new Point(r.X + m_Imgselection.X, r.Y + m_Imgselection.Y)).ToList();
            return list;
        }

        private Rectangle m_Imgselection;
        private MovetoTracker MT;
        private Point crop_startpoint;
        private ZoomBox zoombox;
        private double _OriginWith;
        private ScanConfig _sc;
        private FileStream _fs;
        private UnScan _activedir;
        private ScanData _activescandata;
        private bool  _ScanDataMode;
        private string _ActivePath;
        private AutoAngle _angle;
        private List<Rectangle> _ListFeature;
    }
}
