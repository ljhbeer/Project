using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace AR
{
    public class PaperTemplate
    {
        public PaperTemplate()
        {
            m_unchoose = new List<UnChoose>();
            m_singlechoice = new List<SingleChoice>();
            m_singlechoicearea = new List<SingleChoiceArea>();
            m_filename = "";
            m_fp = null;
            _angle = -2000;
        }
        public void SetImagePath(String imagefilename)
        {
            this.m_filename = imagefilename;
        }
        public void SetImageSize(Size size)
        {
            this.imgsize = size;
        }
        public void AddFeaturePoints(TriAngleFeature p0, TriAngleFeature p1, TriAngleFeature p2)
        {            
            m_fp = new TriAngleFeature[3];   
            m_fp[0] = p0;
            m_fp[1] = p1;
            m_fp[2] = p2;
            List<Point> list = new List<Point>() { p0.CornerPoint(), p1.CornerPoint(), p2.CornerPoint() };
            m_bigfp = new TriAngleFeature(list);
        }
        public void AddUnChoose(UnChoose unChoose)
        {
            m_unchoose.Add(unChoose);
        }
        public void AddSingleChoice(SingleChoice singleChoice)
        {
            m_singlechoice.Add(singleChoice);
        }
        public void AddSingleChoiceArea(SingleChoiceArea singleChoicearea)
        {
            m_singlechoicearea.Add(singleChoicearea);
        }
        public bool CheckEmpty()
        {
            if (m_filename == null || (m_unchoose.Count == 0 && m_singlechoicearea.Count == 0 && m_fp.Count() == 0))
                return true;
            return false;
        }
        public void Reset()
        {
            m_fp = null;
            m_bigfp = null;
            m_filename = "";
            imgsize.Width = imgsize.Height = 0;
            ResetData();
        }
        public void ResetData()
        {
            m_unchoose.Clear();
            m_singlechoice.Clear();
            m_singlechoicearea.Clear();
        }
        public void Save(String xmlFileName)
        {
            if (xmlFileName.ToLower().EndsWith(".xml"))
            {
                XmlDocument xmlDoc = SaveToXmlDoc();
                xmlDoc.Save(xmlFileName);
            }
        }
        public XmlDocument SaveToXmlDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement(NodeName);
            xmlDoc.AppendChild(root);
            XmlNode path = xmlDoc.CreateElement("BASE");
            XmlNode fplist = xmlDoc.CreateElement("FEATUREPOINTS");
            XmlNode sclist = xmlDoc.CreateElement("SINGLECHOICES");
            XmlNode uclist = xmlDoc.CreateElement("UNCHOOSES");
            root.AppendChild(path);
            root.AppendChild(fplist);
            root.AppendChild(sclist);
            root.AppendChild(uclist);
            {
                path.InnerXml = "<SIZE>" + imgsize.Width + "," + imgsize.Height + "</SIZE>"
                                + "<PATH>" + m_filename + "</PATH>";
            }
            int i = 0;
            foreach (TriAngleFeature t in m_fp)
            {
                XmlElement xe = xmlDoc.CreateElement("FPOINTS");
                xe.SetAttribute("ID", i.ToString());
                xe.InnerXml = t.ToXmlString();
                fplist.AppendChild(xe);
                i++;
            }
            i = 0;
            foreach (SingleChoiceArea sc in m_singlechoicearea)
            {
                XmlElement xe = xmlDoc.CreateElement("SCHOICE");
                xe.SetAttribute("ID", i.ToString());
                xe.InnerXml = sc.ToXmlString();
                sclist.AppendChild(xe);
                i++;
            }
            i = 0;
            foreach (UnChoose uc in m_unchoose)
            {
                XmlElement xe = xmlDoc.CreateElement("SCHOICE");
                xe.SetAttribute("ID", i.ToString());
                xe.InnerXml = uc.ToXmlString();
                uclist.AppendChild(xe);
                i++;
            }
            return xmlDoc;
        }
        public XmlDocument SaveToSupperXmlDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement supperroot = xmlDoc.CreateElement("SUPPERROOT");
            xmlDoc.AppendChild(supperroot);
            String name = NodeName.Substring(1, NodeName.Count() - 1);
            XmlElement root = xmlDoc.CreateElement(name);
            supperroot.AppendChild(root);
            XmlNode path = xmlDoc.CreateElement("BASE");
            XmlNode fplist = xmlDoc.CreateElement("FEATUREPOINTS");
            XmlNode sclist = xmlDoc.CreateElement("SINGLECHOICES");
            XmlNode uclist = xmlDoc.CreateElement("UNCHOOSES");
            root.AppendChild(path);
            root.AppendChild(fplist);
            root.AppendChild(sclist);
            root.AppendChild(uclist);
            {
                path.InnerXml = "<SIZE>" + imgsize.Width + "," + imgsize.Height + "</SIZE>"
                                + "<PATH>" + m_filename + "</PATH>";
            }
            int i = 0;
            foreach (TriAngleFeature t in m_fp)
            {
                XmlElement xe = xmlDoc.CreateElement("FPOINTS");
                xe.SetAttribute("ID", i.ToString());
                xe.InnerXml = t.ToXmlString();
                fplist.AppendChild(xe);
                i++;
            }
            i = 0;
            foreach (SingleChoiceArea sc in m_singlechoicearea)
            {
                XmlElement xe = xmlDoc.CreateElement("SCHOICE");
                xe.SetAttribute("ID", i.ToString());
                xe.InnerXml = sc.ToXmlString();
                sclist.AppendChild(xe);
                i++;
            }
            i = 0;
            foreach (UnChoose uc in m_unchoose)
            {
                XmlElement xe = xmlDoc.CreateElement("SCHOICE");
                xe.SetAttribute("ID", i.ToString());
                xe.InnerXml = uc.ToXmlString();
                uclist.AppendChild(xe);
                i++;
            }
            return xmlDoc;
        }
        public bool Load(String xmlFileName)
        {
            Reset();
            if (!xmlFileName.ToLower().EndsWith(".xml")) return false;
            if (!File.Exists(xmlFileName)) return false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFileName);
                m_filename = xmlDoc.SelectSingleNode(NodeName + "/BASE/PATH").InnerText;
                imgsize = StringToSize(xmlDoc.SelectSingleNode(NodeName + "/BASE/SIZE").InnerText);
                Bitmap bitmap =(Bitmap) Bitmap.FromFile(m_filename);
                if (bitmap.Size != imgsize)
                    return false;                 
                
                XmlNodeList fplist = xmlDoc.SelectNodes(NodeName + "/FEATUREPOINTS/*");
                XmlNodeList sclist = xmlDoc.SelectNodes(NodeName + "/SINGLECHOICES/*");
                XmlNodeList uclist = xmlDoc.SelectNodes(NodeName + "/UNCHOOSES/*");
                if (fplist == null) return false;
                List<List<Point>> fp = new List<List<Point>>();
                foreach (XmlNode node in fplist)
                {
                    List<Point> listp=new List<Point>();
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        listp.Add(StringToPoint(node1.InnerText));
                    }
                    if (listp.Count != 3) return false;
                    fp.Add(listp);
                }
                if (fp.Count != 3) return false;
                m_fp = new TriAngleFeature[3];
                m_fp[0] = new TriAngleFeature(fp[0]);
                m_fp[1] = new TriAngleFeature(fp[1]);
                m_fp[2] = new TriAngleFeature(fp[2]);
                List<Point> list = new List<Point>() {
                    m_fp[0].CornerPoint(), m_fp[1].CornerPoint(), m_fp[2].CornerPoint() };
                m_bigfp = new TriAngleFeature(list);
                //=====================================
                foreach (XmlNode node in sclist)
                {
                    Rectangle rect = StringToRectangle(node.SelectSingleNode("RECTANGLE").InnerText);
                    string name = node.SelectSingleNode("NAME").InnerText;
                    Size ssize = StringToSize(node.SelectSingleNode("SIZE").InnerText);
                    List<List<Point>> llistp = new List<List<Point>>();
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        if (node1.Name == "SINGLE")
                        {
                            List<Point> listp = new List<Point>();
                            foreach (XmlNode node2 in node1.ChildNodes)
                            {
                                listp.Add(StringToPoint(node2.InnerText));
                            }                            
                            llistp.Add(listp);
                        }
                    }
                    m_singlechoicearea.Add( new SingleChoiceArea(rect,name,llistp,ssize));
                }
                foreach (XmlNode node in uclist)
                {
                    float score = 0;
                    float.TryParse( node.SelectSingleNode("SCORE").InnerText,out score);
                    string name = node.SelectSingleNode("NAME").InnerText;
                    Rectangle rect = StringToRectangle( node.SelectSingleNode("RECTANGLE").InnerText );
                    UnChoose uc = new UnChoose(score, name, rect);
                    m_unchoose.Add(uc);
                }
            }
            catch
            {
                Reset();
                return false;
            }
            return true;
        }
        public bool LoadXml(XmlNode xmlDocTemplate)
        {
            Reset();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                //XmlElement root = xmlDoc.CreateElement("TEMPLATECONFIG");
                //xmlDoc.AppendChild(root);
                xmlDoc.AppendChild(xmlDoc.ImportNode(xmlDocTemplate, true));
                //xmlDoc.AppendChild(xmlDocTemplate);
                m_filename = xmlDoc.SelectSingleNode(NodeName + "/BASE/PATH").InnerText;
                imgsize = StringToSize(xmlDoc.SelectSingleNode(NodeName + "/BASE/SIZE").InnerText);
                Bitmap bitmap = (Bitmap)Bitmap.FromFile(m_filename);
                if (bitmap.Size != imgsize)
                    return false;

                XmlNodeList fplist = xmlDoc.SelectNodes(NodeName + "/FEATUREPOINTS/*");
                XmlNodeList sclist = xmlDoc.SelectNodes(NodeName + "/SINGLECHOICES/*");
                XmlNodeList uclist = xmlDoc.SelectNodes(NodeName + "/UNCHOOSES/*");
                if (fplist == null) return false;
                List<List<Point>> fp = new List<List<Point>>();
                foreach (XmlNode node in fplist)
                {
                    List<Point> listp = new List<Point>();
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        listp.Add(StringToPoint(node1.InnerText));
                    }
                    if (listp.Count != 3) return false;
                    fp.Add(listp);
                }
                if (fp.Count != 3) return false;
                m_fp = new TriAngleFeature[3];
                m_fp[0] = new TriAngleFeature(fp[0]);
                m_fp[1] = new TriAngleFeature(fp[1]);
                m_fp[2] = new TriAngleFeature(fp[2]);
                List<Point> list = new List<Point>() {
                    m_fp[0].CornerPoint(), m_fp[1].CornerPoint(), m_fp[2].CornerPoint() };
                m_bigfp = new TriAngleFeature(list);
                //=====================================
                foreach (XmlNode node in sclist)
                {
                    Rectangle rect = StringToRectangle(node.SelectSingleNode("RECTANGLE").InnerText);
                    string name = node.SelectSingleNode("NAME").InnerText;
                    Size ssize = StringToSize(node.SelectSingleNode("SIZE").InnerText);
                    List<List<Point>> llistp = new List<List<Point>>();
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        if (node1.Name == "SINGLE")
                        {
                            List<Point> listp = new List<Point>();
                            foreach (XmlNode node2 in node1.ChildNodes)
                            {
                                listp.Add(StringToPoint(node2.InnerText));
                            }
                            llistp.Add(listp);
                        }
                    }
                    m_singlechoicearea.Add(new SingleChoiceArea(rect, name, llistp, ssize));
                }
                foreach (XmlNode node in uclist)
                {
                    float score = 0;
                    float.TryParse(node.SelectSingleNode("SCORE").InnerText, out score);
                    string name = node.SelectSingleNode("NAME").InnerText;
                    Rectangle rect = StringToRectangle(node.SelectSingleNode("RECTANGLE").InnerText);
                    UnChoose uc = new UnChoose(score, name, rect);
                    m_unchoose.Add(uc);
                }
            }
            catch
            {
                Reset();
                return false;
            }
            return true;
        }
        internal void SetDataToNode(TreeNode m_tn)
        {
            String keyname = "特征点";
            TreeNodeCollection fp = m_tn.Nodes[keyname].Nodes;
            foreach (TriAngleFeature ft in m_fp)
            {
                TreeNode t = new TreeNode();
                int pointnum = fp.Count + 1;
                t.Name = t.Text = keyname + pointnum;
                t.Tag = ft;
                fp.Add(t);
            }
            keyname = "选择题";
            TreeNodeCollection sc = m_tn.Nodes[keyname].Nodes;
            foreach (SingleChoiceArea sca in m_singlechoicearea)
            {
                TreeNode t = new TreeNode();
                int pointnum = sc.Count + 1;
                t.Name = t.Text = keyname + pointnum;
                t.Tag = sca;
                sc.Add(t);
            }
            keyname = "非选择题";
            TreeNodeCollection uc = m_tn.Nodes[keyname].Nodes;
            foreach (UnChoose ucc in m_unchoose)
            {
                TreeNode t = new TreeNode();
                int pointnum = uc.Count + 1;
                t.Name = t.Text = keyname + pointnum;
                t.Tag = ucc;
                uc.Add(t);
            }
        }
        
        private Point StringToPoint(string str)
        {            
            String[] vstr = str.Split(',');
            if (vstr.Count() != 2)
                throw new NotImplementedException();
            int x, y;
            int.TryParse(vstr[0], out x);
            int.TryParse(vstr[1], out y);
            return new Point(x,y); 
        }
        private Size StringToSize(string str)
        {
            String[] vstr = str.Split(',');
            if (vstr.Count() != 2)
                throw new NotImplementedException();
            int x, y;
            int.TryParse(vstr[0], out x);
            int.TryParse(vstr[1], out y);
            return new Size(x, y); 
        }
        private Rectangle StringToRectangle(string str)
        {
            String[] vstr = str.Split(',');
            if (vstr.Count() != 4)
                throw new NotImplementedException();
            int x, y, w, h;
            int.TryParse(vstr[0], out x);
            int.TryParse(vstr[1], out y);
            int.TryParse(vstr[2], out w);
            int.TryParse(vstr[3], out h);
            return new Rectangle(x,y,w,h);
        }

        public TriAngleFeature[] FeaturePoints
        {
            get { return m_fp; }
        }
        public List<UnChoose> UnChooses
        {
            get { return m_unchoose; }
        }
        public String Filename
        {
            get { return m_filename; }
            set { m_filename = value; }
        }
        public Size Imgsize
        {
            get { return imgsize; }
            set { imgsize = value; }
        }
        public String NodeName{ get { return "/TEMPLATE"; } }
        internal List<int> ComputeChoice(Image image,Point paperCornerPoint)
        {
            List<int> ret = new List<int>();
            Point tp = m_fp[0].CornerPoint();
            Point offset = paperCornerPoint;
            offset.Offset(-tp.X, -tp.Y);
            foreach (SingleChoiceArea msc in m_singlechoicearea)
            {
                List<int> ccret = new List<int>();
                msc.ComputeChoice(image, offset, ccret);
                ret.AddRange(ccret);
                 CDebug.Debug1(image, ccret, offset, msc);
            }
            CDebug.DebugEndMessageBox("完成一张试卷");
            return ret;
        }
        internal Answer CreateAnswerTemplate()
        {
            //TODO: CreateAnswerTemplate()
            Answer answer = new Answer();
            int cnt = 0; // m_unchoose.Count;
            for (int i = 0; i < m_singlechoicearea.Count; i++)
            {
                cnt += m_singlechoicearea[i].Count();
            }
            if (answer.Count != cnt)
            {
                answer.Count = cnt;
            }
            cnt = 0;
            for (int i = 0; i < m_singlechoicearea.Count; i++)
            {
                for (int j = 0; j < m_singlechoicearea[i].Count(); j++)
                {
                    answer.SetMaxScore(cnt, 1);
                    answer.SetType(cnt, QuestionType.SingleChoice);
                    cnt++;
                }
            }
            //for (int i = 0; i < m_unchoose.Count; i++)
            //{
            //    answer.SetType(cnt, QuestionType.UnChoose);
            //    answer.SetMaxScore(cnt, m_unchoose[i].IntScore);
            //    cnt++;
            //}
            return answer;            
        }
        internal Point CornerPoint()
        {
            return m_fp[0].CornerPoint();
        }
        internal List<PaperBlockTemplate> BlockTemplates()
        {
            List<PaperBlockTemplate> pbts = new List<PaperBlockTemplate>();
            foreach (UnChoose uc in UnChooses)
            {
                PaperBlockTemplate pbt = new PaperBlockTemplate(uc);
                pbts.Add(pbt);
            }
            return pbts;
        }
        public int UnChoiceQuestionCount
        {
            get { return UnChooses.Count; }
        }
        public int ChoiceQuestionCount
        {
            get
            {
                return m_singlechoicearea.Sum(s => s.Count());
            }
        }
        private List<UnChoose> m_unchoose;
        private List<SingleChoice> m_singlechoice;
        private List<SingleChoiceArea> m_singlechoicearea;
        private TriAngleFeature m_bigfp;
        private TriAngleFeature[] m_fp;
        private String m_filename;
        private Size imgsize;
        private double _angle;
        public double Angle
        {
            get
            {
                if (_angle < -1000)
                {
                    Point p0 = FeaturePoints[0].CornerPoint();
                    Point p1 = FeaturePoints[1].CornerPoint();
                    CnblogsDotNetSDK.Utility.Geometry.Vector2f vf1 =
                        new CnblogsDotNetSDK.Utility.Geometry.Vector2f(p1.X - p0.X, p1.Y - p0.Y);
                    _angle = vf1.GetDirection();
                }
                return _angle;
            }
        }
    }
}
