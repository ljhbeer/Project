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
using System.Text.RegularExpressions;
using System.Threading;
using ZXing.Common;
using ZXing;
using Tools;
using ScanTemplate.FormYJ;
namespace ScanTemplate
{
	public delegate void MyInvoke( );
	public partial class FormM : Form
	{
        public static Config g_cfg = new Config();
        private ScanConfig sc;
        private DataTable _rundt;
        private Scan _scan;
		public FormM()
		{
			InitializeComponent();
            _rundt = null;
		}
		private void FormM_Load(object sender, EventArgs e)
		{            
            if ( Directory.Exists(textBoxWorkPath.Text))
            {
                sc = new ScanConfig(textBoxWorkPath.Text);
                listBoxUnScanDir.Items.Clear();
                comboBoxTemplate.Items.Clear();
                listBoxScantData.Items.Clear();
                listBoxUnScanDir.Items.AddRange(sc.Unscans.Unscans.ToArray());
                comboBoxTemplate.Items.AddRange(sc.CommonTemplates.CommonTemplates.ToArray());
                listBoxScantData.Items.AddRange(sc.Scandatas.Scandatas.ToArray());
            }
		}
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            FormM_Load(null, e);
        }
		private void buttonLeftHide_Click(object sender, EventArgs e)
		{
			if (TLP.ColumnStyles[0].Width > 10)
				TLP.ColumnStyles[0].Width =2;
			else
				TLP.ColumnStyles[0].Width =20.0F;

		}
		private void buttonRightHide_Click(object sender, EventArgs e)
		{
			if (TLP.ColumnStyles[2].Width >10)
				TLP.ColumnStyles[2].Width =2;
			else
				TLP.ColumnStyles[2].Width =35.0F;

		}
		private void buttonworkpath_Click(object sender, EventArgs e)
		{
			string str = textBoxWorkPath.Text;
			if (Directory.Exists(str))
                FormM_Load(null, e);
		}
        private void buttonCreateTemplate_Click(object sender, EventArgs e)
        {
            if (listBoxUnScanDir.SelectedIndex == -1) return;
            UnScan dir = (UnScan)listBoxUnScanDir.SelectedItem;
            List<string> nameList = dir.ImgList();
            if (nameList.Count > 0)
            {
                sc.Templateshow = new TemplateShow(dir.FullPath, dir.DirName, nameList[0]);
                if (sc.Templateshow.OK)
                {
                    this.Hide();
                    new FormTemplate(sc.Templateshow.Template).ShowDialog();
                    this.Show();
                }
            }
        }
        private void buttonMatchTemplate_Click(object sender, EventArgs e)
        {
            if (listBoxUnScanDir.SelectedIndex == -1 || comboBoxTemplate.SelectedIndex == -1)
            {
				MessageBox.Show("未选择目录或者模板");
                return;
            }
            TemplateInfo ti = (TemplateInfo)comboBoxTemplate.SelectedItem;
            UnScan dir = (UnScan)listBoxUnScanDir.SelectedItem;
            List<string> nameList = dir.ImgList();
            if (nameList.Count > 0)
            {
                sc.Templateshow = new TemplateShow(dir.FullPath, dir.DirName, nameList[0], ti);
                if (sc.Templateshow.OK)
                {
                    this.Hide();
                    new FormTemplate(sc.Templateshow.Template).ShowDialog();
                    this.Show();
                }
            }
        }
		private void ButtonScanClick(object sender, EventArgs e)
		{
			if (listBoxUnScanDir.SelectedIndex == -1 || comboBoxTemplate.SelectedIndex == -1)
            {
				MessageBox.Show("未选择目录或者模板");
                return;
            }
            TemplateInfo ti = (TemplateInfo)comboBoxTemplate.SelectedItem;
            UnScan dir = (UnScan)listBoxUnScanDir.SelectedItem;
            List<string> nameList = dir.ImgList();
            if (nameList.Count > 0)
            {
                AutoDetectRectAnge.FeatureSetPath = dir.FullPath;
                _scan = new Scan(sc,ti.TemplateFileName, nameList,dir.FullPath);
                _rundt = Tools.DataTableTools.ConstructDataTable( _scan.ColNames.ToArray() );
                dgv.DataSource = _rundt;
                InitDgvUI();
                _scan.DgSaveScanData = new DelegateSaveScanData( ExportData );
                _scan.DgShowScanMsg = new DelegateShowScanMsg( ShowMsg);
                _scan.DoScan();
            }
		}
        private void buttonCreateYJData_Click(object sender, EventArgs e)
        {
            //if (_artemplate == null || _rundt == null || _rundt.Rows.Count == 0)
            //    return;
            //this.Hide();
            //FormYJ.FormYJInit f = new FormYJ.FormYJInit(_artemplate,_rundt,_angle,_workpath);
            //f.ShowDialog();            
            //this.Show();
        }       
		private void listBoxData_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxScantData.SelectedIndex == -1) return;			
		}
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
		private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
            //if (e.RowIndex == -1 || _rundt == null || e.ColumnIndex == -1 || _angle==null || _artemplate==null )
            //    return;
            //string fn = _rundt.Rows[e.RowIndex]["文件名"].ToString().Replace("LJH\\","LJH\\Correct\\");
            //if (File.Exists(fn))
            //{
            //    double angle = (double)(_rundt.Rows[e.RowIndex]["校验角度"]);
            //    Bitmap bmp =(Bitmap) Bitmap.FromFile(fn);
            //    if (_angle != null)
            //        _angle.SetPaper(angle);
            //    pictureBox1.Image = ARTemplate.TemplateTools.DrawInfoBmp(bmp, _artemplate, _angle);
            //}
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
        private void InitDgvUI()
        {
            foreach (DataGridViewColumn dc in dgv.Columns)
                if (dc.Name.StartsWith("x"))
                    dc.Width = 20;
                else if (dc.Name == "序号")
                    dc.Width = 30;
                else
                    dc.Width = 60;
        }
        public void ShowMsg(string  msg)
        {
            string[] ss = msg.Trim().Split(',');
            DataRow dr = _rundt.NewRow();
            MsgToDr(ss, ref dr);
            _rundt.Rows.Add(dr);
        }
        private void ExportData(string exportdata)
        {
            if (InputBox.Input("考试名称"))
            {
                string examname = InputBox.strValue;              
                string Datafilename = _scan.ScanDataPath + "\\data.txt";
                string NewTemplatename = _scan.ScanDataPath + "template.xml";
                string NewImgsPath = _scan.ScanDataPath + "\\img";
                File.WriteAllText(_scan.ScanDataPath + "\\"+examname+".exam", examname);
                File.WriteAllText(Datafilename, string.Join(",",_scan.ExportTitles) + "\r\n" + exportdata);
                Directory.Move(_scan.SourcePath       ,NewImgsPath);
                File.Copy(_scan.TemplateName, NewTemplatename);
            }
        }
        private void MsgToDr(string[] ss, ref DataRow dr)
        {
            dr["序号"] = _rundt.Rows.Count + 1;
            if (ss.Length == _scan.ExportTitles.Count)
            {
                foreach (KeyValuePair<string, int> kv in _scan.Titlepos)
                {
                    if (kv.Key.Contains("校验"))
                        dr[kv.Key] = Convert.ToDouble(ss[kv.Value]);
                    else
                        dr[kv.Key] = ss[kv.Value];
                }
                if (_scan.Xztpos > 0)
                {
                    string[] xx = ss[_scan.Xztpos].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int ii = 0; ii < xx.Length; ii++)
                        dr["x" + (ii + 1)] = xx[ii];
                }
            }
        }
        private void AddDataToDt(DataRow dr, Bitmap bmp, DataTable dt)
        {
            //double angle = (double)(dr["校验角度"]);
            //if (_angle != null)
            //    _angle.SetPaper(angle);
            //for (int i = 0; i < _artemplate.XztRect.Count; i++)
            //{
            //    string value = dr["x" + (i + 1)].ToString();
            //    if (value.Length != 1 || !"ABCD".Contains(value))
            //    {
            //        DataRow ndr = dt.NewRow();
            //        string xuehao = "";
            //        if (dr.Table.Columns.Contains("考号"))
            //            xuehao = dr["考号"].ToString();
            //        ndr["学号"] = new ValueTag(xuehao, dr);
            //        ndr["题号"] = "x" + (i + 1);
            //        Rectangle r = _artemplate.XztRect[i];
            //        //r.Location = _angle.GetCorrectPoint(r.X,r.Y);
            //        Bitmap nbmp = bmp.Clone(r, bmp.PixelFormat);
            //        ndr["图片"] = nbmp;
            //        ndr["你的答案"] = value;
            //        ndr["是否多选"] = false;
            //        ndr["是否修改"] = false;
            //        dt.Rows.Add(ndr);
            //    }
            //}
        }
		private List<string> NameListFromFile(string filename)
		{
			if (File.Exists(filename))
			{
				FileInfo fi = new FileInfo(filename);
				string fidir = fi.Directory.FullName;
                return FileTools.NameListFromDir(fidir);
			}
			return new List<string>();
		}
	}	
}
