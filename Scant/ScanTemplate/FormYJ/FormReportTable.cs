using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScanTemplate.FormYJ;

namespace ScanTemplate
{
    public partial class FormReportTable : Form
    {
        public FormReportTable(ScanConfig _sc,  Examdata _examdata, ARTemplate.Template _template)
        {
            InitializeComponent();
            this._sc = _sc;
            this._examdata = _examdata;
            //this._template = _template;
            InitTitles();
            InitDataTalbes();
            InitDataTableData();
            dgvstudent.DataSource = _dtstudent;
            dgvsubjects.DataSource = _dtsubject;
            InitDgvUI();
        }
        private void InitTitles()
        {
            _Tztitle = new List<string>();
            foreach (Tzsubject T in _examdata.SR._Tzsubjects.Tzs)
            {
                _Tztitle.Add(T.Name);
            }

            _TzOptiontitle = new List<string>();
            if (_examdata.SR._TzOptionsubjects != null)
                foreach (TzOptionsubject T in _examdata.SR._TzOptionsubjects.Tzs)
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
            foreach (Student S in _examdata.SR._Students.students)
            {
                DataRow dr = _dtstudent.NewRow();
                dr["OID"] = new ValueTag(S.ID.ToString(), S);
                dr["姓名"] = S.Name;

                PaperResult pr = ConstructPaperResult(S);
                //sblistscore.AppendLine(showName + "," + pr.TotalScore());
                //sblisttizu.AppendLine(showName + "," + pr.TotalTz() + pr.TotalXztTz(_TzOptionsubjects));
                //sbdetail.AppendLine(showName + "," + pr.Detail());
                dr["总分"] = 0;

                _dtstudent.Rows.Add(dr);
            }
            _dtstudent.AcceptChanges();

            foreach (Optionsubject O in _examdata.SR._Optionsubjects.OptionSubjects)
            {
                DataRow dr = _dtsubject.NewRow();
                dr["OID"] = new ValueTag(O.ToString(), O);
                dr["题号"] = O.OutName;
                dr["均分"] = 0;
                dr["得分率"] = 0;
                dr["正确人数"] = 0;
                dr["错误人数"] = 0;
                _dtsubject.Rows.Add(dr);
            }
            _dtsubject.AcceptChanges();
        }
        private void InitDgvUI()
        {
            InitDgvUI(dgvstudent,"student");
            InitDgvUI(dgvsubjects,"subject");
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
            if (_type == "student")
            {
                dgv.Columns["姓名"].Width = 50;
            }
            else if (_type == "subject")
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
        private Examdata _examdata;
        private DataTable _dtstudent;
        private DataTable _dtstudentErrorID;
        private DataTable _dtstudentRightID;
        private DataTable _dtsubject;
        private DataTable _dtsubjectErrorStudent;
        private DataTable _dtsubjectRightStudent;
        private List<string> _TzOptiontitle;
        private List<string> _Tztitle;
    }
}
