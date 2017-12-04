using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;

namespace ScanTemplate
{
    public  class subject
    {
        public subject(string name, Rectangle rectangle)
        {
            this.Name = name;
            this.Rect = rectangle;
        }
        public override string ToString()
        {
            return Name;
        }
        public Rectangle Rect { get; set; }
        private string Name;

        public int BitmapdataLength { get; set; }
    }
    public class LoadBitmapData
    {
        public LoadBitmapData(string path)
        {
             _nameList = FormM.NameListFromDir(path);
             position = 0;
        }
        public int Count(bool used) //
        {
            if (!used)
                return _nameList.Count;
            return _nameList.Count - position; ;
        }

        public Bitmap GetBitmap(string kh)
        {
            Bitmap bmp =(Bitmap) Bitmap.FromFile(kh);
            return bmp.Clone(Rect, bmp.PixelFormat);
        }
        public string GetNextKh()
        {
            if (position >= 0 && position < _nameList.Count)
            {
                string re =_nameList[position];
                position++;
                return re;
            }
            return "";
        }
        public void SetBitMapRectangle(Rectangle rectangle)
        {
            this.Rect = rectangle;
        }
        public void GoHead()
        {
            if(position<0 || position>= _nameList.Count)
            position = 0;
        }
        private List<string> _nameList;
        private int position;
        private Rectangle Rect;

    }
    public class LoadBitmapDataBack
    {
        FileStream fs;
        MemoryStream ms;
        BufferedStream bs;
        private byte[] buffer;
        private int floorid;
        private string bmpdatapath;
        private List<subject> sublist;
        private subject activesj;
        private Dictionary<string, int> khindex = new Dictionary<string, int>();
        private int _activefloorid;
        private subject _activesubject;
        public LoadBitmapDataBack(string bmpdatapath, int floorid, List<subject> sublist)
        {
            this.floorid = floorid;
            this.bmpdatapath = bmpdatapath;
            this.sublist = sublist;
            this.activesj = null;
            bs = null;
            fs = null;
            Init();
            buffer = new byte[102400];
        }

        public LoadBitmapDataBack(string bmpdatapath, int _activefloorid, subject _activesubject)
        {
            // TODO: Complete member initialization
            this.bmpdatapath = bmpdatapath;
            this._activefloorid = _activefloorid;
            this._activesubject = _activesubject;
        }
        ~LoadBitmapDataBack()
        {
            Clear();
        }
        public void Clear()
        {
            if (bs != null || fs != null)
            {
                bs.Flush();
                bs.Close();
                ms.Close();
                //				fs.Flush();
                fs.Close();
                ms = null;
                bs = null;
                fs = null;
            }
        }
        private void Init()
        {
            //List<List<datainfo>> di = new List<List<datainfo>>();
            //if (Directory.Exists(bmpdatapath))
            //{
            //    string difullpath = bmpdatapath + "datainfo[fid].json".Replace("[fid]", floorid.ToString());
            //    if (File.Exists(difullpath))
            //    {
            //        string str = File.ReadAllText(difullpath);
            //        di = JsonConvert.DeserializeObject<List<List<datainfo>>>(str);
            //        khindex.Clear();
            //        for (int i = 0; i < di.Count; i++)
            //        {
            //            if (di[i].Count > 0)
            //                sublist[i].BitmapdataLength = (int)(di[i][0].count);
            //        }
            //        if (di.Count > 0)
            //            for (int i = 0; i < di[0].Count; i++)
            //            {
            //                string kh = di[0][i].kh;
            //                int index = (int)(di[0][i].startpos / di[0][i].count);
            //                khindex[kh] = index;
            //            }

            //    }
            //}
            //di.Clear();
        }
        public bool SetActiveSubject(subject activesj)
        {
            if (!sublist.Contains(activesj))
                return false;
            if (this.activesj == null || this.activesj != activesj)
            {
                if (this.activesj != null)
                {
                    if (bs != null || fs != null)
                    {
                        bs.Flush();
                        bs.Close();
                        //						fs.Flush();
                        ms.Close();
                        fs.Close();
                        ms = null;
                    }
                }
                string imgdatafilename = bmpdatapath + "roomdata_[roomid].datagz".Replace("[roomid]","");
                fs = new FileStream(imgdatafilename, FileMode.Open, FileAccess.Read);

                using (GZipStream gzs = new GZipStream(fs, CompressionMode.Decompress))
                {
                    ms = new MemoryStream();
                    byte[] bu = new byte[4096];
                    int actual = 0;
                    do
                    {
                        actual = gzs.Read(bu, 0, 4096);
                        ms.Write(bu, 0, actual);
                    } while (actual > 0);
                }
                bs = new BufferedStream(ms, 512000);
                this.activesj = activesj;
            }
            else if (this.activesj == activesj)
            {
                // donothing
            }
            return true;
        }
        public Bitmap GetBitmap(string kh)
        {
            int index = khindex[kh];
            int length = activesj.BitmapdataLength;
            Rectangle r = activesj.Rect;

            if (length > buffer.Length)
                buffer = new byte[length];
            bs.Position = index * length;
            bs.Read(buffer, 0, buffer.Length);
            Bitmap bmp = new Bitmap(r.Width, r.Height);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, r.Width, r.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            IntPtr ptr = bmpData.Scan0;
            int bytes = bmpData.Stride * bmp.Height;
            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
