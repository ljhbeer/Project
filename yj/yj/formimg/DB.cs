using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace Db
{    
    public class ConnDb
    {
        public OleDbConnection conn = null;//连接数据库的对象
        //下面是构造函数连接数据库
        public ConnDb(string source)
        {
            if (conn == null)//判断连接是否为空
            {
                conn = new OleDbConnection();
                
                conn.ConnectionString = "Provider= Microsoft.Jet.OleDB.4.0;Data Source="+source;
                    //"provider=sqloledb.1;data source=.;initial catalog=capucivar;user id=sa;pwd=";//连接数据库的字符串 }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();//打开数据库连接
                }
            }
        }
        ~ConnDb( )
        {
            if (conn != null)//判断连接是否为空
            {
                if (conn.State != ConnectionState.Closed)
                {
                    //conn.Close();//打开数据库连接
                }
            }
        }
        public bool ExistRecord(string sql)
        {
            OleDbCommand oc = new OleDbCommand();//表示要对数据源执行的SQL语句或存储过程
            oc.CommandText = sql;//设置命令的文本
            oc.CommandType = CommandType.Text;//设置命令的类型
            oc.Connection = conn;//设置命令的连接
            try
            {
                object o = oc.ExecuteScalar();
                return o != null;
            }
            catch
            {
            }
            return false;
        }
        public DataSet query(string sql)
        {
            TestConnect();
            DataSet ds = new DataSet();//DataSet是表的集合
            OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);//从数据库中查询
            da.Fill(ds);//将数据填充到DataSet            
            return ds;//返回结果
        }
        public int update(string sql)
        {
            TestConnect();
            OleDbCommand oc = new OleDbCommand();//表示要对数据源执行的SQL语句或存储过程
            oc.CommandText = sql;//设置命令的文本
            oc.CommandType = CommandType.Text;//设置命令的类型
            oc.Connection = conn;//设置命令的连接            
            return oc.ExecuteNonQuery();//执行SQL语句       
        }
        public void connClose(){
            if (conn.State == ConnectionState.Open)
            {//判断数据库的连接状态，如果状态是打开的话就将它关闭
                conn.Close();
            }
        }
        internal void TestConnect()
        {
            if (conn.State == ConnectionState.Closed )
            {//判断数据库的连接状态，如果状态是打开的话就将它关闭
                conn.Open();
            }
        }
    }
}
