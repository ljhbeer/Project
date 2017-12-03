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
    public class KaoHaoChoiceArea : ISelectionInterface
    {
        public KaoHaoChoiceArea(Rectangle m_Imgselection, string name, List<List<Point>> list, Size size)
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

        public string Name { get { return name; } }
    }
    public class SingleChoiceArea : ISelectionInterface
    {
        //public SingleChoiceArea(Rectangle imgrect, string name)
        //{
        //    this.imgselection = imgrect;
        //    this.name = name;
        //}
        //public SingleChoiceArea(SingleChoice[] sc)
        //{
        //    this.scv = sc;
        //}
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

        public string Name { get { return name; } }
    }
    public class UnChoose : ISelectionInterface
    {
        public UnChoose(float score, string name, Rectangle imgrect)
        {
            this.score = score;
            this.name = name;
            this.imgselection = imgrect;
        }
        public bool IntersectsWith(Rectangle rect)
        {
            return imgselection.IntersectsWith(rect);
        }
        public override String ToString()
        {
            return name;
        }
        public int Scores { get { return (int)score; } }
        public Rectangle ImgSelection() 
        {
            return imgselection;
        }
        public bool HasSubSelection() { return false; }
        public Rectangle[] ImgSubSelection() { return null; }
        internal string ToXmlString()
        {
            String str = "";
            //foreach (Point p in imgselection)
            {
                str += "<RECTANGLE>" + imgselection.X + "," + imgselection.Y + "," + imgselection.Width + "," + imgselection.Height + "</RECTANGLE>"
                    + "<NAME>"+name+"</NAME>" + "<SCORE>"+score+"</SCORE>";
            }
            return str;
        }
        private float score;
        private string name;
        private Rectangle imgselection;

        public string Name { get { return name; } }
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
    }
    public class Paper
    {
        public Paper(List<TriAngleFeature> list)
        {
            if (list.Count != 3)
                return;
            cp = list[0].CornerPoint();
            rp = list[1].CornerPoint();
            bp = list[2].CornerPoint();
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


}
