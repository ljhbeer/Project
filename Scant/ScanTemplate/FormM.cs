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
		public FormM()
		{
			InitializeComponent();		
          
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
		private void ButtonScanClick(object sender, EventArgs e)
		{
			if (listBoxUnScanDir.SelectedIndex == -1) return;
            if (comboBoxTemplate.SelectedIndex == -1)
            {
				MessageBox.Show("还未选择模板");
				return ;
			}
            //string path = listBoxUnScanDir.SelectedItem.ToString();
            //List<string> nameList = FileTools.NameListFromDir(path);
            //if (nameList.Count == 0) return;
            //Bitmap bmp = (Bitmap)Bitmap.FromFile(_artemplate.Filename);
            //MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(bmp);
            //DetectAllImgs(dr, nameList);			
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

        private void buttonCreateTemplate_Click(object sender, EventArgs e)
        {
            if ( listBoxUnScanDir .SelectedIndex == -1) return;
            UnScan dir =(UnScan) listBoxUnScanDir.SelectedItem;
            List<string> nameList = dir.ImgList();
            if (nameList.Count > 0)
            {
                sc.Templateshow = new TemplateShow(dir.FullPath,dir.DirName,nameList[0]);
                if (sc.Templateshow.OK)
                {
                    this.Hide();                   
                    new FormTemplate(sc.Templateshow.Template).ShowDialog();
                    this.Show();
                }
            }
        }
	}	
}
