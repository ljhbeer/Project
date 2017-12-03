using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBQuery
{
    class QuestionItem
    {
        public string id;
        public int nd;
        public int usecnt;
        public string date;
        public string type;
        public string group;
        public string source;
        public string content;
        public string txtcontent;
        public String ClearSQL()
        {
            return "delete * from sw";
        }
        public String ClearSQL1()
        {
            return "delete * from filesource";
        }
        internal string UpdateContentSQL()
        {
            return "update sw set 内容预览 = '"
                         + txtcontent.Replace("'","''")+"' ,  题干 = '"
                         + content.Replace("'", "''") + "' where  ID = '" + id + "' ";//limit 1
        }
        public String InsertSQL()
        {
            return "insert into sw(ID,难度,使用次数,入库日期,题型,知识点,来源,内容预览,题干)" +
                " values("+id+","
                         +nd+","
                         +usecnt+",'"
                         +date+"','"
                         + type + "','"
                         + group + "','"
                         +source+ "','"
                         +txtcontent.Replace("'","''")+"','"
                         + content.Replace("'","''") +
                         "')";
        }
        public String CheckSQL()
        {//,内容预览，题干，答案，选择题答案
            return "insert into sw(ID,难度) values("+id+","+nd+")";
        }

        internal string InsertFileSourceSQL(string file)
        {
           // IF NOT EXISTS(SELECT * FROM TABLE_NAME WHERE FILED1 = 1 ) THENINSERT INTO TABLE_NAME VALUES(1);
            return "  insert into filesource(ID,filename)" +
                " values(" + id + ",'"
                         + file + "')";
        }
        internal string ExistRecordSQL()
        {
            return "select 1 from sw where ID = '" + id + "' ";//limit 1
        }
        internal string ExistRecordSQL(string tablename)
        {
            return "select 1 from ["+tablename+"] where ID = '" + id + "' ";//limit 1
        }

        internal string EmptyExistRecordSQL()
        {
            return "select 1 from recempty where ID = '" + id + "' ";//limit 1
        }

    }
}
