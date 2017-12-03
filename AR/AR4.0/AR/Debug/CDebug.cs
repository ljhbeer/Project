using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace AR
{
    public class CDebug
    {
        private static int debug = 0;
        public static void Debug1(Image image, List<int> ret, Point offset, SingleChoiceArea msc)
        {
            if (debug == 0) return;
            FormShowImg f3 = new FormShowImg();
            Bitmap bmp = (Bitmap)image;
            Rectangle r = msc.imgselection;
            r.Offset(offset);
            if (msc.list.Count != ret.Count)
            {
                return;
            }
            Image cropimg;
            if (IsPixelFormatIndexed(bmp.PixelFormat))
            {
                cropimg = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(cropimg))
                {
                    Rectangle dstr = new Rectangle(0, 0, r.Width, r.Height);
                    g.DrawImage(bmp, dstr, r, GraphicsUnit.Pixel);
                }
                //f3.SetImg(cropimg);
                //f3.ShowDialog();
            }
            else
                cropimg = bmp.Clone(r, bmp.PixelFormat);

            using (Graphics g = Graphics.FromImage(cropimg))
            {
                Brush br = Brushes.Red;
                for (int i = 0; i < ret.Count; i++)
                {
                    if (ret[i] >= 0)
                        g.FillRectangle(br,
                          new Rectangle(msc.list[i][ret[i]], new Size(40, 10)));
                    else
                        g.FillRectangle(br,
                          new Rectangle(msc.list[i][-ret[i]], new Size(80, 10)));

                }
            }
            f3.SetImg(cropimg);
            f3.ShowDialog();
        }
        public static void DebugEndMessageBox(string msg)
        {
            if (debug == 0) return;
            MessageBox.Show(msg);
        }
        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }
        private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare, PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed, PixelFormat.Format8bppIndexed };
    }
}
