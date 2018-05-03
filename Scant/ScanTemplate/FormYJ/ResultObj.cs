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
        }
        
        public void Compute()
        {
            float OptionTotalScore = Options.Select(r => r.Floatscore).Sum();

        }
        public void AddOption(ResultObj resultObj)
        {
            Options.Add(resultObj);
        }
        public List<ResultObj> Options;
        public List<ResultObj> Tz;
        public ResultObj Xzt;
        public ResultObj ZF;
        public IEnumerable<ResultObj> TotalObjs()
        {
            List<ResultObj> r = new List<ResultObj>();
            r.AddRange(Options);
            r.AddRange(Tz);
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

    }
    public class ResultObj
    {
        public ResultObj()
        {
        }
        public ResultObj(Rectangle rect, float score,float maxscore,bool showscore = false)
        {
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
        public float Floatscore;
        public Rectangle Rect;
        public int Score;
        public string Txt;
        public Boolean NumberMode;
        public Boolean HalfRightMode;
    }
}
