using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

namespace ARTemplate
{
    public class Template
    {
        // "特征点", "考号","姓名", "选择题", "非选择题", "选区变黑", "选区变白"
        public Template(string imgpath, Bitmap bmp, Rectangle CorrectRect)
        {
            this._imagefilename = imgpath;
            this._src = bmp.Clone( CorrectRect, bmp.PixelFormat);
            this.Correctrect = CorrectRect;
            _xztRect = null;
            _dic = new Dictionary<string, List<Area>>();
    
        }
        public Template(String xmlFileName)
        {
            _src = null;
            _dic = new Dictionary<string, List<Area>>();
            if (Load(xmlFileName))
            {
                if (File.Exists(_imagefilename))
                {
                    _src = (Bitmap)Bitmap.FromFile(_imagefilename);
                    _src = _src.Clone(Correctrect, _src.PixelFormat);
                }
            }
        }
        public void Clear(){
        	ResetData();
        	_dic.Clear();
        	if(_src!=null)
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
        public void ResetData(bool clearFeaturePoint=true)
        { //"特征点",不能清除
            foreach (string s in new string[] {"特征点",  "考号", "姓名", "选择题", "非选择题", "选区变黑", "选区变白" })
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
       
        public void Save(String xmlFileName)
        {
            if (xmlFileName.ToLower().EndsWith(".xml"))
            {
                XmlDocument xmlDoc = SaveToXmlDoc();
                xmlDoc.Save(xmlFileName);
                this._XmlFileName = xmlFileName;
            }
        }
        public XmlDocument SaveToXmlDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement(NodeName);
            xmlDoc.AppendChild(root);
            XmlNode path = xmlDoc.CreateElement("BASE");
            root.AppendChild(path);
            path.InnerXml = Imgsize.ToXmlString() + _imagefilename.ToXmlString("PATH")+Correctrect.ToXmlString().ToXmlString("CORRECTRECT");

            foreach (string s in new string[] { "特征点-FEATUREPOINTSAREA", "考号-KAOHAOAREA", "姓名-NAMEAREA", "选择题-SINGLECHOICES", "非选择题-UNCHOOSES", "选区变黑-BLACKAREA", "选区变白-WHITEAREA" })
            {
                string name = s.Substring(0, s.IndexOf("-"));
                string ENname = s.Substring(s.IndexOf("-")+1);
                XmlNode  list = xmlDoc.CreateElement(ENname+"S");
                root.AppendChild(list);
                int i = 0;
                if(_dic.ContainsKey(name))
                foreach (Area I in _dic[name])
                {
                    XmlElement xe = xmlDoc.CreateElement(ENname);
                    xe.SetAttribute("ID", i.ToString());
                    xe.InnerXml = I.ToXmlString();
                    list.AppendChild(xe);
                    i++;
                }
            }
            return xmlDoc;
        }     
        public bool Load(String xmlFileName)
        {
            ResetData();
            if (!xmlFileName.ToLower().EndsWith(".xml")) return false;
            if (!File.Exists(xmlFileName)) return false;
            this._XmlFileName = xmlFileName;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFileName);
                _imagefilename = xmlDoc.SelectSingleNode(NodeName + "/BASE/PATH").InnerText;
                Size imgsize =Tools.StringTools. StringToSize(xmlDoc.SelectSingleNode(NodeName + "/BASE/SIZE").InnerText);
                Point midpoint = new Point(imgsize.Width / 2, imgsize.Height / 2);
                XmlNode xn = xmlDoc.SelectSingleNode(NodeName + "/BASE/CORRECTRECT");
                if (xn != null)
                    Correctrect = Tools.StringTools.StringToRectangle(xn.InnerText);
                //////Bitmap bitmap =(Bitmap) Bitmap.FromFile(_imagefilename);
                //////if (bitmap.Size != imgsize)
                //////    return false;
                //////_src = bitmap;
                foreach (string s in new string[] { "特征点-FEATUREPOINTSAREA", "考号-KAOHAOAREA", "姓名-NAMEAREA", "选择题-SINGLECHOICES", "非选择题-UNCHOOSES", "选区变黑-BLACKAREA", "选区变白-WHITEAREA" })
                {
                    string name = s.Substring(0, s.IndexOf("-"));
                    string ENname = s.Substring(s.IndexOf("-")+1);
                    string path = (NodeName + "/[]/*").Replace("[]",ENname+"S");
                    XmlNodeList  list = xmlDoc.SelectNodes(path);
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
                        XmlNode rect = list[0].SelectSingleNode("Rectangle");
                        if (rect != null)
                        {
                            Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);                          
                            _dic[name].Add(new NameArea(r));
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
                                _dic[name].Add(new SingleChoiceArea(r,strname,llp,size));
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
                            if (rect != null && xname!=null  && xscore!=null)
                            {
                                Rectangle r = Tools.StringTools.StringToRectangle(rect.InnerText);
                                string strname = xname.InnerText;
                                float score =(float) Convert.ToDouble(xscore.InnerText);
                                _dic[name].Add(new UnChoose(score,strname,r));
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
                }
            }
            catch //(Exception ex)
            {
                ResetData();
                return false;
            }
            return true;
        }       
        public void SetDataToNode(TreeNode m_tn)
        {
            foreach (string s in new string[] { "特征点", "考号", "姓名", "选择题", "非选择题", "选区变黑", "选区变白" })
            {
                TreeNodeCollection tc = m_tn.Nodes[s].Nodes;
                if(_dic.ContainsKey(s))
                foreach (Area I in _dic[s])
                {
                    TreeNode t = new TreeNode();
                    int cnt = tc.Count + 1;
                    t.Name = cnt.ToString();
                    t.Text =s + cnt;
                    t.Tag =I;
                    tc.Add(t);
                }
            }               
        }
        public void SetFeaturePoint(List<Rectangle> list, Rectangle cr)
        {
            if( Correctrect.ToString() != cr.ToString() )
                return;
            Point midpoint = new Point(cr.Width / 2, cr.Height / 2);
            string key = "特征点";
            if (!_dic.ContainsKey(key))
                _dic[key] = new List<Area>();
            _dic[key].Clear();
            for (int i = 0; i < list.Count; i++)
            {
                Rectangle r = list[i];
                r.Offset(-cr.X,-cr.Y);
                _dic[key].Add(new FeaturePoint(r, midpoint));
            }
        }        
		public string GetTemplateName()
		{
			string str = "";
			if(_dic.ContainsKey("选择题"))
				str+= "选择题"+_dic["选择题"].Count;			
			if(_dic.ContainsKey("非选择题"))
				str+= "_非选择题"+_dic["非选择题"].Count;
			if(Correctrect!=null)
				str+="_"+Correctrect.ToString("-");
			return str;
		}
        public String NodeName { get { return "TEMPLATE"; } }
        public Bitmap Image { get { return _src; } }
        public List<Area> SingleAreas {get { return _dic["选择题"];}}
        public Size Imgsize
        {
            get { return _src.Size; }
        }
        public String Filename
        {
            get { return _imagefilename; }
        }
        public Rectangle Correctrect
        {
            get;
            set;
        }
        public string XmlFileName { get { return _XmlFileName; } }
        public Dictionary<string, List<Area>> Dic { get { return _dic; } }
        public Dictionary<int,Rectangle> XztRect{
        	get{
        		if(_xztRect==null){
        			_xztRect = new Dictionary<int, Rectangle>();
        			int cnt = 0;
        			foreach(Area I in _dic["选择题"]){
        				if(I.HasSubArea()){
        					int subcnt = 0;
        					foreach(List<Point> lp in ((SingleChoiceArea)I).list){
	        					Rectangle r =I.ImgArea;
	        					r.Height/= ((SingleChoiceArea)I).Count;
        						r.Y += subcnt* r.Height;
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
        
        private Dictionary< string, List<Area>> _dic;        
        private Dictionary<int, Rectangle> _xztRect;
        private Bitmap _src;
        private string _imagefilename;
        private string _XmlFileName;    	
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

                foreach (string s in new string[] { "特征点", "考号", "姓名", "选择题", "非选择题" })
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
    }
}
