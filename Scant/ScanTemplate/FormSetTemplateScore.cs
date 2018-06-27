using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ARTemplate;
using System.Text.RegularExpressions;

namespace ScanTemplate
{
    public partial class FormSetTemplateScore : Form
    {
        private TreeNode m_tn;
        private DataTable _dtxzt;
        private DataTable _dtUnxzt;
        private FormTemplate formTemplate;
        private bool _InitDgv;
        public FormSetTemplateScore(TreeNode m_tn)
        {
            this.m_tn = m_tn;
            InitializeComponent();
            InitCombobox();
            InitDatatable();
        }

        public FormSetTemplateScore(TreeNode m_tn, FormTemplate formTemplate)
        {
            // TODO: Complete member initialization
            this.m_tn = m_tn;
            InitializeComponent();
            InitCombobox();
            InitDatatable();
            this.formTemplate = formTemplate;
        }
        private void InitCombobox()
        {
            comboBox1.Items.Add("选择题");
            comboBox1.Items.Add("非选择题");
        }
        private void InitDatatable()
        {
            _dtxzt = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "题组名称","题型", "最大分值","半对分值", "正确答案" });
            _dtUnxzt = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "题组名称", "最大分值" });
            foreach (string s in new string[] { "选择题", "非选择题", "题组" })
            {
                string keyname = s;
                if (m_tn.Nodes.ContainsKey(keyname))
                    foreach (TreeNode t in m_tn.Nodes[keyname].Nodes)
                    {
                        if (t.Tag != null)
                        {
                            if (keyname == "选择题")
                            {
                                foreach (DataRow dr in ConstructXztDataRow((Area)(t.Tag)))
                                {
                                    _dtxzt.Rows.Add(dr);                                   
                                }
                            }
                            else if (keyname == "题组")
                            {
                                foreach (DataRow dr in ConstructUnDataRow((Area)(t.Tag)))
                                    _dtUnxzt.Rows.Add(dr);
                            }
                            else if (keyname == "非选择题")
                            {
                                _dtxzt.Rows.Add(ConstructUnChooseDataRow((Area)(t.Tag)));
                            }
                        }
                    }
            }
            _dtUnxzt.AcceptChanges();
            _dtxzt.AcceptChanges();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;
            string TypeName = comboBox1.SelectedItem.ToString();
            if (TypeName == "选择题")
            {
                dgv.DataSource = _dtxzt;
                InitDgv();
            }
            else if (TypeName == "非选择题")
            {
                dgv.DataSource = _dtUnxzt;
                InitDgv();
            }
        }
        private void InitDgv()
        {
            _InitDgv = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersWidth = 75;
            dgv.Columns[0].Visible = false;
            for (int i = 1; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].Width = 45;
                dgv.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < dgv.RowCount; i++)
            {
                dgv.Rows[i].HeaderCell.Value = "第" + (i + 1).ToString() + "题";
            }
            _InitDgv = false;
        }
        private List<DataRow> ConstructUnDataRow(Area I)
        {
            TzArea II = (TzArea)I;
            List<DataRow> list = new List<DataRow>();
            foreach (UnChoose U in II.SubAreas)
            {
                DataRow dr = _dtUnxzt.NewRow();
                dr["OID"] = new ValueTag(U.Title.ToString(), U);
                dr["题组名称"] = U.Name;
                dr["最大分值"] = U.Scores;
                list.Add(dr);
            }
            return list;
        }
        private DataRow ConstructUnChooseDataRow(Area I)
        {
            UnChoose U = (UnChoose)I;
            DataRow dr = _dtUnxzt.NewRow();
            dr["OID"] = new ValueTag(U.Title.ToString(), U);
            RefreshDrUnXzt(dr);
            return dr;
        }
        private List<DataRow> ConstructXztDataRow(Area I)
        {
            SingleChoiceArea II = (SingleChoiceArea)I;
            List<DataRow> list = new List<DataRow>();
            foreach (OptionAnswerScore O in II.Listanswerscore)
            {
                DataRow dr = _dtxzt.NewRow();
                dr["OID"] = new ValueTag(O.ID.ToString(), O);
                RefreshDrXzt(dr);
                list.Add(dr);
            }
            return list;
        }
        private void buttonImportAnswer_Click(object sender, EventArgs e)
        {
            if (FormatImportAnswer.CheckActionStr(textBoxAnswer.Text))
            {
                FormatImportAnswer fia = new FormatImportAnswer(textBoxAnswer.Text);
                fia.IDBE.SetBEID(0, _dtxzt.Rows.Count);
                for (int i = fia.IDBE.Begin; i < fia.IDBE.End; i++)
                {
                    OptionAnswerScore O = (OptionAnswerScore)((ValueTag)(_dtxzt.Rows[i]["OID"])).Tag;
                    if (fia.Type.Type != "")
                    {
                        O.Type = fia.Type.Type;
                    }                    
                    if(fia.Score.Score>0)
                        O.Score = fia.Score.Score;
                    if (fia.Answer.ListAnswer.Count > i - fia.IDBE.Begin)
                        O.Answer = fia.Answer.ListAnswer[i - fia.IDBE.Begin];                    
                }
                formTemplate.RefreshPicture();
                ReFreshDgv();
            }
            else
            {
                ;
            }
        }
        private void ReFreshDgv()
        {
            if (comboBox1.SelectedItem.ToString() == "选择题")
                foreach (DataRow dr in _dtxzt.Rows)
                    RefreshDrXzt(dr);
            else if (comboBox1.SelectedItem.ToString() == "选择题")
                foreach (DataRow dr in _dtUnxzt.Rows)
                    RefreshDrUnXzt(dr);
        }

        private void RefreshDrUnXzt(DataRow dr)
        {
            UnChoose U = (UnChoose)((ValueTag)dr["OID"]).Tag;
            dr["题组名称"] = U.Name;
            dr["最大分值"] = U.Scores;
        }
        private void RefreshDrXzt(DataRow dr)
        {
            OptionAnswerScore O = (OptionAnswerScore)((ValueTag)(dr["OID"])).Tag;
            dr["题组名称"] = O.Name();
            dr["最大分值"] = O.Score;
            dr["正确答案"] = O.Answer;
            dr["题型"] = O.Type;
            if (O.Type != "单选")
                dr["半对分值"] = O.HalfScore;
        }
        private void buttonImportMultiAnswer_Click(object sender, EventArgs e)
        {
            textBoxAnswer.Text = FormatImportAnswer.FormatAnswer(textBoxAnswer.Text);
        }
        private void FormSetTemplateScore_Load(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1 || _InitDgv) return;
            string columnName = dgv.Columns[e.ColumnIndex].Name;
            string str = dgv[e.ColumnIndex,e.RowIndex].Value.ToString();
            if (comboBox1.SelectedItem.ToString()=="选择题") {

                DataRow dr = _dtxzt.Rows[e.RowIndex];
                OptionAnswerScore O = (OptionAnswerScore)((ValueTag)(dr["OID"])).Tag;
                if (columnName == "最大分值")
                {
                    O.Score = Convert.ToSingle(str);
                }
                else if (columnName == "半对分值")
                {
                    O.HalfScore = Convert.ToSingle(str);
                }
                else if (columnName == "正确答案")
                {
                    str = str.ToUpper();
                    str = Regex.Replace(str, "[^A-D]", "");
                    //if(str!="")
                    O.Answer = str;
                }
                RefreshDrXzt(dr);
            }
            else if (comboBox1.SelectedItem.ToString() == "非选择题")
            {
                DataRow dr = _dtUnxzt.Rows[e.RowIndex];
                UnChoose U = (UnChoose)((ValueTag)dr["OID"]).Tag;
                if (columnName == "最大分值")
                    U.SetScore(Convert.ToSingle(str));
                RefreshDrUnXzt(dr);
            }
            formTemplate.RefreshPicture();
        }
    }
    public class FormatImportAnswer
    {// [题号][分值][题型][答案]
        public FormatImportAnswer(string actstr)
        {
            Regex re = new Regex("\\[([-0-9]*)\\]\\[([.,0-9]*)\\]\\[([DMUX]*)\\]\\[([^\\[\\]]*)\\]",RegexOptions.Multiline);
            Match mc = re.Match(actstr);
            string th = mc.Groups[1].Value;
            string fz = mc.Groups[2].Value;
            string tx = mc.Groups[3].Value;
            string an = mc.Groups[4].Value;

            IDBE = new IDBeginEnd(th);
            Score = new FIScore(fz);
            Type = new FIChooseType(tx);
            Answer = new FIAnswer(an);
          
        }
        public static bool CheckActionStr(string actstr)
        {
            Regex re = new Regex("\\[([-0-9]*)\\]\\[([.,0-9]*)\\]\\[([DMUX]*)\\]\\[([^\\[\\]]*)\\]");

            if (re.IsMatch(actstr))
                return true;
            return false;
        }
        public static string FormatAnswer(string str)
        {
            str = str.Trim().ToUpper();
            if (str.StartsWith("MU")) //多选
            {
                return FormatMultAnswer(str);              
            }else{ //单选
                return  FormatSingleAnswer(str);
            }
        }
        private static string FormatMultAnswer(string str)
        {
            str = Regex.Replace(str, "[^A-D ]", " ");
            List<string> list = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
             
            str = "0";
            for (int i = 0; i < list.Count; )
            {
                if (i + 10 < list.Count)
                    str += (i + 1).ToString() + "-" + (i + 10).ToString() + " ";
                else
                    str += (i + 1).ToString() + "-" + list.Count.ToString() + " ";
                for (int j = 0; j < 10 && i < list.Count; j++, i++)
                {
                    str += list[i]+" ";
                    if (j == 4)
                        str += "   ";
                }
                str += "\r\n";
            }
            return str;
        }
        private static string FormatSingleAnswer(string str)
        {
            List<int> la = new List<int>();
            foreach (char c in str)
                if (c >= 'A' && c <= 'D')
                    la.Add(c - 'A');

            str = "0";
            for (int i = 0; i < la.Count; )
            {
                if (i + 10 < la.Count)
                    str += (i + 1).ToString() + "-" + (i + 10).ToString() + " ";
                else
                    str += (i + 1).ToString() + "-" + la.Count.ToString() + " ";
                for (int j = 0; j < 10 && i < la.Count; j++, i++)
                {
                    str += Convert.ToChar(la[i] + 'A') + " ";
                    if (j == 4)
                        str += "   ";
                }
                str += "\r\n";
            }
            return str;
        }
        public IDBeginEnd IDBE;
        public FIScore Score;
        public FIChooseType Type;
        public FIAnswer Answer;
        public class IDBeginEnd
        {
            public IDBeginEnd(string s)
            {
            }
            public int Begin;
            public int End;
            public int Mode;
            public void SetBEID(int b, int e)
            {
                Begin = b;
                End = e;
            }
        }
        public class FIScore
        {
            public FIScore(string s)
            {
                Score = HalfScore = 0;
                if (s.Contains(","))
                {
                    Score = Convert.ToSingle(s.Substring(0,s.IndexOf(",")));
                    HalfScore = Convert.ToSingle(s.Substring( s.IndexOf(",")+1));
                }
                else
                {
                    Score = Convert.ToSingle(s);
                }
            }
            public float Score;
            public float HalfScore;
        }
        public class FIChooseType
        {
            public FIChooseType(string s) //  S M U
            {
                if (s == "多选")
                    Type = "M";
                else if (s == "不定项")
                {
                    Type = "U";
                }
                else if (s == "单选")
                    Type = "S";
                else
                    Type = "";

            }
            public string Type;
        }
        public class FIAnswer
        {
            public FIAnswer(string s)
            {
                string str = Regex.Replace(s, "[^A-D ]", " ");
                ListAnswer = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            public List<string> ListAnswer;
        }
    }
}
