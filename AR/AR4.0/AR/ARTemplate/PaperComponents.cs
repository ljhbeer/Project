using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Drawing.Imaging;
using AForge.Math.Geometry;

namespace AR
{
    interface ISelectionInterface
    {
        Rectangle ImgSelection();
        bool HasSubSelection();
        Rectangle[] ImgSubSelection();
    }
    public class TriAngleFeature : ISelectionInterface
    {
        public TriAngleFeature(List<Point> list)
        {
            if (list.Count != 3)
                throw new System.Exception("点的数目不对");
            this.corners = list;
            Init();
        }
        public TriAngleFeature(List<Point> list, Point offset)
        {
            if (list.Count != 3)
                throw new System.Exception("点的数目不对");
            this.corners = list;
            for (int i = 0; i < corners.Count; i++)
            {
                corners[i] = new Point(corners[i].X + offset.X, corners[i].Y + offset.Y);
            }
            Init();
            //this.ImgSelection = imgrect;
            //this.BoxSelection = boxrect;

        }
        public Point CornerPoint()
        {
            return new Point(corners[rightpos].X, corners[rightpos].Y);
        }
        public int Direction { get; set; }
        public bool IntersectsWith(Rectangle rect)
        {
            return imgselection.IntersectsWith(rect);
        }
        private Rectangle imgselection;
        public Rectangle  ImgSelection(){ return imgselection; }
        public bool HasSubSelection() { return true; }
        public Rectangle[] ImgSubSelection()
        {
            return new Rectangle[]{new Rectangle(imgselection.X - 2 * imgselection.Width,
                imgselection.Y - 2 * imgselection.Height,
                imgselection.Width * 5, imgselection.Height * 5)};
        }
        internal Rectangle BigImgSelection()
        {
           return  new Rectangle(imgselection.X - 2 * imgselection.Width,
                imgselection.Y - 2 * imgselection.Height,
                imgselection.Width * 5, imgselection.Height * 5);
        }
        public String ToXmlString()
        {
            String str = "";
            foreach (Point p in corners)
            {
                str+="<POINT>"+p.X+","+p.Y+"</POINT>";
            }
            return str;
        }
        public int Distance(Point a, Point b)
        {
            return (int)(Math.Sqrt( (a.X-b.X)*(a.X-b.X) + (a.Y-b.Y)*(a.Y-b.Y)));
        }
        private void Init()
        {
            line = new double[3];
            cos = new double[3];
            rightpos = -1;
            for (int i = 0; i < 3; i++)
            {
                int b = (i + 1) % 3;
                int c = (i + 2) % 3;
                line[i] = new double();
                cos[i] = new double();
                line[i] =Distance(corners[b],corners[c]);
            }
            for (int i = 0; i < 3; i++)
            {
                int b = (i + 1) % 3;
                int c = (i + 2) % 3;
                cos[i] = (Math.Pow(line[b], 2) + Math.Pow(line[c], 2) - Math.Pow(line[i], 2)) / (2 * line[b] * line[c]);
                if (Math.Abs(cos[i]) < 0.10)
                    rightpos = i;
            }
            if (rightpos == -1)
                throw new System.Exception("不是正三角形");
            {
                Direction = 0;
                int a = rightpos;
                int b = (rightpos + 1) % 3;
                int c = (rightpos + 2) % 3;
                double x2 = (corners[b].X + corners[c].X) / 2;
                double y2 = (corners[b].Y + corners[c].Y) / 2;
                if (corners[a].X > x2 + 3)
                    Direction += 1;
                if (corners[a].Y > y2 + 3)
                    Direction += 2;
                //
                int width = (int)(Math.Abs(corners[a].X - x2) * 2) - 1;
                int height = (int)(Math.Abs(corners[a].Y - y2) * 2) - 1;
                int X = corners[a].X < x2 ? corners[a].X : corners[a].X - width;
                int Y = corners[a].Y < y2 ? corners[a].Y : corners[a].Y - height;
                this.imgselection = new Rectangle(X, Y, width, height);

            }
        }
        private List<Point> corners;
        private double[] cos;
        private double[] line;
        private int rightpos;
    }
    public class SingleChoice :  ISelectionInterface
    {
        private Rectangle rect;
        private Rectangle rectangle;
        private string text;
        public SingleChoice(Rectangle rect)
        {
            this.rect = rect;
        }       
        public SingleChoice(Rectangle rectangle, string text)
        {
            this.rectangle = rectangle;
            this.text = text;
        }
        public override string ToString()
        {
            return text;
        }
        public Rectangle ImgSelection() { return new Rectangle(); }
        public bool HasSubSelection() { return false; }
        public Rectangle[] ImgSubSelection() { return new Rectangle[0]; }
        internal string ToXmlString()
        {
            return "";
        }
    }
    public class SingleChoiceArea : ISelectionInterface
    {
        public SingleChoiceArea(Rectangle imgrect, string name)
        {
            this.imgselection = imgrect;
            this.name = name;
        }
        public SingleChoiceArea(SingleChoice[] sc)
        {
            this.scv = sc;
        }
        public SingleChoiceArea(Rectangle m_Imgselection, string name, List<List<Point>> list, Size size)
        {
            this.imgselection = m_Imgselection;
            this.name = name;
            this.list = list;
            this.size = size;
        }
        internal bool IntersectsWith(Rectangle rect)
        {
            return this.imgselection.IntersectsWith(rect);
        }
        public Rectangle ImgSelection()
        {
            return imgselection;
        }
        public bool HasSubSelection() { return true; }
        public Rectangle[] ImgSubSelection() { 
            int count = 0;
            foreach(List<Point> l in list)
                count += l.Count;
            if(count == 0 ) return null;
            Rectangle[] rv = new Rectangle[count];
            int i = 0;
            foreach (List<Point> l in list)
            {
                foreach (Point p in l)
                {
                    rv[i] = new Rectangle(p, size);
                    i++;
                }
            } 
            return rv; 
        }
        internal string ToXmlString()
        {
            String str = "";
            String strp = "";
            int i = 0;
            str += "<RECTANGLE>" + imgselection.X + "," + imgselection.Y + "," + imgselection.Width + "," + imgselection.Height + "</RECTANGLE>"
                    + "<NAME>" + name + "</NAME>" + "<SIZE>"+size.Width+","+size.Height+"</SIZE>";
            foreach (List<Point> lp in list)
            {
                strp = "";
                foreach(Point p in lp)
                    strp += "<POINT>" + p.X + "," + p.Y + "</POINT>";
                str += "<SINGLE ID=\""+i+"\">" + strp + "</SINGLE>";
                i++;
            }
            return str;
        }
        internal int Count()
        {
            return list.Count;
        }

        public Rectangle imgselection { get; set; }
        private SingleChoice[] scv;
        private string name;
        public List<List<Point>> list;
        public Size size;
        internal void ComputeChoice(Image image, Point offset, List<int> ret)
        {
            List<List<int>> blackcnt = new List<List<int>>();
            Bitmap bmp = (Bitmap)image;
            foreach (List<Point> lp in list)
            {
                int maxcnt = size.Height * size.Width;
                int choosecnt = maxcnt * 3/10;
                int choosei = -1;
                List<int> cnt = new List<int>();
                int option = 0;
                foreach (Point p in lp)
                {
                    Rectangle r = new Rectangle(p, size);
                    r.Offset(offset);
                    r.Offset(imgselection.Location);
                    //FormShowImg f3 = new FormShowImg();
                    //Image cropimg = bmp.Clone(r, bmp.PixelFormat);
                    //f3.SetImg(cropimg);
                    //f3.ShowDialog();
                    int tcnt = DetectChoiceArea.CountRectBlackcnt(bmp, r);
                    if (tcnt > choosecnt)
                    {
                        if (choosei == -1)
                        {
                            choosei = option;
                        }
                        else if(choosei>=0)
                        {
                            choosei = -option;
                        }
                    } 
                    //DetectChoiceArea.CountRectBlackcnt(bmp, r);
                    cnt.Add(tcnt );
                    option++;
                }
                ret.Add(choosei);
                blackcnt.Add(cnt);
            }
        }
    }
    public class UnChoose : ISelectionInterface
    {
        public float Score { get; set; }
        public string Name { get; set; }
        public Rectangle Imgrectangle { get; set; }
        public UnChoose(float score, string name, Rectangle imgrect)
        {
            this.Score = score;
            this.Name = name;
            this.Imgrectangle = imgrect;
        }
        public Rectangle ImgSelection() {return Imgrectangle; }
        public bool HasSubSelection() { return false; }
        public Rectangle[] ImgSubSelection() { return null; }

        public bool IntersectsWith(Rectangle rect)
        {
            return Imgrectangle.IntersectsWith(rect);
        }
        public int IntScore { get { return (int)Score; } }
        public override String ToString()
        {
            return Name;
        }
        public string ToXmlString()
        {
            String str = "";
            {
                str += "<RECTANGLE>" + Imgrectangle.X + "," + Imgrectangle.Y + "," + Imgrectangle.Width + "," + Imgrectangle.Height + "</RECTANGLE>"
                    + "<NAME>"+Name+"</NAME>" + "<SCORE>"+Score+"</SCORE>";
            }
            return str;
        }
    }
    public class ZoomBox
    {
        public ZoomBox()
        {
            Reset();
        }
        private void Reset()
        {
            img_location = new Point(0, 0);
            img_scale = new SizeF(1, 1);
        }
        public Rectangle ImgToBoxSelection(Rectangle rectangle)
        {
            RectangleF r = rectangle;
            r.X /= img_scale.Width;
            r.Y /= img_scale.Height;
            r.Width /= img_scale.Width;
            r.Height /= img_scale.Height;
            r.Offset(img_location.X, img_location.Y);
            return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
        }
        public Rectangle BoxToImgSelection(Rectangle rectangle)
        {
            RectangleF r = rectangle;
            r.Offset(-img_location.X, -img_location.Y);
            r.X *= img_scale.Width;
            r.Y *= img_scale.Height;
            r.Width *= img_scale.Width;
            r.Height *= img_scale.Height;
            return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
        }
        public void UpdateBoxScale(PictureBox pictureBox1)
        {
            if (pictureBox1.Image != null)
            {
                Rectangle imgrect = GetPictureBoxZoomSize(pictureBox1);
                System.Drawing.SizeF size = pictureBox1.Image.Size;
                img_location = imgrect.Location;
                img_scale = new SizeF((float)(size.Width * 1.0 / imgrect.Width), (float)(size.Height * 1.0 / imgrect.Height));
            }
            else
            {
                Reset();
            }
        }
        public static Rectangle GetPictureBoxZoomSize(System.Windows.Forms.PictureBox p_PictureBox)
        {
            if (p_PictureBox != null)
            {
                System.Reflection.PropertyInfo _ImageRectanglePropert = p_PictureBox.GetType().GetProperty("ImageRectangle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                return (System.Drawing.Rectangle)_ImageRectanglePropert.GetValue(p_PictureBox, null);
            }
            return new System.Drawing.Rectangle(0, 0, 0, 0);
        }
        private SizeF img_scale;
        private Point img_location;
    }
    public class Paper
    {
        public Paper(Papers papers,String filename,List<TriAngleFeature> list,int id)
        {
            if (list.Count != 3)
                return;
            List<Point> _list = list.Select(r => r.CornerPoint()).ToList();
            Init(papers, filename, _list, id);
        }
        public Paper(Papers papers, String filename, List<Point> list, int id)
        {
            Init(papers, filename, list, id);
        }
        private void Init(Papers papers, String filename, List<Point> list, int id)
        {
            cp = list[0];
            rp = list[1];
            bp = list[2];
            FileName = filename;
            this.ID = id;
            _img = null;
            _papers = papers;
            IntBlocks = new List<int>();
            Optionanswers = new List<int>();
            _paperblocks = new List<PaperBlock>();
            _dictpaperblocks = new Dictionary<PaperBlockTemplate, PaperBlock>();
        }
        public PaperBlock PaperBlock(PaperBlockTemplate pbt)
        {
            if (_dictpaperblocks.ContainsKey(pbt))
                return _dictpaperblocks[pbt];
            return null;
        }
        public void InitPaperBlock(List<PaperBlockTemplate> pbts)
        {
            _dictpaperblocks.Clear();
            _paperblocks.Clear();            
            foreach (PaperBlockTemplate pbt in pbts)
            {
                PaperBlock pb = new PaperBlock(this, pbt);
                _paperblocks.Add(pb);
                _dictpaperblocks[pbt] = pb;               
            }
        }
        public string ToXml()
        {
            string option = "";
            if (Optionanswers != null)
            {
                option = "<OPTIONANSWER>";
                foreach (int i in Optionanswers)
                    option += i + ",";
                option = option.Substring(0,option.Length-1) + "</OPTIONANSWER>";
            }
            return PointToXml(cp) + PointToXml(rp) + PointToXml(bp) + option;
        }
        public string BlocksToXml()
        {
            string ret = "";
            if (_paperblocks.Count>0)
            {
                ret = "<BLOCKSCORE>";
                foreach (PaperBlock p in _paperblocks)
                    ret += p.BlockScore.GetScore() + ",";
                ret = ret.Substring(0, ret.Length - 1) + "</BLOCKSCORE>";
            }
            return ret;
        }

        public string PointToXml(Point p)
        {
            return "<POINT>" + p.X + "," + p.Y + "</POINT>";
        }
        public Point CornerPoint()
        {
            return cp;
        }
        public Point RightPoint()
        {
            return rp;
        }
        public double Angle
        {
            get
            {
                if (_angle < -1000)
                {
                    CnblogsDotNetSDK.Utility.Geometry.Vector2f vf1 =
                        new CnblogsDotNetSDK.Utility.Geometry.Vector2f(rp.X - cp.X, rp.Y - cp.Y);
                    _angle = vf1.GetDirection();
                }
                return _angle;
            }
        }
        public Image Img
        {
            get
            {
                if (_img == null)
                {
                    _img = Image.FromFile(FileName);
                }
                return _img;
            }
        }    
        public String FileName { get; set; }
        public int ID { get; set; }
        public String NodeName { get { return "/PAPERS"; } }
        public List<PaperBlock> PaperblockList() { return _paperblocks; }
        public List<int> IntBlocks { get; set; }
        public List<int> Optionanswers { get; set; }

        private Point cp;
        private Point rp;
        private Point bp;
        private Image _img;
        private double _angle;
        private Papers _papers;
        private Dictionary<PaperBlockTemplate, PaperBlock> _dictpaperblocks;
        private List<PaperBlock> _paperblocks;


        internal void InitPaperBlockScore()
        {
            if(IntBlocks.Count == _paperblocks.Count)
                for (int i = 0; i < _paperblocks.Count; i++)
                {
                    _paperblocks[i].BlockScore.SetScore(IntBlocks[i]);
                }
        }
    }
    public class Papers
    {
        public PaperTemplate Papertemplate{ get; set; }
        public List<Paper> PaperList { get { return _papers; } }
        public Papers(PaperTemplate Template)
        {
            this.Papertemplate = Template;
            _papers = new List<Paper>();
            _dictpapers = new Dictionary<int, Paper>();
            _paperblocks = new List<PaperBlocks>();
            _paperscores = new PaperScores();
        }
        public void InitReadDatas()
        {
            //MessageBox.Show("重新生成阅卷数据，这将删除原有成绩！！");
            foreach (PaperBlocks pbs in _paperblocks)
                pbs.Clear();
            _paperblocks.Clear();
            List<PaperBlockTemplate> paperblocktemplates = Papertemplate.BlockTemplates();
            foreach (PaperBlockTemplate pbt in paperblocktemplates)
            {
                PaperBlocks pb = new PaperBlocks(pbt);
                _paperblocks.Add(pb);                
            }
            ////// InitBlocks
            foreach (Paper p in _papers)
                p.InitPaperBlock(paperblocktemplates);
            foreach(PaperBlocks pb in _paperblocks)
                pb.AddBlocks(_papers);
            _paperscores.Init(_papers, _paperblocks);
            foreach (Paper p in _papers)
                p.InitPaperBlockScore( );

        }        
        public IEnumerable<PaperBlocks> PaperBlocks()
        {
            return _paperblocks;
        }
        public void Clear()
        {
            _papers.Clear();
            _dictpapers.Clear();
        }
        public void AddPapers(List<Image> imgs)
        {
            foreach (Image img in imgs)
            {
                List<TriAngleFeature> listta = new List<TriAngleFeature>();
                try
                {
                    foreach (TriAngleFeature ta in Papertemplate.FeaturePoints)
                    {
                        List<Point> corners = new List<Point>();
                        Bitmap bmp = (Bitmap)img;
                        Image cropimg = bmp.Clone(ta.BigImgSelection(), bmp.PixelFormat);
                        Bitmap img8 = ConvertFormat.Convert((Bitmap)cropimg, PixelFormat.Format8bppIndexed, true);
                        if (!CheckImageRectangledTriangle(img8, out corners))
                            continue;
                        listta.Add(new TriAngleFeature(corners, ta.BigImgSelection().Location));
                    }
                }
                catch
                {
                    img.Dispose();
                    continue;
                }
                int ID = _papers.Count;
                if (!_dictpapers.ContainsKey(ID))
                {
                    Paper paper = new Paper(this, (string)img.Tag, listta, ID);
                    //paper.Optionanswers = Papertemplate.ComputeChoice(img, paper.CornerPoint());
                    _papers.Add(paper);
                    _dictpapers[ID] = paper;
                }
                else
                {
                    MessageBox.Show("程序逻辑错误，无法添加试卷: Papers.AddPapers()");
                }

            }            
        }
        public void AddPapers(List<Paper> papers)
        {
            _papers = papers;
            foreach (Paper p in papers)
                _dictpapers[p.ID] = p;
        }
        //public void AddPapers(Paper paper)
        //{
        //    throw new NotImplementedException();
        //}
        public Paper PaperFromID(int ID)
        {
            if (_dictpapers.ContainsKey(ID))
                return _dictpapers[ID];
            return null;
        }
        private static bool CheckImageRectangledTriangle(Bitmap bitmap, out List<System.Drawing.Point> drawcorners)
        {
            // lock image
            List<AForge.IntPoint> corners;
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);
            // step 1 - turn background to black
            unsafe
            {
                byte* ptr = (byte*)(bitmapData.Scan0);
                for (int i = 0; i < bitmapData.Height; i++)
                {
                    for (int j = 0; j < bitmapData.Width; j++)
                    {
                        if (*ptr == 0)
                            *ptr = 255;
                        else *ptr = 0;
                        ptr++;
                    }
                    ptr += bitmapData.Stride - bitmapData.Width;
                }
            }
            // step 2 - locating objects
            AForge.Imaging.BlobCounter blobCounter = new AForge.Imaging.BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 5;
            blobCounter.MinWidth = 5;
            blobCounter.ProcessImage(bitmapData);
            AForge.Imaging.Blob[] blobs = blobCounter.GetObjectsInformation();
            bitmap.UnlockBits(bitmapData);

            // step 3 - check objects' type and highlight
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<AForge.IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                if (shapeChecker.IsTriangle(edgePoints, out corners))
                {
                    PolygonSubType subType = shapeChecker.CheckPolygonSubType(corners);
                    if (subType == PolygonSubType.RectangledTriangle || subType == PolygonSubType.RectangledIsoscelesTriangle)
                    {
                        drawcorners = new List<System.Drawing.Point>();
                        foreach (AForge.IntPoint p in corners)
                            drawcorners.Add(new System.Drawing.Point(p.X, p.Y));
                        return true;
                        //MessageBox.Show("triangle" + corners[0] + corners[1] + corners[2]);
                        //g.DrawPolygon(greenPen, ToPointsArray(corners));
                    }
                }
            }
            drawcorners = null;
            return false;
        }

        private Dictionary<int, Paper> _dictpapers;
        private List<Paper> _papers;
        private List<PaperBlocks> _paperblocks;
        private PaperScores _paperscores;


    }
    public class PaperBlockTemplate
    {
        public PaperBlockTemplate(UnChoose uc)
        {
            this.uc = uc;
            int hcnt = 1;
            ScoreLen = ScoresImg.GetWcnt(IntScore, Height, ref hcnt);
            int scorewidth = (int)(30 * ScoreLen);
            ImgScoreRect = new Rectangle(Width, 0, scorewidth, Height);
            ImgPageRect = new Rectangle(0, 0, Width + scorewidth + 5, Height + 5);

            ScoreLocations = new List<Point>();
            ScoresImg.SetScorelocation(ScoreLocations, Height, IntScore);
            ScoreRectangles = new List<Rectangle>();
            foreach (Point p in ScoreLocations)
                ScoreRectangles.Add(new Rectangle(p, new Size(30, 30)));
        }
        public Rectangle ImgSizeRectangle { get { return new Rectangle(0, 0, Width, Height); } }
        public Rectangle Imgrectangle { get { return uc.Imgrectangle; } }
        public String Name { get { return uc.Name; } }
        public int IntScore { get { return uc.IntScore; } }
        public int Width { get { return uc.Imgrectangle.Width; } }
        public int Height { get { return uc.Imgrectangle.Height; } }
        public int ScoreLen { get; set; }
        public Point Location { get { return uc.Imgrectangle.Location; } }
        public Size Size { get { return uc.Imgrectangle.Size; } }

        public Rectangle ImgScoreRect { get; set; }
        public Rectangle ImgPageRect { get; set; }
        public List<Point> ScoreLocations { get; set; }
      
        public bool Contains(Point point)
        {
            if (ImgScoreRect.Contains(point))
            {
                return true;
            }
            return false;
        }
        public int ComputeMaxCount(Size PageSize)
        {
            return (PageSize.Height / ImgPageRect.Height) * (PageSize.Width / ImgPageRect.Width);
        }
        public int ComputeMaxHeightCount(Size PageSize)
        {
            return (PageSize.Height / ImgPageRect.Height);
        }
        public override string ToString()
        {
            return Name;
        }
        private int GetScore(Point point)
        {
            point.X -= Width;
            for (int i = 0; i < IntScore + 1; i++)
                if (ScoreRectangles[i].Contains(point))
                    return i;
            return -1;
        }

        private UnChoose uc;
        private List<Rectangle> ScoreRectangles;
    }
    public class PaperBlock
    {
        private PaperBlockTemplate pbt;
        public PaperBlock(Paper p, PaperBlockTemplate pbt)
        {
            Paper = p;
            this.pbt = pbt;
        }
        public BlockScore BlockScore { get; set; }
        public Paper Paper { get; set; }
        public Point Location { get; set; } //临时的 

        public Rectangle ImageRectangle { get{return pbt.Imgrectangle;} }  //没有矫正操作
        public Rectangle ImagePageRectangle { get{return pbt.ImgPageRect;} }
    }
    public class PaperBlocks
    {
        public PaperBlocks(PaperBlockTemplate pbt)
        {
            this._pbt = pbt;
            _paperblocks = new List<PaperBlock>();
            _nonescoreblocks = new List<PaperBlock>();

        }
        public void Clear()
        {
            _paperblocks.Clear();
            _nonescoreblocks.Clear();
        }
        public void AddBlocks(List<Paper> _papers)
        {
            Clear();
            foreach (Paper p in _papers)
            {
                PaperBlock pb = p.PaperBlock(_pbt);
                _paperblocks.Add(pb);
            }
            _nonescoreblocks.AddRange(_paperblocks);
        }
        public PaperBlock PaperBlock(Paper p)
        {
            return p.PaperBlock(_pbt);
        }
        public  bool HasPapers()
        {
            _nonescoreblocks.RemoveAll(r => r.BlockScore.HasScore());     
            return _nonescoreblocks.Count> 0;
        }
        public List<PaperBlock> GetPaperBlocks(Size pagesize)
        {
            int cnt = _pbt.ComputeMaxCount(pagesize); //getmaxcount  每个页面放置的数量
            int Hcnt = _pbt.ComputeMaxHeightCount(pagesize);
            return GetPaperBlocks(cnt,Hcnt);
        }
        private List<PaperBlock> GetPaperBlocks(int cnt,int Hcnt )
        {      
            return _nonescoreblocks.Take(cnt).ToList();
        }
        public override string ToString()
        {
            return _pbt.ToString();
        }
        public int IntScore { get { return _pbt.IntScore; } }


        private List<PaperBlock> _paperblocks;
        private PaperBlockTemplate _pbt;
        private List<PaperBlock> _nonescoreblocks;

    }
    public class BlockScore
    {
        private PaperBlock _pb;
        private float _score;
        public BlockScore(PaperBlock pb)
        {
            this._pb = pb;
            pb.BlockScore = this;
            this._score = -1;
        }
        public void SetScore(float score)
        {
            _score = score;
        }
        public bool HasScore() { return _score >= 0; }

        internal int GetScore()
        {
            return (int)(_score + 0.1);
        }
    }
    public class PaperScore
    {        
        public PaperScore(Paper p, List<PaperBlocks> paperblocks)
        {
            _paper = p;
            _blockscores = new List<BlockScore>();
            foreach (PaperBlocks pbs in paperblocks)
            {
                PaperBlock pb = pbs.PaperBlock(p); 
                BlockScore bs = new BlockScore(pb);
                _blockscores.Add(bs);
            }            
        }
        private Paper _paper;
        private List<BlockScore> _blockscores;
    }
    public class PaperScores
    {
        public PaperScores()
        {
            _paperscores = new List<PaperScore>();
        }
        public  void Init(List<Paper> papers, List<PaperBlocks> _paperblocks)
        {
            Clear();
            foreach (Paper p in papers)
            {
                PaperScore ps = new PaperScore(p, _paperblocks);
                _paperscores.Add(ps);
            }

        }
        private void Clear()
        {
            _paperscores.Clear();
        }
        private List<PaperScore> _paperscores;

    }
    public enum QuestionType : int { None = 0, SingleChoice = 1, MultChoice = 2, UnChoose = 4 };
    public class Question
    {
        public Question()
        {
            Init();
        }
        public Question(int answer, float score)
        {
            this.Type = QuestionType.SingleChoice;
            this.OptionAnswer = answer;
            this.Score = score;
        }
        public Question(QuestionType type, int answer, float score)
        {
            this.Type = type;
            this.OptionAnswer = answer;
            this.Score = score;
        }
        internal void Init()
        {
            this.OptionAnswer = 0;
            this.Score = 0;
            this.Type = QuestionType.None;
        }
        internal QuestionType Type { get; set; }
        public static QuestionType TryParse(string type)
        {
            if (type == "SingleChoice")
            {
                return QuestionType.SingleChoice;
            }
            else if (type == "MultChoice")
            {
                return QuestionType.MultChoice;
            }
            else if (type == "UnChoose")
            {
                return QuestionType.UnChoose;
            }
            return QuestionType.None;
        }
        public int OptionAnswer { get; set; }
        public float Score { get; set; }
    }
    public class Answer
    {
        private List<Question> questions;
        public Answer()
        {
            questions = new List<Question>();
        }
        internal void Reset()
        {
            questions.Clear();
        }
        internal void Clear()
        {
            foreach (Question q in questions)
            {
                q.Init();
            }
        }
        public int Count
        {
            get { return questions.Count; }
            set
            {
                questions.Clear();
                for (int i = 0; i < value; i++)
                {
                    questions.Add(new Question(-1, 0));
                }
            }
        }
        internal void SetAnswer(int i, int answer)
        {
            if (GetType(i) == QuestionType.SingleChoice)
                questions[i].OptionAnswer = answer;
        }
        internal void SetType(int i, QuestionType value)
        {
            questions[i].Type = value;
        }
        internal void SetMaxScore(int i, float score)
        {
            questions[i].Score = score;
        }
        internal int GetOptionAnswer(int i)
        {
            return questions[i].OptionAnswer;
        }
        internal float GetMaxScore(int i)
        {
            return questions[i].Score;
        }
        internal float GetUnchooseScore(int i)
        {
            foreach (Question q in questions)
            {
                if (q.Type == QuestionType.SingleChoice)
                    i++;
            }
            return questions[i].Score;
        }
        internal QuestionType GetType(int i)
        {
            return questions[i].Type;
        }
       
        public XmlDocument SaveToXmlDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement(NodeName);
            xmlDoc.AppendChild(root);
            XmlNode uclist = xmlDoc.CreateElement("QUESTION");
            root.AppendChild(uclist);

            int i = 0;
            foreach (Question uc in questions)
            {
                XmlElement xe = xmlDoc.CreateElement("QA");
                xe.SetAttribute("ID", i.ToString());
                xe.SetAttribute("TYPE", uc.Type.ToString());
                xe.SetAttribute("ANSWER", uc.OptionAnswer.ToString());
                xe.SetAttribute("SCORE", uc.Score.ToString());
                uclist.AppendChild(xe);
                i++;
            }
            return xmlDoc;
        }
        public XmlDocument SaveToSupperXmlDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement supperroot = xmlDoc.CreateElement("SUPPERROOT");
            xmlDoc.AppendChild(supperroot);
            String name = NodeName.Substring(1, NodeName.Count() - 1);
            XmlElement root = xmlDoc.CreateElement(name);
            supperroot.AppendChild(root);
            XmlNode uclist = xmlDoc.CreateElement("QUESTION");
            root.AppendChild(uclist);

            int i = 0;
            foreach (Question uc in questions)
            {
                XmlElement xe = xmlDoc.CreateElement("QA");
                xe.SetAttribute("ID", i.ToString());
                xe.SetAttribute("TYPE", uc.Type.ToString());
                xe.SetAttribute("ANSWER", uc.OptionAnswer.ToString());
                xe.SetAttribute("SCORE", uc.Score.ToString());
                uclist.AppendChild(xe);
                i++;
            }
            return xmlDoc;
        }
        internal bool LoadXml(XmlNode xmlNode)
        {
            Reset();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.AppendChild(xmlDoc.ImportNode(xmlNode, true));
                XmlNodeList uclist = xmlDoc.SelectNodes(NodeName + "/QUESTION/*");
                int i = 0;
                foreach (XmlNode node in uclist)
                {
                    int ID = 0;
                    int ANSWER = 0;
                    int SCORE = 0;
                    String type = node.Attributes["TYPE"].InnerText;
                    int.TryParse(node.Attributes["ID"].InnerText, out ID);
                    int.TryParse(node.Attributes["ANSWER"].InnerText, out ANSWER);
                    int.TryParse(node.Attributes["SCORE"].InnerText, out SCORE);
                    QuestionType TYPE = Question.TryParse(type);
                    questions.Add(new Question(TYPE, ANSWER, SCORE));
                    i++;
                }
            }
            catch
            {
                Reset();
                return false;
            }
            return true;
        }       
        internal bool CheckFit(Answer an)
        {
            if (questions.Count != an.questions.Count)
                return false;
            for (int i = 0; i < questions.Count; i++)
            {
                if (questions[i].GetType() != an.questions[i].GetType())
                    return false;
            }
            return true;
        }
        internal void Update(Answer an)
        {
            questions.Clear();
            questions.AddRange(an.questions);
        }
        internal bool CheckAnswer()
        {
            bool ret = true;
            if (questions.Count == 0)
                ret = false;
            foreach (Question q in questions)
            {
                if (q.Type == QuestionType.SingleChoice)
                {
                    if (q.OptionAnswer < 0) return false;
                    if (q.Score <= 0) return false;
                }
                else if (q.Type == QuestionType.UnChoose)
                {
                    if (q.Score <= 0) return false;
                }
            }

            return ret;
        }
        public String NodeName { get { return "/ANSWER"; } }

        internal List<float> ComputeScores(List<int> optionanswers)
        {
            List<float> ret = new List<float>();
            for (int i = 0; i < optionanswers.Count; i++)
            {
                if (optionanswers[i] == questions[i].OptionAnswer)
                {
                    ret.Add(questions[i].Score);
                }
                else
                {
                    ret.Add(0);
                }
            }
            return ret;
        }
    }  
}
