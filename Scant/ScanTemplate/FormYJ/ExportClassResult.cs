using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using ARTemplate;
using Tools;
using System.Data;

namespace ScanTemplate.FormYJ
{
    public class ExportClassResult
    {
        public ExportClassResult(ScanConfig _sc, Examdata _examdata, Template _template)
        {
            this._angle = _template.Angle;
            this._sc = _sc;
            _exam = new Exam(_examdata);
            this._students = _examdata.SR._Students;
            this._SR = _examdata.SR;
            this._Optionsubjects = _examdata.SR._Optionsubjects;
            this._Imgsubjects = _examdata.SR._Imgsubjects;
            this._Tzsubjects = _examdata.SR._Tzsubjects;
            InitAnswer();
            InitMsg();        
        }
        private void InitMsg()
        {
            _Msg = "共有选择题：" + _Optionsubjects.OptionSubjects.Count + " 题,  非选择题： " + _Imgsubjects.Subjects.Count + " 小题";
            _Msg += "\r\n选择题共： " + _Optionsubjects.OptionSubjects.Select(r => r.Score).Sum() + "分";
            _Msg += "\r\n 非选择题共： " + _Imgsubjects.Subjects.Select(r => r.Score).Sum() + "分";
            _Msg += "\r\n 合计共： " + (_Optionsubjects.OptionSubjects.Select(r => r.Score).Sum() + _Imgsubjects.Subjects.Select(r => r.Score).Sum()) + "分";
        }
        private void InitAnswer()
        {
            bReady = false;
            if (CheckResult())
            {
                _Optionanswer = _exam.OSubjects.Select(r => r.Answer).ToList();
                _OptionMaxscore = _exam.OSubjects.Select(r => r.Score).ToList();
                if (!_Optionanswer.Exists(r => r.Length != 1 || !"ABCD".Contains(r))
                    && !_OptionMaxscore.Exists(r => r <= 0))
                    bReady = true;
                _ABCD = new List<string>() { "A", "B", "C", "D" };
                _dicABCDToOption = _ABCD.ToDictionary(r => r, r => r[0] - 'A');
            }
        }
        private bool CheckResult()
        {
            foreach (List<int> L in _SR._Result)
            {
                if (L.Any(r => r < 0))
                    return false;
            }
            return true;
        }        
        public void Export(string resultAction)
        {
            string FileName = "";
            if (!bReady)
            {
                MessageBox.Show("还有选择题没有设定答案或者分值  或者 试卷未改完 \r\n" + _Msg);
                return;
            }
            if (resultAction != "")
                switch (resultAction)
                {
                    case "exother":
                        Export("exresult");
                        Export("eximage");
                        Export("exresultfx");
                        break;
                    case "exresult":
                        FileName = _sc.Baseconfig.ExportResultPath + "\\" + _exam.Name;
                        ExportResult(FileName);
                        MessageBox.Show("已输出成绩");
                        break;
                    case "eximage": 
                        string ImagePath = _sc.Baseconfig.ExportImageRootPath + "\\" + _exam.Name;
                        ExportImages(ImagePath);
                        MessageBox.Show("已输出到"+ImagePath ); 
                        break;
                    case "exresultfx": 
                        FileName = _sc.Baseconfig.ExportResultFxPath + "\\" + _exam.Name + "_成绩分析";
                        StringBuilder sb = new StringBuilder();
                        sb.Append( ExportXztFx() );
                        sb.Append( ExportFxztFx() );
                        File.WriteAllText(FileName+".txt", sb.ToString());
                        MessageBox.Show("已输出成绩分析");
                        break;
                }
        }       
        private void ExportImages(string ImgPath)
        {
            CheckFold(ImgPath);
            foreach (Student S in _students.students)
            {
                PaperResult pr = ConstructPaperResult(S);
                Bitmap bmp = TemplateTools.DrawInfoBmp(S, _angle, pr);
                //Bitmap bmp = TemplateTools.DrawInfoBmp(S, _SR, _angle, _Optionanswer, ltz);
                string filename =ImgPath + "\\" + S.ID + ".jpg";
                if (_sc.Studentbases.HasStudentBase)
                {
                    if (S.KH > 1)
                    {
                        string name = _sc.Studentbases.GetName(S.KH);
                        filename = ImgPath + "\\" + S.ID + "_" + name + ".jpg";
                    }
                }
                bmp.Save(filename);
            }
        }
        private PaperResult ConstructPaperResult(Student S)
        {
            PaperResult pr = new PaperResult();
            float fsum = 0;
            Rectangle r = new Rectangle();
            if (_Optionsubjects.OptionSubjects.Count > 0)
                r = _Optionsubjects.OptionSubjects[0].Rect;
            foreach (Optionsubject O in _Optionsubjects.OptionSubjects)
            {
                r = Rectangle.Union(r, O.Rect);
                float score = S.CorrectXzt(O.Index, _Optionanswer[O.Index]) ? _OptionMaxscore[O.Index] : 0;
                int listindex = _dicABCDToOption[_Optionanswer[O.Index]];
                Rectangle RO = O.Rect;
                if (O.List.Count > listindex)
                    RO.Offset(O.List[listindex]);
                pr.AddOption(new ResultObj(RO, score));
                fsum += score;
            }
            if (r.Y > 30)
                r.Y -= 30;
            pr.Xzt = new ResultObj(r, fsum,true);

            foreach (Tzsubject T in _Tzsubjects.Tzs)
            {
                int subsum = 0;
                foreach (Imgsubject I in T.Subjects)
                {
                    int score = _SR._Result[I.Index][S.Index];
                    pr.AddOption(new ResultObj(I.Rect, score));
                    subsum += score;
                }
                fsum += subsum;
                pr.Tz.Add(new ResultObj(T.Rect, subsum,true));
            }
            pr.ZF = new ResultObj(new Rectangle(r.Width / 3, 30, 30, 30), fsum , true);
            return pr;
        }
        private void CheckFold(string ImgPath)
        {
            if (!Directory.Exists(ImgPath))
                Directory.CreateDirectory(ImgPath);
            else
                MessageBox.Show("已存在文件夹" + ImgPath + "! 继续将覆盖该文件夹内的文件！！ 请确认！！ ");
        }
       
        private StringBuilder ExportXztFx(bool bouterrornamelist = false)
        {
            Optionsubjects _Optionsubjects = _SR._Optionsubjects;
            Imgsubjects _Imgsubjects = _SR._Imgsubjects;
            StringBuilder sb = new StringBuilder();
            foreach (Optionsubject O in _Optionsubjects.OptionSubjects)
            {
                List<int> Iabcd = _ABCD.Select(r =>
                    _students.students.Where(rr => rr.SelectOption(r, O.Index)).Count()).ToList();
                int okindex = _dicABCDToOption[_Optionanswer[O.Index]];
                int rightcnt = Iabcd[okindex];
                int count = _students.students.Count;
                Double rightrate = rightcnt * 1.0 / count;
                sb.Append(O.OutName + " 分值：" + O.Score + " 正确答案：" + _Optionanswer[O.Index] + " 正确率(" + rightcnt + "/" + count + ")：" + rightrate.ToString("0.00%")+" ");
                //sb.AppendLine(
                //    string.Join("\r\n",
                //    _ABCD.Select(r => "  选项：" + r + " (" + Iabcd[_dicABCDToOption[r]].ToString("00") + "/" + count + ")" + ZifuRate(Iabcd[_dicABCDToOption[r]] * 1.0 / count))
                //    )
                //);
                sb.AppendLine(
                   string.Join("\t",
                   _ABCD.Select(r => r + "(" + Iabcd[_dicABCDToOption[r]].ToString("00") + "/" + count + ")")
                   )
               );
                //错误学生明单
                if (bouterrornamelist)
                {
                    sb.AppendLine("错误学生名单");
                    sb.AppendLine(
                        string.Join("\r\n",
                            _ABCD.Where(r => r != _Optionanswer[O.Index]).Select(r =>
                            {
                                return "选项" + r + ":" + string.Join(",",
                                _students.students.Where(rr => rr.SelectOption(r, O.Index)).Select(sr => sr.Name));
                            })
                        )
                    );
                    sb.AppendLine();
                }
            }
            return sb;
        }
        private StringBuilder ExportFxztFx(bool bouterrornamelist = false)
        {
            Imgsubjects _Imgsubjects = _SR._Imgsubjects;
            StringBuilder sb = new StringBuilder();
            foreach (Imgsubject O in _Imgsubjects.Subjects)
            {
                List<int> or = _exam.SR.Result[O.Index];
                int rightcnt = or.Count(r => r > 0);
                int count = or.Count;
                double avg = or.Average();
                Double rightrate = avg / O.Score;
                sb.AppendLine(O.Name + " 分值：" + O.Score + " 正确率(" + rightcnt + "/" + count + ")：" + rightrate.ToString("0.00%"));
                //错误学生明单
                if (bouterrornamelist)
                {
                    sb.AppendLine("错误学生名单");
                    int index = 0;
                    sb.AppendLine(
                        string.Join(",",
                            or.Select(r =>
                            {
                                index++;
                                if (r > 0) return -index; return index;
                            }).Where(r => r > 0).Select(r => _students.students[r - 1].Name)
                                   )
                        );
                    sb.AppendLine();
                }
            }
            return sb;
        }
        private void ExportResult(string FileName)
        {//导出成绩
            string Tztitle = "";
            foreach(Tzsubject T in _Tzsubjects.Tzs)
            {
                Tztitle += "," + T.Name;
            }
            StringBuilder sblistscore = new StringBuilder("姓名,总分\r\n");
            StringBuilder sblisttizu = new StringBuilder("姓名,总分,选择题,"+Tztitle + "\r\n");
            StringBuilder sbdetail = new StringBuilder();
            foreach (Student S in _students.students)
            {
                PaperResult pr = ConstructPaperResult(S);
                sblistscore.AppendLine(S.Name + "," + pr.TotalScore());
                sblisttizu.AppendLine(S.Name + "," + pr.TotalTz());
                sbdetail .AppendLine(S.Name + "," + pr.Detail());
            }
            File.WriteAllText(FileName+".txt", sbdetail.ToString());
            Tools.TextBitmapTool tbl = new TextBitmapTool(
                new Rectangle(0, 0, 960, 720), new Rectangle(40, 30, 880, 660));

            List<string> list = new List<string>(sblistscore.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            List<string> list1 = new List<string>(sblisttizu.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            List<int> cids = _students.StudentsClassid(_sc);
            if (cids.Count == 1)
            {
                List<int> kh = _students.students.Select(r => r.KH).ToList();
                list.AddRange(
                    UnScanNameList(cids[0],kh,_sc).Select( r => r+",未交").ToList()
                    );
            }else
            {
                MessageBox.Show("存在多个班级");
            }
            File.WriteAllText(FileName + "_总分.大题分.txt",
                string.Join("\r\n", list) +"\r\n\r\n"+ string.Join("\r\n", list1));
            tbl.DrawListInPaper(list).Save(FileName + ".jpg");
            tbl.DrawListInPaper(list1).Save(FileName + "_1.jpg");
        }
        private string ZifuRate(double rightrate, int len = 20)
        {
            string right = "*******************************************************************";
            // "■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■";
            string error = "...................................................................";
            // "□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□";
            int r = (int)(len * rightrate + 0.5) % len;
            int e = len - r;
            return right.Substring(0, r) + error.Substring(0, e);
        }

        public static List<string> UnScanNameList(int classid,List<int> kh, ScanConfig _sc)
        {           
            List<string> namelist =
             _sc.Studentbases.GetClassStudent(classid).Where(r => !kh.Exists(rr => rr == r.KH))
                 .Select(r1 => r1.Name).ToList();
            return namelist;
        }

        private ScanConfig _sc;
        private bool bReady;
        private StudentsResultData _SR;
        private List<string> _Optionanswer;
        private List<float> _OptionMaxscore;
        private Dictionary<string, int> _dicABCDToOption;
        private List<string> _ABCD;
        private string _Msg;
        
        private Students _students;
        private Exam _exam;
        private Imgsubjects _Imgsubjects;
        private Optionsubjects _Optionsubjects;
        private TzAreas _TzAreas;
        private Tzsubjects _Tzsubjects;
        private AutoAngle _angle;
    }
}
