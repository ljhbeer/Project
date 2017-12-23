using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using ScanTemplate.FormYJ;

namespace ARTemplate
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Template
    {
        // "特征点", "考号","姓名", "选择题", "非选择题", "选区变黑", "选区变白"
        public Template(string imgpath, Bitmap bmp, Rectangle CorrectRect)
        {
            this._imagefilename = imgpath;
            this._src = bmp.Clone(CorrectRect, bmp.PixelFormat);
            this.Correctrect = CorrectRect;
            _xztRect = null;
            _dic = new Dictionary<string, List<Area>>();

        }

        public void Clear()
        {
            ResetData();
            _dic.Clear();
            if (_src != null)
                _src.Dispose();
        }
        public void ResetBitMap(string imgpath, Bitmap bmp, Rectangle CorrectRect)
        {
            this._imagefilename = imgpath;
            this._src = bmp.Clone(CorrectRect, bmp.PixelFormat);
            this.Correctrect = CorrectRect;
            if (_dic.ContainsKey("特征点"))
                _dic["特征点"].Clear();
        }
        public void ResetData(bool clearFeaturePoint = true)
        { //"特征点",不能清除
            foreach (string s in new string[] { "特征点", "考号", "校对", "选择题", "非选择题", "选区变黑", "选区变白", "题组", "自定义" })
                if (_dic.ContainsKey(s))
                {
                    if (!clearFeaturePoint && s == "特征点")
                        continue;
                    _dic[s].Clear();
                }
        }
        public bool CheckEmpty()
        {
            return _dic["选择题"].Count == 0;
        }
        public void AddArea(Area area, string name)
        {
            if (!_dic.ContainsKey(name))
                _dic[name] = new List<Area>();
            _dic[name].Add(area);
        }

        public void Save(string jsonname)
        {
            string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            File.WriteAllText(jsonname, str);
        }
        public bool Load(string jsonname)
        {
            TemplateData td = new TemplateData(jsonname);
            if (td.Correctrect.Width > 0)
            {
                _dic = td._dic;
                Correctrect = td.Correctrect;
            }
            return false;
        }
        public void SetDataToNode(TreeNode m_tn)
        {
            foreach (string s in new string[] { "特征点", "考号", "校对", "选择题", "非选择题", "选区变黑", "选区变白", "题组", "自定义" })
            {
                TreeNodeCollection tc = m_tn.Nodes[s].Nodes;
                if (_dic.ContainsKey(s))
                    foreach (Area I in _dic[s])
                    {
                        TreeNode t = new TreeNode();
                        int cnt = tc.Count + 1;
                        t.Name = cnt.ToString();
                        t.Text = s + cnt;
                        if ("非选择题|题组|自定义|校对".Contains(s))
                            t.Text = I.ToString();
                        t.Tag = I;
                        tc.Add(t);
                    }
            }
        }
        public void SetFeaturePoint(List<Rectangle> list, Rectangle cr)
        {
            if (Correctrect.ToString() != cr.ToString())
                return;
            Point midpoint = new Point(cr.Width / 2, cr.Height / 2);
            string key = "特征点";
            if (!_dic.ContainsKey(key))
                _dic[key] = new List<Area>();
            _dic[key].Clear();
            for (int i = 0; i < list.Count; i++)
            {
                Rectangle r = list[i];
                r.Offset(-cr.X, -cr.Y);
                _dic[key].Add(new FeaturePoint(r, midpoint));
            }
        }
        public List<string> GetTitles()
        {
            List<string> titles = new List<string>();
            titles.Clear();
            titles.Add("文件名");
            titles.Add("CorrectRect");
            titles.Add("校验角度");
            string item = "考号";
            if (Dic.ContainsKey(item) && Dic[item].Count > 0)
            {
                titles.Add(item);
                titles.Add("姓名");
            }
            //item = "非选择题";
            //if (t.Dic.ContainsKey(item) && t.Dic[item].Count > 0)
            //    titles.Add(item);

            item = "选择题";
            if (Dic.ContainsKey(item) && Dic[item].Count > 0)
                titles.Add(item);

            item = "自定义";
            if (Dic.ContainsKey(item) && Dic[item].Count > 0)
                titles.Add(item);
            return titles;
        }
        public bool HasOptions(string keyname)
        {
            return Dic.ContainsKey(keyname) && Dic[keyname].Count > 0;
        }
        public string GetTemplateName()
        {
            string str = "";
            if (_dic.ContainsKey("考号") && _dic["考号"].Count > 0)
                str += "IDName_";//_dic["选择题"].Count;
            if (_dic.ContainsKey("选择题"))
                str += "X" + XztRect.Count;//_dic["选择题"].Count;
            //str += "选择题" + _dic["选择题"].Count;
            if (_dic.ContainsKey("非选择题"))
                str += "_K" + _dic["非选择题"].Count;

            if (_dic.ContainsKey("自定义") && _dic["自定义"].Count > 0)
            {
                int zw = 0;
                foreach (Area I in _dic["自定义"])
                {
                    if (I.ToString().Contains("座位"))
                    {
                        zw++;
                    }
                }
                if (zw > 0)
                    str += "_ZW" + zw;
            }
            //str += "_非选择题" + _dic["非选择题"].Count;
            //if(Correctrect!=null)
            //    str+="_"+Correctrect.ToString("-");
            return str;
        }

        public String NodeName { get { return "TEMPLATE"; } }
        public Bitmap Image { get { return _src; } }
        public List<Area> SingleAreas { get { return _dic["选择题"]; } }

        public Size Imgsize
        {
            get { return _src.Size; }
        }
        public String Filename
        {
            get { return _imagefilename; }
        }

        public string XmlFileName { get { return _XmlFileName; } }
        public Dictionary<string, List<Area>> Dic { get { return _dic; } }
        public Dictionary<int, Rectangle> XztRect
        {
            get
            {
                if (_xztRect == null)
                {
                    _xztRect = new Dictionary<int, Rectangle>();
                    int cnt = 0;
                    if (_dic.ContainsKey("选择题"))
                        foreach (Area I in _dic["选择题"])
                        {
                            if (I.HasSubArea())
                            {
                                int subcnt = 0;
                                foreach (List<Point> lp in ((SingleChoiceArea)I).list)
                                {
                                    Rectangle r = I.ImgArea;
                                    r.Height /= ((SingleChoiceArea)I).Count;
                                    r.Y += subcnt * r.Height;
                                    subcnt++;

                                    _xztRect[cnt] = r;
                                    cnt++;
                                }
                            }
                        }
                }
                return _xztRect;
            }
        }

        [JsonProperty]
        public Rectangle Correctrect
        {
            get;
            set;
        }
        [JsonProperty]
        private Dictionary<string, List<Area>> _dic;
        private Dictionary<int, Rectangle> _xztRect;
        private Bitmap _src;
        private string _imagefilename;
        private string _XmlFileName;
    }
    public class ConvertTemplateData
    {
        public static bool Load(String xmlFileName)
        {
            Dictionary<string, List<Area>> _dic = new Dictionary<string, List<Area>>();
            Rectangle Correctrect = new Rectangle();
            if (!xmlFileName.ToLower().EndsWith(".xml")) return false;
            if (!File.Exists(xmlFileName)) return false;
            //this._XmlFileName = xmlFileName;
            try
            {
                String NodeName = "TEMPLATE";
                Point midpoint = new Point(0, 0);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFileName);

                XmlNode xn = xmlDoc.SelectSingleNode(NodeName + "/BASE/CORRECTRECT");
                if (xn != null)
                {
                    Correctrect = Tools.StringTools.StringToRectangle(xn.InnerText);
                    midpoint = new Point(Correctrect.Width / 2, Correctrect.Height / 2);
                }
                if (midpoint.X == 0) return false;

                foreach (string s in new string[] { "特征点-FEATUREPOINTSAREA", "考号-KAOHAOAREA", "校对-NAMEAREA", "选择题-SINGLECHOICES", "非选择题-UNCHOOSES", "选区变黑-BLACKAREA", "选区变白-WHITEAREA", "题组-UNCHOOSEGROUP", "自定义-CUSTOMDEFINE" })
                {
                    string name = s.Substring(0, s.IndexOf("-"));
                    string ENname = s.Substring(s.IndexOf("-") + 1);
                    string path = (NodeName + "/[]/*").Replace("[]", ENname + "S");
                    XmlNodeList list = xmlDoc.SelectNodes(path);
                    _dic[name] = new List<Area>();

                    if (ENname == "KAOHAOAREA")
                    {
                        if (list.Count > 0)
                        {
                            XmlNode node = list[0];
                            XmlNode type = node.SelectSingleNode("TYPE");
                            XmlNode rect = node.SelectSingleNode("Rectangle");
                            if (type == null || rect == null)
                                continue;
                            string Type = type.InnerText;
                            Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);
                            if (Type == "条形码")
                                _dic[name].Add(new KaoHaoChoiceArea(r, "考号", Type));
                            else if ("1023456789".Contains(Type))
                            {
                                XmlNode xname = node.SelectSingleNode("NAME");
                                XmlNode xsize = node.SelectSingleNode("SIZE");
                                if (xname != null && xsize != null)
                                {
                                    Size size = Tools.StringTools.StringToSize(xsize.InnerText);
                                    List<List<Point>> llp = new List<List<Point>>();
                                    foreach (XmlNode node1 in node.ChildNodes)
                                    {
                                        if (node1.Name == "SINGLE")
                                        {
                                            List<Point> listp = new List<Point>();
                                            foreach (XmlNode node2 in node1.ChildNodes)
                                            {
                                                listp.Add(Tools.StringTools.StringToPoint(node2.InnerText));
                                            }
                                            llp.Add(listp);
                                        }
                                    }
                                    _dic[name].Add(new KaoHaoChoiceArea(r, "考号", Type, llp, size));
                                }
                            }
                        }
                    }
                    else if (ENname == "CUSTOMDEFINE")
                    {
                        foreach (XmlNode node in list)
                        {
                            //XmlNode node = list[0];
                            XmlNode type = node.SelectSingleNode("TYPE");
                            XmlNode rect = node.SelectSingleNode("Rectangle");
                            if (type == null || rect == null)
                                continue;
                            string Type = type.InnerText;
                            Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);
                            if ("1023456789".Contains(Type))
                            {
                                XmlNode xname = node.SelectSingleNode("NAME");
                                XmlNode xsize = node.SelectSingleNode("SIZE");
                                if (xname != null && xsize != null)
                                {
                                    string strname = xname.InnerText;
                                    Size size = Tools.StringTools.StringToSize(xsize.InnerText);
                                    List<List<Point>> llp = new List<List<Point>>();
                                    foreach (XmlNode node1 in node.ChildNodes)
                                    {
                                        if (node1.Name == "SINGLE")
                                        {
                                            List<Point> listp = new List<Point>();
                                            foreach (XmlNode node2 in node1.ChildNodes)
                                            {
                                                listp.Add(Tools.StringTools.StringToPoint(node2.InnerText));
                                            }
                                            llp.Add(listp);
                                        }
                                    }
                                    _dic[name].Add(new CustomArea(r, strname, Type, llp, size));
                                }
                            }
                        }
                    }
                    else if (ENname == "FEATUREPOINTSAREA")
                    {
                        List<Rectangle> lr = new List<Rectangle>();
                        foreach (XmlNode node in list)
                        {
                            if (node.FirstChild.Name == "Rectangle")
                            {
                                Rectangle r = Tools.StringTools.StringToRectangle(node.InnerText);
                                _dic[name].Add(new FeaturePoint(r, midpoint));
                            }
                        }
                    }
                    else if (ENname == "NAMEAREA")
                    {
                        if (list.Count == 0) continue;
                        foreach (XmlNode node in list)
                        {
                            XmlNode rect = node.SelectSingleNode("Rectangle");
                            XmlNode xname = node.SelectSingleNode("NAME");
                            if (rect != null)
                            {
                                string itemname = "姓名";
                                if (xname != null)
                                    itemname = xname.InnerText;
                                Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);
                                _dic[name].Add(new NameArea(r, itemname));
                            }
                        }
                    }
                    else if (ENname == "SINGLECHOICES")
                    {
                        foreach (XmlNode node in list)
                        {
                            XmlNode rect = node.SelectSingleNode("Rectangle");
                            XmlNode xname = node.SelectSingleNode("NAME");
                            XmlNode xsize = node.SelectSingleNode("SIZE");

                            if (rect != null && xname != null && xsize != null)
                            {
                                Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);
                                string strname = xname.InnerText;
                                Size size = Tools.StringTools.StringToSize(xsize.InnerText);

                                List<List<Point>> llp = new List<List<Point>>();
                                foreach (XmlNode node1 in node.ChildNodes)
                                {
                                    if (node1.Name == "SINGLE")
                                    {
                                        List<Point> listp = new List<Point>();
                                        foreach (XmlNode node2 in node1.ChildNodes)
                                        {
                                            listp.Add(Tools.StringTools.StringToPoint(node2.InnerText));
                                        }
                                        llp.Add(listp);
                                    }
                                }
                                _dic[name].Add(new SingleChoiceArea(r, strname, llp, size));
                            }
                        }
                    }
                    else if (ENname == "UNCHOOSES")
                    {
                        foreach (XmlNode node in list)
                        {
                            XmlNode rect = node.SelectSingleNode("Rectangle");
                            XmlNode xname = node.SelectSingleNode("NAME");
                            XmlNode xscore = node.SelectSingleNode("SCORE");
                            if (rect != null && xname != null && xscore != null)
                            {
                                Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);
                                string strname = xname.InnerText;
                                float score = (float)Convert.ToDouble(xscore.InnerText);
                                _dic[name].Add(new UnChoose(score, strname, r));
                            }
                        }
                    }
                    else if (ENname == "BLACKAREA")
                    {
                        foreach (XmlNode node in list)
                        {
                            XmlNode rect = node.SelectSingleNode("Rectangle");
                            if (rect != null)
                            {
                                Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);
                                _dic[name].Add(new TempArea(r, "选区变黑"));
                            }
                        }
                    }
                    else if (ENname == "WHITEAREA")
                    {
                        foreach (XmlNode node in list)
                        {
                            XmlNode rect = node.SelectSingleNode("Rectangle");
                            if (rect != null)
                            {
                                Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);
                                _dic[name].Add(new TempArea(r, "选区变白"));
                            }
                        }
                    }
                    else if (ENname == "UNCHOOSEGROUP")
                    {
                        foreach (XmlNode node in list)
                        {
                            XmlNode rect = node.SelectSingleNode("Rectangle");
                            XmlNode xname = node.SelectSingleNode("NAME");
                            if (rect != null && xname != null)
                            {
                                Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);
                                string strname = xname.InnerText;
                                _dic[name].Add(new TzArea(r, strname));
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            new TemplateData(_dic, Correctrect).Save(xmlFileName);
            return true;
        }
    }
    public class TemplateData
    {
        [JsonProperty]
        public Rectangle Correctrect { get; set; }
        [JsonProperty]
        public Dictionary<string, List<Area>> _dic;
        public TemplateData(string jsonstr)
        {
            TemplateObject To =
            Newtonsoft.Json.JsonConvert.DeserializeObject<TemplateObject>(jsonstr);
            ConvertToArea(To);
        }
        public TemplateData(Dictionary<string, List<Area>> _dic, Rectangle Correctrect)
        {
            this._dic = _dic;
            this.Correctrect = Correctrect;
        }
        public void Save(string xmlfilename)
        {
            string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            string jsonname = xmlfilename + ".json";
            File.WriteAllText(jsonname, str);
        }
        public void ConvertToArea(TemplateObject To)
        {
            Dictionary<string, List<Area>> dic = new Dictionary<string, List<Area>>();
            foreach (KeyValuePair<string, object> item in To._dic)
            {
                switch (item.Key)
                {
                    case "特征点":
                        dic[item.Key] = new List<Area>();
                        foreach (FeaturePoint A in MyArea<FeaturePoint>.ConvertTo(item.Value))
                            dic[item.Key].Add(A);
                        break;
                    case "考号":
                        dic[item.Key] = new List<Area>();
                        foreach (KaoHaoChoiceArea A in MyArea<KaoHaoChoiceArea>.ConvertTo(item.Value))
                            dic[item.Key].Add(A);
                        break;
                    case "校对":
                        dic[item.Key] = new List<Area>();
                        foreach (NameArea A in MyArea<NameArea>.ConvertTo(item.Value))
                            dic[item.Key].Add(A);
                        break;
                    case "选择题": dic[item.Key] = new List<Area>();
                        foreach (SingleChoiceArea A in MyArea<SingleChoiceArea>.ConvertTo(item.Value))
                            dic[item.Key].Add(A);
                        break;
                    case "非选择题": dic[item.Key] = new List<Area>();
                        foreach (UnChoose A in MyArea<UnChoose>.ConvertTo(item.Value))
                            dic[item.Key].Add(A);
                        break;
                    case "选区变黑": dic[item.Key] = new List<Area>();
                        foreach (TempArea A in MyArea<TempArea>.ConvertTo(item.Value))
                            dic[item.Key].Add(A);
                        break;
                    case "选区变白": dic[item.Key] = new List<Area>();
                        foreach (TempArea A in MyArea<TempArea>.ConvertTo(item.Value))
                            dic[item.Key].Add(A);
                        break;
                    case "题组": dic[item.Key] = new List<Area>();
                        foreach (TzArea A in MyArea<TzArea>.ConvertTo(item.Value))
                            dic[item.Key].Add(A);
                        break;
                    case "自定义": dic[item.Key] = new List<Area>();
                        foreach (CustomArea A in MyArea<CustomArea>.ConvertTo(item.Value))
                            dic[item.Key].Add(A);
                        break;
                }
            }
            _dic = dic;
            Correctrect = To.Correctrect;
        }
        public class MyArea<T>
        {
            public static List<T> ConvertTo(object o)
            {
                string str = o.ToString();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(str);
            }
        }
        public class TemplateObject
        {
            [JsonProperty]
            public Rectangle Correctrect { get; set; }
            [JsonProperty]
            public Dictionary<string, object> _dic;
        }
    }
    public class TemplateTools
    {
        public static Bitmap DrawInfoBmp(Bitmap src, Template _artemplate, ScanTemplate.AutoAngle _angle)
        {
            Bitmap //bmp = src.Clone(new Rectangle(0, 0, src.Width, src.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
             bmp = ConvertFormat.ConvertToRGB(src);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Pen pen = Pens.Red;
                Brush dark = Brushes.Black;
                Brush white = Brushes.White;
                Brush Red = Brushes.Red;
                Font font = SystemFonts.DefaultFont;

                foreach (string s in new string[] { "特征点", "考号", "校对", "选择题", "非选择题" })
                    if (_artemplate.Dic.ContainsKey(s))
                    {
                        int cnt = 0;
                        foreach (Area I in _artemplate.Dic[s])
                        {
                            g.DrawRectangle(pen, I.ImgArea);
                            if (I.HasSubArea())
                            {
                                foreach (Rectangle r in I.ImgSubArea())
                                {
                                    r.Offset(I.ImgArea.Location);
                                    g.DrawRectangle(pen, r);
                                    r.Offset(-1, -1);
                                    g.DrawRectangle(pen, r);
                                    r.Offset(2, 2);
                                    g.DrawRectangle(pen, r);
                                }
                            }
                            if (I.NeedFill())
                            {
                                g.FillRectangle(I.FillPen(), I.ImgArea);
                                g.DrawString(cnt.ToString(), font, Red, I.ImgArea.Location);
                            }
                        }
                    }
            }
            return bmp;
        }
        public static Bitmap DrawInfoBmp(Student S, StudentsResultData SR, ScanTemplate.AutoAngle angle, List<string> optionanswer, List<TzArea> ltz)
        {
            Bitmap src = S.Src.Clone(S.SrcCorrectRect, S.Src.PixelFormat); //bmp = src.Clone(new Rectangle(0, 0, src.Width, src.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Bitmap bmp = ConvertFormat.ConvertToRGB(src);
            angle.SetPaper(S.Angle);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Pen pen = Pens.Red;
                Brush dark = Brushes.Black;
                Brush white = Brushes.White;
                Brush Red = Brushes.Red;
                Font font = new Font(SystemFonts.DefaultFont.SystemFontName, 25, FontStyle.Bold);
                Font font1 = new Font(SystemFonts.DefaultFont.SystemFontName, 16, FontStyle.Bold);


                foreach (Optionsubject I in SR._Optionsubjects.OptionSubjects)
                {
                    Rectangle r = I.Rect;
                    Point p = angle.GetCorrectPoint(r.X, r.Y);
                    r.Location = p;
                    g.DrawRectangle(pen, I.Rect);
                    g.DrawRectangle(pen, r);

                    //g.DrawRectangle(pen, rr);
                    int OKindex = "ABCD".IndexOf(optionanswer[I.Index]);
                    if (OKindex >= 0)
                    {
                        Rectangle rr = new Rectangle(I.List[OKindex], I.Size);
                        rr.Offset(p);
                        if (S.CorrectXzt(I.Index, optionanswer[I.Index]))
                            g.DrawString("√", font1, Red, rr.Location);
                        else
                            g.DrawString("×", font1, Red, rr.Location);
                    }
                }

                foreach (Imgsubject I in SR._Imgsubjects.Subjects)
                {
                    //g.DrawRectangle(pen, I.Rect);
                    Point p = I.Rect.Location;
                    int offsetx = I.Rect.Width * 3 / 10;
                    p.X = I.Rect.Right - offsetx > 0 ? I.Rect.Right - offsetx : I.Rect.Right;
                    p.Y += I.Rect.Height * 3 / 10;

                    if (SR._Result[I.Index][S.Index] == 0)
                        g.DrawString("×", font, Red, p);
                    else
                        g.DrawString("√", font, Red, p);

                }
                foreach (TzArea I in ltz)
                {
                    g.DrawRectangle(pen, I.Rect);
                    g.DrawString(I.ToString(), font, Red, I.Rect.Location);
                }
            }
            return bmp;
        }
    }
}
