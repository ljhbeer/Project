using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MovetoCTL;
using ARTemplate;
using System.IO;
using Tools;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;

namespace ScanTemplate
{
    public partial class FormPreDealImage : Form
    {
        public FormPreDealImage(string _ActivePath, List<string> list)
        {
            InitializeComponent();
            this._ActivePath = _ActivePath;
            this.list = list;
            _fs = null;
            _ListFeature = new List<Rectangle>();
            zoombox = new ZoomBox();
            listBoxfilename.Items.AddRange(list.ToArray());
        }

        private void buttonSetSeletion_Click(object sender, EventArgs e)
        {
            if (MT != null)
                MT.StartDraw(true);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null || MT == null || m_Imgselection.Width == 0 || m_Imgselection.Height == 0) return;
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
            catch (Exception ee)
            {
                MessageBox.Show("检测失败" + ee.Message);
            }
        }
        public void ThreadShowMsg(string msg)
        {
            Msg = msg;
            this.Invoke(new MyInvoke(ShowMsg));
        }
        public void ShowMsg()
        {
            textBoxOut.Text = Msg;
            if (Msg.StartsWith("End"))
                MessageBox.Show(Msg);
        }
        private void buttonAutoRorate_Click(object sender, EventArgs e)
        {
            _autororate = new AutoRorate(m_Imgselection, list, _ActivePath);

            _autororate.DgShowMsg = new DelegateShowScanMsg(ThreadShowMsg);
            //_autororate.RunRorate();
            _autororate.DoScan();
            _bScan = false;
           
        }
        private void buttonApplyAll_Click(object sender, EventArgs e)
        {
            if (listBoxfilename.SelectedIndex == -1 || _ActivePath == "" || !Directory.Exists(_ActivePath)
                || m_Imgselection.Width == 0 || m_Imgselection.Height == 0) return;

            List<string> srclst = GetListBoxNameList(listBoxfilename);

            System.IO.FileStream fs1 = new System.IO.FileStream(_ActivePath + srclst[0], System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap src = (Bitmap)System.Drawing.Image.FromStream(fs1);
            List<Rectangle> yuji = GetYuji(new Rectangle(0, 0, src.Width, src.Height), m_Imgselection);
            fs1.Close();

            string msg = "";
            if (yuji.Count > 0)
                foreach (string s in srclst)
                {
                    string ss = _ActivePath + s;
                    try
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(ss, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        Bitmap orgsrc = (Bitmap)System.Drawing.Image.FromStream(fs);
                        yuji = GetYuji(new Rectangle(0, 0, orgsrc.Width, orgsrc.Height), m_Imgselection);
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
            if (listBoxfilename.SelectedIndex == -1  || _ActivePath == "" || !Directory.Exists(_ActivePath)
                || m_Imgselection.Width == 0 || m_Imgselection.Height == 0) return;

            List<string> srclst = GetListBoxNameList(listBoxfilename);

            System.IO.FileStream fs1 = new System.IO.FileStream(_ActivePath + srclst[0], System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap src = (Bitmap)System.Drawing.Image.FromStream(fs1);
            List<Rectangle> yuji = GetYuji(new Rectangle(0, 0, src.Width, src.Height), m_Imgselection);
            fs1.Close();

            if (yuji.Count > 0)
                foreach (string s in srclst)
                {
                    string ss = _ActivePath + s;
                    System.IO.FileStream fs = new System.IO.FileStream(ss, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    Bitmap orgsrc = (Bitmap)System.Drawing.Image.FromStream(fs);

                    Bitmap rgb = orgsrc.Clone(m_Imgselection, orgsrc.PixelFormat);
                    ss = _ActivePath + "\\img" + s;
                    rgb.Save(ss);
                    fs.Close();
                }
        }
        private void buttonSelectionBlack_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)pictureBox1.Image;
            if (src != null && m_Imgselection.Width > 0 && m_Imgselection.Height > 0)
            {
                Bitmap rgb = ConvertFormat.ConvertToRGB(src);

                using (Graphics g = Graphics.FromImage(rgb))
                {
                    g.FillRectangle(Brushes.Black, m_Imgselection);
                }
                SetPictureImage(rgb);
            }
        }
        private void buttonOutRectWhite_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)pictureBox1.Image;
            if (src != null && m_Imgselection.Width > 0 && m_Imgselection.Height > 0)
            {
                Bitmap rgb = ConvertFormat.ConvertToRGB(src);
                List<Rectangle> yuji = GetYuji(new Rectangle(0, 0, src.Width, src.Height), m_Imgselection);
                if (yuji.Count > 0)
                    using (Graphics g = Graphics.FromImage(rgb))
                    {
                        g.FillRectangles(Brushes.White, yuji.ToArray());
                    }
                SetPictureImage(rgb);
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
                if (A.Bottom > Bi.Bottom)
                    lst.Add(new Rectangle(A.X, Bi.Bottom, A.Width, A.Bottom - Bi.Bottom));
                //L
                if (Bi.X > A.X)
                    lst.Add(new Rectangle(A.X, Bi.Top, Bi.X - A.X + 1, Bi.Height));
                //R
                if (A.Right > Bi.Right)
                    lst.Add(new Rectangle(B.Right, B.Top, A.Right - Bi.Right, Bi.Height));
            }
            return lst;
        }
        private void buttonCutbySelection_Click(object sender, EventArgs e)
        {
            if (m_Imgselection.Width > 0 && m_Imgselection.Height > 0 && pictureBox1.Image != null
                && m_Imgselection.Width < pictureBox1.Image.Width && m_Imgselection.Height < pictureBox1.Image.Height)
            {
                Bitmap src = (Bitmap)pictureBox1.Image;
                SetPictureImage(src.Clone(m_Imgselection, pictureBox1.Image.PixelFormat));
                m_Imgselection.Location = new Point(0, 0);
            }
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
        private void buttonSaveActiveImage_Click(object sender, EventArgs e)
        {
            if (m_Imgselection.Width == 0 || m_Imgselection.Height == 0) return;
            if (listBoxfilename.SelectedIndex == -1 || _ActivePath == "" || !Directory.Exists(_ActivePath)) return;
            string filename = _ActivePath + listBoxfilename.SelectedItem.ToString();

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
            SetPictureImage(src);
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
            SetPictureImage(src);
            _fs.Close();
            _fs = null;
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
        private List<string> GetListBoxNameList(ListBox listBox)
        {
            List<string> lst = new List<string>();
            for (int i = 0; i < listBox.Items.Count; i++)
                lst.Add(listBox.Items[i].ToString());
            return lst;
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
            MT = new MovetoCTL.MovetoTracker(pictureBox1);
            MT.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            MT.StartDraw(true);
        }

        private Rectangle m_Imgselection;
        private Point crop_startpoint;
        private MovetoTracker MT;
        private ZoomBox zoombox;
        private double _OriginWith;
        private string _ActivePath;
        private List<string> list;

        private FileStream _fs;
        private AutoAngle _angle;
        private List<Rectangle> _ListFeature;
        private bool _bScan;
        private AutoRorate _autororate;
        private string Msg;
    }
    public class AutoRorate
    {
        private Rectangle m_Imgselection;
        private List<string> list;
        private string _ActivePath;

        public AutoRorate(Rectangle m_Imgselection, List<string> list, string _ActivePath)
        {
            this.m_Imgselection = m_Imgselection;
            this.list = list;
            this._ActivePath = _ActivePath;
        }
        public void RunRorate()
        {
            Rectangle Correct = new Rectangle();
            if (list.Count > 0)
            {
                FileStream _fs = new FileStream(_ActivePath + list[0], FileMode.Open, FileAccess.Read);
                Bitmap _src = (Bitmap)System.Drawing.Image.FromStream(_fs);
                _src = _src.Clone(m_Imgselection, _src.PixelFormat);
                try
                {
                    DetectData dd = DetectImageTools.DetectImg(_src);
                    dd = DetectImageTools.DetectCorrect.ReDetectCorrectImg(_src, dd);
                    Correct = dd.CorrectRect;
                }
                catch
                {
                    Correct.Width = 0;
                }
                _fs.Close();
                _fs = null;
            }
            int index = 0;
            if(Correct.Width>0)
            foreach (string s in list)
            {
                if (DgShowMsg != null)
                    DgShowMsg("正在处理第"+index+"个：" + s);
                FileStream _fs = new FileStream(_ActivePath + s, FileMode.Open, FileAccess.Read);
                Bitmap src = (Bitmap)System.Drawing.Image.FromStream(_fs);
                src = src.Clone(m_Imgselection, src.PixelFormat);
                //src = Clone(src);
                src.Save("F:\\a.tif");
                src =  RorateImg(src,Correct);
                _fs.Close();
                if (src != null)
                    src.Save(_ActivePath + "\\img" + s);
                index++;
            }
            if (DgShowMsg != null)
                DgShowMsg("End 已经全部处理完，共处理" + index + "个");
        }
        private Bitmap ImageWithUnLock(string s) //unuse
        {
            FileStream _fs = new FileStream(_ActivePath + s, FileMode.Open, FileAccess.Read);
            Bitmap src = (Bitmap)System.Drawing.Image.FromStream(_fs);
            src = src.Clone(m_Imgselection,src.PixelFormat);
            src = Clone(src);
            _fs.Close();
            return src;
        }
        private Bitmap RorateImg(  Bitmap _src,Rectangle correct)
        {
            try
            {
                DetectData dd = DetectImageTools.DetectImg(_src,correct);
                //dd = DetectImageTools.DetectCorrect.ReDetectCorrectImg(_src, dd);
                if (dd.CorrectRect.Width > 0)
                {
                    List<Point> list = ListFeatureToPoints(dd);
                    AutoAngle  _angle = new AutoAngle(list);                   
                    double angle = -_angle.Angle1 * 180 / Math.PI;
                    //Rorate
                    _src = Tools.BitmapRotateTools.Rotate(_src,(float) angle);
                    //
                   _src= ConvertFormat.Convert(_src, PixelFormat.Format1bppIndexed, false);
                    //save
                   return _src;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("检测失败" + ee.Message);
            }
            return null;
        }
        private Bitmap Clone(Bitmap _Source)
        {
            int _Width = _Source.Width;
            int _Height = _Source.Height;
            PixelFormat _SetPixFormat = PixelFormat.Format1bppIndexed;
            Bitmap _NewBitmap = new Bitmap(_Width, _Height, _SetPixFormat);

            //获取两个图形的数据   
            BitmapData _SourceData = _Source.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            BitmapData _NewData = _NewBitmap.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.WriteOnly, _SetPixFormat);

            //复制出图形数据的   
            byte[] _SourceByte = new byte[_SourceData.Stride * _Height];
            byte[] _NewByte = new byte[_NewData.Stride * _Height];
            Marshal.Copy(_SourceData.Scan0, _SourceByte, 0, _SourceByte.Length);
            Marshal.Copy(_NewData.Scan0, _NewByte, 0, _NewByte.Length);
            _Source.UnlockBits(_SourceData);
            Marshal.Copy(_NewByte, 0, _NewData.Scan0, _NewByte.Length);
            _NewBitmap.UnlockBits(_NewData); 
            return _NewBitmap;
        }
        private List<Point> ListFeatureToPoints(DetectData dd)
        {
            List<Point> list = dd.ListFeature.Select(r => r.Location).ToList();
            return list;
        }
        public void DoScan()
        {
            Thread thread = new Thread(new ThreadStart(RunRorate));
            thread.Start();
        }
        public DelegateShowScanMsg DgShowMsg { get; set; }
    }
}
