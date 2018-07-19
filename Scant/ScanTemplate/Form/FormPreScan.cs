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
    enum DetectMode : short { Whole = 0, DetectArea = 1, DetectFeaturesArea = 2, DetectLT = 3, DetectLTninth, DetectLTSixteenth, DetectCorrectArea };

    public partial class FormPreScan : Form
    {
        public List<string> dms = new List<string>() {"全部", "检测范围","特征点范围","左上角范围","Correct范围"};
        public FormPreScan(UnScan dir)
        {
            InitializeComponent();
            _dir = dir;
            _fullpath = dir.Fullpath;
            _dirName = dir.DirName;
            _namelist = dir.ImgList();
            _PreActiveid = 0;
            _pp = new PrePapers();
            _Activedd = null;
            _fs = null;
            Init(null);
            _init = true;
        }
        ~FormPreScan()
        {
            Clear();
            Init(null);
        }

        public void SetUserMode()
        {
            //toolStrip1;
            //toolStrip2;
            _init = false;
            textBoxMessage.Visible = true; ;
            treeView1.Visible = false;
            //toolStripContainer1.Visible = false;
            toolStripDropDownButtonAutoDetect.Visible = false;
            //toolStripButtonDefineDetectArea.Visible = false;
            toolStripButtonDefineFeaturePointDetectArea.Visible = false;
            toolStripButtonScanLTDetectArea.Visible = false;
            //toolStripButtonAutDetect.Visible = false;
            toolStripButtonAutDetect.Text = "检查保存预检测数据";
            toolStripButtonPreDetect.Visible = false;
            toolStripButtonNextImage.Visible = false;
            toolStripButtonZoomout.Visible = false;
            toolStripButtonZoomin.Visible = false;
            //toolStripButtonZoomMouse.Visible = false;
            toolStripComboBoxImageMode.Visible = false;
            toolStripComboBoxDetectMode.Visible = false;
            NinthDetectToolStripMenuItem.Visible = false;
            SixteenthDetectToolStripMenuItem.Visible = false;
            toolStripComboBoxDetectMode.SelectedIndex = 0;
            toolStripComboBoxImageMode.SelectedIndex = 0;
            _init = true;
        }
        public void Clear()
        {
            if (_src != null)
            {
                _src = null;
            }
            if (_fs != null)
            {
                _fs.Close();
                _fs = null;
            }
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
                if (_dtm == DetectMode.DetectFeaturesArea)
                {
                    List<Rectangle> areas = ReadDetectAreas();
                    foreach (string s in _namelist)
                    {
                        _ListFeature = GetListFeature(areas, _src);
                        _dd = DetectImageTools.DetectCorrect.ConstructDetectData(_ListFeature);
                        PrePaper pp = new PrePaper(s);
                        pp.Detectdata = _dd;                       
                        if (File.Exists(s))
                            _pp.AddPrePaper( pp );
                    }
                }
                else
                {                   
                    PrePaper p = PreScanSelect(_namelist[0]);
                    int rept = 0;
                    while (!p.Detected() && rept ++ <10 )
                    {
                        if(rept<_namelist.Count)
                            p = PreScanSelect(_namelist[rept]);
                    }
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
                            _pp.AddPrePaper(PreScan(s, lrtb, p.listFeatures[0].Size));
                        }
                        if (!_pp.AllDetected()) // 对未成功模板重新检测
                        {
                            List<Rectangle> newlrtb = new List<Rectangle>();
                            Rectangle originLT = p.listFeatures[0];
                            originLT.Offset(p.Detectdata.CorrectRect.Location);
                            foreach (PrePaper fp in _pp.PrePaperList)
                            {
                                if (!fp.Detected())
                                {
                                    if (newlrtb.Count == 0)
                                        newlrtb = ResetNewLrtb(lrtb, originLT, fp);
                                    if (newlrtb.Count == 0)
                                        continue; //再次确认
                                    PrePaper newpp = PreScan(fp.ImgFilename, newlrtb, originLT.Size,false);
                                    if (!newpp.Detected())
                                    {
                                        newlrtb = ResetNewLrtb(lrtb, originLT, fp);
                                        if(newlrtb.Count>0)
                                            newpp = PreScan(fp.ImgFilename, newlrtb, originLT.Size,false);
                                    }
                                    if (newpp.Detected())
                                        fp.Detectdata = newpp.Detectdata;
                                    fp.RealseSrc();
                                }
                            }
                        }
                    }
                }
            }
            return _pp;
        }
        private void RePreScan(PrePapers _prepapers)
        {
            PrePaper okp = _prepapers.GetFirstCorrectPaper();
            if (okp == null)
            {
                _prepapers = PreScan();
                return;
            }
            ////////////////////////
            List<Rectangle> lrtb = new List<Rectangle>();
            foreach (Rectangle r in okp.listFeatures)
            {
                r.Inflate(r.Width / 2, r.Height / 2);
                r.Offset(okp.Detectdata.CorrectRect.Location);
                lrtb.Add(r);
            }
            foreach (PrePaper p in _prepapers.PrePaperList)
            {
                if (p.Detected()) continue;
                PrePaper np = PreScan(p.ImgFilename, lrtb, okp.listFeatures[0].Size);
                p.RealseSrc();
                if (np.Detected())
                {
                    p.Detectdata = np.Detectdata;
                    _prepapers._Changed = true;
                    continue;
                }
            }
            if (_prepapers.AllDetected()) return;
            //第二次重新 PreScan
            List<Rectangle> newlrtb = new List<Rectangle>();
            Rectangle originLT = okp.listFeatures[0];
            originLT.Offset(okp.Detectdata.CorrectRect.Location);
            foreach (PrePaper fp in _prepapers.PrePaperList)
            {
                if (!fp.Detected())
                {
                    if (newlrtb.Count == 0)
                        newlrtb = ResetNewLrtb(lrtb, originLT, fp);
                    if (newlrtb.Count == 0)
                        continue; //再次确认
                    PrePaper newpp = PreScan(fp.ImgFilename, newlrtb, originLT.Size, false);
                    if (!newpp.Detected())
                    {
                        newlrtb = ResetNewLrtb(lrtb, originLT, fp);
                        if (newlrtb.Count > 0)
                            newpp = PreScan(fp.ImgFilename, newlrtb, originLT.Size, false);
                    }
                    if (newpp.Detected())
                    {
                        fp.Detectdata = newpp.Detectdata;
                        _prepapers._Changed = true;
                    }
                    fp.RealseSrc();
                }
            }
        }
        private static List<Rectangle> ResetNewLrtb(List<Rectangle> lrtb, Rectangle originLT, PrePaper fp)
        {
            List<Rectangle> newlrtb = new List<Rectangle>();
            Rectangle DetectLT = lrtb[0];
            DetectLT.Width = fp.Src.Width / 3 - DetectLT.Left;
            Rectangle nLT = DetectLTPoint(DetectLT, fp.Src, originLT.Size, false);
            if (nLT.Width == 0 || nLT.Height == 0) //重复
            {
                Rectangle area = DetectLT;
                area.X = 0;
                while (true)
                {
                    if (area.X + 20 > fp.Src.Width / 6)
                        break;
                    area.X += 20;
                    area.Width = fp.Src.Width / 3 - area.X;
                    nLT = DetectLTPoint(area, fp.Src, originLT.Size, false);
                    if (nLT.Width > 0)
                        break;
                }
            }
            if (nLT.Width > 0 && nLT.Height > 0)
            {
                Point offset = new Point(nLT.X - originLT.X, nLT.Y - originLT.Y);            
                foreach (Rectangle r in lrtb)
                {
                    Rectangle rr = r;
                    rr.Offset(offset);
                    newlrtb.Add(rr);
                }
            }
            return newlrtb;
        }

        private PrePaper PreScanSelect(string s)
        {
            if(_dtm == DetectMode.Whole )
                return PreScan(s);
            if (_dtm == DetectMode.DetectArea)
            {
                Rectangle DetectArea = ReadDetectArea();
                if (DetectArea.Width > 0)
                {
                    return PreScan(s, DetectArea);
                }
            }
            if (_dtm == DetectMode.DetectFeaturesArea || _dtm == DetectMode.DetectCorrectArea) //可以直接检测 
            {
                List<Rectangle> areas = ReadDetectAreas();
                _ListFeature = GetListFeature(areas, _src);
                _dd = DetectImageTools.DetectCorrect.ConstructDetectData(_ListFeature);
                PrePaper pp = new PrePaper(s);
                pp.Detectdata = _dd;
                return pp;
            }
            return PreScan(s);
        }
        private static PrePaper PreScan(string s, List<Rectangle> lrtb,Size blocksize, bool LTflate = true) //根据模板，设置四个点，分别检测
        {
            Rectangle LT = lrtb[0]; //LT
            Point Center = new Point(LT.X + LT.Width / 2, LT.Y + LT.Height / 2);
             
            PrePaper pp = new PrePaper(s);
            using (FileStream fs = new System.IO.FileStream(s, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                Bitmap src = (Bitmap)System.Drawing.Image.FromStream(fs);
                Rectangle area = new Rectangle(new Point(), src.Size);

                Rectangle nrLT = DetectLTPoint(LT, src, blocksize,LTflate );
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
                        if (r.Width < 5 || r.Height < 5)
                            break;
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
        private static Rectangle DetectLTPoint(Rectangle LT, Bitmap src, Size blocksize, bool LTflate = true )
        {
            //Size blocksize = new Size(LT.Width/2,LT.Height/2);
            Rectangle nrLT = new Rectangle();
            Rectangle area = new Rectangle(new Point(), src.Size);
            Rectangle detectarea = LT;

            if (LTflate)
            {
                detectarea.Inflate(detectarea.Width / 3, detectarea.Height / 3);
                detectarea.Intersect(area);
                nrLT = Tools.DetectImageTools.DetectCorrect.DetectCorrectFromImg(src, detectarea, true, detectarea.Width / 5);
            }
            else
            {
                nrLT = Tools.DetectImageTools.DetectCorrect.DetectCorrectFromImg(src, detectarea, true, blocksize.Width*4 / 5);
            }

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
                    detectarea.Intersect(area);
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
                Rectangle srcarea = new Rectangle(new Point(), src.Size);
                List<int> inflaterate = new List<int>() { 30, src.Width / 5};
                int circlecount = inflaterate.Count;
                int index = 0;
                for (int i = 0; i < circlecount; i++)
                {
                    Rectangle area = srcarea;
                    area.Inflate(-src.Width / inflaterate[index], -src.Height / inflaterate[index]);
                    DetectData dd = DetectImageTools.DetectImg(src, area, new Rectangle());
                    if (dd.Detected  )
                    if(dd.CheckListFeature(src))
                    {
                        Rectangle LT = dd.ListFeature[0];
                        LT.Offset(dd.CorrectRect.Location);
                        Rectangle RB = dd.ListFeature[3];
                        RB.Offset(dd.CorrectRect.Location);

                        if (DetectImageTools.CheckWholeDetectBlock(src, srcarea, LT))
                        if( DetectImageTools.CheckWholeDetectBlock(src, srcarea, RB))
                        {
                            pp.Detectdata = dd;
                            break; 
                        }
                    }
                    index++;
                    index %= circlecount;
                }
            }
            return pp;
        }

        
        private static PrePaper PreScan(string s,Rectangle darea)
        {
            PrePaper pp = new PrePaper(s);
            using (FileStream fs = new System.IO.FileStream(s, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                Bitmap src = (Bitmap)System.Drawing.Image.FromStream(fs);               
                {
                    Rectangle area = new Rectangle(new Point(), src.Size);
                    darea.Intersect(area);
                    DetectData dd = DetectImageTools.DetectImg(src,darea, new Rectangle());
                    if (dd.Detected)
                    {
                        pp.Detectdata = dd;
                    }
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
            if (_pp != null) ;
            listBox1.Items.AddRange(_pp.PrePaperList.ToArray());
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
            if (bcomplete && _init)
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
            else if (str == "Correct范围")
            {
                _dtm = DetectMode.DetectCorrectArea;
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
            if (global.UserMode)
            {
                _pp.CheckSavePrePapers(_dir.Fullpath + ".prescanpapers.json");
                if (_pp.AllDetected())
                {
                    if(_pp.AllDetectedSize())
                        MessageBox.Show("已生成或校验全部预检测数据，且全部正确");
                    else
                        MessageBox.Show("请检查不匹配的图像，请修改正确，或者移出当前文件夹，以跳过本次扫描，然后重试");
                }
                return;
            }
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
            UnScan dir = _dir;
            if (dir.ImgList().Count == 0) return;
            bool ExistOKScanJson = PreCheckJsonFile(dir);
            if (ExistOKScanJson)
            {
                MessageBox.Show("已生成或校验全部预检测数据，且全部正确");
            }
            else
            {
                MessageBox.Show(" 预检测数据无法完整生成，请检查扫描图片是否全部正确");
            }
        }
        
        private Rectangle ReadDetectArea(String keyname = "检测范围")
        {
            int cnt = m_tn.Nodes[keyname].GetNodeCount(false);
            if (cnt == 0)
            {
                if(!global.UserMode)
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
            //TreeNode t = m_tn.Nodes[keyname].Nodes[0];
            if (keyname == "特征点范围" && areas.Count  == 4)
            {
                LTBRTBTools.CheckSetListLTBRTB(ref areas);
            }
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
            Rectangle DetectArea = ReadDetectArea("左上角范围");
            List<Rectangle> areas = GetNNthDetectAreas(DetectArea, n);
            Size blocksize =  _dd.ListFeature[0].Size;
            bool ok = false;
            foreach (Rectangle r in areas)
            {
                //if (DetectLT(r) && DetectOther3Point()) { ok = true; break; }
                Rectangle LT = DetectLTPoint(r, _src, blocksize,false);
                if (LT.Width > blocksize.Width * 7 / 10 && LT.Height > blocksize.Height * 7 / 10)
                {
                    Rectangle pb = zoombox.ImgToBoxSelection(LT);
                    MT.Selection = pb;
                    CompleteDeFineDetectArea();
                    ok = true;
                    break;
                }
            }
            if (!ok)
                MessageBox.Show("检测失败");
            else
                MessageBox.Show("检测成功");
        }
        private void AutoDetectByFeaturesArea()
        {
            List<Rectangle> areas = ReadDetectAreas();
            _ListFeature = GetListFeature(areas, _src);
            _dd =  DetectImageTools.DetectCorrect.ConstructDetectData(_ListFeature);
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
                                e.Graphics.DrawRectangle(Pens.DarkGreen, zoombox.ImgToBoxSelection(r));
                            }
                        }
                    }
                }
                if (_Activedd != null && _Activedd.Detected)
                {
                    Rectangle r = _Activedd.CorrectRect;
                    DrawRect(e, _Activedd.CorrectRect,Pens.DarkRed);
                    DrawRects(e, _Activedd.ListFeature,_Activedd.CorrectRect.Location,Pens.DarkRed);
                }else
                if (!global.UserMode &&  _dd != null && _dd.Detected)
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
        private void Zoomratzero( )
        {
            Bitmap bitmap_show = (Bitmap)pictureBox1.Image;
            pictureBox1.SetBounds(0, 0, pictureBox1.Width,pictureBox1.Height);
            zoombox.UpdateBoxScale(pictureBox1);
            panelRT.Invalidate();
            panelRT.AutoScrollPosition = new Point();
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
        private UnScan _dir;
        private DetectData _Activedd;
        private bool _init;
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
                if (File.Exists(dir.Fullpath + ".prescanpapers.json"))
                {
                    _prepapers.LoadPrePapers(dir.Fullpath + ".prescanpapers.json");
                    bool ValidNamelist = ValidPreScanData(dir.ImgList(), _prepapers);
                    if (!ValidNamelist)
                    {
                        _prepapers.ReContructPapers(dir.ImgList());
                        _prepapers.SavePrePapers(dir.Fullpath + ".prescanpapers.json");
                        RePreScan(_prepapers);
                        _prepapers.SavePrePapers(dir.Fullpath + ".prescanpapers.json");
                        //File.Delete(dir.Fullpath + ".prescanpapers.json");
                    }
                }
                else //  if (!ExistOKScanJson)
                {
                    _prepapers = PreScan();
                    _prepapers.SavePrePapers(dir.Fullpath + ".prescanpapers.json");
                }
                if (_prepapers.AllDetected()) // 已成功预扫描
                {
                    if (_prepapers.AllDetectedSize())
                        ExistOKScanJson = true;
                    else
                    {
                        _prepapers.CheckSizeInfo();
                        ExistOKScanJson = false;
                        _pp = _prepapers;
                    }
                }
                else
                {
                    ExistOKScanJson = false;
                    _pp = _prepapers;
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
            List<string> namelist2 = prepapers.PrePaperList.Select( r => r.ImgFilename).ToList();
            bool ValidPreScan2 = nameList.Exists( r => !namelist2.Contains(r));
          
            return !ValidPreScan && !ValidPreScan2 ;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            PrePaper pp = (PrePaper)listBox1.SelectedItem;

            pictureBox1.Image = pp.Src;
            _src = pp.Src;
            if (pp.Detected())
            { 
                _Activedd = pp.Detectdata;
                Zoomrat(1, new Point(0, 0));
            }
            else
            {
                _Activedd = null;
                PrePaper newpp;
                if(global.UserMode)
                    newpp = PreScanByArea(pp.ImgFilename);
                else
                    newpp = PreScanBydd(pp.ImgFilename);
                if (newpp != null)
                {
                    pp.Detectdata = newpp.Detectdata;
                    _pp._Changed = true;
                    _Activedd = pp.Detectdata;
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(_pp.PrePaperList.ToArray());
                    Zoomrat(1, new Point(0, 0));
                }
                else
                {
                    pictureBox1.Width =(int) _OriginWith;
                    pictureBox1.Height =(int)(  _OriginWith * _src.Height / _src.Width);
                    pictureBox1.Invalidate();
                    zoombox.UpdateBoxScale(pictureBox1);
                   
                }
            }
        }

        private PrePaper PreScanByArea(string s)
        {
            Rectangle DetectArea = ReadDetectArea();
            if (DetectArea.Width > 0)
                try
                {
                    DetectData dd = DetectImageTools.DetectImg(_src, DetectArea);
                    dd = DetectImageTools.DetectCorrect.ReDetectCorrectImg(_src, dd);
                    if (dd.CorrectRect.Width > 0)
                    {
                        PrePaper pp = new PrePaper(s);
                        pp.Detectdata = dd;
                        if (pp.Detected())
                            return pp;
                    }
                }
                catch { }           
            return null;
        }
        private PrePaper PreScanBydd(string ImgFilename)
        {
            if (_dd != null && _dd.ListFeature.Count > 0)
            {
                Rectangle LT = _dd.ListFeature[0];
                Rectangle DetectLT = LT;
                DetectLT.Offset(_dd.CorrectRect.Location);
                DetectLT.Inflate(DetectLT.Width * 3 / 2, DetectLT.Height * 3 / 2);
                DetectLT.Intersect(new Rectangle(0, 0, _src.Width * 4 / 2, _src.Height));
                int right = _src.Width / 3;
                DetectLT.Width = right - DetectLT.Left;

                Rectangle nLT = DetectLTPoint(DetectLT, _src, LT.Size, false);
                if (nLT.Width > 0 && nLT.Height > 0)
                {
                    List<Rectangle> lrtb = new List<Rectangle>();
                    foreach (Rectangle r in _dd.ListFeature)
                    {
                        r.Inflate(r.Width / 2, r.Height / 2);
                        r.Offset(nLT.Location);
                        lrtb.Add(r);
                    }
                    PrePaper newpp = PreScan(ImgFilename, lrtb, LT.Size);
                    if (newpp.Detected())
                        return newpp;
                }
            }
            return null;
        }
    }
    public class PrePapers
    {
        public PrePapers()
        {
            _PrePapers = new List<PrePaper>();
            _dic = null;
            _Changed = false;
        }
        public PrePapers(List<PrePaper> PrePapers)
        {
            _PrePapers = new List<PrePaper>();
            foreach (PrePaper p in PrePapers)
                AddPrePaper(p);
            _dic = null;
            _Changed = false;
        }
        public void AddPrePaper(PrePaper p)
        {
            _PrePapers.Add(p);
            _Changed = true;
        }
        public void SavePrePapers(string Datafilename)
        {
            string str = Tools.JsonFormatTool.ConvertJsonString(
                Newtonsoft.Json.JsonConvert.SerializeObject(_PrePapers));
            File.WriteAllText(Datafilename, str);
            _Changed = false;
        }
        public void LoadPrePapers(string Datafilename)
        {
            Clear();
            _PrePapers = 
            Newtonsoft.Json.JsonConvert.DeserializeObject<List<PrePaper>>(File.ReadAllText(Datafilename));
            _Changed = false;
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
            if (_PrePapers.Count == 0)
                return false;
            return !_PrePapers.Exists(r => !r.Detected());
        }
        public bool AllDetectedSize()
        {
            if (_PrePapers.Count == 0) return false;
            if (_PrePapers[0].Detectdata == null) return false;
            Size size = _PrePapers[0].Detectdata.CorrectRect.Size;
            foreach (PrePaper p in _PrePapers)
            {
                if (!p.Detected()) return false;
                Size size1 = p.Detectdata.CorrectRect.Size;
                double xrate = size.Width*1.0 / size1.Width;
                double yrate = size.Height*1.0 / size1.Height;
                if (xrate > 1.1 || xrate < 0.9 || yrate > 1.1 || yrate < 0.9)
                {
                    return false;
                }
            }
            return true;
        }
        public void CheckSizeInfo()
        {
            PrePaper okp = GetFirstCorrectPaper();
            if (okp == null) return;
            Size size =okp.Detectdata.CorrectRect.Size;
            foreach (PrePaper p in _PrePapers)
            {
                if (!p.Detected()) continue;
                Size size1 = p.Detectdata.CorrectRect.Size;
                double xrate = size.Width * 1.0 / size1.Width;
                double yrate = size.Height * 1.0 / size1.Height;
                if (xrate > 1.1 || xrate < 0.9 || yrate > 1.1 || yrate < 0.9)
                    p.Msg = "模板大小不匹配";
                else
                    p.Msg = "";
            }
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

        public void ReContructPapers(List<string> list)
        {
            _Changed = true;
            _dic = _PrePapers.ToDictionary(r => r.ImgFilename, r => r);
            _PrePapers.Clear();
            foreach (string s in list)
            {
                if (_dic.ContainsKey(s))
                    _PrePapers.Add(_dic[s]);
                else
                    _PrePapers.Add(new PrePaper(s));
            }
            _dic.Clear();
            _dic = null;
        }
        public  PrePaper GetFirstCorrectPaper()
        {
            foreach (PrePaper p in PrePaperList)
                if (p.Detected())
                    return p;
            return null;
        }
        public  void CheckSavePrePapers(string p)
        {
            if (_Changed)
            {
                SavePrePapers(p);
                _Changed = false;
            }
        }
        public bool _Changed { get; set; }
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
        public override string ToString()
        {
            string sif = _imgfilename;
            if (sif.Contains("\\"))
            {
                sif = sif.Substring(sif.LastIndexOf("\\"));
            }
            return ( Detected()?"":"检测错误 "  ) + Msg +" "+ sif ;
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

        public  void RealseSrc()
        {
            if (_src != null)
            {
                _src.Dispose();                
                _src = null;
            }
        }

        public string Msg { get; set; }
    }
}
