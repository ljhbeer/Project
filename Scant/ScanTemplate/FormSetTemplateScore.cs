using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ARTemplate;

namespace ScanTemplate
{
    public partial class FormSetTemplateScore : Form
    {
        private TreeNode m_tn;
        private DataTable _dtxzt;
        private DataTable _dtUnxzt;
        public FormSetTemplateScore(TreeNode m_tn)
        {
            this.m_tn = m_tn;
            InitializeComponent();
            InitCombobox();
            InitDatatable();
        }
        private void InitCombobox()
        {
            foreach (string s in new string[] { "选择题", "非选择题","题组"})
            {
                if (m_tn.Nodes.ContainsKey(s))
                    foreach (TreeNode t in m_tn.Nodes[s].Nodes)
                    {
                        if (t.Tag != null)
                        {
                            Area I = (Area)(t.Tag);
                            I.TypeName = s;
                            comboBox1.Items.Add(I);
                        }
                    }
            }

        }
        private void InitDatatable()
        {
            _dtxzt = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "题组名称", "最大分值", "正确答案" });
            _dtUnxzt = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "题组名称", "最大分值"});
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;
            Area I = (Area)comboBox1.SelectedItem;
            if (I.TypeName == "选择题")
            {
                SingleChoiceArea II = (SingleChoiceArea)I;
                _dtxzt.Rows.Clear();
                foreach (OptionAnswerScore O in II.Listanswerscore)
                {
                    DataRow dr = _dtxzt.NewRow();
                    dr["OID"] = new ValueTag(O.ID.ToString(),O);
                    dr["题组名称"] = O.OutName;
                    dr["最大分值"] = O.Score;
                    dr["正确答案"] = O.Answer;
                    _dtxzt.Rows.Add(dr);
                }
                _dtxzt.AcceptChanges();
                dataGridView1.DataSource = _dtxzt;

            }
            else if (I.TypeName == "题组")
            {
                TzArea II = (TzArea)I;
                _dtUnxzt.Rows.Clear();
                foreach(  UnChoose  U      in II.SubAreas)
                {
                    DataRow dr = _dtUnxzt.NewRow();
                    dr["OID"] = new ValueTag(U.Title.ToString(), U);
                    dr["题组名称"] = U.Name;
                    dr["最大分值"] = U.Scores;

                    _dtUnxzt.Rows.Add(dr);
                }
                _dtUnxzt.AcceptChanges();
                dataGridView1.DataSource = _dtUnxzt;
            }
            else if (I.TypeName == "非选择题")
            {
                UnChoose II = (UnChoose)I;
            }
        }
    }
}
