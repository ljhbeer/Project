using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ARTemplate;

namespace ScanTemplate.FormYJ
{
    public partial class FormYJTools : Form
    {
        public FormYJTools()
        {
            InitializeComponent();
            _workpath = textBoxWorkPath.Text;
            _exam = null;
            _template = null;
            _src = null;
        }
        private void FormYJTools_Load(object sender, EventArgs e)
        {
            InitExamInfos();
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            InitExamInfos();
        }
        private void InitExamInfos()
        {
            string exampath = _workpath.Substring(0, _workpath.LastIndexOf("\\")) + "\\Exam\\";
            string idindexpath = exampath + "config.json";
            if (!File.Exists(idindexpath))
                return;
            Config g = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(idindexpath));
            listBox1.Items.AddRange(g._examinfo.ToArray());
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            ExamInfo ei = (ExamInfo)listBox1.SelectedItem;
            string filename = ei.Path.Substring(0, ei.Path.Length - ei.Number.ToString().Length - 1) + ei.Name + ".json";
            if(!File.Exists(filename))
                return;
            _template = null;
            _src = null;
            if (File.Exists(ei.TemplateFileName))
            {
                _template = new  Template(ei.TemplateFileName);
                _src = _template.Image;
                if(_src!=null)
                    InitImage();
            }
            Examdata  examdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Examdata >(File.ReadAllText(filename));
            examdata.SR._Students.InitDeserialize();
            examdata.SR._Imgsubjects.InitDeserialize();
            for (int index = 0; index < examdata.SR._Imgsubjects.Subjects.Count; index++)
            {
                examdata.SR._Imgsubjects.Subjects[index].Index = index;
            }
            _exam = new Exam(examdata);

            InitDgvUI();
            AddChooseTodtset(ref _dtsetxzt);
            AddUnChooseTodtset(ref _dtsetfxzt);
            InitDgvSetUI(true);

        }
        private void buttonModifyData_Click(object sender, EventArgs e)
        {

        }
        private void buttonBeginYJ_Click(object sender, EventArgs e)
        {
            if (_exam == null ) return;             
            //TODO: NextWork  saveResult
            FormFullScreenYJ fs = new FormFullScreenYJ(_exam);
            this.Hide();
            fs.ShowDialog();
            this.Show();
        }
        private void buttonShowXztSet_Click(object sender, EventArgs e)
        {
            InitDgvSetUI(true);
        }
        private void buttonShowFXztSet_Click(object sender, EventArgs e)
        {
            InitDgvSetUI(false);
        }
        private void buttonImportOptionAnswerScore_Click(object sender, EventArgs e)
        {
           FormYJInit.ImportOptionAnswerScore(_dtsetxzt);
        }
        private void InitDgvSetUI(bool xzt)
        {
            dgv.DataSource = null;
            if (xzt)
            {
                dgv.RowTemplate.Height = 24;
                dgv.DataSource = _dtsetxzt;
            }
            else
            {
                dgv.RowTemplate.Height = _AvgUnImgHeight;
                dgv.DataSource = _dtsetfxzt;
            }

            foreach (DataGridViewColumn dc in dgv.Columns)
                if (dc.Name.Contains("图片"))
                {
                    dc.Width = _AvgUnImgWith;
                    ((DataGridViewImageColumn)dc).ImageLayout = DataGridViewImageCellLayout.Zoom;
                }
                else
                {
                    dc.Width = 40;
                }
        }
        private void InitDgvUI()
        {
            foreach (DataGridViewColumn dc in dgv.Columns)
                if (dc.Name.StartsWith("x"))
                    dc.Width = 20;
                else
                    dc.Width = 40;
        }
        private void InitImage()
        {            
            pictureBox1.Image = TemplateTools.DrawInfoBmp(_src, _template, null);
        }
        private void AddChooseTodtset(ref DataTable dtset)
        {
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "ID", "题组名称", "最大分值", "正确答案" });
            int cnt = 0;

            foreach (Area I in _template.Dic["选择题"])
            {
                SingleChoiceArea U = (SingleChoiceArea)I;
                if (I.HasSubArea())
                {
                    foreach (List<Point> lp in ((SingleChoiceArea)I).list)
                    {
                        Rectangle r = I.ImgArea;
                        DataRow dr = dtset.NewRow();
                        dr["ID"] = cnt++;
                        dr["题组名称"] = "x" + cnt;
                        dr["最大分值"] = 1;
                        dtset.Rows.Add(dr);
                    }
                }
            }
        }
        private void AddUnChooseTodtset(ref DataTable dtset)
        {
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "OID", "题组名称", "最大分值", "图片" });
            _AvgUnImgWith = 0;
            _AvgUnImgHeight = 0;           
            foreach (Imgsubject S in _exam.Subjects)
            {
                DataRow dr = dtset.NewRow();
                dr["OID"] = new ValueTag(S.ID.ToString(), S);
                dr["题组名称"] = S.Name;
                dr["最大分值"] = S.Score;
                _AvgUnImgHeight += S.Height;
                _AvgUnImgWith += S.Width;
                dr["图片"] = _src.Clone(S.Rect, _src.PixelFormat);
                dtset.Rows.Add(dr);
            }
            _AvgUnImgHeight /= _exam.Subjects.Count;
            _AvgUnImgWith /= _exam.Subjects.Count;
        }

        private Exam _exam;
        private string _workpath;
        private Bitmap _src;
        private DataTable _dtsetxzt;
        private DataTable _dtsetfxzt;
        private int _AvgUnImgWith;
        private int _AvgUnImgHeight;
        private Template _template;

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }
        private void pictureBox1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            double f = 0.0;
            if (numberOfTextLinesToMove > 0)
            {
                for (int i = 0; i < numberOfTextLinesToMove; i++)
                {
                    f += 0.05;
                }
                Zoomrat(f + 1, e.Location);
            }
            else if (numberOfTextLinesToMove < 0)
            {
                for (int i = 0; i > numberOfTextLinesToMove; i--)
                {
                    f -= 0.05;
                }
                Zoomrat(f + 1, e.Location);
            }
        }
        private void Zoomrat(double rat, Point e)
        {
            Bitmap bitmap_show = (Bitmap)pictureBox1.Image;
            Point L = pictureBox1.Location;
            Point S = panel3.AutoScrollPosition;
            int w = (int)(pictureBox1.Width * rat);
            int h = w * bitmap_show.Height / bitmap_show.Width;
            L.Offset((int)(e.X * (rat - 1)), (int)(e.Y * (rat - 1)));
            pictureBox1.SetBounds(S.X, S.Y, w, h);
            //zoombox.UpdateBoxScale(pictureBox1);
            S.Offset((int)(e.X * (1 - rat)), (int)(e.Y * (1 - rat)));
            panel3.Invalidate();
            panel3.AutoScrollPosition = new Point(-S.X, -S.Y);
        }
    }
    public class Examdata
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public StudentsResultData SR { get; set;}       
    }
    public class StudentsResultData
    {
        public string _workpath;
        public Students _Students;
        public Imgsubjects _Imgsubjects;
        public List<List<int>> _Result;
    }
}
