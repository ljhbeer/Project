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
        public static DetectData DetectImg(Bitmap src, Rectangle templateCorrectRect = new Rectangle())
        {
            if(templateCorrectRect.Width==0)
                return DetectCorrect.DetectCorrectImg(src);
            Point p = DetectLT(src);
            if (p.X < 0 || p.Y < 0)
                return new DetectData(new Rectangle(),null);
            Rectangle correctrect = templateCorrectRect;
            correctrect.Location = p;            
            return DetectCorrect.DetectCorrectImg(src,correctrect);
        }
        public static Point GetMidPoint(List<Rectangle> listrect)
        {
            int xmid = (listrect.Select(r => r.X).Min() + listrect.Select(r => r.X).Max()) / 2;
            int ymid = (listrect.Select(r => r.Y).Min() + listrect.Select(r => r.Y).Max()) / 2;
            return new Point(xmid, ymid);
        }
        private static Point DetectLT(Bitmap src)
        { //20点起始
            Rectangle r = new Rectangle(new Point(), new Size(src.Width / 3, src.Height / 3));
            Bitmap _src = (Bitmap)src.Clone(r, src.PixelFormat);
            Rectangle cr = DetectCorrect.DetectCorrectFromImg(_src);
            if (cr.Width == 0 || cr.Height == 0)
                return new Point(-1, -1);
            return cr.Location;
        }
        public class DetectCorrect
        {
            public static DetectData ReDetectCorrectImg(Bitmap src, DetectData dd)
            {
                Rectangle area = new Rectangle(0, 0, src.Width, src.Height);
                List<Rectangle> FourLtbRtbRect = dd.ListFeature;

                List<Rectangle> list = new List<Rectangle>();
                int cnt = 0;
                foreach (Rectangle r in FourLtbRtbRect)
                {
                    r.Offset(dd.CorrectRect.Location);
                    r.Inflate(r.Size);
                    r.Intersect(area);
                    Bitmap src1 = src.Clone(r, src.PixelFormat);

                    Rectangle nr = DetectFeatureFromImg(src1);
                    if (global.Debug && (global.tag & 8) > 0)
                    {
                        src1.Save("F:\\out\\" + cnt + ".tif");
                        if (nr.Width == 0 || nr.Height == 0)
                        {
                            nr.Width = r.Width / 3;
                            nr.Height = r.Height / 3;
                        }
                        src1.Clone(nr, src1.PixelFormat).Save("F:\\out\\N_" + cnt + ".tif");
                        cnt++;
                    }
                    nr.Offset(r.Location);
                    list.Add(nr);
                }

                if (global.Debug && (global.tag & 8) > 0 && list.Count>2)
                {
                    Rectangle correctrect = Rectangle.Union(list[0], list[1]);
                    for(int i = 2; i<list.Count; i++)
                        correctrect = Rectangle.Union(correctrect, list[i]);
                    Bitmap rgb = ARTemplate.ConvertFormat.ConvertToRGB(src);
                    using (Graphics g = Graphics.FromImage(rgb))
                    {
                        g.DrawRectangle(Pens.Green, correctrect);
                        g.DrawRectangles(Pens.Red, list.ToArray());
                    }
                    rgb.Save("F:\\Drawall.jpeg");
                }

                if (list.Count > 0)
                { // 待修改
                    dd.CorrectRect = new Rectangle(  list[0].Location,dd.CorrectRect.Size);
                }
                if (list.Count > 2)
                {
                    Rectangle correctrect = Rectangle.Union(list[0], list[1]);
                    for (int i = 2; i < list.Count; i++)
                        correctrect = Rectangle.Union(correctrect, list[i]);
                    correctrect.Location = list[0].Location;
                    dd.CorrectRect = correctrect;
                }
                Rectangle _CorrectRect = dd.CorrectRect;
                list = list.Select(r => new Rectangle(r.X - _CorrectRect.X, r.Y - _CorrectRect.Y, r.Width, r.Height)).ToList();
                return new DetectData(_CorrectRect, list);

            }
            public  static DetectData DetectCorrectImg(Bitmap src, Rectangle correctrect = new Rectangle())           
            {
                Rectangle _CorrectRect = correctrect;
                if(correctrect.Width ==0)
                    _CorrectRect = DetectCorrectFromImg(src,false);
                Rectangle area = new Rectangle(0, 0, src.Width, src.Height);
                List<Rectangle> FourLtbRtbRect = GetLrbRtb(_CorrectRect, 40, 40);

                //if (global.Debug && (global.tag & 8) > 0)
                //{
                //    Bitmap rgb = ARTemplate.ConvertFormat.ConvertToRGB(src);
                //    using (Graphics g = Graphics.FromImage(rgb))
                //    {
                //        g.DrawRectangle(Pens.Green, _CorrectRect);
                //        g.DrawRectangles(Pens.Red, FourLtbRtbRect.ToArray());
                //    }
                //    rgb.Save("F:\\Drawall.jpeg");
                //}

                List<Rectangle> list = new List<Rectangle>();
                int   cnt = 0;
                foreach (Rectangle r in FourLtbRtbRect)
                {
                    r.Inflate(r.Size);
                    r.Intersect(area);
                    Bitmap src1 = src.Clone(r, src.PixelFormat);

                    Rectangle nr = DetectFeatureFromImg(src1);
                    if (global.Debug && (global.tag & 8) > 0)
                    {
                        src1.Save("F:\\out\\" + cnt + ".tif");
                        if (nr.Width == 0 || nr.Height == 0)
                        {
                            nr.Width = r.Width / 3;
                            nr.Height = r.Height / 3;
                        }
                        src1.Clone(nr, src1.PixelFormat).Save("F:\\out\\N_" + cnt + ".tif");
                        cnt++;
                    }
                    nr.Offset(r.Location);
                    list.Add(nr);
                }
                if(list.Count>0)
                _CorrectRect.Location = list[0].Location;
                list = list.Select(r => new Rectangle(r.X - _CorrectRect.X, r.Y - _CorrectRect.Y,r.Width,r.Height)).ToList();
                return new DetectData(_CorrectRect, list);
            }
            public static Rectangle DetectFeatureFromImg(Bitmap src)
            {
                return DetectCorrectFromImg(src, new Rectangle(1, 1, 1, 1),true);
            }
            public static Rectangle DetectCorrectFromImg(Bitmap src,Boolean continnuity = true)
            {
                if(continnuity)
                    return DetectCorrectFromImg(src, new Rectangle(15, 15, 15, 15),true,10);
                return DetectCorrectFromImg(src, new Rectangle(30, 30, 30, 30));
            }
            private static Rectangle DetectCorrectFromImg(Bitmap src,Rectangle margin) // 从30位算起
            {
                Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
                int[] xxcnt = BitmapTools.CountXPixsum(src, rect);
                int[] yycnt = BitmapTools.CountYPixsum(src, rect);
                xxcnt = xxcnt.Select(r => yycnt.Length - r).ToArray();
                yycnt = yycnt.Select(r => xxcnt.Length - r).ToArray();
                int xxavg = (int)xxcnt.Average() / 3;
                int yyavg = (int)yycnt.Average() / 3;
                xxcnt = xxcnt.Select(r => r > xxavg ? 100 : 0).ToArray();
                yycnt = yycnt.Select(r => r > yyavg ? 100 : 0).ToArray();

                int xpos = xxcnt.Skip(margin.Left ).ToList().FindIndex(r => r > 0) + margin.Left;
                int xendpos = xxcnt.Length - xxcnt.Reverse().Skip(margin.Right).ToList().FindIndex(r => r > 0) - margin.Right;
                int ypos = yycnt.Skip(margin.Top).ToList().FindIndex(r => r > 0) + margin.Top;
                int yendpos = yycnt.Length - yycnt.Reverse().Skip(margin.Bottom).ToList().FindIndex(r => r > 0) - margin.Bottom;
                //OutPixImage(xxcnt, yycnt);
                return new Rectangle(xpos, ypos, xendpos - xpos, yendpos - ypos);
            }
            private static Rectangle DetectCorrectFromImg(Bitmap src, Rectangle margin,bool continnuity,int continuelength = -1 ) // 从30位算起
            {
                Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
                int[] xxcnt = BitmapTools.CountXPixsum(src, rect);
                int[] yycnt = BitmapTools.CountYPixsum(src, rect);
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
                //OutPixImage(xxcnt, yycnt);
                int xpos = xxcnt.Skip(margin.Left).ToList().FindIndex(r => r > 0) + margin.Left;
                int xendpos = xxcnt.Skip(xpos).ToList().FindIndex(r => r == 0) + xpos;
                if (continnuity)
                {
                    if (continuelength == -1 || continuelength < 1)
                        continuelength = src.Width / 8;
                    while (xendpos - xpos < continuelength )
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
                int ypos = yycnt.Skip(margin.Top).ToList().FindIndex(r => r > 0) + margin.Top;
                int yendpos = yycnt.Skip(ypos).ToList().FindIndex(r => r == 0)+ypos; 
                if (continnuity)
                {
                    if (continuelength == -1 || continuelength < 1)
                        continuelength = src.Width / 8;
                    while (yendpos - ypos <continuelength)
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
