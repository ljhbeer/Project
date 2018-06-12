﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ARTemplate;
using Tools;

namespace ScanTemplate.FormYJ
{
    public partial class FormYJTools : Form
    {
        private ScanConfig _sc;
        ExamConfig g;
        public FormYJTools()
        {
            InitializeComponent();
            _workpath = textBoxWorkPath.Text;
            _exam = null;
            _template = null;
            _src = null;
            this._activeitem = null;
            _bexamdatamodified = false;
        }
        private void FormYJTools_Load(object sender, EventArgs e)
        {
            _sc = new ScanConfig(textBoxWorkPath.Text);
            _sc.Examconfig = new ExamConfig();
            _sc.Examconfig.SetWorkPath( _sc.Baseconfig.ScanDataPath  .Replace("s1025","Exam"));
            g = _sc.Examconfig;
            InitExamInfos();
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            InitExamInfos();
        }
        private void InitExamInfos()
        {
            string idindexpath = _sc.Baseconfig.ExamPath + "\\config.json";
            if (!File.Exists(idindexpath))
                return;
            ExamConfig g = Newtonsoft.Json.JsonConvert.DeserializeObject<ExamConfig>(File.ReadAllText(idindexpath));
            listBox1.Items.AddRange(g._examinfo.ToArray());
        }
        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                if (listBox1.SelectedIndex == -1) return;
                if (_examdata.SR._Tzsubjects.Tzs.Count > 0)
                    return;
                ExamInfo ei = (ExamInfo)listBox1.SelectedItem;
                //////////////////////////////////////
                Tzsubjects _Tzsubjects = new Tzsubjects();
                foreach (Area I in _template.Manageareas.Tzareas.list)
                {
                    Tzsubject tzs = new Tzsubject();
                    tzs.Name = I.Title;
                    tzs.Rect = I.Rect;
                    _Tzsubjects.Add(tzs);
                    if (I.HasSubAreas())
                        foreach ( Imgsubject S in _examdata.SR._Imgsubjects.Subjects)
                        { 
                            if ( tzs.Rect.Contains( S.Rect) )
                            {
                                tzs.Add(S);
                            }
                        }
                }
                _examdata.SR._Tzsubjects = _Tzsubjects;
                _Tzsubjects.InitIds();

                string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(_examdata));
                File.WriteAllText(g.ExamPath + "\\" + ei.Name + ".json", str);
                MessageBox.Show("已更新Exam文件");
            }
            else if (e.KeyCode == Keys.Delete)
            { //删除考试数据
                if (listBox1.SelectedIndex == -1) return;
                ExamInfo ei = (ExamInfo)listBox1.SelectedItem;  
                FormInput f = new FormInput("删除确认");
                f.ShowDialog();
                if (f.StrValue==null || f.StrValue != "Delete"+ei.Name)
                    return;  
           
                File.Delete(g.ExamPath + "\\" + ei.Name + ".json");
                if (Directory.Exists(ei.Path))
                    Directory.Delete(ei.Path, true);                
                foreach (ExamInfo exin in g._examinfo)
                {
                    if (exin.Path == ei.Path)
                    {
                        g._examinfo.Remove(exin);
                        break;
                    }
                }
                g.SaveConfig( );
                //某些清理工作
                buttonRefresh.PerformClick();
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;

            ExamInfo ei = (ExamInfo)listBox1.SelectedItem;
            if (_activeitem == ei)
                return;
            _activeitem = ei;
            _bexamdatamodified = false;
            string filename = ei.Path.Substring(0, ei.Path.Length - ei.Number.ToString().Length - 1) + ei.Name + ".json";
            if (!File.Exists(filename))
                return;
            _template = null;
            _src = null;
            _students = null;
            if (File.Exists(ei.TemplateFileName))
                _template = Templates.GetTemplate(ei.TemplateFileName);
            _examdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Examdata>(File.ReadAllText(filename));           
            _examdata.InitDeserialize();
           
            _students = _examdata.SR._Students;
            if(_students.students.Count>0){
                InitSrc(_template, _students.students[0]);
            }
            _exam = new Exam(_examdata);

            InitDgvUI();
            AddChooseTodtset(ref _dtsetxzt);
            AddUnChooseTodtset(ref _dtsetfxzt);
            AddStudentsdtset(ref _dtsetstudents);
            InitDgvSetUI(true);

        }
        private void InitSrc(Template _artemplate,Student S)
        {
            this._src = null;
            if (S !=null)
            {
                _artemplate.Angle.SetPaper(S.Angle);               
                _src = (Bitmap)Bitmap.FromFile(S.ImgFilename);
                _src = _src.Clone(S.SrcCorrectRect, _src.PixelFormat);
                pictureBox1.Image = TemplateTools.DrawInfoBmp(_src, _template,_template.Angle);
            }
        }
        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_bshowstudent) return;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            Student S =(Student) ((ValueTag)dgv[0, e.RowIndex].Value).Tag;

            AutoAngle Angle = _template.Angle;
            if (Angle != null && S!=null)
                Angle.SetPaper(S.Angle);
            pictureBox1.Image = TemplateTools.DrawInfoBmp(S.Src.Clone( S.SrcCorrectRect,S.Src.PixelFormat) , _template, Angle);
        }
       
        private void buttonExportResult_Click(object sender, EventArgs e)
        {
            if (_activeitem == null) return;
            FormChooseResult f = new FormChooseResult();
            f.ShowDialog();
            ExportClassResult ecr = new ExportClassResult(_sc,_examdata,_template);
            ecr.Export(f.Result);
            f = null;
        }
        private void SaveYjData()
        {
            if (_activeitem != null)
            {
                CheckDataTable(); // 修改 选择题的答案和分值， 以及非选择题的分值，以及添加考生
                //保存当前数据
                if (_bexamdatamodified)
                {
                    //MessageBox.Show("是否保存当前数据");
                    String Name = ((ExamInfo)_activeitem).Path + ((ExamInfo)_activeitem).Name;
                    Name = Name.Replace(((ExamInfo)_activeitem).Number + "\\", "");
                    string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(_examdata));
                    File.WriteAllText(Name + ".json", str);
                    MessageBox.Show("已保存当前数据");
                    _bexamdatamodified = false;
                }
            }
        }
        private void buttonBeginYJ_Click(object sender, EventArgs e)
        {
            if (_exam == null ) return;
            FormFullScreenYJ fs = new FormFullScreenYJ(_exam);
            this.Hide();
            fs.ShowDialog();
            _bexamdatamodified = fs.Modified;
            SaveYjData();
            this.Show();
        }
        private void buttonShowXztSet_Click(object sender, EventArgs e)
        {
            InitDgvSetUI(true);
        }
        private void buttonShowFXztSet_Click(object sender, EventArgs e)
        {
            InitDgvSetUI(false);
        }
        private void buttonShowStudents_Click(object sender, EventArgs e)
        {
            dgv.RowTemplate.Height = 24;
            dgv.DataSource = null;
            dgv.DataSource = _dtsetstudents;
            _bshowstudent = true;
            foreach (DataGridViewColumn dc in dgv.Columns)
                if (dc.Name.Contains("考号"))
                    dc.Width = 75;
                else
                    dc.Width = 40;
        }
        private void buttonImportOptionAnswerScore_Click(object sender, EventArgs e)
        {
           FormYJInit.ImportOptionAnswerScore(_dtsetxzt);
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }
        private void pictureBox1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            double f = 0.0;
            if (numberOfTextLinesToMove > 0)
            {
                for (int i = 0; i < numberOfTextLinesToMove; i++)
                {
                    f += 0.05;
                }
                Zoomrat(f + 1, e.Location);
            }
            else if (numberOfTextLinesToMove < 0)
            {
                for (int i = 0; i > numberOfTextLinesToMove; i--)
                {
                    f -= 0.05;
                }
                Zoomrat(f + 1, e.Location);
            }
        }
        private void Zoomrat(double rat, Point e)
        {
            Bitmap bitmap_show = (Bitmap)pictureBox1.Image;
            Point L = pictureBox1.Location;
            Point S = panel3.AutoScrollPosition;
            int w = (int)(pictureBox1.Width * rat);
            int h = w * bitmap_show.Height / bitmap_show.Width;
            L.Offset((int)(e.X * (rat - 1)), (int)(e.Y * (rat - 1)));
            pictureBox1.SetBounds(S.X, S.Y, w, h);
            //zoombox.UpdateBoxScale(pictureBox1);
            S.Offset((int)(e.X * (1 - rat)), (int)(e.Y * (1 - rat)));
            panel3.Invalidate();
            panel3.AutoScrollPosition = new Point(-S.X, -S.Y);
        }
        private void InitDgvSetUI(bool xzt)
        {
            _bshowstudent = false;
            dgv.DataSource = null;
            if (xzt)
            {
                dgv.RowTemplate.Height = 24;
                dgv.DataSource = _dtsetxzt;
            }
            else
            {
                dgv.RowTemplate.Height = _AvgUnImgHeight;
                dgv.DataSource = _dtsetfxzt;
            }

            foreach (DataGridViewColumn dc in dgv.Columns)
                if (dc.Name.Contains("图片"))
                {
                    dc.Width = _AvgUnImgWith;
                    ((DataGridViewImageColumn)dc).ImageLayout = DataGridViewImageCellLayout.Zoom;
                }
                else
                {
                    dc.Width = 40;
                }
        }
        private void InitDgvUI()
        {
            foreach (DataGridViewColumn dc in dgv.Columns)
                if (dc.Name.StartsWith("x"))
                    dc.Width = 20;
                else
                    dc.Width = 40;
        }
        private void AddChooseTodtset(ref DataTable dtset)
        {
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "题组名称", "最大分值", "正确答案" });
            foreach (Optionsubject S in  _exam.OSubjects )
            {
                DataRow dr = dtset.NewRow();
                dr["OID"] = new ValueTag(S.ID.ToString(), S);
                dr["题组名称"] = S.Name();
                dr["最大分值"] = S.Score;
                dr["正确答案"] = S.Answer;
                dtset.Rows.Add(dr);
            }
            dtset.AcceptChanges();
        }
        private void AddUnChooseTodtset(ref DataTable dtset)
        {
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "题组名称", "最大分值", "图片" });
            _AvgUnImgWith = 0;
            _AvgUnImgHeight = 0;
            Rectangle r = new Rectangle(0, 0, 1, 1);
            if (_src != null)
                r.Size = _src.Size;
            foreach (Imgsubject S in _exam.Subjects)
            {
                DataRow dr = dtset.NewRow();
                dr["OID"] = new ValueTag(S.ID.ToString(), S);
                dr["题组名称"] = S.Name;
                dr["最大分值"] = S.Score;
                _AvgUnImgHeight += S.Height;
                _AvgUnImgWith += S.Width;
                Rectangle sr = S.Rect;
                if (_src != null && r.IntersectsWith(sr))
                {
                    sr.Intersect(r);
                    dr["图片"] = _src.Clone(sr, _src.PixelFormat);
                }
                dtset.Rows.Add(dr);
            }
            dtset.AcceptChanges();
            if (_exam.Subjects.Count > 0)
            {
                _AvgUnImgHeight /= _exam.Subjects.Count;
                _AvgUnImgWith /= _exam.Subjects.Count;
            }
        }
        private void buttonReadMe_Click(object sender, EventArgs e)
        {
            FormReadme f = new FormReadme();
            f.SetText("1、列表按键C  题组为空时，重构题组 \r\n   2、列表按键Delete  删除当前考试数据 ");
            f.ShowDialog();
        }
        private void AddStudentsdtset(ref DataTable dtset )
        {
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "姓名", "考号"});           
            foreach(Student S in _students.students)
            {
                DataRow dr = dtset.NewRow();
                dr["OID"] = new ValueTag(S.ID.ToString(), S);
                dr["姓名"] = S.Name;
                dr["考号"] = S.KH;
                dtset.Rows.Add(dr);
            }
            dtset.AcceptChanges();
        }
        private void CheckDataTable()
        {
            AcceptXztDataTableModified();
            AcceptFXztDataTableModified();
            AcceptStudentsDataTableModified();
        }
        private void AcceptXztDataTableModified()
        {
            if (_dtsetxzt != null)
                foreach (DataRow dr in _dtsetxzt.Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        Optionsubject I = (Optionsubject)((ValueTag)dr["OID"]).Tag;
                        I.Score = Convert.ToInt32(dr["最大分值"].ToString());    
                        I.Answer = dr["正确答案"].ToString();
                        dr.AcceptChanges();
                        _bexamdatamodified = true;
                    }
                }
        }
        private void AcceptFXztDataTableModified()
        {
            if (_dtsetfxzt != null)
                foreach (DataRow dr in _dtsetfxzt.Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        Imgsubject I = (Imgsubject)((ValueTag)dr["OID"]).Tag;
                        I.Score = Convert.ToInt32(dr["最大分值"].ToString());
                        I.Name = dr["题组名称"].ToString();
                        dr.AcceptChanges();
                        _bexamdatamodified = true;
                    }
                }
        }
        private void AcceptStudentsDataTableModified()
        {
            if (_dtsetfxzt != null)
                foreach (DataRow dr in _dtsetstudents.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                       //
                        MessageBox.Show("还未实现，待以后添加");
                        //_bexamdatamodified = true;
                    }
                }
        }
        private Exam _exam;
        private string _workpath;
        private Bitmap _src;
        private DataTable _dtsetxzt;
        private DataTable _dtsetfxzt;
        private DataTable _dtsetstudents;
        private int _AvgUnImgWith;
        private int _AvgUnImgHeight;
        private Template _template;
        private Students _students;
        private bool _bshowstudent;
        private object _activeitem;
        private Examdata _examdata;
        private bool _bexamdatamodified;

    }
    public class Examdata
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public StudentsResultData SR { get; set;}
        public void InitDeserialize()
        {
            if (SR != null)
            {
                if (SR._Tzsubjects == null)
                {
                    SR._Tzsubjects = new Tzsubjects();
                }
            }
            SR._Students.InitDeserialize(); //init index and dic
            SR._Imgsubjects.InitDeserialize(); // dic and bitmapdatalength
            SR._Tzsubjects.Deserialize(SR._Imgsubjects);
        }
    }
    public class StudentsResultData
    {
        public string _workpath;
        public Students _Students;
        public Imgsubjects _Imgsubjects;
        public Optionsubjects _Optionsubjects;
        public Tzsubjects _Tzsubjects;
        public List<List<int>> _Result;
    }
}
