using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Tools;
namespace ARTemplate
{
    // X 水平  Y垂直
    class DetectChoiceArea
    {
        public DetectChoiceArea(System.Drawing.Bitmap bitmap, int count)
        {
            this._src = bitmap;
            this.choicecount = count;
            this.choicesize = new Size(0,0);
        }

        public bool DetectCustomDF(Boolean vertical)
        {
            List<int> xposlen, yposlen;
            Rectangle rt = new Rectangle(0, 0, _src.Size.Width, _src.Size.Height);
            int[] ycnt = BitmapTools.CountYPixsum(_src, rt);
            int[] xcnt = BitmapTools.CountXPixsum(_src, rt);

            ycnt = ycnt.Select(r => _src.Width - r).ToArray();
            xcnt = xcnt.Select(r => _src.Height - r).ToArray();

            int ymin = (int)Math.Ceiling(ycnt.Where(r => r > 0).Average()/1.5);
            int xmin = (int)Math.Ceiling(xcnt.Where(r => r > 0).Average() / 1.5);
            yposlen = BitmapTools.SectionCount(ycnt, ymin, _src.Width, 2);
            xposlen = BitmapTools.SectionCount(xcnt, xmin, _src.Height, 2);
            if (xposlen.Count == 0 || yposlen.Count == 0)
                return false;
            MergeSection(yposlen);
            MergeSection(xposlen);

            int optioncount = choicecount;
            if (vertical)
            {
                xposlen = BitmapTools.SectionCount(xcnt, xmin, _src.Height, 2);
                if (RemoveAndCheck(xposlen, 1) && RemoveAndCheck(yposlen, optioncount))
                {
                    m_choicepoint = new List<List<Point>>();
                    int sum = 0;
                    for (int i = 1; i < xposlen.Count; i += 2)
                    {
                        sum += xposlen[i] * xposlen[i];
                    }
                    choicesize.Width = (int)Math.Sqrt(sum * 2 / xposlen.Count);
                    sum = 0;
                    for (int i = 1; i < yposlen.Count; i += 2)
                    {
                        sum += yposlen[i] * yposlen[i];
                    }
                    choicesize.Height = (int)Math.Sqrt(sum * 2 / yposlen.Count);

                    for (int i = 0; i < yposlen.Count; i += 2)
                    {
                        List<Point> c = new List<Point>();
                        for (int j = 0; j < xposlen.Count; j += 2)
                            c.Add(new Point(xposlen[j], yposlen[i]));
                        m_choicepoint.Add(c);
                    }
                    return true;
                }
            }
            else
            {
                if (RemoveAndCheck(xposlen, optioncount) && RemoveAndCheck(yposlen, 1))
                {
                    m_choicepoint = new List<List<Point>>();
                    int sum = 0;
                    for (int i = 1; i < xposlen.Count; i += 2)
                    {
                        sum += xposlen[i] * xposlen[i];
                    }
                    choicesize.Width = (int)Math.Sqrt(sum * 2 / xposlen.Count);
                    sum = 0;
                    for (int i = 1; i < yposlen.Count; i += 2)
                    {
                        sum += yposlen[i] * yposlen[i];
                    }
                    choicesize.Height = (int)Math.Sqrt(sum * 2 / yposlen.Count);

                    for (int i = 0; i < yposlen.Count; i += 2)
                    {
                        List<Point> c = new List<Point>();
                        for (int j = 0; j < xposlen.Count; j += 2)
                            c.Add(new Point(xposlen[j], yposlen[i]));
                        m_choicepoint.Add(c);
                    }
                    return true;
                }

            }
            //DrawToFile(xcnt);
            return false;
        }
        public bool DetectKH(Boolean vertical) //垂直排列
        {
            List<int> xposlen, yposlen;
            Rectangle rt = new Rectangle(0, 0, _src.Size.Width, _src.Size.Height);
            int[] ycnt = BitmapTools.CountYPixsum(_src, rt);
            int[] xcnt = BitmapTools.CountXPixsum(_src, rt);
           
            ycnt = ycnt.Select(r => _src.Width - r).ToArray();
            xcnt = xcnt.Select(r => _src.Height - r).ToArray();

            int ymin = (int)Math.Ceiling(ycnt.Where(r => r > 0).Average() / 10);
            int xmin = (int)Math.Ceiling(xcnt.Where(r => r > 0).Average() / 10);
            yposlen = BitmapTools.SectionCount(ycnt, ymin, _src.Width, 2);
            xposlen = BitmapTools.SectionCount(xcnt, xmin, _src.Height, 2);
            if (xposlen.Count == 0 || yposlen.Count == 0)
                return false;
            MergeSection(yposlen);
            MergeSection(xposlen);

            if (vertical)
            {
                xposlen = BitmapTools.SectionCount(xcnt, xmin, _src.Height, 2);
                if (RemoveAndCheck(xposlen, choicecount ) && RemoveAndCheck(yposlen, 10))
                {
                    m_choicepoint = new List<List<Point>>();
                    int sum = 0;
                    for (int i = 1; i < xposlen.Count; i += 2)
                    {
                        sum += xposlen[i] * xposlen[i];
                    }
                    choicesize.Width = (int)Math.Sqrt(sum * 2 / xposlen.Count);
                    sum = 0;
                    for (int i = 1; i < yposlen.Count; i += 2)
                    {
                        sum += yposlen[i] * yposlen[i];
                    }
                    choicesize.Height = (int)Math.Sqrt(sum * 2 / yposlen.Count);

                    for (int i = 0; i < yposlen.Count; i += 2)
                    {
                        List<Point> c = new List<Point>();
                        for (int j = 0; j < xposlen.Count; j += 2)
                            c.Add(new Point(xposlen[j], yposlen[i]));
                        m_choicepoint.Add(c);
                    }
                    return true;
                }
            }
            else
            {
                if (RemoveAndCheck(xposlen, 10) && RemoveAndCheck(yposlen, choicecount))
                {
                    m_choicepoint = new List<List<Point>>();
                    int sum = 0;
                    for (int i = 1; i < xposlen.Count; i += 2)
                    {
                        sum += xposlen[i] * xposlen[i];
                    }
                    choicesize.Width = (int)Math.Sqrt(sum * 2 / xposlen.Count);
                    sum = 0;
                    for (int i = 1; i < yposlen.Count; i += 2)
                    {
                        sum += yposlen[i] * yposlen[i];
                    }
                    choicesize.Height = (int)Math.Sqrt(sum * 2 / yposlen.Count);

                    for (int i = 0; i < yposlen.Count; i += 2)
                    {
                        List<Point> c = new List<Point>();
                        for (int j = 0; j < xposlen.Count; j += 2)
                            c.Add(new Point(xposlen[j], yposlen[i]));
                        m_choicepoint.Add(c);
                    }
                    return true;
                }
               
            }
            //DrawToFile(xcnt);
            return false;
        }
        public bool Detect()
        {               
            List<int> xposlen,yposlen;
            Rectangle rt = new Rectangle(0, 0, _src.Size.Width, _src.Size.Height);
            int[] ycnt = BitmapTools.CountYPixsum(_src, rt);
            int[] xcnt = BitmapTools.CountXPixsum(_src, rt);
            ycnt = ycnt.Select(r => _src.Width - r).ToArray();
            xcnt = xcnt.Select(r => _src.Height - r).ToArray();
            
            int ymin =(int) Math.Ceiling( ycnt.Where(r => r > 0).Average() / 4);
            int xmin =(int) Math.Ceiling( xcnt.Where(r => r > 0).Average() / 4);
            ycnt = ycnt.Select(r => r>ymin?r :0 ).ToArray();
            xcnt = xcnt.Select(r => r>xmin?r :0).ToArray();

            yposlen = BitmapTools.SectionCount(ycnt, ymin, _src.Width, 2);
            xposlen = BitmapTools.SectionCount(xcnt, xmin, _src.Height, 2);           
            if (xposlen.Count == 0 || yposlen.Count == 0)
                return false;
            MergeSection(yposlen);
            MergeSection(xposlen);
            if (RemoveAndCheck(xposlen,4) && RemoveAndCheck(yposlen,choicecount))
            {
                m_choicepoint = new List<List<Point>>();
                int sum = 0;
                for (int i = 1; i < xposlen.Count; i += 2)
                {
                    sum += xposlen[i] * xposlen[i];
                }
                choicesize.Width = (int)Math.Sqrt(sum * 2 / xposlen.Count);
                sum = 0;
                for (int i = 1; i < yposlen.Count; i += 2)
                {
                    sum += yposlen[i] * yposlen[i];
                }
                choicesize.Height = (int)Math.Sqrt(sum * 2 / yposlen.Count);
                
                for (int i = 0; i < yposlen.Count; i+=2)
                {
                    List<Point> c = new List<Point>();
                    for (int j = 0; j < xposlen.Count; j += 2) 
                        c.Add(new Point(xposlen[j],yposlen[i]));
                    m_choicepoint.Add(c);
                }
                return true;
            }
            //DrawToFile(xcnt);
            return false;
        }        
        private bool RemoveAndCheck(List<int> xposlen, int cnt)
        {
            if (xposlen.Count < cnt * 2) return false;
            if (xposlen.Count > cnt * 2)
            {
                int sum = 0;
                for (int i = 1; i < xposlen.Count; i += 2)
                {
                    sum += xposlen[i]*xposlen[i];
                }
                int avg = (int)Math.Sqrt(sum * 2 / xposlen.Count); 
                for (int i = 1; i < xposlen.Count; i += 2)
                {
                    if (Math.Abs(avg - xposlen[i]) > 10)
                    {
                        xposlen.RemoveRange(i - 1, 2);
                    }
                }
            }
            if (xposlen.Count == cnt * 2)
            {
                int sum = 0;
                for (int i = 1; i < xposlen.Count; i += 2)
                {
                    sum += xposlen[i];
                }
                int avg =(int)( sum*2 / xposlen.Count);
                for (int i = 1; i < xposlen.Count; i += 2)
                {
                    if (Math.Abs(avg - xposlen[i]) > avg/4)
                        return false;
                }
                return true;
            }
            return false;
        }
        private void DrawToFile(int[] xcnt)
        {
            Rectangle r = new Rectangle(0, 0, _src.Size.Width, _src.Size.Height);
            if (_src.PixelFormat == PixelFormat.Format32bppArgb)
            {
                BitmapData bitmapdata = _src.LockBits(r, ImageLockMode.ReadWrite, _src.PixelFormat);
                Color c = Color.Black;
                unsafe
                {
                    byte* ptr = (byte*)(bitmapdata.Scan0) + 3 * bitmapdata.Stride;
                    for (int i = 0; i < bitmapdata.Width; i++)
                    {
                        if (xcnt[i] > 0)
                        {
                            ptr[0] = 0;
                            ptr[1] = 0;
                            ptr[2] = 255;
                            ptr[3] = 255;
                        }
                        ptr += 4;
                    }
                }
                _src.UnlockBits(bitmapdata);
                _src.Save("f:\\" + 5 + ".png");
            }
            if (_src.PixelFormat == PixelFormat.Format1bppIndexed)
            {
                BitmapData bitmapdata = _src.LockBits(r, ImageLockMode.ReadWrite, _src.PixelFormat);
                unsafe
                {
                    byte* ptr = (byte*)(bitmapdata.Scan0) + 3 * bitmapdata.Stride;
                    for (int i = 0; i < bitmapdata.Width; )
                    {
                        Byte flag = 0;
                        for (int k = 0; k < 8 && i < bitmapdata.Width; k++, i++)
                        {
                            flag *= 2;
                            if (xcnt[i] == 0)
                            {
                                flag += 1;
                            }
                        }
                        *ptr = flag;
                        ptr++;
                    }
                }
                _src.UnlockBits(bitmapdata);
                _src.Save("f:\\" + 6 + ".png");
            }           
        }
        private void MergeSection(List<int> pos)
        {
            List<int> npos=new List<int>();
            npos.Add(pos[0]);
            npos.Add(pos[1]);
            for (int i = 2; i < pos.Count - 1; i+=2)
            {
                int gap = pos[i] - npos[npos.Count-2] - npos[npos.Count-1];
                if (gap < 3 || gap < 5 && npos[npos.Count - 1] + pos[i + 1] + gap > 10) //|| gap < 6 && npos[npos.Count - 1] + pos[i + 1] +gap>15
                {
                    npos[npos.Count - 1] += pos[i+1]+gap;
                }
                else
                {
                    npos.Add(pos[i]);
                    npos.Add(pos[i+1]);
                }
            }
            pos.Clear();
            pos.AddRange(npos);
        }

        public Size Choicesize
        {
            get { return choicesize; }
            set { choicesize = value; }
        }
        public List<List<Point>> Choicepoint
        {
            get { return m_choicepoint; }
            set { m_choicepoint = value; }
        }
        private System.Drawing.Bitmap _src;
        private int choicecount;
        private Size choicesize;
        private List<List<Point>> m_choicepoint;
    }
}
