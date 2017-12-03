using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShowImage
{
    public partial class FormReadMe : Form
    {
        public FormReadMe()
        {
            InitializeComponent();
            textBox1.Text = ReadMe();
        }
        public string ReadMe()
        {
            return @"
urltemplate=[url][idb-ide]  或者 [url][nextb-nexte][nextb-nexte].. 
定义两种模式urlid / urlne    id支持previous 和 next    ne仅仅支持next
  通过.SaveUrl来计算nexturl  id只替换  ne靠提取

执行流程
 1、Init();
 2、Next -> ShowItem() 
    a、 url = nu.Url()
    b、 url.Replace    by  nexturlreplace=
    c、 GetRightHtml() -> nu.SaveUrl() -> if then clear(out.html) // 此时计算nextUrl 
    d、 img.GetImglist()  //  if (img.NeedNewThread) RunThread() -> img.GetImgList_Thread()
 3、Img.GetImglist()
   a、save htmlpage to urls,imgs
   b、sum = ContinueIndex  // continueindex=  连接上一次编号
   c、Init- ImgBeginTag ImgEndTag
 ****  ImgMulti = false *******
  1、 imgs =  multImgBE[0, length-1]. Match(imgs);  //提取到imgs的范围
  2、 利用ImgBeginTag-ImgEndTag 提取每一条符合要求的Img
      Foreach img in imgs
	if (!img.Contains(ImgExistCondition))  continue;   //imgexistcondition=
         ""<img src=\""[img]\"">[i]</img>\r\n"".Replace(""[img]"", img).Replace(""[i]"", sum.ToString()); //保存
********************************
 ****  ImgMulti = true *******
  Do ****
	1、getnexturl  // if lastfind // nextimgurllastfind= true 或者 false
	2、imgs =  multImgBE[0, length-1]. Match(imgs);  //提取到imgs的范围 同上
	3、利用ImgBeginTag-ImgEndTag 提取每一条符合要求的Img 同上
  while(urls.Contains(ImgMultiCondition))  //nextimgcondition=
      || nexturl="""" || form1.StopThread || sum>maxitemscount 退出 // maxitemscount= 默认值为50 
********************************




nexturlreplace
nextimgurllastfind= 
replacetonull= 
imguri= 
imgcondition=
nextimgurl= 
nextimgcondition= 
nextimgurllastfind= 
replacetonull=
maxitemscount=1000
说明=""@@""代表""-""
//////////////////////////////
******************************
  本地测试 暂时未说明
txtimguri = //??????

";
        }
    }
}
