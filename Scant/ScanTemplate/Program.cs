using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ScanTemplate
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormChoose());
        }
    }
    public class global
    {
        public static Boolean Debug = false;
        public static int tag = 0;
        public static int featuretype = 0;  // 0 左上下  1右上下
        public static string msg = "说明";

        public static void SaveDirectoryMemo(string path,string infotip)
        {
            string filename = path + "\\desktop.ini";
            string txt = "[.ShellClassInfo]\r\nInfoTip=[infotip]\r\n";
            txt = txt.Replace("[infotip]", infotip);
            System.IO.File.WriteAllText(filename, txt);
        }
    }
    public class globalsave
    {
        public  Boolean Debug;
        public  int tag;
        public  int featuretype;  // 0 左上下  1右上下
        public  string msg;
    }
}
