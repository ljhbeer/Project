using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ARTemplate;

namespace ScanTemplate.FormYJ
{
    public partial class FormYJInit : Form
    {
        private Template _artemplate;
        private DataTable _rundt;
        private AutoAngle _angle;
        private DataTable _dtsetxzt;
        private DataTable _dtsetfxzt;
        private Bitmap _src;
        private int _AvgUnImgWith;
        private int _AvgUnImgHeight;
        public FormYJInit(ARTemplate.Template _artemplate, DataTable _rundt, AutoAngle _angle)
        {
            // TODO: Complete member initialization
            this._artemplate = _artemplate;
            this._rundt = _rundt;
            this._angle = _angle;
            this._src = _artemplate.Image;
            InitializeComponent();
            dgv.DataSource = _rundt;
            InitImage();
        }
        private void InitImage()
        {
            Bitmap bmp = (Bitmap)_artemplate.Image.Clone();
            pictureBox1.Image = ARTemplate.TemplateTools.DrawInfoBmp(bmp,_artemplate,_angle);
        }
        private void InitDgvUI()
        {
            foreach (DataGridViewColumn dc in dgv.Columns)
                if (dc.Name.StartsWith("x"))
                    dc.Width = 20;
                else
                    dc.Width = 40;
        }
        private void InitDgvSetUI(bool xzt)
        {
            dgvSet.DataSource = null;
            if (xzt)
            {
                dgvSet.RowTemplate.Height = 24;
                dgvSet.DataSource = _dtsetxzt;
            }
            else
            {
                dgvSet.RowTemplate.Height = _AvgUnImgHeight;
                dgvSet.DataSource = _dtsetfxzt;
            }

            foreach (DataGridViewColumn dc in dgvSet.Columns)
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
        private void FormYJInit_Load(object sender, EventArgs e)
        {
            InitDgvUI();
            AddChooseTodtset(ref _dtsetxzt);
            AddUnChooseTodtset(ref _dtsetfxzt);
            InitDgvSetUI(true);
        }
        private void AddChooseTodtset(ref DataTable dtset)
        {
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "ID", "题组名称", "最大分值", "正确答案" });
            int cnt = 0;
            foreach (Area I in _artemplate.Dic["选择题"])
            {
                SingleChoiceArea  U = (SingleChoiceArea)I;
                if (I.HasSubArea())
                {
                    foreach (List<Point> lp in ((SingleChoiceArea)I).list)
                    {
                        Rectangle r = I.ImgArea;
                        DataRow dr = dtset.NewRow();
                        dr["ID"] =cnt++;
                        dr["题组名称"] ="x"+cnt;
                        dr["最大分值"] =1;
                        //dr["图片"] = _src.Clone(U.ImgArea, _src.PixelFormat);
                        dtset.Rows.Add(dr);
                    }
                }
            }
        }
        private void AddUnChooseTodtset(ref DataTable dtset)
        {
            dtset = Tools.DataTableTools.ConstructDataTable(new string[] { "ID", "题组名称", "最大分值", "图片" });
            _AvgUnImgWith = 0;
            _AvgUnImgHeight = 0;
            int ID = 0;
            foreach (Area I in _artemplate.Dic["非选择题"])
            {
                UnChoose U = (UnChoose)I;
                DataRow dr = dtset.NewRow();
                dr["ID"] = ID++;
                dr["题组名称"] = U.Name;
                dr["最大分值"] = U.Scores;
                _AvgUnImgHeight += I.ImgArea.Height;
                _AvgUnImgWith += I.ImgArea.Width;
                dr["图片"] = _src.Clone(U.ImgArea, _src.PixelFormat);
                dtset.Rows.Add(dr);
            }
            _AvgUnImgHeight /= ID;
            _AvgUnImgWith /= ID;
        }
        private void buttonShowXztSet_Click(object sender, EventArgs e)
        {
            InitDgvSetUI(true);
        }
        private void buttonShowFXztSet_Click(object sender, EventArgs e)
        {
            InitDgvSetUI(false);
        }
        private void buttonCreateYJData_Click(object sender, EventArgs e)
        {

        }

        private void buttonImportOptionAnswerScore_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            foreach(DataRow dr in _dtsetxzt.Rows)
                ids.Add("xz"+ dr["ID"].ToString());
            FormSetscore f = new FormSetscore(ids);
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < f.Xzt().Count; i++)
                {
                    if(f.Xzt()[i].ID.ToString().EndsWith( 
                    _dtsetxzt.Rows[i]["ID"].ToString() ))
                    {
                        _dtsetxzt.Rows[i]["正确答案"] = f.Xzt()[i].OptionAnswer;
                        _dtsetxzt.Rows[i]["最大分值"] = f.Xzt()[i].Score;
                    }

                }
            }
        }
    }
}
