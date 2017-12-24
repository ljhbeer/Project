using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Newtonsoft.Json;

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
        public static string strValue;
        public static int IntValue;
        public static float FloatValue;

    }
     [JsonObject(MemberSerialization.OptIn)]
    public class Area
    {
        public Rectangle ImgArea { get { return Rect; } }
        public bool IntersectsWith(Rectangle rect)
        {
            return Rect.IntersectsWith(rect);
        }

        public virtual Rectangle[] ImgSubArea() {  return null;   }
        public virtual bool HasSubArea() {  return false;  }
        public virtual bool NeedFill() { return false; }
        public virtual Brush FillPen() { return Brushes.Black; }
        public virtual string ToXmlString()
        {
            return Rect.ToXmlString();
        }
        public override  string ToString()
        {
            return TypeName;
        }
         [JsonProperty]
        public Rectangle Rect;
         [JsonIgnore]
        public string TypeName { get; set; }
    }
     [JsonObject(MemberSerialization.OptIn)]
    public class FeaturePoint : Area
    {
        public FeaturePoint(Rectangle r,Point midpoint) // 0,左上  1，右上  2左下 3又下
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
        public override bool HasSubArea(){ return true;  }
        public override Rectangle[] ImgSubArea()
        {
                return new Rectangle[] { Rect, BigImgSelection() };
        }
        private Rectangle BigImgSelection()
        {
           return  new Rectangle(Rect.X - Rect.Width,
                Rect.Y - Rect.Height,
                Rect.Width * 3, Rect.Height * 3);
        }
        [JsonProperty]
        public int Direction { get; set; }
    }   
     [JsonObject(MemberSerialization.OptIn)]
    public class KaoHaoChoiceArea : Area
    {
         public KaoHaoChoiceArea()
         {
             list = new List<List<Point>>();
         }
        public KaoHaoChoiceArea(Rectangle m_Imgselection, string name, string type)
        {
            this.TypeName = "考号";
            this.Rect = m_Imgselection;
            this.Name = name;
            this.Type = type; // 条形码  ，  填涂横向， 填涂纵向
        }
        public KaoHaoChoiceArea(Rectangle m_Imgselection, string name, string type, List<List<Point>> list, Size size)
        {
            this.TypeName = "考号";
            this.Rect = m_Imgselection;
            this.Name = name;
            this.Type = type;
            this.list = list;
            this.Size = size;
        }
        public override bool HasSubArea()
        {
            if (Type == "条形码")
                return false;
            return true;
        }
        public override Rectangle[] ImgSubArea() {
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
        public override string ToXmlString() //分Type
        {
            String str =Type.ToXmlString("TYPE") + Rect.ToXmlString() 
                + Name.ToXmlString("NAME"); //+ "<SIZE>" + size.Width + "," + size.Height + "</SIZE>"
            if (Type == "条形码")
                str += "";
            else if("1023456789".Contains(Type)) // (Type == "填涂横向" || Type == "填涂纵向")
            { 
                int i = 0;
                str += Size.ToXmlString();
                foreach (List<Point> lp in list)
                {
                   str += "<SINGLE ID=\"" + i++ + "\">" + string.Join("", lp.Select(r => r.ToXmlString())) + "</SINGLE>";
                }
            }
            return str;
        }
        public override string ToString()
        {
            return Name;
        }
         [JsonProperty]
        public string Name { get; set; }
          [JsonProperty]
        public string Type { get; set; }
        // "填涂横向" || Type == "填涂纵向"
          [JsonProperty]
        public List<List<Point>> list;
          [JsonProperty]
        public Size Size;
    }
     [JsonObject(MemberSerialization.OptIn)]
    public class SingleChoiceArea : Area
    {
         public SingleChoiceArea()
         {
             list = new List<List<Point>>();
         }
        public SingleChoiceArea(Rectangle  rect, string name)
        {
            this.TypeName = "选择题";
            this.Rect = rect;
            this._name = name;
        }
        public SingleChoiceArea(Rectangle rect, string name, List<List<Point>> list, Size size)
        {
            this.TypeName = "选择题";
            this.Rect = rect;
            this._name = name;
            this.list = list;
            this.Size = size;
        }
        public override  bool HasSubArea() { return true; }
        public override  Rectangle[] ImgSubArea() { 
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
        public override string ToXmlString()
        {
            String str = Rect.ToXmlString() + _name.ToXmlString("NAME")+Size.ToXmlString();
            int i = 0;
            foreach (List<Point> lp in list)
            { 
                str +="<SINGLE ID=\"" + i + "\">" + string.Join("", lp.Select(r => r.ToXmlString())) + "</SINGLE>";
                i++;
            }
            return str;
        }
        public int Count 
        {
            get
            {
                return list.Count;
            }
        }
        public string Name { get { return _name; } }
          [JsonProperty]
        public List<List<Point>> list;
          [JsonProperty]
        public Size Size;
          [JsonProperty]
        private string _name;
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
        }
        public int Scores { get { return (int)score; } }       
        public override string ToXmlString()
        {
            return Rect.ToXmlString() + _name.ToXmlString("NAME") + score.ToString().ToXmlString("SCORE");
        }
        public override String ToString()
        {
            return _name;
        }
        public string Name { get { return _name; } }
        public void SetName(string name)
        {
            if (name != "")
                _name = name;
        }
          [JsonProperty]
        private float score;
          [JsonProperty]
        private string _name;
    }
     [JsonObject(MemberSerialization.OptIn)]
    public class NameArea : Area //校对
    {
        public NameArea(Rectangle rect,string name)
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
        public override string ToXmlString()
        {
            return Rect.ToXmlString() + _name.ToXmlString("NAME");
        }
          [JsonProperty]
        private string _name;
    }
     [JsonObject(MemberSerialization.OptIn)]
    public class TempArea : Area
    {
         public TempArea()
         {
             _Name = "";
             _P = Brushes.Black;
         }
        public TempArea(Rectangle rect, string name)
        {
            this.Rect = rect;
            this._Name = name;
            if (_Name.Contains("黑"))
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
        public override  bool NeedFill() { return true; }
        public override  Brush FillPen() { return _P; }
        public override string ToString()
        {
            return _Name;
        }
          [JsonProperty]
        private string _Name;
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
        }
        public void SetName(string name)
        {
            if(name!="")
            _name = name;
        }
        public override string ToXmlString()
        {
            return Rect.ToXmlString() + _name.ToXmlString("NAME") ;
        }
        public override String ToString()
        {
            return _name;
        }
          [JsonProperty]
        private string _name;
    }
     [JsonObject(MemberSerialization.OptIn)]
    public class CustomArea : Area
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
        public override bool HasSubArea()
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
        public override string ToXmlString() //分Type
        {
            String str = Type.ToXmlString("TYPE") + Rect.ToXmlString()
                + Name.ToXmlString("NAME"); //+ "<SIZE>" + size.Width + "," + size.Height + "</SIZE>"
            if ("1023456789".Contains(Type)) // (Type == "填涂横向" || Type == "填涂纵向")
            {
                int i = 0;
                str += Size.ToXmlString();
                foreach (List<Point> lp in list)
                {
                    str += "<SINGLE ID=\"" + i++ + "\">" + string.Join("", lp.Select(r => r.ToXmlString())) + "</SINGLE>";
                }
            }
            return str;
        }
        public override String ToString()
        {
            return Name;
        }
          [JsonProperty]
        public string Name { get; set; }
          [JsonProperty]
        public string Type { get; set; }
        // "填涂横向" || Type == "填涂纵向"
          [JsonProperty]
        public List<List<Point>> list;
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
                if (_list == null  && baselist != null)
                {
                    _list = new List<FeaturePoint>();
                    foreach (Area A in baselist)
                        _list.Add((FeaturePoint)A);
                }
                return _list;
            }
        }

        public static List<Area> GetFeaturesfromTBO(List<Rectangle> listTBO)
        {
            List<Area> list = new List<Area>();
            Point mid = Tools.AutoTBO.GetMidPoint(listTBO);
            for (int i = 0; i < listTBO.Count; i++)
            {
                list.Add( new FeaturePoint( listTBO[i], mid));
            }
            return list;
        }
    }
    public class KaoHaoChoiceAreas :Areas
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
                if(list==null || list.Count==0)
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
                        Rectangle r =sc.ImgArea;
                        r.Height/= sc.Count;
        				r.Y += subcnt* r.Height;
        				subcnt++;
                        l.Add(r);
                    }	
                }
                if (l.Count > 0)
                    _singlerectlist = l;
            }
            if (_singlerectlist != null)
            {
                if (i > 0 && i < _singlerectlist.Count)
                    return _singlerectlist[i];
            }
            return new Rectangle();
        }
        private List<Rectangle> _singlerectlist;
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
    public class Paper
    {
        public Paper(List<FeaturePoint> list)
        {
            if (list.Count != 3)
                return;
            cp = list[0].ImgArea.Location;
            rp = list[1].ImgArea.Location;
            bp = list[2].ImgArea.Location;
        }
        public Paper(List<Point> list)
        {
            cp = list[0];
            rp = list[1];
            bp = list[2];
        }
        public string ToXml()
        {
            return PointToXml(cp) + PointToXml(rp) + PointToXml(bp);
        }
        public  string PointToXml(Point p)
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
        public String NodeName { get { return "/PAPERS"; } }
        Point cp;
        Point rp;
        Point bp;
    }
    
    //Rectangle Tools
    public static class extend
    {
    	public static string ToString(this Rectangle Rect,string split){
    		return Rect.X + split + Rect.Y + split + +Rect.Width + split + Rect.Height;
    	}
        public static string ToXmlString(this Rectangle Rect)
        {
            return "<Rectangle>" + Rect.X + "," + Rect.Y + "," + +Rect.Width + "," + Rect.Height + "</Rectangle>";
        }
        public static string ToXmlString(this String str,String tagname)
        {
            return ("<[tag]>"+str+"</[tag]>").Replace("[tag]",tagname);
        }
        public static string ToXmlString(this Point p)
        {
            return "<POINT>" + p.X + "," + p.Y + "</POINT>";
        }
        public static string ToXmlString(this Size s)
        {
            return "<SIZE>" + s.Width + "," + s.Height + "</SIZE>";
        }

    }
}
