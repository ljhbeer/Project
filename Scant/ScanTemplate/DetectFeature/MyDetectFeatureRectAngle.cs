using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Tools;
using System.Windows.Forms;

namespace ScanTemplate
{
    class MyDetectFeatureRectAngle
    {
        public Rectangle CorrectRect { get; set; }
        public List<Point> ListPoint
        {
            get
            {
                return new List<Point>(){
                    _listFeatureRectangles[0].Location, //T
                    _listFeatureRectangles[1].Location, //B
                    _listFeatureRectangles[2].Location}; //other
            }
        }
        public List<Rectangle> ListFeatureRectangle
        {
            get
            {
                return _listFeatureRectangles;
            }
        }
       
        public MyDetectFeatureRectAngle(System.Drawing.Bitmap bmp)
        {
            this._src = bmp;
            if (_src != null)
            {
                AutoDetectRectAnge adr = new AutoDetectRectAnge();
                adr.InitDetectRectAngel(bmp.Size);
                _listsubjects = adr.GetSubjects();
                Detect3Point();
            }
            //Bitmap newbmp = (Bitmap)_src.Clone(CorrectRect, _src.PixelFormat);
            //newbmp.Save("correct.tif");
        }
        public void Detect3Point()
        {
            _listFeatureRectangles = new List<Rectangle>();
            foreach (subject sub in _listsubjects)
            {
                Rectangle r = DetectFeatureRect(sub);
                if (r.Width == 0)
                    break;
                _listFeatureRectangles.Add(r);
            }
            if (_listFeatureRectangles.Count != 3)
            {
                MessageBox.Show("特征点检测失败");
                return;
            }
            Rectangle T = _listFeatureRectangles[0];
            Rectangle B = _listFeatureRectangles[1];
            Rectangle O = _listFeatureRectangles[2];
            string Ostr = _listsubjects[2].ToString();
            if(Ostr.StartsWith("L"))
            {
                CorrectRect = new Rectangle( O.X,T.Y, T.Right - O.Left, B.Bottom -T.Top);
            }else
            {
                CorrectRect = new Rectangle(T.X, T.Y, O.Right - T.Left, B.Bottom - T.Top);
            }
        }
        public Rectangle Detected(Bitmap bmp,List<Rectangle> TBO)
        {
            Rectangle T = DetectFeatureRect(_listsubjects[0], bmp);   // T             
            if (T.Width == 0)
                return new Rectangle();
            {
                Rectangle FT = _listFeatureRectangles[0];
                Rectangle FB = _listFeatureRectangles[1];
                Rectangle FO = _listFeatureRectangles[2];
                FB.Offset(-FT.X, -FT.Y);
                FO.Offset(-FT.X, -FT.Y);
                FB.Offset(T.Location);
                FO.Offset(T.Location);

                //FB.Offset(-FB.Width / 2, -FB.Height / 2);
                //FO.Offset(-FO.Width / 2, -FO.Height / 2);
                FB.Inflate(FB.Width/2,FB.Height/2);
                FO.Inflate(FO.Width/2,FO.Height/2);

                TBO.Add(T);
                TBO.Add(FB);
                TBO.Add(FO);
            }
            string Ostr = _listsubjects[2].ToString();
            if (Ostr.StartsWith("L"))
            {
                T.X = T.Right - CorrectRect.Width;
            }
            T.Width = CorrectRect.Width;
            T.Height = CorrectRect.Height;
            return T;
        }
        public Rectangle Detected(Rectangle subrect, Bitmap src)
        {
            if (src == null)
                return new Rectangle();
            subrect.Intersect(new Rectangle(0, 0, src.Width, src.Height));
            return DetectFeatureRect(subrect, src);
        }
        public bool Detected()
        {
            return CorrectRect.Width > 0;
        }

        private Rectangle DetectFeatureRect(subject sub, Bitmap src = null)
        {
            return DetectFeatureRect(sub.Rect, src);
        }
        private Rectangle DetectFeatureRect(Rectangle subrect,Bitmap src=null)
        {
            Size minsize = new Size(30, 30);
            if (src == null)
                src = _src;
            Bitmap bmp =src.Clone(subrect, src.PixelFormat);

            //bmp.Save(subrect.ToString() + ".tif");
            Rectangle rect = DetectFeatureRectAngle(bmp);
            if (rect.Width == 1 || rect.Height == 1)
                return new Rectangle();
            Rectangle rect2 = DetectFeatureRectAngle2(bmp, rect);
            if (rect2.Width == 1 || rect2.Height == 1)
                return new Rectangle();
            if(Math.Abs(rect2.Width - rect.Width)<3 && Math.Abs(rect2.Height - rect.Height)<3){
                rect.Offset(subrect.Location);
                return rect;
            }

            if (rect2.Width != rect.Width || rect2.Height != rect.Height)
            {
                int perheight = minsize.Height * 8 / 10;
                int maxlen = minsize.Width * 9 / 10;
                Rectangle outrect = new Rectangle();
                bool suss = false;
                for (int y = perheight; y < bmp.Height; y += perheight)
                {
                    Rectangle r = new Rectangle(0, y, bmp.Width, 2);
                    suss = DetectFeatureRectAngle3(bmp, r, maxlen, out outrect);
                    if (suss)
                        break;
                }
                if (suss)
                {
                    int x = outrect.X + outrect.Width / 2;
                    Rectangle r = new Rectangle(x, 0, 2, bmp.Height);
                    maxlen = minsize.Height * 9 / 10;
                    bool su = DetectFeatureRectAngle4(bmp, r, maxlen, ref outrect);
                    if (su)
                    {
                        outrect.Offset(subrect.Location);
                        return outrect;
                    }
                }
            }
            return new Rectangle();
        }
        private bool DetectFeatureRectAngle4(Bitmap bmp, Rectangle r, int maxlen, ref Rectangle outrect)
        {
            Rectangle rectyline = r;
            int[] yycnt = BitmapTools.CountYPixsum(bmp, rectyline);
            List<int> ycnt = yycnt.Select(rec => 2 - rec).ToList();

            //count Xpoint
            int Ypoint = 0;
            int len = 0;
            int yblackcnt = 0;
            int Ylen = 1;
            for (int i = 0; i < ycnt.Count; i++)
            {
                if (ycnt[i] > yblackcnt)
                {
                    if (len == 0)
                        Ypoint = i;
                    len++;
                }
                else
                {
                    if (len >= maxlen)
                    {
                        Ylen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            if (len > Ylen)
                Ylen = len;

            outrect.Y = Ypoint;
            outrect.Height = Ylen;
            if (Ylen >= maxlen)
                return true;
            return false;
        }
        private bool DetectFeatureRectAngle3(Bitmap bmp, Rectangle r, int maxlen, out Rectangle outrect)
        {
            Rectangle rectxline = r;
            int[] xxcnt =  BitmapTools.CountXPixsum(bmp, rectxline);
            List<int> xcnt = xxcnt.Select(rec => 2 - rec).ToList();

            //count Xpoint
            int Xpoint = 0;
            int len = 0;
            int xblackcnt = 0;
            int Xlen = 1;
            for (int i = 0; i < xcnt.Count; i++)
            {
                if (xcnt[i] > xblackcnt)
                {
                    if (len == 0)
                        Xpoint = i;
                    len++;
                }
                else
                {
                    if (len >= maxlen)
                    {
                        Xlen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            if (len > Xlen)
                Xlen = len;

            outrect = new Rectangle(Xpoint, 0, Xlen, 2);
            if (Xlen >= maxlen)
                return true;
            return false;
        }
        private Rectangle DetectFeatureRectAngle2(Bitmap bmp, Rectangle rect) //由图片限定
        {
            Rectangle rectxline = new Rectangle(0, rect.Height / 2, rect.Width, 2);
            Rectangle rectyline = new Rectangle(rect.Width / 2, 0, 2, rect.Height);
            rectxline.Offset(rect.Location);
            rectyline.Offset(rect.Location);

            int[] xxcnt = BitmapTools.CountXPixsum(bmp, rectxline);
            int[] yycnt = BitmapTools.CountYPixsum(bmp, rectyline);
            List<int> xcnt = xxcnt.Select(rec => 2 - rec).ToList();
            List<int> ycnt = yycnt.Select(rec => 2 - rec).ToList();


            //count Xpoint
            int Xpoint = 0;
            int len = 0;
            int xblackcnt = 0;
            int yblackcnt = 0;
            int Xlen = 1;
            int Ylen = 1;
            for (int i = 0; i < xcnt.Count; i++)
            {
                if (xcnt[i] > xblackcnt)
                {
                    if (len == 0)
                        Xpoint = i;
                    len++;
                }
                else
                {
                    if (len > yblackcnt)
                    {
                        Xlen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            if (len > Xlen)
                Xlen = len;

            int Ypoint = 0;
            len = 0;
            for (int i = 0; i < ycnt.Count; i++)
            {
                if (ycnt[i] > yblackcnt)
                {
                    if (len == 0)
                        Ypoint = i;
                    len++;
                }
                else
                {
                    if (len > xblackcnt)
                    {
                        Ylen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            if (len > Ylen)
                Ylen = len;
            //string str = string.Join(",", xxcnt) + "\r\n" + string.Join(",", yycnt);
            //File.WriteAllText("a.txt", str);
            return new Rectangle(Xpoint, Ypoint, Xlen, Ylen);
        }
        private Rectangle DetectFeatureRectAngle(Bitmap bmp) //由图片限定
        {
            Size blacktag = new Size(33, 33);
            Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);            
            int[] xxcnt = BitmapTools.CountXPixsum(bmp, r);
            int[] yycnt = BitmapTools.CountYPixsum(bmp, r);
            List<int> xcnt = xxcnt.Select(rec =>
            {
                if ((r.Height - rec) > blacktag.Height / 3)
                    return r.Height - rec;
                return 0;
            }).ToList();
            List<int> ycnt = yycnt.Select(rec =>
            {
                if ((r.Width - rec) > blacktag.Width / 3)
                    return r.Width - rec;
                return 0;
            }).ToList();

            //count Xpoint
            int Xpoint = 0;
            int len = 0;
            int xblackcnt = blacktag.Height * 2 / 3;
            int yblackcnt = blacktag.Width * 2 / 3;
            int Xlen = 1;
            int Ylen = 1;
            for (int i = 0; i < xcnt.Count; i++)
            {
                if (xcnt[i] > xblackcnt)
                {
                    if (len == 0)
                        Xpoint = i;
                    len++;
                }
                else
                {
                    if (len > yblackcnt)
                    {
                        Xlen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
             if (len > yblackcnt)
                Xlen = len;
            int Ypoint = 0;
            len = 0;
            for (int i = 0; i < ycnt.Count; i++)
            {
                if (ycnt[i] > yblackcnt)
                {
                    if (len == 0)
                        Ypoint = i;
                    len++;
                }
                else
                {
                    if (len > xblackcnt)
                    {
                        Ylen = len;
                        break;
                    }
                    else
                        len = 0;
                }
            }
            if (len > xblackcnt)
                Ylen = len;
            //string str = string.Join(",", xxcnt) + "\r\n" + string.Join(",", yycnt);
            //File.WriteAllText("a.txt", str);
            Rectangle rect = new Rectangle(Xpoint, Ypoint, Xlen, Ylen);
            return rect;
        }
        private static void BitMapTo01Map(Bitmap bmp, Rectangle rect)
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < rect.Height; y++)
            {
                for (int x = 0; x < rect.Width; x++)
                {
                    Color c = bmp.GetPixel(x + rect.X, y + rect.Y);
                    int cv = c.ToArgb();
                    if (cv == Color.White.ToArgb())
                        sb.Append("0");
                    else if (cv == Color.Black.ToArgb())
                        sb.Append("1");
                    else
                        sb.Append(".");
                }
                sb.AppendLine();
            }
            File.WriteAllText("bmp电子图.txt", sb.ToString());

        }
        private string Recttostring(Rectangle r)
        {
            return "(" + r.X + "," + r.Y + "," + r.Width + "," + r.Height + ")";
        }

        private System.Drawing.Bitmap _src;
        private List<subject> _listsubjects;
        private List<Rectangle> _listFeatureRectangles;

    }
    public class AutoDetectRectAnge
    {
        public Rectangle Top;
        public Rectangle Bottom;
        public Rectangle Other;
        private List<Rectangle> _P; // _P[0]  LT      _P[1]  LB       _p[2] RT    _p[3]  RB
        public AutoDetectRectAnge()
        {
            _P = new List<Rectangle>();
            _P.Add(new Rectangle());
            _P.Add(new Rectangle());
            _P.Add(new Rectangle());
            _P.Add(new Rectangle());
        }
        public void Clear()
        {
            Top = Bottom = _P[0] = _P[1] = _P[2] = _P[3] = new Rectangle();
        }
        public void InitDetectRectAngel(Size size)
        {
            List<Rectangle> listrect = new List<Rectangle>()
                {
                    new  Rectangle(50, 80, 250, 100),
                    new  Rectangle(size.Width-300, 80, 300, 100),
                    new  Rectangle(50, size.Height-280, 250, 100)
                };
            if (FeatureSetPath != "")
            {
                string filename = FeatureSetPath + ".detectFeatureSet.json";
                if (File.Exists(filename))
                {
                    //string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(Lr));
                    //File.WriteAllText("detectFeatureSet.json", str);
                    listrect = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Rectangle>>(File.ReadAllText(filename));
                }
            }
            ComputTBO(listrect);
        }
        public  void ComputTBO(List<Rectangle> listrect)
        {
            Clear();
            Point Mid = GetMidPoint(listrect);
            foreach (Rectangle r in listrect) //_P[0]  LT      _P[1]  LB       _p[2] RT    _p[3]  RB
            {
                if (r.X < Mid.X) // L
                {
                    if (r.Y < Mid.Y)  // T 
                        _P[0] = r;
                    else //B
                        _P[1] = r;
                }
                else  // R
                {
                    if (r.Y < Mid.Y)  // T 
                        _P[2] = r;
                    else //B
                        _P[3] = r;
                }
            }
            if (_P[0].Width == 0 && _P[2].Width == 0 || _P[1].Width == 0 && _P[3].Width == 0)
                return;
            if (_P[0].Width * _P[1].Width == 0)
            {
                Top = _P[2];
                Bottom = _P[3];
                if (_P[0].Width == 0)
                    Other = _P[1];
                else
                    Other = _P[0];
            }
            else
            {
                Top = _P[0];
                Bottom = _P[1];
                if (_P[2].Width == 0)
                    Other = _P[3];
                else
                    Other = _P[2];
            }
        }
        public List<Point> TBO()
        {
            return new List<Point>()
            {
                Top.Location,Bottom.Location,Other.Location
            };
        }
        private Point GetMidPoint(List<Rectangle> listrect)
        {
            if(listrect.Count<2)
                 throw new NotImplementedException();
            int xmid = (listrect.Select(r => r.Left).Min() + listrect.Select(r => r.Right).Max()) / 2;
            int ymid = (listrect.Select(r => r.Top).Min() + listrect.Select(r => r.Bottom).Max()) / 2;
            return new Point(xmid, ymid);
        }
        public  List<subject> GetSubjects()
        {
            string othername = "L";
            if (Other.X * 2 > Top.X + Bottom.X)
                othername = "R";
            if (Other.Y * 2 > Top.Y + Bottom.Y)
                othername += "B";
            else
                othername += "T";
            return new List<subject>()
            {
                new subject( "Top",Top),
                new subject("Botton",Bottom),
                new subject(othername,Other)
            };
        }
        public static string FeatureSetPath = "";
    }
}
