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
        public OptionGroup(string name, List<int> list)
        {
            this.TypeName = "选择题题组";
            this.Rect = new Rectangle();
            ShowTitle = true;
           
            this._name = name;
            this._indexlist = list; 

            _subareas = new List<Area>();
        }
        public override string Title
        {
            get
            {
                if(_indexlist!=null && _indexlist.Count>0)
                    return _name + "["+ (_indexlist[0]+1)+"-"+(_indexlist[_indexlist.Count-1]+1)+"]";
                return _name;
            }
        }
        public override String ToString()
        {
            return _name;
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
        [JsonIgnore]
        public List<int> IndexList { get {
            if (_indexlist == null)
                return new List<int>();
            return _indexlist; 
        } }
        [JsonIgnore]
        private List<Area> _subareas;
        [JsonProperty]
        private string _name;       
        [JsonProperty]
        private List<int> _indexlist;

    }
}
