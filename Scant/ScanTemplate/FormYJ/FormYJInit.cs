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

namespace ScanTemplate.FormYJ
{
    public partial class FormYJInit : Form
    {
        public FormYJInit(Template _artemplate, DataTable _rundt, AutoAngle _angle, string _workpath)
        {
            // TODO: Complete member initialization
            this._artemplate = _artemplate;
            this._rundt = _rundt;
            this._angle = _angle;
            this._src = _artemplate.Image;
            this._workpath = _workpath;
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
                dr["最大分值"] = S.Score;
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
            //FormFullScreenYJ f = new FormFullScreenYJ(_artemplate, _rundt);
            ImgbinManagesubjects ims = new ImgbinManagesubjects(_Students, _Imgsubjects);
            string path = _workpath.Replace("\\LJH","\\LJH\\bindata")  + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            ims.SaveBitmapFixedDataToData(path);
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
        private string _workpath;
    }
    public class Imgsubjects
    {
        List<Imgsubject> _subjects;
        Dictionary<int, Imgsubject> _dic;
        private int _activeid;
        public Imgsubjects()
        {
            _subjects = new List<Imgsubject>();
            _dic = new Dictionary<int, Imgsubject>();
            _activeid = -1;
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
        public Imgsubject ActiveSubject
        {
            get
            {
                return ImgSubjectByID(_activeid);
            }
        }
        public void SetActiveSubject(Imgsubject S)
        {
            if (S != null)
                _activeid = S.ID;
            else
                _activeid = -1;
        }
    }
    public class Imgsubject
    {      
        public Imgsubject(UnChoose U, int ID)
        {
            this._U = U;
            this.ID = ID;
            this.Score = U.Scores;
            this.Name = U.Name;
        }
        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; } 
        public int ID { get; set; }
        public int Score { get; set; }
        public Rectangle Rect { get { return _U.ImgArea; } }
        public int Height { get { return Rect.Height; } }
        public int Width { get { return Rect.Width; } }
        private UnChoose _U;
        //public int ID;
        //public Double Score;
        public int BitmapdataLength{ get; set; }
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
        public List<Student> students
        {
            get
            {
                return _students;
            }
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
    public class ImgbinManagesubjects
    {
        public ImgbinManagesubjects(Students _Students, Imgsubjects _Imgsubjects)
        {
            // TODO: Complete member initialization
            this._Students = _Students;
            this._Imgsubjects = _Imgsubjects;
            _fs = null;
            _bs = null;
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
        public void InitLoadBindata(string workpath)
        {
            this._bindatapath = workpath;
             //for (int i = 0; i <_Imgsubjects.Subjects.Count; i++)
             //    _Imgsubjects.Subjects[i].BitmapdataLength = (int)(di[i][0].count);
        }
        public bool SetActiveSubject(Imgsubject S)
        {
            _Imgsubjects.SetActiveSubject(S);
            if(_Imgsubjects.ActiveSubject==null)
                return false;          
            if (_bs != null || _fs != null)
            {
                _bs.Flush();
                _bs.Close();
                _fs.Close();
            }
            string imgdatafilename = _bindatapath .Replace("[roomid]", S.ID.ToString());
            _fs = new FileStream(imgdatafilename, FileMode.Open, FileAccess.Read);
            _bs = new BufferedStream(_fs, 512000);
            return true;
        }
        public Bitmap ActiveSubjectBitmap(Student S) //indexdic
        {
            int length = _Imgsubjects.ActiveSubject.BitmapdataLength;
            Rectangle r = _Imgsubjects.ActiveSubject.Rect;

            int index =  0; //_Students.
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
                            Bitmap img = S.Src.Clone(I.Rect, S.Src.PixelFormat);
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
            SaveIDIndex(IDIndex, bindatapath + "idindex.json");
            StringBuilder sb = new StringBuilder();
            foreach(List<long> L in LL)
                sb.AppendLine( string.Join(",",L));
            File.WriteAllText(bindatapath + "length.txt", sb.ToString());
            MessageBox.Show("以保存好BitmapBindata");
        }
        private void SaveIDIndex(Dictionary<int, int> IDIndex, string filename)
        {
            string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(IDIndex));
            File.WriteAllText(filename, str);
        }       
        public void SaveBitmapFixedDataToData(string bindatapath)
        {
            List<List<long>> LL = new List<List<long>>();
            List<FileStream> fs = new List<FileStream>();
            List<BufferedStream> bsf = new List<BufferedStream>();
            List<long> startpos = new List<long>();
            for (int i = 0; i < _Imgsubjects.Subjects.Count; i++)
            {
                if (File.Exists(bindatapath + "subjectfixed_" + i + ".data"))
                    File.Delete(bindatapath + "subjectfixed_" + i + ".data");
                fs.Add(new FileStream(bindatapath + "subjectfixed_" + i + ".data", FileMode.Append, FileAccess.Write));
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
                BitmapData bmpdata = S.Src.LockBits(new Rectangle(0, 0, S.Src.Width, S.Src.Height), ImageLockMode.ReadOnly, S.Src.PixelFormat);
                int i = 0;
                foreach (Imgsubject I in _Imgsubjects.Subjects)
                {
                    Rectangle r = I.Rect;
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
                
                    startpos[i] += buff.Length;
                    i++;
                }
            }

            for (int i = 0; i < _Imgsubjects.Subjects.Count; i++)
            {
                bsf[i].Flush();
                bsf[i].Close();
                fs[i].Close();
            }

            SaveIDIndex(IDIndex, bindatapath + "idindex.json");
            StringBuilder sb = new StringBuilder();
            foreach (List<long> L in LL)
                sb.AppendLine(string.Join(",", L));
            File.WriteAllText(bindatapath + "length.txt", sb.ToString());
            MessageBox.Show("已保存好BitmapBinfixeddata");
        }
        public void TestReadBitmapData(int roomcnt=-1,int savecnt=5)
        {
            //List<List<datainfo>> di = LoadDatainfo("datainfo" + activefloor.sID + ".json");
            int index = 0;
            foreach (Imgsubject I in _Imgsubjects.Subjects)
            //foreach (ScanTemplate.formimg.Room room in activefloor.Rooms)
            {
                Rectangle r = I.Rect;
                r.X = (r.X / 8) * 8 + (r.X % 8 > 2 ? 8 : 0);
                r.Width = (r.Width / 8) * 8 + (r.Width % 8 > 2 ? 8 : 0);

                //ReadBitmapdata(di[index],_bitmapdatapath, index, r, savecnt);
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
    }
}
