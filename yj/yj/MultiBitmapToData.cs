using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data;
using Newtonsoft.Json;
using System.Drawing.Imaging;


namespace yj
{
    public class MultiBitmapToData
    {
        private Db.ConnDb db;
        private formimg.Floor activefloor;
        private string workpath;
        public MultiBitmapToData(Db.ConnDb db, formimg.Floor activefloor, string workpath)
        {
            this.db = db;
            this.activefloor = activefloor;
            this.workpath = workpath;
        }
        public void TestReadBitmap()
        {
            List<List<datainfo>> di = LoadDatainfo("datainfo" + activefloor.sID + ".json");
            int index=0;
            foreach (yj.formimg.Room r in activefloor.Rooms)
            {
                ReadBitmap(di[index], "room_" + r.id + ".data",r.id);
                index++;
            }
        }
        public void SaveBitmapToData()
        {
            List<formimg.Room> rooms = activefloor.Rooms;
            //List<formimg.Room> rooms = new List<formimg.Room>();
            //rooms.Add(activefloor.Rooms[0]);

            List<FileStream> fs = new List<FileStream>();
            List<BufferedStream> bsf = new List<BufferedStream>();
            List<List<datainfo>> di = new List<List<datainfo>>();
            List<long> startpos = new List<long>();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (File.Exists("room_" + rooms[i].id + ".data"))
                    File.Delete("room_" + rooms[i].id + ".data");
                fs.Add( new FileStream("room_" + rooms[i].id + ".data", FileMode.Append, FileAccess.Write));
                startpos.Add(0);
                di.Add(new List<datainfo>());
                bsf.Add( new BufferedStream( fs[i],102400));
            }

            foreach (string kh in KhList())
            {
                Bitmap bmp = GetBitmap(kh);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BufferedStream bs = new BufferedStream(ms))
                    {
                        for (int i = 0; i < rooms.Count; i++)
                        {
                            Bitmap imgc = bmp.Clone(rooms[i].Imgselection, bmp.PixelFormat);
                            imgc.Save(bs, System.Drawing.Imaging.ImageFormat.Tiff);
                            bs.Flush();
                            byte[] buff = ms.ToArray();

                            bsf[i].Write(buff, 0, buff.Length);

                            di[i].Add(new datainfo(kh, startpos[i], bs.Length));
                            startpos[i] +=  bs.Length;
                        }
                    }
                }
            }

            for (int i = 0; i < rooms.Count; i++)
            {
                bsf[i].Flush();
                bsf[i].Close();
                fs[i].Flush();
                fs[i].Close();
            }
            //ReadBitmap(di[0], "room_" + rooms[0].id + ".data");

            SaveDatainfo(di,"datainfo"+activefloor.sID+".json");

        }
        public void SaveBitmapDataToData()
        {
            string path = workpath + "floor[fid]bitmapdata\\".Replace("[fid]", activefloor.sID);
            List<formimg.Room> rooms = activefloor.Rooms;

            List<FileStream> fs = new List<FileStream>();
            List<BufferedStream> bsf = new List<BufferedStream>();
            List<List<datainfo>> di = new List<List<datainfo>>();
            List<long> startpos = new List<long>();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (File.Exists(path+"roomdata_" + rooms[i].id + ".data"))
                    File.Delete(path+"roomdata_" + rooms[i].id + ".data");

                fs.Add(new FileStream(path+"roomdata_" + rooms[i].id + ".data", FileMode.Append, FileAccess.Write));
                startpos.Add(0);
                di.Add(new List<datainfo>());
                bsf.Add(new BufferedStream(fs[i], 102400));
            }

            foreach (string kh in KhList())
            {
                Bitmap bmp = GetBitmap(kh);
                BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
               
                for (int index = 0; index < rooms.Count; index++)
                {
                    Rectangle r = rooms[index].Imgselection;
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
                    di[index].Add(new datainfo(kh, startpos[index], buff.Length));
                    startpos[index] += buff.Length;
                }
            }

            for (int index = 0; index < rooms.Count; index++)
            {
                bsf[index].Flush();
                bsf[index].Close();
                //fs[index].Flush();
                fs[index].Close();
            }
            SaveDatainfo(di, path + "datainfo" + activefloor.sID + ".json");

        }
        public void TestReadBitmapData(int roomcnt=-1,int savecnt=5)
        {
            List<List<datainfo>> di = LoadDatainfo("datainfo" + activefloor.sID + ".json");
            int index = 0;
            foreach (yj.formimg.Room room in activefloor.Rooms)
            {
                Rectangle r = room.Imgselection;
                r.X = (r.X / 8) * 8 + (r.X % 8 > 2 ? 8 : 0);
                r.Width = (r.Width / 8) * 8 + (r.Width % 8 > 2 ? 8 : 0);

                string roomdatapath =  "floor"+activefloor.sID+"bitmapdata\\roomdata_" + room.id + ".data";
                roomdatapath = workpath.Replace("C_IMAGES\\",roomdatapath);
                ReadBitmapdata(di[index],roomdatapath , room.id,r,savecnt);
                index++;
                if(roomcnt == index ) break;
            }
        }        
        private static void ReadBitmapdata(List<datainfo> di, string imgdatafilename, int roomid, Rectangle r,int savecnt=5)
        {
            FileStream fs = new FileStream(imgdatafilename, FileMode.Open, FileAccess.Read);
            BufferedStream bs = new BufferedStream(fs, 4096);
            int cnt = savecnt;
            if(savecnt<0 || savecnt > di.Count)
            	cnt = di.Count; 
            for (int i = 0; i < cnt; i++)
            {
                byte[] buffer = new byte[di[i].count];
                bs.Position = di[i].startpos;
                bs.Read(buffer, 0, buffer.Length);
                unsafe
                {
                    fixed (byte* p = &buffer[0])
                    {
                        int stride = r.Width / 8;
                        stride = (stride / 4 * 4) + (stride % 4 > 0 ? 4 : 0);
                        Bitmap Bmp = new Bitmap(r.Width, r.Height, stride, PixelFormat.Format1bppIndexed, (IntPtr)p);
                        Bmp.Save(di[i].kh + "_" + roomid + ".tif");
                    }
                }
            }
            bs.Close();
            fs.Close();
        }
        private static void ReadBitmap(List<datainfo> di,string imgdatafilename,int roomid = 0)
        {
            FileStream fs = new FileStream(imgdatafilename, FileMode.Open, FileAccess.Read);
            BufferedStream bs = new BufferedStream(fs, 4096);
            for (int i = di.Count - 10; i >= di.Count-15; i--)
            {
                byte[] buffer = new byte[di[i].count];
                bs.Position = di[i].startpos;
                bs.Read(buffer, 0, buffer.Length);

                Stream sss = new MemoryStream();
                sss.Write(buffer, 0, buffer.Length);
                Bitmap imgc = (Bitmap)Bitmap.FromStream(sss);
                imgc.Save(di[i].kh +"_"+roomid+ ".tif");
                sss.Dispose();
                sss.Close();
            }
            bs.Close();
            fs.Close();
        }       
        private void SaveDatainfo(List<List<datainfo>> di, string filename)
        {
            string str = Tools.JsonFormatTool.ConvertJsonString(JsonConvert.SerializeObject(di));
            File.WriteAllText(filename, str);
        }
        private List<List<datainfo>> LoadDatainfo(string datainfojsonfilename)
        {
        	string path =   datainfojsonfilename;
        	if(!datainfojsonfilename.Contains(":"))
        		path = workpath.Replace("C_IMAGES\\","floor[fid]bitmapdata\\".Replace("[fid]",activefloor.sID)+datainfojsonfilename);
            if (File.Exists(path))
            {
                string str = File.ReadAllText(path);
                List<List<datainfo>> di = JsonConvert.DeserializeObject<List<List<datainfo>>>(str);
                return di;
            }
            return null;
        }
        private List<string> KhList()
        {
            string sql = "select kh from subjectscore_[floorid] ".Replace("[floorid]",activefloor.sID);
            DataTable dt = db.query(sql).Tables[0];
            List<string> khlist = new List<string>();
            foreach (DataRow dr in dt.Rows)
                khlist.Add(dr["kh"].ToString());
            return khlist;
        }
        private string ImageFileName(string kh)
        {
            return workpath + "C_IMAGES\\" + kh + "_00_1_p1.TIF";
        }
        private Bitmap GetBitmap(string kh)
        {
            string fullname = ImageFileName(kh);
            if (File.Exists(fullname))
                return (Bitmap)Bitmap.FromFile(fullname);
            return null;
        }

        //for Test //Not using  //deleteable 
        private List<string> NameList(string filename)
        {
            List<string> namelist = new List<string>();
            if (File.Exists(filename))
            {
                FileInfo fi = new FileInfo(filename);
                DirectoryInfo dirinfo = fi.Directory;
                string ext = fi.Extension;
                foreach (FileInfo f in dirinfo.GetFiles())
                    if (f.Extension.ToLower() == ".tif")
                        namelist.Add(f.FullName);
            }
            return namelist;
        }
        private static List<datainfo> ConstructImgData(List<string> namelist, ref Rectangle imgrect)
        {
            FileStream fs = new FileStream("img.data", FileMode.Append, FileAccess.Write);
            MemoryStream ms = new MemoryStream();
            BufferedStream bs = new BufferedStream(ms, 40960);

            int startpos = 0;
            List<datainfo> di = new List<datainfo>();
            for (int cnt = 0; cnt < namelist.Count; cnt++)
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(namelist[cnt]);
                Bitmap imgc = bmp.Clone(imgrect, bmp.PixelFormat);
                imgc.Save(bs, System.Drawing.Imaging.ImageFormat.Tiff);
                bs.Flush();
                byte[] buff = ms.ToArray();
                fs.Write(buff, 0, buff.Length);

                di.Add(new datainfo(namelist[cnt], startpos, bs.Length));
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
            return di;
        }

    }
    public class datainfo
    {
        public datainfo(string kh, long  startpos, long count)
        {
            this.kh = kh;
            this.startpos = startpos;
            this.count = count;
        }
        public string kh;
        public long startpos;
        public long count;
        public override string ToString()
        {
            return kh + " " + startpos + " " + count;
        }
    }
}
