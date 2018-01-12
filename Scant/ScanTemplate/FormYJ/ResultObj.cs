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
    }

    public class ResultObj
    {
        public ResultObj()
        {
        }
        public ResultObj(Rectangle rect, float score)
        {
            this.Rect = rect;
            this.Floatscore = score;
            this.Score = (int)score;
            if (Score == 0)
            {
                Txt = "√";
            }
            else
            {
                Txt = "×";
            }
        }
        public float Floatscore;
        public Rectangle Rect;
        public int Score;
        public string Txt; 
    }
}
