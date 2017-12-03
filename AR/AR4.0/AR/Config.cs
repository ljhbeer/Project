using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

namespace AR
{
    public class Config
    {
        public PaperTemplate   PaperTemplate()
        {
            return _papertemplate;
        }
        public Papers  Papers()
        {
            return _papers;
        }
        public Answer Answer { get { return _answer; } }
        public Config(Papers _papers, PaperTemplate _papertemplate)
        {
            this._papers = _papers;
            this._papertemplate = _papertemplate;
            this._answer = null;
        }
        public void LoadPaperData(string fullname)
        {
            string rootname = "CONFIGDATA";
            string PapersNodeName = "/PAPERS";
            string PaperScoresName = "/PAPERSCORES";
            string path = fullname.Substring(0, fullname.LastIndexOf("\\"));
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fullname);
            _papertemplate.LoadXml(xmlDoc.SelectSingleNode(rootname + _papertemplate.NodeName));
            //answer
            LoadPapersXml(xmlDoc.SelectSingleNode(rootname + PapersNodeName));
            LoadPaperScoresXml(xmlDoc.SelectSingleNode(rootname + PaperScoresName));
            this._answer = _papertemplate.CreateAnswerTemplate();
            _answer.LoadXml(xmlDoc.SelectSingleNode(rootname + _answer.NodeName));            
        }

        
        public void SavePaperData(string fullname)
        {
            string path = fullname.Substring(0, fullname.LastIndexOf("\\"));
            //
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("CONFIGDATA");
            xmlDoc.AppendChild(root);
            XmlDocument xmlDocTemplate = _papertemplate.SaveToSupperXmlDoc();
            if (_answer == null)
                _answer = _papertemplate.CreateAnswerTemplate();
            XmlDocument xmlDocAnswer = _answer.SaveToSupperXmlDoc();
            XmlDocument xmlDocPaper = SavePaperToXmlDoc();
            XmlDocument xmlDocPaperScore = SavePaperScoreToXmlDoc();

            root.AppendChild(xmlDoc.ImportNode(xmlDocTemplate.DocumentElement.LastChild, true));
            root.AppendChild(xmlDoc.ImportNode(xmlDocAnswer.DocumentElement.LastChild, true));
            root.AppendChild(xmlDoc.ImportNode(xmlDocPaper.DocumentElement.LastChild, true));
            root.AppendChild(xmlDoc.ImportNode(xmlDocPaperScore.DocumentElement.LastChild, true));
            xmlDoc.Save(fullname);
        }


        internal void SaveScores(string filename)
        {
            string s="ID,总分,卷一,卷二";
            for (int i = 1; i < _answer.Count + 1; i++)
            {
                s += ",X" + i;
            }
            foreach (PaperBlockTemplate pbt in  _papertemplate.BlockTemplates())
            {
                s += "," + pbt.Name;
            }
            foreach (Paper p in _papers.PaperList)
            {
                s += "\r\n" + p.ID;
                string ss = "";
                float j1 = 0;
                int j2 = 0;
                foreach(float f in  _answer.ComputeScores(p.Optionanswers))
                {
                    ss += "," + f;
                    j1 += f;
                }
                foreach (PaperBlock pb in p.PaperblockList())
                {
                    j2 += pb.BlockScore.GetScore();
                    ss +=","+ pb.BlockScore.GetScore();
                }
                s +=","+ (j1 + j2) + "," + j1 + "," + j2 + ss;
            }
            s = s.Replace(",", "\t");
            System.IO.File.WriteAllText(filename, s);
        }
        public void InitAnswer()
        {
            Answer an = _papertemplate.CreateAnswerTemplate();
            if (_answer == null || !CompareStruct(_answer,an))
                _answer = an;           
        }
        private XmlDocument SavePaperToXmlDoc()
        {
            //string PapersNodeName = "/PAPERS";
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("CONFIGPAPERS");
            xmlDoc.AppendChild(root);
            XmlNode uclist = xmlDoc.CreateElement("PAPERS");
            root.AppendChild(uclist);

            foreach (Paper paper in _papers.PaperList)
            {
                XmlElement xe = xmlDoc.CreateElement("PAPER");
                xe.SetAttribute("ID", paper.ID.ToString());
                xe.SetAttribute("PATH", paper.FileName);
                xe.InnerXml = paper.ToXml();
                uclist.AppendChild(xe);              
            }
            return xmlDoc;
        }
        private XmlDocument SavePaperScoreToXmlDoc()
        {
            //string PapersNodeName = "/PAPERS";
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("CONFIGPAPERS");
            xmlDoc.AppendChild(root);
            XmlNode uclist = xmlDoc.CreateElement("PAPERSCORES");
            root.AppendChild(uclist);

            foreach (Paper paper in _papers.PaperList)
            {
                XmlElement xe = xmlDoc.CreateElement("PAPER");
                xe.SetAttribute("ID", paper.ID.ToString());
                xe.InnerXml = paper.BlocksToXml();
                uclist.AppendChild(xe);              
            }
            return xmlDoc;            
        }
        
        private bool CompareStruct(Answer _answer, Answer an)
        {
            if (_answer.Count != an.Count) return false;
            //for (int i = 0; i < _answer.Count; i++)    // Question 应该有ID
            //    if (_answer.question[i].ID != an.question[i].ID)
            //        return false;
            return true;
        }
        private void LoadPapersXml(XmlNode xmlNode)
        {
            try
            {
                String NodeName = "PAPERS";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.AppendChild(xmlDoc.ImportNode(xmlNode, true));
                XmlNodeList uclist = xmlDoc.SelectNodes(NodeName + "/*");
                List<Paper> papers = new List<Paper>();
                foreach (XmlNode node in uclist)
                {
                    List<Point> lp = new List<Point>();
                    List<int> intoption = new List<int>();
                    foreach (XmlNode n in node.ChildNodes)
                    {
                        if (n.Name == "POINT")
                        {
                            lp.Add(StringToPoint(n.InnerText));
                        }else if(n.Name == "OPTIONANSWER")
                        {
                            string Option = n.InnerText;
                            string[] suboption = Option.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            intoption = suboption.Select(r => Convert.ToInt32(r)).ToList();                            
                        }
                    }
                    string path = node.Attributes["PATH"].InnerText;
                    string sID = node.Attributes["ID"].InnerText;
                    if (lp.Count == 3)
                    {
                        Paper paper = new Paper(_papers, path, lp, Convert.ToInt32(sID));
                        papers.Add(paper);
                        if (intoption.Count == _papertemplate.ChoiceQuestionCount)
                                paper.Optionanswers = intoption;
                    }
                }
                _papers.Clear();
                _papers.AddPapers(papers);
            }
            catch
            {              
            }
        }
        private void LoadPaperScoresXml(XmlNode xmlNode)
        {
            try
            {
                String NodeName = "PAPERSCORES";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.AppendChild(xmlDoc.ImportNode(xmlNode, true));
                XmlNodeList uclist = xmlDoc.SelectNodes(NodeName + "/*");
                List<Paper> papers = new List<Paper>();
                foreach (XmlNode node in uclist)
                {
                    List<int> intblock = new List<int>();
                    foreach (XmlNode n in node.ChildNodes)
                    {
                        if (n.Name == "BLOCKSCORE")
                        {
                            string sblock = n.InnerText;
                            string[] subsblock = sblock.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            intblock = subsblock.Select(r => Convert.ToInt32(r)).ToList();
                        }
                    }
                   int ID = Convert.ToInt32( node.Attributes["ID"].InnerText);
                   _papers.PaperFromID(ID).IntBlocks = intblock;
                }                
            }
            catch
            {
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
            return new Point(x, y);
        }
        
        private Papers _papers;
        private PaperTemplate _papertemplate;
        private Answer _answer;

    }
}
