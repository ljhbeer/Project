using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using ScanTemplate;
using System.Text;
using System.IO;

namespace Tools
{
    public class BitmapTools
    {
        public BitmapTools()
        {
            bitcount = null;
            InitBitCount();
        }
        private static void InitBitCount()
        {
            bitcount = new int[256];
            for (int i = 0; i < 256; i++)
            {
                int flag = 1;
                for (int j = 0; j < 8; j++)
                {
                    if ((i & flag) > 0) bitcount[i]++;
                    flag *= 2;
                }
            }
        }
        public static void DrawBitmap(Bitmap bmp, List<double> ListItem, double max)
        {
            int rw = bmp.Width / ListItem.Count;
            int W = bmp.Width - 10;
            int H = bmp.Height - 4;
            Pen pr = Pens.Red;
            Brush br = Brushes.Cyan;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                double a = H * 1.0 / max;
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, bmp.Width, bmp.Height));
                for (int i = 0; i < ListItem.Count; i++)
                {
                    int y = (int)(a * ListItem[i]);
                    y = y > 0 ? y : -y;
                    int x = i * W / ListItem.Count + 5;
                    Rectangle r = new Rectangle(x, H + 2 - y, rw, y);
                    if (ListItem[i] > 0)
                        g.DrawRectangle(pr, r);
                    else
                        g.FillRectangle(br, r);
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
        public static void DrawRect(Rectangle r, BitmapData data, Color c)
        {
            if (r.X >= 0 && r.Right < data.Width && r.Y >= 0 && r.Bottom < data.Height
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
        public static void DrawLine(Rectangle r, BitmapData data, Color c)
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
        public static int[] CountImgXBlackCnt(Bitmap bmp, Rectangle r)
        {
            int[] rv = new int[r.Size.Width];
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, bmp.PixelFormat);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                byte* ptr1 = ptr;
                if (bmp.PixelFormat == PixelFormat.Format1bppIndexed)//OK
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        ptr = ptr1;
                        for (int j = 0; j < data.Width; )
                        {
                            Byte flag = 0x80;
                            for (int k = 0; k < 8 && j < data.Width; k++, j++)
                            {
                                if ((*ptr & flag) > 0) rv[j]++;
                                flag /= 2;
                            }
                            ptr++;
                        }
                        ptr1 += data.Stride;
                    }
                }
                else if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        { // write the logic implementation here 
                            rv[j] += ptr[0];
                            //ptr += 3;
                            //MFile.AppendAllText("outx.txt", j + "\t" + ptr[0] + "\r\n");
                            ptr++;
                        }
                        ptr += data.Stride - data.Width;
                    }

                }
                else if (bmp.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        { // write the logic implementation here 
                            rv[j] += ptr[0];
                            ptr += 3;
                        }
                        ptr += data.Stride - data.Width * 3;
                    }
                    for (int i = 0; i < rv.Length; i++)
                        rv[i] /= 255;
                }
                else if (bmp.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        { // write the logic implementation here 
                            rv[j] += ptr[0] + ptr[1] + ptr[2];
                            ptr += 4;
                        }
                        ptr += data.Stride - data.Width * 4;
                    }
                    for (int i = 0; i < rv.Length; i++)
                        rv[i] /= 255;
                }
            }
            bmp.UnlockBits(data);
            return rv;
        }
        public static int[] CountImgYBlackCnt(Bitmap bmp, Rectangle r)
        {
            int[] rv = new int[r.Size.Height];
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, bmp.PixelFormat);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);

                if (bmp.PixelFormat == PixelFormat.Format1bppIndexed)
                {
                    if (bitcount == null) InitBitCount();
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width / 8; j++)
                        {
                            rv[i] += bitcount[ptr[0]];
                            ptr++;
                        }
                        byte flag = 128;
                        for (int j = 0; j < data.Width % 8; j++)
                        {
                            if ((*ptr & flag) > 0) rv[i]++;
                            flag /= 2;
                        }
                        ptr += data.Stride - data.Width / 8;
                    }
                }
                else if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        { // write the logic implementation here 
                            rv[i] += ptr[0];
                            //ptr += 3;
                            //MFile.AppendAllText("outy.txt",j+"\t"+ ptr[0] + "\r\n");
                            ptr++;
                        }
                        ptr += data.Stride - data.Width;
                    }

                }
                else if (bmp.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        { // write the logic implementation here 
                            rv[i] += ptr[0];
                            ptr += 3;
                        }
                        ptr += data.Stride - data.Width * 3;
                    }
                    for (int i = 0; i < rv.Length; i++)
                        rv[i] /= 255;
                }
                else if (bmp.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        { // write the logic implementation here 
                            rv[i] += ptr[0] + ptr[1] + ptr[2];  // rv[i] += ptr[1];
                            ptr += 4;
                        }
                        ptr += data.Stride - data.Width * 4;
                    }
                    for (int i = 0; i < rv.Length; i++)
                        rv[i] /= 255;
                }
            }
            bmp.UnlockBits(data);
            return rv;
        }
        public static int[] CountXPixsum(Bitmap bmp, Rectangle r)
        {
            return CountImgXBlackCnt(bmp, r);
        }
        public static int[] CountYPixsum(Bitmap bmp, Rectangle r)
        {
            return CountImgYBlackCnt(bmp, r);
        }
        public static int CountRectBlackcnt(Bitmap bmp, Rectangle r)
        {
            int[] xcnt = CountImgXBlackCnt(bmp, r);
            return r.Width * r.Height - xcnt.Sum();
        }
        private static List<int> GetEdgePos(int[] va, ref List<int> black)
        {
            List<int> r = new List<int>();
            for (int i = 0; i < va.Length; i++)
            {
                //if (va[i] != 0) break;
                if (!black.Contains(va[i]))
                    r.Add(i);
            }
            return r;
        }
        private static int GetBeginEndPos(int[] va, ref List<int> black, bool begin)
        {
            int vnow = (va.Length + 1) / 2;
            int i = 0;
            if (begin)
                for (i = vnow; i > 0; i--)
                {
                    // if (va[i] != 0) break;
                    if (!black.Contains(va[i])) break;
                }
            else
                for (i = vnow; i < va.Length; i++)
                {
                    //if (va[i] != 0) break;
                    if (!black.Contains(va[i])) break;
                }
            return i;
        }
        private static void NewComputeLinepos(int[] cnt, out List<int> ylinepos)
        {
            List<int> pos = SectionCount(cnt, 2, 200, 5);
            ylinepos = new List<int>();
            List<int> lcnt = new List<int>(cnt);
            for (int i = 0; i < pos.Count; i += 2)
            {//
                int avg = (int)(lcnt.GetRange(pos[i], pos[i + 1]).Average() * 1.3);
                for (int j = pos[i]; j < pos[i] + 3; j++)
                {
                    if (lcnt[j] > avg)
                    {
                        ylinepos.Add(j);
                        break;
                    }
                }
                for (int j = pos[i] + pos[i + 1] - 3; j < pos[i] + pos[i + 1]; j++)
                {
                    if (lcnt[j] > avg)
                    {
                        ylinepos.Add(j);
                        break;
                    }
                }
            }
        }
        public static List<int> SectionCount(int[] cnt, int min, int max, int minlen)// [0: startpos  1:length]
        {
            int len = 0;
            int spos = 0;
            List<int> pos = new List<int>();
            bool flag = false;
            for (int i = 0; i < cnt.Count(); i++)
            {
                if (cnt[i] > min && cnt[i] < max)
                {
                    if (flag == false)
                    {
                        spos = i;
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
                        pos.Add(len + 1);
                        len = 0;
                    }
                }
            }
            return pos;
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
        public static int GetstatisticGamma(Bitmap bmp, int pergamma, Rectangle r)
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
        public static void GammaImg(Bitmap gdt, Bitmap src, int gamma, Rectangle r)
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

        private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare, PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed, PixelFormat.Format8bppIndexed };
        public static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }
        public static int[] bitcount;
    }    
}
