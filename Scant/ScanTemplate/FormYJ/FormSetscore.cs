using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ScanTemplate.FormYJ
{
    public partial class FormSetscore : Form
    {
        public FormSetscore(DataTable _dtsetxzt)
        {
            this._dtsetxzt = _dtsetxzt;
            InitializeComponent();
            buttonClearanswer.Visible = false;
            
            InitXZTQuestion(_dtsetxzt);
            InitDgvAndCbx();
            ReFreshDgv();            
        }
        private void InitDgv(int questioncnt, DataGridView dgv) // used by settypeform
        {
            string[] strxq = new string[] { "题型", "答案", "分值","半对分值" };
            dgv.RowCount = questioncnt > 0 ? questioncnt : 1;
            dgv.ColumnCount = 4;
            dgv.ReadOnly = true;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.RowHeadersWidth = 100;
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].Width = 60;
                dgv.Columns[i].HeaderText = strxq[i];
                dgv.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < dgv.RowCount; i++)
            {
                dgv.Rows[i].HeaderCell.Value = "第" + (i + 1).ToString() + "题";
            }
        }
        private void InitXZTQuestion(DataTable _dtsetxzt) // 题组名称 最大分值 正确答案
        {
            _xzt = new List<XztQuestion>();
            foreach (DataRow dr in _dtsetxzt.Rows)
            {
                XztQuestion x = new XztQuestion();
                string s = dr["题组名称"].ToString();
                x.ID = Convert.ToInt32(s.Substring(1));
                x.OptionAnswer = dr["正确答案"].ToString();
                x.Score =(float) Convert.ToDouble(dr["最大分值"].ToString());
                _xzt.Add(x);
            }
        }
        private void buttonSetScore_Click(object sender, EventArgs e)
        {
            float score = ReadScore();
            if (score < 0)
                return;
           
            int begin = comboBoxBegin.SelectedIndex;
            int end = comboBoxEnd.SelectedIndex + 1;
            if (begin == -1 || end == -1 || begin > end)
            {
                MessageBox.Show("请正确地选择起始题和末尾题");
                return ;
            }
            for (int i = begin; i < end; i++)
            {
                _xzt[i].Score = score;
            }
            ReFreshDgv();

        }

        private float ReadScore()
        {
            string text = textBoxScore.Text;
            bool bdot = false;
            if (text == "")
            {
                MessageBox.Show("请输入数值，并在0.5-100之间");
                return -1;
            }
            foreach (char c in text)
            {
                if (!Char.IsNumber(c))
                {
                    if (!bdot && c == '.')
                    {
                        bdot = true;
                    }
                    else
                    {
                        textBoxScore.Text = "";
                        MessageBox.Show("请输入数值，并在0.5-100之间");
                        return -1;
                    }
                }
            }
            float score = (float)Convert.ToDouble(text);
            if (score < 0.5 || score > 100)
            {
                textBoxScore.Text = "";
                MessageBox.Show("请输入数值，并在0.5-100之间");
                return -1;
            }
            return score;
        }        
        private void buttonImportAnswer_Click(object sender, EventArgs e)
        {
            List<int> la = new List<int>();
            string str = textBoxAnswer.Text.ToUpper();
            foreach (char c in str)
                if (c >= 'A' && c <= 'D')
                    la.Add(c - 'A');

            for (int i = 0; i < _xzt.Count && i < la.Count; i++)
            {
                _xzt[i].OptionAnswer =Convert.ToString(  Convert.ToChar (la[i]+'A') ) ;
            }
            str = "0";
            for (int i = 0;  i < la.Count;)
            {
                if (i + 10 < la.Count) 
                    str += (i + 1).ToString() + "-" + (i + 10).ToString()+" ";
                else
                    str += (i + 1).ToString() + "-" + la.Count.ToString()+" ";
                for (int j = 0; j < 10 && i < la.Count; j++, i++)
                {
                    str += Convert.ToChar(la[i] + 'A') + " ";
                    if (j == 4)
                        str += "   ";
                }
                str += "\r\n";
            }
            textBoxAnswer.Text = str;
            InitDgvAndCbx();
            ReFreshDgv();
        }
        private void buttonImportMultiAnswer_Click(object sender, EventArgs e)
        {
            List<int> la = new List<int>();
            string str = textBoxAnswer.Text.ToUpper();

            str = Regex.Replace(str, "[^A-Z ]", " ");
            List<string> list = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

            int begin = comboBoxBegin.SelectedIndex;
            int end = comboBoxEnd.SelectedIndex + 1;
            if (begin == -1 || end == -1 || begin > end)
            {
                MessageBox.Show("请正确地选择起始题和末尾题");
                return;
            }

            int index = 0;
            for (int i = begin; i < end  && index< list.Count; i++,index++)
            {
                if(_xzt[i].Type == "U" || _xzt[i].Type == "M" )
                    _xzt[i].OptionAnswer = list[index];
            }
           
            textBoxAnswer.Text = str;
            InitDgvAndCbx();
            ReFreshDgv();
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private void comboBoxBegin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBegin.SelectedIndex != -1 )//&& comboBoxBegin.SelectedIndex < comboBoxEnd.SelectedIndex)
                comboBoxEnd.SelectedIndex = comboBoxBegin.SelectedIndex;
        }       
        private void InitXZTQuestion(List<string> xztids)
        {
            _xzt = new List<XztQuestion>();
            foreach (string s in xztids)
            {
                if (s.StartsWith("xz"))
                {
                    XztQuestion x = new XztQuestion();
                    x.ID = Convert.ToInt32(s.Substring(2));
                    _xzt.Add(x);
                }
            }
        }
        private void ReFreshDgv()
        {
            for (int i = 0; i < _xzt.Count; i++)
            {
                dataGridViewAnswer[0, i].Value = _xzt[i].TypeName();
                dataGridViewAnswer[1, i].Value = _xzt[i].OptionAnswer;
                dataGridViewAnswer[2, i].Value = _xzt[i].Score;
                dataGridViewAnswer[3, i].Value = _xzt[i].HalfScore;
            }
        }
        private void InitDgvAndCbx()
        {
            InitDgv(_xzt.Count, dataGridViewAnswer);
            InitCbx(_xzt.Count, comboBoxBegin);
            InitCbx(_xzt.Count, comboBoxEnd);
            if (_xzt.Count != 0)
            {
                comboBoxBegin.SelectedIndex = 0;
                comboBoxEnd.SelectedIndex = _xzt.Count - 1;
            }
        }

        public List<XztQuestion> Xzt(){ return _xzt;}
        public bool CheckOK(){
            foreach (XztQuestion q in _xzt)
                if (q.OptionAnswer==null ||  !"ABCD".Contains(q.OptionAnswer) || q.Score <= 0)
                    return false;
            return true;
        }
        private List<XztQuestion> _xzt;
        private DataTable _dtsetxzt;

        public static void InitCbx(int questioncnt, ComboBox cbx)
        {
            cbx.Items.Clear();
            for (int i = 1; i < questioncnt + 1; i++)
                cbx.Items.Add(i);
        }
        public static bool isnumeric(string str)
        {
            char[] ch = new char[str.Length];
            ch = str.ToCharArray();
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] < 48 || ch[i] > 57)
                    return false;
            }
            return true;
        }
        private void buttonSetHalfScore_Click(object sender, EventArgs e)
        {
            float score = ReadScore();
            if (score < 0)
                return;

            int begin = comboBoxBegin.SelectedIndex;
            int end = comboBoxEnd.SelectedIndex + 1;
            if (begin == -1 || end == -1 || begin > end)
            {
                MessageBox.Show("请正确地选择起始题和末尾题");
                return;
            }
            for (int i = begin; i < end; i++)
            {
                if(_xzt[i].Type == "U")
                    _xzt[i].HalfScore = score;
            }
            ReFreshDgv();
        }
        private void buttonSetOptionType_Click(object sender, EventArgs e)
        {
            int begin = comboBoxBegin.SelectedIndex;
            int end = comboBoxEnd.SelectedIndex + 1;
            if (begin == -1 || end == -1 || begin > end)
            {
                MessageBox.Show("请正确地选择起始题和末尾题");
                return;
            }
            string Type = "S";
            if (comboBoxType.SelectedIndex != -1)
            {
                if (comboBoxType.SelectedItem.ToString() == "多选")
                    Type = "M";
                else if (comboBoxType.SelectedItem.ToString() == "不定项")
                    Type = "U";
            }

            for (int i = begin; i < end; i++)
            {
                _xzt[i].Type = Type;
            }
            ReFreshDgv();
        }
        private void FormSetscore_Load(object sender, EventArgs e)
        {
            comboBoxType.SelectedIndex = 0;
        }
    }
    
    public class XztQuestion
    {
        public int ID{ get; set; }
        public string OptionAnswer { get; set; }
        public float Score { get; set; }
        public string Type { get; set; }
        public float HalfScore { get; set; }

        public string TypeName()
        {
            if (Type == "S")
                return "单选";
            if (Type == "M")
                return "多选";
            if (Type == "U")
                return "不定项";
            Type = "S";
            return "单选";
        }
    }
}
