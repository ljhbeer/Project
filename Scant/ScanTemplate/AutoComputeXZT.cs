using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARTemplate;
using System.Drawing.Imaging;
using System.Drawing;
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
        }
        public string ComputeXZT(string num1, AutoAngle _angle)
        {
            StringBuilder sb = new StringBuilder();
            foreach(SingleChoiceArea sca in _artemplate.Manageareas.SinglechoiceAreas.list)
            {
                Rectangle r = sca.Rect;
                Point nL = _angle.GetCorrectPoint(r.X, r.Y);
                //((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\img\\"+num+"_beforeoffset.jpg");
                r.Location = nL;
                //((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\img\\" + num + "_offset2.jpg");            	
                Bitmap bmp = (Bitmap)_src.Clone(r, _src.PixelFormat);
                //BitmapData bmpdata = _src.LockBits(r,ImageLockMode.ReadOnly,_src.PixelFormat);
                //暂不采用该方法
            	Rectangle rp = new Rectangle(0,0,sca.Size.Width,sca.Size.Height);
                int validblackcnt = rp.Width * rp.Height * 8 / 20;
            	foreach(List<Point> lp in sca.list){
            		List<int> blackpixs = new List<int>();
            		foreach(Point p in lp){
            			rp.Location = p;
                        int cnt = Tools.BitmapTools.CountRectBlackcnt(bmp, rp);
            			blackpixs.Add(cnt);
            		}
            		sb.Append( GetOptions(blackpixs,validblackcnt)+"|");
            	}
                //_src.UnlockBits(bmpdata);
            }
            return sb.ToString();
        }
        public string ComputeCustomDF(CustomArea sca, AutoAngle _angle ) // 改用接口 //KH
        {
            StringBuilder sb = new StringBuilder();
            {
                Rectangle r = sca.Rect;
                Point nL = _angle.GetCorrectPoint(r.X, r.Y);
                //((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\out\\" + 11 + "_beforeoffset.jpg");
                r.Location = nL;
                //((Bitmap)_src.Clone(r, _src.PixelFormat)).Save("f:\\out\\" + 22 + "_offset2.jpg");            	
                Bitmap bmp = (Bitmap)_src.Clone(r, _src.PixelFormat);
                //BitmapData bmpdata = _src.LockBits(r,ImageLockMode.ReadOnly,_src.PixelFormat);
                //暂不采用该方法
                Rectangle rp = new Rectangle(0, 0, sca.Size.Width, sca.Size.Height);
                int validblackcnt = rp.Width * rp.Height * 8 / 20;
                foreach (List<Point> lp in sca.list)
                {
                    List<int> blackpixs = new List<int>();
                    foreach (Point p in lp)
                    {
                        rp.Location = p;
                        int cnt = Tools.BitmapTools.CountRectBlackcnt(bmp, rp);
                        blackpixs.Add(cnt);
                    }
                    sb.Append(GetKHOptions(blackpixs, validblackcnt));
                }
                //_src.UnlockBits(bmpdata);
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
                //BitmapData bmpdata = _src.LockBits(r,ImageLockMode.ReadOnly,_src.PixelFormat);
                //暂不采用该方法
                Rectangle rp = new Rectangle(0, 0, sca.Size.Width, sca.Size.Height);
                int validblackcnt = rp.Width * rp.Height * 8 / 20;
                foreach (List<Point> lp in sca.list)
                {
                    List<int> blackpixs = new List<int>();
                    foreach (Point p in lp)
                    {
                        rp.Location = p;
                        int cnt = Tools.BitmapTools.CountRectBlackcnt(bmp, rp);
                        blackpixs.Add(cnt);
                    }
                    sb.Append(GetKHOptions(blackpixs, validblackcnt));
                }
                //_src.UnlockBits(bmpdata);
            }
            return sb.ToString();
        }
        public string GetOptions(List<int> blackpixs,int validblackcnt){

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < blackpixs.Count; i++)
            {
                if (blackpixs[i] > validblackcnt)
                    sb.Append(Convert.ToChar(i + 'A'));
            }
            if (sb.Length > 0)
                return sb.ToString();
        	return "-";
        }
        public string GetKHOptions(List<int> blackpixs,int validblackcnt){
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < blackpixs.Count; i++)
            {
                if (blackpixs[i] > validblackcnt)
                    sb.Append(Convert.ToChar(i + '0'));
            }
            if (sb.Length > 0)
                return sb.ToString();
        	return "-";
        }
    }
}
