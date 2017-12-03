using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AR
{
    public partial class FormSetscore : Form
    {
        public FormSetscore(Config config)
        {
            InitializeComponent();
            buttonClearanswer.Visible = false;           
            //this.config = config;
            this.answer = config.Answer;
            InitDgvAndCbx();
            ReFreshDgv();            
        }
        public void InitDgv(int questioncnt, DataGridView dgv) // used by settypeform
        {
            string[] strxq = new string[] { "题型", "答案", "分值" };
            dgv.RowCount = questioncnt > 0 ? questioncnt : 1;
            dgv.ColumnCount = 3;
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
        private void buttonImportAnswer_Click(object sender, EventArgs e)
        {
            List<int> la = new List<int>();
            string str = textBoxAnswer.Text.ToUpper();
            foreach (char c in str)
                if (c >= 'A' && c <= 'D')
                    la.Add(c - 'A');

            for (int i = 0; i < answer.Count && i < la.Count; i++)
            {
                answer.SetAnswer(i, la[i]);
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
        private void buttonClearanswer_Click(object sender, EventArgs e)
        {
            //dataGridViewAnswer.RowCount = 1;
            //answer.Clear();
            //dataGridViewAnswer[0, 0].Value = "";
            //dataGridViewAnswer[1, 0].Value = "";
            //dataGridViewAnswer[2, 0].Value = "";
            //dataGridViewAnswer.Rows[0].HeaderCell.Value = "";
            // dataGridViewAnswer[3, 0].Value = "";
        }       
        private void comboBoxBegin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBegin.SelectedIndex != -1 )//&& comboBoxBegin.SelectedIndex < comboBoxEnd.SelectedIndex)
                comboBoxEnd.SelectedIndex = comboBoxBegin.SelectedIndex;
        }
        private void buttonSetScore_Click(object sender, EventArgs e)
        {
            string text = textBoxScore.Text;
            bool bdot = false;
            if (text == "") return;
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
                        return;
                    }
                }
            }
            float score = (float)Convert.ToDouble(text);
            if (score < 0.5 || score > 100)
            {
                textBoxScore.Text = "";
                MessageBox.Show("请输入数值，并在0.5-100之间");
                return;
            }
            int begin = comboBoxBegin.SelectedIndex;
            int end = comboBoxEnd.SelectedIndex + 1;
            if (begin == -1 || end == -1 || begin > end)
            {
                MessageBox.Show("请正确地选择起始题和末尾题");
                return;
            }
            for (int i = begin; i < end; i++)
            {
                answer.SetMaxScore(i, score);
            }
            ReFreshDgv();

        }        
        private void buttonImportFromXml_Click(object sender, EventArgs e)
        {
            //OpenFileDialog OpenFileDialog2 = new OpenFileDialog();
            //OpenFileDialog2.FileName = "OpenFileDialog2";
            //OpenFileDialog2.Filter = "Xml files (*.xml)|*.xml";
            //OpenFileDialog2.Title = "Open xml file";
            //if (OpenFileDialog2.ShowDialog() == DialogResult.OK)
            //{
            //    Answer an = new Answer();
            //    if (an.Load(OpenFileDialog2.FileName))
            //    {
            //        if (answer.CheckFit(an))
            //        {
            //            answer.Update(an);
            //            ReFreshDgv();
            //        }
            //    }
            //}
        }
        private void ReFreshDgv()
        {
            for (int i = 0; i < answer.Count; i++)
            {
                if (answer.GetType(i) == QuestionType.SingleChoice)
                {
                    dataGridViewAnswer[0, i].Value = "单选题";
                    if(answer.GetOptionAnswer(i)>=0)
                    dataGridViewAnswer[1, i].Value = Convert.ToChar(answer.GetOptionAnswer(i) + 'A');
                    else
                        dataGridViewAnswer[1, i].Value = "";
                }
                else
                {
                    dataGridViewAnswer[0, i].Value = "非选择题";
                    dataGridViewAnswer[1, i].Value = "--";
                }
                dataGridViewAnswer[2, i].Value = answer.GetMaxScore(i);
            }
        }
        private void InitDgvAndCbx()
        {
            InitDgv(answer.Count, dataGridViewAnswer);
            InitCbx(answer.Count, comboBoxBegin);
            InitCbx(answer.Count, comboBoxEnd);
            if (answer.Count != 0)
            {
                comboBoxBegin.SelectedIndex = 0;
                comboBoxEnd.SelectedIndex = answer.Count - 1;
            }
        }
        private Answer answer;
    }
}
