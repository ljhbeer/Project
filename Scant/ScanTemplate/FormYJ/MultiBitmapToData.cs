using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using ARTemplate;


namespace ScanTemplate.FormYJ
{
    public class MultiBitmapToData
    {
        private string _workpath;
        private Template _artemplate;
        private DataTable _rundt;
        private DataTable _dtsetfxzt;
        private string _bitmapdatapath;

        public MultiBitmapToData(Template _artemplate, DataTable _rundt, DataTable _dtsetfxzt)
        {
            this._workpath = "";
            this._artemplate = _artemplate;
            this._rundt = _rundt;
            this._dtsetfxzt = _dtsetfxzt;
        }       
        public void SaveBitmapToData()
        {
            int RoomsCount = _dtsetfxzt.Rows.Count;
            List<FileStream> fs = new List<FileStream>();
            List<BufferedStream> bsf = new List<BufferedStream>();
            //List<List<datainfo>> di = new List<List<datainfo>>();
            List<long> startpos = new List<long>();
            for (int i = 0; i <RoomsCount; i++)
            {
                if (File.Exists("room_" + i + ".data"))
                    File.Delete("room_" + i + ".data");
                fs.Add( new FileStream("room_" + i + ".data", FileMode.Append, FileAccess.Write));
                startpos.Add(0);
                //di.Add(new List<datainfo>());
                bsf.Add( new BufferedStream( fs[i],102400));
            }

            foreach (string kh in KhList())
            {
                Bitmap bmp = GetBitmap(kh);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BufferedStream bs = new BufferedStream(ms))
                    {
                        int i = 0;
                        foreach(Area I in _artemplate.Dic["非选择题"])
                        {
                            Bitmap imgc = bmp.Clone(I.ImgArea, bmp.PixelFormat);
                            imgc.Save(bs, System.Drawing.Imaging.ImageFormat.Tiff);
                            bs.Flush();
                            byte[] buff = ms.ToArray();
                            bsf[i].Write(buff, 0, buff.Length);
                            //di[i].Add(new datainfo(kh, startpos[i], bs.Length));
                            startpos[i] +=  bs.Length;
                            i++;
                        }
                    }
                }
            }

            for (int i = 0; i < RoomsCount; i++)
            {
                bsf[i].Flush();
                bsf[i].Close();
                fs[i].Flush();
                fs[i].Close();
            }
            //ReadBitmap(di[0], "room_" + rooms[0].id + ".data");
            //SaveDatainfo(di,"datainfo"+activefloor.sID+".json");
        }
        public void SaveBitmapDataToData()
        {
            int RoomsCount = _dtsetfxzt.Rows.Count;
            List<FileStream> fs = new List<FileStream>();
            List<BufferedStream> bsf = new List<BufferedStream>();
            //List<List<datainfo>> di = new List<List<datainfo>>();
            List<long> startpos = new List<long>();
            for (int i = 0; i < RoomsCount; i++)
            {
                if (File.Exists(_bitmapdatapath+"roomdata_" + i + ".data"))
                    File.Delete(_bitmapdatapath+"roomdata_" + i + ".data");

                fs.Add(new FileStream(_bitmapdatapath+"roomdata_" + i + ".data", FileMode.Append, FileAccess.Write));
                startpos.Add(0);
                //di.Add(new List<datainfo>());
                bsf.Add(new BufferedStream(fs[i], 102400));
            }

            foreach (string kh in KhList())
            {
                Bitmap bmp = GetBitmap(kh);
                BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
               
                int index = 0;
                foreach (Area I in _artemplate.Dic["非选择题"])
                //for (int index = 0; index < rooms.Count; index++)
                {
                    Rectangle r = I.ImgArea;
                    r.X = (r.X / 8) * 8 + (r.X % 8 > 2 ? 8 : 0); 
                    r.Width = (r.Width / 8) * 8 + (r.Width % 8 > 2 ? 8 : 0);
                    int stride = r.Width / 8;
                        stride = (stride / 4 * 4) + (stride % 4 > 0 ? 4 : 0);
                    byte[] buff = new byte[stride * r.Height];

                    unsafe
                    {
                        byte* bmpPtr = (byte*)bmpdata.Scan0.ToPointer();
                        bmpPtr += r.Y * bmpdata.Stride + r.X / 8;

                        for (int y = 0; y < r.Height; y++)
                        {
                            for (int i = 0; i < r.Width / 8; i++)
                            {
                                buff[y * (stride) + i] = bmpPtr[i];
                            }                          
                            bmpPtr += bmpdata.Stride;
                        }
                    }

                    bsf[index].Write(buff, 0, buff.Length);
                    //di[index].Add(new datainfo(kh, startpos[index], buff.Length));
                    startpos[index] += buff.Length;
                    index++;
                }
            }

            for (int index = 0; index < RoomsCount; index++)
            {
                bsf[index].Flush();
                bsf[index].Close();
                //fs[index].Flush();
                fs[index].Close();
            }
            //SaveDatainfo(di, _bitmapdatapath + "datainfo" + activefloor.sID + ".json");
        }
        public void TestReadBitmapData(int roomcnt=-1,int savecnt=5)
        {
            //List<List<datainfo>> di = LoadDatainfo("datainfo" + activefloor.sID + ".json");
            int index = 0;
            foreach (Area I in _artemplate.Dic["非选择题"])
            //foreach (ScanTemplate.formimg.Room room in activefloor.Rooms)
            {
                Rectangle r = I.ImgArea;
                r.X = (r.X / 8) * 8 + (r.X % 8 > 2 ? 8 : 0);
                r.Width = (r.Width / 8) * 8 + (r.Width % 8 > 2 ? 8 : 0);

                //ReadBitmapdata(di[index],_bitmapdatapath, index, r, savecnt);
                index++;
                if (roomcnt == index) break;
            }
        }        
       
        private List<string> KhList()
        {
            List<string> khlist = new List<string>();
            Dictionary<string, string> dickhname = new Dictionary<string, string>();
            foreach (DataRow dr in _rundt.Rows)
            {
                khlist.Add(dr["考号"].ToString());
                dickhname[dr["考号"].ToString()] = dr["文件名"].ToString();
            }
            return khlist;
        }
        private string ImageFileName(string kh)
        {
            return _workpath + "C_IMAGES\\" + kh + "_00_1_p1.TIF";
        }
        private Bitmap GetBitmap(string kh)
        {
            string fullname = ImageFileName(kh);
            if (File.Exists(fullname))
                return (Bitmap)Bitmap.FromFile(fullname);
            return null;
        }

        private static void ConstructImgData(List<string> namelist, ref Rectangle imgrect)
        {
            FileStream fs = new FileStream("img.data", FileMode.Append, FileAccess.Write);
            MemoryStream ms = new MemoryStream();
            BufferedStream bs = new BufferedStream(ms, 40960);

            int startpos = 0;
            //List<datainfo> di = new List<datainfo>();
            for (int cnt = 0; cnt < namelist.Count; cnt++)
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(namelist[cnt]);
                Bitmap imgc = bmp.Clone(imgrect, bmp.PixelFormat);
                imgc.Save(bs, System.Drawing.Imaging.ImageFormat.Tiff);
                bs.Flush();
                byte[] buff = ms.ToArray();
                fs.Write(buff, 0, buff.Length);

                //di.Add(new datainfo(namelist[cnt], startpos, bs.Length));
                startpos += (int)bs.Length;

                if (buff.Length != bs.Length)
                {
                    System.Windows.Forms.MessageBox.Show("Error: buff.length=" + buff.Length + ",bs.length=" + bs.Length);
                }
            }
            ms.Flush();
            bs.Flush();
            fs.Flush();
            ms.Close();
            bs.Close();
            fs.Close();
            //return di;
        }
    }
}
