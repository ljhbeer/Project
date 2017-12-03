using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace AR
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.panelyuejuan.Visible = false; 
            _papertemplate = new PaperTemplate();
            _papers = new Papers(_papertemplate);
            _config = new Config(_papers,_papertemplate);
        }  
        private void 导出阅卷数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.FileName = "保存阅卷数据";
            saveFileDialog2.Filter = "Xml files (*.xml)|*.xml";
            saveFileDialog2.Title = "保存阅卷数据";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //TODO: config.SavePaperData
                    _config.SavePaperData(saveFileDialog2.FileName);
                }
                catch
                {
                    MessageBox.Show("保存阅卷数据故障", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("已保存阅卷数据到： " + saveFileDialog2.FileName + "文件");
            }
        }
        private void 导入阅卷数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog2 = new OpenFileDialog();
            OpenFileDialog2.FileName = "导入阅卷数据";
            OpenFileDialog2.Filter = "Xml files (*.xml)|*.xml";
            OpenFileDialog2.Title = "导入阅卷数据";
            if (OpenFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _config.LoadPaperData(OpenFileDialog2.FileName);
                    //config.Paper
                   //显示阅卷界面
                    _papers.InitReadDatas();
                }
                catch
                {
                    MessageBox.Show("导入阅卷数据故障", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        private void 试卷初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormInitData f = new FormInitData(_config);
            f.ShowDialog();
            this.Show();
        }
        private void 开始阅卷ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StartYueJuan();
        }
        private void 停止阅卷ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.panelyuejuan.Visible = false;
            if(_read!=null)
            _read.Read(false, null);

        }

        private void 导出成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.panelyuejuan.Visible == false)
            {
                SaveFileDialog saveFileDialog2 = new SaveFileDialog();
                saveFileDialog2.FileName = "导出成绩";
                saveFileDialog2.Filter = "Text files (*.txt)|*.txt";
                saveFileDialog2.Title = "导出成绩";
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    try
                    {                       
                        if (!_config.Answer.CheckAnswer())
                        {
                            _config.InitAnswer();
                            new FormSetscore(_config).ShowDialog();                            
                        }
                        if (_config.Answer.CheckAnswer())
                        {
                            _config.SaveScores(saveFileDialog2.FileName);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("导出成绩故障", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show("已导入成绩到： " + saveFileDialog2.FileName + "文件");
                }
            }
        }        
        private void 更改客观题答案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _config.InitAnswer();
            new FormSetscore(_config).ShowDialog(); 
        }
        private void StartYueJuan()
        {          
            ActivePage ap = new ActivePage(pictureBox2.Size);
            this.comboBoxQustiongroup.Items.Clear();
            foreach(PaperBlocks uc in _papers.PaperBlocks())
            {
                this.comboBoxQustiongroup.Items.Add(uc);
            }
            _read = new ARReader(this.comboBoxQustiongroup, this.buttonPageUp, this.buttonPageDown, this.pictureBox2,
                this.comboBoxSetScore, this.buttonupdate, this);
            _read.Read(true, ap);
            this.panelyuejuan.Visible = true;
        }
        
        
        private Config _config;
        private PaperTemplate _papertemplate;
        private Papers _papers;
        private ARReader _read;

    }
}