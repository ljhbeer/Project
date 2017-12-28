using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Tools
{
    public  class TextBitmapTool
    {
        public Bitmap Src { get { return _Src; } }
        public bool OK { get { return _ok; } }
        public TextBitmapTool(Rectangle paper, Rectangle Content)
        {
            if (paper.Contains(Content))
            {
                this._paper = paper;
                this._content = Content;
                _Src = new Bitmap(paper.Size.Width, paper.Size.Height);
                WhiteImage();
                this._ok = true;
            }
            else
                this._ok = false;           
        }
        public Bitmap DrawListInPaper(string filename)
        {
            if (File.Exists(filename))
            {
                List<string> list = GetListNameScore(filename);
                return DrawListInPaper(list);
            }
            return null;
        }
        public  Bitmap DrawListInPaper( List<string> list )
        {
            WhiteImage();
            using (Graphics g = Graphics.FromImage(Src))
            {
                int fontsize = ComputFontSize(list.Count,list[1], g);
                Font font = new Font(SystemFonts.DefaultFont.SystemFontName, fontsize, FontStyle.Bold);

                DrawInXY(list, g, font);
            }
            return Src;
        }

        private int ComputFontSize(int Count,string itemtxt, Graphics g)
        {
            SizeF sizetxtitem = new SizeF(1,1);
            Size Scnt = GetCountSize(g, Count,itemtxt,ref sizetxtitem);
            SizeF Sitem = GetItemSize(_content.Size, Scnt);
            double rat = Sitem.Width / sizetxtitem.Width <Sitem.Height /  sizetxtitem.Height ? Sitem.Width / sizetxtitem.Width  : Sitem.Height /  sizetxtitem.Height;

            //改变字号大小
            int fontsize =(int)(25 * rat);
            return fontsize;
        }
        private void DrawInXY(List<string> list, Graphics g, Font font)
        {
            SizeF sizetxtitem = new SizeF(1, 1);
            Size Scnt = GetCountSize(g, list.Count, list[1],ref sizetxtitem);
            SizeF Sitem = GetItemSize(_content.Size, Scnt);
            list.Reverse();
            Point Loc = _content.Location;
            Stack<string> stack = new Stack<string>(list);
            for (int x = 0; x < Scnt.Width; x++)
            {
                for (int y = 0; y < Scnt.Height; y++)
                {
                    if (stack.Count == 0)
                        break;
                    PointF pos = new PointF(Loc.X + x * Sitem.Width, Loc.Y + y * Sitem.Height);
                    g.DrawString(stack.Pop(), font, Brushes.Red, pos);
                }
                if (stack.Count == 0)
                    break;
            }
        }
        private Size GetCountSize(Graphics g, int Count, string txtitem, ref SizeF txtitemsize)
        {
            Size sizecontent = _content.Size;
            Font font1 = new Font(SystemFonts.DefaultFont.SystemFontName, 25, FontStyle.Bold);
            SizeF size = g.MeasureString(txtitem, font1);
            txtitemsize = size;
            double Smax = sizecontent.Width * sizecontent.Height;
            Double SItem = Smax / Count;
            float rat = (float)Math.Sqrt(SItem / (size.Height * size.Width * 1.2 * 1.2));
                size.Width *= rat;
                size.Height *= rat;
            int W = (int)(sizecontent.Width / size.Width);
            int H = (int)(sizecontent.Height / size.Height);
            while (W * H < Count)
                H++;
            return new Size(W, H);
        }
        private SizeF GetItemSize(Size sizepaper, Size sizecount)
        {
            return new SizeF(sizepaper.Width / sizecount.Width, sizepaper.Height / sizecount.Height);
        }
        private void WhiteImage()
        {
            if (_Src != null)
                using (Graphics g = Graphics.FromImage(_Src))
                {
                    g.FillRectangle(Brushes.White, _paper);
                }
        }
        private List<string> GetListNameScore(string filename)
        {
            List<string> list = new List<string>();
            string[] ss = File.ReadAllLines(filename);
            foreach (string s in ss)
            {
                string[] item = s.Split(',');
                list.Add(item[2] + " " + item[5]);
            }
            return list;
        }
        private Bitmap _Src;
        private Rectangle _content;
        private Rectangle _paper;
        private bool _ok;
    }
}
