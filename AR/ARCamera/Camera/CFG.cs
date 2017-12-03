using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace Camera
{
    public partial class CFGForm : Form
    {
        public CFGForm()
        {
            InitializeComponent();
            if (File.Exists("cfg.xml"))
            {
                xmlFileName = "cfg.xml";
                ReadConfig();
            }
        } 
        public Answer[] Answer
        {
            get { return answer ; }
        }
        public Size Xuanzhetitable
        {
            get { return xuanzhetitable; }
        }
        public Rectangle[] Xuanzhetiquestion
        {
            get { return xuanzhetiquestion; }
            set { xuanzhetiquestion = value; }
        }
        public Rectangle[] Xuehaonum
        {
            get { return xuehaonum; }
            set { xuehaonum = value; }
        }
        public Rectangle Photorect
        {
            get { return photorect; }
            set { photorect = value; }
        }
        public Rectangle GetGdtRectangle(Rectangle rect, string name)
        {
            Rectangle r = new Rectangle();
            if (name == "xuehao")
            {
                r = new Rectangle(rect.X + xuehaogdt.X * rect.Width / xuehaotable.Width,
                                   rect.Y + xuehaogdt.Y * rect.Height / xuehaotable.Height,
                                   rect.Width*xuehaogdt.Width / xuehaotable.Width,
                                   rect.Height*xuehaogdt.Height / xuehaotable.Height);
            }
            else if (name == "xuanzheti")
            {
                r = new Rectangle(rect.X + xuanzhetigdt.X * rect.Width / xuanzhetitable.Width,
                                   rect.Y + xuanzhetigdt.Y * rect.Height / xuanzhetitable.Height,
                                   rect.Width*xuanzhetigdt.Width / xuanzhetitable.Width,
                                   rect.Height*xuanzhetigdt.Height / xuanzhetitable.Height);
            }
            else if (name == "tiankongti")
            {
                r = rect;
            }
            return r;
        }

        private void btnImportXmlCfg_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnDlg = new OpenFileDialog();
            opnDlg.Filter = "配置文件|*.xml; ";
            opnDlg.Title = "打开配置文件";
            opnDlg.ShowHelp = true;
            if (opnDlg.ShowDialog() == DialogResult.OK)
            {
                xmlFileName = opnDlg.FileName;
                ReadConfig();
            }
        }
        private void ReadConfig()
        {
            if (!File.Exists(xmlFileName)) return;
            XmlDocument xmlDoc=new XmlDocument(); 
            xmlDoc.Load(xmlFileName );
            XmlNodeList nodelist = xmlDoc.SelectNodes("/ARCFG/Nodes");
            XmlNode root = nodelist.Item(0);//查找 

            XmlNode xmlphoto = root.SelectSingleNode("Node/photorectangle");
            XmlNode xmlxuehao = root.SelectSingleNode("Node/dtk/xuehao");
            XmlNode xmlxuanzheti = root.SelectSingleNode("Node/dtk/xuanzheti");
            XmlNode xmltiankongti = root.SelectSingleNode("Node/dtk/tiankongti");
            XmlNode xmlanwer = root.SelectSingleNode("Node/answer");

            //--------
            photorect = GetRectangle(xmlphoto.FirstChild.InnerText);
            xuehaotable = GetTableSize(xmlxuehao.SelectSingleNode("table").InnerText );
            xuehaogdt = GetRectangle(xmlxuehao.SelectSingleNode("gdt").InnerText);
            XmlNodeList xmlnums = xmlxuehao.SelectNodes("num/*");
            xuehaonum = new Rectangle[xmlnums.Count];
            for (int i = 0; i < xmlnums.Count; i++)
                xuehaonum[i] = GetRectangle(xmlnums.Item(i).InnerText);

            xuanzhetitable = GetTableSize(xmlxuanzheti.SelectSingleNode("table").InnerText);
            xuanzhetigdt = GetRectangle(xmlxuanzheti.SelectSingleNode("gdt").InnerText);
            XmlNodeList xmlxzt = xmlxuanzheti.SelectNodes("question/*");
            xuanzhetiquestion = new Rectangle[xmlxzt.Count];
            for (int i = 0; i < xmlxzt.Count; i++)
                xuanzhetiquestion[i] = GetRectangle(xmlxzt.Item(i).InnerText);

            XmlNodeList xmlan = xmlanwer.SelectNodes("*");
            answer = new Answer[xmlan.Count];
            for (int i = 0; i < xmlan.Count; i++)
            {
                int id =Convert.ToInt32(  xmlan.Item(i).Attributes["id"].InnerText );
                int typeid = Convert.ToInt32( xmlan.Item(i).Attributes["typeid"].InnerText );
                double  value = Convert.ToDouble( xmlan.Item(i).Attributes["value"].InnerText );
                int option = GetOption(xmlan.Item(i).InnerText);
                answer[i] = new Answer(id, typeid,(float ) value, option);
            }

            //xe1.SetAttribute("genre","李赞红");//设置该节点genre属性 
            //xe1.SetAttribute("ISBN","2-3631-4");//设置该节点ISBN属性 
        }
        private int  GetOption(string str)
        {
            str.Trim();
            if (str.Length == 1)
            {
                int option = str.ToUpper().ElementAt(0) - 'A';
                if (option >= 0 && option < 4)
                    return option;
            }
            return -1;
        }
        private Size GetTableSize(string str)
        {
            Size s = new Size();
            string rexstr = "([0-9]+)\\.([0-9]+)";
            Regex regex = new Regex(rexstr, RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(str);
            if (mc.Count ==1)
            {
                foreach (Match m in mc)
                {
                    s.Width = Convert.ToInt32(m.Groups[1].Value);
                    s.Height  = Convert.ToInt32(m.Groups[2].Value);
                }
            }
            return s;
        }
        private Rectangle GetRectangle(string str)
        {
            Rectangle r = new Rectangle();
            string rexstr = "([0-9]+)\\.([0-9]+)-([0-9]+)\\.([0-9]+)";
            Regex regex = new Regex(rexstr, RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(str);
            if(mc.Count == 1){
                foreach(Match m in mc){
                     r.X = Convert.ToInt32(m.Groups[1].Value);
                     r.Y = Convert.ToInt32(m.Groups[2].Value);
                     r.Width = Convert.ToInt32(m.Groups[3].Value);
                     r.Height= Convert.ToInt32(m.Groups[4].Value);		             
                }
	        }
            return r;
        }

        private string xmlFileName;
        private Rectangle photorect;
        private Size xuehaotable;
        private Rectangle xuehaogdt;
        private Rectangle[] xuehaonum;
        private Size xuanzhetitable;
        private Rectangle xuanzhetigdt;
        private Rectangle[] xuanzhetiquestion;
        private Answer[] answer;

    }
}
