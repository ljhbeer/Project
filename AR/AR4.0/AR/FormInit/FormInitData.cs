using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AR
{
    public partial class FormInitData : Form
    {
        public FormInitData(Config config)
        {
            InitializeComponent();
            this.config = config;

            _papertemplate = new PaperTemplate();
            _papers = new Papers(_papertemplate) ;//config.Papers();            
            this._config = new Config(_papers, _papertemplate);
            _controllist = new List<Control>(){//linkLabel1ChooseTemplate,
                                               linkLabel2ChoosePicturePath,
                                               linkLabel3SetAnswerScore,
                                               //linkLabel4LoadNameList,
                                               buttonScan,BtnSaveReadData};
            linkLabel4LoadNameList.Enabled = false;
            foreach (Control c in _controllist)
                c.Enabled = false; 
        }
        private void InitShow()
        {
            foreach (Control c in _controllist)
                c.Enabled = true;
            BtnSaveReadData.Enabled = false;
        }      
        private void linkLabel1ChooseTemplate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            if (LoadTemplate())
            {
                linkLabel2ChoosePicturePath.Enabled = true;
                textBoxTemplateName.Text = _papertemplate.Filename;
            }
        }
        private void linkLabel2ChoosePicturePath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            List<Image> imgs = LoadPictures(_papertemplate.Imgsize);
            if (imgs.Count > 0)
            {
                _papers.Clear();
                _papers.AddPapers(imgs);
                //_papers.InitReadDatas();
                InitShow();
            }
            else
            {
                MessageBox.Show("没有合适的试卷");
            }
        }
        private void linkLabel3SetAnswerScore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //////////原先的 ANswer没有导入
            _config.InitAnswer();
            new FormSetscore(_config).ShowDialog();
            if (_config.Answer.CheckAnswer())
            {
                if (!linkLabel3SetAnswerScore.Text.EndsWith("√"))
                    linkLabel3SetAnswerScore.Text += "√";
            }
        }
        private void linkLabel4LoadNameList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { //采用Excel 导入 设定Tablebiao

        }
        private void buttonScan_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<string> titles = new List<string>(){"ID","Path"};
            for (int i = 0; i < _papertemplate.ChoiceQuestionCount; i++)
                titles.Add("x" + (i + 1));
            InitDataTable(dt,titles);
            dgv1.DataSource = dt;
            InitDgv(dgv1, titles);
            string[] ABCD = new string[]{"A","B","C","D"};
            foreach (Paper paper in _papers.PaperList)
            {
                paper.Optionanswers = _papertemplate.ComputeChoice(paper.Img, paper.CornerPoint());
                DataRow dr = dt.NewRow();
                dr["ID"] = paper.ID;
                paper.Optionanswers = _papertemplate.ComputeChoice(paper.Img, paper.CornerPoint());
                dr["Path"] = paper.FileName;
                for (int i = 0; i < paper.Optionanswers.Count; i++)
                    if(paper.Optionanswers[i]>=0)
                    dr["x" + (i + 1)] = ABCD[ paper.Optionanswers[i] ] ;
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                BtnSaveReadData.Enabled = true;
            }
        }
        private void InitDataTable(DataTable dt, List<string> titles)
        {
            foreach (string s in titles)
            {
                DataColumn dc = new DataColumn(s);
                dt.Columns.Add(dc);
            }           
        }
        private void BtnSaveReadData_Click(object sender, EventArgs e)
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
        public void InitDgv(DataGridView dgv, List<string> titles) // used by settypeform
        {   
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.RowHeadersVisible = false;
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                if (titles[i].StartsWith("x"))
                {
                    dgv.Columns[i].Width = 25;
                    //dgv.Columns[i].HeaderText = titles[i];
                    dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                else
                {
                    dgv1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
            }    
        }       
        private bool LoadTemplate()
        {
            OpenFileDialog OpenFileDialog2 = new OpenFileDialog();
            OpenFileDialog2.FileName = "导入模板";
            OpenFileDialog2.Filter = "Xml files (*.xml)|*.xml";
            OpenFileDialog2.Title = "导入模板";
            if (OpenFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (_papertemplate.Load(OpenFileDialog2.FileName)
                        && _papertemplate.CheckEmpty())
                        return false;
                    //MessageBox.Show("已导入模板" + _papertemplate.Filename);
                }
                catch
                {
                    MessageBox.Show("导入模板故障", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        private List<Image> LoadPictures(Size Imgsize)
        {
            List<Image> imgs = new List<Image>();
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo[] files;
                System.IO.DirectoryInfo floder = System.IO.Directory.CreateDirectory(folderBrowserDialog1.SelectedPath);
                files = floder.GetFiles();
                foreach (System.IO.FileInfo file in files)
                {
                    if (!".tif.jpg.jpeg.png.gif.bmp".Contains(file.Extension.ToLower()))
                        continue;
                    Image img = Bitmap.FromFile(file.FullName);
                    img.Tag = file.FullName;
                    if (Math.Abs((img.Size.Height - Imgsize.Height) / Imgsize.Height) < 0.1
                      && Math.Abs((img.Size.Width - Imgsize.Width) / Imgsize.Width) < 0.1)
                        imgs.Add(img);
                }
            }
            return imgs;
        }
        private Config config;
        private List<Control> _controllist;
        private Config _config;
        public PaperTemplate _papertemplate { get; set; }
        public Papers _papers { get; set; }

        
    }
}
