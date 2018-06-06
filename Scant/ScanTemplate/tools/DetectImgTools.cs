using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ScanTemplate;

namespace  Tools
{
    public class DetectImageTools
    {
        public static DetectData DetectImg(Bitmap src,Rectangle area, Rectangle templateCorrectRect = new Rectangle())
        {
            if(templateCorrectRect.Width==0)
                return DetectCorrect.DetectCorrectImg(src,area,templateCorrectRect);
            Point p = DetectLT(src,area,true);
            if (p.X < 0 || p.Y < 0)
                return new DetectData(new Rectangle(),null);
            Rectangle correctrect = templateCorrectRect;
            correctrect.Location = p;            
            return DetectCorrect.DetectCorrectImg(src,area,correctrect);
        }
        public static Point GetMidPoint(List<Rectangle> listrect)
        {
            int xmid = (listrect.Select(r => r.X).Min() + listrect.Select(r => r.X).Max()) / 2;
            int ymid = (listrect.Select(r => r.Y).Min() + listrect.Select(r => r.Y).Max()) / 2;
            return new Point(xmid, ymid);
        }
        public static Point DetectLT(Bitmap src,Rectangle area,bool debug = false)
        {
            Rectangle r = new Rectangle(new Point(), new Size(src.Width / 6, src.Height / 6));
            //Rectangle r = new Rectangle(new Point(), new Size(src.Width / 3, src.Height / 3));
            area.Intersect(r);
            Rectangle cr = DetectCorrect.DetectCorrectFromImg(src,area,true,10,debug);
            if (cr.Width == 0 || cr.Height == 0)
                return new Point(-1, -1);
            //src.Clone(cr, src.PixelFormat).Save("F:\\debug\\LT.tif");
            return cr.Location;
        }
        public class DetectCorrect
        {
            public static DetectData ReDetectCorrectImg(Bitmap src, DetectData dd)
            {
                Rectangle area = new Rectangle(0, 0, src.Width, src.Height);
                Rectangle _CorrectRect = dd.CorrectRect;
                List<Rectangle> FourLtbRtbRect = GetLrbRtb(_CorrectRect, 40, 40);
                List<Rectangle> list = new List<Rectangle>();
                //int   cnt = 0;
                foreach (Rectangle r in FourLtbRtbRect)
                {
                    r.Inflate(r.Size);
                    r.Intersect(area);
                    Rectangle nr2 = DetectCorrectFromImg(src, r, true, r.Width / 6);
                    list.Add(nr2);
                }
                return ConstructDetectData(false, _CorrectRect, list);
            }
            public static DetectData DetectCorrectImg(Bitmap src,Rectangle area,Rectangle correctrect = new Rectangle()) //
            {
                bool HasCorrectRect = true;
                Rectangle _CorrectRect = correctrect;
                if (correctrect.Width == 0)
                {
                    HasCorrectRect = false;
                    _CorrectRect = DetectCorrectFromImg(src, area,false, 10);                  
                }
                //Rectangle area = new Rectangle(0, 0, src.Width, src.Height);
                List<Rectangle> FourLtbRtbRect = GetLrbRtb(_CorrectRect, 40, 40);
                List<Rectangle> list = new List<Rectangle>();
                int cnt = 0;
                foreach (Rectangle r in FourLtbRtbRect)
                {
                    r.Inflate(r.Size);
                    r.Intersect(area);
                    Rectangle nr2 = DetectCorrectFromImg(src, r, true, r.Width / 6);
                    list.Add(nr2);
                    if(global.Debug && (global.tag & 8)>0)
                        src.Clone(nr2, src.PixelFormat).Save("F:\\debug\\" + cnt + "-" + nr2.ToString("_") + ".tif");
                    cnt++;
                }
                return ConstructDetectData(HasCorrectRect, _CorrectRect,list);
            }
            public static Rectangle DetectCorrectFromImg(Bitmap src, Rectangle area, Boolean continnuity = true, int continuelength = -1,bool debug = false)
            {
                Rectangle rect;
                if (continnuity)
                {
                    rect = DetectCorrectFromImgArea(src, area,continnuity,continuelength,debug);
                }
                else
                {
                    rect = DetectCorrectFromImgArea(src, area);
                }
                rect.Offset(area.Location);
                return rect;
            }
         
            private static DetectData ConstructDetectData(bool HasCorrectRect, Rectangle _CorrectRect,  List<Rectangle> list)
            {
                if (list.Count > 0)
                {
                    _CorrectRect.Location = list[0].Location;
                    if (!HasCorrectRect)
                    {
                        int w = list.Select(r => r.Right).Max() - list[0].X;
                        int h = list.Select(r => r.Bottom).Max() - list[0].Y;
                        _CorrectRect = new Rectangle(list[0].X, list[0].Y, w, h);
                    }
                }
                list = list.Select(r => new Rectangle(r.X - _CorrectRect.X, r.Y - _CorrectRect.Y, r.Width, r.Height)).ToList();
                return new DetectData(_CorrectRect, list);
            }   
            private static Rectangle DetectCorrectFromImg2(Bitmap src, Rectangle margin)
            { // 从30位算起
                Rectangle r = new Rectangle(0,0,src.Width,src.Height);
                Rectangle area = new Rectangle(margin.X, margin.Y, src.Width - margin.X - margin.Width, src.Height - margin.Y - margin.Height);
                Rectangle rect = DetectCorrectFromImgArea(src, area);
                rect.Offset(margin.Location);
                return rect;
            }
            private static Rectangle DetectCorrectFromImg2(Bitmap src, Rectangle margin, bool continnuity, int continuelength = -1)
            {

                Rectangle r = new Rectangle(0, 0, src.Width, src.Height);
                Rectangle area = new Rectangle(margin.X, margin.Y, src.Width - margin.X - margin.Width, src.Height - margin.Y - margin.Height);
                Rectangle rect = DetectCorrectFromImgArea(src, area,continnuity,continuelength);
                rect.Offset(margin.Location);
                return rect;
            }
            private static Rectangle DetectCorrectFromImgArea(Bitmap src, Rectangle area) // 从30位算起
            {
                //验证参数准确性
                int[] xxcnt = BitmapTools.CountXPixsum(src, area);
                int[] yycnt = BitmapTools.CountYPixsum(src, area);
                xxcnt = xxcnt.Select(r => yycnt.Length - r).ToArray();
                yycnt = yycnt.Select(r => xxcnt.Length - r).ToArray();
                int xxavg = (int)xxcnt.Average() / 3;
                int yyavg = (int)yycnt.Average() / 3;
                xxcnt = xxcnt.Select(r => r > xxavg ? 100 : 0).ToArray();
                yycnt = yycnt.Select(r => r > yyavg ? 100 : 0).ToArray();

                int xpos = xxcnt.ToList().FindIndex(r => r > 0) ;
                int xendpos = xxcnt.Length - xxcnt.Reverse().ToList().FindIndex(r => r > 0) ;
                int ypos = yycnt.ToList().FindIndex(r => r > 0) ;
                int yendpos = yycnt.Length - yycnt.Reverse().ToList().FindIndex(r => r > 0);
                //OutPixImage(xxcnt, yycnt);
                return new Rectangle(xpos, ypos, xendpos - xpos, yendpos - ypos);
            }
            private static Rectangle DetectCorrectFromImgArea(Bitmap src, Rectangle area, bool continnuity, int continuelength = -1,bool debug = false) // 从30位算起
            {
                //验证参数准确性
                int[] xxcnt = BitmapTools.CountXPixsum(src, area);
                int[] yycnt = BitmapTools.CountYPixsum(src, area);
                xxcnt = xxcnt.Select(r => yycnt.Length - r).ToArray();
                yycnt = yycnt.Select(r => xxcnt.Length - r).ToArray();
                int xxavg = (int)xxcnt.Average() / 3;
                int yyavg = (int)yycnt.Average() / 3;
                if (continnuity)
                {
                    xxavg = (int)(xxcnt.Where(r => r > xxavg).Average() / 2);
                    yyavg = (int)(yycnt.Where(r => r > yyavg).Average() / 2);
                }
                xxcnt = xxcnt.Select(r => r > xxavg ? 100 : 0).ToArray();
                yycnt = yycnt.Select(r => r > yyavg ? 100 : 0).ToArray();

                //完全同上
                //debugout
                if(debug)
                    OutPixImage(xxcnt, yycnt);
                int xpos = xxcnt.ToList().FindIndex(r => r > 0);
                int xendpos = xxcnt.Skip(xpos).ToList().FindIndex(r => r == 0) + xpos;
                if (continnuity)
                {
                    if (continuelength == -1 || continuelength < 1)
                        continuelength = src.Width / 8;
                    while (xendpos - xpos < continuelength)
                    {
                        xpos = xxcnt.Skip(xendpos).ToList().FindIndex(r => r > 0);
                        if (xpos == -1 || xendpos == -1)
                            break;
                        xpos += xendpos;
                        xendpos = xxcnt.Skip(xpos).ToList().FindIndex(r => r == 0);
                        // 修改逻辑错误
                        if (xendpos == -1)
                        {
                            xendpos = xxcnt.Length - 1;
                            break;
                        }
                        else
                            xendpos += xpos;
                    }
                    if (xpos == -1 || xendpos == -1)
                        return new Rectangle(1, 1, 0, 0);
                }
                //xxcnt.Length - xxcnt.Reverse().Skip(margin.Right).ToList().FindIndex(r => r > 0) - margin.Right;
                int ypos = yycnt.ToList().FindIndex(r => r > 0);
                int yendpos = yycnt.Skip(ypos).ToList().FindIndex(r => r == 0) + ypos;
                if (continnuity)
                {
                    if (continuelength == -1 || continuelength < 1)
                        continuelength = src.Width / 8;
                    while (yendpos - ypos < continuelength)
                    {
                        ypos = yycnt.Skip(yendpos).ToList().FindIndex(r => r > 0);
                        if (ypos == -1 || yendpos == -1)
                            break;
                        ypos += yendpos;
                        yendpos = yycnt.Skip(ypos).ToList().FindIndex(r => r == 0);
                        if (yendpos == -1)
                        {
                            yendpos = yycnt.Length - 1;
                            break;
                        }
                        else
                            yendpos += ypos;
                    }
                    if (ypos == -1 || yendpos == -1)
                        return new Rectangle(1, 1, 0, 0);
                }

                return new Rectangle(xpos, ypos, xendpos - xpos, yendpos - ypos);
            }
            public static List<Rectangle> GetLrbRtb(Rectangle r, int width, int height)
            {
                return new List<Rectangle>()
                {
                    new Rectangle(r.Left,r.Top,width,height),
                    new Rectangle(r.Left,r.Bottom-height,width,height),
                    new Rectangle(r.Right-width,r.Top ,width,height),
                    new Rectangle(r.Right-width,r.Bottom-height,width,height),
                };
            }
            private static void OutPixImage(int[] xxcnt, int[] yycnt)
            {
                Bitmap xb = new Bitmap(xxcnt.Count(), 100);
                Bitmap yb = new Bitmap(100, yycnt.Count());
                WhiteImage(xb);
                WhiteImage(yb);
                using (Graphics g = Graphics.FromImage(xb))
                {
                    for (int i = 0; i < xxcnt.Count(); i++)
                    {
                        g.DrawLine(Pens.Black, new Point(i, 0), new Point(i, xxcnt[i]));
                    }
                }
                xb.Save("F:\\out\\xb.jpg");

                using (Graphics g = Graphics.FromImage(yb))
                {
                    for (int i = 0; i < yycnt.Count(); i++)
                    {
                        g.DrawLine(Pens.Black, new Point(0, i), new Point(yycnt[i], i));
                    }
                }
                yb.Save("F:\\out\\yb.jpg");
            }
            private static void WhiteImage(Bitmap _Src)
            {
                if (_Src != null)
                    using (Graphics g = Graphics.FromImage(_Src))
                    {
                        Rectangle _paper = new Rectangle(new Point(), _Src.Size);
                        g.FillRectangle(Brushes.White, _paper);
                    }
            }
        }
    }
    public class DetectData
    {
        public DetectData(Rectangle _CorrectRect, List<Rectangle> list)
        {
            this.CorrectRect = _CorrectRect;
            this.ListFeature = list;
        }
        public Rectangle CorrectRect { get; set; }
        public List<Rectangle> ListFeature { get; set; }
    }    
}
