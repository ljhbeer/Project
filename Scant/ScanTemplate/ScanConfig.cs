using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using ARTemplate;
using System.Threading;
using System.Text;
using ZXing.Common;
using ZXing;

namespace ScanTemplate
{
    public delegate void DelegateShowScanMsg(string msg);
    public delegate void DelegateSaveScanData(string data);
    public class Config
    {
        public Config()
        {
            _examinfo = new List<ExamInfo>();
        }
        public void SetWorkPath(string exampath)
        {
            this._workpath = exampath;
            Studentbases = new ScanTemplate.FormYJ.StudentBases(StudentBaseFileName());
            LoadConfig();
        }
        public bool CheckExamInfoName(ExamInfo ei)
        {
            if (!_examinfo.Exists(r => r.Name == ei.Name))
            //|| !_examinfo.Exists( r=> r.Path == ei.Path) )
            {
                ei.Number = 1001;
                if (_examinfo.Count > 0)
                    ei.Number = _examinfo.Max(r => r.Number) + 1;
                ei.Path = _workpath + "\\" + ei.Number + "\\";
                return true;
            }
            return false;
        }
        public void AddExamInfo(ExamInfo ei)
        {
            _examinfo.Add(ei);
        }
        public void SaveConfig(string filename = "")
        {
            string fn = _filename;
            if (filename != "")
                fn = filename;
            if (fn != "")
            {
                if (!fn.Contains(":"))
                    fn = _workpath + "\\" + fn;
                _filename = fn;
                string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(this));
                File.WriteAllText(_filename, str);
            }
        }
        public string ExamPath
        {
            get { return _workpath; }
        }
        public ScanTemplate.FormYJ.StudentBases Studentbases { get; set; }
        private void LoadConfig()
        {
            string filename = _workpath + "\\config.json";
            if (File.Exists(filename))
            {
                this._filename = filename;
                Config f = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(_filename));
                //_examinfo = f._examinfo;
                //_workpath = f._workpath;
            }
        }
        private string StudentBaseFileName()
        {
            return _workpath.Replace("Exam", "StudentBaseList.txt");
        }
        public List<ExamInfo> _examinfo;
        private string _workpath;
        private string _filename;
    }
    public class ScanConfig
    {
        private UnScans _unscans;
        private Templates _commontemplates;
        private ScanDatas _scandatas;
        private ZXing.BarcodeReader _br;
        public ScanConfig(string workpath) //E:\Scan\s1025
        {
            Baseconfig = new BaseConfig(workpath);
            _unscans = new UnScans(Baseconfig.UnScanPath);
            _commontemplates = new Templates(Baseconfig.TemplatePath);
            _scandatas = new ScanDatas(Baseconfig.ScanDataPath);

            //for 二维码
            DecodingOptions decodeOption = new DecodingOptions();
            decodeOption.PossibleFormats = new List<BarcodeFormat>() {
				BarcodeFormat.All_1D
			};
            _br = new BarcodeReader();
            _br.Options = decodeOption;
        }
        public UnScans Unscans { get { return _unscans; } }
        public Templates CommonTemplates { get { return _commontemplates; } }
        public ScanDatas Scandatas { get { return _scandatas; } }
        public TemplateShow Templateshow { get; set; }   
        public BaseConfig Baseconfig { get; set; }
        public ZXing.BarcodeReader BR { get { return _br; } }
    }
    public class BaseConfig
    {
        private string _workpath;
        public BaseConfig(string workpath) //  //E:\Scan\s1025
        {
            this._workpath = workpath;
        }
        public string Workpath { get { return _workpath; } }
        public string UnScanPath { get { return _workpath; } }
        public string ScanDataPath { get { return _workpath.Replace("\\s1025","\\LJH\\s1025"); } }
        public string TemplatePath { get { return _workpath.Replace("\\s1025", "\\LJH\\Template"); } }
        public string ExamPath { get { return _workpath.Replace("\\s1025","\\LJH\\Exam"); } }
        public string CorrectImgPath { get { return _workpath.Replace("\\s1025","\\LJH\\Correct\\s1025"); } }
    }
    public class UnScans
    {
        private string _path;
        private List<UnScan> _unscans;

        public UnScans(string path)
        {
            this._path = path;
            _unscans = new List<UnScan>();
            foreach (string dirname in Tools.FileTools.SubDirNameListFromDir(path))
                Unscans.Add(new UnScan(dirname, path));
        }
        public List<UnScan> Unscans { get { return _unscans; } }
    }
    public class UnScan
    {
        private string dirname;
        private string path;

        public UnScan(string dirname, string path)
        {
            this.dirname = dirname;
            this.path = path;
        }
        public override string ToString()
        {
            return dirname;
        }
        public string DirName { get { return dirname; } }
        public string FullPath { get { return path + "\\" + dirname; } }
        public List<string> ImgList()
        {
            return Tools.FileTools.NameListFromDir(FullPath);
        }
    }
    public class ScanDatas
    {
        private List<ScanData> _scandatas;
        private string path;
        public ScanDatas(string path)
        {
            this.path = path;
            _scandatas = new List<ScanData>();
            foreach (string dirname in Tools.FileTools.SubDirNameListFromDir(path))
            {
                Scandatas.Add(new ScanData(dirname, path));
            }
        }
        public List<ScanData> Scandatas { get { return _scandatas; } }
    }
    public class ScanData
    {
        private string dirname;
        private string path;
        public ScanData(string dirname, string path)
        {
            this.dirname = dirname;
            this.path = path;
        }
        public override string ToString()
        {
            return dirname;
        }
    }
    public class Templates
    {
        private List<TemplateInfo> _commonTemplates;
        private string _commontemplatespath;
        public Templates(string commontemplatespath)
        {
            this._commontemplatespath = commontemplatespath;
            _commonTemplates = new List<TemplateInfo>();
            foreach (string filename in Tools.FileTools.NameListFromDir(commontemplatespath, ".xml"))
            {
                _commonTemplates.Add(new TemplateInfo(filename,_commontemplatespath));
            }
        }
        public List<TemplateInfo> CommonTemplates { get { return _commonTemplates; } }
    }
    public class TemplateInfo
    {
        private string _filename;
        private string _path;
        
        public TemplateInfo(string filename, string _commontemplatespath)
        {
            this._path = _commontemplatespath;
            this._filename = filename.Substring(_path.Length+1);
        }
        public override string ToString()
        {
            return _filename;
        }
        public string TemplateFileName { get { return _path + "\\" + _filename; } }
    }
    public class TemplateShow
    {
        private string _imgfilename;
        private Bitmap _src;
        private MyDetectFeatureRectAngle _dr;
        private Template _artemplate;
        private AutoAngle _angle;
        private string _fullpath;
        private string _dirname;
        private TemplateInfo ti;
    
        public TemplateShow(string fullpath,string dirname, string imgfilename, TemplateInfo ti=null)
        {
            this._fullpath = fullpath;
            this._imgfilename = imgfilename;
            this._dirname = dirname;
            this.ti = ti;
            this._src = (Bitmap)Bitmap.FromFile(_imgfilename);
            
            AutoDetectRectAnge.FeatureSetPath = _fullpath;
            this._dr = new MyDetectFeatureRectAngle(_src);
            this.OK = CreateTemplate();
        }

        private bool CreateTemplate()
        {
            if (_dr.Detected())
            {
                //TODO: autoDetectRectAngle set static
                    _artemplate = new ARTemplate.Template(_imgfilename, _src, _dr.CorrectRect);
                    if (ti != null)
                        _artemplate.Load(ti.TemplateFileName);
                List<Point> zeroListPoint = new List<Point>();
                for (int i = 0; i < _dr.ListPoint.Count; i++)
                {
                    zeroListPoint.Add(new Point(_dr.ListPoint[i].X - _dr.CorrectRect.X, _dr.ListPoint[i].Y - _dr.CorrectRect.Y));
                }
                _angle = new AutoAngle(zeroListPoint);
                _artemplate.SetFeaturePoint(_dr.ListFeatureRectangle, _dr.CorrectRect);
                return true;
            }
            return false;
        }
        public Template Template { get { return _artemplate; } }
        public bool OK { get; set; }
    }
    public class Scan
    {
        private ScanConfig _sc;
        private MyDetectFeatureRectAngle _dr;
        private Template _template;
        private AutoAngle _angle;
        private string _templatename;
        private List<string> _nameList;
        private string _dirname;
        private string _exportdata;
        private Dictionary<string, int> _titlepos;
        private int _xztpos;

        public DelegateShowScanMsg DgShowScanMsg;
        public DelegateSaveScanData DgSaveScanData;
        private string _srcpath;
        public Scan(ScanConfig sc,string templatename, List<string> nameList,string fulldirpath)
        {
            this.DgSaveScanData = null;
            this.DgShowScanMsg = null;
            _xztpos = -1;
            _titlepos = null;
            this._sc = sc;
            this._templatename = templatename;
            this._nameList = nameList;
            this._dirname = fulldirpath.Substring(fulldirpath.LastIndexOf("\\")+1);
            this._srcpath = fulldirpath;
            Template t = new Template(_templatename);
            _dr = new MyDetectFeatureRectAngle((Bitmap)Bitmap.FromFile(t.Filename));
            if (!Directory.Exists(CorrectPath))
                Directory.CreateDirectory(CorrectPath);
            InitTemplate();
        }
        public void DoScan()
        {
            Thread thread = new Thread(new ThreadStart(RunScan));
            thread.Start();
        }
        public void RunScan()
        {
            Msg = "";
            StringBuilder sb = new StringBuilder();
			foreach (string s in _nameList)
			{
				string _runmsg = DetectImg(s);
				sb.Append(_runmsg);
                if(DgShowScanMsg!=null)
                DgShowScanMsg(_runmsg);//this.Invoke(new MyInvoke(ShowMsg));
				Thread.Sleep(10);
			}
			_exportdata = sb.ToString();
            if (DgSaveScanData != null)//this.Invoke(new MyInvoke(ExportData));
                DgSaveScanData(_exportdata);
        }
        private string DetectImg(string s)
        {
            StringBuilder sb = new StringBuilder();
            Bitmap orgsrc = (Bitmap)Bitmap.FromFile(s);
            List<Rectangle> TBO = new List<Rectangle>();
            Rectangle CorrectRect = _dr.Detected(orgsrc, TBO);
            if (CorrectRect.Width > 0 && TBO.Count == 3)
            {
                if (!SetAngle(orgsrc, TBO,CorrectRect))
                {
                    Msg += s + "检测特征点B失败\r\n";
                    return "";
                }
                sb.Append(s + "," + CorrectRect.ToString("-"));// 文件名 , CorrectRect
                sb.Append("," + _angle.Angle2 ); //校验角度

                Bitmap src = (Bitmap)orgsrc.Clone(CorrectRect, orgsrc.PixelFormat);
                src.Save(CorrectPath + s.Substring(s.LastIndexOf("\\")));
                AutoComputeXZTKH acx = new AutoComputeXZTKH(_template,src);
                if (_template.HasOptions("考号"))
                {
                    KaoHaoChoiceArea kha = (KaoHaoChoiceArea)(_template.Dic["考号"][0]);
                    if (kha.Type == "条形码")
                    {
                        Rectangle Ir = kha.ImgArea;
                        Bitmap barmap = (Bitmap)src.Clone(kha.ImgArea, src.PixelFormat);
                        //barmap.Save("f:\\aa.tif");
                        //Ir.Offset(CorrectRect.Location);
                        ZXing.Result rs = _sc.BR.Decode(barmap);
                        if (rs != null)
                        {
                            sb.Append("," + rs.Text + ",-");  //考号-条形码 姓名-未知 MsgtoDr中处理
                        }
                    }
                    else if ("1023456789".Contains(kha.Type))
                    {
                        sb.Append("," + acx.ComputeKH(kha, _angle) + ",-");   //考号-涂卡 姓名-未知 MsgtoDr中处理
                    }
                }
                string str = s.Substring(s.Length - 7, 3);
                sb.Append("," + acx.ComputeXZT(str,_angle)); //选择题
                //计算座位号
                if (_template.HasOptions("自定义"))
                {
                    StringBuilder tsb = new StringBuilder();
                    foreach (Area I in _template.Dic["自定义"])
                    {
                        CustomArea ca = (CustomArea)I;
                        if ("1023456789".Contains(ca.Type))
                        {
                            AutoComputeXZTKH acxzdy = new AutoComputeXZTKH(_template, src);
                            //sb.Append("," + acx.ComputeCustomDF(ca, _angle, nbmp));
                            tsb.Append(acx.ComputeCustomDF(ca, _angle) + "|");
                        }
                    }
                    sb.Append("," + tsb); //自定义
                }
            }
            else
            {
                //检测失败
            }
            sb.AppendLine();
            return sb.ToString();
        }
        private bool SetAngle(Bitmap orgsrc, List<Rectangle> TBO, Rectangle CorrectRect)
        {
            Rectangle T = TBO[0];
            Rectangle B = _dr.Detected(TBO[1], orgsrc);
            Rectangle O = new Rectangle();
            O = _dr.Detected(TBO[2], orgsrc);
            if (B.Width == 0)
            {
                Rectangle R = TBO[1];
                R.Inflate(R.Width / 2, R.Height / 5);
                //bmpB = (Bitmap)bmp.Clone(R, bmp.PixelFormat);
                B = _dr.Detected(R, orgsrc);
                if (B.Width == 0)
                {
                    return false;
                    //MessageBox.Show("检测特征点B失败");
                    //Msg += s + "检测特征点B失败\r\n";
                    //return "";
                }
            }
            Point offset = new Point(-CorrectRect.X, -CorrectRect.Y);
            T.Offset(offset);
            B.Offset(offset);
            O.Offset(offset);
            _angle.SetPaper(T.Location, B.Location, O.Location);
            return true;
        }
        public string CorrectPath { get { return _sc.Baseconfig.CorrectImgPath + "\\" + _dirname; } }
        public List<string> ExportTitles { get { return _template.GetTitles(); } }
        public List<string> ColNames
        {
            get
            {
                InitTitlePos();
                List<string> colnames = new List<string> { "序号" };
                colnames.AddRange(ExportTitles);
                if (colnames.Contains("选择题"))
                {
                    colnames.Remove("选择题");
                    for (int i = 0; i <_template.XztRect.Count; i++)
                        colnames.Add("x" + (i + 1));
                }
                return colnames;
            }
        }

        private void InitTitlePos()
        {
            if (_titlepos == null)
            {
                _titlepos = ConstructTitlePos(ExportTitles);
                if (_titlepos.ContainsKey("选择题"))
                    _xztpos = _titlepos["选择题"];
                _titlepos.Remove("选择题");
            }
        }
        private Dictionary<string, int> ConstructTitlePos(List<string> Titles)
        {
            Dictionary<string, int> titlepos = new Dictionary<string, int>();
            for (int i = 0; i < Titles.Count; i++)
            {
                titlepos[Titles[i]] = i;
            }
            return titlepos;
        }
        private void InitTemplate()
        {
            Template t = new Template(_templatename);
            if (t.Image != null)
            {
                _template = t;
                List<Rectangle> listrect = new List<Rectangle>();
                foreach (Area I in t.Dic["特征点"])
                {
                    listrect.Add(I.ImgArea);
                }
                if (listrect.Count == 3)
                {
                    AutoDetectRectAnge adr = new AutoDetectRectAnge();
                    adr.ComputTBO(listrect);
                    _angle = new AutoAngle(adr.TBO());
                }
            }
        }
        public string Msg { get; set; }
        public Dictionary<string, int> Titlepos { get { return _titlepos; } }
        public int Xztpos { get { return _xztpos; } }
        public string ScanDataPath { get { return _sc.Baseconfig.ScanDataPath + "\\"+ _dirname; } }
        public string DirName { get { return _dirname; } }
        public string SourcePath { get { return _srcpath; } }
        public string TemplateName { get { return _templatename; } }
    }
    public class ValueTag
    {
        public ValueTag(string value, Object tag)
        {
            this.Value = value;
            this.Tag = tag;
        }
        public Object Tag;
        public String Value;
        public override string ToString()
        {
            return Value;
        }
    }
    public class ExamInfo
    {
        public string Name;
        public string Path;
        public int Number;
        public string TemplateFileName;
        public override string ToString()
        {
            return Name + "_" + Number + "_" + Path; ;
        }
    }
}
