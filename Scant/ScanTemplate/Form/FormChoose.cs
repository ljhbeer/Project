using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ScanTemplate
{
    public partial class FormChoose : Form
    {
        public FormChoose()
        {
            InitializeComponent();
        }
        private void FormChoose_Load(object sender, EventArgs e)
        {
            if (File.Exists("global.json"))
            {
                try
                {
                    string _filename = "global.json";
                    globalsave gs = Newtonsoft.Json.JsonConvert.DeserializeObject<globalsave>(File.ReadAllText(_filename));
                    global.Debug = gs.Debug;
                    global.featuretype = gs.featuretype;
                    global.tag = gs.tag;
                    global.msg = gs.msg;
                    global.UserMode = true;
                    if (gs.ProgramerCode == "ljh_beer@163.com.copyright")
                    {
                        global.UserMode = false;
                    }
                }
                catch
                {
                }
            }
            else
            {
                SaveGlobal();
            }
            if (!global.LocReghelper.CheckReged())
            {
                SoftRegTools.FormRegShow f = new SoftRegTools.FormRegShow(global.LocReghelper);
                f.ShowDialog();
                //this.Close();
            }
            else
            {
                if (!global.Reghelper.CheckReged())
                {
                    MessageBox.Show("你还没有注册");
                    global.LocReghelper.IsReged = false;
                    global.Reghelper.IsReged = false;
                    //this.Close();
                }else
                global.Threadchecksign.StartRun();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormM().ShowDialog();
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormYJ.FormYJTools().ShowDialog();
            this.Close();
        }
        private void SaveGlobal()
        {
            //string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(new  global()));
            //File.WriteAllText("global.json", str);
            globalsave gs = new globalsave();
            gs.Debug = global.Debug;
            gs.featuretype = global.featuretype;
            gs.tag = global.tag;
            gs.msg = global.msg;
            string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(gs));
            File.WriteAllText("global.json", str);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRegInfo_Click(object sender, EventArgs e)
        {
            SoftRegTools.FormRegShow f = new SoftRegTools.FormRegShow(global.LocReghelper);
            f.ShowDialog();
        }
  
    }
}
