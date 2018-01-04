using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tools;
using System.IO;

namespace ScanTemplate
{
    public partial class FormOutTxtToImg : Form
    {
        private ScanConfig _sc;
        private TextBitmapTool _tbl;
        public FormOutTxtToImg(ScanConfig _sc)
        {
            InitializeComponent();
            this._sc = _sc;
            _tbl = new TextBitmapTool(
                          new Rectangle(0, 0, 960, 720), new Rectangle(40, 30, 880, 660));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                SaveFileDialog saveFileDialog2 = new SaveFileDialog();
                saveFileDialog2.FileName = "输出列表";
                saveFileDialog2.Filter = "image files (*.jpeg)|*.jpeg";
                saveFileDialog2.Title = "导出";
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(saveFileDialog2.FileName);
                }
            }
        }
        private void buttonTxtToImg_Click(object sender, EventArgs e)
        {
            List<string> list = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            pictureBox1.Image = _tbl.DrawListInPaper(list, checkBoxBlackFont.Checked);
        }

        private void buttonOutNameList_Click(object sender, EventArgs e)
        {
            List<int> cls = _sc.Studentbases.Studentbase.Select(r => r.Classid).Distinct().ToList();
            List<object> clsobj = cls.Select(r => (Object)r).ToList();
            if (ARTemplate.InputBox.Input("选择班级", clsobj))
            {
                int clsid = ARTemplate.InputBox.IntValue;
                List<FormYJ.StudentBase> lst = _sc.Studentbases.GetClassStudent(clsid);
                lst.Sort((r1, r2) => r1.KH - r2.KH);
                List<string> list = lst.Select(r => r.Name + " " + r.KH).ToList();
                pictureBox1.Image = _tbl.DrawListInPaper(list, checkBoxBlackFont.Checked);
            }
        }

        private void FormOutTxtToImg_Load(object sender, EventArgs e)
        {
            if (_sc != null)
            {
                listBoxScantData.Items.Clear();
                listBoxScantData.Items.AddRange(_sc.Scandatas.Scandatas.ToArray());
                string idindexpath = _sc.Baseconfig.ExamPath + "\\config.json";
                if (!File.Exists(idindexpath))
                    return;
                ExamConfig g = Newtonsoft.Json.JsonConvert.DeserializeObject<ExamConfig>(File.ReadAllText(idindexpath));
                listBox2.Items.AddRange(g._examinfo.ToArray());
            }
        }
        private void listBoxScantData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxScantData.SelectedIndex == -1) return;
            ScanData sd = (ScanData)listBoxScantData.SelectedItem;
            if (File.Exists(sd.DataFullName))
            {
                string[] ls = File.ReadAllLines(sd.DataFullName);
                List<string> titles = ls[0].Split(',').ToList();

                //StringBuilder sb = new StringBuilder()
                List<string> list = new List<string>();
                List<int> kh = new List<int>();
                list.Add("已交学生名单");
                for (int i = 1; i < ls.Length; i++)
                {
                    string[] ss = ls[i].Split(',');
                    if (ss.Length > 5)
                        list.Add(ss[4] + " " + ss[3]);
                    if (!ss[3].Contains("-") && ss[3] != "")
                    {
                        try
                        {
                            kh.Add(Convert.ToInt32(ss[3].Trim()));
                        }
                        catch { }
                    }
                }
                list.Add("未交学生名单");
                foreach (string s in UnScanNameList(kh))
                    list.Add(s + " 未交");
                textBox1.Text = string.Join("\r\n", list);
                pictureBox1.Image = _tbl.DrawListInPaper(list, checkBoxBlackFont.Checked);
            }
            else
            {
                MessageBox.Show("没有发现扫描数据");
            }
        }

        private List<string> UnScanNameList(List<int> kh)
        {
            List<int> Lclassid = kh.Select(r => _sc.Studentbases.GetClass(r)).Distinct().ToList();
            Lclassid.Remove(0);
            if (Lclassid.Count > 1)
                return new List<string>();
            int cid = Lclassid[0];
            List<string> namelist =
             _sc.Studentbases.GetClassStudent(cid).Where(r => !kh.Exists(rr => rr == r.KH))
                 .Select(r1 => r1.Name).ToList();
            return namelist;
        }
    }
}
