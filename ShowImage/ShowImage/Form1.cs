using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;

namespace ShowImage
{
    public delegate void InvokDeleGate(string act);
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class Form1 : Form
    {
        public delegate string deteRightHtml(string html);
        public Form1()
        {
            InitializeComponent();
            buttonHide.Text = ">>";
            StopThread = false;
            splitContainer2.Panel1Collapsed = true;
            Ctrlist = new List<Control>();
            Ctrlist.AddRange(new Control[]{this.buttonN100,this.buttonNext,this.buttonP100,//this.buttonHide,
                this.buttonPrevious,this.buttonQuit,this.buttonSetDatabase,this.buttonTest,this.checkBoxouthtml});
            ReadConfig();
            Init();
            //foreach (Control b in Ctrlist)
            //    b.Enabled = false;
            //this.buttonHide.Enabled = true;
        }
        private void Init()
        {
            this.activequestionid = -1;
            encode = "utf-8";
            db = null;
            existdb = false;
            NexturlLastfind = false;
            ///////
            string urlt = "";
            if (cfgkeyvalue.ContainsKey("urltemplate"))
                urlt = cfgkeyvalue["urltemplate"];
            if (urlt.Contains("(*)") && urlt.Contains("http"))
                nu = new NextUrlBEID(urlt);
            else if (!urlt.Contains("(*)") && urlt.Contains("http"))
                nu = new NextUrlNE(urlt);
            else if (!urlt.Contains("(*)") && urlt.Contains(".txt") && urlt.Contains("[lineasurl]"))
                nu = new NextUrlTextLineBE(urlt);
            else
                return;
            if (!nu.ConstructOK())
                return;
            img = new Img(cfgkeyvalue);
            if (!img.OK())
                return;
            string replacetonullstr = "";
            if (cfgkeyvalue.ContainsKey("replacetonull"))
                replacetonullstr = cfgkeyvalue["replacetonull"];
            replacetonull = SplitToMultiList(replacetonullstr);
            if (cfgkeyvalue.ContainsKey("nextimgurllastfind"))
            {
                if (cfgkeyvalue["nextimgurllastfind"].ToLower() == "true")
                    NexturlLastfind = true;
            }
            string nexturlreplacestr = "";
            NextUrlReplace = new List<string>();
            if (cfgkeyvalue.ContainsKey("nexturlreplace"))
            {
                nexturlreplacestr = cfgkeyvalue["nexturlreplace"];
                string[] items = nexturlreplacestr.Split(new string[] {"[","]", "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length == 2)
                {
                    NextUrlReplace.Add(items[0].Replace("@@", "-"));
                    NextUrlReplace.Add(items[1].Replace("@@", "-"));
                }
            }          
           existdb = true;
        }
        public static List<List<string>> SplitToMultiList(string str)
        {
            List<List<string>> rell = new List<List<string>>();
            string[] replace = str.Split(new string[] { "]", "[" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in replace)
            {
                string[] items = s.Split(new string[] { "[", "]", "-" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> ls = new List<string>();
                if (items.Length == 2)
                {
                    ls.Add(items[0].Replace("@@", "-"));
                    ls.Add(items[1].Replace("@@", "-"));
                }
                else if (items.Length == 1)
                {
                    ls.Add(items[0].Replace("@@", "-"));
                    ls.Add("");
                }
                else
                {
                    continue;
                }
                rell.Add(ls);
            }
            return rell;
        }
        private void ReadConfig(string filename = "cfg.ini")
        {
            if (File.Exists(filename))
            {
                string content = File.ReadAllText(filename);
                List<string> items = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                cfgkeyvalue = new Dictionary<string, string>();
                foreach (string s in items)
                {
                    if (s.Contains("="))
                    {
                        string name = s.Substring(0, s.IndexOf('=')).Trim();
                        if (ValidateName(name) && !cfgkeyvalue.ContainsKey(name))
                            cfgkeyvalue[name] = s.Substring(s.IndexOf('=') + 1);
                    }
                }
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            webBrowser1.ObjectForScripting = this;
            base.OnLoad(e);
        }
        private void buttonHide_Click(object sender, EventArgs e)
        {
            if (buttonN100.Enabled == false)
            {
                StopThread = true;
                return;
            }
            if (buttonHide.Text == "<<")
            {
                buttonHide.Text = ">>";
                splitContainer2.Panel1Collapsed = true;
            }
            else
            {
                buttonHide.Text = "<<";
                splitContainer2.Panel1Collapsed = false;
            }
        }
        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            PreviousShow();
        }
        private void buttonNext_Click(object sender, EventArgs e)
        {
            NextShow();
        }
        private void buttonQuit_Click(object sender, EventArgs e)
        {

        }
        private void NextShow()
        {
            this.activequestionid++;
            if (!nu.HasNextUrl(activequestionid))
            {
                this.activequestionid--;
                webBrowser1.DocumentText = "<h1>已经是最后一组没有可以显示的图片了<h1>";
                return;
            }
            ShowItem();
        }
        private void PreviousShow()
        {
            this.activequestionid--;
            if (!nu.HasNextUrl(activequestionid))
            {
                this.activequestionid++;
                webBrowser1.DocumentText = "<h1>这是第一题没有可以显示的图片了</h1>";
                return;
            }
            ShowItem();
        }
        private void buttonP100_Click(object sender, EventArgs e)
        {
            this.activequestionid -= 100;
            if (!nu.HasNextUrl(activequestionid))
            {
                this.activequestionid = -1;
                webBrowser1.DocumentText = "<h1>这是第一题没有可以显示的图片了</h1>";
                return;
                //if (au.Sum() == 0)
                //{
                //}
            }
            ShowItem();
        }
        private void buttonN100_Click(object sender, EventArgs e)
        {
            this.activequestionid += 100;
            if (!nu.HasNextUrl(activequestionid))
            {
                this.activequestionid = - 100;
                webBrowser1.DocumentText = "<h1>已经是最后一组没有可以显示的图片了<h1>";
                return;
            }
            ShowItem();
        }
        private void ShowItem()
        {
            try
            {
                //showfiletxt(activequestionid + "- pageid:" + au.Sum());
                //string html = "<h1>暂时未找到该题</h1>";
                string url = nu.Url();
                Url = url;
                //string htmlpage = GetWebClient(url,encode);
                if (NextUrlReplace.Count == 2)
                    url = url.Replace(NextUrlReplace[0], NextUrlReplace[1]);

                string htmlpage = GetRightHtml(url);
                nu.SaveUrl(htmlpage, activequestionid, url);
                // if img.has imglist
                if (checkBoxouthtml.Checked)
                    File.WriteAllText("out.html",""); 
                htmlpage = img.GetImgList(htmlpage, this); //这里会输出
                if( nu  is  NextUrlTextLineBE )
                    htmlpage = htmlpage.Replace(">1<",">当前序号"+activequestionid+"<");
                if (img.NeedNewThread)
                {
                    this.Html = htmlpage;
                    img.NeedNewThread = false;
                  
                    System.Threading.Thread nonParameterThread = new Thread(new ThreadStart(RunThread));
                    nonParameterThread.Start();
                    return;
                }

                if (checkBoxShow.Checked)
                {
                    foreach (List<string> s in replacetonull)
                        htmlpage = htmlpage.Replace(s[0], s[1]);                    
                }
                else
                {
                    htmlpage = "<br>本页面以加载完毕，不显示<br>";
                }
                webBrowser1.DocumentText = "<a href=\"" + url + "\">本页网址" + url + "</a>    \r\n" + htmlpage;
            }
            catch (System.Data.OleDb.OleDbException ole)
            {
                webBrowser1.DocumentText = "<h1>找到该题出现故障</h1><p>" + ole.Message + "</p>"; ;
            }
            catch (Exception e)
            {
                nu.SaveUrl("", activequestionid, Url);
                webBrowser1.DocumentText = "<h1>找不到;" + Url + "<br> 故障原因;</h1><p>" + e.Message + "</p>"; ;
            }
            //DataRow drs = sortdt.Rows[activequestionid];
            //DataTable dt = QueryImgTableByPageId((int)drs["id"]);
            //showfiletxt(activequestionid + "- pageid:" + (int)drs["id"]);
            //if (dt.Rows.Count > 0)
            //{
            //    html = data.ConstructItem(dt);
            //}
        }

        private void RunThread()
        {            
            try
            {
                this.Invoke(new InvokDeleGate(EnabledBtn),new object[]{"false"});
                string htmlpage =img.GetImgList_Thread(this);               
                if (checkBoxouthtml.Checked || checkBoxShow.Checked)
                {
                    foreach (List<string> s in replacetonull)
                        htmlpage = htmlpage.Replace(s[0], s[1]);
                }
                else
                {
                    htmlpage = "<br>本页面以加载完毕，不显示<br>";
                }
                webBrowser1.DocumentText = "<a href=\"" + Url + "\">本页网址" + Url + "</a>    \r\n" + htmlpage;
            }
            catch (System.Data.OleDb.OleDbException ole)
            {
                webBrowser1.DocumentText = "<h1>找到该题出现故障</h1><p>" + ole.Message + "</p>"; ;
            }
            catch (Exception e)
            {
                nu.SaveUrl("", activequestionid, Url);
                webBrowser1.DocumentText = "<h1>找不到;" + Url + "<br> 故障原因;</h1><p>" + e.Message + "</p>"; ;
            }

            this.Invoke(new InvokDeleGate(EnabledBtn), new object[] { "true" });
        }
        public void EnabledBtn(string act)
        {
            bool action = false;
            if (act == "true")
            {
                action = true;
                this.buttonHide.Text = this.buttonHide.Text.Substring(0, 2);
            }
            else
            {
                action = false;
                this.buttonHide.Text = this.buttonHide.Text + "Stop";
            }
            foreach (Control c in Ctrlist)
                c.Enabled = action;
        }
        public string GetRightHtml(string url)
        {
            string htmlpage = GetHttpWebRequest(url, encode);
            if (!htmlpage.Contains(encode))
            {
                if (htmlpage.Contains("gb2312")) encode = "gb2312";
                if (htmlpage.Contains("utf-8")) encode = "utf-8";
                if (htmlpage.Contains("gbk")) encode = "gbk";

                //htmlpage = GetWebClient(url, encode);
                htmlpage = GetHttpWebRequest(url, encode);
            }
            return htmlpage;
        }
        private bool ValidateName(string name)
        {
            if (name == "") return false;
            foreach (char c in name.ToLower())
                if (!"0123456789abcdefghikjlmnopqrstuvwxyz".Contains(c))
                    return false;
            if ("0123456789".Contains(name[0]))
                return false;
            return true;
        }
        private string GetIniString(string src, string b, string e)
        {
            string dst = src.Substring(src.IndexOf(b) + b.Length) + e;
            dst = dst.Substring(0, dst.IndexOf(e)).Trim();           
            return dst;
        }
        public void showfiletxt(string text)
        {
            this.textBoxShow.Text = text;
        }

        private string GetWebClient(string url, string encode)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding(encode));
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }
        private string GetWebRequest(string url, string encode)
        {
            Uri uri = new Uri(url);
            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(encode));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        private string GetHttpWebRequest(string url, string encode)
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(encode));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }


        private void SetNewDatabaseFilename(string dbfullname)
        {
            //this.dbfullname = dbfullname;
            //if (!File.Exists(dbfullname))
            //{
            //    existdb = false;
            //   // MessageBox.Show(
            //    //throw new Exception(dbfullname + "数据库不存在，请重新选择");
            //    return;
            //}
            //if (db != null)
            //    db.connClose();
            //db = null;
            //db = new Db.ConnDb(dbfullname);
            //existdb = true;
        }
        private void RefreshInit()
        {
            //if (!existdb)
            //    return;

            //showfiletxt("本题库共有 " + au.Sum() + " 道记录");
        }
        private void buttonSetDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.textBoxShow.Text = "配置文件：" + fd.FileName;
                if(fd.FileName.EndsWith("ini") && File.Exists(fd.FileName))
                {
                    ReadConfig(fd.FileName);
                    Init();
                }
            }
        }
        private void buttonTest_Click(object sender, EventArgs e)
        {
            if (!existdb) return;
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.textBoxShow.Text = "测试文件：" + fd.FileName;
                if (fd.FileName.EndsWith("txt") && File.Exists(fd.FileName))
                {
                    string urlhead = nu.Url().Substring(0, nu.Url().LastIndexOf('/')) + "/";
                    string html = File.ReadAllText(fd.FileName);
                    string htmlpage = "";
                    int sum = 0;
                    //if(html.Length>1024000)
                    //    htmlpage = img.GetImgListBigStr(html,urlhead);
                    //else
                        htmlpage = img.GetImgList(html, urlhead,ref sum,this);
                    foreach (List<string> s in replacetonull)
                        htmlpage = htmlpage.Replace(s[0], s[1]);
                    File.WriteAllText(fd.FileName + ".html", htmlpage);
                    if (checkBoxouthtml.Checked)
                        webBrowser1.DocumentText = "<a href=\"" + fd.FileName + "\">本页测试文件" + fd.FileName + "</a>    \r\n" + htmlpage;
                }
                else
                {
                    this.textBoxShow.Text = "错误！  待测试文件不是txt文件";
                }
            }
        }
        private DataTable QueryImgTableByPageId(int pid)
        {
            string sql = "select * from [img] where pid =" + pid + " order by id";
            DataSet ds = db.query(sql);
            return ds.Tables[0];
        }

        private bool existdb;
        private Db.ConnDb db;
        private int activequestionid;

        private NextUrl nu;
        private Img     img;
        private Dictionary<string, string> cfgkeyvalue;
        private List<List<string>> replacetonull;
        private List<string> NextUrlReplace;
        public string encode { get; set; }
        public string Url { get; set; }
        public bool NexturlLastfind { get; set; }
        public string Html { get; set; }
        public bool StopThread { get; set; }
        private List<Control> Ctrlist;

        private void buttonHide_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.O)
            {
                CreateHtml ch = new CreateHtml();
                if( ch.createhtml())
                    MessageBox.Show("已生成html");
                else
                    MessageBox.Show("未生成html，out.html文件或不存在");
            }else if(e.KeyCode == Keys.R)
            {
                this.Hide();
                FormReplace f = new FormReplace();
                string replacestr = "";
                if (replacetonull.Count > 0) 
                replacestr = cfgkeyvalue["replacetonull"];
                f.Init(replacestr);
                f.ShowDialog();
                this.Show();
               
            }
        }

        private void buttonReadme_Click(object sender, EventArgs e)
        {
            FormReadMe f = new FormReadMe();
            f.ShowDialog();
        }

    }
    public class Img
    {
        public Img(Dictionary<string, string> cfgkeyvalue)
        {
            string imguri = Getcfgkeyvalue(cfgkeyvalue, "imguri");
            string nextimgurl = Getcfgkeyvalue(cfgkeyvalue, "nextimgurl");
            string imgmult = Getcfgkeyvalue(cfgkeyvalue, "imgmult");
            string txtimguri = Getcfgkeyvalue(cfgkeyvalue, "txtimguri");
            string txtlineasurl = Getcfgkeyvalue(cfgkeyvalue, "txtlineasurl");
            string continuestr = Getcfgkeyvalue(cfgkeyvalue, "continueindex");

            if (imgmult == "true")
                ImgMulti = true;
            else
                ImgMulti = false;
            multImgBE = SplitToMultiList(imguri);
            multUrlBE = SplitToMultiList(nextimgurl);
            multTxtImgBE = SplitToMultiList(txtimguri);

            TxtLineAsUrlBE = SplitToMultiList(txtlineasurl);
            ImgMultiCondition = Getcfgkeyvalue(cfgkeyvalue, "nextimgcondition");
            ImgExistCondition = Getcfgkeyvalue(cfgkeyvalue, "imgexistcondition");
            if (multUrlBE.Count == 0 || multImgBE.Count == 0)
                ImgMulti = false;           
            maxitemscount = 50;
            string ms = Getcfgkeyvalue(cfgkeyvalue, "maxitemscount");
            if (ValidNumber(ms))
                maxitemscount = Convert.ToInt32(ms);          
            NeedNewThread = false;
            NowIndex = 1;
            ContinueIndex = -1;
            if (ValidNumber(continuestr))
                ContinueIndex = Convert.ToInt32(continuestr);

        }
        private static List<List<string>> SplitToMultiList(string multistr)
        {
            List<List<string>> rell = new List<List<string>>();
            string[] cfgitem = multistr.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < cfgitem.Length; i++)
            {
                string[] be = cfgitem[i].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (be.Length == 2)
                {
                    List<string> item = new List<string>();
                    item.Add(be[0].Replace("@@", "-"));
                    item.Add(be[1].Replace("@@", "-"));
                    rell.Add(new List<string>(item));
                }
            }
            return rell;
        }
        internal string GetImgListBigStr(string htmlpage, string urlhead)
        {
            string ret = "";
            int indexB = 0,indexE = 0;
            int sum= 0;
            if (htmlpage.Length > 500 * 1024)
            {
                while (indexE != -1)
                {
                    indexE = htmlpage.LastIndexOf("<p>", indexB + 512000<htmlpage.Length-1?indexB + 512000:htmlpage.Length-1);
                    if (indexE != -1 && indexE > indexB)
                    {
                        ret += GetImgList(htmlpage.Substring(indexB, indexE - indexB), urlhead, ref sum);
                        indexB = indexE + "<p>".Length;
                    }
                    else
                    {
                        ret += GetImgList(htmlpage.Substring(indexB), urlhead,ref sum);
                        break;
                    }
                }
            }
            return ret;
        }
        internal string GetImgList(string htmlpage, string urlhead)
        {
            int sum = -1;
            return GetImgList(htmlpage, urlhead,ref  sum);
        }
        internal string GetImgList(string htmlpage, string urlhead, ref int indexsum, Form1 form1)
        {
            string html = "";
            if (TxtLineAsUrlBE.Count > 0)
            {
                int sum =0;
                string imgs = htmlpage;
                string ImgBeginTag = multTxtImgBE[multTxtImgBE.Count - 1][0];
                string ImgEndTag = multTxtImgBE[multTxtImgBE.Count - 1][1];
                string urlBeginTag = TxtLineAsUrlBE[TxtLineAsUrlBE.Count - 1][0];
                string urlEndTag = TxtLineAsUrlBE[TxtLineAsUrlBE.Count - 1][1];
                for (int i = 0; i < TxtLineAsUrlBE.Count - 1; i++)
                {
                    List<string> ls = TxtLineAsUrlBE[i];
                    imgs = GetIniString(imgs, ls[0], ls[1]);
                }
                List<string> urlls = GetIniStrings(imgs, urlBeginTag, urlEndTag);
                foreach (string url in urlls)
                {
                    string urls = form1. GetRightHtml(url);
                    for (int i = 0; i < multTxtImgBE.Count - 1; i++)
                    {
                        List<string> ls = multTxtImgBE[i];
                        urls = GetIniString(urls, ls[0], ls[1]);
                    }

                    List<string> imgls = GetIniStrings(urls, ImgBeginTag, ImgEndTag);
                    foreach (string img in imgls)
                    {
                        string imgt = img;
                        if (img.Contains(ImgExistCondition))
                        {
                            //if (!img.StartsWith("http:"))
                            //    imgt = urlhead + img;
                            sum++;
                            if (form1.checkBoxouthtml.Checked)
                                File.AppendAllText("out.html", "<img src=\"[img]\">[i]</img>\r\n".Replace("[img]", imgt).Replace("[i]", sum.ToString()));//.Replace("<nexturl>",nexturl));
                            if (form1.checkBoxShow.Checked)
                                html += "<img src=\"[img]\">[i]</img>\r\n".Replace("[img]", imgt).Replace("[i]", sum.ToString());
                        }
                    }
                }
            }
            return html;
        }
        internal string GetImgList(string htmlpage, string urlhead, ref int indexsum)
        {
            string html;
            {
                #region getimglist txt
                string imgs = htmlpage;
                string title = "";
                if (htmlpage.Contains("<title>") && htmlpage.Contains("</title>"))
                    title = GetIniString(htmlpage, "<title>", "</title>");
                if (title.Contains("-"))
                    title = title.Substring(0, title.IndexOf("-"));
                html = title + "<br>\r\n";
                /////////////
                for (int i = 0; i < multTxtImgBE.Count - 1; i++)
                {
                    List<string> ls = multTxtImgBE[i];
                    imgs = GetIniString(imgs, ls[0], ls[1]);
                }
                string ImgBeginTag = multTxtImgBE[multTxtImgBE.Count - 1][0];
                string ImgEndTag = multTxtImgBE[multTxtImgBE.Count - 1][1];
                {
                    int index = 0;
                    int sum = 0;
                    if (indexsum >= 0)
                        sum = indexsum;
                    while (index != -1)
                    {
                        index = imgs.IndexOf(ImgBeginTag, index);
                        if (index != -1 && index + ImgBeginTag.Length < imgs.Length)
                        {
                            int endindex = imgs.IndexOf(ImgEndTag, index + ImgBeginTag.Length);
                            if (endindex != -1)
                            {
                                string img = imgs.Substring(index + ImgBeginTag.Length, endindex - index - ImgBeginTag.Length);
                                index = endindex + ImgEndTag.Length;
                                if (!img.Contains(ImgExistCondition))
                                    continue;
                                if (!img.StartsWith("http:"))
                                    img = urlhead + img;
                                sum++;
                                html += "<img src=\"[img]\">[i]</img>\r\n".Replace("[img]", img).Replace("[i]", sum.ToString());
                            }
                            else
                            {
                                index = -1;
                            }
                        }
                    }
                    if (indexsum >= 0)
                        indexsum = sum;
                }
                #endregion
            }
            return html;
        }
        public  string GetImgList(string htmlpage, Form1 form1)
        {
            string urlhead = form1.Url.Substring(0, form1.Url.LastIndexOf('/')) + "/";
            string urls = htmlpage;
            string imgs =htmlpage;
            string title = GetIniString(urls, "<title>", "</title>");
            if (title.Contains("-"))
                title = title.Substring(0, title.IndexOf("-"));
            string html = title + "<br>\r\n";
            /////////////
        
            int sum = 0;
            if (ContinueIndex != -1)
                sum = ContinueIndex;               

            string ImgBeginTag = multImgBE[multImgBE.Count - 1][0];
            string ImgEndTag = multImgBE[multImgBE.Count - 1][1];
            if (ImgMulti == false)
            {
                for (int i = 0; i < multImgBE.Count - 1; i++)
                {
                    List<string> ls = multImgBE[i];
                    imgs = GetIniString(imgs, ls[0], ls[1]);
                }
                int index = 0;
                while (index != -1)
                {
                    index = imgs.IndexOf(ImgBeginTag, index);
                    if (index != -1 && index + ImgBeginTag.Length < imgs.Length)
                    {
                        int endindex = imgs.IndexOf(ImgEndTag, index + ImgBeginTag.Length);
                        if (endindex != -1)
                        {
                            string img = imgs.Substring(index + ImgBeginTag.Length, endindex - index - ImgBeginTag.Length) ;
                            if (!img.Contains(ImgExistCondition))
                                continue;
                            if (!img.StartsWith("http:"))
                                img = urlhead + img;                        
                            sum++;
                            html += "<img src=\"[img]\">[i]</img>\r\n".Replace("[img]", img).Replace("[i]", sum.ToString());
                            index = endindex + ImgEndTag.Length;
                        }
                        else
                        {
                            index = -1;
                        }
                    }
                }
                if (form1.checkBoxouthtml.Checked)
                    File.AppendAllText("out.html", html);
            }
            else
            {
                do
                {
                    string nexturl = GetNextUrl(urls, form1.Url, form1.NexturlLastfind);

                    for (int i = 0; i < multImgBE.Count - 1; i++)
                    {
                        List<string> ls = multImgBE[i];
                        imgs = GetIniString(imgs, ls[0], ls[1]);
                    }

                    List<string> imgls = GetIniStrings(imgs, ImgBeginTag, ImgEndTag);
                    foreach (string img in imgls)
                    {
                        string imgt = img;
                        if (img.Contains(ImgExistCondition))
                        {
                            if (!img.StartsWith("http:"))
                                imgt = urlhead + img;
                            sum++;
                            if (form1.checkBoxouthtml.Checked)
                                File.AppendAllText("out.html", "<img src=\"[img]\">[i]</img>\r\n".Replace("[img]", imgt).Replace("[i]", sum.ToString()));//.Replace("<nexturl>",nexturl));
                            if (form1.checkBoxShow.Checked)
                                html += "<img src=\"[img]\">[i]</img>\r\n".Replace("[img]", imgt).Replace("[i]", sum.ToString());
                        }
                    }


                    if (nexturl == "") break;
                    try
                    {
                        urls = form1.GetRightHtml(nexturl);
                    }
                    catch (Exception e)
                    {
                        urls = "";
                        break;
                    }
                    imgs = urls;

                    if (sum > 10 && !NeedNewThread)
                    {
                        NeedNewThread = true;
                        this.ThreadHtml = html;
                        this.ThreadImgs = urls;
                        this.NowIndex = sum;
                        break;
                    }
                    else if (sum > maxitemscount)
                    {
                        break;
                    }
                } while (urls.Contains(ImgMultiCondition));
            }
            return html;
        }
        public  string GetImgList_Thread(Form1 form1)
        {
            string urlhead = form1.Url.Substring(0, form1.Url.LastIndexOf('/')) + "/";
            string html = "";
            string imgs = this.ThreadImgs;
            string urls = imgs;

            string ImgBeginTag = multImgBE[multImgBE.Count - 1][0];
            string ImgEndTag = multImgBE[multImgBE.Count - 1][1];
            if (ImgMulti == true)            
            {
                int sum = this.NowIndex;
                do
                {
                    if (form1.checkBoxouthtml.Checked)
                        File.AppendAllText("out.html", "<nexturl>\r\n");
                            
                    string nexturl = GetNextUrl(urls, form1.Url, form1.NexturlLastfind);
                    form1.Invoke(new InvokDeleGate(form1.showfiletxt), new object[] { nexturl });
                    for (int i = 0; i < multImgBE.Count - 1; i++)
                    {
                        List<string> ls = multImgBE[i];
                        imgs = GetIniString(imgs, ls[0], ls[1]);
                    }

                    List<string> imgls = GetIniStrings(imgs, ImgBeginTag, ImgEndTag);
                    foreach (string img in imgls)
                    {
                        string imgt = img;
                        if (img.Contains(ImgExistCondition))
                        {
                            if (!img.StartsWith("http:"))
                                imgt = urlhead + img;
                            sum++;
                            if (form1.checkBoxouthtml.Checked)
                                File.AppendAllText("out.html", "<img src=\"[img]\">[i]</img>\r\n".Replace("[img]", imgt).Replace("[i]", sum.ToString()));//.Replace("<nexturl>",nexturl));
                            if (form1.checkBoxShow.Checked)
                                html += "<img src=\"[img]\">[i]</img>\r\n".Replace("[img]", imgt).Replace("[i]", sum.ToString());
                        }
                    }

                   
                    if (nexturl == "") break;
                    if (form1.StopThread || sum>maxitemscount)
                    {
                        form1.StopThread = false;
                        File.WriteAllText("nexturl_" + System.DateTime.Now.ToString().Replace(" ", "_").Replace(":", ".") + ".txt", nexturl
                             + "  nextIndex:" + sum);
                        break;
                    }
                    try
                    {
                        urls = form1.GetRightHtml(nexturl);
                    }
                    catch (Exception e)
                    {
                        urls = "";
                        break;
                    }
                    imgs = urls;
                } while (urls.Contains(ImgMultiCondition));
            }
            return this.ThreadHtml+html;
        }

        public bool OK()
        {
            return true;;
        }
        public bool ImgMulti { get; set; }
        private string GetNextUrl(string html,string url,bool lastfind)
        {
            if (lastfind)
            {
                for (int i = 0; i < multUrlBE.Count - 1; i++)
                {
                    List<string> ls = multUrlBE[i];
                    html = GetIniString(html, ls[0], ls[1]);
                }
                html = GetIniStringLastFind(html, multUrlBE[multUrlBE.Count - 1][0], multUrlBE[multUrlBE.Count - 1][1]);
            }
            else
            {
                foreach (List<string> ls in multUrlBE)
                {
                    html = GetIniString(html, ls[0], ls[1]);
                }
            }
            html = html.Trim();
            if(html.Contains("<") ||html.Contains(" ") || html.Contains("\""))
                return "";
            if (html.StartsWith("http://"))
                return html.Trim();
            else if( html.Trim().EndsWith(".html") || html.EndsWith(".htm"))
            {
                if (url.StartsWith("http:"))
                {
                    if (html.Contains("/"))
                    {
                       string[]  paths = html.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                       foreach (string s in paths)
                       {
                           if (url.Contains(s))
                           {
                              return   url.Substring(0, url.LastIndexOf("/"+s)) + html;
                           }
                       }
                    }
                    return url.Substring(0, url.LastIndexOf('/')) + "/" + html;                   
                }
            }
            return "";
        }
        private List<string> GetIniStrings(string txt, string b, string e)
        {
            List<string> r = new List<string>();           
            BEPos bp = new BEPos(0, txt.Length, txt);
            BETag bt = new BETag(b+"-"+e);
            bp = bt.BEPos(txt);
            //是否 一个txt 中包含多条记录
            while (bp.Valid())
            {
                r.Add(bp.String);
               bp = bt.NextBEPos(bp);
            }
            return r;
        }
        private string GetIniString(string src, string b, string e)
        {
            string dst = src.Substring(src.IndexOf(b) + b.Length) + e;
            dst = dst.Substring(0, dst.IndexOf(e)).Trim();
            return dst;
        }
        private string GetIniStringLastFind(string src, string b, string e)
        {
            string dst = src.Substring(src.LastIndexOf(b) + b.Length) + e;
            dst = dst.Substring(0, dst.IndexOf(e)).Trim();
            return dst;
        }
        private string Getcfgkeyvalue(Dictionary<string, string> cfgkeyvalue, string name)
        {
            if (cfgkeyvalue.ContainsKey(name))
                return cfgkeyvalue[name];
            return "";
        }
        private bool ValidNumber(string ms)
        {
            if (ms == "") return false;
            foreach (char c in ms)
                if (!"0123456789".Contains(c)) return false;
            return true;
        }        
         
        public bool NeedNewThread { get; set; }
        public string ThreadHtml { get; set; }
        public string ThreadImgs { get; set; }
        public int NowIndex { get; set; }
        public int ContinueIndex { get; set; }  
   
        private List<List<string>> multUrlBE;
        private List<List<string>> multImgBE;
        private string ImgMultiCondition;
        private string ImgExistCondition;
        private int maxitemscount;
        private List<List<string>> multTxtImgBE;
        private List<List<string>> TxtLineAsUrlBE;
        
    }

    public abstract class NextUrl
    {
        public NextUrl() { }//string cfg
        public abstract bool ConstructOK();
        public abstract string Url();
        public abstract void SaveUrl(string html,int activeid,string url);
        public abstract bool HasNextUrl(int activeid);
        public bool IsNumber(string s)
        {
            if (s == null) return false;
            foreach (char c in s)
            {
                if (!"0123456789".Contains(c))
                    return false;
            }
            return true;
        }
    }
    public class NextUrlBEID : NextUrl
    {
        private string urlt;
        private int E;
        private int B;
        private string nexturl;
        public NextUrlBEID(string cfg)
        {// [http://xxx.xxx.xxx/xx/xx/(*).html][begin-end]
            nexturl = "";
            string[] cfgitem = cfg.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            if (cfgitem.Length != 2) { cok = false; return; }
            if (cfgitem[0].Contains("http://") && cfgitem[0].Contains("(*)"))
            {
                urlt = cfgitem[0].Trim();
            }
            else { cok = false; return ; }
            string[] be = cfgitem[1].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (be.Length != 2 || !IsNumber(be[0]) || !IsNumber(be[1]))
            { cok = false; return; }
            B = Convert.ToInt32(be[0]);
            E = Convert.ToInt32(be[1]);
            if (B >= E)
                cok = false;
            else
                cok = true;
            nexturl = urlt.Replace("(*)", B.ToString());
        }
        public override bool ConstructOK()
        {
            return cok;
        }
        public override string Url()
        {
            return nexturl;
        }
        public override void SaveUrl(string html,int activeid,string url="")
        {
            if (HasNextUrl(activeid))
                nexturl = urlt.Replace("(*)", (B +1+ activeid).ToString());
            //else
            //    nexturl = "";
        }
        public override bool HasNextUrl(int activeid)
        {
            if (activeid > -1 && B + activeid < E)
                return true;
            return false;
        }

        public bool cok { get; set; }
    }
    public class NextUrlNE : NextUrl
    {
        private string nexturl;
        private bool cok;
        private bool hasnexturl;
        private List<List<string>> multBE;
        public NextUrlNE(string cfg)
        {
            // [http://xxx.xxx.xxx/xx/xx/(*).html][begin-end]
            hasnexturl = false;
            nexturl = "";
            string[] cfgitem = cfg.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            if (cfgitem.Length < 2) { cok = false; return; }
            if (cfgitem[0].Contains("http://") )
            {
                nexturl  = cfgitem[0].Trim();
                hasnexturl = true;
            }
            else { cok = false; return; }
            multBE = new List<List<string>>();
            for (int i = 1; i < cfgitem.Length; i++)
            {
                string[] be = cfgitem[i].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (be.Length == 2)
                {
                    List<string> item = new List<string>();
                    item.Add(be[0].Replace("@@","-"));
                    item.Add(be[1].Replace("@@","-"));
                    multBE.Add(new List<string>(item));
                }
            }
            if (multBE.Count==0)
            { cok = false; return; }
            cok = true;
        }
        public override bool ConstructOK()
        {
            return cok;
        }
        public override string Url()
        {
            return nexturl;
        }
        public override void SaveUrl(string html,int activeid,string url)
        {// if activeid 比上一个大 
            foreach (List<string> ls in multBE)
            {
                html = GetBEString(html, ls[0], ls[1]);
            }
            html = html.Trim().Replace("&amp;", "&");
            if (html.StartsWith("http://"))
            {
                nexturl = html;
                hasnexturl = true;
            }
            else if(html.Contains("<") || html.Contains(" ") || html.Contains("\""))
            {
                hasnexturl = false;
            }
            else if (html.Trim().EndsWith(".html") || html.EndsWith(".htm")||html.Contains("?"))
            {
                if (url.StartsWith("http:"))
                {
                    if (html.Contains("/"))
                    {
                        string[] paths = html.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in paths)
                        {
                            if (url.Contains(s))
                            {
                                nexturl = url.Substring(0, url.LastIndexOf("/"+s)) + html;
                                break;
                            }
                        }
                    }
                    else
                    {
                        nexturl = url.Substring(0, url.LastIndexOf('/')) + "/" + html;
                    }
                    hasnexturl = true;
                }
            }else{                
                hasnexturl = false;               
            }
        }
        public override bool HasNextUrl(int activeid)
        {
            return hasnexturl;
        }
        private string GetBEString(string src, string b, string e)
        {
            if (!src.Contains(b)) return "";
            string dst = src.Substring(src.IndexOf(b) + b.Length) + e;
            if (!dst.Contains(e)) return "";
            dst = dst.Substring(0, dst.IndexOf(e)).Trim();
            return dst;
        }
    }
    public class NextUrlTextLineBE : NextUrl
    {
        private string urlt;
        private List<string> urls;
        private int index;
        private string nexturl;
        public NextUrlTextLineBE(string cfg)
        {
            cok = true;
            urls = new List<string>();
            nexturl = "";
            index = -1;
            string[] cfgitem = cfg.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            if (cfgitem.Length != 2) { cok = false; return; }
            if (cfgitem[0].Contains(".txt") )
            {
                urlt = cfgitem[0].Trim();
                if(File.Exists(urlt))
                {
                    foreach (string s in File.ReadAllLines(urlt))
                        if (s.Contains("http"))
                            urls.Add(s);
                    if (urls.Count > 0)
                    {
                        index++;
                        urlt = nexturl = urls[index];
                        return;
                    }
                }
            }
            cok = false;             
        }
        public override bool ConstructOK()
        {
            return cok;
        }
        public override string Url()
        {
            return nexturl;
        }
        public override void SaveUrl(string html, int activeid, string url = "")
        {
            if (HasNextUrl(activeid))
            {
                index++;
                if (index > -1 && index < urls.Count)
                    nexturl = urls[index];
            }
        }
        public override bool HasNextUrl(int activeid)
        {
            if (activeid > -1 &&  activeid < urls.Count)
                return true;
            return false;
        }

        public bool cok { get; set; }
    }
}