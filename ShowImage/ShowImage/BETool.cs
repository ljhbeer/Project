using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowImage
{
    class BETool
    {
    }
    public class BETag
    {
        public BETag(string s)
        {
            OK = false;
            if (s == null)
                return;
            if (s.StartsWith("{"))
            {
                BEPos bp = BETag.FormatCmd(s, '{', '}');
                if (bp.Valid())
                {
                    Cmd = new Cmd(bp.String);//////////???????   
                    s = s.Substring(bp.E + 1);
                }
            }
            string[] items = s.Split(new string[] { "[", "]", "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 2)
            {
                Begin = items[0].Replace("@@", "-");
                End = items[1].Replace("@@", "-");
            }
            else if (items.Length == 1)
            {
                Begin = items[0].Replace("@@", "-");
                End = "";
            }
            else
                return;
            OK = true;
        }
        public BEPos BEPos(string s)
        {
            if (OK)
            {
                int B = s.IndexOf(Begin);
                if (B != -1)
                {
                    B = B + Begin.Length;
                    int E = s.IndexOf(End, B);
                    if (E != -1)
                        return new BEPos(B, E, s);
                }
            }
            return new BEPos(-1, -1, s);
        }
        public BEPos BEPos(BEPos bp)//ReverseMatch
        {
            string s = bp.InnerStr;
            if (!OK || s == null || !bp.Valid())
                return new BEPos(-1, -1, s);

            int B = s.IndexOf(Begin, bp.B);
            if (Cmd != null )
                B = s.LastIndexOf(Begin, bp.B, bp.E - bp.B);
            if (B != -1 && B < bp.E)
            {
                B = B + Begin.Length;
                int E = s.IndexOf(End, B, bp.E - B);
                if (E != -1)
                    return new BEPos(B, E, s);
            }

            B = s.LastIndexOf(Begin, bp.E, bp.E - bp.B);
            if (B != -1)
            {
                B = B + Begin.Length;
                int E = s.IndexOf(End, B, bp.E - B);
                if (E != -1)
                    return new BEPos(B, E, s);
            }
            return new BEPos(-1, -1, s);
        }
        public BEPos NextBEPos(BEPos bp) //同一个才能NextBEPos
        {
            string s = bp.InnerStr;
            if (!OK || !bp.Valid() || s == null)
                return new BEPos(-1, -1, s);
            int B = s.IndexOf(Begin, bp.E + End.Length);
            if (B == -1)
                return new BEPos(-1, -1, s);
            B = B + Begin.Length;
            int E = s.IndexOf(End, B);
            if (E == -1)
                return new BEPos(-1, -1, s);
            return new BEPos(B, E, s);
        }
        public static BEPos FormatCmd(string Rule, char begin, char end, int startIndex = 0)
        {
            int B = 0;
            int Pos = Rule.IndexOfAny(new char[] { begin, end }, startIndex);
            if (Pos != -1 && Rule[Pos] == begin)
            {
                B = Pos + 1;
                int stack = 1;
                while (Pos != -1)
                {
                    Pos = Rule.IndexOfAny(new char[] { begin, end }, Pos + 1); //??
                    if (Rule[Pos] == begin)
                        stack++;
                    else if (Rule[Pos] == end)
                        stack--;
                    if (stack == 0)
                        return new BEPos(B, Pos, Rule);
                    if (stack < 0)
                        break;
                }
            }
            return new BEPos(-1, -1, Rule);
        }
        public string Begin { get; set; }
        public string End { get; set; }
        public Cmd Cmd { get; set; }
        public Boolean OK { get; set; }
    }
    public class BETags
    {
        public BETags(List<BETag> rbts)
        {
            this.tags = rbts;
        }
        public BETags(string Rule)
        {
            tags = new List<BETag>();
            BEPos bp;
            if (Rule.StartsWith("{"))
            {
                bp = BETag.FormatCmd(Rule, '{', '}');
                if (bp.Valid())
                {
                    string cmdstr = bp.String;/////////////////////////////////
                    this.Cmd = new Cmd(cmdstr);
                    Rule = Rule.Substring(bp.E + 1);
                }
            }
            //Compute Rule
            bp = BETag.FormatCmd(Rule, '[', ']', 0);
            while (bp.Valid())
            {
                BETag bt = new BETag(bp.String);
                if (bt.OK)
                    tags.Add(bt);
                bp = BETag.FormatCmd(Rule, '[', ']', bp.E + 1);
            }
            //Compute Cmd
        }
        public void Add(BETag bt)
        {
            tags.Add(bt);
        }
        public BETags SubTags(int b, int length = -1)
        {
            List<BETag> rbts = new List<BETag>();
            if (b >= 0)
                for (int i = b, len = 0; i < tags.Count && len != length; i++, len++)
                    rbts.Add(tags[i]);
            return new BETags(rbts);
        }
        public List<BETag> tags { get; set; }

        public Cmd Cmd { get; set; }

        public string Match(string txt)
        {
            BEPos bp = null;
            if (tags.Count > 0)
                bp = tags[0].BEPos(txt);
            for (int i = 1; i < tags.Count; i++)
                bp = tags[i].BEPos(bp);
            if (bp == null)
                return "";
            return bp.String;
        }
    }
    public class BEPos
    {
        public BEPos()
        {
            str = null;
            B = -1;
            E = -1;
        }
        public BEPos(int b, int e, string str)
        {
            this.str = str;
            B = b;
            E = e;
        }
        public bool Valid()
        {
            return B > -1 && str != null && (E > -1 && B < E || E == -1);
        }
        public string String
        {
            get
            {
                if (!Valid()) return "";
                return str.Substring(B, E == -1 ? -1 : E - B);
            }
        }
        public int B { get; set; }
        public int E { get; set; }
        public string InnerStr { get { return str; } }
        private string str;
    }
    public class Cmd
    {
        public Cmd(string str)
        {           
        }      
    }
}
