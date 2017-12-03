using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ShowImage
{
    public partial class FormReplace : Form
    {
        public FormReplace()
        {
            InitializeComponent();
        }

        internal void Init(string replacestr)
        {
            if(ValidateReplaceStr(replacestr))
                listBox1.Items.Add(replacestr);
            if (File.Exists("replace.ini"))
            {
                string[] lines = File.ReadAllLines("replace.ini");
                foreach (string s in lines)
                {
                    if (ValidateReplaceStr(s.Trim()))
                        listBox1.Items.Add(s.Trim());
                }
                if (listBox1.Items.Count > 0)
                    textBox1.Text = listBox1.Items[0].ToString();
            }
        }

        private bool ValidateReplaceStr(string s)
        {
            if (s.Length > 4 && s.StartsWith("[") && s.EndsWith("]"))
            {
                int sum = 0;
                foreach (char c in s)
                {
                    if (c == '[')
                        sum++;
                    else if (c == ']')
                        sum--;
                    if (sum < 0 || sum > 1)
                        return false;
                }
                return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (fd.FileName.EndsWith("txt")
                   || fd.FileName.EndsWith("htm")
                   || fd.FileName.EndsWith("html")
                   || fd.FileName.EndsWith("ini"))
                {
                    string htmlpage = File.ReadAllText(fd.FileName);
                    List<List<string>> replacetonull = Form1.SplitToMultiList(textBox1.Text);
                    foreach (List<string> s in replacetonull)
                        htmlpage = htmlpage.Replace(s[0], s[1]);
                    File.WriteAllText(fd.FileName + ".html", htmlpage);
                    MessageBox.Show("已替换，新名称为" + fd.FileName + ".html");
                }
                else
                {
                    MessageBox.Show("暂时只支持txt,Html,htm,ini文件");
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                textBox1.Text = listBox1.SelectedItem.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
