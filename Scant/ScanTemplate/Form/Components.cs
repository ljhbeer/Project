using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

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
                if (keyname == "考试名称")
                    strValue = f.StrValue;
                else if (keyname == "选择题" || keyname == "非选择题")
                    IntValue = f.IntValue;

                if (f.StrValue == "" || f.IntValue < 0)
                    return false;
                return true;
            }
            return false;
        }
        public static string strValue;
        public static int IntValue;
        public static float FloatValue;
    }
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
        public Rectangle Rect;
    }
    public class FeaturePoint : Area
    {
        public FeaturePoint(Rectangle r,Point midpoint) // 0,左上  1，右上  2左下 3又下
        {
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
        public int Direction { get; set; }
    }
    public class SingleChoice :  Area
    {
        private string text;
        public SingleChoice(Rectangle rect, string text="")
        {
            this.Rect = rect;
            this.text = text;
        }       
        public override string ToString()
        {
            return text;
        }
        public override string ToXmlString()
        {
            return base.ToXmlString() + text.ToXmlString("TXT");
        }
    }
    public class KaoHaoChoiceArea : Area
    {
        public KaoHaoChoiceArea(Rectangle m_Imgselection, string name, string type)
        {
            this.Rect = m_Imgselection;
            this.Name = name;
            this.Type = type; // 条形码  ，  填涂横向， 填涂纵向
        }
        public KaoHaoChoiceArea(Rectangle m_Imgselection, string name, string type, List<List<Point>> list, Size size)
        {
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
        public string Name { get; set; }
        public string Type { get; set; }
        // "填涂横向" || Type == "填涂纵向"
        public List<List<Point>> list;
        public Size Size;
    }
    public class SingleChoiceArea : Area
    {
        public SingleChoiceArea(Rectangle  rect, string name)
        {
            this.Rect = rect;
            this._name = name;
        }
        public SingleChoiceArea(Rectangle rect, string name, List<List<Point>> list, Size size)
        {
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
        public List<List<Point>> list;
        public Size Size;
        private string _name;
    }
    public class UnChoose : Area
    {
        public UnChoose(float score, string name, Rectangle imgrect)
        {
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
        private float score;
        private string _name;
    }
    public class NameArea : Area
    {
        public NameArea(Rectangle rect)
        {
            this.Rect = rect;
        }
    }
    public class TempArea : Area
    {
        public TempArea(Rectangle rect, string name)
        {
            this.Rect = rect;
            this._Name = name;
            if(_Name.Contains("黑"))
                _P = Brushes.Black;
            else
                _P = Brushes.White;
        }
        public override  bool NeedFill() { return true; }
        public override  Brush FillPen() { return _P; }
        //public override string ToXmlString()
        //{
        //    return base.ToXmlString() + _Name.ToXmlString("Name");
        //}
        private string _Name;
        private Brush _P;
    }
    public class TzArea : Area
    {
        public TzArea(Rectangle rect, string name)
        {
            this.Rect = rect;
            this._name = name;
        }
        public void SetName(string name)
        {
            if(name!="")
            _name = name;
        }
        private string _name;
        public override string ToXmlString()
        {
            return Rect.ToXmlString() + _name.ToXmlString("NAME") ;
        }
        public override String ToString()
        {
            return _name;
        }
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
