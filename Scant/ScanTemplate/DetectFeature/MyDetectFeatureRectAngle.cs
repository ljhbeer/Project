using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Tools;
using System.Windows.Forms;
using ScanTemplate;

namespace Tools
{
    class MyDetectFeatureRectAngle
    {
       
        public MyDetectFeatureRectAngle(System.Drawing.Bitmap bmp)
        {
            this._src = bmp;
            if (_src != null)
            {
                _listsubjects = AutoDetectRectAnge.GetSubjects(bmp.Size);
                DetectedOK = Detect3Point();
                if( (!DetectedOK && global.Debug) || (global.Debug && ( global.tag & 1)>0) )
                {
                    //List<int> ll = 
                    _listsubjects.Select(r =>
                    {
                        _src.Clone(r.Rect, _src.PixelFormat).Save("F:\\debug\\"+ r.ToString()+"_"+r.Rect.ToString("-") + ".tif");
                        return r.Rect.X;
                    }).ToList();
                }
            }
        }
        public MyDetectFeatureRectAngle(List<ARTemplate.FeaturePoint> list, Rectangle CorrectRect)
        {
            _listFeatureRectangles = list.Select(r => r.Rect).ToList();
            if(list.Count>0)
            _listsubjects = AutoDetectRectAnge.GetSubjects( list[0].Rect.Size );
            this.CorrectRect = CorrectRect;
        }
        public bool Detect3Point()
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
                return false;
            }
            Rectangle T = _listFeatureRectangles[0];
            Rectangle B = _listFeatureRectangles[1];
            Rectangle O = _listFeatureRectangles[2];
            return true;
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

        public Rectangle CorrectRect { get; set; }
        public List<Rectangle> ListFeatureRectangle
        {
            get
            {
                return _listFeatureRectangles;
            }
        }
        public  bool DetectedOK { get; set; }

        private System.Drawing.Bitmap _src;
        private List<subject> _listsubjects;
        private List<Rectangle> _listFeatureRectangles;
    }
    public class AutoDetectRectAnge
    {
        public static string FeatureSetPath = "";
        public static List<subject> GetSubjects(Size size)
        {
            return GetSubjects(size.Width, size.Height);
        }
        public static List<subject> GetSubjects(int sizewidth=30, int sizeheight=30)
        {
            List<Rectangle> list = AutoGetDetectRectAngel(new Size(sizewidth, sizeheight));
            List<Rectangle> TBO = AutoTBO.GetAutoTBORect(list);
            string othername = OtherName(TBO);
            return new List<subject>()
            {
                new subject( "Top",TBO[0]),
                new subject("Bottom",TBO[1]),
                new subject(othername,TBO[2])
            };
        }
        public static string OtherName(List<Rectangle> TBO)
        {
            string othername = "L";
            if (TBO[2].X * 2 > TBO[0].X + TBO[1].X)
                othername = "R";
            if (TBO[2].Y * 2 > TBO[0].Y + TBO[1].Y)
                othername += "B";
            else
                othername += "T";
            return othername;
        }
        private static List<Rectangle> AutoGetDetectRectAngel(Size size)
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
                if (!File.Exists(filename))
                    filename = FeatureSetPath.Substring(0, FeatureSetPath.LastIndexOf("\\") + 1) + "default.detectFeatureSet.json";
                if (File.Exists(filename))
                {
                    listrect = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Rectangle>>(File.ReadAllText(filename));
                }
            }
            return listrect;
        }

    }
    public class AutoTBO  //确保 
    {
        public static List<Rectangle> GetAutoTBORect(List<Rectangle> list)
        {
            if(list.Count<2)
                 throw new NotImplementedException();
            List<Point> _list = ComputTBO(list.Select(r => r.Location).ToList());
            if (list == null)
                return null;
            List<Rectangle> listrect = new List<Rectangle>();
            foreach (Point p in _list)
            {
                listrect.AddRange(list.Where(r => r.Location == p));
            }
            return listrect;
        }
        public static List<Point>  GetAutoTBO(List<Rectangle> list )
        {
            if(list.Count<2)
                 throw new NotImplementedException();
            return ComputTBO(list.Select(r => r.Location).ToList());
        }
        public static List<Point>  GetAutoTBO(List<Point> list )
        {
            if(list.Count<2)
                 throw new NotImplementedException();
            return ComputTBO(list);
        }
        private static List<Point> ComputTBO(List<Point> listrect)
        {
            Point Top, Bottom,Other;
            Point[]  _P = new Point[4];
            Point Mid = GetMidPoint(listrect);
            foreach (Point r in listrect) //_P[0]  LT      _P[1]  LB       _p[2] RT    _p[3]  RB
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
            if (_P[0]==null && _P[2]==null || _P[1]==null && _P[3]==null)
                return null;
            if (_P[0]==null || _P[1]==null)
            {
                Top = _P[2];
                Bottom = _P[3];
                if (_P[0]==null)
                    Other = _P[1];
                else
                    Other = _P[0];
            }
            else
            {
                Top = _P[0];
                Bottom = _P[1];
                if (_P[2]==null)
                    Other = _P[3];
                else
                    Other = _P[2];
            }
            return new List<Point>() { Top, Bottom, Other };
        }
        public static Point GetMidPoint(List<Point> listrect)
        {
            int xmid = (listrect.Select(r => r.X).Min() + listrect.Select(r => r.X).Max()) / 2;
            int ymid = (listrect.Select(r => r.Y).Min() + listrect.Select(r => r.Y).Max()) / 2;
            return new Point(xmid, ymid);
        }
        public static Point GetMidPoint(List<Rectangle> listrect)
        {
            int xmid = (listrect.Select(r => r.X).Min() + listrect.Select(r => r.X).Max()) / 2;
            int ymid = (listrect.Select(r => r.Y).Min() + listrect.Select(r => r.Y).Max()) / 2;
            return new Point(xmid, ymid);
        }
    }
}
