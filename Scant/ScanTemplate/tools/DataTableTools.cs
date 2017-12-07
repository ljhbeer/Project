using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace Tools
{
    public  class DataTableTools
    {
        public static DataTable ConstructDataTable(string[] columntitles)
        {
            DataTable dt = new DataTable();
            for (int count = 0; count < columntitles.Length; count++)
            {
                DataColumn dc = new DataColumn(columntitles[count]);
                if ("roomname题组名称".Contains(columntitles[count]))
                {
                    dc.DataType = typeof(string);
                    //dc.MaxLength = 60;
                }
                else if ("ID".Contains(columntitles[count]) )
                {
                    dc.DataType = typeof(int);
                }
                else if ("maxscore最大分值校验角度".Contains(columntitles[count]))
                {
                    dc.DataType = typeof(double);
                }
                else if ( columntitles[count].Contains("是否") )
                {
                    dc.DataType = typeof(bool);
                }
                else if (columntitles[count].Contains("学号")
                    || columntitles[count].Contains("OID")
                    || columntitles[count].Contains("object"))
                {
                    dc.DataType = typeof(Object);
                }
                else if ("img图片".Contains(columntitles[count]) || columntitles[count].Contains("图片"))
                {
                    dc.DataType = typeof(System.Drawing.Image);
                }else{
                	dc.DataType = typeof(string);
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }
        public static DataTable GetDgvToTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].HeaderText);
                dc.DataType = dgv.Columns[count].ValueType;
                dt.Columns.Add(dc);
            }
            //DataGridViewColumn[] dcs = new DataGridViewColumn[dt.Columns.Count];
            //dgv.Columns.CopyTo(dcs, 0);
            //List<string> ss = new List<string>(dcs.Select(r => r.ValueType.Name));
            //string s = string.Join("\t", ss);
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                {
                    Type type = dgv.Columns[countsub].ValueType;
                    if (dgv.Rows[count].Cells[countsub].Value != DBNull.Value
                       && dgv.Rows[count].Cells[countsub].Value != null)
                    {
                        if (type == typeof(string))
                            dr[countsub] = dgv.Rows[count].Cells[countsub].Value.ToString();
                        else if (type == typeof(double) || type == typeof(float))
                            dr[countsub] = Convert.ToDouble(dgv.Rows[count].Cells[countsub].Value.ToString());
                        else if (type == typeof(int))
                            dr[countsub] = Convert.ToInt32(dgv.Rows[count].Cells[countsub].Value.ToString());
                        else
                        {
                            MessageBox.Show(type.ToString());
                            ;
                        }
                    }
                    else
                    {
                        if (type == typeof(string))
                            dr[countsub] = "";
                        else
                            dr[countsub] = 0;
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static string DataTableToTxt(DataTable dtsave)
        {
            StringBuilder outstr = new StringBuilder();
            foreach (DataColumn dc in dtsave.Columns)
            {
                outstr.Append(dc.ColumnName + ",");
            }
            outstr.Append("\r\n");
            foreach (DataRow dr in dtsave.Rows)
            {
                foreach (DataColumn dc in dtsave.Columns)
                {
                    if (dc.DataType != typeof(System.Drawing.Image))
                        outstr.Append(dr[dc.ColumnName] + ",");
                    else
                        outstr.Append("image,");

                }
                outstr.Append("\r\n");
            }
            outstr = outstr.Replace(",\r\n", "\r\n");
            return outstr.ToString();
        }
    }
    public class StringTools
    {
        public static Point StringToPoint(string str)
        {
            String[] vstr = str.Split(',');
            if (vstr.Count() != 2)
                throw new NotImplementedException();
            int x, y;
            int.TryParse(vstr[0], out x);
            int.TryParse(vstr[1], out y);
            return new Point(x, y);
        }
        public static Size StringToSize(string str)
        {
            String[] vstr = str.Split(',');
            if (vstr.Count() != 2)
                throw new NotImplementedException();
            int x, y;
            int.TryParse(vstr[0], out x);
            int.TryParse(vstr[1], out y);
            return new Size(x, y);
        }
        public static Rectangle StringToRectangle(string str,char splitchar = ',')
        {
            String[] vstr = str.Split(splitchar);
            if (vstr.Count() != 4)
                throw new NotImplementedException();
            int x, y, w, h;
            int.TryParse(vstr[0], out x);
            int.TryParse(vstr[1], out y);
            int.TryParse(vstr[2], out w);
            int.TryParse(vstr[3], out h);
            return new Rectangle(x, y, w, h);
        }
    }
    public class FileTools
    {
        public static string GetLastestSubDirectory(string path)
        {
            List<string> sudirects = GetLastestSubDirectorys(path);
            if (sudirects.Count > 0)
                return sudirects.Max();
            return "";
        }
        public static List<string> GetLastestSubDirectorys(string path)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("[0-9]{8}-[0-9]+");
            if (System.IO.Directory.Exists(path))
            {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
                List<string> sudirects = new List<string>();
                foreach (System.IO.DirectoryInfo d in dir.GetDirectories())
                {
                    if (r.IsMatch(d.Name))
                    {
                        sudirects.Add(d.FullName);
                    }
                }
                return sudirects;
            };
            return new List<string>();
        }
        public static List<string> NameListFromDir(string fidir, string ext = ".tif")
        {
            List<string> namelist = new List<string>();
            System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(fidir);
            //string ext = fi.Extension;
            foreach (System.IO.FileInfo f in dirinfo.GetFiles())
                if (f.Extension.ToLower() == ext)
                    namelist.Add(f.FullName);
            return namelist;
        }
    }
}
