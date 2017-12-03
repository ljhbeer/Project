using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ShowImage
{
    public class Data
    {
        private DataTable htmldt;
        private string htmltemplate;
        private string htmlreplace;
        private string itemreplace;
        private string itemtemplate;
        private string itempapertemplate;
        private string itempaperreplace;
        private string itemcustomertemplate;
        private string itemcustomerreplace;
        private string itemshowtemplate;
        private string itemshowreplace;
        public Data()
        {
            htmltemplate = "";
            htmlreplace = "";
            itemreplace = "";
            itemtemplate = "";
            itempaperreplace = "";
            itempapertemplate = "";
        }
        internal void Init(DataTable dt)
        {
            this.htmldt = dt;
            htmltemplate = GetHtmlTemplateCode("htmltemplate", "code");
            htmlreplace = GetHtmlTemplateCode("htmltemplate", "replace");
            string js = GetHtmlTemplateCode("js", "code");
            string css = GetHtmlTemplateCode("css", "code");
            string jsreplace = GetHtmlTemplateCode("js", "replace");
            string cssreplace = GetHtmlTemplateCode("css", "replace");
            htmltemplate = htmltemplate.Replace(jsreplace, js);
            htmltemplate = htmltemplate.Replace(cssreplace, css);
            itemtemplate = GetHtmlTemplateCode("itemtemplate", "code");
            itemreplace = GetHtmlTemplateCode("itemtemplate", "replace");
            itempapertemplate = GetHtmlTemplateCode("itempaper", "code");
            itempaperreplace = GetHtmlTemplateCode("itempaper", "replace");
            itemcustomertemplate = GetHtmlTemplateCode("itemcustomer", "code");
            itemcustomerreplace = GetHtmlTemplateCode("itemcustomer", "replace");
            itemshowtemplate = GetHtmlTemplateCode("itemshow", "code");
            itemshowreplace = GetHtmlTemplateCode("itemshow", "replace");
        }
        private string GetHtmlTemplateCode(string name, string colname)
        {
            DataRow[] dr = htmldt.Select(" CNAME = '" + name + "'");
            if (dr.Count() == 1)
            {
                return dr[0][colname].ToString().Trim();
            }
            return "";
        }
        internal string ConstructItems(DataTable dt)
        {
            string html = "";
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                string content = "<P><SPAN>" + i + " .</SPAN>" + dr[2].ToString().Substring(3);
                string scode = itemtemplate.Replace(itemreplace, content);
                scode = scode.Replace("<!--contentinfo1-->", "<p>" + dr[0].ToString() + " 来源：" + dr[4].ToString() + "</p>");
                html += scode.Replace("[id]", dr[1].ToString());
                if (i++ > 100)
                    break;
            }
            return htmltemplate.Replace(htmlreplace, html);
        }
        internal string ConstructItem(DataRow dr)
        {
            string html = "";  
         
            string content = "<P><SPAN>" + 1 + " .</SPAN>" + dr[2].ToString().Substring(3);
            string scode = itemshowtemplate.Replace(itemreplace, content);
            //scode = scode.Replace("<!--contentinfo1-->", "<p>" + dr[0].ToString() + " 来源：" + dr[4].ToString() + "</p>");
            html += scode.Replace("[id]", dr[1].ToString());

            return html;// template.Replace(htmlreplace, html);
        }
        internal string ConstructOutput(DataTable dt)
        {
            string html = "";
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                string content = "<P><SPAN>" + i + " .</SPAN>" + dr[2].ToString().Substring(3);
                string scode = itemshowtemplate.Replace(itemshowreplace, content);
                //scode = scode.Replace("<!--contentinfo1-->", "<p>" + dr[0].ToString() + " 来源：" + dr[4].ToString() + "</p>");
                html += scode.Replace("[id]", dr[1].ToString());
                if (i++ > 100)
                    break;
            }
            return htmltemplate.Replace(htmlreplace, html);
        }
        internal string ConstructOutputAnswer(DataTable dt)
        {
            string html = "";
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                string content = "<SPAN>" + i + " .</SPAN>" + dr["answer"].ToString();
                string scode = itemshowtemplate.Replace(itemshowreplace, content);
                //scode = scode.Replace("<!--contentinfo1-->", "<p>" + dr[0].ToString() + " 来源：" + dr[4].ToString() + "</p>");
                html += scode.Replace("[id]", dr["id"].ToString());
                if (i++ > 100)
                    break;
            }
            return html; //template.Replace(htmlreplace, html)
        }
        public  string ConstructPaperItems(DataTable dt)
        {
            string html = "";
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                string content = "<P><SPAN>" + (i++) + " .</SPAN>" + dr["cname"].ToString()
                    + "  试题数量(" + dr["cnt"].ToString() + ")题</P>"; 
                string scode = itempapertemplate.Replace(itempaperreplace, content);
                 html += scode.Replace("[id]", dr["id"].ToString());
            }
            return htmltemplate.Replace(htmlreplace, html);
        }
        public  string ConstructCustomerItems(DataTable dt)
        {
            string html = "";
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                string content = "<P><SPAN>" + (i++) + " .</SPAN>" + dr["cname"].ToString()
                    + "  试题数量(" + dr["cnt"].ToString() + ")题</P>"; 
                string scode = itemcustomertemplate.Replace(itemcustomerreplace, content);
                 html += scode.Replace("[id]", dr["id"].ToString());
            }
            return htmltemplate.Replace(htmlreplace, html);
        }
        public string ConstructPagelist(int items, DataTable dt, int maxitems)
        {
            string pagehtml = "";
            int cnt = (int)dt.Rows[0][0];           
            if (maxitems > 0)
            {
                cnt = cnt > maxitems ? maxitems : cnt;
                int maxpage = cnt / items;
                for (int pi = 0; pi < maxpage; pi++)
                {
                    pagehtml += "<a href=\"javascript:gopage(" + pi * items + "," + items + ", 1 )\"  >" + (pi + 1) + "页 </a>  \r\n";
                }   // 1 pagelist item
            }
            else
            {
                //items *= 4;
                int maxpage = cnt / items;
                for (int pi = 0; pi < maxpage; pi++)
                {
                    pagehtml += "<a href=\"javascript:gopage(" + pi * items + "," + items + ", 2 )\"  >" + (pi + 1) + "页 </a>  \r\n";
                }   // 2 paperpagelist
            }
            return pagehtml;
        }

        internal string ConstructItem(DataTable dt)
        {
            string html = "";
            this.htmldt = dt;
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                string content = "<IMG SRC=\"" + dr["url"].ToString()+ "\"  />"+ i+"_"+dr["id"].ToString()+"<br>\r\n";
                html += content;
                if (i++ > 200)
                    break;
            }

            return html;
        }
    }
}
