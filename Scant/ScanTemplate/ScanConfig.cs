using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using ARTemplate;

namespace ScanTemplate
{
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
        public ScanConfig(string workpath) //E:\Scan\s1025
        {
            Baseconfig = new BaseConfig(workpath);
            _unscans = new UnScans(Baseconfig.UnScanPath);
            _commontemplates = new Templates(Baseconfig.TemplatePath);
            _scandatas = new ScanDatas(Baseconfig.ScanDataPath);
           
        }
        public UnScans Unscans { get { return _unscans; } }
        public Templates CommonTemplates { get { return _commontemplates; } }
        public ScanDatas Scandatas { get { return _scandatas; } }
        public TemplateShow Templateshow { get; set; }   
        public BaseConfig Baseconfig { get; set; }
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
    
        public TemplateShow(string fullpath,string dirname, string imgfilename)
        {
            this._fullpath = fullpath;
            this._imgfilename = imgfilename;
            this._dirname = dirname;
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
