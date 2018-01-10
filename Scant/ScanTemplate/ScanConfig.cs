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
using Tools;

namespace ScanTemplate
{
    public delegate void DelegateShowScanMsg(string msg);
    public delegate void DelegateSaveScanData(string data);   
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
            //studentbase
            Studentbases = new FormYJ.StudentBases( Baseconfig.StudentList +"\\StudentBaseList.txt");
        }
        public UnScans Unscans { get { return _unscans; } }
        public Templates CommonTemplates { get { return _commontemplates; } }
        public ScanDatas Scandatas { get { return _scandatas; } }
        public TemplateShow Templateshow { get; set; }   
        public BaseConfig Baseconfig { get; set; }
        public ZXing.BarcodeReader BR { get { return _br; } }
        public FormYJ.StudentBases Studentbases { get; set; }
        //
        public ExamConfig Examconfig { get; set; }
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
        public string StudentList { get { return _workpath.Replace("\\s1025", "\\LJH"); } }

        public string ExportResultFxPath { get { return "F:\\out\\FX"; } }
        public string ExportResultPath { get { return "F:\\out\\FX"; } }
        public string ExportImageRootPath { get { return "F:\\out"; } }
    }
    public class ExamConfig
    {
        public ExamConfig()
        {
        }
        public void SetWorkPath(string exampath)
        {
            this._workpath = exampath;
            string filename = _workpath + "\\config.json";
            if (File.Exists(filename))
            {
                this._filename = filename;
                ExamConfig f = Newtonsoft.Json.JsonConvert.DeserializeObject<ExamConfig>(File.ReadAllText(_filename));
                _examinfo = f._examinfo;
                //_workpath = f._workpath;
            }
            else
            {
                _examinfo = new List<ExamInfo>();
            }
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
        public List<ExamInfo> _examinfo;
        private string _filename;
        private string _workpath;
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
        private string _dirname;
        private string _path;
        private string _templatename;
        private string _examname;
        public ScanData(string dirname, string path)
        {
            this._dirname = dirname;
            this._path = path;
            this._templatename = path + "\\" + dirname + "\\template.json";
           
            _examname = dirname;
            if (Directory.Exists(Fullpath))
            {
                List<string> examlist = Tools.FileTools.NameListFromDir(Fullpath, ".exam");
                if (examlist.Count > 0)
                {
                    _examname = examlist[0].Substring(Fullpath.Length + 1);
                    _examname = _examname.Substring(0, _examname.Length - 5);
                }
            }
        }
        public override string ToString()
        {
            if (File.Exists(Fullpath + "\\已生成阅卷数据.txt"))
                return _examname + " 已生成阅卷数据";
            return _examname;
        }
        public string Fullpath { get { return _path + "\\" + _dirname; } }
        public string Imgpath { get { return _path + "\\" + _dirname + "\\img"; } }
        public string DataFullName { get { return Fullpath + "\\data.txt"; } }
        public string ExamName { get { return _examname; } }
        public string TemplateFileName
        {
            get
            {
                if (!File.Exists(_templatename))
                    return "";
                return _templatename;
            }
        }
        public List<string> ImgList { get { return  Tools.FileTools.NameListFromDir(Imgpath,".tif");} }
    }
    public class Templates
    {
        private List<TemplateInfo> _commonTemplates;
        private string _commontemplatespath;
        public Templates(string commontemplatespath)
        {
            this._commontemplatespath = commontemplatespath;
            _commonTemplates = new List<TemplateInfo>();
            foreach (string filename in Tools.FileTools.NameListFromDir(commontemplatespath, ".json"))
            {
                _commonTemplates.Add(new TemplateInfo(filename,_commontemplatespath));
            }
        }
        public List<TemplateInfo> CommonTemplates { get { return _commonTemplates; } }

        public static Template GetTemplate(string filename)
        {
            TemplateData td = new TemplateData(File.ReadAllText(  filename));
            if (td.Correctrect.Width > 0)
            {
               return new Template(td);
            }
            return null;
        }
        public static Rectangle GetTemplateCorrect(string filename)
        {
            TemplateData td = new TemplateData(File.ReadAllText(filename));
            if (td.Correctrect.Width > 0)
            {
                return td.Correctrect; ;
            }
            return new Rectangle();
        }
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
        //private MyDetectFeatureRectAngle _dr;
        private Template _artemplate;
        private string _fullpath;
        private string _dirname;
        private TemplateInfo ti;
    
        public TemplateShow(string fullpath,string dirname, string imgfilename, TemplateInfo ti=null,bool savefilename=false)
        {
            this._fullpath = fullpath;
            this._imgfilename = imgfilename;
            this._dirname = dirname;
            this.ti = ti;
            this.OK = false;
            //this._src = (Bitmap)Bitmap.FromFile(_imgfilename);
            System.IO.FileStream fs = new System.IO.FileStream(imgfilename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap _src = (Bitmap)System.Drawing.Image.FromStream(fs);
         
            Rectangle cr = new Rectangle();
            if (ti != null)
            {
                cr = Templates.GetTemplateCorrect(ti.TemplateFileName);
            }
            DetectData dd = DetectImageTools.DetectImg(_src,cr);
            //dd = DetectImageTools.DetectImg(_src, dd.CorrectRect);
            if(ti==null)
            dd = DetectImageTools.DetectCorrect.ReDetectCorrectImg(_src, dd);
            if (dd.CorrectRect.Width > 0)
            {
                 if (ti == null)
                 {
                     _artemplate = new Template(dd.ListFeature,dd.CorrectRect);
                 }
                 else
                 {
                     _artemplate = new Template(dd.ListFeature,dd.CorrectRect);
                     Template t = Templates.GetTemplate(ti.TemplateFileName);
                     _artemplate.Match(t);
                     if (savefilename)
                         _artemplate.FileName = ti.TemplateFileName;                    
                 }
                 this.OK = true;
            }
           
            fs.Close();
            fs = null;
        }

        public Template Template { get { return _artemplate; } }
        public string SrcFileName { get { return _imgfilename; } }
        public bool OK { get; set; }
    }
    public class Scan
    {
        private ScanConfig _sc;
      
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
        private bool _forscan;
        public Scan(ScanConfig sc,string templatename, List<string> nameList,string fulldirpath,bool forscan=true)
        {
            this._forscan = forscan;
            this.DgSaveScanData = null;
            this.DgShowScanMsg = null;
            _xztpos = -1;
            _titlepos = null;
            this._sc = sc;
            this._templatename = templatename;
            this._nameList = nameList;
            this._dirname = fulldirpath.Substring(fulldirpath.LastIndexOf("\\") + 1);
            this._srcpath = fulldirpath;
            Template t = Templates.GetTemplate(templatename);
            //if (forscan)
            //    _dr = new MyDetectFeatureRectAngle( t.Manageareas.FeaturePoints.list ,t.CorrectRect);
            if (!Directory.Exists(CorrectPath))
                Directory.CreateDirectory(CorrectPath);
            _template = t;
        }
        public void Clear()
        {
            //TODO: ClearScanData
        }
        public void DoScan()
        {
            Thread thread = new Thread(new ThreadStart(RunScan));
            thread.Start();
        }
        public void RunScan()
        {
            Msg = "";
            StringBuilder msg = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            _angle = _template.Angle;
			foreach (string s in _nameList)
			{
				string _runmsg = DetectImg(s,ref msg);
				sb.Append(_runmsg);
                if(DgShowScanMsg!=null)
                DgShowScanMsg(_runmsg);//this.Invoke(new MyInvoke(ShowMsg));
				Thread.Sleep(10);
			}
			_exportdata = sb.ToString();
            Msg = msg.ToString();
            if (DgSaveScanData != null)//this.Invoke(new MyInvoke(ExportData));
                DgSaveScanData(_exportdata);
        }
        private string DetectImg(string s,ref StringBuilder  msg)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.FileStream fs = new System.IO.FileStream(s,System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap orgsrc = (Bitmap)System.Drawing.Image.FromStream(fs);
            DetectData dd = DetectImageTools.DetectImg(orgsrc, this.Template.CorrectRect );         
            if (dd.CorrectRect.Width > 0 ) //TODO: 进一步判断
            {
                _angle.SetPaper(dd.ListFeature);
               
                sb.Append(s + "," + dd.CorrectRect.ToString("-"));// 文件名 , CorrectRect
                sb.Append("," + _angle.Angle2 ); //校验角度

                Bitmap src = (Bitmap)orgsrc.Clone(dd.CorrectRect, orgsrc.PixelFormat);
                src.Save(CorrectPath + s.Substring(s.LastIndexOf("\\")));
                AutoComputeXZTKH acx = new AutoComputeXZTKH(_template,src);
                if (_template.Manageareas.KaohaoChoiceAreas.HasItems())
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
                            sb.Append("," + rs.Text);  //考号-条形码 姓名 MsgtoDr中处理
                            if ( _sc. Studentbases.HasStudentBase)
                                sb.Append("," + _sc.Studentbases.GetName(Convert.ToInt32(rs.Text)));
                            else
                                sb.Append(",-");
                        }
                    }
                    else if ("1023456789".Contains(kha.Type))
                    {
                        string kh = acx.ComputeKH(kha, _angle);
                        if (kh.Contains("-"))
                            sb.Append("," + kh + ",-");   //考号-涂卡 姓名-未知 MsgtoDr中处理
                        else
                        {
                            if (_sc.Studentbases.HasStudentBase)
                                sb.Append("," + kh + "," + _sc.Studentbases.GetName(Convert.ToInt32(kh)));
                            else
                                sb.Append("," + kh + ",-");
                        }
                    }
                }
                string str = s.Substring(s.Length - 7, 3);
                sb.Append("," + acx.ComputeXZT(str,_angle)); //选择题
                //计算座位号
                if (_template.Manageareas.Customareas.HasItems())
                {
                    StringBuilder tsb = new StringBuilder();
                    foreach (CustomArea ca in _template.Manageareas.Customareas.list)
                    {                      
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
                msg.AppendLine(s);   
                //检测失败
            }
            sb.AppendLine();
            fs.Close();
            return sb.ToString();
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
                    for (int i = 0; i <_template.Manageareas.SinglechoiceAreas.Count; i++)
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
        public string Msg { get; set; }
        public Dictionary<string, int> Titlepos { get { return _titlepos; } }
        public int Xztpos { get { return _xztpos; } }
        public string ScanDataPath { get { return _sc.Baseconfig.ScanDataPath + "\\"+ _dirname; } }
        public string DirName { get { return _dirname; } }
        public string SourcePath { get { return _srcpath; } }
        public string TemplateName { get { return _templatename; } }
        public AutoAngle Angle { get { return _angle; } }
        public Template Template { get { return _template; } }
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
