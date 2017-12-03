using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.IO;

namespace yj.formimg{
    public class Floor
    {
        public Floor(int ID, string floorname, Size size, string path, string md5 )
        {
            this.maxNewRoomLength = 100;
            blackcolor = new List<int>();
            bitmap_src = null;
            bitmap_lock = null;
            bitmapdata = null;
            m_select = new List<Rectangle>();
            m_grid = new List<List<Grid>>();
            m_room = new List<Room>();
            colcnt = rowcnt = 0;
            collength = rowlength = 0;
            this.ID = ID;
            this.floorname = floorname;
            this.imgsize = size;
            this.bitmap_Filename = path;
            this.md5 = md5;
            if (!Check())
                return;
        }
        ~Floor()
        {
            
        }

        public void AddBlack(int c)
        {
            blackcolor.Add(c);
        }
        public void InitSetImageSrc(Db.ConnDb db)
        {
            colcnt = imgsize.Width / 50;
            collength = imgsize.Width * 1.0 / colcnt;
            rowcnt = imgsize.Height / 50;
            rowlength = imgsize.Height * 1.0 / rowcnt;
            m_grid.Clear();
            for (int r = 0; r < rowcnt; r++)
            {
                List<Grid> g = new List<Grid>();
                for (int c = 0; c < colcnt; c++)
                    g.Add(new Grid());
                m_grid.Add(g);
            }
            m_room.Clear();
            string sql = "select ID,rx,ry,rw,rh,roomnumber,roomname from room where floorid =" + ID +"  order by ID ";
            
            m_select.Clear();
            db.TestConnect();
            DataSet ds = db.query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int rid = Convert.ToInt32(dr["ID"].ToString());
                Rectangle rect = new Rectangle(Convert.ToInt32(dr["rx"].ToString()),
                                                Convert.ToInt32(dr["ry"].ToString()),
                                                Convert.ToInt32(dr["rw"].ToString()),
                                                Convert.ToInt32(dr["rh"].ToString()));
                Room r = new Room(rid, rect,dr["roomname"].ToString());
                m_select.Add(rect);
                AddRoom(r);
            }
        }
        public void AddNewRooms(Rectangle s, Db.ConnDb db)
        {
            Rectangle Hr, Vr;
            Hr = Vr = s;
            Hr.Y += Hr.Height / 2;
            Hr.Height =1;
            Vr.X += Hr.Width / 2;
            Vr.Width = 1;
            Bitmap Hb = Clone(Hr);
            Bitmap Vb = Clone(Vr);
            if (CheckLine(Hb))
            {
                Hr.Y -= s.Height / 4;
                Hb.Dispose();
                Hb = Clone(Hr);
                if (CheckLine(Hb)) return;
            }
            if (CheckLine(Vb))
            {
                Vr.Y -= s.Width / 4;
                Vb.Dispose();
                Vb = Clone(Vr);
                if (CheckLine(Vb)) return;
            }
            List<Rectangle> rects = DetectChoiceArea.DectRectangles(Hb, Vb, ref blackcolor);
            List<Grid> vg = this.GetGridRelationRectangle(s);
            List<Room> rooms = new List<Room>();
            foreach (Grid g in vg)
            {
                rooms.AddRange(g.Rooms);
            }
            if (rects.Count == 0 || rects.Count>100) return ;
            foreach (Rectangle r in rects)
            {
                r.Offset(s.Location);
           
                if (r.Width == 0 || r.Width <9 || r.Width>120 || r.Height<9 || r.Height>120) continue;
                bool flag = true;
                foreach (Room room in rooms)
                {
                    if (!room.Imgselection.IntersectsWith(r))
                    {
                       //
                    }
                    else
                    {
                        flag = false; return;
                    }
                }
                if (flag)
                {
                     m_select.Add(r);
                    int rid = m_room.Count;
                    Room rm = new Room(rid, r,"");
                    AddRoom(rm);
                    AddToDatabase(rm, db);
                }
            }
        }
        public void DeleteRooms(Rectangle s, Db.ConnDb db)
        {
            List<Grid> vg = this.GetGridRelationRectangle(s);
            List<Room> rooms = new List<Room>();
            foreach (Grid g in vg)
            {
                foreach (Room room in g.Rooms)
                {
                    if (!rooms.Contains(room) && s.Contains(room.Imgselection))
                    {
                        rooms.Add(room);
                    }
                }
            }
            foreach (Room room in rooms)
            {
                foreach (Grid g in vg)
                {
                    if (g.Rooms.Contains(room))
                        g.Remove(room);
                }
            }
            //
            foreach (Room room in rooms)
            {
                m_select.Remove(room.Imgselection);
                RemoveFromDatabase(room, db);
            }
        }
        public bool AddNewRoom(out Room r, Point p)
        {
            int lenr = this.maxNewRoomLength;
            r = null;
            Rectangle Hr = new Rectangle(p.X - lenr > 0 ? p.X - lenr : 0, p.Y,
                  imgsize.Width - p.X < lenr ? imgsize.Width - p.X+lenr : 2 * lenr, 1);
            Rectangle Vr = new Rectangle(p.X, p.Y - lenr > 0 ? p.Y - lenr : 0,
                                           1, imgsize.Height - p.Y < lenr ? imgsize.Height - p.Y+lenr : 2 * lenr);

            //Bitmap debug = Clone(new Rectangle(Hr.X, Vr.Y, Hr.Width, Vr.Height));
            //debug.Save("debug.png");

            Bitmap Hb = Clone(Hr); 
            Bitmap Vb = Clone(Vr);            
            Rectangle rect = DetectChoiceArea.DectRectangle(Hb, Vb,ref blackcolor);
            if (rect.Width == 0) return false; ;
            rect.Offset(p.X - lenr > 0 ? p.X - lenr : 0, p.Y - lenr > 0 ? p.Y - lenr : 0);
            m_select.Add(rect);//new Rectangle(p, new Size(20, 20))

            int rid = m_room.Count;
            r = new Room(rid, rect,"");
            AddRoom(r);
            return true;
        }

        internal bool AddNewRoom(out Room r, Rectangle rect)
        {
            r = null;
            if (rect.Width == 0  || rect.Height==0) return false; ;            
            m_select.Add(rect);//new Rectangle(p, new Size(20, 20))
            int rid = m_room.Count;
            r = new Room(rid, rect, "");
            AddRoom(r);
            return true;
        }
        public void AddToDatabase(Room r, Db.ConnDb db)
        {
            string sql = r.InsertSql(ID);
            db.update(sql);
        }
        public void AddToDatabase(Room r, int roomtype, Db.ConnDb db)
        {
            string sql = r.InsertTypeSql(ID,roomtype);
            db.update(sql);
        }
        public void RemoveFromDatabase(Room r, Db.ConnDb db)
        {
            string sql = r.DeleteSql(ID);
           int rows =  db.update(sql);
           if (rows == 0)
           {
               rows++;
           }
        }
        public void AddSelect(Rectangle s)
        {
            m_select.Add(s);
        }
        public void Save(Bitmap bitmap_show)
        {
            if (imgsize != bitmap_show.Size) return;
            bitmap_src = (Bitmap)bitmap_show.Clone();
        }
        public bool HasSelect()
        {
            return m_select.Count != 0;
        }
        public string GetPixValue(Point p)
        {
            if (bitmap_lock == null) return "";
            string ret = "value=";
            if( bitmap_lock.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                return ret;
            }
            unsafe
            {
                //byte* ptr = (byte*)(bitmapdata.Scan0);
                //ptr += bitmapdata.Stride * p.Y + p.X;
                //ret += ptr[0];
                //Color c = colorpalette.Entries[ptr[0]];
                //ret += c.ToString();
                //
                bitmapdata = bitmap_lock.LockBits( new Rectangle(p,new Size(1,1)), ImageLockMode.ReadOnly, bitmap_lock.PixelFormat);
                byte* ptr = (byte*)(bitmapdata.Scan0);
                Color c = colorpalette.Entries[ptr[0]];

                ret +=ptr[0] +  c.ToString();
                bitmap_lock.UnlockBits(bitmapdata);
            }
            return ret;
        }
        public int maxNewRoomLength { get; set; }
        public void ClearBlack()
        {
            this.blackcolor.Clear();
        }
        public List<int> Blackcolor
        {
            get { return blackcolor; }
        }
        public Grid GetGrid(Point p)
        {
            return m_grid[p.Y * rowcnt / imgsize.Height][p.X * colcnt / imgsize.Width];
        }
        public IEnumerable<Rectangle> Selection()
        {
            return m_select;
        }
        public Color GetPix(Point p)
        {
            return  bitmap_src.GetPixel(p.X, p.Y);
        }
        public Bitmap OutPutBitmap()
        {
            if (bitmap_src == null) return null;
            //Bitmap bmp =(Bitmap) bitmap_src.Clone();
            //Graphics g = Graphics.FromImage(bmp);
            //Pen pen = Pens.Red;
            //g.DrawRectangles(pen,m_select.ToArray());
            //foreach (Rectangle r in m_select)
            //{
            //    g.
            //}

            Bitmap bmp = new Bitmap(bitmap_src.Width, bitmap_src.Height, PixelFormat.Format32bppArgb); 
            using (Graphics g = Graphics.FromImage(bmp)) 
            { 
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; 
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; 
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(bitmap_src, 0, 0);
                Pen pen = Pens.Red;
                g.DrawRectangles(pen, m_select.ToArray());
                Brush br = Brushes.Yellow;
                Brush brtxt = Brushes.White;
                Font ft = new Font("宋体", 8, FontStyle.Regular);
                //foreach (Rectangle t in m_select)
                //{
                //    Rectangle nr = new Rectangle(t.Left, t.Top, t.Width / 5, t.Height / 5);
                //    if (nr.Height > 0 && nr.Width > 0)
                //    g.FillEllipse(br, nr);
                //}
                foreach (Room rr in Rooms)
                {
                    Rectangle r = rr.Imgselection;
                    Rectangle brushr = new System.Drawing.Rectangle(r.X, r.Y, r.Width / 5, r.Height / 5);
                    g.FillEllipse(br, brushr);
                    Rectangle txtr = new Rectangle(r.X,(int)(r.Y+0.6*r.Height),r.Y,r.Height);
                    g.DrawString(rr.RoomName, ft, brtxt, new PointF(txtr.X, txtr.Y));
                }
            }
            return bmp;
        }
        public Bitmap Clone(Rectangle Hr)
        {
            return (Bitmap)bitmap_src.Clone(Hr, bitmap_src.PixelFormat);
        }
        public Rectangle Rectangle()
        {
            return new Rectangle(0, 0, bitmap_src.Width, bitmap_src.Height);
        }
        public List<Room> Rooms
        {
            get { return m_room; }
        }
        
        
        private bool Check()
        {
            try
            {
                bitmap_src = (Bitmap)Bitmap.FromFile( bitmap_Filename );
                if (bitmap_src.Size != imgsize) return false;
                string checkmd5 = Tools.GetMd5Hash(bitmap_Filename);
                if (md5 != checkmd5) return false;
                if (bitmap_Filename.EndsWith(""))
                {
                    bitmap_lock =(Bitmap) bitmap_src.Clone();
                    bitmapdata = bitmap_lock.LockBits(Rectangle(), ImageLockMode.ReadOnly, bitmap_lock.PixelFormat);
                    bitmap_lock.UnlockBits(bitmapdata);
                    colorpalette = bitmap_lock.Palette;
                   // bmp.LockBits(r, ImageLockMode.ReadWrite, bmp.PixelFormat);
                }
                //InitSetImageSrc();
                //ReSetPictureBoxImage();  
            }catch{
                return false;
            }
            return true;
        }
        private bool CheckLine(Bitmap b)
        {
            Rectangle r = new Rectangle(0, 0, b.Width, b.Height);
            int[] va;
            if (b.Width == 1)
                va = DetectChoiceArea.CountImgYBlackCnt(b, r);
            else
                va = DetectChoiceArea.CountImgXBlackCnt(b, r);
            if (va.Count(i => blackcolor.Contains(i)) > va.Count() * 0.8)
                return false;
            return true;
        }
        private void AddRoom(Room r)
        {
            m_room.Add(r);
            List<Grid> rg = GetGridRelationRectangle(r.Imgselection);
            foreach (Grid g in rg)
                g.AddRoom(r);
        }
        
        private List<Grid> GetGridRelationRectangle(Rectangle rect)
        {
            List<Grid> g = new List<Grid>();
            int cb = rect.Left * colcnt / imgsize.Width;
            int ce = rect.Right * colcnt / imgsize.Width;
            int rb = rect.Top * rowcnt / imgsize.Height;
            int re = rect.Bottom * rowcnt / imgsize.Height;
            for (int r = rb; r <= re; r++)
                for (int c = cb; c <= ce; c++)
                    g.Add(m_grid[r][c]);
            return g;
        }
        private List<Room> m_room;

        public string Imgfilename()
        {
            return bitmap_Filename;
        }
        public string ImgExt()
        {
            if (File.Exists(bitmap_Filename))
            {
                FileInfo fi = new FileInfo(bitmap_Filename);
                return fi.Extension;
            }
            return "";
        }
        public int GetID(){ return ID;}
        public string sID {get { return ID.ToString();}}
        private List<List<Grid>> m_grid;
        private List<Rectangle> m_select;
        private int colcnt, rowcnt;
        private double collength, rowlength;
        private string bitmap_Filename;
        private Bitmap bitmap_src;
        private Bitmap bitmap_lock;
        private BitmapData bitmapdata;
        private ColorPalette colorpalette;
        private Size imgsize;
        private string floorname;
        private string md5;
        private List<int> blackcolor;
        private int ID;


    }
}
