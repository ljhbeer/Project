using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace ToolsWeb
{
    public class CWeb
    {
        public CWeb()
        {
            OutCookies = "";
            strCookies = "";
            charset = "utf-8";
            charsettags = new BETags("[<head>-<title>][charset=->]");
        }

        public string GetOKUrl(string Url)
        {
            if (Url.StartsWith("https"))
            {
                return GetWebRequesthttps_Charset(ref charset, Url);
            }
            if(OutCookies=="")
            return GetWebRequest_Charset(ref charset, Url);
            return GetWebRequest_Charset(ref charset, Url, OutCookies);
        }

        private string GetWebRequesthttps_Charset(ref string charset, string url)
        {
            string txt;
            try
            {
                txt = GetWebRequesthttps(url, charset);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("The hostname could not be parsed."))
                {
                    txt = GetWebClient(url);
                    return txt;
                }
                else if (e.Message.Contains("未能解析此远程名称:"))
                {
                    throw new Exception("没有联网，无法获取远程数据");
                }
                else
                    throw e;
            }
            return txt;
        }
        private string GetWebRequesthttps(string url, string charset)
        {
            Uri uri = new Uri(url);
            Util.SetCertificatePolicy();

            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(charset));
            string strHTML = readerOfStream.ReadToEnd();

            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;

        }

        private string GetWebRequest_Charset(ref string charset, string url, string OutCookies)
        {
            string txt;
            try
            {
                txt = GetWebRequest(url, charset,OutCookies);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("The hostname could not be parsed."))
                {
                    txt = GetWebClient(url);
                    return txt;
                }
                else
                    throw e;
            }
            string cs = charsettags.Match(txt);
            if (cs == "")
            {
                charsettags = new BETags("[</title>-</head>][charset=->]");
                cs = charsettags.Match(txt).ToLower();
            }
            if (cs != "" && !cs.Contains(charset))
            {
                if (!DgvTools.ValidName(cs))
                {
                    if (cs.StartsWith("\""))
                        cs = cs.Substring(1, cs.IndexOf('\"') - 1);
                    if (cs.StartsWith("'"))
                        cs = cs.Substring(1, cs.IndexOf('\'') - 1);
                    if (cs.Contains("\""))
                        cs = cs.Substring(0, cs.IndexOf('\"'));
                    if (cs.Contains("'"))
                        cs = cs.Substring(0, cs.IndexOf('\''));
                    if (cs.EndsWith("/"))
                        cs = cs.Substring(0, cs.IndexOf('/')).Trim();

                }
                if (DgvTools.ValidName(cs))
                {
                    charset = cs;
                    txt = GetWebRequest(url, charset,OutCookies);
                }
            }
            return txt;
        }
        public string GetWebRequest_Charset(ref string charset, string url)
        {
            string txt ;
            try{
            	txt= GetWebRequest(url, charset);
            }catch(Exception e){
            	if(e.Message.Contains("The hostname could not be parsed.")){
            		txt = GetWebClient(url);
            		return txt;
            	}else
            	throw e;
            }
            string cs = charsettags.Match(txt);
            if (cs == "")
            {
                charsettags = new BETags("[</title>-</head>][charset=->]");
                cs = charsettags.Match(txt).ToLower();
            }
            if (cs!="" && !cs.Contains(charset))
            {
                if (!DgvTools.ValidName(cs))
                {
                    if (cs.StartsWith("\""))
                        cs = cs.Substring(1, cs.IndexOf('\"') - 1);
                    if (cs.StartsWith("'"))
                        cs = cs.Substring(1, cs.IndexOf('\'') - 1);
                    if (cs.Contains("\""))
                        cs = cs.Substring(0, cs.IndexOf('\"'));
                    if (cs.Contains("'"))
                        cs = cs.Substring(0, cs.IndexOf('\''));
                    if (cs.EndsWith("/"))
                        cs = cs.Substring(0,cs.IndexOf('/')).Trim();

                }
                if (DgvTools.ValidName(cs))
                {
                    charset = cs;
                    txt = GetWebRequest(url, charset);
                }
            }
            return txt;
        }
        public string GetHttpWebRequest(string url, out string strCookies)
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            strCookies = FenxiCookie(result.Headers["Set-Cookie"]);
            result.Close();
            return strHTML;
        }
        public string GetHttpWebRequest(string url, string strCookies)
        {
            if (wl > 0)
            {
                strCookies += "WL=" + wl + ";";
            }
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            myReq.Headers.Add("Cookie", strCookies);
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            wl = FenxiCookieWl(result.Headers["Set-Cookie"]);
            result.Close();
            return strHTML;
        }
        public static string GetWebClient(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            //if (url.StartsWith("https//"))
            //    Util.SetCertificatePolicy();

            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }
        public string GetWebRequest(string url, string charset = "utf-8")
        {
            Uri uri = new Uri(url);
            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(charset));
            string strHTML = readerOfStream.ReadToEnd();

            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        // For OutCookies
        private string GetWebRequest(string url, string charset = "utf-8", string OutCookies = "")
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);

            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            myReq.Headers.Add("Cookie",OutCookies);

            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(charset));
            string strHTML = readerOfStream.ReadToEnd();

            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        public int FenxiCookieWl(string cookie)
        {
            if (cookie == null)
                return this.wl;
            int wl = Convert.ToInt32(DgvTools.GetEqualValue(cookie, "WL=", ";"));
            return wl;
        }
        public string FenxiCookie(string cookie)
        {
            string ret = "jyean=" + DgvTools.GetEqualValue(cookie, "jyean=", ";") + ";";
            return ret;
        }
        //public static string PostHtml(string url, ref string strCookies)
        //{
        //    PostSubmitter post = new PostSubmitter();
        //    System.Net.ServicePointManager.Expect100Continue = false;
        //    post.Url = url;
        //    post.Type = PostSubmitter.PostTypeEnum.Post;
        //    // 加入cookies，必须是这么写，一次性添加是不正确的。
        //    post.strCookies = strCookies;
        //    string ret = post.Post();
        //    strCookies = post.strCookies;
        //    return ret;
        //}
        public static void DownLoadFile(String url, String FileName, bool cookie = false) //,Form1 fm = null
        {
            try
            {
                if (cookie)
                {
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);

                    myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                    myReq.Accept = "*/*";
                    myReq.KeepAlive = true;
                    myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                    myReq.Headers.Add("Cookie",ToolsWeb.CWeb.web.OutCookies  );

                    myReq.Timeout = 3000;
                    WebResponse wr = (HttpWebResponse)myReq.GetResponse();
                    Stream httpStream = wr.GetResponseStream();
                    httpStream = DealStreamZip(wr, httpStream);

                    if (wr.Headers.AllKeys.Contains("Content-Disposition"))
                    {
                        string savename = wr.Headers["Content-Disposition"];
                        savename = System.Web.HttpUtility.UrlDecode(savename);
                        BETag be = new BETag("[filename=\"-\"]");

                        savename = be.BEPos(savename).String;
                        FileName = FileName + savename;
                    }
                    else
                    {
                        FileName = FileName + ".flv";
                    }
                    FileStream outputStream = new FileStream(FileName, FileMode.Create);
                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[bufferSize];
                    readCount = httpStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);

                        readCount = httpStream.Read(buffer, 0, bufferSize);
                    }
                    httpStream.Close();
                    outputStream.Close();

                    //if (fm != null && fm._TestRun)
                    //{                      
                    //    fm.AppendDebugMsg("download-Newfilename", FileName);
                    //}
                }
                else
                {
                    FileStream outputStream = new FileStream(FileName, FileMode.Create);
                    WebRequest request = WebRequest.Create(url);
                    request.Timeout = 3000;
                    WebResponse wr = (HttpWebResponse)request.GetResponse();
                    Stream httpStream = wr.GetResponseStream();
                    httpStream = DealStreamZip(wr, httpStream);

                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[bufferSize];
                    readCount = httpStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);

                        readCount = httpStream.Read(buffer, 0, bufferSize);
                    }
                    httpStream.Close();
                    outputStream.Close();
                }
            }
            catch (Exception ex)
            {
                FileStream outputStream = new FileStream(Application.StartupPath + @"\downloaderror.log", FileMode.Append);
                StreamWriter sw = new StreamWriter(outputStream);
                String s = url + "\t" + FileName + "\t 文件下载失败错误为" + ex.Message.ToString() + "\r\n";
                sw.Write(s);
                sw.Close();
                outputStream.Close();
                //return "";
                //MessageBox.Show("文件下载失败错误为" + ex.Message.ToString(), "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public static Stream DealStreamZip(WebResponse wr, Stream httpStream)
        {
            if (wr.Headers["Content-Encoding"] == "gzip")//gzip解压处理
            {
                MemoryStream msTemp = new MemoryStream();
                GZipStream gzs = new GZipStream(httpStream, CompressionMode.Decompress);
                byte[] buf = new byte[1024];
                int len;
                while ((len = gzs.Read(buf, 0, buf.Length)) > 0)
                {
                    msTemp.Write(buf, 0, len);
                }
                msTemp.Position = 0;
                httpStream = msTemp;
            }
            else if (wr.Headers["Content-Encoding"] == "deflate")//gzip解压处理
            {
                MemoryStream msTemp = new MemoryStream();
                DeflateStream gzs = new DeflateStream(httpStream, CompressionMode.Decompress);
                byte[] buf = new byte[1024];
                int len;
                while ((len = gzs.Read(buf, 0, buf.Length)) > 0)
                {
                    msTemp.Write(buf, 0, len);
                }
                msTemp.Position = 0;
                httpStream = msTemp;
            }
            return httpStream;
        }

            /// <summary>  
    /// 指定Post地址使用Get 方式获取全部字符串  
    /// </summary>  
    /// <param name="url">请求后台地址</param>  
    /// <returns></returns>  
    	public static string Post(string url)  
    	{  
	        string result = "";  
	        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);  
	        req.Method = "POST";  
	        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();  
	        Stream stream = resp.GetResponseStream();  
	        //获取内容  
	        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))  
	        {  
	            result = reader.ReadToEnd();  
	        }  
	        return result;  
    	}
        public string OutCookies { get; set; }
        public string strCookies;
        public int wl;

        private string charset;
        private BETags charsettags;

        public static CWeb web = new CWeb();
    }
    public static class Util
    {
        /// <summary>
        /// Sets the cert policy.
        /// </summary>
        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }

        /// <summary>
        /// Remotes the certificate validate.
        /// </summary>
        private static bool RemoteCertificateValidate(
           object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!
            System.Console.WriteLine("Warning, trust any certificate");
            return true;
        }
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
                    s = s.Substring(bp.E + 1);
                }
            }
            string[] items = s.Split(new string[] { "[", "]", "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 2)
            {
                Begin = items[0].Replace("@@##", "[").Replace("##@@", "]").Replace("@@", "-");
                End = items[1].Replace("@@##", "[").Replace("##@@", "]").Replace("@@", "-");
            }
            else if (items.Length == 1)
            {
                Begin = items[0].Replace("@@##", "[").Replace("##@@", "]").Replace("@@", "-");
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
       
        public Boolean OK { get; set; }

        public override string ToString()
        {            
             return "[" + Begin.Replace("-", "@@") + "-" + End.Replace("-", "@@") + "]";
        }
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
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("tags=");
            foreach (BETag bt in tags)
                sb.Append(bt.ToString());           
            return sb.ToString();
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
    public class DgvTools
    {
        public static bool ValidNumber(string ms)
        {
            if (ms == "") return false;
            if (!"-0123456789".Contains(ms[0])) return false;
            foreach (char c in ms.Substring(1))
                if (!"0123456789".Contains(c)) return false;
            return true;
        }
        public static bool ValidDoubleNumber(string ms)
        {
            if (ms == "") return false;
            if (!".-0123456789".Contains(ms[0])) return false;
            int point = 0;
            if (ms[0] == '.') point++;
            foreach (char c in ms.Substring(1))
            {
                if (!"0123456789".Contains(c)) return false;
                if (c == '.')
                {
                    point++;
                    if (point == 2) return false;
                }
            }
            return true;
        }
        public static bool ValidName(string name)
        {
            if (name == "") return false;
            foreach (char c in name.ToLower())
                if (!"0123456789abcdefghikjlmnopqrstuvwxyz-_".Contains(c))
                    return false;
            if ("0123456789-_".Contains(name[0]))
                return false;
            return true;
        }
        public static bool ValidBase64(string name)
        {
            if (name == "") return false;
            foreach (char c in name.ToLower())
                if (!"0123456789abcdefghikjlmnopqrstuvwxyz".Contains(c))
                    return false;
            return true;
        }
        public static bool ValidUrl(string name)
        {
            if (name == "") return false;
            foreach (char c in name.ToLower())
                if (!"0123456789abcdefghikjlmnopqrstuvwxyz-_/:.".Contains(c))
                    return false;
            if ("0123456789-_:.".Contains(name[0]))
                return false;
            return true;
        }
        public static string GetEqualValue(string src, string begin, string end)
        {
            if (!src.Contains(begin))
                return "";
            src = src.Substring(src.IndexOf(begin) + begin.Length) + end;
            src = src.Substring(0, src.IndexOf(end)).Trim();
            return src;
        }
    }
}
