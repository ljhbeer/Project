using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARTemplate;
using System.Drawing.Imaging;
using System.Drawing;
using Tools;
using System.IO;
namespace ScanTemplate
{
    class AutoComputeXZTKH
    {
        private ARTemplate.Template _artemplate;
        private System.Drawing.Bitmap _src;

        public AutoComputeXZTKH(ARTemplate.Template _artemplate, System.Drawing.Bitmap bmp)
        {            
            this._artemplate = _artemplate;
            //this._angle = _angle;
            this._src = bmp;
            if (global.Debug && (global.tag & 16) > 0)
            {
                if (!Directory.Exists("F:\\img"))
                {
                    Directory.CreateDirectory("F:\\img");
                }
            }
        }
        public string ComputeXZT(string num1, AutoAngle _angle)
        {
            StringBuilder sb = new StringBuilder();
            int num = 1;
            foreach(SingleChoiceArea sca in _artemplate.Manageareas.SinglechoiceAreas.list)
            {
                Rectangle r = sca.Rect;
                Point nL = _angle.GetCorrectPoint(r.X, r.Y);

                if (global.Debug && (global.tag & 16) > 0)
                    ((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\img\\" + (num++) + "_beforeoffset.jpg");
                r.Location = nL;

                if (global.Debug && (global.tag & 16) > 0)
                     ((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\img\\" + num + "_offset2.jpg");            	
                Bitmap bmp = (Bitmap)_src.Clone(r, _src.PixelFormat);
                ComputeSelectedOption(sb, sca, bmp);
            }
            return sb.ToString();
        }public string ComputeCustomDF(CustomArea sca, AutoAngle _angle ) // 改用接口 //KH
        {
            StringBuilder sb = new StringBuilder();
            {
                Rectangle r = sca.Rect;
                Point nL = _angle.GetCorrectPoint(r.X, r.Y);
                //((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\out\\" + 11 + "_beforeoffset.jpg");
                r.Location = nL;
                //((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\out\\" + 22 + "_offset2.jpg");            	
                Bitmap bmp = (Bitmap)_src.Clone(r, _src.PixelFormat);
                ComputeSelectedOption(sb, sca, bmp,false);               
            }
            return sb.ToString();
        }
        public string ComputeKH(KaoHaoChoiceArea sca, AutoAngle _angle)  //只支持横向
        {
            StringBuilder sb = new StringBuilder();
            {
                Rectangle r = sca.Rect;
                Point nL = _angle.GetCorrectPoint(r.X, r.Y);
                //((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\out\\" + 11 + "_beforeoffset.jpg");
                r.Location = nL;
                //((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\out\\" + 22 + "_offset2.jpg");            	
                Bitmap bmp = (Bitmap)_src.Clone(r, _src.PixelFormat);
                ComputeSelectedOption(sb, sca, bmp,false);                
            }
            sb.Replace("|", "");
            return sb.ToString();
        }
        private void ComputeSelectedOption(StringBuilder sb, ListArea sca, Bitmap bmp,bool ABCDMode = true) //ABCDMode = false means NumberMode
        {
            Rectangle rp = new Rectangle(0, 0, sca.ItemSize.Width, sca.ItemSize.Height);
            int validblackcnt = rp.Width * rp.Height * 14 / 20;
            foreach (List<Point> lp in sca.list)
            {
                List<int> blackpixs = new List<int>();
                foreach (Point p in lp)
                {
                    rp.Location = p;
                    int cnt = Tools.BitmapTools.CountRectBlackcnt(bmp, rp);
                    blackpixs.Add(cnt);
                }
                sb.Append(GetOptions(blackpixs, validblackcnt,ABCDMode ) + "|");
            }
        }
        private string GetOptions(List<int> blackpixs, int validblackcnt, bool ABCDMode = true)
        {
            char startchar = 'A';
            if(!ABCDMode)
                startchar = '0';
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < blackpixs.Count; i++)
            {
                if (blackpixs[i] > validblackcnt)
                    sb.Append(Convert.ToChar(i + startchar));

            }
            if (sb.Length > 0)
                return sb.ToString();
            int max = blackpixs.Max();
            int avg =(int) blackpixs.Average();
            int avg2 =(int)( (blackpixs.Sum() - max) * 1.5 / (blackpixs.Count - 1) );
            List<int> st = blackpixs.Select(r=>r).ToList();
            st.Sort();
            if (max>avg2 && avg > st[2] && max - avg > avg - st[1])
            {
                int index = blackpixs.IndexOf(max);
                return Convert.ToChar(index + startchar).ToString();
            }
        	return "-";
        }
        
    }
}
