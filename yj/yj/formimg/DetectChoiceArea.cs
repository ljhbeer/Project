using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace yj.formimg
{
    // X 水平  Y垂直
    class DetectChoiceArea
    {
        public DetectChoiceArea ( )
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
        public DetectChoiceArea(System.Drawing.Bitmap bitmap, int count)
        {
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
        
        public static int[] bitcount;
        public static Rectangle DectRectangle(Bitmap Hb, Bitmap Vb,ref List<int> black)
        {
            Rectangle hr = new Rectangle(0,0,Hb.Width,Hb.Height);
            Rectangle vr = new Rectangle(0,0,Vb.Width,Vb.Height);
            int[] va = CountImgYBlackCnt(Vb, vr);
            int[] ha = CountImgXBlackCnt(Hb, hr);
            int vb, ve, vn, hb, he, hn;
            vb = ve = vn = (va.Length + 1) / 2;
            hb = he = hn = (ha.Length + 1) / 2;
            vb = GetBeginEndPos(va,ref black,  true);
            ve = GetBeginEndPos(va,ref black,  false);
            hb = GetBeginEndPos(ha,ref black, true);
            he = GetBeginEndPos(ha,ref black, false);
            Rectangle r = new Rectangle(hb, vb, he-hb, ve-vb);
            return r;
        }
        public static List<Rectangle> DectRectangles(Bitmap Hb, Bitmap Vb, ref List<int> black)
        {
            List<Rectangle> r = new List<Rectangle>();
            Rectangle hr = new Rectangle(0, 0, Hb.Width, Hb.Height);
            Rectangle vr = new Rectangle(0, 0, Vb.Width, Vb.Height);
            int[] va = CountImgYBlackCnt(Vb, vr);
            int[] ha = CountImgXBlackCnt(Hb, hr);
            List<int> v = GetEdgePos(va, ref black);
            List<int> h = GetEdgePos(ha, ref black);
            if (v.Count > 1 && h.Count > 1)
            {
                for (int y = 0; y < v.Count-1; y++)
                {
                    for (int x = 0; x < h.Count - 1; x++)
                    {
                        r.Add(new Rectangle(h[x], v[y], h[x + 1] - h[x], v[y + 1] - v[y]));//h[x + 1] - h[x]+1??
                    }
                }
            }
            return r;
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
        public static void DrawLine(Rectangle r, BitmapData data,Color c)
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
        public static int[] CountImgXBlackCnt(Bitmap bmp, Rectangle r)
        {
            int[] rv = new int[r.Size.Width];
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, bmp.PixelFormat);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);

                if (bmp.PixelFormat == PixelFormat.Format1bppIndexed)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width / 8; j++)
                        {
                            rv[j] += bitcount[ptr[0]];
                            ptr++;
                        }
                        byte flag = 128;
                        for (int j = 0; j < data.Width % 8; j++)
                        {
                            if ((*ptr & flag) > 0) rv[j]++;
                            flag /= 2;
                        }
                        ptr += data.Stride - (data.Width+7) / 8;
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
                            //File.AppendAllText("outx.txt", j + "\t" + ptr[0] + "\r\n");
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
                        ptr += data.Stride - data.Width*3;
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
        }  //private
        public static int[] CountImgYBlackCnt(Bitmap bmp, Rectangle r)     //private
        {
            int[] rv = new int[r.Size.Height];
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, bmp.PixelFormat);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);

                if (bmp.PixelFormat == PixelFormat.Format1bppIndexed)
                {
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
                        ptr += data.Stride - (data.Width+7) / 8;
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
                            //File.AppendAllText("outy.txt",j+"\t"+ ptr[0] + "\r\n");
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
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        {
                            cnt += ptr[0];
                            ptr += 3;
                        }
                        ptr += data.Stride - data.Width * 3;
                    }
                }
                else if (bmp.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        {
                            cnt += ptr[1];
                            ptr += 4;
                        }
                        ptr += data.Stride - data.Width * 4;
                    }
                }
            }
            bmp.UnlockBits(data);
            return r.Width*r.Height - cnt;
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
        private static int GetBeginEndPos(int[] va,ref List<int> black, bool begin)
        {
            int vnow = (va.Length + 1) / 2;
            int i=0;
            if(begin)
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
                        ptr += data.Stride - (data.Width + 7) / 8;
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
        
        private bool RemoveAndCheck(List<int> xposlen, int cnt)
        {
            if (xposlen.Count < cnt * 2) return false;
            if (xposlen.Count > cnt * 2)
            {
                int sum = 0;
                for (int i = 1; i < xposlen.Count; i += 2)
                {
                    sum += xposlen[i] * xposlen[i];
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
                int avg = (int)(sum * 2 / xposlen.Count);
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
            List<int> npos = new List<int>();
            npos.Add(pos[0]);
            npos.Add(pos[1]);
            for (int i = 2; i < pos.Count - 1; i += 2)
            {
                int gap = pos[i] - npos[npos.Count - 2] - npos[npos.Count - 1];
                if (gap < 3 || gap < 5 && npos[npos.Count - 1] + pos[i + 1] + gap > 10) //|| gap < 6 && npos[npos.Count - 1] + pos[i + 1] +gap>15
                {
                    npos[npos.Count - 1] += pos[i + 1] + gap;
                }
                else
                {
                    npos.Add(pos[i]);
                    npos.Add(pos[i + 1]);
                }
            }
            pos.Clear();
            pos.AddRange(npos);
        }
        private System.Drawing.Bitmap _src;
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
        private int choicecount;
        private Size choicesize;
        private List<List<Point>> m_choicepoint;
    }
}
