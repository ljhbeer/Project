using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace AR
{
    // X 水平  Y垂直
   public class DetectChoiceArea
    {
        public DetectChoiceArea(System.Drawing.Bitmap bitmap, int count)
        {
            // TODO: Complete member initialization
            this._src = bitmap;
            this.choicecount = count;
            this.choicesize = new Size(0,0);
        }
        public bool Detect()
        {
            int[] ycnt,xcnt;                  
            List<int> xposlen,yposlen;            
            Rectangle r = new Rectangle(0,0,_src.Size.Width,_src.Size.Height);
            CountYPixsum(_src, r, out ycnt);
            CountXPixsum(_src, r, out xcnt);            
            for (int i = 0; i < ycnt.Count(); i++)
                ycnt[i] = _src.Width - ycnt[i];
            for (int j = 0; j < xcnt.Count(); j++)
                xcnt[j] = _src.Height - xcnt[j];
            yposlen = SectionCount(ycnt, 0, 200, 2);
            xposlen = SectionCount(xcnt, 0, 200, 1);
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
                    if (Math.Abs(avg - xposlen[i]) > 4)
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
        public static int[] bitcount;
        static DetectChoiceArea ( )
        {
            bitcount = new int[256];

            for (int i = 0; i < 256; i++)
            {
                int flag = 1;
                for (int j = 0; j < 8; j++)
                {
                    if( (i & flag)>0) bitcount[i]++;
                    flag *= 2;
                }

            }

         }
        private static void NewComputeLinepos(int[] cnt, out List<int> ylinepos)
        {
            List<int> pos = SectionCount(cnt,2,200,5);
            ylinepos = new List<int>();
            List<int> lcnt = new List<int>(cnt);
            for (int i = 0; i < pos.Count; i += 2)
            {//
                int avg =(int )( lcnt.GetRange(pos[i], pos[i + 1]).Average()*1.3);
                for (int j = pos[i]; j < pos[i] + 3; j++)
                {
                    if (lcnt[j] > avg)
                    {
                        ylinepos.Add(j);
                        break;
                    }
                }
                for (int j = pos[i]+pos[i+1]-3; j < pos[i] + pos[i+1]; j++)
                {
                    if (lcnt[j] > avg)
                    {
                        ylinepos.Add(j);
                        break;
                    }
                }
            }
        }
        private static List<int> SectionCount(int[] cnt,int min, int max, int minlen)// [0: startpos  1:length]
        {
            int len = 0;
            int spos = 0;
            List<int> pos = new List<int>();
            bool flag = false;
            for (int i = 0; i < cnt.Count(); i++)
            {
                if (cnt[i] > min && cnt[i]<max)
                {
                    if (flag == false)
                    {
                        spos =  i;
                        len = 0;
                    }
                    else
                    {
                        len++;
                    }
                    flag = true;
                }
                else
                {
                    flag = false;
                    if (len >= minlen)
                    {//
                        pos.Add(spos);
                        pos.Add(len+1);
                        len = 0;
                    }
                }
            }
            return pos;
        }
        public static void DrawRect(Rectangle r, BitmapData data, Color c)
        {
            if (r.X > 0 && r.Right < data.Width && r.Y > 0 && r.Bottom < data.Height
                && r.Width > 2 && r.Width < data.Width && r.Height > 2 && r.Height < data.Height)
            {
                unsafe
                {
                    byte* ptr = (byte*)(data.Scan0);
                    byte* ptrfirst = ptr;
                    //TB
                    ptr = ptrfirst + data.Stride * r.Y + r.X * 3;
                    for (int j = 0; j < r.Width; j++)
                    {
                        ptr[0] = c.B;
                        ptr[1] = c.G;
                        ptr[2] = c.R;
                        ptr += 3;
                    }
                    ptr = ptrfirst + data.Stride * r.Bottom + r.X * 3;              
                    for (int j = 0; j < r.Width; j++)
                    {
                        ptr[0] = c.B;
                        ptr[1] = c.G;
                        ptr[2] = c.R;
                        ptr += 3;
                    }
                    //LR   
                    ptr = ptrfirst + data.Stride * r.Y + r.X * 3;
                    for (int i = 0; i < r.Height; i++)
                    {
                        ptr[0] = c.B;
                        ptr[1] = c.G;
                        ptr[2] = c.R;
                        ptr += data.Stride;
                    }
                    ptr = ptrfirst + data.Stride * r.Y + r.Right * 3;
                    for (int i = 0; i < r.Height; i++)
                    {
                        ptr[0] = c.B;
                        ptr[1] = c.G;
                        ptr[2] = c.R;
                        ptr += data.Stride;
                    }
                }
            }
        }
        private static void DrawLine(Rectangle r, BitmapData data,Color c)
        {
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                byte* ptrfirst = ptr;
                //TB
                ptr = ptrfirst + data.Stride * r.Y + r.X * 3;
                for (int j = 0; j < r.Width; j++)
                {
                    ptr[0] = c.R;
                    ptr[1] = c.G;
                    ptr[2] = c.B;
                    ptr += 3;
                }
            }
        }
        public static void Gray(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //循环处理 
            unsafe
            {
                byte temp = 0;
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        // write the logic implementation here 
                        temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);
                        ptr[0] = ptr[1] = ptr[2] = temp;
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);
        }
        public static void Fill(Bitmap bmp, int color)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //循环处理 
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        ptr[0] = ptr[1] = ptr[2] = (byte)color;
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);
        }
        private static int[] CountImgXBlackCnt(Bitmap bmp, Rectangle r)
        {
            int[] rv = new int[r.Width];
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                byte* ptr0 = ptr;
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        if (ptr[0] == 0) rv[j]++;
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);
            return rv;
        }
        public static int CountRectBlackcnt(Bitmap bmp, Rectangle r)
        {
            int cnt = 0;
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, bmp.PixelFormat );
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                byte* ptr0 = ptr;
                if (bmp.PixelFormat == PixelFormat.Format1bppIndexed)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; )
                        {
                            Byte flag = 0x80;
                            for (int k = 0; k < 8 && j < data.Width; k++, j++)
                            {
                                if ((*ptr & flag) > 0) cnt++;
                                flag /= 2;
                            }
                            ptr++;
                        }
                        ptr += data.Stride - (data.Width + 7) / 8;
                    }
                }
                else if (bmp.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    long lcnt = 0;
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        {
                            lcnt += ptr[0];
                            ptr += 3;
                        }
                        ptr += data.Stride - data.Width * 3;
                    }
                    cnt =(int)( lcnt / 255);
                }
                else if (bmp.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    long lcnt = 0;
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        {
                            lcnt += ptr[1];
                            ptr += 4;
                        }
                        ptr += data.Stride - data.Width * 4;
                    }
                    cnt = (int)(lcnt / 255);
                }
            }
            bmp.UnlockBits(data);
            return r.Width*r.Height - cnt;
        }
        private static void CountXPixsum(Bitmap bmp, Rectangle r, out int[] xcnt)
        {
            xcnt = new int[r.Size.Width];
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, bmp.PixelFormat);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                if (bmp.PixelFormat == PixelFormat.Format1bppIndexed)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width ; )
                        {
                            Byte flag= 0x80;
                            for (int k = 0; k < 8 && j < data.Width; k++, j++)
                            {
                                if ((*ptr & flag) > 0) xcnt[j]++;
                                flag /= 2;
                            }
                            ptr++;
                        } 
                        ptr += data.Stride - (data.Width+7) / 8;
                    }
                }
                else if (bmp.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        { 
                            xcnt[j] += ptr[0];
                            ptr += 3;
                        }
                        ptr += data.Stride - data.Width * 3;
                    }
                    for (int i = 0; i < xcnt.Length; i++)
                        xcnt[i] /= 255;
                }
                else if (bmp.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        {
                            xcnt[j] += ptr[1];
                            ptr += 4;
                        }
                        ptr += data.Stride - data.Width * 4;
                    }
                    for (int i = 0; i < xcnt.Length; i++)
                        xcnt[i] /= 255;
                }
            }
            bmp.UnlockBits(data);
        }
        private static void CountYPixsum(Bitmap bmp, Rectangle r, out int[] ycnt)
        {
            ycnt = new int[r.Size.Height];
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, bmp.PixelFormat);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);

                if (bmp.PixelFormat == PixelFormat.Format1bppIndexed)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width/8; j++)
                        {
                            ycnt[i] += bitcount[ ptr[0] ];
                            ptr ++;
                        }
                        byte flag = 128;
                        for (int j = 0; j < data.Width % 8; j++)
                        {
                            if( (*ptr & flag) > 0) ycnt[i]++;
                            flag /=2;
                        }
                        ptr += data.Stride - data.Width/8;
                    }
                }
                else if (bmp.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        { // write the logic implementation here 
                            ycnt[i] += ptr[0];
                            ptr += 3;
                        }
                        ptr += data.Stride - data.Width * 3;
                    }
                    for (int i = 0; i < ycnt.Length; i++)
                        ycnt[i] /= 255;
                }
                else if (bmp.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        { // write the logic implementation here 
                            ycnt[i] += ptr[1];
                            ptr += 4;
                        }
                        ptr += data.Stride - data.Width * 4;
                    }
                    for (int i = 0; i < ycnt.Length; i++)
                        ycnt[i] /= 255;
                }
            }
            bmp.UnlockBits(data);
        }
        private static void ComputeLinepos(int[] xcnt, out List<int> linepos)
        {//从0开始
            linepos = new List<int>();
            int[] cnt;
            cnt = new int[xcnt.Length];
            //int b = photorect.Left ,e = photorect.Right ; 
            for (int i = 2; i < xcnt.Length; i++)
            {
                cnt[i] = xcnt[i] - xcnt[i - 2];
            }
            cnt[0] = cnt[1] = 0;

            int[] maxpos, minpos, flag;
            maxpos = new int[xcnt.Length / 20];
            minpos = new int[xcnt.Length / 20];
            flag = new int[xcnt.Length];
            for (int i = 0; i < maxpos.Length; i++)
            {
                maxpos[i] = Max(cnt, i * 20, 20);
                minpos[i] = Min(cnt, i * 20, 20);
            }

            var queryResults = from n in maxpos select n;
            int maxavg = (int)queryResults.Average() + 1;
            queryResults = from n in minpos select n;
            int minavg = (int)queryResults.Average() + 1;
            // if(debug&2 && option&1) std::printf("X-maxavg:%d  minavg:%d\n",maxavg,minavg);
            if (maxavg < 5) maxavg = 5;
            if (minavg > -5) minavg = -5;

            for (int x = 2; x < xcnt.Length - 1; x++)
            {
                if ((cnt[x] > maxavg || cnt[x + 1] > maxavg)
                  && (cnt[x] < -minavg || cnt[x - 1] < -minavg))
                {
                    flag[x] = 1;
                    linepos.Add(x);
                    x++;
                }
            }
        }
        private static int Min(int[] cnt, int bindex, int len)
        {
            int min = cnt[bindex];
            for (int i = 1; i < len; i++)
            {
                if (min > cnt[i + bindex]) min = cnt[i + bindex];
            }
            return min;
        }
        private static int Max(int[] cnt, int bindex, int len)
        {
            int max = cnt[bindex];
            for (int i = 1; i < len; i++)
            {
                if (max < cnt[i + bindex]) max = cnt[i + bindex];
            }
            return max;
        }
        private static int GetMaxPos(int a, int b, int c)
        {
            int max = a > b ? a : b;
            max = max > c ? max : c;
            if (a == max) return 0;
            if (b == max) return 1;
            if (c == max) return 2;
            throw new NotImplementedException();
        }
        private static int GetMinPos(int minpos, int[] cnt)
        {
            if (minpos < 3 || minpos > cnt.Length - 3) return minpos;
            int min = Min(cnt, minpos - 3, 6);
            for (int i = minpos - 3; i < minpos + 4; i++)
                if (min == cnt[i]) return i;
            return 0;
        }
        private static int GetMaxPos(int maxpos, int[] cnt)
        {
            if (maxpos < 3 || maxpos > cnt.Length - 3) return maxpos;
            int max = Max(cnt, maxpos - 3, 6);
            for (int i = maxpos - 3; i < maxpos + 4; i++)
                if (max == cnt[i]) return i;
            return 0;
        }
        private static void CountGridOption(Bitmap gdt, Rectangle r, int[,] blackcnt)
        {
            int H = blackcnt.GetLength(1);
            int W = blackcnt.GetLength(0);
           // ShowForm s = new ShowForm();
            for (int x = 0; x < W; x++)
            {
                for (int y = 0; y < H; y++)
                {
                    int xcent = (int)((x + 0.5) * r.Width / W + r.X);
                    int ycent = (int)((y + 0.5) * r.Height / H + r.Y);
                    Rectangle nr = new Rectangle(xcent - 5, ycent - 5, 10, 10);//
                    blackcnt[x, y] = CountRectBlackcnt(gdt, nr);
                    //if ((debug & 1024) != 0)
                    //{
                    //    BitmapData data = gdt.LockBits(new Rectangle(0, 0, gdt.Width, gdt.Height),
                    //                        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    //    DrawRect(nr, data, Color.Green);//debug
                    //    gdt.UnlockBits(data);
                    //    s.ShowImg(gdt);
                    //}
                }
            }
        }
        private static bool TestRightOption(int[,] blackcnt, Rectangle r, ref  int value)
        {
            List<int> vcnt = new List<int>();
            if (r.Width == 1)
            { // 选项向下
                for (int i = 0; i < r.Height; i++) vcnt.Add(blackcnt[r.X, i + r.Y]);
            }
            else if (r.Height == 1)
            {
                for (int i = 0; i < r.Width; i++) vcnt.Add(blackcnt[i + r.X, r.Y]);
            }
            else { value = -1; return false; }

            int max = vcnt.Max();
            int sum = vcnt.Sum();
            int cnt = vcnt.Count(n => n > 50);
            int maxpos = vcnt.FindIndex(n => n == max);

            value = maxpos;
            if (cnt == 1)
            {
                return true;
            }	//单选， 暂时不支持多选
            if (max >= 40 && max > sum - max)
            {
                return true;
            }
            else if (max >= 25 && max > 2 * (sum - max))
            {
                return true;
            }
            value = -1;
            return false;
        }
        private static int GetstatisticGamma(Bitmap bmp, int pergamma, Rectangle r)
        {
            int[] colorcnt = new int[256];
            int sum = 0;
            int alltotal = 0;
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int y = 0; y < r.Height; y++)
                {
                    for (int x = 0; x < r.Width; x++)
                    {
                        colorcnt[ptr[0]]++;
                        alltotal++;
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);
            for (int i = 0; i < 256; i++)
            {
                sum += colorcnt[i];
                int y = (int)(sum * 1.0 / (alltotal * 1.0 / 100.0));
                if (y >= pergamma)
                    return i;
            }
            return 0;
        }
        private static void GammaImg(Bitmap gdt, Bitmap src, int gamma, Rectangle r)
        {
            BitmapData datagdt = gdt.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData databmp = src.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(databmp.Scan0);
                byte* ptrgdt = (byte*)(datagdt.Scan0);
                for (int y = 0; y < r.Height; y++)
                {
                    for (int x = 0; x < r.Width; x++)
                    {
                        ptrgdt[0] = ptrgdt[1] = ptrgdt[2] = (byte)(ptr[0] > gamma ? 255 : 0);
                        ptr += 3;
                        ptrgdt += 3;
                    }
                    ptr += databmp.Stride - databmp.Width * 3;
                    ptrgdt += datagdt.Stride - datagdt.Width * 3;
                }
            }
            src.UnlockBits(databmp);
            gdt.UnlockBits(datagdt);
        }
    }
   public class ScoresImg
   {
       public static Bitmap scoresimg;
       static ScoresImg()
       {
           SolidBrush br = new SolidBrush(System.Drawing.SystemColors.ControlText);
           br.Color = Color.Red;
           float currentSize = 20;
           Font font = new Font("宋体", currentSize,
               FontStyle.Bold, GraphicsUnit.Pixel);

           StringFormat sf = new StringFormat();
           sf.Alignment = StringAlignment.Center;

           scoresimg = new Bitmap(30 * 20, 30, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
           ARDection.Fill(scoresimg, 128);
           Graphics g = Graphics.FromImage(scoresimg);
           for (int i = 0; i < 20; i++)
           {
               Rectangle centered = new Rectangle(i * 30, 0, 30, 30);
               Rectangle r = new Rectangle(i * 30, 0, 30, 30);
               centered.Offset(0, (int)(centered.Height - g.MeasureString(i.ToString(), font).Height) / 2);
               g.DrawString(i.ToString(), font, br, centered, sf);
               ARDection.Fill(scoresimg, r, 128 + i, 1);
           }
           g.Dispose();
           ARDection.Fill(scoresimg, 128, 0);
       }

       public static Bitmap GetScoreImg()
       {
           return new Bitmap(0, 0);
       }
       public static void DrawBitmap(Bitmap bitmap, int score, Point startpoint, int drawmaxheight,ref Rectangle ostr)
       {
           int hcnt = 1;
           int wcnt = GetWcnt(score, drawmaxheight, ref hcnt);
           //draw
           Graphics g = Graphics.FromImage(bitmap);
           Point p = startpoint;
           int len = wcnt;
           ostr = new Rectangle(p.X, p.Y, wcnt * 30, hcnt * 30);
           for (int i = 0; i < hcnt; i++)  ///2分以内
           {
               p.Y += i * 30;
               int srcx = i * 30 * wcnt;
               if (i == hcnt - 1)
               {
                   if ((score + 1) % wcnt != 0)
                       wcnt = (score + 1) % wcnt;
               }
               Rectangle dstr = new Rectangle(p.X, p.Y, wcnt * 30, 30);
               Rectangle srcr = new Rectangle(srcx, 0, wcnt * 30, 30);
               g.DrawImage(scoresimg, dstr, srcr, GraphicsUnit.Pixel);             
           }
           g.Dispose();          
       }

       public static int GetWcnt(int score, int drawmaxheight, ref int hcnt)
       {
           int wcnt = 1;
           if (score == 1 || score == 2)
           {
               wcnt = score + 1;
           }
           else if (score > 2 && score < 6)
           {
               wcnt = score + 1;
               if (drawmaxheight / 30.0 > 2)
               {
                   hcnt = 2;
                   wcnt = (score + 1) / 2;
               }
           }
           else if (score >= 6)
           {
               hcnt = drawmaxheight / 30;
               wcnt = (score + wcnt + 1) / hcnt;
           }
           return wcnt;
       }
       public static void SetScorelocation(List<Point> scorelocation, int drawmaxheight, int score)
       {
           int hcnt = 1;
           int wcnt = GetWcnt(score, drawmaxheight, ref hcnt);
           scorelocation.Clear();
           Point p = new Point(0, 0);
           int len = wcnt;
           for (int i = 0; i < score + 1; i++)  ///2分以内
           {
               int x = i % wcnt * 30;
               int y = i / wcnt * 30;
               scorelocation.Add(new Point(p.X + x, p.Y + y));
           }
       }
   }
   public class ARDection
   {
       public ARDection()
       {
       }
       ~ARDection()
       {
       }
       //public void cleardata()
       //{
       //    this._src = null;
       //    this._gdt = null;
       //    _readrect = new Rectangle();
       //    _xuehaorect = new Rectangle();
       //    _xuanzhetirect = new Rectangle();
       //    _tiankongtirect = new Rectangle();
       //    //_selectvalue = null;
       //    //xuehao = 0;
       //    //result = 0;
       //    //infor = "";
       //}
       //private Bitmap _src;
       //private Bitmap _gdt;
       //private Rectangle _photorect;
       //private Rectangle _readrect; //ValidImg
       //private Rectangle _xuehaorect;
       //private Rectangle _xuanzhetirect;
       //private Rectangle _tiankongtirect;
       //private int[] _selectvalue;
       //private string infor;
       //private float result;
       //private CFGForm cfg;

       //public bool TestImg(Bitmap bmp, bool bignorexuehao)
       //{
       //    cleardata();
       //    if (_photorect.X < 5 || _photorect.Y < 5 || _photorect.Width < 300 || _photorect.Height < 100 ||
       //        _photorect.Right > 630 || _photorect.Bottom > 460) { infor = "Wrong Photorect"; return false; }
       //    this._src = bmp;
       //    this._gdt = (Bitmap)bmp.Clone();
       //    Fill(_gdt, 0);
       //    Gray(_src);
       //    if (!ComputeReadRect()) { infor = "Rect Wrong"; return false; }

       //    if (!ValidXuehaoLine()) { infor = "XuehaoLine Wrong"; return false; }


       //    if (!ComputeXuehao()) { infor = "Xuehao Wrong"; if (!bignorexuehao) return false; }
       //    // if validxuanzhetir
       //    ComputXuanzheti();

       //    Answer[] answervalue = cfg.Answer;
       //    float score = 0;
       //    int ops = answervalue.Length < _selectvalue.Length ? answervalue.Length : _selectvalue.Length;
       //    for (int i = 0; i < ops; i++)
       //    {
       //        if (_selectvalue[i] == answervalue[i].option)
       //        {
       //            score += answervalue[i].score;
       //        }
       //    }
       //    this.result = score;
       //    return true;
       //}

       //private bool ComputeReadRect()
       //{
       //    int midx = (_photorect.Left + _photorect.Right) / 2;
       //    int midy = (_photorect.Top + _photorect.Bottom) / 2;
       //    Rectangle xr = new Rectangle(_photorect.Left, midy - 50, _photorect.Width, 100);
       //    Rectangle yr = new Rectangle(midx - 50, _photorect.Top, 100, _photorect.Height);
       //    int[] xcnt, ycnt;
       //    List<int> xlinepos, ylinepos;
       //    CountXPixsum(_src, xr, out xcnt);
       //    CountYPixsum(_src, yr, out ycnt);
       //    ComputeLinepos(xcnt, out xlinepos);
       //    ComputeLinepos(ycnt, out ylinepos);

       //    for (int i = 0; i < xlinepos.Count; i++)
       //        xlinepos[i] = GetMinPos(xlinepos[i], xcnt);
       //    for (int i = 0; i < ylinepos.Count; i++)
       //        ylinepos[i] = GetMinPos(ylinepos[i], ycnt);
       //    if (xlinepos.Count != 0 && ylinepos.Count != 0)
       //    {
       //        _readrect.X = xlinepos[0];
       //        _readrect.Y = ylinepos[0];
       //        _readrect.Width = xlinepos[xlinepos.Count - 1] - _readrect.X;
       //        _readrect.Height = ylinepos[ylinepos.Count - 1] - _readrect.Y;
       //    }
       //    if (_readrect.Left > 10 && _readrect.Top > 10 && _readrect.Right < (_photorect.Right - 5) && _readrect.Bottom < (_photorect.Bottom - 5) &&
       //               _readrect.Width > (0.7 * _photorect.Width) && _readrect.Height > 100)
       //    {
       //        ComputeDetailRect(xlinepos, ylinepos);
       //        _xuehaorect.Offset(_photorect.Location);
       //        _xuanzhetirect.Offset(_photorect.Location);
       //        _tiankongtirect.Offset(_photorect.Location);
       //        return true;
       //    }
       //    return false;
       //}
       //private void ComputeDetailRect(List<int> xlinepos, List<int> ylinepos)
       //{
       //    _xuehaorect = new Rectangle();
       //    _xuanzhetirect = new Rectangle();
       //    _tiankongtirect = new Rectangle();
       //    if (xlinepos.Count == 0 || ylinepos.Count == 0) return;
       //    int[] xlinedif, ylinedif;
       //    xlinedif = new int[xlinepos.Count];
       //    ylinedif = new int[ylinepos.Count];
       //    for (int i = 1; i < xlinepos.Count; i++)
       //        xlinedif[i] = xlinepos[i] - xlinepos[i - 1];  //xlinedif[0] = 0
       //    for (int i = 1; i < ylinepos.Count; i++)
       //        ylinedif[i] = ylinepos[i] - ylinepos[i - 1];   //ylinedif[0]
       //    if (xlinedif[1] >= 5 && xlinedif[1] <= 11 && xlinedif[xlinedif.Length - 1] >= 5 && xlinedif[xlinedif.Length - 1] <= 11 &&
       //        ylinedif[1] >= 5 && ylinedif[1] <= 11 && ylinedif[ylinedif.Length - 1] >= 5 && ylinedif[ylinedif.Length - 1] <= 11)
       //    {
       //        var query = from nd in ylinedif where nd > 4 && nd < 12 select nd;
       //        int n = query.Count() - 2;
       //        if (n == 1)
       //        {
       //            for (int i = 2; i < ylinedif.Length - 1; i++)
       //            {
       //                if (ylinedif[i] > 4 && ylinedif[i] < 12)
       //                {
       //                    _xuehaorect = new Rectangle(xlinepos[1], ylinepos[1],
       //                        xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[i - 1] - ylinepos[1]);
       //                    _xuanzhetirect = new Rectangle(xlinepos[1], ylinepos[i],
       //                        xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[ylinepos.Count - 2] - ylinepos[i]);
       //                    break;
       //                }
       //            }
       //        }
       //        else if (n == 2)
       //        {
       //            int nagain = 0, xzti = 0;
       //            for (int i = 2; i < ylinedif.Length - 1; i++)
       //            {
       //                if (ylinedif[i] > 4 && ylinedif[i] < 9)
       //                {
       //                    nagain++;
       //                    if (nagain == 1)
       //                    {
       //                        _xuehaorect = new Rectangle(xlinepos[1], ylinepos[1],
       //                           xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[i - 1] - ylinepos[1]);
       //                        xzti = i;
       //                    }
       //                    if (nagain == 2)
       //                    {
       //                        _xuanzhetirect = new Rectangle(xlinepos[1], ylinepos[xzti],
       //                            xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[i - 1] - ylinepos[xzti]);
       //                        _tiankongtirect = new Rectangle(xlinepos[1], ylinepos[i],
       //                            xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[ylinepos.Count - 2 - 1] - ylinepos[i]);
       //                        break;
       //                    }
       //                }
       //            }
       //        }
       //    }
       //}
       //private bool ValidXuehaoLine()
       //{
       //    Rectangle xhgdtr = cfg.GetGdtRectangle(_xuehaorect, "xuehao");
       //    Rectangle xhgdtbaker = xhgdtr;
       //    //gdt.extend(5);

       //    xhgdtr.X -= 5;
       //    xhgdtr.Y -= 5;
       //    xhgdtr.Width += 10;
       //    xhgdtr.Height += 10;
       //    Gdt(xhgdtr, 2, 1, 25);
       //    int[] xuehaoxv = CountImgXBlackCnt(_gdt, xhgdtr);
       //    List<int> xuehaolinepos = new List<int>();
       //    xuehaoxv[xuehaoxv.Length - 1] = 0;
       //    xuehaoxv[xuehaoxv.Length - 2] = 0;

       //    var queryResults = from n in xuehaoxv select n;
       //    int avg = (int)queryResults.Average() + 4;
       //    for (int i = 2; i < xuehaoxv.Length - 3; i++)
       //    {
       //        if (xuehaoxv[i] > 2 * avg)
       //        {
       //            if (xuehaoxv[i + 3] > 2 * avg)
       //            {
       //                if (i + 15 < xuehaoxv.Length)
       //                {
       //                    queryResults = from n in xuehaoxv select n;
       //                    int b2avg = queryResults.Count();
       //                    if (b2avg > 8)
       //                    {
       //                        i += b2avg;
       //                        continue;
       //                    }
       //                }
       //                else
       //                {
       //                    //std::printf(" [i+3]>a*avg "); 
       //                    return false;
       //                }
       //            }
       //            int maxpos = GetMaxPos(xuehaoxv[i], xuehaoxv[i + 1], xuehaoxv[i + 2]);
       //            if (xuehaolinepos.Count == 0 && maxpos + i > 20)
       //                xuehaolinepos.Add(5);
       //            xuehaolinepos.Add(maxpos + i);
       //            i += 2;
       //        }
       //    }
       //    if (xuehaolinepos.Count < 7 || xuehaolinepos.Count > 11)
       //    {
       //        return false;
       //    }
       //    bool[] lineflag = new bool[11];
       //    int[] linedif = new int[11];
       //    double len = (xhgdtr.Width - 10) / 10.0;
       //    int linecnt = 0;
       //    for (int i = 0; i < xuehaolinepos.Count; i++)
       //    {
       //        int pos = (int)((xuehaolinepos[i] - 5 + 2) / len);
       //        int dif = (int)(xuehaolinepos[i] - pos * len - 5);
       //        linedif[pos] = dif;
       //        if (dif == -1 || dif == 0 || dif == 1)
       //        {
       //            if (lineflag[pos] == false)
       //            {
       //                lineflag[pos] = true;
       //                linecnt++;
       //            }
       //            else
       //                return false;
       //        }
       //    }
       //    if (linecnt < 6) { return false; }
       //    return true;
       //}
       //private bool ComputeXuehao()
       //{
       //    //Rectangle gdtr = cfg.GetGdtRectangle(_xuehaorect, "xuehao");
       //    //int[,] blackcnt = new int[10, 4];
       //    //CountGridOption(_gdt, gdtr, blackcnt);
       //    //int a = 0, b = 0, c = 0;
       //    //Rectangle ra = cfg.Xuehaonum[0], rb = cfg.Xuehaonum[1], rc = cfg.Xuehaonum[2];
       //    //ra.X = rb.X = rc.X = 0;

       //    //if (TestRightOption(blackcnt, ra, ref  a) &&
       //    //    TestRightOption(blackcnt, rb, ref  b) &&
       //    //    TestRightOption(blackcnt, rc, ref  c))
       //    //{
       //    //    xuehao = a * 100 + b * 10 + c;
       //    //    return true;
       //    //}
       //    return false;
       //}
       //private void ComputXuanzheti()
       //{
       //    //Rectangle gdtr = cfg.GetGdtRectangle(_xuanzhetirect, "xuanzheti");
       //    ////gdt.extend(5);
       //    //Rectangle xztbaker = gdtr;
       //    //gdtr.X -= 5;
       //    //gdtr.Y -= 5;
       //    //gdtr.Width += 10;
       //    //gdtr.Height += 10;
       //    ////if(!gdtr.valid()) return ;
       //    //Gdt(gdtr, 3, 1, 15);
       //    //Size g = cfg.Xuanzhetitable;
       //    //int[,] blackcnt = new int[g.Width, g.Height];
       //    //CountGridOption(_gdt, xztbaker, blackcnt);

       //    //Rectangle[] question = cfg.Xuanzhetiquestion;
       //    //_selectvalue = new int[question.Length];
       //    //for (int i = 0; i < question.Length; i++)
       //    //{
       //    //    Rectangle r = question[i];
       //    //    r.X++;
       //    //    TestRightOption(blackcnt, r, ref _selectvalue[i]);
       //    //}
       //    //Rectangle r = xztbaker;
       //    //Rectangle[] q = cfg.Xuanzhetiquestion;
       //    //if (q.Count() == 0) return;
       //    //if (q[0].Height == 1)
       //    //{               
       //    //    for (int i = 0; i < q.Length; i++)
       //    //    {
       //    //        Rectangle nr = new Rectangle(q[i].X * r.Width / g.Width + r.X,
       //    //            q[i].Y * r.Height / g.Height + r.Y,  
       //    //           10, 
       //    //           10);  // r  b
       //    //        CountGridOption( nr, blackcnt, q[i]); //可以修正 T B
       //    //    }
       //    //}
       //    //else
       //    //{
       //    //    CountGridOption(r, blackcnt);
       //    //} 
       //}
       //private void DrawOption(Bitmap bmp)
       //{
       //    //Rectangle r = cfg.GetGdtRectangle(_xuanzhetirect, "xuanzheti");
       //    //Size grid = cfg.Xuanzhetitable;
       //    //Rectangle[] question = cfg.Xuanzhetiquestion;
       //    //Answer[] answervalue = cfg.Answer;

       //    //int ops = answervalue.Length < question.Length ? answervalue.Length : question.Length;
       //    //for (int i = 0; i < ops; i++)
       //    //{
       //    //    Rectangle qr = question[i];
       //    //    if (qr.Width == 1) qr.Y += answervalue[i].option + 1;
       //    //    if (qr.Height == 1) qr.X += answervalue[i].option + 1;
       //    //    Rectangle nr = new Rectangle((int)((qr.X + 0.5) * r.Width / grid.Width + r.X) - 5,
       //    //        (int)((qr.Y + 0.5) * r.Height / grid.Height + r.Y) - 5, 10, 10);
       //    //    if (_selectvalue[i] == answervalue[i].option)
       //    //    {
       //    //        DrawRectEdge(bmp, nr, 1, Color.Red);
       //    //        nr.X -= 1; nr.Y -= 1; nr.Width += 2; nr.Height += 2;
       //    //        DrawRectEdge(bmp, nr, 1, Color.Red);
       //    //        nr.X -= 1; nr.Y -= 1; nr.Width += 2; nr.Height += 2;
       //    //        DrawRectEdge(bmp, nr, 1, Color.Red);
       //    //    }
       //    //    else
       //    //    {
       //    //        DrawRectEdge(bmp, nr, 1, Color.Yellow);
       //    //        nr.X -= 1; nr.Y -= 1; nr.Width += 2; nr.Height += 2;
       //    //        DrawRectEdge(bmp, nr, 1, Color.Yellow);
       //    //    }
       //    //}
       //}


       //private void Gdt(Rectangle r, int xnum, int ynum, int pergamma)
       //{
       //    Rectangle tempr = new Rectangle();
       //    for (int xi = 0; xi < xnum; xi++)
       //    {
       //        for (int yi = 0; yi < ynum; yi++)
       //        {
       //            tempr.X = r.Left + xi * r.Width / xnum;
       //            tempr.Y = r.Top + yi * r.Height / ynum;
       //            tempr.Width = r.Width / xnum;
       //            tempr.Height = r.Height / ynum;
       //            int gamma = 0;
       //            if (xi == 0 || xi == xnum - 1)
       //            {
       //                gamma = GetstatisticGamma(_src, pergamma, tempr);
       //            }
       //            else
       //            {
       //                gamma = GetstatisticGamma(_src, pergamma, tempr);
       //            }
       //            if (gamma >= 240)
       //                gamma -= 10;
       //            GammaImg(_gdt, _src, gamma, tempr);
       //        }
       //    }
       //}
       ////private void DrawRect(Bitmap src)
       ////{
       ////    BitmapData data = src.LockBits(new Rectangle(0, 0, src.Width, src.Height),
       ////        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
       ////    DrawRect(_xuehaorect, data, Color.Red);
       ////    DrawRect(_xuanzhetirect, data, Color.Red);
       ////    DrawRect(_tiankongtirect, data, Color.Red);
       ////    src.UnlockBits(data);
       ////}
       //private void DrawRectEdge(Bitmap bmp, Rectangle r, int border, Color c)
       //{
       //    BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
       //        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
       //    //extendR
       //    DrawRect(r, data, c);
       //    bmp.UnlockBits(data);
       //}
       public static void DrawRect(Rectangle r, BitmapData data, Color c)
       {
           if (r.X > 0 && r.Right < data.Width && r.Y > 0 && r.Bottom < data.Height
               && r.Width > 2 && r.Width < data.Width && r.Height > 2 && r.Height < data.Height)
           {
               unsafe
               {
                   byte* ptr = (byte*)(data.Scan0);
                   byte* ptrfirst = ptr;
                   //TB
                   ptr = ptrfirst + data.Stride * r.Y + r.X * 3;
                   for (int j = 0; j < r.Width; j++)
                   {
                       ptr[0] = c.B;
                       ptr[1] = c.G;
                       ptr[2] = c.R;
                       ptr += 3;
                   }
                   ptr = ptrfirst + data.Stride * r.Bottom + r.X * 3;
                   for (int j = 0; j < r.Width; j++)
                   {
                       ptr[0] = c.B;
                       ptr[1] = c.G;
                       ptr[2] = c.R;
                       ptr += 3;
                   }
                   //LR   
                   ptr = ptrfirst + data.Stride * r.Y + r.X * 3;
                   for (int i = 0; i < r.Height; i++)
                   {
                       ptr[0] = c.B;
                       ptr[1] = c.G;
                       ptr[2] = c.R;
                       ptr += data.Stride;
                   }
                   ptr = ptrfirst + data.Stride * r.Y + r.Right * 3;
                   for (int i = 0; i < r.Height; i++)
                   {
                       ptr[0] = c.B;
                       ptr[1] = c.G;
                       ptr[2] = c.R;
                       ptr += data.Stride;
                   }
               }
           }
       }
       public static void Gray(Bitmap bmp)
       {
           BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           //循环处理 
           unsafe
           {
               byte temp = 0;
               byte* ptr = (byte*)(data.Scan0);
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j < data.Width; j++)
                   {
                       // write the logic implementation here 
                       temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);
                       ptr[0] = ptr[1] = ptr[2] = temp;
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
       }
       public static void Fill(Bitmap bmp, int color)
       {
           BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           //循环处理 
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j < data.Width; j++)
                   {
                       ptr[0] = ptr[1] = ptr[2] = (byte)color;
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
       }
       public static void Fill(Bitmap bmp, int color, int p0)
       {
           if (p0 < 0 || p0 > 2)
               return;
           BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           //循环处理 
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j < data.Width; j++)
                   {
                       ptr[p0] = (byte)color;
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
       }
       public static void Fill(Bitmap bmp, int color, int p0, int p1)
       {
           if (p0 < 0 || p0 > 2 || p1 < 0 || p1 > 2)
               return;
           BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           //循环处理 
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j < data.Width; j++)
                   {
                       ptr[p0] = (byte)color;
                       ptr[p1] = (byte)color;
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
       }
       internal static void Fill(Bitmap bmp, Rectangle rect, int color, int p0)
       {
           if (p0 < 0 || p0 > 2)
               return;
           BitmapData data = bmp.LockBits(rect,
               ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           //循环处理 
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j < data.Width; j++)
                   {
                       ptr[p0] = (byte)color;
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
       }

       public static int CountRectBlackcnt(Bitmap bmp, Rectangle r)
       {
           int cnt = 0;
           BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               byte* ptr0 = ptr;
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j < data.Width; j++)
                   {
                       if (ptr[0] == 0) cnt++;
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
           return cnt;
       }
       private static int[] CountImgXBlackCnt(Bitmap bmp, Rectangle r)
       {
           int[] rv = new int[r.Width];
           BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               byte* ptr0 = ptr;
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j < data.Width; j++)
                   {
                       if (ptr[0] == 0) rv[j]++;
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
           return rv;
       }
       private static void CountXPixsum(Bitmap bmp, Rectangle r, out int[] xcnt)
       {
           xcnt = new int[r.Size.Width];
           BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j < data.Width; j++)
                   { // write the logic implementation here 
                       xcnt[j] += ptr[0];
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
           for (int i = 0; i < xcnt.Length; i++)
               xcnt[i] /= 255;
       }
       private static void CountYPixsum(Bitmap bmp, Rectangle r, out int[] ycnt)
       {
           ycnt = new int[r.Size.Height];
           BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j < data.Width; j++)
                   { // write the logic implementation here 
                       ycnt[i] += ptr[0];
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
           for (int i = 0; i < ycnt.Length; i++)
               ycnt[i] /= 255;
       }
       private static void ComputeLinepos(int[] xcnt, out List<int> linepos)
       {//从0开始
           linepos = new List<int>();
           int[] cnt;
           cnt = new int[xcnt.Length];
           //int b = photorect.Left ,e = photorect.Right ; 
           for (int i = 2; i < xcnt.Length; i++)
           {
               cnt[i] = xcnt[i] - xcnt[i - 2];
           }
           cnt[0] = cnt[1] = 0;

           int[] maxpos, minpos, flag;
           maxpos = new int[xcnt.Length / 20];
           minpos = new int[xcnt.Length / 20];
           flag = new int[xcnt.Length];
           for (int i = 0; i < maxpos.Length; i++)
           {
               maxpos[i] = Max(cnt, i * 20, 20);
               minpos[i] = Min(cnt, i * 20, 20);
           }
           var queryResults = from n in maxpos select n;
           int maxavg = (int)queryResults.Average() + 1;
           queryResults = from n in minpos select n;
           int minavg = (int)queryResults.Average() + 1;
           // if(debug&2 && option&1) std::printf("X-maxavg:%d  minavg:%d\n",maxavg,minavg);
           if (maxavg < 5) maxavg = 5;
           if (minavg > -5) minavg = -5;

           for (int x = 2; x < xcnt.Length - 1; x++)
           {
               if ((cnt[x] > maxavg || cnt[x + 1] > maxavg)
                 && (cnt[x] < -minavg || cnt[x - 1] < -minavg))
               {
                   flag[x] = 1;
                   linepos.Add(x);
                   x++;
               }
           }
       }
       private static int Min(int[] cnt, int bindex, int len)
       {
           int min = cnt[bindex];
           for (int i = 1; i < len; i++)
           {
               if (min > cnt[i + bindex]) min = cnt[i + bindex];
           }
           return min;
       }
       private static int Max(int[] cnt, int bindex, int len)
       {
           int max = cnt[bindex];
           for (int i = 1; i < len; i++)
           {
               if (max < cnt[i + bindex]) max = cnt[i + bindex];
           }
           return max;
       }
       private static int GetMaxPos(int a, int b, int c)
       {
           int max = a > b ? a : b;
           max = max > c ? max : c;
           if (a == max) return 0;
           if (b == max) return 1;
           if (c == max) return 2;
           throw new NotImplementedException();
       }
       private static int GetMinPos(int minpos, int[] cnt)
       {
           if (minpos < 3 || minpos > cnt.Length - 3) return minpos;
           int min = Min(cnt, minpos - 3, 6);
           for (int i = minpos - 3; i < minpos + 4; i++)
               if (min == cnt[i]) return i;
           return 0;
       }
       private static void CountGridOption(Bitmap gdt, Rectangle r, int[,] blackcnt)
       {
           int H = blackcnt.GetLength(1);
           int W = blackcnt.GetLength(0);
           //ShowForm s = new ShowForm();
           for (int x = 0; x < W; x++)
           {
               for (int y = 0; y < H; y++)
               {
                   int xcent = (int)((x + 0.5) * r.Width / W + r.X);
                   int ycent = (int)((y + 0.5) * r.Height / H + r.Y);
                   Rectangle nr = new Rectangle(xcent - 5, ycent - 5, 10, 10);//
                   blackcnt[x, y] = CountRectBlackcnt(gdt, nr);
                   //if ((debug & 1024) != 0)
                   //{
                   //    BitmapData data = gdt.LockBits(new Rectangle(0, 0, gdt.Width, gdt.Height),
                   //                        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                   //    DrawRect(nr, data, Color.Green);//debug
                   //    gdt.UnlockBits(data);
                   //    s.ShowImg(gdt);
                   //}
               }
           }
       }
       private static bool TestRightOption(int[,] blackcnt, Rectangle r, ref  int value)
       {
           List<int> vcnt = new List<int>();
           if (r.Width == 1)
           { // 选项向下
               for (int i = 0; i < r.Height; i++) vcnt.Add(blackcnt[r.X, i + r.Y]);
           }
           else if (r.Height == 1)
           {
               for (int i = 0; i < r.Width; i++) vcnt.Add(blackcnt[i + r.X, r.Y]);
           }
           else { value = -1; return false; }

           int max = vcnt.Max();
           int sum = vcnt.Sum();
           int cnt = vcnt.Count(n => n > 50);
           int maxpos = vcnt.FindIndex(n => n == max);

           value = maxpos;
           if (cnt == 1)
           {
               return true;
           }	//单选， 暂时不支持多选
           if (max >= 40 && max > sum - max)
           {
               return true;
           }
           else if (max >= 25 && max > 2 * (sum - max))
           {
               return true;
           }
           value = -1;
           return false;
       }
       private static int GetstatisticGamma(Bitmap bmp, int pergamma, Rectangle r)
       {
           int[] colorcnt = new int[256];
           int sum = 0;
           int alltotal = 0;
           BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               for (int y = 0; y < r.Height; y++)
               {
                   for (int x = 0; x < r.Width; x++)
                   {
                       colorcnt[ptr[0]]++;
                       alltotal++;
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);
           for (int i = 0; i < 256; i++)
           {
               sum += colorcnt[i];
               int y = (int)(sum * 1.0 / (alltotal * 1.0 / 100.0));
               if (y >= pergamma)
                   return i;
           }
           return 0;
       }
       private static void GammaImg(Bitmap gdt, Bitmap src, int gamma, Rectangle r)
       {
           BitmapData datagdt = gdt.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           BitmapData databmp = src.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           unsafe
           {
               byte* ptr = (byte*)(databmp.Scan0);
               byte* ptrgdt = (byte*)(datagdt.Scan0);
               for (int y = 0; y < r.Height; y++)
               {
                   for (int x = 0; x < r.Width; x++)
                   {
                       ptrgdt[0] = ptrgdt[1] = ptrgdt[2] = (byte)(ptr[0] > gamma ? 255 : 0);
                       ptr += 3;
                       ptrgdt += 3;
                   }
                   ptr += databmp.Stride - databmp.Width * 3;
                   ptrgdt += datagdt.Stride - datagdt.Width * 3;
               }
           }
           src.UnlockBits(databmp);
           gdt.UnlockBits(datagdt);
       }
   }
}
