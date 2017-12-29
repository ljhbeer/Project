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
    public class DetectImageTools
    {
        public static DetectData DetectImg(string FileName,int type = 3)
        {
            DetectData dd = new DetectData();
            System.IO.FileStream fs = new System.IO.FileStream(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap _src = (Bitmap)System.Drawing.Image.FromStream(fs);
            DetectCorrect dc = new DetectCorrect(_src);
            dd.CorrectRect = dc.CorrectRect;
            dd.ListDetectArea = dc.DectFeature(type);
            fs.Close();
            return dd;
        }

        public static DetectData DetectImg(Bitmap src, int type = 3)
        {
            DetectData dd = new DetectData();
            DetectCorrect dc = new DetectCorrect(src);
            dd.CorrectRect = dc.CorrectRect;
            dd.ListDetectArea = dc.DectFeature(type);
            return dd;
        }

        public static DetectData DetectImg(Bitmap src, Rectangle templateCorrectRect, int type)
        {
            if (type < 0 || type > 3)
                type = 3;
            Point p = DetectLT(src);
            Rectangle rect = templateCorrectRect;
            rect.Location = p;
            List<Rectangle> tbo = DetectCorrect.LtbRtbRect(rect, src.Size);
            tbo.RemoveAt(type);

            List<Rectangle> result = new List<Rectangle>();
            foreach (Rectangle r in tbo)
            {
                Bitmap t_src = src.Clone(r, src.PixelFormat);
                result.Add(  DetectImgLTRect(t_src ) );
            }
            DetectData dd = new DetectData();
            rect.Location = result[0].Location; // 一定是左上角
            dd.CorrectRect = rect ;
            dd.ListDetectArea = result;
            return dd;
        }
        private static Point DetectLT(Bitmap src)
        { //20点起始
            Rectangle r  = new Rectangle(new Point(),new Size(src.Width/3,src.Height/3));
            Bitmap _src = (Bitmap)src.Clone(r,src.PixelFormat);
            DetectCorrect dc = new DetectCorrect(_src);
            return dc.CorrectRect.Location;
        }
        private static Rectangle DetectImgLTRect(Bitmap src )
        {
            Rectangle rect = DetectImgLTRect2(src);
            if (rect.Width == 1 || rect.Height == 1)
                return new Rectangle();
            return rect;
        }
        private static Rectangle DetectImgLTRect2(Bitmap bmp)
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
        public class DetectCorrect
        {
            private Rectangle _CorrectRect;
            private List<Rectangle> _FourLtbRtbRect_Detect;
            public Rectangle CorrectRect { get { return _CorrectRect; } }
            public List<Rectangle> DectFeature(int type) //type = 0123
            {
                if (type < 0 || type > 3)
                    type = 3;
                List<Rectangle> lst = _FourLtbRtbRect_Detect;
               
                lst.RemoveAt(type);
                return lst;
            }
            
            public DetectCorrect(Bitmap src)
            {
                _CorrectRect = DetectCorrectFromImg(src);
                 _FourLtbRtbRect_Detect = LtbRtbRect(_CorrectRect,src.Size);
            }
            public static List<Rectangle> LtbRtbRect(Rectangle _CorrectRect, Size size)
            {
                List<Rectangle> FourLtbRtbRect = GetLrbRtb(_CorrectRect, 160, 160);
                List<Rectangle> FourLtbRtbRect_Little = GetLrbRtb(_CorrectRect, 40, 40);
                List<Rectangle> lst = GetFeatureDetect(FourLtbRtbRect, FourLtbRtbRect_Little);
                //intersect
                Rectangle r = new Rectangle(new Point(), size);
                for (int i = 0; i < lst.Count; i++)
                   lst[i].Intersect(r);
                return lst;
            }

            private static List<Rectangle> GetFeatureDetect(List<Rectangle> B, List<Rectangle> L)
            {
                List<Rectangle> list = new List<Rectangle>();
                for (int i = 0; i < B.Count; i++)
                {
                    int xoff =(L[i].Left+L[i].Right- B[i].Left -B[i].Right)/2;
                    int yoff =(L[i].Top+L[i].Bottom- B[i].Top  -B[i].Bottom)/2;
                    Rectangle r =B[i];
                    r.Offset(xoff,xoff);
                    list.Add(r);
                }
                return list;
            }
            private static List<Rectangle> GetLrbRtb(Rectangle r, int width,int height)
            {
                return new List<Rectangle>()
                {
                    new Rectangle(r.Left,r.Top,width,height),
                    new Rectangle(r.Left,r.Bottom-height,width,height),
                    new Rectangle(r.Right-width,r.Top ,width,height),
                    new Rectangle(r.Right-width,r.Bottom-height,width,height),
                };
            }
            private static List<Rectangle> AutoGetDetectRectAngel(Size size,  Bitmap _src)
            {
                Rectangle r = DetectCorrectFromImg(_src);
                Rectangle rect = new Rectangle(new Point(15, 15), new Size(size.Width - 30, size.Height - 30));

                Rectangle r1 = new Rectangle(r.Left - 60, r.Top - 60, 160, 160);
                Rectangle r2 = new Rectangle(r.Left - 60, r.Bottom - 100, 160, 160);
                Rectangle r3 = new Rectangle(r.Right - 100, r.Top - 60, 160, 160);
                r1.Intersect(rect);
                r2.Intersect(rect);
                r3.Intersect(rect);

                return new List<Rectangle>() { r1, r2, r3 };
                
            }
            private static Rectangle DetectCorrectFromImg(Bitmap src) // 从30位算起
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

                int xpos = xxcnt.Skip(30).ToList().FindIndex(r => r > 0) + 30;
                int xendpos = xxcnt.Length - xxcnt.Reverse().Skip(30).ToList().FindIndex(r => r > 0) - 30;
                int ypos = yycnt.Skip(30).ToList().FindIndex(r => r > 0) + 30;
                int yendpos = yycnt.Length - yycnt.Reverse().Skip(30).ToList().FindIndex(r => r > 0) - 30;
                //OutPixImage(xxcnt, yycnt);
                return new Rectangle(xpos, ypos, xendpos - xpos, yendpos - ypos);
            }
            private void OutPixImage(int[] xxcnt, int[] yycnt)
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
            private void WhiteImage(Bitmap _Src)
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
        public Rectangle CorrectRect { get; set; }
        public List<Rectangle> ListDetectArea { get; set; }
    }
    public class AutoDetectRectAnge
    {
        public static List<subject> GetSubjects(List<Rectangle> list, int sizewidth = 30, int sizeheight = 30)
        {
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
    }
    public class AutoTBO  //确保 
    {
        public static List<Rectangle> GetAutoTBORect(List<Rectangle> list)
        {
            if (list.Count < 2)
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
        public static List<Point> GetAutoTBO(List<Rectangle> list)
        {
            if (list.Count < 2)
                throw new NotImplementedException();
            return ComputTBO(list.Select(r => r.Location).ToList());
        }
        public static List<Point> GetAutoTBO(List<Point> list)
        {
            if (list.Count < 2)
                throw new NotImplementedException();
            return ComputTBO(list);
        }
        private static List<Point> ComputTBO(List<Point> listrect)
        {
            Point Top, Bottom, Other;
            Point[] _P = new Point[4];
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
            if (_P[0] == null && _P[2] == null || _P[1] == null && _P[3] == null)
                return null;
            if (_P[0] == null || _P[1] == null)
            {
                Top = _P[2];
                Bottom = _P[3];
                if (_P[0] == null)
                    Other = _P[1];
                else
                    Other = _P[0];
            }
            else
            {
                Top = _P[0];
                Bottom = _P[1];
                if (_P[2] == null)
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
