using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DBQuery
{
    public partial class FormData : Form
    {
        private Db.ConnDb dbdata;
        public FormData(Db.ConnDb dbdata)
        {
            InitializeComponent();
            this.dbdata = dbdata;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] rs = textBoxData.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (checkBoxDataFromTxtFile.Checked && rs.Count() == 1)
            {
                if(File.Exists(rs[0]))
                    rs = File.ReadAllText(rs[0]).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (rs.Count() == 0) return;
            string sqlt = textBoxSql.Text.Trim();
            if (sqlt == "") return;
            string strdem = textBoxCustom.Text;
            if (checkBoxTab.Checked)
                strdem += "\r\n\t";
            if (checkBoxComma.Checked)
                strdem += "\r\n,";            
            string[] dem =strdem .Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int sum = 0;
            foreach (string r in rs)
            {
                string[] items = r.Split(dem,StringSplitOptions.None);//, StringSplitOptions.RemoveEmptyEntries
                string sql = sqlt;
                for (int i = 0; i < items.Count(); i++)
                {
                    sql = sql.Replace("[id" + i + "]", items[i]);
                }
                try
                {
                    dbdata.update(sql);
                }
                catch (Exception ee)
                {
                    sum++;
                    File.AppendAllText("FormData.log","Exception.Msg=" + ee.Message +"  record=\""+r+"\"   sql=\""+sql+"\"\r\n");
                }
            }
            MessageBox.Show("执行成功 " + (rs.Length-sum)+"/"+rs.Length+"条记录");
            
        }
    }
}
