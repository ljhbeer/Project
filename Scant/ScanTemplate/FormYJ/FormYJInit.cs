using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ARTemplate;

namespace ScanTemplate.FormYJ
{
    public partial class FormYJInit : Form
    {
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
        public FormYJInit(Template _artemplate, DataTable _rundt, AutoAngle _angle)
        {
            // TODO: Complete member initialization
            this._artemplate = _artemplate;
            this._rundt = _rundt;
            this._angle = _angle;
            this._src = _artemplate.Image;
            InitializeComponent();
            InitStudents();
            InitImgSubjects();
            dgv.DataSource = _rundt;
            InitImage();
        }
        private void InitImgSubjects()
        {
            _Imgsubjects = new Imgsubjects();
            int ID = 0;
            foreach (Area I in _artemplate.Dic["非选择题"])
            {
                UnChoose U = (UnChoose)I;
                Imgsubject S = new Imgsubject(U,ID);
                if( _Imgsubjects.Add(S) )
                ID++;
            }
        }
        private void InitStudents()
        {
            _Students = new Students(_rundt);
        }
        private void InitImage()
        {
            Bitmap bmp = (Bitmap)_artemplate.Image.Clone();
            pictureBox1.Image = ARTemplate.TemplateTools.DrawInfoBmp(bmp,_artemplate,_angle);
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
            }
            else
            {
                dgvSet.RowTemplate.Height = _AvgUnImgHeight;
                dgvSet.DataSource = _dtsetfxzt;
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
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "ID", "题组名称", "最大分值", "正确答案" });
            int cnt = 0;
            foreach (Area I in _artemplate.Dic["选择题"])
            {
                SingleChoiceArea  U = (SingleChoiceArea)I;
                if (I.HasSubArea())
                {
                    foreach (List<Point> lp in ((SingleChoiceArea)I).list)
                    {
                        Rectangle r = I.ImgArea;
                        DataRow dr = dtset.NewRow();
                        dr["ID"] =cnt++;
                        dr["题组名称"] ="x"+cnt;
                        dr["最大分值"] =1;
                        //dr["图片"] = _src.Clone(U.ImgArea, _src.PixelFormat);
                        dtset.Rows.Add(dr);
                    }
                }
            }
        }
        private void AddUnChooseTodtset(ref DataTable dtset)
        {
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "题组名称", "最大分值", "图片" });
            _AvgUnImgWith = 0;
            _AvgUnImgHeight = 0;

            foreach (Imgsubject S in _Imgsubjects.Subjects)
            {
                DataRow dr = dtset.NewRow();
                dr["OID"] = new ValueTag(S.ID.ToString(), S);
                dr["题组名称"] = S.Name;
                dr["最大分值"] = S.Scores;
                _AvgUnImgHeight += S.Height;
                _AvgUnImgWith += S.Width;
                dr["图片"] = _src.Clone(S.Rect, _src.PixelFormat);
                dtset.Rows.Add(dr);
            }           
            _AvgUnImgHeight /= _Imgsubjects.Subjects.Count;
            _AvgUnImgWith /= _Imgsubjects.Subjects.Count;
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
            MultiBitmapToData mbt = new MultiBitmapToData(_artemplate, _rundt, _dtsetfxzt);

            //FormFullScreenYJ f = new FormFullScreenYJ(_artemplate, _rundt);
        }
        private void buttonImportOptionAnswerScore_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            foreach(DataRow dr in _dtsetxzt.Rows)
                ids.Add("xz"+ dr["ID"].ToString());
            FormSetscore f = new FormSetscore(ids);
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < f.Xzt().Count; i++)
                {
                    if(f.Xzt()[i].ID.ToString().EndsWith( 
                    _dtsetxzt.Rows[i]["ID"].ToString() ))
                    {
                        _dtsetxzt.Rows[i]["正确答案"] = f.Xzt()[i].OptionAnswer;
                        _dtsetxzt.Rows[i]["最大分值"] = f.Xzt()[i].Score;
                    }

                }
            }
        }
    }
    public class Imgsubjects
    {
        List<Imgsubject> _subjects;
        Dictionary<int, Imgsubject> _dic;
        public Imgsubjects()
        {
            _subjects = new List<Imgsubject>();
            _dic = new Dictionary<int, Imgsubject>();
        }
        public bool Add(Imgsubject S)
        {
            if (!_dic.ContainsKey(S.ID))
            {
                _subjects.Add(S);
                _dic[S.ID] = S;
                return true;
            }
            return false;

        }
        public List<Imgsubject> Subjects
        {
            get
            {
                return _subjects;
            }
        }
        public Imgsubject ImgSubjectByID(int ID)
        {
            if (_dic.ContainsKey(ID))
                return _dic[ID];
            return null;
        }
    }
    public class Imgsubject
    {      
        public Imgsubject(UnChoose U, int ID)
        {
            // TODO: Complete member initialization
            this.U = U;
            this.ID = ID;
            this.Name = U.Name;
            this.Subid = ID;
            this.MaxResult = U.Scores;
            this.Rect = U.Rect;
            this.BitmapdataLength = -1;
        }
        public int Subid;
        public Double MaxResult;
        public string Name;
        public Rectangle Rect;
        public int BitmapdataLength;
        private UnChoose U;
        public int ID;
        public override string ToString()
        {
            return Name;
        }

        public int Scores { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
    public class Students
    {
        private List<Student> _students;
        private DataTable _studt;
        private Dictionary<int, Student> _xhdic;
        private Dictionary<int, Student> _khdic;

        public Students(DataTable _rundt)
        {
            this._studt = _rundt;
            int cnt = 0;
            foreach (DataColumn dc in _rundt.Columns)
                if (dc.ColumnName.StartsWith("x"))
                    cnt++;
            _students = new List<Student>();
            _xhdic = new Dictionary<int, Student>();
            _khdic = new Dictionary<int, Student>();
            foreach (DataRow dr in _rundt.Rows)
            {
                Student s = new Student(dr,cnt);
                _students.Add(s);
                _xhdic[s.ID] = s;
                _khdic[s.KH] = s;
            }
        }
        public Student StudentFromXh(int ID)
        {
            return _xhdic[ID];
        }
        public Student StudentFromKh(int KH)
        {
            return _khdic[KH];
        }

    }
    public class Student
    {
        public Student(DataRow dr,int XZTcount)
        {
            // TODO: Complete member initialization
            this.ID = Convert.ToInt32(dr["序号"].ToString());
            this._imgfilename = dr["文件名"].ToString();
            this.Angle = Convert.ToDouble( dr["校验角度"].ToString());
            this.Name =  dr["姓名"].ToString();
            this.KH = Convert.ToInt32(dr["考号"].ToString());
            this._XZT = new List<string>();
            for (int i = 1; i < XZTcount+1; i++)
            {
                _XZT.Add(dr["x" + i].ToString());
            }
            _src = null;
        }

        public int ID { get; set; }
        public double Angle { get; set; }
        public int KH { get; set; }
        public string Name { get; set; }
        public Bitmap Src
        {
            get
            {
                if (_src == null)
                {
                    if (System.IO.File.Exists(_imgfilename))
                        _src =(Bitmap) Bitmap.FromFile(_imgfilename);
                }
                return _src;
            }
        }
        private string _imgfilename;
        private Bitmap _src;
        private List<string> _XZT;
    }
}
