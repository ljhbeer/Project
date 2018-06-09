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
    [Flags]
    enum PreAct : short { None = 0, DefineDetectArea = 1, DefineFeaturePointDetectArea = 2, DefineScanLTDetectArea = 4, AutoDetect = 8, ShowImageMode = 16, NinthDetect, SixteenthDetect, PreDetect, NextImage, ZoomMouse };
    
    public partial class FormPreScan : Form
    {
        public FormPreScan(UnScan dir)
        {
            InitializeComponent();
            _fullpath = dir.FullPath;
            _dirName = dir.DirName;
            _namelist = dir.ImgList();
            _PreActiveid = 0;
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
            
            //m_tn = new TreeNode();
            //if (t != null)
            //    m_tn = t.GetTreeNode();           
            //treeView1.Nodes.Add(m_tn);
            //treeView1.ExpandAll();
            Reset();
        }
        private void Reset()
        {
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            _OriginWith = pictureBox1.Width;
            zoombox.Reset();
            m_PreAct = PreAct.None;
            //m_tn.Nodes.Clear();
            treeView1.Nodes.Clear();
            m_tn = GetTreeNode();
            treeView1.Nodes.Add(m_tn);
            treeView1.ExpandAll();
        }
        public TreeNode GetTreeNode()
        {
            TreeNode root = new TreeNode();
            foreach (string s in new string[] { "动态检测左上角区域", "特征点检测区域", "检测区域" })
            {
                TreeNode opt = new TreeNode();
                opt.Name = opt.Text = s;
                root.Nodes.Add(opt);
            }
            root.Text = "网上阅卷-预处理试卷";
            return root;
        }
        private void InitSrc( )
        {
            if (File.Exists(PreActiveFileName))
            {
                Image orgsrc = GetPreActiveImage();
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
        public string PreActiveFileName
        {
            get
            {
                if (_PreActiveid < 0 || _PreActiveid > _namelist.Count)
                    _PreActiveid = 0;
                return _namelist[_PreActiveid];
            }
        }
        public Image GetPreActiveImage()
        {
            if (_fs != null)
            {
                _fs.Close();
                _fs = null;
            }
            _fs = new System.IO.FileStream(PreActiveFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Image _orgsrc = (Bitmap)System.Drawing.Image.FromStream(_fs);
            return _orgsrc;
        }


        private void toolStripButtonZoomin_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
            Zoomrat(1.1, new Point(pictureBox1.Width / 2, pictureBox1.Height / 2));
        }
        private void toolStripButtonZoomMouse_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
            m_PreAct = PreAct.ZoomMouse;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonZoomout_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
            Zoomrat(0.9, new Point(pictureBox1.Width / 2, pictureBox1.Height / 2));
        }
        private void toolStripButton_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton b in toolStrip1.Items)
                b.Checked = false;
            ToolStripButton click = (ToolStripButton)sender;
            if (click.Checked)
                m_PreAct = PreAct.None;
            ShowMessage("PreAct:" + m_PreAct);
            click.Checked = !click.Checked;
            //MT.ClearEvent();
            if (m_PreAct == PreAct.DefineDetectArea || m_PreAct == PreAct.DefineFeaturePointDetectArea || 
                m_PreAct == PreAct.DefineScanLTDetectArea 
               )
            {
                MT.StartDraw(true);
                //MT.completevent += CompleteSelection;
            }
        }
        private void toolStripButtonDefineDetectArea_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_PreAct = PreAct.DefineDetectArea;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonDefineFeaturePointDetectArea_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_PreAct = PreAct.DefineFeaturePointDetectArea;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonDefineScanLTDetectArea_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_PreAct = PreAct.DefineScanLTDetectArea;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonAutDetect_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_PreAct = PreAct.AutoDetect;
            toolStripButton_Click(sender, e);
        }
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_PreAct = PreAct.ShowImageMode;
            toolStripButton_Click(sender, e);
        }
        private void NinthDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_PreAct = PreAct.NinthDetect;
            toolStripButton_Click(sender, e);
        }
        private void SixteenthDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_PreAct = PreAct.SixteenthDetect;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonPreDetect_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_PreAct = PreAct.PreDetect;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonNextImage_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_PreAct = PreAct.NextImage;
            toolStripButton_Click(sender, e);
        }
        private void CompleteSelection(bool bcomplete)
        {
            if (bcomplete)
            {
                ShowMessage("Complete: " + m_PreAct);
                m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                {
                    switch (m_PreAct)
                    {
                        case PreAct.DefineDetectArea: CompleteDeFineDetectArea(); break;
                        case PreAct.DefineFeaturePointDetectArea: CompleteDefineFeaturePointDetectArea(); break;
                        case PreAct.DefineScanLTDetectArea: CompleteDefineScanLTDetectArea(); break;
                    }
                }
            }
            pictureBox1.Invalidate();
        }

        private void CompleteDefineScanLTDetectArea()
        {
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            String keyname = "动态检测左上角区域";
            //if (!ExistDeFineSelection(keyname))
            {
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                string nodename = "左上角-" + m_Imgselection.ToString("-"); ;
                if (cnt == 1)
                {
                    TreeNode t = new TreeNode();
                    String name = nodename;
                    t.Name = nodename;
                    t.Text = nodename ;
                    t.Tag = m_Imgselection;
                    m_tn.Nodes[keyname].Nodes.Add(t);                    
                }
                else
                {
                    TreeNode t = m_tn.Nodes[keyname].Nodes[0];
                    t.Tag = m_Imgselection;
                }
            }
        }

        private void CompleteDefineFeaturePointDetectArea()
        {
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            String keyname = "特征点检测区域";
            //if (!ExistDeFineSelection(keyname))
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                string nodename = "特征点-" + cnt+"|"+ m_Imgselection.ToString("-");;
               
                String name = nodename;
                t.Name = nodename;
                t.Text = nodename;
                t.Tag = m_Imgselection;
                m_tn.Nodes[keyname].Nodes.Add(t);
            }
        }

        private void CompleteDeFineDetectArea()
        {
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            String keyname = "检测区域";
            //if (!ExistDeFineSelection(keyname))
            {
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                string nodename = "检测区域-" + m_Imgselection.ToString("-");
                if (cnt == 1)
                {
                    TreeNode t = new TreeNode();
                    String name = nodename;
                    t.Name = nodename;
                    t.Text = nodename;
                    t.Tag = m_Imgselection;
                    m_tn.Nodes[keyname].Nodes.Add(t);
                }
                else
                {
                    TreeNode t = m_tn.Nodes[keyname].Nodes[0];
                    t.Tag = m_Imgselection;
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (_PreActiveEditMode && _PreActiveEditArea != null)
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
            if (m_PreAct == PreAct.ZoomMouse)
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
                if (m_tn.Nodes.Count != 0)
                {
                   
                    foreach (TreeNode tt in m_tn.Nodes)
                        foreach (TreeNode t in tt.Nodes)
                        {
                            if (t.Tag != null)
                            {
                                Rectangle r = (Rectangle)(t.Tag);
                                e.Graphics.DrawRectangle(Pens.Red, zoombox.ImgToBoxSelection(r));
                            }
                        }
                }
                if (_dd != null)
                {
                    Rectangle r = _dd.CorrectRect;
                    DrawRect(e, _dd.CorrectRect);
                    DrawRects(e, _dd.ListFeature,_dd.CorrectRect.Location);
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
        private PreAct m_PreAct;
        private Point crop_startpoint;
        private ZoomBox zoombox;
        private Rectangle _TestR;
        private Bitmap _src;

        ////////////
        private int _PreActiveid;
        private string _fullpath;
        private string _dirName;
        private List<string> _namelist;
        private System.IO.FileStream _fs;
        private DetectData _dd;
    }
}
