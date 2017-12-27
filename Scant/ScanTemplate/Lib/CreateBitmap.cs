using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace FormTest
{
    public  class CreateBitmap
    {
        public  Bitmap CreateBitmap()
        {
            List<string> list = GetListNameScore();
            Bitmap bmp = new Bitmap(960, 720);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), bmp.Size));
                Font font = SystemFonts.DefaultFont;
                Font font2 = new Font(SystemFonts.DefaultFont.SystemFontName, 16, FontStyle.Bold);
                Point Loc = new Point(50, 40);
                Size sizepaper = new Size(860, 640);
                DrawListString(list, g, sizepaper, Loc);
            }
            return bmp;
        }
        private Size DrawListString(List<string> list, Graphics g, Size sizepaper, Point Loc)
        {
            Size Scnt = GetCountSize(sizepaper, list.Count, list[0]);
            SizeF Sitem = GetItemSize(sizepaper, Scnt);
            double rat = sizepaper.Width / Sitem.Width < sizepaper.Height / Sitem.Height ? sizepaper.Width / Sitem.Width : sizepaper.Height / Sitem.Height;

            //改变字号大小
            int pix = 25; // (int)(25 * rat);
            Font font1 = new Font(SystemFonts.DefaultFont.SystemFontName, pix, FontStyle.Bold);
            list.Reverse();
            Stack<string> stack = new Stack<string>(list);
            string S = "";
            PointF l = Loc;
            for (int x = 0; x < Scnt.Width; x++)
            {
                for (int y = 0; y < Scnt.Height; y++)
                {
                    if (stack.Count == 0)
                        break;
                    S = stack.Pop();
                    l.X = Loc.X + x * Sitem.Width;
                    l.Y = Loc.Y + y * Sitem.Height;
                    g.DrawString(S, font1, Brushes.Red, l);
                }
                if (stack.Count == 0)
                    break;
            }
            return sizepaper;
        }
        private List<string> GetListNameScore()
        {
            List<string> list = new List<string>();
            string[] ss = File.ReadAllLines("NewTestScanT.txt");
            foreach (string s in ss)
            {
                string[] item = s.Split(',');
                list.Add(item[2] + " " + item[5]);
            }
            return list;
        }
        private Size GetCountSize(Size sizepaper, int Count, string txtitem)
        {
            Font font1 = new Font(SystemFonts.DefaultFont.SystemFontName, 25, FontStyle.Bold);
            SizeF size;
            using (Graphics g = this.CreateGraphics())
            {
                size = g.MeasureString(txtitem, font1);
            }
            double Smax = sizepaper.Width * sizepaper.Height;
            Double SItem = Smax / Count;
            float rat = (float)Math.Sqrt(SItem / (size.Height * size.Width * 1.2 * 1.2));
            size.Width *= rat;
            size.Height *= rat;
            int W = (int)(sizepaper.Width / size.Width);
            int H = (int)(sizepaper.Height / size.Height);
            while (W * H < Count)
                H++;
            return new Size(W, H);
        }
        private SizeF GetItemSize(Size sizepaper, Size sizecount)
        {
            return new SizeF(sizepaper.Width / sizecount.Width, sizepaper.Height / sizecount.Height);
        }
    }
}
