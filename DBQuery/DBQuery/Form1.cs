using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MetarCommonSupport;

namespace DBQuery
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            folderpath = "";
            runclass = false;
            filelist = new List<string>();
            dbsqlfullname = "data\\sql.mdb";
            dbdatafullname = "data\\swtk.mdb";
            activeTableName = "";
            ReadConfig();
            dbdata = new Db.ConnDb(dbdatafullname);
            dbsql = new Db.ConnDb(dbsqlfullname);
            Refreshtable();
            RefreshAction();
            dbdata.connClose();
            dbsql.connClose();
            //Thread nonParameterThread = new Thread(new ThreadStart(RunFrame));
            //nonParameterThread.Start();
        }
        private void buttonrefreshtable_Click(object sender, EventArgs e)
        {
            Refreshtable();
            RefreshAction();
        }
        private void buttonQuery_Click(object sender, EventArgs e)
        {
            string sql = textBoxquery.Text;
            DataSet ds;
            try
            {
                dbdata.TestConnect();
                ds = dbdata.query(sql);
                dbdata.connClose();
                if (checkBoxOutput.Checked)
                {
                    string str = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        str += dr[0].ToString() + "\r\n";
                    }
                    File.WriteAllText("output.txt", str);
                    textBoxShow.Text = "执行成功 并输出到文件";
                }
                else
                {
                    dataGridViewtablestruct.DataSource = ds.Tables[0];
                    activeTableName = sql.Substring(sql.IndexOf("from")+"from".Length).Trim()+" ";
                    activeTableName = activeTableName.Substring(0, activeTableName.IndexOf(" "));
                    if (!MetarnetRegex.IsNumAndEnCh(activeTableName))
                        activeTableName = "";
                    textBoxShow.Text = "执行成功";
                }
            }
            catch (System.Data.OleDb.OleDbException ee)
            {
                textBoxShow.Text = ee.ToString();
            }
        }
        private void buttonDone_Click(object sender, EventArgs e)
        {
            string sql = textBoxquery.Text;
            string rsql = sql;
            if (checkBox1.Checked)
            {
                Regex regex = new Regex("'\\[\\[([^']*)\\]\\]'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Match match = regex.Match(rsql);//Regex.Match(input, pattern)   
                while (match.Success)
                {
                    string filename = match.Groups[1].Value;
                    string filecontent = "";
                    if (File.Exists(filename))
                        filecontent = File.ReadAllText(filename);
                    filecontent = filecontent.Replace("'", "''");
                    sql = sql.Replace("[[" + filename + "]]", filecontent);
                    match = match.NextMatch();
                }
            }
            try
            {
                dbdata.TestConnect();
                if (dbdata.update(sql) > 0)
                {
                    textBoxShow.Text = "执行成功";
                }
                dbdata.connClose();
            }
            catch (System.Data.OleDb.OleDbException ee)
            {
                textBoxShow.Text = ee.ToString();
            }
        }

        private void buttonSaveSqlCmd_Click(object sender, EventArgs e)
        {
            string sqlname = textBoxSQLname.Text;
            if (sqlname == "")
            {
                textBoxShow.Text = "请在SQLName栏内填上地址";
                return;
            }
            string sqlcontent = textBoxquery.Text;
            if (sqlcontent == "")
                sqlcontent = textBoxquery.Text;
            if (sqlcontent == "")
            {
                textBoxShow.Text = "SQL语句为空";
                return;
            }
            string sql = "insert into SQLsave(sqlname,sqlcontent) values('"
                + sqlname + "','"
                + sqlcontent + "')";
            dbsql.TestConnect();
            if (dbsql.update(sql) > 0)
            {
                textBoxShow.Text = "执行成功";
                //comboBoxSQLname.Items.Add(sqlname);
                sql = "select * from sqlsave ";
                DataSet ds1 = dbsql.query(sql);
                this.comboBoxSQLname.DataSource = ds1.Tables[0];
                this.comboBoxSQLname.DisplayMember = "sqlname";
                this.comboBoxSQLname.ValueMember = "id";
                //DataTable dt = (DataTable)comboBoxSQLname.DataSource;
                //dt.Rows.Add();
            }
            else
            {
                textBoxShow.Text = "执行失败";
            }
            dbsql.connClose();
        }
        private void buttonDelSQL_Click(object sender, EventArgs e)
        {
            if (comboBoxSQLname.SelectedIndex != -1)
            {
                 string sql = "delete * from sqlsave where id= " + comboBoxSQLname.SelectedValue  ;
                 dbsql.TestConnect();
                 if (dbsql.update(sql) > 0)
                 {
                     textBoxShow.Text = "删除完成";
                     DataTable dt = (DataTable)comboBoxSQLname.DataSource;
                     dt.Rows.RemoveAt(comboBoxSQLname.SelectedIndex);
                 }
                 else
                 {
                     textBoxShow.Text = "删除失败";
                 }
                 dbsql.connClose();
            }
        }
        private void buttonUpdateToDatabase_Click(object sender, EventArgs e)
        {
            if (activeTableName == "") return;
            if (dataGridViewtablestruct.DataSource == null) return;
            DataTable dt = (DataTable)dataGridViewtablestruct.DataSource;
            if( dt.Rows.Count == 0 || dt.Columns.Count<2) return;
            try
            {
                foreach(DataRow dr in dt.Rows){
                    if (dr.RowState == DataRowState.Modified){
                        string condition = " where " + dt.Columns[0].ColumnName + " = " + dr[0].ToString();
                        string update = "update [" + activeTableName + "] set "; //
                        for (int i = 1; i < dt.Columns.Count; i++)
                        {
                            if (dr.IsNull(i))
                            {
                                update += "[" + dt.Columns[i].ColumnName + "] = null ,";
                            }else  if (dt.Columns[i].DataType.Name.Contains("Int") ||
                                dt.Columns[i].DataType.Name.Contains("Single"))
                            {
                                update += "["+ dt.Columns[i].ColumnName + "] = " + dr[i].ToString() + ",";
                            }
                            else if (dt.Columns[i].DataType.Name.Contains("String"))
                            {
                                update += "["+dt.Columns[i].ColumnName + "] = '" + dr[i].ToString().Replace("'","''") + "'," ;
                            }
                            else if (dt.Columns[i].DataType.Name.Contains("DateTime"))
                            {
                                update += "["+dt.Columns[i].ColumnName + "] = '" + dr[i].ToString().Replace("'", "''") + "',";
                            }
                        }
                        update = update.Substring(0, update.Length - 1)+ condition;
                        dbdata.update(update  );
                        dr.AcceptChanges();
                    }
                    else if (dr.RowState == DataRowState.Added)
                    {
                        string update = "insert into [" + activeTableName + "]("; //
                        for (int i = 1; i < dt.Columns.Count; i++)
                        {
                            if (dr.IsNull(i))
                            {
                                ;
                            }
                            else
                            {
                                update += "[" + dt.Columns[i].ColumnName + "],";
                            }
                        }
                        update = (update + ")").Replace(",)", ") ") + "values(";
                        for (int i = 1; i < dt.Columns.Count; i++)
                        {
                            if (dr.IsNull(i))
                            {
                                ;
                            }
                            else
                            if (dt.Columns[i].DataType.Name.Contains("Int") ||
                                dt.Columns[i].DataType.Name.Contains("Single"))
                            {
                                update += dr[i].ToString() + ",";
                            }
                            else if (dt.Columns[i].DataType.Name.Contains("String"))
                            {
                                update +=" '" + dr[i].ToString().Replace("'", "''") + "',";
                            }
                            else if (dt.Columns[i].DataType.Name.Contains("DateTime"))
                            {
                                update +=" '" + dr[i].ToString().Replace("'", "''") + "',";
                            }
                        }
                        update = (update + ")").Replace(",)", ") ");
                        dbdata.update(update);
                    }
                }
            }catch
            {
                showmsg("更新失败");
            }
        }
       
        private void buttonsaveupdateaction_Click(object sender, EventArgs e)
        {
            string cname = textBoxSQLname.Text;
            string sqlcontent = textBoxquery.Text;
            string actiontype = "update";
            string result = "none";
            string updatereplace = "F";
            if (checkBox1.Checked)
                updatereplace = "T";
            SaveAction(cname, sqlcontent, actiontype, result, updatereplace);
        }
        private void buttonsaveselectsingleaction_Click(object sender, EventArgs e)
        {
            string cname = textBoxSQLname.Text;
            string sqlcontent = textBoxquery.Text;
            string actiontype = "select";
            string result = "single";
            string updatereplace = "F";
            if (checkBox1.Checked)
                updatereplace = "T";
            SaveAction(cname, sqlcontent, actiontype, result, updatereplace);
        }
        private void buttonsaveselectmultiaction_Click(object sender, EventArgs e)
        {
            string cname = textBoxSQLname.Text;
            string sqlcontent = textBoxquery.Text;
            string actiontype = "select";
            string result = "multi";
            string updatereplace = "F";
            if (checkBox1.Checked)
                updatereplace = "T";
            SaveAction(cname, sqlcontent, actiontype, result, updatereplace);
        }
        private void buttonImportImgToFile_Click(object sender, EventArgs e)
        {
            string sql = textBoxquery.Text;
            if (!sql.Contains("url") || !sql.Contains("pid"))
            {
                this.showmsg("数据库的表中应含有 url 和 pid ");
                return;
            }
            DataSet ds;
            try
            {
                ds = dbdata.query(sql);
                string oldpid = ds.Tables[0].Rows[0][1].ToString();
                string str = oldpid + ".html";
                File.WriteAllText("output.txt", "");
                File.WriteAllText("show.txt", "");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (oldpid == dr["pid"].ToString())
                    {
                        str += "<>" + dr["url"].ToString();
                    }
                    else
                    {
                        File.AppendAllText("output.txt", str);
                        File.AppendAllText("show.txt", "<a href=\"\" onclick=\"showimg(this);return false;\">" + oldpid + "</a>\r\n");
                        oldpid = dr[1].ToString();
                        str = "<>" + oldpid + ".html<>" + dr["url"].ToString();
                    }
                }
                File.AppendAllText("output.txt", str);
                textBoxShow.Text = "执行成功 并输出到文件";

            }
            catch (System.Data.OleDb.OleDbException ee)
            {
                textBoxShow.Text = ee.ToString();
            }
        }
        private void buttonSetSQLDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.dbsqlfullname = fd.FileName;
                this.textBoxShow.Text = "当前SQL数据库：" + dbdatafullname;
                if (dbsql != null)
                {
                    dbsql.connClose();
                    dbsql = null;
                }
                dbsql = new Db.ConnDb(dbdatafullname);
                RefreshAction();
                dbsql.connClose();
            }
        }
        private void buttonSetDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.dbdatafullname = fd.FileName;
                this.textBoxShow.Text = "当前数据数据库：" + dbdatafullname;
                if (dbdata != null)
                {
                    dbdata.connClose();
                    dbdata = null;
                }
                dbdata = new Db.ConnDb(dbdatafullname);
                Refreshtable();
                //RefreshAction();
                dbdata.connClose();
            }
        }
        private void buttonbrows_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.folderpath = folderBrowserDialog1.SelectedPath;
                this.textBoxShow.Text = "当前路径：" + folderpath;
               // this.textBoxPath.Text = folderpath;
            }
        }       
        private void buttonImportTest_Click(object sender, EventArgs e)
        {
            if (runclass == true)
            {
                this.textBoxShow.Text = "当前线程正在执行，本操作无效";
                return;
            }
            runclass = true;
            Thread nonParameterThread = new Thread(new ThreadStart(RunUpdateSource1));
            nonParameterThread.Start();
        }
        private void btnimport_Click(object sender, EventArgs e)
        {
            showmsg("按键说明：");

        }
        private void btnimport_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.O)
            {
                List<string> tablesname = new List<string>();              
                foreach (Object o in comboBoxtablename.Items)
                    tablesname.Add(o.ToString());
                if (tablesname.Count == 0)
                    return;
                string newdb = GetBakeName(this.dbdatafullname);
                string strpath = this.GetType().Assembly.Location;
                strpath = strpath.Substring(0, strpath.LastIndexOf("\\"));
                if (!File.Exists(strpath + "\\Data\\Database1.mdb"))
                    return;
                File.Copy(strpath + "\\Data\\Database1.mdb", newdb);
                FormShow fs = new FormShow(tablesname);
                if (fs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    List<bool> ret = fs.GetResult();                   
                    for (int i = 0; i < ret.Count; i++)
                    {
                        if (ret[i])
                        {
                            string sql = "select * into [;DATABASE=" + newdb + "].[" + tablesname[i] + "]  from  ["
                                        + tablesname[i]+"]";
                            dbdata.update(sql);
                        }
                        else
                        {
                            string sql = "select top 1 * into [;DATABASE=" + newdb + "].[" + tablesname[i] + "] from ["
                                        + tablesname[i]+"]";
                            dbdata.update(sql);
                            sql = "delete * from [;DATABASE=" + newdb + "]." + tablesname[i];
                            dbdata.update(sql);
                        }
                    }
                    dbdata.connClose();
                }
            }
            if (e.KeyCode == Keys.Q)
            {
                string sql = textBoxquery.Text;
                DataSet ds;
                try
                {
                    dbdata.TestConnect();
                    ds = dbdata.query(sql);
                    dbdata.connClose();

                    //dataGridViewtablestruct.DataSource = ds.Tables[0];
                    activeTableName = sql.Substring(sql.IndexOf("from") + "from".Length).Trim() + " ";
                    activeTableName = activeTableName.Substring(0, activeTableName.IndexOf(" "));
                    if (!MetarnetRegex.IsNumAndEnCh(activeTableName))
                        activeTableName = "";
                    //textBoxShow.Text = "执行成功";
                    //                        
                    string str = AllRecord(ds.Tables[0]);
                    File.WriteAllText(activeTableName + ".txt", str);
                    textBoxShow.Text = "执行成功 并输出到文件";

                }
                catch (System.Data.OleDb.OleDbException ee)
                {
                    textBoxShow.Text = ee.ToString();
                }
            }
            else if (e.KeyCode == Keys.A)
            {
               
            }
            else if (e.KeyCode == Keys.O)
            {
                
            }
        }

        private string AllRecord(DataTable dt)
        {
            string str = "";
            foreach (DataRow dr in dt.Rows)
            {
                string drstr = "{";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dr.IsNull(i))
                    {
                        drstr += "null,";
                    }
                    else if (dt.Columns[i].DataType.Name.Contains("Int") ||
                      dt.Columns[i].DataType.Name.Contains("Single"))
                    {
                        drstr +=dr[i].ToString() + ",";
                    }
                    else if (dt.Columns[i].DataType.Name.Contains("String"))
                    {
                        drstr += "'" + dr[i].ToString().Replace("'", "''") + "',";
                    }
                    else if (dt.Columns[i].DataType.Name.Contains("DateTime"))
                    {
                        drstr += "'" + dr[i].ToString().Replace("'", "''") + "',";
                    }
                }
                drstr = (drstr + "}").Replace(",}", "}");
                str += drstr  + "\r\n";
            }
            return str;
        }

        private string GetBakeName(string p)
        {
            string name = p.Substring(0,p.IndexOf(".mdb"));
            string num = "";
            int inum = -1;
            if (name.Contains("_bak"))
            {
                num = name.Substring(name.LastIndexOf("_bak")+4) + "0";
                if (MetarCommonSupport.MetarnetRegex.IsNumber(num))
                {
                    inum = Convert.ToInt32(num)/10 + 1;
                    name = name.Substring(0, name.LastIndexOf("_bak")+4);
                    while (true)
                    {
                        if (File.Exists(name + inum + ".mdb"))
                            inum++;
                        else
                            return name + inum + ".mdb";
                    }
                }
            }
            inum = 1;
            while (true)
            {
                if (File.Exists(name+"_bak" + inum + ".mdb"))
                    inum++;
                else
                    return name+"_bak" + inum + ".mdb";
            }
        }

        private void SaveAction(string cname, string sqlcontent, string actiontype, string result, string updatereplace)
        {
            if (cname == "" || cname == null)
            {
                this.showmsg("动作名字不能为空");
                return;
            }
            if (cname.Trim().ToLower().StartsWith("r"))
                cname = "_" + cname.Trim();
            string sql = "insert into taction(cname,sqlcontent,acttype,result,updatereplace) values('" +
                            cname + "','" + sqlcontent  + "','" + actiontype  + "','" + result  + "','" + updatereplace + "')";
            string existsql = "select 1 from [taction] where cname = '" + cname + "' "; ;
            try
            {
                dbsql.TestConnect();
                if (!dbsql.ExistRecord(existsql))
                {  //RunImport
                    if (dbsql.update(sql) > 0)
                    {
                        textBoxShow.Text = "插入成功";
                        RefreshAction( );
                    }
                }
                else
                {
                    this.showmsg("存在相同名字");
                }
                dbsql.connClose();
            }
            catch (System.Data.OleDb.OleDbException ee)
            {
                textBoxShow.Text = ee.ToString();
            }
        }
        private void listBoxaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxaction.SelectedIndex != -1)
            {
                showmsg(((DataRowView)listBoxaction.SelectedItem).Row[2].ToString() );
            }
        }
        private void listBoxaction_KeyUp(object sender, KeyEventArgs e)
        {
            if (listBoxaction.SelectedIndex != -1)
            {
                if (e.KeyCode == Keys.R)
                {
                    DataRow dr = ((DataRowView)listBoxaction.SelectedItem).Row;
                    string cname = dr[1].ToString();
                    string sqlcontent = dr[2].ToString();
                    string actiontype =dr[3].ToString();
                    string result = dr[4].ToString();
                    string updatereplace = dr[5].ToString();
                    try
                    {
                        dbdata.TestConnect();
                        if (actiontype == "update")
                        {
                            if (updatereplace == "T")
                            {
                                string rsql = sqlcontent;
                                Regex regex = new Regex("'\\[\\[([^']*)\\]\\]'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                                Match match = regex.Match(rsql);//Regex.Match(input, pattern)   
                                while (match.Success)
                                {
                                    string filename = match.Groups[1].Value;
                                    string filecontent = "";
                                    if (File.Exists(filename))
                                        filecontent = File.ReadAllText(filename);
                                    filecontent = filecontent.Replace("'", "''");
                                    sqlcontent = sqlcontent.Replace("[[" + filename + "]]", filecontent);
                                    match = match.NextMatch();
                                }
                            }
                            dbdata.update(sqlcontent);
                            this.showmsg("更新成功" );
                        }
                        else if (actiontype == "select")
                        {
                            DataSet ds = dbdata.query(sqlcontent);
                            if (result == "single")
                            {
                                this.showmsg(ds.Tables[0].Rows[0][0].ToString());
                            }
                            else
                            {
                                this.dataGridViewtablestruct.DataSource = ds.Tables[0];
                                string sql = sqlcontent;
                                activeTableName = sql.Substring(sql.IndexOf("from")).Trim() + " ";
                                activeTableName = activeTableName.Substring(0, activeTableName.IndexOf(" "));
                                if (!MetarnetRegex.IsNumAndEnCh(activeTableName))
                                    activeTableName = "";
                                this.showmsg("查询成功");
                            }

                        }
                        dbdata.connClose();
                    }
                    catch (System.Data.OleDb.OleDbException ee)
                    {
                        textBoxShow.Text = ee.ToString();
                    }        
                }
                else if (e.KeyCode == Keys.D)
                {
                    string sql = "delete  from taction where id = " + listBoxaction.SelectedValue;
                    try
                    {
                        dbsql.TestConnect();
                        if (dbsql.update(sql) > 0)
                        {
                            DataTable dt = (DataTable)listBoxaction.DataSource;
                            dt.Rows.RemoveAt(listBoxaction.SelectedIndex); //.Remove(listBoxaction.SelectedItem);//
                        }
                        dbsql.connClose();
                    }
                    catch (System.Data.OleDb.OleDbException ee)
                    {
                        textBoxShow.Text =   ee.ToString();
                    }        
                }
            }
        }
        private void comboBoxtablename_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxtablename.SelectedIndex != -1)
            {
                string sql = "select top 1 * from [" + comboBoxtablename.Text + "]";
                if(checkBoxShowTopRc100.Checked)
                    sql = "select top 100 * from [" + comboBoxtablename.Text + "]";
                DataSet ds = dbdata.query(sql);
                if(checkBoxShowTopRc100.Checked)
                   dataGridViewtablestruct.DataSource = ds.Tables[0];
                string text = "";
                foreach(DataColumn dc in ds.Tables[0].Columns){
                    text += dc.Caption + "  " + dc.DataType + "\r\n";
                }
                textBoxtable.Text = text;
                //==========
                sql = "select count(*) from [" + comboBoxtablename.Text + "]";
                ds = dbdata.query(sql);
                showmsg("表[" + comboBoxtablename.Text + "] 共有 " + ds.Tables[0].Rows[0][0] + " 条记录");
                activeTableName = comboBoxtablename.Text;
            }
        }
        private void comboBoxSQLname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSQLname.SelectedIndex != -1)
            {
                DataRow dr = ((DataRowView)(comboBoxSQLname.SelectedItem)).Row;
                textBoxquery.Text = dr["sqlcontent"].ToString();
            }
        }   
        private void FenxiOther(string s, QuestionItem q)
        {
            //难度
            MatchCollection mc = Regex.Matches(s, "(sts.gif|nsts.gif)");
            if (mc.Count == 5)
            {
                int nd = 0;
                foreach (Match m in mc)
                {
                    if (m.Groups[1].Value == "nsts.gif")
                        break;
                    nd++;
                }
                q.nd = nd;
            }
            //使用次数
            Match m1 = Regex.Match(s, "使用次数：(\\d+)");
            if (m1.Success)
            {
                q.usecnt = int.Parse(m1.Groups[1].Value);
            }
            //入库时间
            m1 = Regex.Match(s, "入库时间：(\\d+-\\d+-\\d+)");
            if (m1.Success)
            {
                q.date = m1.Groups[1].Value;
            }
            //ID
            m1 = Regex.Match(s, "test-(\\d+)");
            if (m1.Success)
            {
                q.id = m1.Groups[1].Value;
            }
            //来源
            m1 = Regex.Match(s, "来源： <SPAN[^<>]*>([^<>]*)</SPAN>");
            if (m1.Success)
            {
                q.source = m1.Groups[1].Value;
            }
            //题型
            m1 = Regex.Match(s, "题型：<SPAN[^<>]*>([^<>]*)</SPAN>");
            if (m1.Success)
            {
                q.type = m1.Groups[1].Value.Replace("&nbsp;", "");
            } //知识的
            m1 = Regex.Match(s, "知识点：<SPAN[^<>]*>([^<>]*)</SPAN>");
            if (m1.Success)
            {
                q.group = m1.Groups[1].Value;
            }
        }
        private bool FenxiContent(string text, QuestionItem q)
        {
            List<int> matchpos = new List<int>();
            if (!CountDivPos(text, matchpos))
                return false;
            if (!(matchpos.Count == 8 || matchpos.Count == 6 || matchpos.Count == 10))
                return false;
            string content = "";
            int maxlen = 0;
            int maxpos = 0;
            for (int i = 1; i < matchpos.Count - 1; i += 2)
            {
                if (maxlen < matchpos[i + 1] - matchpos[i])
                {
                    maxlen = matchpos[i + 1] - matchpos[i];
                    maxpos = matchpos[i];
                }
            }
            if (maxlen > 20)
            {
                content = text.Substring(maxpos, maxlen);
            }
            else if (maxlen < 5)
            {
                int bpos = 0, epos = 0;
                GetBetween(text, ref bpos, ref epos);
                //List<string> ls = new List<string>();
                //for (int i = 0; i < matchpos.Count; i += 2)
                //{
                //    ls.Add(text.Substring(matchpos[i], matchpos[i + 1] - matchpos[i])); //
                //}   
                if (epos >= bpos && bpos > 10)
                {
                    for (int i = 1; i < matchpos.Count; i++)
                    {
                        if (matchpos[i] > epos && bpos > matchpos[i - 1])
                        {
                            content = text.Substring(matchpos[i - 1], matchpos[i] - matchpos[i - 1]);
                        }
                    }
                }
            }
            q.content = content;
            q.txtcontent = Regex.Replace(content, "<[^<>]*>|&nbsp;", "");
            return true;
        }
        private void GetBetween(string text,ref int bpos,ref int epos)
        {
            MatchCollection mc = Regex.Matches(text, "</p>|<p>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (mc.Count >0 )
            {
                bpos = mc[0].Groups[0].Index;
                epos = mc[mc.Count - 1].Groups[0].Index;
            }
        }
        private bool CountDivPos(string text, List<int> matchpos)
        {
            Stack<int> s = new Stack<int>();
            Regex regex = new Regex("(<DIV[^<>]*>|</DIV>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match match = regex.Match(text);//Regex.Match(input, pattern);
            while (match.Success)
            {
                string matchtext = match.Groups[1].Value;
                if (matchtext.EndsWith("/>"))
                {
                    if (s.Count == 0)
                    {
                        matchpos.Add(match.Groups[1].Index);
                        matchpos.Add(match.Groups[1].Index + match.Groups[1].Length);
                    }
                }
                else if (matchtext.EndsWith("</DIV>"))
                {
                    if (s.Count == 0)
                    {
                        string id = "0";
                        Match m1 = Regex.Match(text, "test-(\\d+)");
                        if (m1.Success)
                        {
                            id = m1.Groups[1].Value;
                        }
                       // File.AppendAllText("D:\\log.txt", file + "\t" + id + "Count=0" + "\r\n");
                        return false;
                    }
                    else if (s.Count == 1)
                    {
                        matchpos.Add(s.Pop());
                        matchpos.Add(match.Groups[1].Index + match.Groups[1].Length);
                    }
                    else
                    {
                        s.Pop();
                    }
                }
                else
                {
                    s.Push(match.Groups[1].Index);
                }
                match = match.NextMatch();
            }
            return true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                showmsg("执行语句中含有文件替换，格式为'[[D:\\a.txt]]'的形式");
            }
            else
            {
                showmsg("执行语句中不含有文件替换");
            }
        }

        public void showmsg(string msg)
        {
            this.textBoxShow.Text = msg;
           
        }
        private void Refreshtable()
        {
            try
            {
                string sql = "select [name] from [msysobjects] where type=1 and flags = 0 order by [name]";
                DataSet ds = dbdata.query(sql);
                comboBoxtablename.Items.Clear();
                foreach (DataRow drc in ds.Tables[0].Rows)
                {
                    comboBoxtablename.Items.Add(drc[0].ToString());
                }

                sql = "select * from sqlsave ";
                DataSet ds1 = dbsql.query(sql);
                this.comboBoxSQLname.DataSource = ds1.Tables[0];
                this.comboBoxSQLname.DisplayMember = "sqlname";
                this.comboBoxSQLname.ValueMember = "id";

                showmsg("当前数据库：" + dbdatafullname);
            }
            catch (System.Data.OleDb.OleDbException ee)
            {
                textBoxShow.Text = dbdatafullname + ee.ToString();
            }
        }
        private void RefreshAction()
        {
            string sql = "select * from taction ";
            try
            {
                if (dbsql == null)
                    dbsql = new Db.ConnDb(dbdatafullname);
                dbsql.TestConnect();
                DataSet ds2 = dbsql.query(sql);
                if (ds2.Tables.Count != 0)
                {
                    this.listBoxaction.DataSource = ds2.Tables[0];
                    this.listBoxaction.DisplayMember = "cname";
                    this.listBoxaction.ValueMember = "id";
                }
            }
            catch (System.Data.OleDb.OleDbException ee)
            {
                textBoxShow.Text = dbdatafullname + ee.ToString();
            }     
        }
        private void RunFrame()
        {
            while (true)
            {
                //this.Invoke(new ShowDeleGate(showfiletxt), new object[] { "" });
                Thread.Sleep(3000);
            }
        }
        private void RunUpdateSource1()
        {
            string sql = "select ID from sw where 入库日期=''";
            DataSet ds = dbdata.query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sql = "update sw set 入库日期 = 'none' where ID = '" + dr[0].ToString() + "' ";
                dbdata.update(sql);
            }
            runclass = false;
            dbdata.connClose();
            this.Invoke(new ShowDeleGate(showmsg), new object[] { "所有资料已导入" });

        }
        private void RunUpdateSource2()
        {
            string sql = "select ID,题干 from sw where 来源=''";
            DataSet ds = dbdata.query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string txtcontent = Regex.Replace(dr[1].ToString(), "<[^<>]*>|&nbsp;", "");
                sql = "update sw set 内容预览 = '" + txtcontent.Replace("'", "''")
                       + "' where  ID = '" + dr[0].ToString() + "' ";
                dbdata.update(sql);
            }
            runclass = false;
            dbdata.connClose();
            this.Invoke(new ShowDeleGate(showmsg), new object[] { "所有资料已导入" });
        }
        private void RunUpdateSource()
        {
            string sql = "select ID,内容预览 from sw where 来源=''";
            DataSet ds = dbdata.query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //filelist.Add(dr[0].ToString());
                string str = dr[1].ToString().Trim();
                int bpos = str.IndexOf("（");
                int epos = str.IndexOf("）");
                if (bpos >= 0 && epos > bpos && bpos < 5 && epos - bpos > 8)
                {
                    string ly = str.Substring(bpos + 1, epos - bpos - 1).Trim();
                    if (ly.Length == 0 || ly.Contains(")") || ly.Contains("分")) continue;
                    if (ly[0] >= '0' && ly[0] <= '9')
                    {
                        //File.AppendAllText("D:\\lyuan.log", dr[0].ToString() + "\t" + ly + "\r\n");
                        string txtcontent = str.Substring(epos + 1);
                        sql = "update sw set 来源 = '"
                              + ly + "' ,  内容预览 = '"
                                + txtcontent.Replace("'", "''") + "' where  ID = '" + dr[0].ToString() + "' ";//limit 1      
                        dbdata.update(sql);
                    }
                }
            }
            //foreach (string file in filelist)
            //{
            //    this.Invoke(new ShowDeleGate(showfiletxt), new object[] { file });
            //    RunFile();
            //}
            runclass = false;
            dbdata.connClose();
            this.Invoke(new ShowDeleGate(showmsg), new object[] { "所有资料已导入" });
        }
        private bool ReadConfig()
        {
            dbsqlfullname = "D:\\Backup\\sql.mdb";
            dbdatafullname = "D:\\back\\swtk.mdb";

            if (File.Exists("cfg.ini"))
            {
                string address = File.ReadAllText("cfg.ini");
                if (!address.Contains("\r\n"))
                    address += "\r\n";

                if (address.Contains("dbsqlfullname="))
                {
                    dbsqlfullname = address.Substring(address.IndexOf("dbsqlfullname=") + "dbsqlfullname=".Length) + "\r\n";
                    dbsqlfullname = dbsqlfullname.Substring(0, dbsqlfullname.IndexOf("\r\n")).Trim();
                }
                if (address.Contains("dbdatafullname="))
                {
                    dbdatafullname = address.Substring(address.IndexOf("dbdatafullname=") + "dbdatafullname=".Length) + "\r\n";
                    dbdatafullname = dbdatafullname.Substring(0, dbdatafullname.IndexOf("\r\n")).Trim();
                }
                //check
                //if (!(dbsqlfullname=="" || File.Exists(dbsqlfullname)
                //    && (dbdatafullname == "" || MetarnetRegex.IsEnglisCh(dbdatafullname))))
                //    return false;
                return true;
            }
            return false;
        }
        
        private Db.ConnDb dbdata;
        private Db.ConnDb dbsql;
        private string dbdatafullname;
        private string dbsqlfullname;
        private string folderpath;
        private bool runclass;
        private List<string> filelist;
        private string activeTableName;
        public delegate void ShowDeleGate(string file);

        private void buttonFormData_Click(object sender, EventArgs e)
        {
            if (dbdata != null)
            {
                try
                {
                    dbdata.TestConnect();
                    FormData f = new FormData(dbdata);
                    f.ShowDialog();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }

        

       
    }
}
