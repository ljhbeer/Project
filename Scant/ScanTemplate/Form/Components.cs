using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Newtonsoft.Json;
using Tools;
namespace ARTemplate
{
    public class InputBox
    {
        public InputBox()
        {
        }

        public static bool Input(string keyname)
        {
            FormInput f = new FormInput(keyname);
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (keyname == "考试名称" || keyname == "校对")
                    strValue = f.StrValue;
                else if (keyname == "选择题" || keyname == "非选择题")
                    IntValue = f.IntValue;
                else if (keyname == "自定义")
                {
                    IntValue = f.IntValue;
                    strValue = f.StrValue;
                }
                if (f.StrValue == "" || f.IntValue < 0)
                    return false;
                return true;
            }
            f.Dispose();
            f = null;
            return false;
        }
        public static bool Input(string keyname, ComboBox.ObjectCollection objectCollection)
        {
            FormInputComboBox f = new FormInputComboBox(keyname, objectCollection);
            if (f.ShowDialog() == DialogResult.OK)
            {
                strValue = f.StrValue;
            }
            return true;
        }

        public static bool Input(string keyname, List<object> objectCollection)
        {
            FormInputComboBox f = new FormInputComboBox(keyname, objectCollection);
            if (f.ShowDialog() == DialogResult.OK)
            {
                strValue = f.StrValue;
                try
                {
                    IntValue = Convert.ToInt32(strValue);
                }catch{;}
            }
            return true;
        }
        public static string strValue;
        public static int IntValue;
        public static float FloatValue;


    }
    [JsonObject(MemberSerialization.OptIn)]
    public class Area
    {
        public Area()
        {
            EditMode = false;
            ShowTitle = false;
        }
        public Rectangle ImgArea { get { return Rect; } }
        public bool IntersectsWith(Rectangle rect)
        {
            return Rect.IntersectsWith(rect);
        }

        public virtual Rectangle[] ImgSubArea() { return null; }
        public virtual bool HasImgSubArea() { return false; }
        public virtual bool HasSubAreas() { return false; }
        public virtual List<Area> SubAreas { get { return null; } }
        public virtual bool NeedFill() { return false; }
        public virtual Brush FillPen() { return Brushes.Black; }
        public virtual string Title
        {
            get { return ToString(); }
        }
        public virtual void SetName(string name)
        {
            ;
        }
        public virtual float GetTotalScore()
        {
            return 0;
        }
        public virtual String GetScoreInfomation()
        {
            return  GetTotalScore()  + "分";
        }
        public override string ToString()
        {
            return TypeName;
        }
        [JsonProperty]
        public Rectangle Rect;
        [JsonIgnore]
        public string TypeName { get; set; }
        [JsonIgnore]
        public Boolean EditMode { get; set; }
        [JsonIgnore]
        public Boolean ShowTitle { get; set; }
        
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class ListArea:Area
    {
        public ListArea()
        {
        }
        [JsonIgnore]
        public virtual Size ItemSize { get { return  new Size(); } }
        [JsonProperty]
        public List<List<Point>> list;
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class FeaturePoint : Area
    {
        public FeaturePoint(Rectangle r, Point midpoint) // 0,左上  1，右上  2左下 3又下
        {
            TypeName = "特征点";
            this.Rect = r;
            if (r.X < midpoint.X)
            {
                if (r.Y < midpoint.Y)
                    Direction = 0;
                else
                    Direction = 1;
            }
            else
            {
                if (r.Y < midpoint.Y)
                    Direction = 2;
                else
                    Direction = 3;
            }
        }
        public override bool HasImgSubArea() { return true; }
        public override Rectangle[] ImgSubArea()
        {
            return new Rectangle[] { Rect, BigImgSelection() };
        }
        private Rectangle BigImgSelection()
        {
            return new Rectangle(Rect.X - Rect.Width,
                 Rect.Y - Rect.Height,
                 Rect.Width * 3, Rect.Height * 3);
        }
        [JsonProperty]
        public int Direction { get; set; }
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class KaoHaoChoiceArea : ListArea
    {
        public KaoHaoChoiceArea()
        {
            list = new List<List<Point>>();
            ShowTitle = true;
        }
        public KaoHaoChoiceArea(Rectangle m_Imgselection, string name, string type)
        {
            this.TypeName = "考号";
            this.Rect = m_Imgselection;
            this._name = name;
            this.Type = type; // 条形码  ，  填涂横向， 填涂纵向
        }
        public KaoHaoChoiceArea(Rectangle m_Imgselection, string name, string type, List<List<Point>> list, Size size)
        {
            this.TypeName = "考号";
            this.Rect = m_Imgselection;
            this._name = name;
            this.Type = type;
            this.list = list;
            this.Size = size;
        }
        public override bool HasImgSubArea()
        {
            if (Type == "条形码" || Type  == "无")
                return false;
            return true;
        }
        public override Rectangle[] ImgSubArea()
        {
            if ("1023456789".Contains(Type))                                    //(Type == "填涂横向" || Type == "填涂纵向")
            {
                int count = 0;
                foreach (List<Point> l in list)
                    count += l.Count;
                if (count == 0) return null;

                Rectangle[] rv = new Rectangle[count];
                int i = 0;
                foreach (List<Point> l in list)
                {
                    foreach (Point p in l)
                    {
                        rv[i] = new Rectangle(p, Size);
                        //rv[i].Offset(Rect.Location);
                        i++;
                    }
                }
                return rv;
            }
            return null;
        }
        public override string Title
        {
            get
            {
                return _name;
            }
        }
        public override Size ItemSize
        {
            get
            {
                return Size;
            }
        }
        public override string ToString()
        {
            return _name;
        }
        public override void SetName(string name)
        {
            _name = name;
        }
        [JsonProperty]
        public string _name { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        // "填涂横向" || Type == "填涂纵向"
        //[JsonProperty]
        //public List<List<Point>> list;
        [JsonProperty]
        public Size Size;
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class SingleChoiceArea : ListArea
    {
        public SingleChoiceArea()
        {
            list = new List<List<Point>>();
            Listanswerscore = new List<OptionAnswerScore>();
        }
        public void InitAnswerScore()
        {
            if ( Listanswerscore.Count == 0 && list.Count > 0)
                InitListAnswerScore();
        }
        public SingleChoiceArea(Rectangle rect, string name)
        {
            this.TypeName = "选择题";
            this.Rect = rect;
            this._name = name;
            ShowTitle = true;
            //
            list = new List<List<Point>>();
            Listanswerscore = new List<OptionAnswerScore>();
        }
        public SingleChoiceArea(Rectangle rect, string name, List<List<Point>> list, Size size)
        {
            this.TypeName = "选择题";
            this.Rect = rect;
            this._name = name;
            this.list = list;
            this.Size = size;
            list = new List<List<Point>>(); 
            InitListAnswerScore();
        }

        private void InitListAnswerScore()
        {
            Listanswerscore = new List<OptionAnswerScore>();
            int index = 0;
            int count = 0;
            foreach (List<Point> l in list)
                count += l.Count;
            if (count == 0) return;

            int pos = 0;
            foreach (List<Point> l in list)
            {
                Listanswerscore.Add(
                new OptionAnswerScore(this, index, index)
                );
                pos++;
                index++;
            }
        }
        public override bool HasImgSubArea() { return true; }
        public override Rectangle[] ImgSubArea()
        {
            int count = 0;
            foreach (List<Point> l in list)
                count += l.Count;
            if (count == 0) return null;
            Rectangle[] rv = new Rectangle[count];
            int i = 0;
            foreach (List<Point> l in list)
            {
                foreach (Point p in l)
                {
                    rv[i] = new Rectangle(p, Size);
                    i++;
                }
            }
            return rv;
        }
        public override string ToString()
        {
            return _name;
        }
        public override string Title
        {
            get
            {
                return _name;
            }
        }
        public override Size ItemSize
        {
            get
            {
                return Size;
            }
        }
        public int Count
        {
            get
            {
                return list.Count;
            }
        }
        public override void SetName(string name)
        {
            _name = name;
        }
        public string Name { get { return _name; } }
        public override float GetTotalScore()
        {
            if (Listanswerscore == null && list.Count > 0)
                InitListAnswerScore();               
            if(Listanswerscore == null || Listanswerscore.Count == 0)
                return 0;
            return Listanswerscore.Sum(r => r.Score);
        }
        //[JsonProperty]
        //public List<List<Point>> list;
        [JsonIgnore]
        public List<OptionAnswerScore> Listanswerscore;
        [JsonProperty]
        public Size Size;
        [JsonProperty]
        private string _name;

    }
    [JsonObject(MemberSerialization.OptIn)]
    public class OptionAnswerScore   
    {
        public OptionAnswerScore()
        {
        }
        public OptionAnswerScore(SingleChoiceArea U, int index, int pos)
        {
            this.U = U;
            this.ID = index + 1;
            this.Index = index;
            this.Score = 1;
            this._Rect = U.ImgArea;
            this.Size = U.Size;
            this.List = new List<Point>();
            List = U.list[pos];
        }
        public void InitDeserialize()
        {

        }
        public override string ToString()
        {
            return Name();
        }

        [JsonProperty]
        public float Score { get; set; }
        [JsonProperty]
        public int Index { get; set; }
        [JsonProperty]
        public string Answer { get; set; }
        [JsonProperty]
        public float HalfScore { get; set; }
        [JsonProperty]
        public string Type { get; set; }

        public int ID { get; set; }
        public List<Point> List { get; set; }
        public Size Size { get; set; }
        public string Name() { return "x" + ID; }
        public string OutName { get { return "选择题" + ID; } }
        private Rectangle _Rect;

        [JsonIgnore]
        public Rectangle Rect { get { return _Rect; } }
        [JsonIgnore]
        public int Height { get { return Rect.Height; } }
        [JsonIgnore]
        public int Width { get { return Rect.Width; } }
        [JsonIgnore]
        private SingleChoiceArea U;
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class UnChoose : Area
    {
        public UnChoose(float score, string name, Rectangle imgrect)
        {
            this.TypeName = "非选择题";
            this.score = score;
            this._name = name;
            this.Rect = imgrect;
            ShowTitle = true;
        }
        public int Scores { get { return (int)score; } }
        public override string Title
        {
            get
            {
                return _name;
            }
        }
        public override String ToString()
        {
            return _name;
        }
        public string Name { get { return _name; } }       
        public override void SetName(string name)
        {
            _name = name;
        }
        public override float GetTotalScore()
        {
            return score;
        }
        [JsonProperty]
        private float score;
        [JsonProperty]
        private string _name;
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class NameArea : Area //校对
    {
        public NameArea(Rectangle rect, string name)
        {
            this.TypeName = "校对";
            this.Rect = rect;
            this._name = name;
        }
        public string Name { get { return _name; } }
        public override String ToString()
        {
            return _name;
        }
        public override string Title
        {
            get
            {
                return _name;
            }
        }
        public override void SetName(string name)
        {
            _name = name;
        }
        [JsonProperty]
        private string _name;
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class TempArea : Area
    {
        public TempArea()
        {
            _name = "";
            _P = Brushes.Black;
            ShowTitle = true;
        }
        public TempArea(Rectangle rect, string name)
        {
            this.Rect = rect;
            this._name = name;
            if (_name.Contains("黑"))
            {
                this.TypeName = "选区变黑";
                _P = Brushes.Black;
            }
            else
            {
                this.TypeName = "选区变白";
                _P = Brushes.White;
            }
        }
        public void SetBrush(Brush brush)
        {
            _P = brush;
        }
        public override bool NeedFill() { return true; }
        public override Brush FillPen() { return _P; }
        public override string Title
        {
            get
            {
                return _name;
            }
        }
        public override string ToString()
        {
            return _name;
        }
        public override void SetName(string name)
        {
            _name = name;
        }
        [JsonProperty]
        private string _name;
        private Brush _P;
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class TzArea : Area
    {
        public TzArea(Rectangle rect, string name)
        {
            this.TypeName = "题组";
            this.Rect = rect;
            this._name = name;
            ShowTitle = true;
            _subareas = new List<Area>();
        }
        public override string Title
        {
            get
            {
                return _name;
            }
        }
        public override String ToString()
        {
            return _name;
        }
        public override void SetName(string name)
        {
            _name = name;
        }
        public void AddSubArea(Area I)
        {
            _subareas.Add(I);
        }
        public override bool HasSubAreas()
        {
            return true;
        }
        public override float GetTotalScore()
        {
            if(_subareas.Count==0)
            return base.GetTotalScore();
            return _subareas.Sum(r => r.GetTotalScore());
        }
        [JsonIgnore]
        public override List<Area> SubAreas
        {
            get
            {
                return _subareas;
            }
        }
        [JsonProperty]
        private string _name;
        [JsonProperty]
        private List<Area> _subareas;       
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class CustomArea : ListArea
    {
        public CustomArea()
        {
            list = new List<List<Point>>();
        }
        public CustomArea(Rectangle m_Imgselection, string name, string type, List<List<Point>> list, Size size)
        {
            this.TypeName = "自定义";
            this.Rect = m_Imgselection;
            this.Name = name;
            this.Type = type;
            this.list = list;
            this.Size = size;
        }
        public override bool HasImgSubArea()
        {
            return true;
        }
        public override Rectangle[] ImgSubArea()
        {
            if ("1023456789".Contains(Type))                                    //(Type == "填涂横向" || Type == "填涂纵向")
            {
                int count = 0;
                foreach (List<Point> l in list)
                    count += l.Count;
                if (count == 0) return null;

                Rectangle[] rv = new Rectangle[count];
                int i = 0;
                foreach (List<Point> l in list)
                {
                    foreach (Point p in l)
                    {
                        rv[i] = new Rectangle(p, Size);
                        //rv[i].Offset(Rect.Location);
                        i++;
                    }
                }
                return rv;
            }
            return null;
        }
        public override string Title
        {
            get
            {
                return Name;
            }
        }
        public override Size ItemSize
        {
            get
            {
                return Size;
            }
        }
        public override String ToString()
        {
            return Name;
        }

        public override void SetName(string name)
        {
            Name = name;
        }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        // "填涂横向" || Type == "填涂纵向"
        //[JsonProperty]
        //public List<List<Point>> list;
        [JsonProperty]
        public Size Size;
    }

    public class Areas
    {
        public string TypeName { get; set; }
        public List<Area> baselist { get; set; }
        public bool HasItems()
        {
            if (baselist == null || baselist.Count == 0)
                return false;
            return true;
        }
        public virtual string GetScoreInfomation()
        {
            return "";
        }
    }
    public class FeaturePoints : Areas
    {
        public FeaturePoints()
        {
            _list = null;
        }
        public FeaturePoints(List<Area> lista)
        {
            _list = null;
            baselist = lista;
        }
        private List<FeaturePoint> _list;
        public List<FeaturePoint> list
        {
            get
            {
                if (_list == null && baselist != null)
                {
                    _list = new List<FeaturePoint>();
                    foreach (Area A in baselist)
                        _list.Add((FeaturePoint)A);
                }
                return _list;
            }
        }
        public static List<Area> GetFeaturesfromrects(List<Rectangle> list)
        {
            List<Area> lst = new List<Area>();
            Point mid = Tools.DetectImageTools.GetMidPoint(list);
            for (int i = 0; i < list.Count; i++)
            {
                lst.Add(new FeaturePoint(list[i], mid));
            }
            return lst;
        }
    }
    public class KaoHaoChoiceAreas : Areas
    {
        public KaoHaoChoiceAreas()
        {
            _list = null;
        }
        public KaoHaoChoiceAreas(List<Area> lista)
        {
            _list = null;
            baselist = lista;
        }
        private List<KaoHaoChoiceArea> _list;
        public List<KaoHaoChoiceArea> list
        {
            get
            {
                if (_list == null && baselist != null)
                {
                    _list = new List<KaoHaoChoiceArea>();
                    foreach (Area A in baselist)
                        _list.Add((KaoHaoChoiceArea)A);
                }
                return _list;
            }
        }
    }
    public class SingleChoiceAreas : Areas
    {
        public SingleChoiceAreas()
        {
            _list = null;
            _singlerectlist = null;
        }
        public SingleChoiceAreas(List<Area> lista)
        {
            _list = null;
            baselist = lista;
        }
        private List<SingleChoiceArea> _list;
        public List<SingleChoiceArea> list
        {
            get
            {
                if (_list == null && baselist != null)
                {
                    _list = new List<SingleChoiceArea>();
                    foreach (Area A in baselist)
                        _list.Add((SingleChoiceArea)A);
                }
                return _list;
            }
        }

        public int Count
        {
            get
            {
                if (list == null || list.Count == 0)
                    return 0;
                return _list.Sum(r => r.Count);
            }
        }

        public Rectangle SingleRectangle(int i)
        {
            if (_singlerectlist == null)
            {
                List<Rectangle> l = new List<Rectangle>();
                foreach (SingleChoiceArea sc in _list)
                {
                    int subcnt = 0;
                    foreach (List<Point> lp in sc.list)
                    {
                        Rectangle r = sc.ImgArea;
                        r.Height /= sc.Count;
                        r.Y += subcnt * r.Height;
                        subcnt++;
                        l.Add(r);
                    }
                }
                if (l.Count > 0)
                    _singlerectlist = l;
            }
            if (_singlerectlist != null)
            {
                if (i >= 0 && i < _singlerectlist.Count)
                    return _singlerectlist[i];
            }
            return new Rectangle();
        }
        private List<Rectangle> _singlerectlist;

        public double TotalScore()
        {
            //return  _list.Sum(  r => r.SubAreas.Sum( rr => rr.Score );
            return Count;
        }
        public override string GetScoreInfomation()
        {
            if (list == null)
                return base.GetScoreInfomation();
            float totalscore = _list.Sum(r => r.GetTotalScore());
            return "小题总分：" + totalscore;
        }
        public string AnswerScoreInfomation()
        {
            return "共"+Count+"小题";


        }
    }
    public class UnChooseAreas : Areas
    {
        public UnChooseAreas()
        {
            _list = null;
        }
        public UnChooseAreas(List<Area> lista)
        {
            _list = null;
            baselist = lista;
        }
        private List<UnChoose> _list;
        public List<UnChoose> list
        {
            get
            {
                if (_list == null && baselist != null)
                {
                    _list = new List<UnChoose>();
                    foreach (Area A in baselist)
                        _list.Add((UnChoose)A);
                }
                return _list;
            }
        }
        public override string GetScoreInfomation()
        {
            if(list==null)
            return base.GetScoreInfomation();
            float totalscore = _list.Sum(r => r.GetTotalScore());
            return "小题总分：" + totalscore;
        }
        public string Count { get; set; }
    }
    public class NameAreas : Areas
    {
        public NameAreas()
        {
            _list = null;
        }
        public NameAreas(List<Area> lista)
        {
            _list = null;
            baselist = lista;
        }
        private List<NameArea> _list;
        public List<NameArea> list
        {
            get
            {
                if (_list == null && baselist != null)
                {
                    _list = new List<NameArea>();
                    foreach (Area A in baselist)
                        _list.Add((NameArea)A);
                }
                return _list;
            }
        }
    }
    public class TempAreas : Areas
    {
        public TempAreas()
        {
            _list = null;
        }
        public TempAreas(List<Area> lista)
        {
            _list = null;
            baselist = lista;
        }
        private List<TempArea> _list;
        public List<TempArea> list
        {
            get
            {
                if (_list == null && baselist != null)
                {
                    _list = new List<TempArea>();
                    foreach (Area A in baselist)
                        _list.Add((TempArea)A);
                }
                return _list;
            }
        }
    }
    public class TzAreas : Areas
    {
        public TzAreas()
        {
            _list = null;
        }
        public TzAreas(List<Area> lista)
        {
            _list = null;
            baselist = lista;
        }
        private List<TzArea> _list;
        public List<TzArea> list
        {
            get
            {
                if (_list == null && baselist != null)
                {
                    _list = new List<TzArea>();
                    foreach (Area A in baselist)
                        _list.Add((TzArea)A);
                }
                return _list;
            }
        }

        public  double  TotalScore()
        {
            if(list!=null)
            return list.Sum(r => r.SubAreas.Sum(rr => ((UnChoose)rr).Scores));
            return 0;
        }

        internal string ScoreInfomation()
        {
            if(list!=null)
            return string.Join("\r\n",
                list.Select(r =>
                {
                    string str  = "";
                   str =  r.Title + "\t"+ r.SubAreas.Sum(rr => ((UnChoose)rr).Scores)+"分\t"+ 
                       string.Join(" ",r.SubAreas.Select( rr => ((UnChoose)rr).Scores + "分"));
                    return str;
                }).ToList()
                );
            return "";
        }
    }
    public class CustomAreas : Areas
    {
        public CustomAreas()
        {
            _list = null;
        }
        public CustomAreas(List<Area> lista)
        {
            _list = null;
            baselist = lista;
        }
        private List<CustomArea> _list;
        public List<CustomArea> list
        {
            get
            {
                if (_list == null && baselist != null)
                {
                    _list = new List<CustomArea>();
                    foreach (Area A in baselist)
                        _list.Add((CustomArea)A);
                }
                return _list;
            }
        }
    }
    public class ManageAreas
    {
        public ManageAreas()
        {
            FeaturePoints = new FeaturePoints();
            KaohaoChoiceAreas = new KaoHaoChoiceAreas();
            SinglechoiceAreas = new SingleChoiceAreas();
            Unchooseareas = new UnChooseAreas();
            Nameareas = new NameAreas();
            BlackTempareas = new TempAreas();
            WhiteTempareas = new TempAreas();
            Tzareas = new TzAreas();
            Customareas = new CustomAreas();
        }
        public FeaturePoints FeaturePoints { get; set; }
        public KaoHaoChoiceAreas KaohaoChoiceAreas { get; set; }
        public SingleChoiceAreas SinglechoiceAreas { get; set; }
        public UnChooseAreas Unchooseareas { get; set; }
        public NameAreas Nameareas { get; set; }
        public TempAreas BlackTempareas { get; set; }
        public TempAreas WhiteTempareas { get; set; }
        public TzAreas Tzareas { get; set; }
        public CustomAreas Customareas { get; set; }
    }

    public class ZoomBox
    {
        public ZoomBox()
        {
            Reset();
        }
        public void Reset()
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

        private Rectangle GetPictureBoxZoomSize(PictureBox p_PictureBox)
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

        public double ImageWith(PictureBox p_PictureBox)
        {
            return GetPictureBoxZoomSize(p_PictureBox).Width * 1.0;
        }
    }
}