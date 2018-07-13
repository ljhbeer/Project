using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScanTemplate.FormYJ;
using Tools;

namespace ScanTemplate
{
    public partial class FormReportTable : Form
    {
        //public FormReportTable(ScanConfig _sc,  Examdata _examdata, ARTemplate.Template _template)
        public FormReportTable(ScanConfig _sc, Exam _exam, AutoAngle _angle)
        {
            InitializeComponent();
            this._angle = _angle;
            this._sc = _sc;
            this._exam = _exam;
            Paperconstruct = new PaperConstruct(_exam);
            InitTitles();
            InitDataTalbes();
            InitDataTableData();
            dgvstudent.DataSource = _dtstudent;
            dgvsubjects.DataSource = _dtsubject;
            dgvstudentrightsubjectid.DataSource = _dtstudentRightID;
            dgvstudenterrorsubjectid.DataSource = _dtstudentErrorID;
            dgvSubjectRightErrorStudentList.DataSource = _dtsubjectRightStudent;
            InitDgvUI();
        }
        private void InitTitles()
        {
            _Tztitle = new List<string>();
            foreach (Tzsubject T in _exam.TzSubjects.Tzs)
            {
                _Tztitle.Add(T.Name);
            }

            _TzOptiontitle = new List<string>();
            if (_exam.TzOptionsubjects != null)
                foreach (TzOptionsubject T in _exam.TzOptionsubjects.Tzs)
                {
                    _TzOptiontitle.Add(T.Name);
                }
        }
        private void InitDataTalbes()
        {
            List<string> studenttitle = new List<string> { "OID", "姓名", "总分", "选择题" };
            studenttitle.AddRange(_Tztitle);
            studenttitle.AddRange(_TzOptiontitle);
            _dtstudent = Tools.DataTableTools.ConstructDataTable(studenttitle.ToArray());
            _dtstudentErrorID = Tools.DataTableTools.ConstructDataTable(new List<string> { "OID", "题号", "均分", "得分率", "错误人数", "正确人数" }.ToArray());
            _dtstudentRightID = Tools.DataTableTools.ConstructDataTable(new List<string> { "OID", "题号", "均分", "得分率", "错误人数", "正确人数" }.ToArray());

            _dtsubject = Tools.DataTableTools.ConstructDataTable(new List<string> { "OID", "题号", "均分", "得分率", "错误人数", "正确人数" }.ToArray());
            _dtsubjectErrorStudent = Tools.DataTableTools.ConstructDataTable(studenttitle.ToArray());
            _dtsubjectRightStudent = Tools.DataTableTools.ConstructDataTable(studenttitle.ToArray());
        }
        private void InitDataTableData()
        {
            foreach (Student S in _exam.Students)
            {
                DataRow dr = _dtstudent.NewRow();
                dr["OID"] = new ValueTag(S.ID.ToString(), S);
                dr["姓名"] = S.Name;
                PaperResult pr = Paperconstruct.ConstructPaperResult(S);
                S.TagInfor = "PR";
                S.Tag = pr;
                dr["总分"] = pr.TotalScore();
                dr["选择题"] = pr.Xzt.Floatscore;
               
                int index = 0;
                foreach (Tzsubject T in _exam.TzSubjects.Tzs)
                {
                    //if (T.Name == pr.Tz[index].Txt)
                        dr[T.Name] = pr.Tz[index].Floatscore;
                    index++;
                }
                if (_exam.TzOptionsubjects != null)
                    foreach (TzOptionsubject T in _exam.TzOptionsubjects.Tzs)
                    {
                        dr[T.Name] = pr.TotalXztTz(T);
                    }
                _dtstudent.Rows.Add(dr);
            }
            _dtstudent.AcceptChanges();

            foreach (Optionsubject O in _exam.OSubjects)
            {
                DataRow dr = _dtsubject.NewRow();
                dr["OID"] = new ValueTag(O.ToString(), O);
                dr["题号"] = O.OutName;
                List<int> or = _exam.SR.Result[O.Index];
                int rightcnt = or.Count(r => r > 0);
                int count = or.Count;
                double avg = or.Average();
                Double rightrate = avg / O.Score;

                dr["均分"] = avg.ToString("0.00");
                dr["得分率"] = (rightrate*100).ToString("0.0");
                dr["正确人数"] = rightcnt;
                dr["错误人数"] = or.Count - rightcnt;
                _dtsubject.Rows.Add(dr);
            }
            _dtsubject.AcceptChanges();
        }
        private void InitDgvUI()
        {
            InitDgvUI(dgvstudent,"student");
            InitDgvUI(dgvsubjects,"subject");
            InitDgvUI(dgvstudenterrorsubjectid, "errorsubject");
            InitDgvUI(dgvstudentrightsubjectid, "rightsubject");
            InitDgvUI(dgvSubjectRightErrorStudentList, "righterrorstudent");
        }
        private void InitDgvUI(DataGridView dgv, string _type="",DataTable _dt=null)
        {
            dgv.RowHeadersVisible = false;
            foreach (DataGridViewColumn dc in dgv.Columns)
                if (dc.Name == "OID")
                    dc.Visible = false;
                else if (dc.Name == "序号")
                    dc.Width = 30;
                else
                    dc.Width = 40;
            if (_type == "student" || _type.EndsWith("student"))
            {
                dgv.Columns["姓名"].Width = 50;
            }
            else if (_type == "subject"  || _type.EndsWith("subject"))
            {
                dgv.Columns["题号"].Width =60;
            }else if (_type == "选择题")
            {
                dgv.RowTemplate.Height = 30;
                dgv.DataSource = null;
                dgv.DataSource = _dt;
                foreach (DataGridViewColumn dc in dgv.Columns)
                    if (dc.Name.Contains("图片"))
                        ((DataGridViewImageColumn)(dc)).ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgv.Columns["题号"].Width = 30;
            }          
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void dgvstudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == -1 && e.RowIndex == -1) return;
            Student S = (Student)((ValueTag)dgv["OID", e.RowIndex].Value).Tag;
            if (S.TagInfor != "PR")
                return;
            PaperResult PR = (PaperResult)S.Tag;

            pictureBox1.Image = ARTemplate.TemplateTools.DrawInfoBmp(S,_angle, PR);
            _dtstudentRightID.Rows.Clear();
            _dtstudentErrorID.Rows.Clear();
            int count = 0;
            foreach (ResultObj RO in PR.Options) //UNdo Unchoose
            {
                DataRow dr1;
                if (count < _exam.OSubjects.Count)
                    dr1 = _dtsubject.Rows[RO.Index];
                else
                    break;
                count++; 
                if (RO.Score == 0)
                {
                    DataRow dr = _dtstudentErrorID.NewRow();
                    for (int index = 0; index < dr1.Table.Columns.Count; index++)
                        dr[index] = dr1[index];
                    _dtstudentErrorID.Rows.Add(dr);
                }
                else
                {
                    DataRow dr = _dtstudentRightID.NewRow();
                    for (int index = 0; index < dr1.Table.Columns.Count; index++)
                        dr[index] = dr1[index];
                    _dtstudentRightID.Rows.Add(dr);
                }
            }
            _dtstudentErrorID.AcceptChanges();
            _dtstudentRightID.AcceptChanges();
            
        }
        private void dgvcorrectid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgverrorid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void checkBoxOnlyShowerror_CheckedChanged(object sender, EventArgs e)
        {

        }        
        ///////////////////////////////////////////
        private void dgvsubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvRightErrorStudentList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void buttonShowOption_Click(object sender, EventArgs e)
        {

        }
        private void buttonShowUnChoose_Click(object sender, EventArgs e)
        {

        }
        private void buttonShowCorrectStudentList_Click(object sender, EventArgs e)
        {

        }
        private void buttonShowerrorList_Click(object sender, EventArgs e)
        {

        }

        private ScanConfig _sc;
        private Exam _exam;
        //private Examdata _examdata;
        private DataTable _dtstudent;
        private DataTable _dtstudentErrorID;
        private DataTable _dtstudentRightID;
        private DataTable _dtsubject;
        private DataTable _dtsubjectErrorStudent;
        private DataTable _dtsubjectRightStudent;
        private List<string> _TzOptiontitle;
        private List<string> _Tztitle;
        private AutoAngle _angle;
        public PaperConstruct Paperconstruct { get; set; }
    }
}
