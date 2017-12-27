using System;
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
            _examdata.SR._Students.InitDeserialize(); //init index and dic
            _examdata.SR._Imgsubjects.InitDeserialize(); // dic and bitmapdatalength
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
        private void buttonImportImage_Click(object sender, EventArgs e)
        {
            List<float > maxscore = new List<float>();
            List<string> optionanswer = new List<string>();
            if (!(CheckOptionAnswer(maxscore, optionanswer)
                && CheckResult()))
                return;
            ///////////////
            List<Rectangle> listrect = new List<Rectangle>();
			foreach (Area I in _template.Dic["特征点"])
			{
				listrect.Add(I.ImgArea );
			}
            if (listrect.Count != 3)
                return;
            //AutoDetectRectAnge adr = new AutoDetectRectAnge();
            //adr.ComputTBO(listrect);
            
            List<Rectangle> listTBO = AutoTBO.GetAutoTBORect(listrect);
            AutoAngle angle = new AutoAngle(listTBO.Select(r=>r.Location).ToList());


            //题组
            List<TzArea> ltz = new List<TzArea>();
            foreach (Area I in _template.Dic["题组"])
            {
                ltz.Add((TzArea)I);
            }
            Rectangle zfrect = _template.Dic["校对"][0].Rect;
            zfrect.Offset(zfrect.Width,0);
            zfrect.Width = 0;
            zfrect.Height = 0;


            Rectangle xztrect = _template.Dic["选择题"][0].Rect;
            xztrect.Offset(-30, -50);
            xztrect.Width = 1;
            xztrect.Height = 1;
            ltz.Add(new TzArea(zfrect, ""));
            ltz.Add(new TzArea(xztrect, ""));


            List<List<Imgsubject>> Tz = new List<List<Imgsubject>>();
            foreach (TzArea t in _template.Dic["题组"])
            {
                List<Imgsubject> L = new List<Imgsubject>();
                foreach (Imgsubject i in _examdata.SR._Imgsubjects.Subjects)
                {
                    if (t.ImgArea.Contains(i.Rect))
                        L.Add(i);
                }
                Tz.Add(L);
                //Tztitle += t.ToString() + ",";
            }

            if (!Directory.Exists("F:\\Out\\" + _exam.Name))
            {
                Directory.CreateDirectory("F:\\Out\\" + _exam.Name);
            }
            else
            {
                MessageBox.Show("已存在文件夹"+ _exam.Name + "! 继续将覆盖该文件夹内的文件！！ 请确认！！ ");
            }
           
            foreach (Student S in _students.students)
            {
                float sum = 0;
                S.OutXzt(optionanswer, maxscore, ref sum);
                float fsum = _examdata.SR._Result.Sum(rr => rr[S.Index]);
                int zfsum =(int) (sum + fsum);

                int tzindex = 0;
                foreach (List<Imgsubject> L in Tz)
                {
                    string name = L.Select(I => _examdata.SR._Result[I.Index][S.Index]).Sum() + "分";
                    ltz[tzindex].SetName( name);
                    tzindex++;
                }
                ltz[tzindex].SetName(zfsum.ToString());
                tzindex++;
                ltz[tzindex].SetName( sum.ToString());

                Bitmap bmp = TemplateTools.DrawInfoBmp(S,_examdata.SR,angle,optionanswer, ltz );

                string filename = "F:\\Out\\" + _exam.Name + "\\" + S.ID + ".jpg";
                if (_sc.Studentbases.HasStudentBase)
                {
                    if (S.KH > 1)
                    {
                        string name = _sc.Studentbases.GetName(S.KH);
                        filename = "F:\\Out\\" + _exam.Name + "\\" + S.ID + "_" + name + ".jpg";
                    }
                }
                bmp.Save(filename);
            }
            MessageBox.Show("已输出到F:\\Out\\"+_exam.Name);
        }
        private void buttonExportResult_Click(object sender, EventArgs e)
        {
            buttonModifyData_Click(sender, e);
        }
        private void buttonModifyData_Click(object sender, EventArgs e)
        {//导出成绩
            if(_activeitem == null) return;
            Optionsubjects _Optionsubjects = _examdata.SR._Optionsubjects;
            Imgsubjects _Imgsubjects = _examdata.SR._Imgsubjects;
            string msg = "共有选择题：" + _Optionsubjects.OptionSubjects.Count + " 题,  非选择题： " + _Imgsubjects.Subjects.Count + " 小题";
            msg += "\r\n选择题共： " + _Optionsubjects.OptionSubjects.Select(r => r.Score).Sum() + "分";
            msg += "\r\n 非选择题共： " + _Imgsubjects.Subjects.Select(r => r.Score).Sum() + "分";
            msg += "\r\n 合计共： " + (_Optionsubjects.OptionSubjects.Select(r => r.Score).Sum() + _Imgsubjects.Subjects.Select(r => r.Score).Sum()) + "分";
            //MessageBox.Show(msg);

            List<float > maxscore = new List<float>();
            List<string> optionanswer = new List<string>();
            if (CheckOptionAnswer(maxscore, optionanswer)
                && CheckResult())
            {
                int Oscore = (int)_exam.OSubjects.Sum(r => r.Score);
                int Sscore = _exam.Subjects.Sum(r => r.Score);
                MessageBox.Show("导出成绩" + msg);
                SaveFileDialog saveFileDialog2 = new SaveFileDialog();
                saveFileDialog2.FileName = _examdata.Name;
                saveFileDialog2.Filter = "txt files (*.txt)|*.txt";
                saveFileDialog2.Title = "导出成绩";
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        List<List<Imgsubject>> Tz = new List<List<Imgsubject>>();
                        string Tztitle="";
                        foreach (TzArea t in _template.Dic["题组"])
                        {
                            List<Imgsubject> L = new List<Imgsubject>();
                            foreach (Imgsubject i in _examdata.SR._Imgsubjects.Subjects)
                            {
                                if (t.ImgArea.Contains(i.Rect))
                                    L.Add(i);
                            }
                            Tz.Add(L);
                            Tztitle += t.ToString()+",";
                        }


                        string title = Student.ResultTitle() +"选择题,非选择题,总分,"+ string.Join(",", _exam.OSubjects.Select(r => r.Name()))
                         +","  + string.Join(",", _examdata.SR._Imgsubjects.Subjects.Select(r => r.Name)) + ","+Tztitle+"\r\n";

                        StringBuilder sb = new StringBuilder(title);
                        foreach (Student r in _students.students)
                        {
                            sb.Append(r.ResultInfo());
                            float sum = 0;
                            string xzt = r.OutXzt(optionanswer, maxscore, ref sum);
                            float fsum = _examdata.SR._Result.Sum( rr => rr[r.Index]);
                            sb.Append(sum + "," + fsum + "," + (sum + fsum) + ",");
                            sb.Append(xzt);
                            sb.Append(",");
                            sb.Append(string.Join(",",_examdata.SR._Result.Select(rr => rr[r.Index].ToString()).ToArray()));
                            sb.Append(",");
                            foreach (List<Imgsubject> L in Tz)
                            {
                                sb.Append(L.Select(I => _examdata.SR._Result[I.Index][r.Index]).Sum() + ",");
                            }

                            sb.AppendLine();
                        }
                        File.WriteAllText(saveFileDialog2.FileName, sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("还有选择题没有设定答案或者分值  或者 试卷未改完 \r\n"+msg);
            }
        }
        private bool CheckResult()
        {
            foreach (List<int> L in _examdata.SR._Result)
            {
                if (L.Any(r => r < 0))
                    return false;
            }
            return true;
        }
        private bool CheckOptionAnswer(List<float> maxscore, List<string> optionanswer)
        {
            bool allhasanswer = true;
            foreach (DataRow dr in _dtsetxzt.Rows)
            {
                string AN = dr["正确答案"].ToString();
                string SC = dr["最大分值"].ToString();
                if (AN.Length == 1 && "ABCD".Contains(AN) && SC != "")
                {
                    optionanswer.Add(AN);
                    try
                    {
                        maxscore.Add(Convert.ToSingle(SC));
                        continue;
                    }
                    catch { }
                }
                allhasanswer = false;
                break;
            }
            return allhasanswer;
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
            //TODO: NextWork  saveResult
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
            foreach (Imgsubject S in _exam.Subjects)
            {
                DataRow dr = dtset.NewRow();
                dr["OID"] = new ValueTag(S.ID.ToString(), S);
                dr["题组名称"] = S.Name;
                dr["最大分值"] = S.Score;
                _AvgUnImgHeight += S.Height;
                _AvgUnImgWith += S.Width;
                if (_src != null)
                    dr["图片"] = _src.Clone(S.Rect, _src.PixelFormat);
                dtset.Rows.Add(dr);
            }
            dtset.AcceptChanges();
            _AvgUnImgHeight /= _exam.Subjects.Count;
            _AvgUnImgWith /= _exam.Subjects.Count;
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
        public Optionsubjects _Optionsubjects;
        public List<List<int>> _Result;
    }
}
