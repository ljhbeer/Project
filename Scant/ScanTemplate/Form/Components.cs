using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace ARTemplate
{
    public class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 354);
            this.Name = "InputBox";
            this.Text = "InputBox";
            buttonCancel = new Button();
            buttonOK = new Button();
            buttonOK.Text = "确定(&O)";
            buttonCancel.Text = "取消(&O)";
            this.Controls.Add(buttonOK);
            this.Controls.Add(buttonCancel);
            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
            buttonOK.Click += new EventHandler(buttonOK_Click);
            buttonCancel.Click += new EventHandler(buttonCancel_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public static bool Input(string Title, string textTitle, ref string textValue, string numTitle, ref float numValue)
        {
            List<string> subTitle = new List<string>();
            List<string> subValue = new List<string>();
            subTitle.Add(textTitle);
            subTitle.Add(numTitle);
            subValue.AddRange(new string[] { textValue, "" });
            if (Input(Title, ref subTitle, ref subValue, 2))
            {
                textValue = subValue[0];
                numValue = float.Parse(subValue[1]);
                return true;
            }
            return false;
        }
        public static bool Input(string Title, ref List<string> subTitle, ref List<string> subValue, int Numflag)
        {
            InputBox inputBox = new InputBox();
            inputBox.Text = Title;
            inputBox.numflag = Numflag;
            inputBox.Init(subTitle, subValue);
            DialogResult result = inputBox.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (inputBox.GetValue(subValue))
                    return true;
            }
            return false;
        }

        private bool GetValue(List<string> subValue)
        {
            int flag = 1;
            float value;
            for (int i = 0; i < label.Count(); i++)
            {
                subValue[i] = textBox[i].Text;
                if (subValue[i] == "")
                    return false;
                if ((numflag & flag) != 0)
                {
                    if (!float.TryParse(subValue[i], out value))
                        return false;
                }
                flag *= 2;
            }
            return true;
        }

        private void Init(List<string> subTitle, List<string> subValue)
        {
            if (subValue.Count != subValue.Count || subValue.Count == 0)
                throw new SyntaxErrorException();
            this.SuspendLayout();
            label = new Label[subValue.Count];
            textBox = new TextBox[subValue.Count];
            int flag = 1;
            for (int i = 0; i < subValue.Count; i++)
            {
                label[i] = new Label();
                textBox[i] = new TextBox();
                if (subTitle[i].Length > 8)
                    subTitle[i] = subTitle[i].Substring(subTitle[i].Length - 8);
                label[i].Text = subTitle[i];
                label[i].Location = new Point(60, 33 + i * 30);
                textBox[i].Location = new Point(160, 33 + i * 30);
                textBox[i].Size = new Size(200, 21);

                if ((numflag & flag) != 0)
                    textBox[i].KeyPress += textboxValue_KeyPress;
                flag *= 2;

            }
            buttonOK.Location = new Point(100, 33 + 30 * subValue.Count);
            buttonCancel.Location = new Point(220, 33 + 30 * subValue.Count);
            this.Controls.AddRange(label);
            this.Controls.AddRange(textBox);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        void textboxValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < (char)Keys.D0 || e.KeyChar > (char)Keys.D9) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' || e.KeyChar == '-')
            {
                e.Handled = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label[] label;
        private System.Windows.Forms.TextBox[] textBox;
        private Button buttonOK;
        private Button buttonCancel;
        private int numflag;
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
        public override bool HasSubArea()
        {
            if (Type == "条形码")
                return false;
            return true;
        }
        public override Rectangle[] ImgSubArea() {
            if (Type == "填涂横向" || Type == "填涂纵向")
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
            return null;
            
        }
        public override string ToXmlString() //分Type
        {
            String str =Type.ToXmlString("TYPE") + Rect.ToXmlString() 
                + Name.ToXmlString("NAME"); //+ "<SIZE>" + size.Width + "," + size.Height + "</SIZE>"
            if (Type == "条形码")
                str += "";
            else if (Type == "填涂横向" || Type == "填涂纵向")
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
