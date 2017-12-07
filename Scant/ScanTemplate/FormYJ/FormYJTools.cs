using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ScanTemplate.FormYJ
{
    public partial class FormYJTools : Form
    {
        private string _workpath;
        public FormYJTools()
        {
            InitializeComponent();
            _workpath = textBoxWorkPath.Text;			
        }
        private void FormYJTools_Load(object sender, EventArgs e)
        {
            InitExamInfos();
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            InitExamInfos();
        }
        private void InitExamInfos()
        {
            string exampath = _workpath.Substring(0, _workpath.LastIndexOf("\\")) + "\\Exam\\";
            string idindexpath = exampath + "config.json";
            if (!File.Exists(idindexpath))
                return;
            Config g = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(idindexpath));
            listBox1.Items.AddRange(g._examinfo.ToArray());
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            ExamInfo ei = (ExamInfo)listBox1.SelectedItem;
            string filename = ei.Path.Substring(0, ei.Path.Length - ei.Number.ToString().Length - 1) + ei.Name + ".json";
            if(!File.Exists(filename))
                return;
            Exam exam = Newtonsoft.Json.JsonConvert.DeserializeObject<Exam>(File.ReadAllText(filename));

            MessageBox.Show(exam.Name);

        }
        private void buttonModifyData_Click(object sender, EventArgs e)
        {

        }
        private void buttonBeginYJ_Click(object sender, EventArgs e)
        {

        }
        private void buttonShowXztSet_Click(object sender, EventArgs e)
        {

        }
        private void buttonShowFXztSet_Click(object sender, EventArgs e)
        {

        }
        private void buttonImportOptionAnswerScore_Click(object sender, EventArgs e)
        {

        }


    }
}
