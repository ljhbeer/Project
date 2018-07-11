using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ARTemplate;
using System.IO;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Tools;
using System.Text.RegularExpressions;

namespace ScanTemplate.FormYJ
{
    public partial class FormYJInit : Form
    {
        private  bool _InitDgv;
        //public FormYJInit(ExamConfig g, Template _artemplate, DataTable _rundt, string _workpath,string ExamName,string Datafullpath) 
        public FormYJInit(ScanConfig _sc, Template _artemplate, DataTable _rundt, string _workpath, string ExamName, string Datafullpath)
        {
            InitSrc(_artemplate, _rundt);
            this._sc = _sc;
            this.g = _sc.Examconfig;
            this._examname = ExamName;
            this._DataFullPath = Datafullpath;
            this._artemplate = _artemplate;
            this._rundt = _rundt;
            this._angle = _artemplate.Angle;
            this._workpath = _workpath;
            _InitDgv = false;
            InitializeComponent();
            InitStudents();
            InitOptionImgSubjects();
            dgv.DataSource = _rundt;
            InitImage();
        }

        private void InitSrc(Template _artemplate, DataTable _rundt)
        {
            this._src = null;
            if (_rundt.Rows.Count > 0)
            {
                string filename = _rundt.Rows[0]["文件名"].ToString();
                double angle = (double)_rundt.Rows[0]["校验角度"];
                _artemplate.Angle.SetPaper(angle);
                Rectangle correctRect = StringTools.StringToRectangle(_rundt.Rows[0]["CorrectRect"].ToString(), '-');
                _src = (Bitmap)Bitmap.FromFile(filename);
                _src = _src.Clone(correctRect, _src.PixelFormat);
            }
        }
        private void InitOptionImgSubjects()
        { 
            _Imgsubjects = Template.ConstructImgsubjects(_artemplate);
            _Tzsubjects = Template.ConstructTzsubjects(_artemplate,_Imgsubjects);
            _Optionsubjects = Template.ConstructOptionSubject(_artemplate);
        }
        private void InitStudents()
        {
            _Students = new Students(_rundt);
        }
        private void InitImage()
        {
            //Bitmap bmp = (Bitmap)_artemplate.Image.Clone();
            //pictureBox1.Image = ARTemplate.TemplateTools.DrawInfoBmp(bmp,_artemplate,_angle);
        }
        private void InitDgvUI()
        {
            foreach (DataGridViewColumn dc in dgv.Columns)
                if (dc.Name.StartsWith("x"))
                    dc.Width = 20;
                else
                    dc.Width = 40;
        }
        private void InitDgvSetUI(bool xzt)
        {
            dgvSet.DataSource = null;
            if (xzt)
            {
                dgvSet.RowTemplate.Height = 24;
                dgvSet.DataSource = _dtsetxzt;
                DgvShowOptionMode = true;
            }
            else
            {
                dgvSet.RowTemplate.Height = _AvgUnImgHeight;
                dgvSet.DataSource = _dtsetfxzt;
                DgvShowOptionMode = false;
            }

            foreach (DataGridViewColumn dc in dgvSet.Columns)
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
        private void FormYJInit_Load(object sender, EventArgs e)
        {
            InitDgvUI();
            AddChooseTodtset(ref _dtsetxzt);
            AddUnChooseTodtset(ref _dtsetfxzt);
            InitDgvSetUI(true);
        }
        private void AddChooseTodtset(ref DataTable dtset)
        {
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "题组名称", "最大分值", "正确答案" });
            foreach (Optionsubject S in _Optionsubjects.OptionSubjects)
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

            foreach (Imgsubject S in _Imgsubjects.Subjects)
            {
                try
                {
                    DataRow dr = dtset.NewRow();
                    dr["OID"] = new ValueTag(S.ID.ToString(), S);
                    dr["题组名称"] = S.Name;
                    dr["最大分值"] = S.Score;
                    _AvgUnImgHeight += S.Height;
                    _AvgUnImgWith += S.Width;
                    if(_src!=null)
                    dr["图片"] = _src.Clone(S.Rect, _src.PixelFormat);
                    dtset.Rows.Add(dr);
                }
                catch
                {
                    ;
                }
            }
            dtset.AcceptChanges();
            if (_Imgsubjects.Subjects.Count > 0)
            {
                _AvgUnImgHeight /= _Imgsubjects.Subjects.Count;
                _AvgUnImgWith /= _Imgsubjects.Subjects.Count;
            }
        }
        private void buttonShowXztSet_Click(object sender, EventArgs e)
        {
            InitDgvSetUI(true);
        }
        private void buttonShowFXztSet_Click(object sender, EventArgs e)
        {
            InitDgvSetUI(false);
        }
        private void buttonCreateYJData_Click(object sender, EventArgs e)
        {
           
            //if (!FormTemplate.CheckTemplate(_artemplate, true))
                //return;           
            if (!CheckScore())
                return;
            if (!FormM.CheckClassInformation(_Students,_sc))
                return;

            Boolean bOverLap = false;
            ExamInfo ei = new ExamInfo();
            ei.Name = _examname;
            ei.TemplateFileName = _DataFullPath + "\\template.json";  
            if (!g.CheckExamInfoName(ei)){
                //MessageBox.Show("考试名称存在重复，请重新设置");
                if (MessageBox.Show("考试名称存在重复，是否覆盖请点\"  确定  \" \r\n,一旦覆盖,该考试的所有已阅数据将被删除,\r\n否则请退回上一步更改考试名称","考试名称重复", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                    return ;
                FormInput f = new FormInput("考试名称重复删除确认","请输入 Delete 以确认覆盖本次考试的原有数据");
                f.ShowDialog();                
                if (f.StrValue == null || f.StrValue != "Delete")
                    return;
                if(! g.GetExistExamInfo(ei) )
                    return ;
                bOverLap = true;
            }
            {
                AcceptXztDataTableModified();
                Exam exam = new Exam(_Students, _Imgsubjects,_Optionsubjects,_Tzsubjects, ei.Path);
                exam.Name = _examname;
                if (!Directory.Exists(ei.Path))
                    Directory.CreateDirectory(ei.Path);
                ImgbinManagesubjects ims = new ImgbinManagesubjects(_Students, _Imgsubjects);
                ims.SaveBitmapFixedDataToData(ei.Path);
                //UTODO: 实现 exam.checkindex
                if (bOverLap)
                    g.SetExistExamInfo(ei);
                else
                    g.AddExamInfo(ei);
                g.SaveConfig("config.json");

                exam.TzSubjects.InitIds();
                string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(exam));
                File.WriteAllText(g.ExamPath + "\\"+ei.Name+".json", str);
                File.WriteAllText(_DataFullPath + "\\已生成阅卷数据.txt", "");
            }           
        }

        private bool CheckScore(bool CheckAnswer = true)
        {
            string Msg = ResultInfomation();
            if (CheckAnswer)
            {
                if (CheckAllOptionAnswerFixed())
                    Msg += "\r\n选择题已全部设好答案，是否需要重新检查确认是否正确";
                else
                    Msg += "\r\n还有选择题没有设定答案，是否需要重新设定答案";

            }
            if (MessageBox.Show(Msg, "模板成绩信息确认，是否继续保存", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                return false;
            return true;
        }
        private bool CheckAllOptionAnswerFixed()
        {
            return !this._Optionsubjects.OptionSubjects.Exists(r => r.Answer == "");
        }
        private void buttonShowScore_Click(object sender, EventArgs e)
        {
            string msg = ResultInfomation();
            ShowOptionScoreMsg();
            MessageBox.Show(msg);            
        }
        private string ResultInfomation()
        {
            float OptionScore =  _Optionsubjects.OptionSubjects.Select(r => r.Score).Sum() ;
            float UnChoosScore =  _Imgsubjects.Subjects.Select(r => r.Score).Sum() ;
            float TotalScore = OptionScore+ UnChoosScore;
            string msg =string.Format("总分：{0}  \r\n选择题：{1}题 {2}分 \r\n非选择题：{3}题 {4}分",TotalScore, 
                _Optionsubjects.OptionSubjects.Count, OptionScore,
                _Imgsubjects.Subjects.Count,UnChoosScore);
           return msg;
        }
        private void ShowOptionScoreMsg()
        {           
            textBoxMsg.Text = ResultInfomation();
        }
        private void dgvSet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = dgvSet;
            if (e.RowIndex == -1 || e.ColumnIndex == -1 || _InitDgv) return;
            string columnName = dgv.Columns[e.ColumnIndex].Name;
            string str = dgv[e.ColumnIndex, e.RowIndex].Value.ToString();
            if (DgvShowOptionMode)
            {
                UpdateDgvOptionSubjectValue(columnName, str,  (DataRow) _dtsetxzt.Rows[e.RowIndex] );
            }
            else
            {
                UpdateDgvImgSubjectValue(columnName, str, _dtsetfxzt.Rows[e.RowIndex]);
            }         
            ShowOptionScoreMsg();
        }
        public static void UpdateDgvOptionSubjectValue(string columnName, string cellvalue, DataRow dr)
        {
            Optionsubject O = (Optionsubject)((ValueTag)(dr["OID"])).Tag;
            if (columnName == "最大分值")
            {
                O.Score = Convert.ToSingle(cellvalue);
                //dr["最大分值"] = O.Score;
            }
            else if (columnName == "半对分值")
            {
                O.HalfScore = Convert.ToSingle(cellvalue);
            }
            else if (columnName == "正确答案")
            {
                cellvalue = cellvalue.ToUpper();
                cellvalue = Regex.Replace(cellvalue, "[^A-D]", "");
                O.Answer = cellvalue;
            }
        }
        public static void UpdateDgvImgSubjectValue(string columnName, string cellvalue, DataRow dr)
        {
            Imgsubject U = (Imgsubject)((ValueTag)dr["OID"]).Tag;
            if (columnName == "最大分值")
                U.Score = Convert.ToInt32(cellvalue);
        }
        private void AcceptXztDataTableModified()
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
                    }
                }
        }
        private void buttonVerify_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < _Imgsubjects.Subjects.Count; index++)
            {
                _Imgsubjects.Subjects[index].Index = index;
            }
            string path = _workpath.Replace("\\LJH", "\\LJH\\bindata") + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Exam exam = new Exam(_Students, _Imgsubjects, _Optionsubjects, _Tzsubjects, path);
            FormFullScreenYJ fs = new FormFullScreenYJ(exam);
            this.Hide();
            fs.ShowDialog();
            this.Show();
            //ImgbinManagesubjects ims = new ImgbinManagesubjects(_Students, _Imgsubjects);
            //ims.InitLoadBindata( path);
            //if (ims.SetActiveSubject(_Imgsubjects.Subjects[0]))
            //{
            //    int cnt = 0;
            //    foreach (Student S in _Students.students)
            //    {
            //        if (cnt++ == 5) break;
            //        //ims.ActiveSubjectBitmap(S).Save(S.ID + "_1_" + S.KH + ".tif");
            //    }
            //}
            //ims.SetActiveSubject(null);
            //return;
        }
        private void buttonImportOptionAnswerScore_Click(object sender, EventArgs e)
        {           
            ImportOptionAnswerScore(_dtsetxzt);
        }
        public static void ImportOptionAnswerScore(DataTable _dtsetxzt)
        {
            FormSetscore f = new FormSetscore(_dtsetxzt);
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < f.Xzt().Count; i++)
                {
                    Optionsubject O = (Optionsubject)((ValueTag)_dtsetxzt.Rows[i]["OID"]).Tag;
                    //if (_dtsetxzt.Rows[i]["题组名称"].ToString().EndsWith(f.Xzt()[i].ID.ToString()))
                    if (O.ID == f.Xzt()[i].ID)
                    {
                        _dtsetxzt.Rows[i]["正确答案"] = f.Xzt()[i].OptionAnswer;
                        _dtsetxzt.Rows[i]["最大分值"] = f.Xzt()[i].Score;
                        O.Answer = f.Xzt()[i].OptionAnswer;
                        O.Score = f.Xzt()[i].Score;
                        O.Type = f.Xzt()[i].Type;
                        if (O.Type == "U") // S 单选, M 多选,U 不定项
                        {
                            O.HalfScore = f.Xzt()[i].HalfScore;
                        }
                    }
                }
            }
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
        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {//
            if (e.RowIndex == -1 || e.ColumnIndex == -1|| _rundt == null)
                return;         
            string fn = _rundt.Rows[e.RowIndex]["文件名"].ToString();
            if (File.Exists(fn))
            {
                {//SameTo FormYJTools.dgv_CellContentClick
                    Student S = new Student();
                    double S_angle = (double)(_rundt.Rows[e.RowIndex]["校验角度"]);
                    Bitmap S_Src = (Bitmap)Bitmap.FromFile(fn);
                    AutoAngle Angle =_artemplate.Angle;

                    string str = _rundt.Rows[e.RowIndex]["CorrectRect"].ToString();
                    Rectangle S_SrcCorrectRect = Tools.StringTools.StringToRectangle(str, '-');

                    if (Angle != null && S_SrcCorrectRect.Width > 20 && S_SrcCorrectRect.Height > 20)
                        Angle.SetPaper(S_angle);
                    pictureBox1.Image = TemplateTools.DrawInfoBmp(S_Src.Clone(S_SrcCorrectRect, S_Src.PixelFormat),_artemplate, Angle);
                }
            }
        }
        private Template _artemplate;
        private DataTable _rundt;
        private AutoAngle _angle;
        private DataTable _dtsetxzt;
        private DataTable _dtsetfxzt;
        private Bitmap _src;
        private int _AvgUnImgWith;
        private int _AvgUnImgHeight;

        private Students _Students;
        private Imgsubjects _Imgsubjects;
        private Optionsubjects _Optionsubjects;
        private string _workpath;
        private ExamConfig g;
        private string _examname;
        private string _DataFullPath;
        private Tzsubjects _Tzsubjects;
        private bool DgvShowOptionMode;
        private ScanConfig _sc;

    }    
    public class Optionsubjects
    {
        public Optionsubjects()
        {
            OptionSubjects = new List<Optionsubject>();
        }
        public void Clear()
        {
        }
        public void Add(Optionsubject S)
        {
            OptionSubjects.Add(S);
        }
        public List<Optionsubject> OptionSubjects { get; set; }
    }
    public class Optionsubject
    {
        public Optionsubject()
        {
        }
        public Optionsubject(SingleChoiceArea U, int index, int pos)
        {
            OptionAnswerScore O = U.Listanswerscore[pos];
            this.Index = O.Index;
            this.ID = O.ID;
            this.Score = O.Score;
            this.Answer = O.Answer;
            this.Type = O.Type;
            InitDeserialize(U, pos);

        }
        private void InitDeserialize(SingleChoiceArea U, int pos)
        {
            this._Rect = U.ImgArea;
            this.Size = U.Size;
            this.List = new List<Point>();
            List = U.list[pos];
        }
        public override string ToString()
        {
            return Name();
        }

        public int ID { get; set; }
        public float Score { get; set; }
        public int Index { get; set; }
        public List<Point> List{ get; set; }
        public Size Size { get; set; }
        public string Name() { return "x" + ID; }
        public string OutName { get { return "选择题" + ID; } }
        public string Answer { get; set; }

        public float HalfScore { get; set; }
        public string Type { get; set; }
        [JsonProperty]
        private Rectangle _Rect;

        [JsonIgnore]
        public Rectangle Rect { get { return _Rect; } }
        [JsonIgnore]
        public int Height { get { return Rect.Height; } }
        [JsonIgnore]
        public int Width { get { return Rect.Width; } }
        //[JsonIgnore]
        //private SingleChoiceArea U;


    }
    [JsonObject(MemberSerialization.OptIn)]
    public class Tzsubjects
    {
        public Tzsubjects()
        {
            Tzs = new List<Tzsubject>();
        }
        [JsonProperty]
        public List<Tzsubject> Tzs;
        public void Add(Tzsubject tz)
        {
            Tzs.Add(tz);
        }
        public void InitIds()
        {
            foreach (Tzsubject tz in Tzs)
                tz.InitIds();
        }
        public void Deserialize(Imgsubjects imgsubjects)
        {
            foreach (Tzsubject r in Tzs)
                r.InitDeserialize(imgsubjects);
        }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class Tzsubject
    {        
        public Tzsubject()
        {
            Subjects = new List<Imgsubject>();
            Subjectids = new List<int>();
        }
        public void Add(Imgsubject tz)
        {
            Subjects.Add(tz);
        }
        public void InitIds()
        {
            Subjectids.Clear();
            if(Subjects.Count>0)
            Subjectids.AddRange(
                Subjects.Select(r => r.ID).ToArray());     
        }
        public void InitDeserialize(Imgsubjects ibs)
        {
            Subjects.Clear();
            if(Subjectids.Count>0)
            Subjects = Subjectids.Select(r => ibs.ImgSubjectByID(r)).ToList();
        }
        [JsonIgnore]
        public List<Imgsubject> Subjects;
        [JsonProperty]
        private List<int> Subjectids;
        public Rectangle Rect;
        public string Name;
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class Imgsubjects
    {
        public Imgsubjects()
        {
            _subjects = new List<Imgsubject>();
            _dic = new Dictionary<int, Imgsubject>();
            _activeid = -1;
        }
        public void InitDeserialize()
        {
            _dic = new Dictionary<int, Imgsubject>();
            _activeid = -1;
            foreach (Imgsubject S in _subjects)
            {
                _dic[S.ID] = S;
                S.InitDeserialize();
            }
        }
        public bool Add(Imgsubject S)
        {
            if (!_dic.ContainsKey(S.ID)) //FormYJInit.InitImgSubjects() 中 设定
            {
                _subjects.Add(S);
                _dic[S.ID] = S;
                return true;
            }
            return false;

        }
        public Imgsubject ImgSubjectByID(int ID)
        {
            if (_dic.ContainsKey(ID))
                return _dic[ID];
            return null;
        }
        public void SetActiveSubject(Imgsubject S)
        {
            if (S != null)
                _activeid = S.ID;
            else
                _activeid = -1;
        }
        [JsonIgnore]
        public List<Imgsubject> Subjects
        {
            get
            {
                return _subjects;
            }
        }
        [JsonIgnore]
        public Imgsubject ActiveSubject
        {
            get
            {
                return ImgSubjectByID(_activeid);
            }
        }

        [JsonProperty]
        private List<Imgsubject> _subjects;
        private Dictionary<int, Imgsubject> _dic;
        private int _activeid;
    }
    public class Imgsubject
    {      
        public Imgsubject(UnChoose U, int index)
        {
            this._U = U;
            this.ID = index+1;
            this.Index = index;
            this.Score = U.Scores;
            this.Name = U.Name;
            this._Rect = _U.ImgArea;
        }
        public Imgsubject()
        {
        }
        public void InitDeserialize()
        {
            int w = Width;
            w = (w / 8) * 8 + (w % 8 > 2 ? 8 : 0);
            int stride = w / 8;
            stride = (stride / 4 * 4) + (stride % 4 > 0 ? 4 : 0);
            BitmapdataLength = w * stride;
        }
        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; } 
        public int ID { get; set; }
        public int Score { get; set; }
        public int Index { get; set; }
        [JsonProperty]
        private Rectangle _Rect;
        [JsonIgnore]
        public Rectangle Rect { get { return _Rect; } }
        [JsonIgnore]
        public int Height { get { return Rect.Height; } }
        [JsonIgnore]
        public int Width { get { return Rect.Width; } }
        [JsonIgnore]
        public int BitmapdataLength{ get; set; }
        private UnChoose _U;
        //public int ID;
        //public Double Score;
    }
    public class Papers 
    {
        public Papers()
        {
            _papers = new List<Paper>();
            _iddic = new Dictionary<int, Paper>();
            _khdic = new Dictionary<int, Paper>();
        }

        public Papers(List<Paper> papers)
        {
            _papers = new List<Paper>();
            _iddic = new Dictionary<int, Paper>();
            _khdic = new Dictionary<int, Paper>();
            foreach (Paper p in papers)
                AddPaper(p);
        }

        public void AddPaper(Paper p)
        {
            _iddic[p.ID] = p;
            _khdic[p.KH] = p;
            _papers.Add(p);
        }
        public void RebuildDic()
        {
            _khdic.Clear();
            foreach (Paper p in _papers)
                _khdic[p.KH] = p;
        }
        public Paper PaperFromID(int ID)
        {
            if (_iddic.ContainsKey(ID))
                return _iddic[ID];
            return new Paper(); //null
        }
        public Paper PaperFromKh(int KH)
        {
            if(_khdic.ContainsKey(KH))
            return _khdic[KH];
            return new Paper(); //null
        }
        public void SavePapers(string Datafilename)
        {
            string str = Tools.JsonFormatTool.ConvertJsonString(
                Newtonsoft.Json.JsonConvert.SerializeObject(_papers));
            File.WriteAllText(Datafilename, str);
        }

        [JsonProperty]
        private List<Paper> _papers;
        private Dictionary<int, Paper> _iddic;
        private Dictionary<int, Paper> _khdic;
        public List<Paper> PaperList { get { return _papers; } }
        // Can be null
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class Paper
    {
        public Paper( )
        {
            BackScore = -1;
        }
        public Paper(string filename, Rectangle correctrect, List<Rectangle> listFeature)
        {
            // TODO: Complete member initialization
            this._imgfilename = filename;
            this._SrcCorrectRect = correctrect;
            this.list = listFeature;

            BackScore = -1;
            _id = -1;
            KH = -1;
            Name = "";
            _XZT = new List<string>();
        }       
        public bool CorrectXzt(int index, string answer)
        {
            if (index < 0 || index > _XZT.Count)
                return false;
            return _XZT[index] == answer;
        }
        public string OptionAnswer(int optionindex)
        {
            if (_XZT != null && optionindex >= 0 && optionindex < _XZT.Count)
                return _XZT[optionindex];
            return "";
        }
        public string OutXzt(List<string> optionanswer, List<float> maxscore, ref float sum)
        {
            int i = 0;
            List<float> result = _XZT.Select(r =>
            {
                float ret = 0;
                if (r == optionanswer[i])
                    ret = maxscore[i];
                i++;
                return ret;
            }).ToList();
            sum = result.Sum();
            return string.Join(",", result);
        }
        public void AddXZT(string xzt)
        {
            if (xzt.EndsWith("|"))
                xzt = xzt.Substring(0, xzt.Length - 1);
            _XZT = xzt.Split('|').ToList();
        }
        public void InitID()
        {
            if (_imgfilename != "" && _imgfilename.Contains("."))
            {
                string s = _imgfilename.Substring(0, _imgfilename.IndexOf("."));
                if (s.Contains("\\"))
                    s = s.Substring(s.LastIndexOf("\\") + 1);
                if (s.Contains("-"))
                {
                    s = s.Substring(s.LastIndexOf("-") + 1);
                    _id = Convert.ToInt32(s.Substring(s.Length - 4));
                }
            }
        }
        public void SetNewFileName(string imgfilename)
        {
            _imgfilename = imgfilename;
        }
        public string ToJsonString()
        {
            return Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(this));
        }
        public string ResultInfo()
        {
            return ID + "," + KH + "," + Name + ",";
        }

        public double Angle { get; set; }
        public int KH { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string Custom { get; set; }
        [JsonIgnore]
        public int ID { get { return _id; } }
        [JsonIgnore]
        public int BackScore { get; set; }
        [JsonIgnore]
        public string ImgFilename { get { return _imgfilename; } }
        [JsonIgnore]
        public Bitmap Src
        {
            get
            {
                if (_src == null)
                {
                    if (System.IO.File.Exists(_imgfilename))
                        _src = (Bitmap)Bitmap.FromFile(_imgfilename);
                }
                return _src;
            }
        }
        [JsonIgnore]
        public Rectangle SrcCorrectRect
        {
            get
            {
                return _SrcCorrectRect;
            }
        }
        [JsonIgnore]
        public List<String> XZT { get { return _XZT; } }
        
        [JsonProperty]
        private List<Rectangle> list;
        [JsonProperty]
        private int _id;
        [JsonProperty]
        private string _imgfilename;
        [JsonProperty]
        private Rectangle _SrcCorrectRect;
        [JsonProperty]
        private List<string> _XZT;
        ///[JsonIgnore]
        private Bitmap _src;
    }

    public class Students
    {
        public Students(DataTable _rundt)
        {
            this._studt = _rundt;
            int cnt = 0;
            foreach (DataColumn dc in _rundt.Columns)
                if (dc.ColumnName.StartsWith("x"))
                    cnt++;
            _students = new List<Student>();
            _iddic = new Dictionary<int, Student>();
            _khdic = new Dictionary<int, Student>();
            //int index = 0;
            foreach (DataRow dr in _rundt.Rows)
            {
                Student s = new Student(dr,cnt);
                _students.Add(s);
                _iddic[s.ID] = s;
                _khdic[s.KH] = s;
                //s.Index = index;
                //index++;
            }
        }
        public Students()
        {
            _studt = null;
        }
        public void InitDeserialize()
        {
            _iddic = new Dictionary<int, Student>();
            _khdic = new Dictionary<int, Student>();
            foreach (Student s  in _students)
            {
                s.InitDeserialize();
                _iddic[s.ID] = s;
                _khdic[s.KH] = s;
            }
        }
       
        public Student StudentFromID(int ID)
        {
            if(_iddic.ContainsKey(ID))
            return _iddic[ID];
            return new Student();
        }
        public Student StudentFromKh(int KH)
        {
            return _khdic[KH];
        }
        [JsonIgnore]
        public List<Student> students
        {
            get
            {
                return _students;
            }
        }
        public List<int> StudentsClassid(ScanConfig _sc)
        {
            List<int> Lclassid = _students.Select( r => _sc.Studentbases.GetClass(r.KH) ).Distinct().ToList();
            Lclassid.Remove(0);
            return Lclassid;
        }
        public bool CheckIndex()
        {
            for (int i = 0; i < _students.Count; i++)
            {
                if (i != _students[i].Index)
                    return false;
            }
            return true;
        }
        [JsonProperty]
        private List<Student> _students;
        private Dictionary<int, Student> _iddic;
        private Dictionary<int, Student> _khdic;
        // Can be null
        private DataTable _studt;
      
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class Student
    {
        public Student(DataRow dr,int XZTcount)
        {           
            this._imgfilename = dr["文件名"].ToString();
            string str = _imgfilename.Substring(_imgfilename.LastIndexOf("-")+1);
            str = str.Substring(0, str.IndexOf("."));
            _id = Convert.ToInt32(str);
            _id %= 10000;

            str = dr["CorrectRect"].ToString();
            _SrcCorrectRect = Tools.StringTools.StringToRectangle(str,'-');
            this.Angle = Convert.ToDouble( dr["校验角度"].ToString());
            this.Name =  dr["姓名"].ToString();
            if (dr.Table.Columns.Contains("考号"))
            {
                string skh = dr["考号"].ToString();
                if (skh.Contains("-"))
                    this.KH = 0;
                else
                    this.KH = Convert.ToInt32(dr["考号"].ToString());
            }
            this._XZT = new List<string>();
            for (int i = 1; i < XZTcount+1; i++)
            {
                _XZT.Add(dr["x" + i].ToString());
            }
            _src = null;
            Sort = new StudentSort();
            BackScore = -1;
        }
        public Student()
        {
            BackScore = -1;
        }
        public void InitDeserialize()
        {
            string str = _imgfilename.Substring(_imgfilename.LastIndexOf("-") + 1);
            str = str.Substring(0, str.IndexOf("."));
            _id = Convert.ToInt32(str);
            _id %= 10000;
            Sort = new StudentSort();
            Sort.SetValue(Index);
            BackScore = -1;
        }
        public string ResultInfo()
        {
            return ID + "," + KH + "," + Name + ",";
        }
        public bool CorrectXzt(int index, string answer)
        {
            if (index < 0 || index > _XZT.Count)
                return false;
            return _XZT[index] == answer;
        }
        public string OptionAnswer(int optionindex)
        {
            if (_XZT != null && optionindex >= 0 && optionindex < _XZT.Count)
                return _XZT[optionindex];
            return "";
        }
        public string OutXzt(List<string> optionanswer, List<float> maxscore, ref float sum)
        {
            int i = 0;
            List<float> result = _XZT.Select(r =>
            {
                float ret = 0;
                if (r == optionanswer[i])
                    ret = maxscore[i];
                i++;
                return ret;
            }).ToList();
            sum = result.Sum();
            return string.Join(",", result);
        }
        public static string ResultTitle()
        {
            return "ID,考号,姓名,";
        }
        [JsonIgnore]
        public int ID { get { return _id; } }
        public double Angle { get; set; }
        public int KH { get; set; }
        public string Name { get; set; }
        public string ImgFilename { get { return _imgfilename; } }
        public int Index { get; set; }

        [JsonIgnore]
        public int BackScore { get; set; }
        [JsonIgnore]
        public StudentSort Sort { get; set; }
        [JsonIgnore]
        public Bitmap Src
        {
            get
            {
                if (_src == null)
                {
                    if (System.IO.File.Exists(_imgfilename))
                        _src =(Bitmap) Bitmap.FromFile(_imgfilename);
                }
                if (_src != null)
                {
                    if (_src.VerticalResolution != 200 || _src.HorizontalResolution != 200)
                        _src.SetResolution(200, 200);
                }
                return _src;
            }
        }
        [JsonIgnore]
        public Rectangle SrcCorrectRect
        {
            get
            {
                return _SrcCorrectRect;
            }
        }
        [JsonProperty]
        private string _imgfilename;
        [JsonProperty]
        private List<string> _XZT;
        private int _id;
        [JsonProperty]
        private Rectangle _SrcCorrectRect;
        private Bitmap _src;

        public bool SelectOption(string r, int index)
        {
            if (index >= 0 && index < _XZT.Count)
                return r == _XZT[index];
            return false;
        }
    }
    public class StudentBases
    {
        public StudentBases(string listfilename)
        {
            InitStudentBase(listfilename);
        }
        public void InitStudentBase(string listfilename)
        {
            HasStudentBase = false;
            string msg = "";
            if (File.Exists(listfilename))
            {
                String[] ss = File.ReadAllLines(listfilename);
                if (ss.Length > 2 && ss[0].Contains("班级,姓名,考号"))
                {
                    bool haserror = false;
                    HasStudentBase = true;
                    _studentbases = new List<StudentBase>();
                    _khbasedic = new Dictionary<int, StudentBase>();
                    _classiddic = new Dictionary<int, List<StudentBase>>();
                    for (int i = 1; i < ss.Length; i++)
                    {
                        string[] items = ss[i].Split(',');
                        if (items.Length == 3)
                        {
                            int classid = Convert.ToInt32(items[0]);
                            int kh = Convert.ToInt32(items[2]);
                            StudentBase sb = new StudentBase(classid, items[1], kh);
                            _studentbases.Add(sb);
                            if (!_khbasedic.ContainsKey(kh))
                                _khbasedic[kh] = sb;
                            if (!_classiddic.ContainsKey(classid))
                                _classiddic[classid] = new List<StudentBase>();
                            if (!_classiddic[classid].Contains(sb))
                                _classiddic[classid].Add(sb);
                            else
                            {
                                haserror = true;
                                msg += "Line: " + i + "\t重复添加对象：kh=" + sb.KH + "\r\n";
                            }

                        }
                        else
                        {
                            haserror = true;
                            msg += "Line: " + i + "\t" + ss[i] + "\r\n";
                        }
                    }
                    if (haserror)
                    {
                        MessageBox.Show("以下条目中存在错误，每行超过3个项目\r\n" + msg);
                    }
                }
            }
        }
        public List<StudentBase> GetClassStudent(int classid)
        {
            if (HasStudentBase && _classiddic.ContainsKey(classid))
                return _classiddic[classid];
            return new List<StudentBase>();
        }
        public string GetName(int kh)
        {
            if (HasStudentBase && _khbasedic.ContainsKey(kh))
                return _khbasedic[kh].Name;
            return "-";
        }

        public bool HasStudentBase { get; set; }
        private List<StudentBase> _studentbases;
        private Dictionary<int, StudentBase> _khbasedic;
        private Dictionary<int, List<StudentBase>> _classiddic;

        public List<StudentBase> Studentbase { get { return _studentbases; } }
        public  bool ContainsKey(int kh)
        {
            return _khbasedic.ContainsKey(kh);
        }
        public int GetClass(Student r)
        {
            if (HasStudentBase && _khbasedic.ContainsKey(r.KH))
                return _khbasedic[r.KH].Classid;
            return 0;
        }
        public  StudentBase GetStudent(int kh)
        {
            if (HasStudentBase && _khbasedic.ContainsKey(kh))
                return _khbasedic[kh] ;
            return null;
        }
        public  int GetClass(int kh)
        {
            if (HasStudentBase && _khbasedic.ContainsKey(kh))
                return _khbasedic[kh].Classid;
            return 0;
        }
    }
    public class StudentBase
    {
        public StudentBase(int classid, string name, int  kh)
        {
            this.Classid = classid;
            this.Name = name;
            this.KH = kh;
            this.PYCode = PYTool.GetChineseSpell(name);
        }
        public int Classid { get; set; }
        public string Name { get; set; }
        public int KH { get; set; }
        public string PYCode { get; set; }
    }
    public class StudentSort 
    {
        public StudentSort() 
        {
            _V = _v1=_v2=_v3=0;
        }
        // 用于阅卷时 v2 = -10000 ,可以累加 v3
        public void SetValue(int v1, int v2 = -1, int v3 = -1) // v1  + v2 * 100 + v3 * 10000 v1 v2 v3 >0
        {
            if (v1 >= 0)
                _v1 = v1;
            if (v2 >= 0)
                _v2 = v2;
            if (v3 >= 0)
            {
                _v3 = v3;
                if (v2 == -10000)
                    _v2 += v3;
            }
            _V = _v1 + _v2 * 100 + _v3 * 10000;
        }
       
        public int SortValue { get { return _V; } }
        public override string ToString()
        {
            return _v3 + "_" + _v2 + "_" + _v1;
        }
        private int _v1;
        private int _v2;
        private int _v3;
        private int _V;

    }
    public class ImgbinManagesubjects
    {
        public ImgbinManagesubjects(Students _Students, Imgsubjects _Imgsubjects)
        {
            this._Students = _Students;
            this._Imgsubjects = _Imgsubjects;
            _fs = null;
            _bs = null;
            _IDInfo = null;
            _buffer = new byte[102400];
        }
        ~ImgbinManagesubjects()
        {
            Clear();
        }
        public void Clear()
        {
            if (_bs != null || _fs != null)
            {
                _bs.Flush();
                _bs.Close();
                _fs.Close();
                _bs = null;
                _fs = null;
            }
        }
        public void InitLoadBindata(string bindatapath)
        {
            this._bindatapath = bindatapath;
            string lengthpath = bindatapath + "length.txt";
            string idindexpath = bindatapath + "idinfo.json";          
            if (!File.Exists(idindexpath))
                return;
            _IDInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgbinDataInfo>(File.ReadAllText(idindexpath));
            foreach (KeyValuePair<int, int> kv in _IDInfo.IDIndex)
            {
                _Students.StudentFromID(kv.Key).Index = kv.Value;
            }
            for (int i = 0; i < _Imgsubjects.Subjects.Count; i++)
                _Imgsubjects.Subjects[i].BitmapdataLength = _IDInfo.SubjectLengthInfo[i];
        }
        public bool SetActiveSubject(Imgsubject S)
        {
            if (_bs != null || _fs != null)
            {
                _bs.Flush();
                _bs.Close();
                _fs.Close();
                _bs = null;
                _fs = null;
            }
            _Imgsubjects.SetActiveSubject(S);
            if(_Imgsubjects.ActiveSubject==null)
                return false;
            string imgdatafilename = _bindatapath + "subjectfixed_" + S.ID + ".data";
            _fs = new FileStream(imgdatafilename, FileMode.Open, FileAccess.Read);
            _bs = new BufferedStream(_fs, 512000);
            return true;
        }
        public Bitmap ActiveSubjectBitmap(Student S) //indexdic
        {
            int length = _Imgsubjects.ActiveSubject.BitmapdataLength;
            Rectangle r = _Imgsubjects.ActiveSubject.Rect;
            r.Width = (r.Width / 8) * 8 + (r.Width % 8 > 2 ? 8 : 0);
            if (_IDInfo == null || !_IDInfo.IDIndex.ContainsKey(S.ID))
                return null;

            int index = _IDInfo.IDIndex[S.ID];
            if (length > _buffer.Length)
                _buffer = new byte[length];
            _bs.Position = index * length;
            _bs.Read(_buffer, 0, _buffer.Length);
            Bitmap bmp = new Bitmap(r.Width, r.Height);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, r.Width, r.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            IntPtr ptr = bmpData.Scan0;
            int bytes = bmpData.Stride * bmp.Height;
            System.Runtime.InteropServices.Marshal.Copy(_buffer, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        //for SaveBinData 
        private void SaveBitmapToData(string bindatapath) //bitmap.Length 不定长 ， 低效
        {
            List<List<long>> LL = new List<List<long>>();
            List<FileStream> fs = new List<FileStream>();
            List<BufferedStream> bsf = new List<BufferedStream>();
            List<long> startpos = new List<long>();
            for (int i = 0; i < _Imgsubjects.Subjects.Count; i++)
            {
                if (File.Exists(bindatapath + "subject_" + i + ".data"))
                    File.Delete(bindatapath+"subject_" + i + ".data");
                fs.Add(new FileStream(bindatapath+"subject_" + i + ".data", FileMode.Append, FileAccess.Write));
                startpos.Add(0);
                bsf.Add(new BufferedStream(fs[i], 102400));
            }
            Dictionary<int, int> IDIndex = new Dictionary<int, int>();
            int index = 0;
            foreach (Student S in _Students.students)
            {
                List<long> L = new List<long>();
                LL.Add(L);
                IDIndex[S.ID] = index;
                index++;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BufferedStream bs = new BufferedStream(ms))
                    {
                        int i = 0;
                        foreach (Imgsubject I in _Imgsubjects.Subjects)
                        {
                            Rectangle r = I.Rect;
                            r.Offset( S.SrcCorrectRect.Location );
                            Bitmap img = S.Src.Clone(r, S.Src.PixelFormat);
                            img.Save(bs, System.Drawing.Imaging.ImageFormat.Tiff);
                            bs.Flush();
                            byte[] buff = ms.ToArray();
                            bsf[i].Write(buff, 0, buff.Length);
                            //di[i].Add(new datainfo(kh, startpos[i], bs.Length));
                            L.Add(bs.Length);
                            startpos[i] += bs.Length;
                            i++;
                        }
                    }
                }
            }

            for (int i = 0; i < _Imgsubjects.Subjects.Count; i++)
            {
                bsf[i].Flush();
                bsf[i].Close();
                //fs[i].Flush();
                fs[i].Close();
            }
            SaveIDIndex(_IDInfo, bindatapath + "idindex.json");
            //StringBuilder sb = new StringBuilder();
            //foreach(List<long> L in LL)
            //    sb.AppendLine( string.Join(",",L));
            //File.WriteAllText(bindatapath + "length.txt", sb.ToString());
            MessageBox.Show("以保存好BitmapBindata");
        }
        private void SaveIDIndex(ImgbinDataInfo IDIndex, string filename)
        {
            string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(IDIndex));
            File.WriteAllText(filename, str);
        }       
        public void SaveBitmapFixedDataToData(string bindatapath)
        {
            List<FileStream> fs = new List<FileStream>();
            List<BufferedStream> bsf = new List<BufferedStream>();
            List<long> startpos = new List<long>();
            for (int i = 0; i < _Imgsubjects.Subjects.Count; i++)
            {
                Imgsubject S = _Imgsubjects.Subjects[i];
                if (File.Exists(bindatapath + "subjectfixed_" +S.ID + ".data"))
                    File.Delete(bindatapath + "subjectfixed_" + S.ID + ".data");
                fs.Add(new FileStream(bindatapath + "subjectfixed_" + S.ID + ".data", FileMode.Append, FileAccess.Write));
                startpos.Add(0);
                bsf.Add(new BufferedStream(fs[i], 102400));
            }
            Dictionary<int, int> IDIndex = new Dictionary<int, int>();
            List<List<long>> LL = new List<List<long>>();
            int index = 0;
            foreach (Student S in _Students.students)
            {
                List<long> L = new List<long>();
                LL.Add(L);
                IDIndex[S.ID] = index;
                S.Index = index;
                index++;
                BitmapData bmpdata = S.Src.LockBits(new Rectangle(0, 0, S.Src.Width, S.Src.Height), ImageLockMode.ReadOnly, S.Src.PixelFormat);
                int i = 0;
                foreach (Imgsubject I in _Imgsubjects.Subjects)
                {
                    Rectangle r = I.Rect;
                    r.Offset(S.SrcCorrectRect.Location);
                    r.X = (r.X / 8) * 8 + (r.X % 8 > 2 ? 8 : 0);
                    r.Width = (r.Width / 8) * 8 + (r.Width % 8 > 2 ? 8 : 0);
                    int stride = r.Width / 8;
                    stride = (stride / 4 * 4) + (stride % 4 > 0 ? 4 : 0);
                    byte[] buff = new byte[stride * r.Height];

                    unsafe
                    {
                        byte* bmpPtr = (byte*)bmpdata.Scan0.ToPointer();
                        bmpPtr += r.Y * bmpdata.Stride + r.X / 8;

                        for (int y = 0; y < r.Height; y++)
                        {
                            for (int k = 0; k < r.Width / 8; k++)
                            {
                                buff[y * (stride) + k] = bmpPtr[k];
                            }
                            bmpPtr += bmpdata.Stride;
                        }
                    }

                    bsf[i].Write(buff, 0, buff.Length);
                    L.Add(buff.Length);
                    //L.Add(bsf[i].Length);
                    //bsf[i].Flush();
                    startpos[i] += buff.Length;
                    i++;
                }
                S.Src.UnlockBits(bmpdata);
            }

            for (int i = 0; i < _Imgsubjects.Subjects.Count; i++)
            {
                bsf[i].Flush();
                bsf[i].Close();
                fs[i].Close();
            }
            _IDInfo = new ImgbinDataInfo();
            _IDInfo.IDIndex = IDIndex;
            _IDInfo.SubjectLengthInfo = new Dictionary<int, int>();
            if (LL.Count > 0)
            {
                for (int i = 0; i < LL[0].Count; i++)
                {
                    _IDInfo.SubjectLengthInfo[i] =(int) LL[0][i];
                }
            }
            SaveIDIndex(_IDInfo, bindatapath + "idinfo.json");
            //StringBuilder sb = new StringBuilder();
            //foreach (List<long> L in LL)
            //    sb.AppendLine(string.Join(",", L));
            MessageBox.Show("已保存好BitmapBinfixeddata");
        }
        public void TestReadBitmapData(string bindatapath, int roomcnt = -1, int savecnt = 5)
        {
            string idindexpath = bindatapath + "idindex.json";
            string lengthpath = bindatapath + "length.txt";
            Dictionary<int, int> IDIndex = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, int>>( File.ReadAllText(idindexpath));
            List<int> L = new List<int>() { 1484, 1908, 2052 };          

            int index = 0;
            foreach (Imgsubject I in _Imgsubjects.Subjects)
            {
                Rectangle r = I.Rect;
                r.X = (r.X / 8) * 8 + (r.X % 8 > 2 ? 8 : 0);
                r.Width = (r.Width / 8) * 8 + (r.Width % 8 > 2 ? 8 : 0);
                //ReadBitmapdata(IDIndex,L, index, r, savecnt);
                index++;
                if (roomcnt == index) break;
            }
        }
        private static void ConstructImgData(Students students, ref Rectangle imgrect)
        {
            FileStream fs = new FileStream("img.data", FileMode.Append, FileAccess.Write);
            MemoryStream ms = new MemoryStream();
            BufferedStream bs = new BufferedStream(ms, 40960);

            int startpos = 0;
            for (int cnt = 0; cnt <students.students.Count; cnt++)
            {               
                Bitmap imgc = students.students[cnt].Src.Clone(imgrect, students.students[cnt].Src.PixelFormat);
                imgc.Save(bs, System.Drawing.Imaging.ImageFormat.Tiff);
                bs.Flush();
                byte[] buff = ms.ToArray();
                fs.Write(buff, 0, buff.Length);

                startpos += (int)bs.Length;
                if (buff.Length != bs.Length)
                {
                    System.Windows.Forms.MessageBox.Show("Error: buff.length=" + buff.Length + ",bs.length=" + bs.Length);
                }
            }
            ms.Flush();
            bs.Flush();
            fs.Flush();
            ms.Close();
            bs.Close();
            fs.Close();
        }

        private Students _Students;
        private Imgsubjects _Imgsubjects;
        //for LoadBinData
		private FileStream _fs;
        private BufferedStream _bs;
		private byte[] _buffer;
        private string _bindatapath;
        private ImgbinDataInfo _IDInfo;
    }
    public class ImgbinDataInfo
    {
        public Dictionary<int, int> IDIndex;
        public Dictionary<int, int> SubjectLengthInfo;
    }
   [JsonObject(MemberSerialization.OptIn)]
    public class StudentsResult
    {
        public Imgsubject ActiveSubject { get { return _activesubject; } }
        public List<Student> Students { get; set; }        
        public StudentsResult(FormYJ.Students _Students, FormYJ.Imgsubjects _Imgsubjects, Optionsubjects _Optionsubjects,Tzsubjects _Tzsubjects, string _workpath)
        {
            this._Students = _Students;
            this._Imgsubjects = _Imgsubjects;
            this._workpath = _workpath;
            this._Optionsubjects = _Optionsubjects;
            this._Tzsubjects = _Tzsubjects;
            _Result = new List<List<int>>();
            for (int i = 0; i < _Imgsubjects.Subjects.Count; i++)
            {
                List<int> L = new List<int>();
                for (int index = 0; index < _Students.students.Count; index++)
                {
                    L.Add(-index - 1);
                }
                _Result.Add(L);
            }
            _Ims = new ImgbinManagesubjects(_Students, _Imgsubjects);
            _Ims.InitLoadBindata(_workpath);
        }
        public StudentsResult(FormYJ.Students _Students, Imgsubjects _Imgsubjects, Optionsubjects _Optionsubjects,Tzsubjects _Tzsubjects, string Path, List<List<int>> result)
        {
            this._Students = _Students;
            this._Imgsubjects = _Imgsubjects;
            this._Optionsubjects = _Optionsubjects;
            this._Tzsubjects = _Tzsubjects;
            this._workpath = Path;
            this._Result = result;
            _Ims = new ImgbinManagesubjects(_Students, _Imgsubjects);
            _Ims.InitLoadBindata(_workpath);
            if (!_Students.CheckIndex())
                MessageBox.Show("index Error");
        }

        public void SetActiveSubject(Imgsubject S)
        {
            this._activesubject = S;
            _Ims.SetActiveSubject(S);
        }
        public void SetScoreByKh(Student S, int Score)
        {
            _Result[_activesubject.Index][S.Index] = Score;
        }
        public Bitmap GetBitMap(Student S)
        {
            return _Ims.ActiveSubjectBitmap(S);
        }
      
        public void LoadNextStudents(bool back = false)
        {
            if (back)
            {
                int index = -1;
                List<int> _back = _Result[_activesubject.Index].Select(r =>
                {
                    index++;
                    return r >= 0 ? index : -1;
                }).ToList(); 
                Students = _back.Where(r => r >= 0).Select(r =>{
                    Student s = _Students.students[r];
                    s.BackScore = _Result[_activesubject.Index][r];
                    return s;
                }).ToList();
            }
            else
                Students = _Result[_activesubject.Index].Where(r => r < 0).Select(r => _Students.students[-r - 1]).ToList();
            if(global.Debug && (global.tag & 2)>0)
                if( Students.Count>0){
                    string str = "\r\n\r\nbefore:ID姓名：,"+ string.Join(",", Students.Select(r => r.ID + r.Name)) + "\r\nSortValue"
                        + string.Join(",", Students.Select(r => r.Sort.SortValue.ToString()));
                    File.AppendAllText( "F:\\Sortdebug.txt",str );
                }
            if(back)
                Students.Sort(delegate(Student S1, Student S2)
                {
                    return S1.BackScore - S2.BackScore;
                });
            else
                Students.Sort(delegate(Student S1, Student S2)
                {
                    return S1.Sort.SortValue - S2.Sort.SortValue;
                });
            if(global.Debug && (global.tag & 4)>0)
                if( Students.Count>0){
                    string str = "\r\n\r\nsorted:ID姓名：," + string.Join(",", Students.Select(r => r.ID + r.Name)) + "\r\nSortValue"
                        + string.Join(",", Students.Select(r => r.Sort.SortValue.ToString()));
                    File.AppendAllText( "F:\\Sortdebug.txt",str );
                }
        }
        public Imgsubjects GetImgsubjects()
        {
            return _Imgsubjects;
        }
        public List<List<int>> Result{  get{return _Result;}}
        private ImgbinManagesubjects _Ims;
        private Imgsubject _activesubject;
        [JsonProperty]
        private string _workpath;
        [JsonProperty]
        private FormYJ.Students _Students;
        [JsonProperty]
        private FormYJ.Imgsubjects _Imgsubjects;
        [JsonProperty]
        private Optionsubjects _Optionsubjects;
        [JsonProperty]
        private Tzsubjects _Tzsubjects;
        [JsonProperty]
        private List<List<int>> _Result;
    }
    public class Exam
    {
        private Students _Students;
        private Imgsubjects _Imgsubjects;
        private StudentsResult _SR;
        private Optionsubjects _Optionsubjects;
        private Tzsubjects _Tzsubjects;
        public Exam(Students _Students, Imgsubjects _Imgsubjects, Optionsubjects _Optionsubjects,Tzsubjects _Tzsubjects,  string path )
        {
            this._Students = _Students;
            this._Imgsubjects = _Imgsubjects;
            this._Optionsubjects = _Optionsubjects;
            this._Tzsubjects = _Tzsubjects;
            this.Path = path;
            _SR = new StudentsResult(_Students, _Imgsubjects,_Optionsubjects,_Tzsubjects, path);
        }
        public Exam(Examdata ed)
        {
            this.Name = ed.Name;
            this.Path = ed.Path;
            _Imgsubjects = ed.SR._Imgsubjects;
            _Students = ed.SR._Students;
            _Optionsubjects = ed.SR._Optionsubjects;
            _Tzsubjects = ed.SR._Tzsubjects;
            this._SR = new StudentsResult(_Students, _Imgsubjects, _Optionsubjects,_Tzsubjects, Path,ed.SR._Result);
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public StudentsResult SR { get { return _SR; } }
        [JsonIgnore]
        public List<Imgsubject> Subjects { get { return _Imgsubjects.Subjects; } }        
        [JsonIgnore]
        public List<Optionsubject> OSubjects { get { return _Optionsubjects.OptionSubjects; } }
        [JsonIgnore]
        public  Tzsubjects TzSubjects { get { return _Tzsubjects; } }
    }
}
