using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace yj
{
    public partial class FormSetFzxt : Form
    {
        public FormSetFzxt(string imgpath, int activefloorid, Db.ConnDb db)
        {
            InitializeComponent();
            this.imgpath = imgpath;
            this.activefloorid = activefloorid;
            this.db = db;
            Init();
        }
        private void Init()
        {
            string sql = "select * from room where floorid = [floorid] order by id"
                .Replace("[floorid]", activefloorid.ToString());
            DataTable dt = db.query(sql).Tables[0];
            List<string> roomids = new List<string>();

            _dtshow = Tools.DataTableTools.ConstructDataTable(new string[] { "ID", "题组名称", "最大分值", "图片" });

            string imgname = imgpath.Replace("[id]", imgpath);
            Bitmap img = (Bitmap)Bitmap.FromFile(imgname);
            int with = 100;
            int height = 100;
            int sumwith = 0;
            int sumheight = 0;
            foreach (DataRow dr in dt.Rows)
            {
                DataRow dts = _dtshow.NewRow();
                dts["ID"] = dr["ID"];
                dts["题组名称"] = dr["roomname"];
                dts["最大分值"] = dr["maxscore"];

                int x = Convert.ToInt32(dr["rx"].ToString());
                int y = Convert.ToInt32(dr["ry"].ToString());
                int w = Convert.ToInt32(dr["rw"].ToString());
                int h = Convert.ToInt32(dr["rh"].ToString());
                Rectangle r = new Rectangle(x, y, w, h);
                Bitmap imgc = img.Clone(r, img.PixelFormat);
                sumheight+=imgc.Height;
                sumwith+=imgc.Width;
                with = with > imgc.Width ? with : imgc.Width;
                height = height > imgc.Height ? height : imgc.Height;

//                ((PictureBox)(dts["图片"])).Image = imgc;
				dts["图片"] = imgc;
                _dtshow.Rows.Add(dts);
            }
			
            dgv.DataSource = _dtshow;
            
            DataGridViewImageColumn dgvimgc =(DataGridViewImageColumn) dgv.Columns["图片"];
        	dgvimgc.ImageLayout = DataGridViewImageCellLayout.Zoom;
        	
            dgv.RowTemplate.Height =(int)( sumheight /(3* dt.Rows.Count));
            dgv.Columns["图片"].Width =(int)( sumwith/ (3* dt.Rows.Count));
        }
        private void buttonSetOK_Click(object sender, EventArgs e)
        {
            string sqlt = "update room set roomname='[roomname]' , maxscore=[score] where id = [id] ";
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
            	string score = dgv.Rows[count].Cells["最大分值"].Value.ToString();
            	if(score == "")
            		score="0";
                string sql = sqlt.Replace("[roomname]", dgv.Rows[count].Cells["题组名称"].Value.ToString())
                    .Replace("[score]", score)
                    .Replace("[id]", dgv.Rows[count].Cells["id"].Value.ToString());
                db.update(sql);
            }
            MessageBox.Show("更新完成，请关闭本窗口");
        }
        
        private int activefloorid;
        private Db.ConnDb db;
        private string imgpath;
        public DataTable _dtshow { get; set; }
        
    }
}
