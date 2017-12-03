using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MetarCommonSupport;

namespace yj.formimg
{
    [Flags]
    enum Act : short { None = 0, DefinePoint = 1, DefineId = 2, DefineChoose = 4, DefineUnChoose = 8 };
    public delegate void CompleteMouseMove(bool bcompleted);
    public delegate bool CompleteConvertColor(byte src, byte dst);
    public delegate void CompleteFormTest(string action, string value);
    public partial class FormCreateTemplate : Form
    {
        public FormCreateTemplate()
        {
            InitializeComponent();
            init();
            if (!InitDatabase())
                return;
        }
        private void init()
        {
            activefloor = null;
            MT = new MoveTracker(pictureBox1);
            m_act = "zoom";
            radioButton1.Checked = true; //"zoom"
            wrect = new Rectangle(286, 209, 2299, 1523);
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            m_activeselection = new Rectangle(0, 0, 0, 0);
            crop_startpoint = new Rectangle(0, 0, 0, 0).Location;
            zoombox = new ZoomBox();
            activeroom = null;
            this.FillRectangle = false;
            this.DrawText = false;
        }
        public FormCreateTemplate(Db.ConnDb db, string workpath)
        {
            InitializeComponent();
            init();
            this.db = db;
            this.workpath = workpath;
            RefreshDatabasedata();
        }
        private bool InitDatabase()
        {
            while (true)
            {
                dbfilename = "data\\yj.mdb";
                if (!File.Exists(dbfilename))
                {
                    MessageBox.Show("数据库" + dbfilename + "不存在");
                    if (MessageBox.Show("重新选择数据库", "重新选择数据库", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                        return false;
                    OpenFileDialog fd = new OpenFileDialog();
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        this.dbfilename = fd.FileName;
                    }
                }
                try
                {
                    if (db != null)
                    {
                        db.connClose();
                        db = null;
                    }
                    FileInfo fi = new FileInfo(dbfilename);
                    db = new Db.ConnDb(fi.FullName);
                    RefreshDatabasedata();
                    db.connClose();
                    this.textBoxShow.Text = "当前数据数据库：" + dbfilename;
                    break;
                }
                catch
                {
                    MessageBox.Show("数据库：" +  dbfilename +" 格式不正确");
                    dbfilename = "";
                }
            }
            return true;
        }
        private void RefreshDatabasedata()
        {
            string sql = "select * from floor";
            try
            {
                DataSet ds = db.query(sql);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch
            {
            }
        }
        private void buttonAddFloor_Click(object sender, EventArgs e)
        {
             string floorname="";
             float floornum=0;
             string md5="";
             Size size;
             if (!InputBox.Input("输入模板名称", "模板名称：", ref  floorname, "编号(必填为0)", ref  floornum)) { MessageBox.Show("信息不全"); return; }//floor : 模板  ， room 小题

             OpenFileDialog OpenFileDialog2 = new OpenFileDialog();
             OpenFileDialog2.FileName = "OpenFileDialog2";
             OpenFileDialog2.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.TIF;*.png)|*.BMP;*.JPG;*.GIF;*.TIF;*.PNG|All files (*.*)|*.*"; //"Picture files (*.gif)|*.gif"; 
             OpenFileDialog2.Title = "Open Picture file";
             if (OpenFileDialog2.ShowDialog() == DialogResult.OK)
             {
                 try
                 {
                     Bitmap bitmap = (Bitmap)Bitmap.FromFile(OpenFileDialog2.FileName);
                     size = bitmap.Size;
                     bitmap.Dispose();
                     //md5 = MD5Encrypt(bitmap_src.ToString());
                     md5 = Tools.GetMd5Hash(OpenFileDialog2.FileName);
                     string sql = "insert into floor(floorname,path,imgw,imgh,md5) values('" + floorname+"','"
                                  + OpenFileDialog2.FileName + "',"+ size.Width + ","+size.Height + ",'"+md5+"');";
                     db.update(sql);
                     RefreshDatabasedata();
                     this.textBoxShow.Text = "OK";
                 }
                 catch(Exception ex)
                 {
                     MessageBox.Show("图片错误或者格式不支持"+ex.Message);
                     return;
                 }
             }
        }
        private void buttonRemoveFloor_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow!=null)
            {
                int index = dataGridView1.CurrentRow.Index;
                if (index == -1) return;
                string ID = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                string sql = "delete from floor where ID="+ID; //还应删除room
                try
                {
                    db.update(sql);
                    RefreshDatabasedata();
                }
                catch
                {
                }
            }
        }
        private void buttonEnterFloor_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                buttonQuitEditFloor.Visible = true;
                int ID = (int)dataGridView1.CurrentRow.Cells["ID"].Value;
                int w = (short)dataGridView1.CurrentRow.Cells["imgw"].Value;
                int h = (short)dataGridView1.CurrentRow.Cells["imgh"].Value;
                Size size = new System.Drawing.Size(w, h);
                string floorname = (string)dataGridView1.CurrentRow.Cells["floorname"].Value;
                string path = (string)dataGridView1.CurrentRow.Cells["path"].Value;
                string md5 = (string)dataGridView1.CurrentRow.Cells["md5"].Value;
                string blackcolor = (string)dataGridView1.CurrentRow.Cells["blackcolor"].Value.ToString();
                if (!File.Exists(path))
                {
                    MessageBox.Show(path + " 文件不存在");
                    return;
                }
                activefloor = new Floor(ID, floorname, size, path, md5);
                bitmap_show = activefloor.Clone(activefloor.Rectangle());
                pictureBox1.Image = bitmap_show;
                activefloor.InitSetImageSrc(db);
                ReSetPictureBoxImage();
                if (blackcolor != null)
                {
                    string[] vb = blackcolor.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    List<int> black = new List<int>();
                    foreach (string s in vb)
                    {
                        int c = Convert.ToInt32(s);
                        if (c >= 0 && c <= 255)
                            activefloor.AddBlack(c);
                    }

                }
                this.panelshow.Visible = false;
                pictureBox1.Visible = true;
            }

        }
        private void buttonSaveToSrc_Click(object sender, EventArgs e)
        {
            activefloor.Save(bitmap_show);
        }
        private void buttonback_Click(object sender, EventArgs e)
        {
            ReSetPictureBoxImage();
        }
        private void buttonQuitEditFloor_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Visible = false;
            activefloor = null;
            panelshow.Visible = true; ;

            buttonQuitEditFloor.Visible = false;
        }
        private void buttonReadme_Click(object sender, EventArgs e)
        {
            textBoxReadme.Visible = !textBoxReadme.Visible;
        }
        private void ButtonSetFxztClick(object sender, EventArgs e)
        {
        	SetFxzt();
        }
        
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_act = (string)((RadioButton)sender).Tag;
            if (m_act == "")
                m_act = "none";
            ShowMessage("act:" + m_act);

            MT.ClearEvent();
            if (m_act == "select" || m_act == "zoom" || m_act == "selectlist" || m_act == "selectdelete" || m_act == "listbyselection")
            {
                MT.StartDraw(true);
                MT.completevent += CompleteSelection;
            }
        }
        private void buttonTest_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.H)
            {
                if (activefloor == null || activefloor.Rooms.Count == 0)
                {
                    ShowMessage("no activefloor");
                    return;
                }
                string filename = "html\\img\\";
//                string imgsrcpath = "img\\";

                List<Room> rooms = new List<Room>(activefloor.Rooms);
                rooms.Sort((m1, m2) => m1.RoomName.CompareTo(m2.RoomName));

                string ext = ".png";
                if (activefloor.ImgExt().ToLower() == ".tif")
                    ext = ".tif";
                FileInfo fi = new FileInfo(activefloor.Imgfilename());
                DirectoryInfo dirinfo = fi.Directory;
                int cnt = 0;
                foreach (FileInfo f in dirinfo.GetFiles())
                {
                    if (f.Extension.ToLower() == ".tif")
                    {
                        cnt++;
                        string pre = f.Name.Substring(0, 6) + "_";
                        foreach (Room r in rooms)
                        {
                            string imgname = filename + pre + r.id.ToString() + ext;// imgsrcpath + r.id.ToString() + ext;
                            Bitmap img = (Bitmap)Bitmap.FromFile(f.FullName);
                            Bitmap imgc = img.Clone(r.Imgselection, img.PixelFormat);
                            imgc.Save(imgname);
                        }
                    }
                }
                MessageBox.Show("共切分" + cnt);
            }
            else if (e.KeyCode == Keys.I)
            {
                if (activefloor == null || activefloor.Rooms.Count == 0)
                {
                    ShowMessage("no activefloor");
                    return;
                }
                MultiBitmapToData mbt = new MultiBitmapToData(db, activefloor, workpath);
                mbt.SaveBitmapToData();
                mbt.TestReadBitmap();
            }
            else if (e.KeyCode == Keys.O)
            {
                if (activefloor == null || activefloor.Rooms.Count == 0)
                {
                    ShowMessage("no activefloor");
                    return;
                }
                MultiBitmapToData mbt = new MultiBitmapToData(db, activefloor, workpath);

                System.DateTime dt1 = System.DateTime.Now;
//                mbt.SaveBitmapDataToData();
                System.TimeSpan ts = System.DateTime.Now - dt1;
//                MessageBox.Show("耗时" + (ts.Minutes * 60 + ts.Seconds) + "." + ts.Milliseconds * 1.0 / 1000.0 + "秒");


                dt1 = System.DateTime.Now;
                mbt.TestReadBitmapData(1,-1); //全部输出 sub1
                
                ts = System.DateTime.Now - dt1;
                MessageBox.Show("耗时" + (ts.Minutes * 60 + ts.Seconds )+ "." + ts.Milliseconds * 1.0 / 1000.0 + "秒");
            }
            else if (e.KeyCode == Keys.C)
            {
                string dst = activefloor.maxNewRoomLength.ToString();
                if (InputBox.Input("SetmaxNewRoomLength", "maxNewRoomLength", ref dst))
                {
                    if (MetarnetRegex.IsNumber(dst))
                    {
                        int idst = Convert.ToInt32(dst);
                        activefloor.maxNewRoomLength = idst;
                    }
                }
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // ShowMessage("MouseMove");
            if (m_act != "list") return;
            Point p = new Point(e.X, e.Y);
            p = zoombox.BoxToImgPoint(p, crop_startpoint);//
            if (!activefloor.Rectangle().Contains(p))
                return;
            Color c = activefloor.GetPix(p);
            string value = activefloor.GetPixValue(p);
            ShowMessage(p + c.ToString() + value);

        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_act != "list") return;
            Point p = new Point(e.X, e.Y);
            if (!activefloor.Rectangle().Contains(p))
                return;
            p = zoombox.BoxToImgPoint(p, crop_startpoint);//
            Grid g = activefloor.GetGrid(p);
            foreach (Room room in g.Rooms)
            {
                if (room.Imgselection.Contains(p))
                {
                    m_activeselection = room.Imgselection;
                    this.ShowMessage("rooid" + room.id + " " + m_activeselection);
                    this.activeroom = room;
                    pictureBox1.Invalidate();
                    return;
                }
            }
            Room r;
            int roomtype = comboBoxFloor.SelectedIndex + 1;
            if (roomtype == 0)
            {
                MessageBox.Show("还未设定铺面类型");
            }else  if (roomtype == 2 || roomtype == 3 || roomtype == 4)
            {
                if (activefloor.AddNewRoom(out r, p))
                    activefloor.AddToDatabase(r,roomtype, db);
            }
            else
            {
                if (activefloor.AddNewRoom(out r, p))
                    activefloor.AddToDatabase(r, db);
            }
            pictureBox1.Invalidate();

        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null && activefloor.HasSelect())
            {
                Pen pen = Pens.Red;
                foreach (Rectangle t in activefloor.Selection())
                {
                    e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(t, crop_startpoint));
                }
            }
            if (pictureBox1.Image != null && activefloor.HasSelect())
            {
                Pen pen = Pens.Red;
                Brush br = Brushes.Yellow;
                Brush brt = Brushes.White;
                Font ft = new Font("宋体", 8, FontStyle.Regular);
                if (FillRectangle)
                    foreach (Rectangle t in activefloor.Selection())
                    {
                        Rectangle nr = new Rectangle(t.Left, t.Top, t.Width / 5, t.Height / 5);
                        if (nr.Height > 0 && nr.Width > 0)
                            e.Graphics.FillRectangle(br, zoombox.ImgToBoxSelection(nr, crop_startpoint));
                    }
                if (DrawText)
                    foreach (Room r in activefloor.Rooms)
                    {
                        RectangleF nr = zoombox.ImgToBoxSelection(r.Imgselection, crop_startpoint);
                        e.Graphics.DrawString(r.RoomName, ft, brt, nr.Left, nr.Top + nr.Height / 2);
                    }
            }
            if (pictureBox1.Image != null && m_activeselection.Width > 2)
            {//DrawActive
                Pen pen = Pens.Yellow;
                e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(m_activeselection, crop_startpoint));
                Brush br = Brushes.Yellow;
                e.Graphics.FillRectangle(br, zoombox.ImgToBoxSelection(m_activeselection, crop_startpoint));

            }
            if (pictureBox1.Image != null && bitmap_show!=null)//&& DrawGrid
            {
                Size imgr = bitmap_show.Size; //m_Imgselection;
                if (imgr.Width < 200 && imgr.Height < 200 && imgr.Width > 10 && imgr.Height > 10)
                {
                    Pen pen = Pens.Red;
                    Rectangle r = zoombox.GetPictureBoxZoomSize(pictureBox1);
                    float hrate = r.Height / imgr.Height;
                    for (int i = 0; i < imgr.Height; i++)
                        e.Graphics.DrawLine(pen, new Point(0+r.X, (int)(i * r.Height/imgr.Height) + r.Y), new Point(r.Width+r.X, (int)(i * r.Height/imgr.Height)+r.Y));
                    float wrate = r.Width / imgr.Width;
                    for (int i = 0; i < imgr.Width; i++)
                        e.Graphics.DrawLine(pen, new Point((int)(i * r.Width/imgr.Width) + r.X, 0 + r.Y), new Point((int)(i * r.Width/imgr.Width)+r.X, r.Height+r.Y));
                }
            }
        }

        private void CompleteSelection(bool bcomplete)
        {
            if (bcomplete)
            {
                ShowMessage("Complete: " + m_act);
                m_Imgselection = zoombox.BoxToImgSelection(MT.Selection, crop_startpoint );//
                switch (m_act)
                {
                    case "zoom": CompleteZoom(); break;
                    case "select": CompleteSelect( m_Imgselection); break;
                    case "selectlist": CompleteSelectList( m_Imgselection); break;
                    case "listbyselection": CompleteListBySelection(m_Imgselection); break;
                    case "selectdelete": CompleteSelectDelete( m_Imgselection); break;
                }
            }
            pictureBox1.Invalidate();
        }
        private void CompleteListBySelection(Rectangle s)
        {
            Room r;
            //s = zoombox.BoxToImgSelection(s, crop_startpoint);
            int roomtype = comboBoxFloor.SelectedIndex + 1;
            if (roomtype == 0)
            {
                MessageBox.Show("还未设定题型");
            }
            else if (roomtype == 2 || roomtype == 3 || roomtype == 4)
            {
                if (activefloor.AddNewRoom(out r, s))
                    activefloor.AddToDatabase(r, roomtype, db);
            }
            else
            {
                if (activefloor.AddNewRoom(out r, s))
                    activefloor.AddToDatabase(r, db);
            }
            pictureBox1.Invalidate();
        }
        private void CompleteZoom()
        {
            m_Imgselection.Intersect(activefloor.Rectangle());
            if (m_Imgselection.Width == 0) 
                return;
            bitmap_show.Dispose();
            bitmap_show = activefloor.Clone(m_Imgselection);
            crop_startpoint = m_Imgselection.Location ;
            pictureBox1.Image = bitmap_show;
            if(checkBoxCropSave.Checked)
             pictureBox1.Image.Save("show2.jpg");
            zoombox.UpdateBoxScale(pictureBox1);
        }
        private void CompleteSelect(Rectangle s)
        {
            // = m_Imgselection;
            ShowMessage(s.ToString());
            activefloor.AddSelect(s);   
           
        }  
        private void CompleteSelectList(Rectangle s)
        {
            
            ShowMessage(s.ToString());
            activefloor.AddNewRooms(s,db);
        }
        private void CompleteSelectDelete(Rectangle s)
        {
             //= m_Imgselection;
            ShowMessage(s.ToString());
            activefloor.DeleteRooms(s, db);
            m_act = "zoom";
            radioButton1.Checked = true;
        }
        private void SetFxzt()
        {
        	if (activefloor==null || !File.Exists(activefloor.Imgfilename()))
                return;
        	FormSetFzxt f = new FormSetFzxt(activefloor.Imgfilename(),activefloor.GetID(),db);
            f.ShowDialog();
        }
        private void ReSetPictureBoxImage()
        {
            //m_select.Clear();
            crop_startpoint.X = crop_startpoint.Y = 0;
            bitmap_show = activefloor.Clone(activefloor.Rectangle());  //(Bitmap)bitmap_src.Clone();
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            pictureBox1.Image = bitmap_show;
            zoombox.UpdateBoxScale(pictureBox1);
        }       
        private void ShowMessage(string p)
        {
            textBoxShow.Text = p;
        }

        private Bitmap bitmap_show;
        private Rectangle m_activeselection;
        private Room activeroom;
        private Point crop_startpoint;
        private Rectangle m_Imgselection;
        private MoveTracker MT;
        private ZoomBox zoombox;
        private string m_act;
        public Floor activefloor;
        private string dbfilename;    
        public static byte blackcolor = 40;
        private Rectangle wrect;
        private bool FillRectangle;
        private bool DrawText;

        private Db.ConnDb db;
        private string workpath;
    }
}
