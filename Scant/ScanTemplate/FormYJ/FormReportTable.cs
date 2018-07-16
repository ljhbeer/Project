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
            SubjectShowOptionMode = true;          
            buttonShowOption.Enabled = false;
            ShowRightStudentMode = false;
            buttonShowErrorStudentList.Enabled = false;

            _ActiveStudent = null;
            
            InitOptionAnswer();
            Paperconstruct = new PaperConstruct(_exam);
            InitTitles();
            InitDataTalbes();
            InitDataTableData();
            dgvstudent.DataSource = _dtstudent;
            dgvsubjects.DataSource = _dtsubjectoption;
            dgvstudentrightsubjectid.DataSource = _dtstudentRightID;
            dgvstudenterrorsubjectid.DataSource = _dtstudentErrorID;
            dgvSubjectRightErrorStudentList.DataSource = _dtsubjectRightStudent;
            dgvsubjects.DataSource = _dtsubjectoption;
            InitDgvUI();

            _idbe = new IDbeTool();
            _idbe.B = 0;
            _idbe.E = 2;
            _idbe.MoveToTop();
        }
        private void InitOptionAnswer()
        {
            _Optionanswer = _exam.OSubjects.Select(r => r.Answer).ToList();
            _OptionMaxscore = _exam.OSubjects.Select(r => r.Score).ToList();
            _ABCD = new List<string>() { "A", "B", "C", "D" };
            List<string> _ABCDL = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N" };
            _dicABCDToOption = _ABCDL.ToDictionary(r => r, r => r[0] - 'A');
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

            _dtsubjectoption = Tools.DataTableTools.ConstructDataTable(new List<string> { "OID", "题号", "均分", "得分率", "错误人数", "正确人数" }.ToArray());
            _dtsubjectunchoose = Tools.DataTableTools.ConstructDataTable(new List<string> { "OID", "题号", "均分", "得分率", "错误人数", "正确人数" }.ToArray());
            _dtsubjectErrorStudent = Tools.DataTableTools.ConstructDataTable(studenttitle.ToArray());
            _dtsubjectRightStudent = Tools.DataTableTools.ConstructDataTable(studenttitle.ToArray());
        }
        private void InitDataTableData()
        {
            InitDtstudentData();
            InitDtsubjectoptionData();
            InitDtsubjectunchooseData();
        }
        private void InitDtstudentData()
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
        }
        private void InitDtsubjectoptionData()
        {

            foreach (Optionsubject O in _exam.OSubjects)
            {
                DataRow dr = _dtsubjectoption.NewRow();
                dr["OID"] = new ValueTag(O.ToString(), O);
                dr["题号"] = O.OutName;

                List<int> Iabcd = _ABCD.Select(r => _exam.Students.Where(rr => rr.SelectOption(r, O.Index)).Count()).ToList();
                // TODO: 不定项没有统计
                int okindex = _dicABCDToOption[_Optionanswer[O.Index].Substring(0, 1)]; //存在不定项
                int rightcnt = Iabcd[okindex];
                int count = _exam.Students.Count;
                Double rightrate = rightcnt * 1.0 / count;
                double avg = rightrate * O.Score;

                dr["均分"] = avg.ToString("0.00");
                dr["得分率"] = (rightrate * 100).ToString("0.0");
                dr["正确人数"] = rightcnt;
                dr["错误人数"] = count - rightcnt;
                _dtsubjectoption.Rows.Add(dr);
            }
            _dtsubjectoption.AcceptChanges();
        }
        private void InitDtsubjectunchooseData()
        {
            foreach (Imgsubject O in _exam.Subjects)
            {
                DataRow dr = _dtsubjectunchoose.NewRow();
                dr["OID"] = new ValueTag(O.ToString(), O);
                dr["题号"] = O.Name;
                List<int> or = _exam.SR.Result[O.Index];
                int rightcnt = or.Count(r => r > 0);
                int count = or.Count;
                double avg = or.Average();
                Double rightrate = avg / O.Score;

                dr["均分"] = avg.ToString("0.00");
                dr["得分率"] = (rightrate * 100).ToString("0.0");
                dr["正确人数"] = rightcnt;
                dr["错误人数"] = or.Count - rightcnt;
                _dtsubjectunchoose.Rows.Add(dr);
            }
            _dtsubjectunchoose.AcceptChanges();
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
            dgv.MultiSelect = false;
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
        private void dgvstudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            Student S = (Student)((ValueTag)dgv["OID", e.RowIndex].Value).Tag;
            if (S.TagInfor != "PR")
                return;
            PaperResult PR = (PaperResult)S.Tag;
            _ActiveStudent = S;
            pictureBox1.Image = ARTemplate.TemplateTools.DrawInfoBmp(S,_angle, PR,checkBoxOnlyShowerror.Checked);
            _dtstudentRightID.Rows.Clear();
            _dtstudentErrorID.Rows.Clear();
            int count = 0;
            foreach (ResultObj RO in PR.Options)
            {
                DataRow dr1;
                if (count < PR.OptionCount) //选择题
                    dr1 = _dtsubjectoption.Rows[RO.Index];
                else
                    dr1 = _dtsubjectunchoose.Rows[RO.Index];
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
                count++;
            }           
            _dtstudentErrorID.AcceptChanges();
            _dtstudentRightID.AcceptChanges();            
        }
        private void checkBoxOnlyShowerror_CheckedChanged(object sender, EventArgs e)
        {
            if (_ActiveStudent == null)
                return;
            Student S = _ActiveStudent;
            PaperResult PR = (PaperResult)S.Tag;
            pictureBox1.Image = ARTemplate.TemplateTools.DrawInfoBmp(S, _angle, PR, checkBoxOnlyShowerror.Checked);
        }        
        private void dgvsubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            _dtsubjectErrorStudent.Rows.Clear();
            _dtsubjectRightStudent.Rows.Clear();
            if (!SubjectShowOptionMode)
            {
                Imgsubject O = (Imgsubject)((ValueTag)dgv["OID", e.RowIndex].Value).Tag;
                dgvsubjecttodtclickedwithUnchoose(O);
            }
            else
            {
                Optionsubject O = (Optionsubject)((ValueTag)dgv["OID", e.RowIndex].Value).Tag;
                dgvsubjecttodtclickedwithOption(O);
            }
            _dtsubjectRightStudent.AcceptChanges();
            _dtsubjectErrorStudent.AcceptChanges();
            if (ShowRightStudentMode)
                dgvSubjectRightErrorStudentList.DataSource = _dtsubjectRightStudent;
            else
                dgvSubjectRightErrorStudentList.DataSource = _dtsubjectErrorStudent;
        }
        private void dgvsubjecttodtclickedwithOption(Optionsubject O)
        {
            for (int i = 0; i < _exam.Students.Count; i++)
            {
                Student S = _exam.Students[i];
                DataTable dt;
                if (S.SelectOption(O.Answer, O.Index)) //OK
                {
                    dt = _dtsubjectRightStudent;
                }
                else //Error
                {
                    dt = _dtsubjectErrorStudent;
                }
                DataRow dr = dt.NewRow();
                DataRow dr1 = _dtstudent.Rows[i];
                for (int index = 0; index < dr1.Table.Columns.Count; index++)
                    dr[index] = dr1[index];
                dt.Rows.Add(dr);
            }
        }
        private void dgvsubjecttodtclickedwithUnchoose(Imgsubject O)
        {
            for (int i = 0; i < _exam.Students.Count; i++)
            {
                Student S = _exam.Students[i];
                DataTable dt;
                if (_exam.SR.Result[O.Index][S.Index] > 0) //OK
                {
                    dt = _dtsubjectRightStudent;
                }
                else //Error
                {
                    dt = _dtsubjectErrorStudent;
                }
                DataRow dr = dt.NewRow();
                DataRow dr1 = _dtstudent.Rows[i];
                for (int index = 0; index < dr1.Table.Columns.Count; index++)
                    dr[index] = dr1[index];
                dt.Rows.Add(dr);
            }
        }
        //Click UnImplement
        private void dgvcorrectid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgverrorid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvRightErrorStudentList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //DoubleClick
        private void dgvstudenterrorsubjectid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            studentrighterrorsubjectDealDoubleClick(e, dgv,false);
        }
        private void dgvstudentrightsubjectid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            studentrighterrorsubjectDealDoubleClick(e, dgv, true);
        }
        private void studentrighterrorsubjectDealDoubleClick(DataGridViewCellEventArgs e, DataGridView dgv, bool righterror)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            if (e.ColumnIndex != 1) return;
            //1 转到按题,  2. 显示选择题/非选择题   2,  显示错误名单
            string Value = dgv[e.ColumnIndex, e.RowIndex].Value.ToString();

            SwitchTab();
            _dtsubjectErrorStudent.Rows.Clear();
            _dtsubjectRightStudent.Rows.Clear();
            if (Value.Contains("选择题")) //选择题
            {
                if (buttonShowOption.Enabled)
                    buttonShowOption.PerformClick();
                Optionsubject O = (Optionsubject)(((ValueTag)(dgv["OID", e.RowIndex].Value)).Tag);
                dgvsubjecttodtclickedwithOption(O);
                dgvsubjects[1, O.Index].Selected = true;
            }
            else //非选择题
            {
                if (buttonShowUnChoose.Enabled)
                    buttonShowUnChoose.PerformClick();
                Imgsubject O = (Imgsubject)(((ValueTag)(dgv["OID", e.RowIndex].Value)).Tag);
                dgvsubjecttodtclickedwithUnchoose(O);
              
                dgvsubjects[1, O.Index].Selected = true;
            }
            _dtsubjectRightStudent.AcceptChanges();
            _dtsubjectErrorStudent.AcceptChanges();

            if (righterror)
            {
                if (buttonShowRightStudentList.Enabled)
                    buttonShowRightStudentList.PerformClick();
            }
            else
            {
                if (buttonShowErrorStudentList.Enabled)
                    buttonShowErrorStudentList.PerformClick();
            }
        }

        private void dgvSubjectRightErrorStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            if (e.ColumnIndex != 1) return;
            SwitchTab();
            //1 转到按人,  2. 定位到人
            Student S = (Student)(((ValueTag)(dgv["OID", e.RowIndex].Value)).Tag);
            dgvstudent[1, S.Index].Selected = true;
            dgvstudent_CellClick(dgvstudent,
                new DataGridViewCellEventArgs(dgvstudent.SelectedCells[0].ColumnIndex, dgvstudent.SelectedCells[0].RowIndex));

        }
        private void SwitchTab()
        {
            _idbe.MoveCircleNext();
            tabControl1.SelectedIndex = _idbe.ActiveIndex;
        }
        private void buttonShowOption_Click(object sender, EventArgs e)
        {
            switchShowOptionUnChoose();
        }
        private void buttonShowUnChoose_Click(object sender, EventArgs e)
        {
            switchShowOptionUnChoose();
        }
        private void switchShowOptionUnChoose()
        {
            buttonShowOption.Enabled = !buttonShowOption.Enabled;
            buttonShowUnChoose.Enabled = !buttonShowUnChoose.Enabled;
            SubjectShowOptionMode = !SubjectShowOptionMode;
            if (SubjectShowOptionMode)
                dgvsubjects.DataSource = _dtsubjectoption;
            else
                dgvsubjects.DataSource = _dtsubjectunchoose;
        }
        private void buttonShowRightStudentList_Click(object sender, EventArgs e)
        {
            switchShowRightErrorStudent();
        }
        private void buttonShowErrorStudentList_Click(object sender, EventArgs e)
        {
            switchShowRightErrorStudent();
        }
        private void switchShowRightErrorStudent()
        {
            buttonShowErrorStudentList.Enabled = !buttonShowErrorStudentList.Enabled;
            buttonShowRightStudentList.Enabled = !buttonShowRightStudentList.Enabled;
            ShowRightStudentMode = !ShowRightStudentMode;
            if (ShowRightStudentMode)
                dgvSubjectRightErrorStudentList.DataSource = _dtsubjectRightStudent;
            else
                dgvSubjectRightErrorStudentList.DataSource = _dtsubjectErrorStudent;
        }

        private ScanConfig _sc;
        private Exam _exam;
        private Boolean SubjectShowOptionMode;
        private Boolean ShowRightStudentMode;
        private IDbeTool _idbe;
        //private Examdata _examdata;
        private DataTable _dtstudent;
        private DataTable _dtstudentErrorID;
        private DataTable _dtstudentRightID;
        private DataTable _dtsubjectoption;
        private DataTable _dtsubjectunchoose;
        private DataTable _dtsubjectErrorStudent;
        private DataTable _dtsubjectRightStudent;
        private List<string> _TzOptiontitle;
        private List<string> _Tztitle;
        private AutoAngle _angle;
        private List<string> _Optionanswer;
        private List<float> _OptionMaxscore;
        private List<string> _ABCD;
        private Dictionary<string, int> _dicABCDToOption;
        private Student _ActiveStudent;
        public PaperConstruct Paperconstruct { get; set; }

    }
}
