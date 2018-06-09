using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tools;
using MovetoCTL;
using ARTemplate;
using System.IO;

namespace ScanTemplate
{
    public partial class FormPreScan : Form
    {
        public FormPreScan(UnScan dir)
        {
            InitializeComponent();
            _fullpath = dir.FullPath;
            _dirName = dir.DirName;
            _namelist = dir.ImgList();
            _activeid = 0;
            _fs = null;
            Init(null);
            InitSrc();
        }
        private void Init(Template t)
        {
            if (t == null)
            {
                _fs = null;
                _src = null;
            }
            //_template = t;          
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            zoombox = new ZoomBox();
            Reset();
        }
        private void Reset()
        {
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            _OriginWith = pictureBox1.Width;
            zoombox.Reset();
            m_act = Act.None;
        }
        private void InitSrc( )
        {
            if (File.Exists(ActiveFileName))
            {
                Image orgsrc = GetActiveImage();
                _src =(Bitmap) orgsrc.Clone();
                if (_src != null)
                    SetImage(_src);
            }
        }
        private void SetImage(Bitmap image)
        {
            pictureBox1.Image = image;
            ReSetPictureBoxImage();
            MT = new MovetoCTL.MovetoTracker(pictureBox1);
            MT.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            MT.StartDraw(true);
        }
        private void FormPreScan_Load(object sender, EventArgs e)
        {
            Rectangle area = new Rectangle(new Point(), _src.Size);          
            Rectangle cr = new Rectangle();
            DetectData dd = DetectImageTools.DetectImg(_src, area, cr);
            if (dd.CorrectRect.Width > 0)
            {
                _dd = dd;
            }
        }
        public string ActiveFileName
        {
            get
            {
                if (_activeid < 0 || _activeid > _namelist.Count)
                    _activeid = 0;
                return _namelist[_activeid];
            }
        }
        public Image GetActiveImage()
        {
            if (_fs != null)
            {
                _fs.Close();
                _fs = null;
            }
            _fs = new System.IO.FileStream(ActiveFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Image _orgsrc = (Bitmap)System.Drawing.Image.FromStream(_fs);
            return _orgsrc;
        }

        private int _activeid;
        private string _fullpath;
        private string _dirName;
        private List<string> _namelist;
        private System.IO.FileStream _fs;
        private DetectData _dd;

        private void CompleteSelection(bool bcomplete)
        {
            if (bcomplete)
            {
                ShowMessage("Complete: " + m_act);
                m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                if (_ActiveEditMode && _ActiveEditArea != null)
                {
                    //_TestR = Rectangle.Union(_ActiveEditArea.Rect, m_Imgselection);
                    _ActiveEditArea.Rect = m_Imgselection;// Rectangle.Union(_ActiveEditArea.Rect, m_Imgselection);
                    _ActiveEditArea.EditMode = false;
                    _ActiveEditMode = false;
                    _ActiveEditArea = null;
                }
                else
                {
                    switch (m_act)
                    {
                        //case Act.DefinePoint: CompleteDeFinePoint(); break;
                        //case Act.DefineChoose: CompleteDeFineChoose(); break;
                        //case Act.DefineUnChoose: CompleteDeFineUnChoose(); break;
                        //case Act.DefineId: CompleteDeFineId(); break;
                        //case Act.SeclectionToWhite: CompleteSelectionToWhite(); break;
                        //case Act.SeclectionToDark: CompleteSelectionToDark(); break;
                        //case Act.DefineName: CompleteDeFineName(); break;
                        //case Act.SelectionToGroup: CompleteSelectionGroup(); break;
                        //case Act.DefineCustom: CompleteDefineCustom(); break;
                    }
                }
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (_ActiveEditMode && _ActiveEditArea != null)
            //{
            //    int index = 0;
            //    foreach (Rectangle r in _ControlRects)
            //    {
            //        if (r.Contains(e.Location))
            //        {
            //            if(index==1||index==2)
            //            this.Cursor = Cursors.SizeNESW;
            //            else 
            //            this.Cursor = Cursors.SizeNWSE;
            //            return;
            //        }
            //        index++;
            //    }
            //}
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            if (MT != null && MT.Selection.Width > 3)
            {
                m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                CompleteSelection(true);
            }
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (m_act == Act.ZoomMouse)
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
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Pen pen = Pens.Red;
                Brush dark = Brushes.Black;
                Brush white = Brushes.White;
                Brush Red = Brushes.Red;
                Font font = DefaultFont;
                if (_dd != null)
                {
                    Rectangle r = _dd.CorrectRect;
                    DrawRect(e, _dd.CorrectRect);
                    DrawRects(e, _dd.ListFeature,_dd.CorrectRect.Location);
                }

                if (_ActiveEditMode && _ActiveEditArea != null)
                {
                    Pen pen1 = Pens.DarkBlue;
                    Area I = _ActiveEditArea;
                    Rectangle r = zoombox.ImgToBoxSelection(I.ImgArea);
                    e.Graphics.DrawRectangle(pen1, r);
                    _ControlRects = DetectImageTools.DetectCorrect.GetLrbRtb(r, 5, 5);
                    e.Graphics.DrawRectangles(pen1, _ControlRects.ToArray());
                }
                if (_TestR.Width > 0 && _TestR.Height > 0)
                {
                    Rectangle r = zoombox.ImgToBoxSelection(_TestR);
                    e.Graphics.DrawRectangle(Pens.Yellow, r);
                }
            }
        }

        private void DrawRects(PaintEventArgs e, List<Rectangle> list, Point offset)
        {
            foreach (Rectangle r in list)
            {
                r.Offset(offset);
                e.Graphics.DrawRectangle(Pens.Red, zoombox.ImgToBoxSelection(r));
            }
        }
        private void DrawRect(PaintEventArgs e, Rectangle r)
        {
            e.Graphics.DrawRectangle(Pens.Red, zoombox.ImgToBoxSelection(r));
        }
        private void Zoomrat(double rat, Point e)
        {
            Bitmap bitmap_show = (Bitmap)pictureBox1.Image;
            Point L = pictureBox1.Location;
            Point S = panelRT.AutoScrollPosition;
            int w = (int)(pictureBox1.Width * rat);
            int h = w * bitmap_show.Height / bitmap_show.Width;
            L.Offset((int)(e.X * (rat - 1)), (int)(e.Y * (rat - 1)));
            pictureBox1.SetBounds(S.X, S.Y, w, h);
            zoombox.UpdateBoxScale(pictureBox1);

            S.Offset((int)(e.X * (1 - rat)), (int)(e.Y * (1 - rat)));
            panelRT.Invalidate();
            panelRT.AutoScrollPosition = new Point(-S.X, -S.Y);
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
        }
        private void ShowMessage(string message)
        {
            textBoxMessage.Text = message;
        }

        private double _OriginWith;
        private Rectangle m_Imgselection;
        private TreeNode m_tn;
        private MovetoTracker MT;
        private Act m_act;
        private Point crop_startpoint;
        private ZoomBox zoombox;
        private Area _ActiveEditArea;
        private bool _ActiveEditMode;
        private List<Rectangle> _ControlRects;
        private Rectangle _TestR;
        private Bitmap _src;
    }
}
