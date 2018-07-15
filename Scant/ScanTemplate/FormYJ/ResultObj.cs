using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ScanTemplate.FormYJ
{
    public class PaperResult
    {
        public PaperResult()
        {
            Options = new List<ResultObj>();
            Tz = new List<ResultObj>();  
            OptionCount = 0;
            UnChooseCount = 0;
        }
        public void Compute()
        {
            float OptionTotalScore = Options.Select(r => r.Floatscore).Sum();

        }
        public void AddOption(ResultObj resultObj)
        {
            Options.Add(resultObj);
        }
        public IEnumerable<ResultObj> TotalObjs()
        {
            List<ResultObj> r = new List<ResultObj>();
            r.AddRange(Options);
            if(Tz.Count>0)
                r.AddRange(Tz);
            if(HasOption)
                r.Add(Xzt);
            r.Add(ZF);
            return r;
        }
        public float TotalScore()
        {
            return ZF.Score;
        }
        public string TotalTz()//姓名,总分,选择题,"+Tztitle
        {
            return TotalScore() + "," + Xzt.Score+","
                + string.Join("," , Tz.Select(r => r.Score).ToList());
        }
        public string Detail()
        {
            return TotalScore() + "," + Xzt.Score + ","+ Tz.Select(r => r.Score).Sum()+","
                 + string.Join(",", Options.Select( r => r.Score).ToList());
        }
        public string DetailTitle()
        {
            return "总分,选择题,非选择题,"+ string.Join(",", Options.Select( r => r.Txt).ToList());
        }
        private float GetTzOptionScore(List<int> list)
        {
            return list.Select(r => Options[r].Score).Sum();
        }
        public string TotalXztTz(TzOptionsubjects   t_TzOptionsubjects)
        {
            string strtz = "";
            if (t_TzOptionsubjects != null)
            {
                foreach (TzOptionsubject tzo in t_TzOptionsubjects.Tzs)
                {
                    strtz += "," + GetTzOptionScore(tzo.SubjectIndexs);
                }
            }
            return strtz;
        }
        public float TotalTz(Tzsubject T)
        {
            return 0;
        }
        public float TotalXztTz(TzOptionsubject T)
        {
            return GetTzOptionScore(T.SubjectIndexs);
        }
        public List<ResultObj> Options;
        public List<ResultObj> Tz;
        public ResultObj Xzt;
        public ResultObj ZF;
        //Tags
        public int OptionCount { get; set; }
        public Boolean HasOption { get{return OptionCount>0;} }
        public int UnChooseCount { get; set; }
        public Boolean HasUnchoose { get { return UnChooseCount > 0; } }
    }
    public class ResultObj
    {
        public ResultObj()
        {
        }
        public ResultObj(Rectangle rect, float score,float maxscore,bool showscore = false)
        {
            Index = -1;
            HalfRightMode = false;
            this.Rect = rect;
            this.Floatscore = score;
            this.Score = (int)score;
            if (showscore)
            {
                Txt = score.ToString();
                NumberMode = true;
            }
            else
            {
                if (Score == 0)
                {
                    Txt = "×";
                }
                else
                {
                    Txt = "√";
                    if (maxscore -score > 0.2 )
                        HalfRightMode = true;
                }                
                NumberMode = false;
            }
        }
        public int Index { get; set; }
        public float Floatscore;
        public Rectangle Rect;
        public int Score;
        public string Txt;
        public Boolean NumberMode;
        public Boolean HalfRightMode;
    }
}
