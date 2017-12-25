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
using Tools;

namespace ARTemplate
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Template
    {
        public Template(List<Rectangle> list)
        {
            _dic = new Dictionary<string, List<Area>>();
            List<Rectangle> listTBO = AutoTBO.GetAutoTBORect(list);
            InitCorrectRect(listTBO);
            foreach (string s in new string[] { "特征点", "考号", "校对", "选择题", "非选择题", "选区变黑", "选区变白", "题组", "自定义" })
                if (!_dic.ContainsKey(s))
                    _dic[s] = new List<Area>();
            List<Rectangle> newlist = new List<Rectangle>();
            foreach (Rectangle r in listTBO)
                newlist.Add(new Rectangle(r.X - CorrectRect.X, r.Y - CorrectRect.Y, r.Width, r.Height));              
            _dic["特征点"].AddRange( FeaturePoints.GetFeaturesfromTBO(newlist) );
            _manageareas = null;
            _angle = new AutoAngle( list.Select(r => new Point(r.X - CorrectRect.X,r.Y-CorrectRect.Y)).ToList() ); 
        }
        public Template(TemplateData td)
        {
            CorrectRect = td.Correctrect;
            _dic = td._dic;
            _manageareas = null;
            _angle = new AutoAngle(Manageareas.FeaturePoints.list.Select(r => r.Rect.Location).ToList());
        }
        private void InitCorrectRect(List<Rectangle> listTBO)
        {
            string Ostr = AutoDetectRectAnge.OtherName(listTBO);
            if (Ostr.StartsWith("L"))
            {
                CorrectRect = new Rectangle(listTBO[2].X, listTBO[0].Y, listTBO[0].Right - listTBO[2].Left, listTBO[1].Bottom - listTBO[0].Top);
            }
            else
            {
                CorrectRect = new Rectangle(listTBO[0].X, listTBO[0].Y, listTBO[2].Right - listTBO[0].Left, listTBO[1].Bottom - listTBO[0].Top);
            }
        }
       
        public void UpdateTreeNodes(TreeNode m_tn)
        {
            Clear();
            foreach (TreeNode t in m_tn.Nodes)
            {
                foreach (TreeNode n in m_tn.Nodes[t.Name].Nodes)
                {
                    if (n.Tag != null)
                    {
                        AddArea((Area)n.Tag, t.Name);
                    }
                }
            }
        }
        public void Save(string jsonname)
        {
            string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            File.WriteAllText(jsonname, str);
        }
        public bool Load(string jsonname)
        {
            TemplateData td = new TemplateData(File.ReadAllText(  jsonname));
            if (td.Correctrect.Width > 0)
            {
                _dic = td._dic;
                CorrectRect = td.Correctrect;
                _manageareas = null;
                return true;
            }
            return false;
        }
        public TreeNode GetTreeNode()
        {
            TreeNode root = new TreeNode();
            foreach (KeyValuePair<string, List<Area>> kv in _dic)
            {
                TreeNode opt = new TreeNode();
                opt.Name = opt.Text = kv.Key;
                foreach (Area I in kv.Value)
                {
                    TreeNode t = new TreeNode();
                    t.Name= t.Text = I.ToString();
                    if (t.Name == "")
                        t.Name = t.Text = I.TypeName;
                    t.Tag = I;
                    opt.Nodes.Add(t);
                }
                root.Nodes.Add(opt);
            }
            root.Text = "网上阅卷";
            return root;
        }
        public List<string> GetTitles()
        {
            List<string> titles = new List<string>();
            titles.Clear();
            titles.Add("文件名");
            titles.Add("CorrectRect");
            titles.Add("校验角度");
            if (Manageareas.KaohaoChoiceAreas.HasItems())
            {
                titles.Add("考号");
                titles.Add("姓名");
            }
            if (Manageareas.SinglechoiceAreas.HasItems())
                titles.Add("选择题");
            if (Manageareas.Customareas.HasItems())
                titles.Add("自定义");
            return titles;
        }      
        public string GetTemplateName()
        {
            string str = "";
            if (Manageareas.KaohaoChoiceAreas.HasItems())
                str += "IDName_";
            if (Manageareas.SinglechoiceAreas.HasItems())
                str += "X" + Manageareas.SinglechoiceAreas.Count;
            if (Manageareas.Unchooseareas.HasItems())
                str += "_K" + Manageareas.Unchooseareas.Count;
            if (Manageareas.Customareas.HasItems())
            {
                int zw = 0;
                foreach (Area I in Manageareas.Customareas.list)
                {
                    if (I.ToString().Contains("座位"))
                    {
                        zw++;
                    }
                }
                if (zw > 0)
                    str += "_ZW" + zw;
            }
            return str;
        }

        private void Clear()
        {
            foreach (KeyValuePair<string, List<Area>> kv in _dic)
                kv.Value.Clear();
            _dic.Clear();
        }
        private void AddArea(Area area, string typename)
        {
            if (!_dic.ContainsKey(typename))
                _dic[typename] = new List<Area>();
            _dic[typename].Add(area);
        }

        public Dictionary<string, List<Area>> Dic { get { return _dic; } }
        [JsonIgnore]
        public ManageAreas Manageareas
        {
            get
            {
                if (_manageareas == null && _dic!=null)
                {
                    _manageareas = new ManageAreas();
                    foreach (KeyValuePair<string, List<Area>> kv in _dic)
                    {
                        switch (kv.Key)
                        {
                            case "特征点": _manageareas.FeaturePoints = new FeaturePoints(kv.Value); break;
                            case "考号": _manageareas.KaohaoChoiceAreas = new KaoHaoChoiceAreas  (kv.Value); break;
                            case "校对": _manageareas.Nameareas = new NameAreas  (kv.Value); break;
                            case "选择题": _manageareas.SinglechoiceAreas = new SingleChoiceAreas (kv.Value); break;
                            case "非选择题": _manageareas.Unchooseareas =new UnChooseAreas (kv.Value); break;
                            case "题组": _manageareas.Tzareas =new TzAreas (kv.Value); break;
                            case "自定义": _manageareas.Customareas = new CustomAreas(kv.Value); break;
                            case "选区变黑": _manageareas.BlackTempareas = new TempAreas(kv.Value); break;
                            case "选区变白": _manageareas.WhiteTempareas = new TempAreas(kv.Value); break;
                        }
                    }
                }
                return _manageareas;
            }
        }
        [JsonProperty]
        public Rectangle CorrectRect
        {
            get;
            set;
        }
        [JsonProperty]
        private Dictionary<string, List<Area>> _dic;
        private ManageAreas _manageareas;
        private AutoAngle _angle;
        private TemplateData td;
        public string FileName { get { return ""; } }
        //public void SetFeaturePoint(List<Rectangle> list, Rectangle cr)
        //{
        //    if (Correctrect.ToString() != cr.ToString())
        //        return;
        //    Point midpoint = new Point(cr.Width / 2, cr.Height / 2);
        //    string key = "特征点";
        //    if (!_dic.ContainsKey(key))
        //        _dic[key] = new List<Area>();
        //    _dic[key].Clear();
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        Rectangle r = list[i];
        //        r.Offset(-cr.X, -cr.Y);
        //        _dic[key].Add(new FeaturePoint(r, midpoint));
        //    }
        //}

        public AutoAngle Angle { get { return _angle; } }
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
            Correctrect = To.CorrectRect;
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
            public Dictionary<string, object> _dic;
            [JsonProperty]
            public Rectangle CorrectRect { get; set; }
        }
    }
    public class TemplateTools
    {
        public static Bitmap DrawInfoBmp(Bitmap src, Template _artemplate, AutoAngle _angle)
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
        public static Bitmap DrawInfoBmp(Student S, StudentsResultData SR, AutoAngle angle, List<string> optionanswer, List<TzArea> ltz)
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
