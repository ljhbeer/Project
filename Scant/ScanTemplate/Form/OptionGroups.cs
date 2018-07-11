using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Newtonsoft.Json;
using Tools;

namespace ARTemplate
{
    public class OptionGroups: Areas
    {
        public OptionGroups()
        {
            _list = null;
        }
        public OptionGroups(List<Area> lista)
        {
            _list = null;
            baselist = lista;
        }
        private List<OptionGroup> _list;
        public List<OptionGroup> list
        {
            get
            {
                if (_list == null && baselist != null)
                {
                    _list = new List<OptionGroup>();
                    foreach (Area A in baselist)
                        _list.Add((OptionGroup)A);
                }
                return _list;
            }
        }

        public  double  TotalScore()
        {
            if(list!=null)
            return list.Sum(r => r.SubAreas.Sum(rr => ((UnChoose)rr).Scores));
            return 0;
        }
        public  string ScoreInfomation()
        {
            if(list!=null)
            //return string.Join("\r\n",
            //    list.Select(r =>
            //    {
            //        string str  = "";
            //       str =  r.Title + "\t"+ r.SubAreas.Sum(rr => ((UnChoose)rr).Scores)+"分\t"+ 
            //           string.Join(" ",r.SubAreas.Select( rr => ((UnChoose)rr).Scores + "分"));
            //        return str;
            //    }).ToList()
            //    );
            if (list == null)
                return base.GetScoreInfomation();
            float totalscore = list.Sum(r => r.GetTotalScore());
            return "选择题：共" +list.Count + "大题 总分:" + totalscore+"\r\n"+
                 string.Join("\r\n", list.Select( r => r.GetScoreInfomation())) ;

        }
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class OptionGroup : Area
    {
        public OptionGroup(Rectangle rect, string name)
        {
            this.TypeName = "选择题题组";
            this.Rect = rect;
            this._name = name;
            ShowTitle = false;
            _subareas = new List<Area>();
        }
        public override string Title
        {
            get
            {
                return _name;
            }
        }
        public override String ToString()
        {
            return _name;
        }
        public override void SetName(string name)
        {
            _name = name;
        }
        public void AddSubArea(Area I)
        {
            _subareas.Add(I);
        }
        public override bool HasSubAreas()
        {
            return true;
        }
        public override float GetTotalScore()
        {
            if(_subareas.Count==0)
            return base.GetTotalScore();
            return _subareas.Sum(r => r.GetTotalScore());
        }
        public override string GetScoreInfomation()
        {
            return _name + ": " + GetTotalScore() + "分";
        }
        [JsonIgnore]
        public override List<Area> SubAreas
        {
            get
            {
                return _subareas;
            }
        }
        [JsonProperty]
        private string _name;
        [JsonProperty]
        private List<Area> _subareas; 
        //public List<SingleChoiceArea> _scas;
    }
}
