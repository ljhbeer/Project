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
    public partial class Formhp : Form
    {
        private Db.ConnDb db;
        private subject activesj;
        private List<string> done;
        private string workpath;
        private string activeid;

        public Formhp(Db.ConnDb db, string workpath, subject activesj, List<string> done)
        {
            this.db = db;
            this.workpath = workpath;
            this.activesj = activesj;
            this.done = done;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            activeid = done[done.Count-1];
            string imgname = workpath + activeid + ".tif";
            pictureBox1.Image = Bitmap.FromFile(imgname);            
        }

        private void buttonok_Click(object sender, EventArgs e)
        {
            string fs = textBoxFenshu.Text;
            try
            {
                int result = Convert.ToInt32(fs);
                if (result >= 0 && result <= activesj.MaxResult)
                {
                    string sql = "update subjectresult set result = [result] where id = '[id]'";
                    sql = sql.Replace("[result]", result.ToString()).Replace("[id]", activeid);
                    if (db.update(sql) == 1)
                    {
                        pictureBox1.Image = null;                        
                        pictureBox1.Invalidate();
                        textBoxShow.Text = "已完成回评，请关闭";                      
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
    }
}
