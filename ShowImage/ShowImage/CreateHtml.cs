using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShowImage
{
    public class CreateHtml
    {
        private string Template;
        private string hreftemplate;
        public CreateHtml()
        {
            Init();
        }
        internal bool createhtml()
        {
            if (!File.Exists("out.html"))
                return false;
            StringBuilder hrefreplace = new StringBuilder();
            StringBuilder imgurlsreplace = new StringBuilder();
            int sum = 1, iXH = 0;
            string line, img;
            System.IO.StreamReader file = new System.IO.StreamReader("out.html");
            imgurlsreplace.Append("1.html<>");
            hrefreplace.AppendLine(hreftemplate.Replace("<number>", sum.ToString()));
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("src"))
                {
                    img = GetIniString(line, "src=\"", "\"") + "<>";
                    imgurlsreplace.Append(img);
                }
                else if (line.Contains("<nexturl>"))
                {
                    sum++;
                    hrefreplace.AppendLine(hreftemplate.Replace("<number>", sum.ToString()));
                    imgurlsreplace.Append(sum.ToString() + ".html<>");
                }
                iXH++;//计数,总共几行
            }
            file.Close();//关闭文件读取流

            string outstr = Template.Replace("<hrefreplace>", hrefreplace.ToString())
                                    .Replace("<imgurlsreplace>", imgurlsreplace.ToString());
            File.WriteAllText("outimgs.html", outstr);
            return true;
        }
        private string GetIniString(string src, string b, string e)
        {
            string dst = src.Substring(src.IndexOf(b) + b.Length) + e;
            dst = dst.Substring(0, dst.IndexOf(e)).Trim();
            return dst;
        }
        private void Init()
        {
            this.hreftemplate = "<a href=\"\" onclick=\"showimg(this);return false;\"><number></a>";

            this.Template = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
<title>无标题文档</title>


</head>

<body>
<div id=""divinfor""><br> 目录 </br></div>
<div id=""divref"">
<hrefreplace>


</div>
<div id=""divnav""></div>
<div id=""divimg""></div>
<div id=""divnave""></div>
</div>
</body>
</html>
<script type=""text/javascript"">
	function showdiv() {
		document.getElementById(""bg"").style.display = ""block"";             
		document.getElementById(""show"").style.display = ""none""; 
	}
	function hidediv() {            
		document.getElementById(""bg"").style.display = 'none';
		document.getElementById(""show"").style.display = 'none';
	}
	window.onload=function(){
    var divnav=document.getElementById('divnav');
    var divref=document.getElementById('divref');
    var divimg=document.getElementById('divimg');
	}
  
	function showimg(obj){
		var divimg=document.getElementById('divimg');
		var divnav=document.getElementById('divnav');
		var divref=document.getElementById('divref');
		
    String: num = obj.innerHTML;
    num = Trim(num);
    document.getElementById('divinfor').innerHTML = num;
    if(num==""目录""){
    	divref.style.display = ""block"";  
    	divnav.innerHTML = """";  
    	divimg.innerHTML = """";   	
    	
    }else if(num==""首页""){
    	displayimg(0);
    }else if(num==""上一组""){
    	String: str = getfilestring();
    	var spos = obj.href.substr(obj.href.lastIndexOf(""/"")+1);
    	spos = str.lastIndexOf("".html"",spos-10)+5;
    	if(spos==4) spos=-2;
    	displayimg(spos);
    }else if(num==""下一组""){  
    	String: str = getfilestring();
    	var spos = obj.href.substr(obj.href.lastIndexOf(""/"")+1);
    	spos = str.indexOf("".html"",spos)+5;     	
    	if(spos==4) spos=0;     	
    	displayimg(spos);
    }else if(num==""尾页""){
    	displayimg(-2);
    }else{    
	    String: str = getfilestring();
	    var bstr = obj.innerHTML;
	    var spos = str.indexOf(bstr+"".html"",0) + bstr.length + 5;	  
	    displayimg(spos);
		}
	}
	function displayimg(spos){
		  var divinfor = document.getElementById('divinfor');
		  var divimg=document.getElementById('divimg');
		  var divnav=document.getElementById('divnav');
	    var divref=document.getElementById('divref');
	   	divref.style.display = ""none"";  
	   	//showimg	
			//String: num = obj.innerHTML;
			String: str = getfilestring();
			if(spos==-2){
				spos = str.lastIndexOf("".html"",str.length);
				spos = str.lastIndexOf("".html"",spos-4) + 5;
			}else if( spos<0){
				spos=0;
			}			
			if(spos==0){
			   spos = 	str.indexOf("".html"",0)+4;
		  }
		  //shownav 
	   	divnav.innerHTML = ""<br><a href=\""\"" onclick=\""showimg(this);return false;\"">目录</a> <a   href=\""\"" onclick=\""showimg(this);return false;\"">首页</a> <a href=\""""+ spos +""\"" onclick=\""showimg(this);return false;\"">上一组</a>  <a href=\""""+ spos +""\""  onclick=\""showimg(this);return false;\"">下一组</a> <a href=\""\"" onclick=\""showimg(this);return false;\"">尾页</a></br>""
		divnave.innerHTML = divnav.innerHTML;	   	
//
			var epos = str.indexOf("".html"",spos);
			str=str.substring(spos,epos);	
			divinfor.innerHTML += "" Pos:"" + spos;
			var newspos = str.indexOf(""<"",0);
			var newepos = str.lastIndexOf("">"",str.length)+1;
			str=str.substring(newspos,newepos);
			//divimg.innerHTML = str
			ch = str.split(""<>"");
			divimg.innerHTML = """"
			//alert(num + str + spos);
			for(i=0;i<ch.length;i++){
				var e = document.createElement(""div"");
				e.innerHTML = ""<img src=\""""+ ch[i] + ""\"">"" + i + ""</img><br>"" ;
			  divimg.appendChild(e);
			}
	}
	
	//此处为独立函数
	function LTrim(str)
	{
	    var i;
	    for(i=0;i<str.length;i++)
	    {
	        if(str.charAt(i)!="" ""&&str.charAt(i)!="" "")break;
	    }
	    str=str.substring(i,str.length);
	    return str;
	}
	function RTrim(str)
	{
	    var i;
	    for(i=str.length-1;i>=0;i--)
	    {
	        if(str.charAt(i)!="" ""&&str.charAt(i)!="" "")break;
	    }
	    str=str.substring(0,i+1);
	    return str;
	}
	function Trim(str)
	{
	    return LTrim(RTrim(str));
	}
        function getfilestring(){  	        
		return ""<imgurlsreplace>"";
        }
</script>
";
        }

        /////////////////////////
        private static bool UrlExistsUsingHttpWebRequest(string url)
        {
            try
            {
                System.Net.HttpWebRequest myRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                myRequest.Method = "HEAD";
                myRequest.Timeout = 200;
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)myRequest.GetResponse();
                return (res.StatusCode == System.Net.HttpStatusCode.OK);
            }
            catch (System.Net.WebException we)
            {
                System.Diagnostics.Trace.Write(we.Message);
                return false;
            }
        }

        /////////////////
        private bool UrlIsExist(String url)
        {
            System.Uri u = null;
            try
            {
                u = new Uri(url);
            }
            catch { return false; }
            bool isExist = false;
            System.Net.HttpWebRequest r = System.Net.HttpWebRequest.Create(u)
                                    as System.Net.HttpWebRequest;
            r.Method = "HEAD";
            try
            {
                System.Net.HttpWebResponse s = r.GetResponse() as System.Net.HttpWebResponse;
                if (s.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    isExist = true;
                }
            }
            catch (System.Net.WebException x)
            {
                try
                {
                    isExist = ((x.Response as System.Net.HttpWebResponse).StatusCode !=
                                 System.Net.HttpStatusCode.NotFound);
                }
                catch { isExist = (x.Status == System.Net.WebExceptionStatus.Success); }
            }
            return isExist;
        }

        /////////

    }
}
