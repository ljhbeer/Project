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
            this._sc = _sc;
            this._examdata = _examdata;
            this._template = _template;
            //
            _exam = new Exam(_examdata);
            this._students = _examdata.SR._Students;

            //////
            this._SR = _examdata.SR;
            Init();
            Optionsubjects _Optionsubjects = _examdata.SR._Optionsubjects;
            Imgsubjects _Imgsubjects = _examdata.SR._Imgsubjects;
            _Msg = "共有选择题：" + _Optionsubjects.OptionSubjects.Count + " 题,  非选择题： " + _Imgsubjects.Subjects.Count + " 小题";
            _Msg += "\r\n选择题共： " + _Optionsubjects.OptionSubjects.Select(r => r.Score).Sum() + "分";
            _Msg += "\r\n 非选择题共： " + _Imgsubjects.Subjects.Select(r => r.Score).Sum() + "分";
            _Msg += "\r\n 合计共： " + (_Optionsubjects.OptionSubjects.Select(r => r.Score).Sum() + _Imgsubjects.Subjects.Select(r => r.Score).Sum()) + "分";
        }
        private void Init()
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
                    case "exother": ExportAll(); break;
                    case "exresult":
                        FileName = _sc.Baseconfig.ExportResultPath + "\\" + _examdata.Name;
                        ExportResult(FileName);
                        MessageBox.Show("已输出成绩");
                        break;
                    case "eximage": 
                        ExportImages();
                        string ImagePath = _sc.Baseconfig.ExportImageRootPath + "\\" + _exam.Name;
                        MessageBox.Show("已输出到"+ImagePath ); 
                        break;
                    case "exresultfx": 
                        FileName = _sc.Baseconfig.ExportResultFxPath + "\\" + _examdata.Name + "_成绩分析";
                        StringBuilder sb = new StringBuilder();
                        sb.Append( ExportXztFx() );
                        sb.Append( ExportFxztFx() );
                        File.WriteAllText(FileName, sb.ToString());
                        MessageBox.Show("已输出成绩分析");
                        break;
                }
        }
        private void ExportAll()
        {
            ExportImages();
            //ExportResult();
            //ExportXztFx();
            MessageBox.Show("已全部导出");
        }
        private void ExportImages()
        {
            
            AutoAngle angle = _template.Angle;
            List<TzArea> ltz = GetDrawTzlist();
            List<List<Imgsubject>> Tz = GetDrawTzlist1();
            CheckFold(_exam.Name);
            foreach (Student S in _students.students)
            {
                float sum = 0;
                S.OutXzt(_Optionanswer, _OptionMaxscore, ref sum);
                float fsum = _examdata.SR._Result.Sum(rr => rr[S.Index]);
                int zfsum = (int)(sum + fsum);
                int tzindex = 0;
                foreach (List<Imgsubject> L in Tz)
                {
                    string name = L.Select(I => _examdata.SR._Result[I.Index][S.Index]).Sum() + "分";
                    ltz[tzindex].SetName(name);
                    tzindex++;
                }
                ltz[tzindex].SetName(zfsum.ToString());
                tzindex++;
                ltz[tzindex].SetName(sum.ToString());

                Bitmap bmp = TemplateTools.DrawInfoBmp(S, _examdata.SR, angle, _Optionanswer, ltz);
                string filename = _sc.Baseconfig.ExportImageRootPath + "\\" + _exam.Name + "\\" + S.ID + ".jpg";
                if (_sc.Studentbases.HasStudentBase)
                {
                    if (S.KH > 1)
                    {
                        string name = _sc.Studentbases.GetName(S.KH);
                        filename = _sc.Baseconfig.ExportImageRootPath + "\\" + _exam.Name + "\\" + S.ID + "_" + name + ".jpg";
                    }
                }
                bmp.Save(filename);
            }
        }
        private void CheckFold(string examName)
        {
            string path = _sc.Baseconfig.ExportImageRootPath + "\\" + _exam.Name;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            else
                MessageBox.Show("已存在文件夹" + path + "! 继续将覆盖该文件夹内的文件！！ 请确认！！ ");
        }
        private List<List<Imgsubject>> GetDrawTzlist1()
        {
            List<List<Imgsubject>> Tz = new List<List<Imgsubject>>();
            foreach (TzArea t in _template.Manageareas.Tzareas.list)
            {
                List<Imgsubject> L = new List<Imgsubject>();
                foreach (Imgsubject i in _SR._Imgsubjects.Subjects)
                {
                    if (t.ImgArea.Contains(i.Rect))
                        L.Add(i);
                }
                Tz.Add(L);
            }
            return Tz;
        }
        private List<TzArea> GetDrawTzlist()
        {
            List<TzArea> ltz = _template.Manageareas.Tzareas.list;
            //题组
            List<TzArea> ltz1 = new List<TzArea>();
            foreach (Area I in _template.Dic["题组"])
            {
                ltz.Add((TzArea)I);
            }

            Rectangle zfrect = _template.Dic["校对"][0].Rect;
            zfrect.Offset(zfrect.Width, 0);
            zfrect.Width = 0;
            zfrect.Height = 0;
            Rectangle xztrect = _template.Dic["选择题"][0].Rect;
            xztrect.Offset(-30, -50);
            xztrect.Width = 1;
            xztrect.Height = 1;
            ltz.Add(new TzArea(zfrect, ""));
            ltz.Add(new TzArea(xztrect, ""));
            return ltz;
        }
        private StringBuilder ExportXztFx()
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
                sb.AppendLine(O.OutName + " 分值：" + O.Score + " 正确答案：" + _Optionanswer[O.Index] + " 正确率(" + rightcnt + "/" + count + ")：" + rightrate.ToString("0.00%"));
                sb.AppendLine(
                    string.Join("\r\n",
                    _ABCD.Select(r => "  选项：" + r + " (" + Iabcd[_dicABCDToOption[r]].ToString("00") + "/" + count + ")" + ZifuRate(Iabcd[_dicABCDToOption[r]] * 1.0 / count))
                    )
                );
                //错误学生明单
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
            return sb;
        }
        private StringBuilder ExportFxztFx()
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
            return sb;
        }
        private void ExportResult(string FileName)
        {//导出成绩
            Optionsubjects _Optionsubjects = _examdata.SR._Optionsubjects;
            Imgsubjects _Imgsubjects = _examdata.SR._Imgsubjects;
            int Oscore = (int)_exam.OSubjects.Sum(r => r.Score);
            int Sscore = _exam.Subjects.Sum(r => r.Score);
            try
            {
                List<List<Imgsubject>> Tz = new List<List<Imgsubject>>();
                string Tztitle = "";
                foreach (TzArea t in _template.Dic["题组"])
                {
                    List<Imgsubject> L = new List<Imgsubject>();
                    foreach (Imgsubject i in _examdata.SR._Imgsubjects.Subjects)
                    {
                        if (t.ImgArea.Contains(i.Rect))
                            L.Add(i);
                    }
                    Tz.Add(L);
                    Tztitle += t.ToString() + ",";
                }

                string title = Student.ResultTitle() + "选择题,非选择题,总分," + string.Join(",", _exam.OSubjects.Select(r => r.Name()))
                 + "," + string.Join(",", _examdata.SR._Imgsubjects.Subjects.Select(r => r.Name)) + "," + Tztitle + "\r\n";

                StringBuilder sblistscore = new StringBuilder("姓名,总分\r\n");
                StringBuilder sblisttizu = new StringBuilder("姓名,总分,选择题," + Tztitle + "\r\n");
                StringBuilder sb = new StringBuilder(title);
                foreach (Student r in _students.students)
                {
                    sb.Append(r.ResultInfo());
                    float sum = 0;
                    string xzt = r.OutXzt(_Optionanswer, _OptionMaxscore, ref sum);
                    float fsum = _examdata.SR._Result.Sum(rr => rr[r.Index]);
                    sb.Append(sum + "," + fsum + "," + (sum + fsum) + ",");
                    sb.Append(xzt);
                    sb.Append(",");
                    sb.Append(string.Join(",", _examdata.SR._Result.Select(rr => rr[r.Index].ToString()).ToArray()));
                    sb.Append(",");
                    StringBuilder sbt = new StringBuilder();
                    foreach (List<Imgsubject> L in Tz)
                    {
                        sbt.Append(L.Select(I => _examdata.SR._Result[I.Index][r.Index]).Sum() + ",");
                    }
                    sb.Append(sbt);
                    sb.AppendLine();
                    sblistscore.AppendLine(r.Name + " " + (sum + fsum));
                    sblisttizu.AppendLine(r.Name + "," + (sum + fsum) + "," + sum + "," + sbt);
                }
                File.WriteAllText( FileName, sb.ToString());
                Tools.TextBitmapTool tbl = new TextBitmapTool(
                    new Rectangle(0, 0, 960, 720), new Rectangle(40, 30, 880, 660));

                List<string> list = new List<string>(sblistscore.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
                List<string> list1 = new List<string>(sblisttizu.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
                tbl.DrawListInPaper(list).Save(FileName + ".jpg");
                tbl.DrawListInPaper(list1).Save(FileName + "_1.jpg");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private ScanConfig _sc;
        private bool bReady;
        private StudentsResultData _SR;
        private List<string> _Optionanswer;
        private List<float> _OptionMaxscore;
        private Dictionary<string, int> _dicABCDToOption;
        private List<string> _ABCD;
        private string _Msg;

        private Template _template;
        private Students _students;
        private Examdata _examdata;
        private Exam _exam;
    }
}
