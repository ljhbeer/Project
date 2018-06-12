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
using Newtonsoft.Json;

namespace ScanTemplate
{
    [Flags]
    enum PreAct : short { None = 0, DefineDetectArea = 1, DefineFeaturePointDetectArea = 2, DefineScanLTDetectArea = 4, AutoDetect = 8, ShowImageMode = 16, NinthDetect, SixteenthDetect, PreDetect, NextImage, ZoomMouse, DetectMode };
    enum ShowImageMode : short { ShowFullImageMode, ShowCorrectImageMode }
    enum DetectMode : short { Whole = 0, DetectArea =1,  DetectFeaturesArea=2, DetectLT=3, DetectLTninth, DetectLTSixteenth };

    public partial class FormPreScan : Form
    {
        public List<string> dms = new List<string>() {"全部", "检测范围","特征点范围","左上角范围"};
        public FormPreScan(UnScan dir)
        {
            InitializeComponent();
            _fullpath = dir.FullPath;
            _dirName = dir.DirName;
            _namelist = dir.ImgList();
            _PreActiveid = 0;
            _pp = new PrePapers();
            _fs = null;
            Init(null);
        }
        private void Init(Template t)
        {
            if (t == null)
            {
                _fs = null;
                _src = null;
            }
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            zoombox = new ZoomBox();
            Reset();
        }
        private void Reset()
        {
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            _OriginWith = pictureBox1.Width;
            zoombox.Reset();
            m_PreAct = PreAct.None;
            _sim = ShowImageMode.ShowFullImageMode;
            _dtm = DetectMode.Whole;
            //m_tn.Nodes.Clear();
            treeView1.Nodes.Clear();
            m_tn = GetTreeNode();
            treeView1.Nodes.Add(m_tn);
            treeView1.ExpandAll();
        }
        public TreeNode GetTreeNode()
        {
            TreeNode root = new TreeNode();
            foreach (string s in new string[] { "左上角范围", "特征点范围", "检测范围" })
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
        public PrePapers PreScan()
        {
            _pp.Clear();
            if (_namelist.Count > 0)
            {
                PrePaper p = PreScan(_namelist[0]);
                if (p.Detected())
                {
                    List<Rectangle> lrtb = new List<Rectangle>();
                    foreach (Rectangle r in p.listFeatures)
                    {
                        r.Inflate(r.Width / 2, r.Height / 2);
                        r.Offset(p.Detectdata.CorrectRect.Location);
                        lrtb.Add(r);
                    }
                    foreach (string s in _namelist)
                    {
                        if (File.Exists(s))
                            _pp.AddPrePaper(PreScan(s,lrtb,p.Detectdata.CorrectRect.Size));
                    }
                }
            }
            return _pp;
        }
        private static PrePaper PreScan(string s, List<Rectangle> lrtb,Size correctsize) //根据模板，设置四个点，分别检测
        {
            Rectangle LT = lrtb[0]; //LT
            Point Center = new Point(LT.X + LT.Width / 2, LT.Y + LT.Height / 2);
             
            PrePaper pp = new PrePaper(s);
            using (FileStream fs = new System.IO.FileStream(s, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                Bitmap src = (Bitmap)System.Drawing.Image.FromStream(fs);
                Rectangle area = new Rectangle(new Point(), src.Size);

                Rectangle nrLT = DetectLTPoint(LT, src, correctsize );
                //已检测到左上角
                List<Rectangle> ListFeature = new List<Rectangle>();
                if (nrLT.Width > 0 && nrLT.Height > 0)
                {
                    //ListFeature.Add(nrLT);
                    Point DetectCenter = new Point(nrLT.X + nrLT.Width / 2, nrLT.Y + nrLT.Height / 2);
                    Point offset = DetectCenter;
                    offset.Offset(-Center.X, -Center.Y);

                    for (int i = 0; i < lrtb.Count; i++)
                    {
                        Rectangle r = lrtb[i];
                        r.Offset(offset);
                        r.Intersect(area);
                        ListFeature.Add(
                            Tools.DetectImageTools.DetectCorrect.DetectCorrectFromImg(src, r, true, r.Width / 9)
                            );
                    }

                    DetectData dd = DetectImageTools.DetectCorrect.ConstructDetectData(ListFeature);
                    if (dd.Detected)
                    {
                        pp.Detectdata = dd;
                    }
                }
            }
            return pp;
        }

        private static Rectangle DetectLTPoint(Rectangle LT, Bitmap src, Size correctsize)
        {
            Size blocksize = new Size(LT.Width/2,LT.Height/2);
            Rectangle nrLT = new Rectangle();
            Rectangle area = new Rectangle(new Point(), src.Size);
            Rectangle detectarea = LT;
            
            detectarea.Inflate(detectarea.Width / 3, detectarea.Height / 3);
            detectarea.Intersect(area);
            nrLT = Tools.DetectImageTools.DetectCorrect.DetectCorrectFromImg(src, detectarea, true, detectarea.Width / 5);

            //重新设置范围检测
            if (nrLT.Width == 0 || nrLT.Height == 0)
            {
            }
            if (nrLT.Width > 0 || nrLT.Height > 0)
            {
                double wrate = nrLT.Width * 1.0 / blocksize.Width;
                double hrate = nrLT.Height * 1.0 / blocksize.Height;
                if (wrate > 1.2 || wrate < 0.8 || hrate > 1.2 || hrate < 0.8)
                {
                    detectarea = new Rectangle(nrLT.Location, new Size());
                    detectarea.Inflate(blocksize);
                    nrLT = Tools.DetectImageTools.DetectCorrect.DetectCorrectFromImg(src, detectarea, true, detectarea.Width / 5);
                }
            }
            return nrLT;
        }
        private static PrePaper PreScan(string s)
        {
            PrePaper pp = new PrePaper(s);
            using (FileStream fs = new System.IO.FileStream(s, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                Bitmap src = (Bitmap)System.Drawing.Image.FromStream(fs);
                Rectangle area = new Rectangle(new Point(), src.Size);
                List<int> inflaterate = new List<int>() { 30, src.Width / 5};
                int circlecount = inflaterate.Count;
                int index = 0;
                for (int i = 0; i < circlecount; i++)
                {
                    area.Inflate(-src.Width / inflaterate[index], -src.Height / inflaterate[index]);
                    DetectData dd = DetectImageTools.DetectImg(src, area, new Rectangle());
                    if (dd.Detected)
                    {
                        pp.Detectdata = dd;
                        break;
                    }
                    index++;
                    index %= circlecount;
                }
            }
            return pp;
        }
        private void FormPreScan_Load(object sender, EventArgs e)
        {
            InitSrc();
            Rectangle area = new Rectangle(new Point(), _src.Size);
            Rectangle cr = new Rectangle();
            area.Inflate(-_src.Width / 30, -_src.Height / 30);
            DetectData dd = DetectImageTools.DetectImg(_src, area, cr);
            if (dd.CorrectRect.Width > 0)
            {
                _dd = dd;
            }
            toolStripComboBoxDetectMode.SelectedIndex = 0;
            toolStripComboBoxImageMode.SelectedIndex = 0;
        }
        public string PreActiveFileName
        {
            get
            {
                if (_PreActiveid < 0 || _PreActiveid >= _namelist.Count)
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
            if (m_PreAct == PreAct.DefineDetectArea || m_PreAct == PreAct.DefineFeaturePointDetectArea ||
                m_PreAct == PreAct.DefineScanLTDetectArea
               )
            {
                MT.StartDraw(true);
            }
            else
            {
                CompleteSelection(true);
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
                ((ToolStripButton)sender).Checked = false;
                m_PreAct = PreAct.AutoDetect;
            toolStripButton_Click(sender, e);
        }
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_PreAct = PreAct.ShowImageMode;
            CompleteSelection(true);
        }
        private void toolStripComboBoxDetectMode_SelectedIndexChanged(object sender, EventArgs e)
        {          
            m_PreAct = PreAct.DetectMode;
            CompleteSelection(true);
        }
        private void NinthDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
                m_PreAct = PreAct.NinthDetect;
            toolStripButton_Click(sender, e);
        }
        private void SixteenthDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
                m_PreAct = PreAct.SixteenthDetect;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonPreDetect_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
                m_PreAct = PreAct.PreDetect;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonNextImage_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
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
                        case PreAct.AutoDetect: CompleteAutoDetect(); break;
                        case PreAct.ShowImageMode: CompleteShowImageMode(); break;
                        case PreAct.DetectMode: CompleteDetectMode(); break;
                        case PreAct.NinthDetect: CompleteNinthDetect(); break;
                        case PreAct.SixteenthDetect: CompleteSixteenthDetect(); break;
                        case PreAct.NextImage: CompleteNextImage(); break;
                        case PreAct.PreDetect: CompletePreDetect(); break;
                    }
                }
            }
            pictureBox1.Invalidate();
        }
        private void CompleteDefineScanLTDetectArea()
        {
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            String keyname = "左上角范围";
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
            String keyname = "特征点范围";
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
            String keyname = "检测范围";
            //if (!ExistDeFineSelection(keyname))
            {
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                string nodename = "检测范围-" + m_Imgselection.ToString("-");
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
        private void CompleteNextImage()
        {
            _PreActiveid++;
            InitSrc();
        }
        private void CompleteSixteenthDetect()
        {
            //TODO: sixteenth 
            Rectangle DetectArea = ReadDetectArea("左上角范围");
            if (DetectArea.Width > 0)
            {
                List<Rectangle> listLT = GetNNthDetectAreas(DetectArea, 4);
                foreach (Rectangle r in listLT)
                {
                    //if(Detect r .OK  break;
                }
            }
        }
        private void CompleteNinthDetect()
        {
            //TODO: Ninth 
            Rectangle DetectArea = ReadDetectArea("左上角范围");
            if (DetectArea.Width > 0)
            {
                List<Rectangle> listLT = GetNNthDetectAreas(DetectArea, 3);
                foreach (Rectangle r in listLT)
                {
                    //if(Detect r .OK  break;
                }
            }
        }
        private void CompleteShowImageMode()
        {
            //TODO: CompleteShowImageMode
             if(toolStripComboBoxImageMode.SelectedText == "")
             {
                 _sim = ShowImageMode.ShowFullImageMode;
             }else if(toolStripComboBoxImageMode.SelectedText == ""){
                 //加上其他条件
                 _sim = ShowImageMode.ShowCorrectImageMode;
             }
        }
        private void CompleteDetectMode()
        {
            string str = toolStripComboBoxDetectMode.SelectedItem.ToString();
            if (str == "全部")
            {
                _dtm = DetectMode.Whole;
            }
            else if (str == "检测范围")
            {
                _dtm = DetectMode.DetectArea;
            }
            else if (str == "特征点范围")
            {
                _dtm = DetectMode.DetectFeaturesArea;
            }
            else if (str == "左上角范围")
            {
                _dtm = DetectMode.DetectLT;
                //九分 十六分 暂未实现
            }
            pictureBox1.Invalidate();
        }
        private void CompleteAutoDetect()
        {
            if (pictureBox1.Image == null ) return;
            switch (_dtm)
            {
                case DetectMode.Whole:
                case DetectMode.DetectArea: AutoDetectByDetectArea(); break;
                case DetectMode.DetectFeaturesArea: AutoDetectByFeaturesArea(); break;
                case DetectMode.DetectLT:           AutoDetectByLT(1); break;
                case DetectMode.DetectLTninth:      AutoDetectByLT(3); break;
                case DetectMode.DetectLTSixteenth:  AutoDetectByLT(4); break;
            }
        }        
        private void CompletePreDetect()
        {
            //TODO: PreDetectAllFiles
            // 检测所有文件
            List<Rectangle> areas = ReadDetectAreas();
            if (areas.Count > 0)
                foreach (string s in _namelist)
                {
                    string ss =  s;
                    System.IO.FileStream fs = new System.IO.FileStream(ss, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    Bitmap orgsrc = (Bitmap)System.Drawing.Image.FromStream(fs);
                    List<Rectangle> ListFeature = GetListFeature(areas, orgsrc);
                    //_dd = DetectImageTools.DetectCorrect.ConstructDetectData(_ListFeature);
                    Bitmap rgb = orgsrc.Clone(ListFeature[0], orgsrc.PixelFormat);
                    //ss =  "\\img" + s;
                    //rgb.Save(ss);
                    fs.Close();
                }

            //if (list.Count == 0) return;
            //Bitmap _src = (Bitmap)pictureBox1.Image;
            ////_src = _src.Clone(m_Imgselection,_src.PixelFormat);
            //DetectData dd;
            //try
            //{
            //    dd = DetectImageTools.DetectImg(_src, m_Imgselection);
            //    dd = DetectImageTools.DetectCorrect.ReDetectCorrectImg(_src, dd);
            //    double _dangle = AutoAngle.ComputeAngle(dd.ListFeature[0].Location, dd.ListFeature[1].Location);
            //    double angle = -_dangle * 180 / Math.PI;
            //    //Rorate
            //    Bitmap _src1 = Tools.BitmapRotateTools.Rotate(_src, (float)angle);
            //    _src1 = ConvertFormat.Convert(_src1, PixelFormat.Format1bppIndexed, false);

            //    Rectangle Rect = dd.CorrectRect;
            //    Rect.Inflate(40, 40);
            //    Rect.Intersect(m_Imgselection);
            //    dd = DetectImageTools.DetectImg(_src, Rect);
            //    //dd = DetectImageTools.DetectCorrect.ReDetectCorrectImg(_src, dd);
            //}
            //catch
            //{
            //    return;
            //}
            //if (dd.CorrectRect.Width == 0) return;

            //_autororate = new AutoRorate(dd.CorrectRect, list, _ActivePath, checkBoxVertical.Checked);
            //_autororate.DgShowMsg = new DelegateShowMsg(ThreadShowMsg);
            //_autororate.DoScan();
            //_bScan = false;
        }
        
        private Rectangle ReadDetectArea(String keyname = "检测范围")
        {
            int cnt = m_tn.Nodes[keyname].GetNodeCount(false);
            if (cnt == 0)
            {
                MessageBox.Show("没有"+keyname);
                return new Rectangle();
            }
            TreeNode t = m_tn.Nodes[keyname].Nodes[0];
            Rectangle  DetectArea = (Rectangle)t.Tag;
            if (DetectArea.Height == 0)
                DetectArea.Width = 0;
            return DetectArea;
        }
        private List<Rectangle> ReadDetectAreas(String keyname = "特征点范围")
        {
            int cnt = m_tn.Nodes[keyname].GetNodeCount(false);
            if (cnt == 0)
            {
                MessageBox.Show("没有"+keyname);
                return new List<Rectangle>();
            }
            List<Rectangle> areas = new List<Rectangle>();
            foreach (TreeNode tn in m_tn.Nodes[keyname].Nodes)
            {
                if (tn.Tag != null)
                {
                    Rectangle DetectArea = (Rectangle)tn.Tag;
                    if (DetectArea.Height == 0)
                        DetectArea.Width = 0;
                    areas.Add(DetectArea);
                }
            }
            TreeNode t = m_tn.Nodes[keyname].Nodes[0];
            return areas;
        }
        private void AutoDetectByDetectArea()
        {
            Rectangle DetectArea = ReadDetectArea();
            if (DetectArea.Width > 0)
                try
                {
                    DetectData dd = DetectImageTools.DetectImg(_src, DetectArea);
                    dd = DetectImageTools.DetectCorrect.ReDetectCorrectImg(_src, dd);
                    if (dd.CorrectRect.Width > 0)
                    {
                        _dd = dd;
                        pictureBox1.Invalidate();
                        //InitListFeature(dd);
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show("检测失败" + ee.Message);
                }
        }
        private void AutoDetectByLT(int n)
        {
            Rectangle DetectArea = ReadDetectArea("左上角");
            List<Rectangle> areas = GetNNthDetectAreas(DetectArea, n);
            bool ok = false;
            foreach (Rectangle r in areas)
            {
                // if( DetectLT (r) && DetectOther3Point())  { ok = true; break;}
                
            }
            if (!ok)
                MessageBox.Show("检测失败");
        }
        private void AutoDetectByFeaturesArea()
        {
            List<Rectangle> areas = ReadDetectAreas();
            _ListFeature = GetListFeature(areas, _src);
            _dd =  DetectImageTools.DetectCorrect.ConstructDetectData(_ListFeature);

            //foreach (Rectangle r in areas)
            //{
            //    Rectangle nr2 = Tools.DetectImageTools.DetectCorrect.DetectCorrectFromImg(_src, r, true, r.Width / 9);
            //}
            //Rectangle r = (Rectangle)listBoxDetectareas.SelectedItem;
            //Bitmap src = (Bitmap)pictureBox1.Image;
            //Rectangle nr2 = Tools.DetectImageTools.DetectCorrect.DetectCorrectFromImg(_src, r, true, r.Width / 9);
           ///////////////
        }

        private void SetDetectAreas(List<Rectangle> list)
        {
            String keyname = "特征点范围";
            TreeNode tn = m_tn.Nodes[keyname];
            tn.Nodes.Clear();

            List<String> names = new List<String>() {"LT","LB","RT","RB" };
            for (int index = 0; index < list.Count; index++)
            {
                TreeNode t = new TreeNode();
                t.Tag = list[index];
                t.Text = t.Name = "特征点-" + names[index] + "|" + list[index].ToString("-");
                tn.Nodes.Add(t);
            }
        }
        private List<Rectangle> GetNNthDetectAreas(Rectangle  Area, int n)
        {
            List<Rectangle> list = new List<Rectangle>();
            if (n <= 0)
                return list;
            if (n == 1 || n==2)
            {
                list.Add(Area);
                return list;
            }
            //三以上
            double W = Area.Width*1.0/n;
            double H = Area.Height * 1.0/n;
            Size size = new Size((int)(W * 2),(int)( H * 2));
            for(int x=0; x<n-1; x++)
                for (int y = 0; y < n - 1; y++)
                {
                    list.Add( new Rectangle( (int)( Area.X + x*W), (int)(Area.Y + y*H),size.Width,size.Height));
                }
                return list;
        }
        private List<Rectangle> GetNNthLines(Rectangle Area, int n)
        {
            List<Rectangle> list = new List<Rectangle>();
            double W = Area.Width * 1.0 / n;
            double H = Area.Height * 1.0 / n;
            Size size = new Size((int)(W * 2), (int)(H * 2));
            for (int x = 0; x < n + 1; x++)
                list.Add(new Rectangle((int)(Area.X + x * W),  Area.Y, 1, size.Height));
            for (int y = 0; y < n - 1; y++)
            {
                list.Add(new Rectangle( Area.X , (int)(Area.Y + y * H), size.Width, 1));
            }
            return list;
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
                    {
                        int index =(int) _dtm;
                        string keyname = dms[index];
                        if(keyname == "全部" || tt.Text == keyname)
                        foreach (TreeNode t in tt.Nodes)
                        {
                            if (t.Tag != null)
                            {
                                Rectangle r = (Rectangle)(t.Tag);
                                e.Graphics.DrawRectangle(Pens.YellowGreen, zoombox.ImgToBoxSelection(r));
                            }
                        }
                    }
                }
                if (_dd != null && _dd.Detected)
                {
                    Rectangle r = _dd.CorrectRect;
                    DrawRect(e, _dd.CorrectRect,Pens.Red);
                    DrawRects(e, _dd.ListFeature,_dd.CorrectRect.Location,Pens.Red);
                }
            }
        }

        private void DrawRects(PaintEventArgs e, List<Rectangle> list, Point offset,Pen pen)
        {
            foreach (Rectangle r in list)
            {
                r.Offset(offset);
                DrawRect(e, r,pen);
            }
        }
        private void DrawRect(PaintEventArgs e, Rectangle r,Pen pen)
        {
            e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(r));
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
        private Bitmap _src;
        ////////////
        private int _PreActiveid;
        private string _fullpath;
        private string _dirName;
        private List<string> _namelist;
        private System.IO.FileStream _fs;
        private DetectData _dd;
        ////////
        private ShowImageMode _sim;
        private DetectMode _dtm;
        //private DetectMode _dtm2;
        ////////
        private List<Rectangle> _ListFeature;
        private PrePapers _pp;
        private PrePapers _prepapers;
        public PrePapers Prepapers
        {
            get { return _prepapers; }
        }
        private void InitListFeature(DetectData dd)
        {
            _ListFeature = dd.ListFeature.Select(r => { r.Offset(dd.CorrectRect.Location); return r; }).ToList();
            _ListFeature.Add(dd.CorrectRect);
        }
        private List<Point> ListFeatureToPoints(DetectData dd)
        {
            List<Point> list = dd.ListFeature.Select(r => new Point(r.X + m_Imgselection.X, r.Y + m_Imgselection.Y)).ToList();
            return list;
        }
        private static List<Rectangle> GetListFeature(List<Rectangle> areas, Bitmap src)
        {
            List<Rectangle> ListFeature = new List<Rectangle>();
            foreach (Rectangle r in areas)
            {
                Rectangle nr2 = Tools.DetectImageTools.DetectCorrect.DetectCorrectFromImg(src, r, true, r.Width / 9);
                ListFeature.Add(nr2);
            }
            return ListFeature;
        }
        private Rectangle GetUnion(List<Rectangle> ListRect)
        {
            Rectangle UnionR = new Rectangle();
            if (ListRect.Count > 0)
            {
                UnionR = ListRect[0];
                foreach (Rectangle r in ListRect)
                    UnionR = Rectangle.Union(UnionR, r);
            }
            return UnionR;
        }
        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Parent == null) return;
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.D)
            {
                if (treeView1.SelectedNode.Parent.Text != "网上阅卷-预处理试卷")
                {
                    TreeNode t = treeView1.SelectedNode.NextNode;
                    if (t == null)
                        t = treeView1.SelectedNode.PrevNode;
                    treeView1.SelectedNode.Remove();
                    treeView1.SelectedNode = t;
                }
                pictureBox1.Invalidate();
            }
            else if (e.KeyCode == Keys.R &&   treeView1.SelectedNode.Text == "特征点范围")
            {
                List<Rectangle> areas = ReadDetectAreas();
                Rectangle ur = GetUnion(areas);
                Point center = ur.Location;
                center.Offset(ur.Width / 2, ur.Height / 2);
                List<Rectangle> LTRB = new List<Rectangle>( ); // LT,LB,RT,RB
                for (int i = 0; i < 4; i++)
                    LTRB.Add(new Rectangle());

                foreach (Rectangle r in areas)
                {
                    Point c = r.Location;
                    c.Offset(r.Width / 2, r.Height / 2);
                    if (c.X > center.X)
                    {
                        if (c.Y > center.Y)
                            LTRB[3] = r;
                        else
                            LTRB[2] = r;
                    }
                    else
                    {
                        if (c.Y > center.Y)
                            LTRB[1] = r;
                        else
                            LTRB [0]= r;
                    }
                }
                if (LTRB[0].Width!=0 && LTRB[1].Width!=0 && LTRB[2].Width!=0 && LTRB[3].Width!=0   )
                {
                    SetDetectAreas(LTRB);
                }

                pictureBox1.Invalidate();
            }
        }
         
        public bool PreCheckJsonFile(UnScan dir)
        {
            Boolean ExistOKScanJson = false;
            _prepapers = new PrePapers();
            if (!File.Exists(dir.Path + "data.txt.json"))
            {
                if (File.Exists(dir.FullPath + ".prescanpapers.json"))
                {
                    _prepapers.LoadPrePapers(dir.FullPath + ".prescanpapers.json");
                    ExistOKScanJson = ValidPreScanData(dir.ImgList(), _prepapers);
                    if (!ExistOKScanJson)
                        File.Delete(dir.FullPath + ".prescanpapers.json");
                }
                if (!ExistOKScanJson)
                {
                    _prepapers = PreScan();
                    if (_prepapers.AllDetected()) // 已成功预扫描
                    {
                        _prepapers.SavePrePapers(dir.FullPath + ".prescanpapers.json");
                        ExistOKScanJson = true;
                    }
                }
            }
            else //扫描数据
            {
                ScanTemplate.FormYJ.Papers papers = new FormYJ.Papers();
                //////////////
            }
            return ExistOKScanJson;
        }
        private static bool ValidPreScanData(List<string> nameList, PrePapers prepapers)
        {
            bool ValidPreScan = prepapers.PrePaperList.Exists(r => !nameList.Contains(r.ImgFilename));
            return !ValidPreScan;
        }
    }
    public class PrePapers
    {
        public PrePapers()
        {
            _PrePapers = new List<PrePaper>();
            _dic = null;
        }
        public PrePapers(List<PrePaper> PrePapers)
        {
            _PrePapers = new List<PrePaper>();
            foreach (PrePaper p in PrePapers)
                AddPrePaper(p);
            _dic = null;
        }
        public void AddPrePaper(PrePaper p)
        {
            _PrePapers.Add(p);
        }
        public void SavePrePapers(string Datafilename)
        {
            string str = Tools.JsonFormatTool.ConvertJsonString(
                Newtonsoft.Json.JsonConvert.SerializeObject(_PrePapers));
            File.WriteAllText(Datafilename, str);
        }
        public void LoadPrePapers(string Datafilename)
        {
            Clear();
            _PrePapers = 
            Newtonsoft.Json.JsonConvert.DeserializeObject<List<PrePaper>>(File.ReadAllText(Datafilename)); 
        }
        public void Clear()
        {
            _PrePapers.Clear();
            if (_dic != null)
            {
                _dic.Clear();
                _dic = null;
            }
        }
        public bool AllDetected()
        {
            return !_PrePapers.Exists(r => !r.Detected());
        }
        [JsonProperty]
        private List<PrePaper> _PrePapers;
        public List<PrePaper> PrePaperList { get { return _PrePapers; } }

        private Dictionary<string, PrePaper> _dic;
        public PrePaper GetPrepaper(string s)
        {
            if (_dic == null)
            {
                _dic = new Dictionary<string, PrePaper>();
                foreach (PrePaper p in _PrePapers)
                    _dic[p.ImgFilename] = p;
            }
            if (_dic.ContainsKey(s))
                return _dic[s];
            return null;
        }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class PrePaper
    {
        public PrePaper(string filename)
        {
            this._imgfilename = filename;
            Detectdata = null;
        }
        public void SetNewFileName(string imgfilename)
        {
            _imgfilename = imgfilename;
        }
        public string ToJsonString()
        {
            return Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(this));
        }
        public bool Detected()
        {
            if (Detectdata == null)
                return false;
            return  Detectdata.Detected;
        }
        [JsonIgnore]
        public string ImgFilename { get { return _imgfilename; } }
        [JsonIgnore]
        public Bitmap Src
        {
            get
            {
                if (_src == null)
                {
                    if (System.IO.File.Exists(_imgfilename))
                        _src = (Bitmap)Bitmap.FromFile(_imgfilename);
                }
                return _src;
            }
        }
        [JsonIgnore]
        public List<Rectangle> listFeatures
        {
            get
            {
                if (Detectdata == null)
                    return new List<Rectangle>();
                return Detectdata.ListFeature;
            }
        }
        [JsonProperty]
        public DetectData Detectdata { get; set; }
        [JsonProperty]
        private string _imgfilename;
        [JsonIgnore]
        private Bitmap _src;
    }
}
