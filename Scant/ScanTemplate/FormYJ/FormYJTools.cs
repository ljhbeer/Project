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
            Examdata  examdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Examdata >(File.ReadAllText(filename));

            examdata.SR._Students.InitDeserialize();
            examdata.SR._Imgsubjects.InitDeserialize();
            for (int index = 0; index < examdata.SR._Imgsubjects.Subjects.Count; index++)
            {
                examdata.SR._Imgsubjects.Subjects[index].Index = index;
            }
            Exam exam = new Exam(examdata);
            //TODO: NextWork
            FormFullScreenYJ fs = new FormFullScreenYJ(exam);
            this.Hide();
            fs.ShowDialog();
            this.Show();

            MessageBox.Show(examdata.Name + examdata.Path);
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
    public class Examdata
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public StudentsResultData SR { get; set;}       
    }
    public class StudentsResultData
    {
        public string _workpath;
        public Students _Students;
        public Imgsubjects _Imgsubjects;
        public List<List<int>> _Result;
    }
}
