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
namespace ScanTemplate
{
	public delegate void MyInvoke( );
	public partial class FormM : Form
	{
		private string _workpath;
		private AutoAngle _angle;
		private Template _artemplate;
		private MyDetectFeatureRectAngle _rundr;
		private List<string> _runnameList;
		private String _runmsg;
		private DataTable _rundt;
		private ZXing.BarcodeReader _br;
		private string _exportdata;
		private List<string> _exporttitle;
		private Dictionary<string, int> _titlepos;
		private int _xztpos;
		public FormM()
		{
			InitializeComponent();
			_workpath = textBoxWorkPath.Text;
			_angle = null;
			_artemplate = null;
			_rundr = null;
			_runnameList=null;
			_runmsg = "";
			_exportdata = "";
			_xztpos = -1;
			_titlepos =  new Dictionary<string, int>();
			//for 二维码
			DecodingOptions decodeOption = new DecodingOptions();
			decodeOption.PossibleFormats = new List<BarcodeFormat>() {
				BarcodeFormat.All_1D
			};
			_br = new BarcodeReader();
			_br.Options = decodeOption;
		}
		private void FormM_Load(object sender, EventArgs e)
		{
			//AutoLoadLatestImg(_workpath);
			foreach (string s in GetLastestSubDirectorys(_workpath))
			{
				//TODO: 使用类，显示相关信息
				listBox1.Items.Add(s);
			}
			string templatepath = _workpath.Substring( 0,_workpath.LastIndexOf("\\"))+"\\Template";
			foreach (string s in NameListFromDir(templatepath,".xml"))
			{
				//TODO: 使用类，显示相关信息
				string value = s.Substring(s.LastIndexOf("\\")+1);
				listBoxTemplate.Items.Add( new ValueTag(value,s));
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
				_workpath = textBoxWorkPath.Text;
		}
		private void buttonGo_Click(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1) return;
			string path = listBox1.SelectedItem.ToString();
			List<string> nameList = NameListFromDir(path);
			if (nameList.Count == 0) return;
			string filename = nameList[0];
			CreateTemplate(filename);
		}
		private void ButtonUseTemplateClick(object sender, System.EventArgs e)
		{
			if (listBox1.SelectedIndex == -1) return;
			if(listBoxTemplate.SelectedIndex==-1){
				MessageBox.Show("还未选择模板");
				return ;
			}
			
			string path = listBox1.SelectedItem.ToString();
			List<string> nameList = NameListFromDir(path);
			if (nameList.Count == 0) return;
			string filename = nameList[0];
			
			string templatefile =((ValueTag) listBoxTemplate.SelectedItem).Tag.ToString();
			CreateTemplate(filename,templatefile);
		}
		private void ButtonScanClick(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1) return;
			if(listBoxTemplate.SelectedIndex==-1){
				MessageBox.Show("还未选择模板");
				return ;
			}
			string path = listBox1.SelectedItem.ToString();
			List<string> nameList = NameListFromDir(path);
			if (nameList.Count == 0) return;
			Bitmap bmp = (Bitmap)Bitmap.FromFile(_artemplate.Filename);
			MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(bmp);
			DetectAllImgs(dr, nameList);
			
		}
		private void buttonVerify_Click(object sender, EventArgs e)
		{
			if (_artemplate == null || _rundt == null || _rundt.Rows.Count == 0)
				return;
			this.Hide();
			//for 考号
			VerifyKaoHao();
			//for 选择题
			VerifyXzt();
			this.Show();
		}
        private void buttonCreateYJData_Click(object sender, EventArgs e)
        {
            if (_artemplate == null || _rundt == null || _rundt.Rows.Count == 0)
                return;
            this.Hide();
            FormYJ.FormYJInit f = new FormYJ.FormYJInit(_artemplate,_rundt,_angle,_workpath);
            f.ShowDialog();

            this.Show();
        }
		//暂时不起作用
		private void VerifyKaoHao()
		{
			DataTable dt = Tools.DataTableTools.ConstructDataTable(new string[] {
			                                                       	"学号",
			                                                       	"图片",
			                                                       	"姓名",
			                                                       	"是否修改"
			                                                       });
			Rectangle r = _artemplate.Dic["考号"][0].ImgArea;
			foreach (DataRow dr in _rundt.Rows) {
				if (dr["考号"].ToString() == "") {
					string fn = dr["文件名"].ToString().Replace("LJH\\", "LJH\\Correct\\");
					if (File.Exists(fn)) {
						double angle = (double)(dr["校验角度"]);
						Bitmap bmp = (Bitmap)Bitmap.FromFile(fn);
						if (_angle != null)
							_angle.SetPaper(angle);
						DataRow ndr = dt.NewRow();
						ndr["学号"] = new ValueTag(dr["考号"].ToString(), dr);

						Bitmap nbmp = bmp.Clone(r, bmp.PixelFormat);
						ndr["图片"] = nbmp;
						ndr["姓名"] = "";
						ndr["是否修改"] = false;
						dt.Rows.Add(ndr);
					}
				}
			}
			if(dt.Rows.Count>0){
				MessageBox.Show("暂未实现，待修改");
				FormVerify f = new FormVerify(dt);
				if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
					MessageBox.Show("校验失败");
				}
				//修改之后
				dt.Rows.Clear();
			}
		}
		private void VerifyXzt()
		{
			DataTable dt = Tools.DataTableTools.ConstructDataTable(new string[] {
			                                                       	"学号",
			                                                       	"题号",
			                                                       	"图片",
			                                                       	"你的答案",
			                                                       	"A",
			                                                       	"B",
			                                                       	"C",
			                                                       	"D",
			                                                       	"是否多选",
			                                                       	"是否修改"
			                                                       });
			int xztcnt = _artemplate.XztRect.Count;
			int runcnt = 0;
			foreach (DataRow dr in _rundt.Rows) {
				runcnt++;
				bool b = false;
				int xi = 0;
				for (; xi < xztcnt; xi++) {
					if (!"ABCD".Contains(dr["x" + (xi + 1)].ToString()) || dr["x" + (xi + 1)].ToString().Length > 1) {
						b = true;
						break;
					}
				}
				if (b) {
					string fn = dr["文件名"].ToString().Replace("LJH\\", "LJH\\Correct\\");
					if (File.Exists(fn)) {
						double angle = (double)(dr["校验角度"]);
						Bitmap bmp = (Bitmap)Bitmap.FromFile(fn);
						if (_angle != null)
							_angle.SetPaper(angle);
						AddDataToDt(dr, bmp, dt);
					}
				}
				if (b || runcnt == _rundt.Rows.Count) {
					if (dt.Rows.Count > 20 || (runcnt == _rundt.Rows.Count && dt.Rows.Count > 0)) {
						FormVerify f = new FormVerify(dt);
						if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
							MessageBox.Show("校验失败");
						}
						//修改之后
						dt.Rows.Clear();
					}
				}
			}
		}
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1) return;
			string templatename = listBox1.SelectedItem.ToString();
			string txt = templatename.Substring(templatename.LastIndexOf("\\") + 1);
			bool unselected = true;
			for(int i=0; i<listBoxTemplate.Items.Count; i++){
				if(listBoxTemplate.Items[i].ToString().StartsWith(txt)){
					listBoxTemplate.SelectedIndex = i;
					string templatefilename = ((ValueTag)listBoxTemplate.SelectedItem).Tag.ToString();
					if (_artemplate == null)
						InitTemplate(templatefilename);
					InitListBoxData(templatefilename );
					unselected = false;
					break;
				}
			}
			if(unselected)
				listBoxTemplate.SelectedIndex = -1;
		}
		private void InitTemplate(string templatefilename)
		{
			{
				Template t = new Template(templatefilename);
				if (t.Image != null)
				{
					_artemplate = t;
					List<Point> ListPoint = new List<Point>();
					foreach (Area I in t.Dic["特征点"])
					{
						ListPoint.Add(I.ImgArea.Location);
					}
					if (ListPoint.Count == 3)
						_angle = new AutoAngle(ListPoint);
				}
			}
		}
		private void InitListBoxData(string templatefilename)
		{
			string dataname = templatefilename.Replace(".xml", ".txt");
			string dir = templatefilename.Substring(0, templatefilename.LastIndexOf("\\"));
			List<string> data = NameListFromDir(dir, ".txt");
			data = data.Where(r => r.Contains(templatefilename.Replace(".xml", ""))).ToList();
			listBoxData.Items.Clear();
			foreach (string s in data)
			{
				string value = s.Substring(s.LastIndexOf("\\")+1);
				listBoxData.Items.Add(new ValueTag(value,s));
			}
		}
		private void listBoxTemplate_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxTemplate.SelectedIndex == -1) return;
			string templatefilename = ((ValueTag)listBoxTemplate.SelectedItem).Tag.ToString();
			InitTemplate(templatefilename);
			InitListBoxData(templatefilename);
		}
		private void listBoxData_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxData.SelectedIndex == -1) return;
			string dataname = ((ValueTag)listBoxData.SelectedItem).Tag.ToString();
			//TODO： 检测是否导入已有数据
			if (!File.Exists(dataname))
				return;
			InitRundt(_artemplate);
			dgv.DataSource = _rundt;
			InitDgvUI();
			InitDgvData(dataname);
		}
		private void pictureBox1_MouseEnter(object sender, EventArgs e)
		{
			//if (m_act == Act.ZoomMouse)
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
			if (e.RowIndex == -1 || _rundt == null || e.ColumnIndex == -1 || _angle==null || _artemplate==null )
				return;
			string fn = _rundt.Rows[e.RowIndex]["文件名"].ToString().Replace("LJH\\","LJH\\Correct\\");
			if (File.Exists(fn))
			{
				double angle = (double)(_rundt.Rows[e.RowIndex]["校验角度"]);
				Bitmap bmp =(Bitmap) Bitmap.FromFile(fn);
                if (_angle != null)
                    _angle.SetPaper(angle);
                pictureBox1.Image = ARTemplate.TemplateTools.DrawInfoBmp(bmp, _artemplate, _angle);
			}
		}
		//dr["选择题"] = ss[3];
		private void Zoomrat(double rat, Point e)
		{
			Bitmap bitmap_show = (Bitmap)pictureBox1.Image;
			Point L = pictureBox1.Location;
			Point S = panel1.AutoScrollPosition;
			int w = (int)(pictureBox1.Width * rat);
			int h = w * bitmap_show.Height / bitmap_show.Width;
			L.Offset((int)(e.X * (rat - 1)), (int)(e.Y * (rat - 1)));
			pictureBox1.SetBounds(S.X, S.Y, w, h);
			//zoombox.UpdateBoxScale(pictureBox1);
			S.Offset((int)(e.X * (1 - rat)), (int)(e.Y * (1 - rat)));
			panel1.Invalidate();
			panel1.AutoScrollPosition = new Point(-S.X, -S.Y);
		}		
		private void CreateTemplate(string filename, string templatefilename = ""){
			Bitmap bmp = (Bitmap)Bitmap.FromFile(filename);
			MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(bmp);
			if (dr.Detected())
			{
				if(_artemplate!=null)
					_artemplate.Clear();
				
				_artemplate = new ARTemplate.Template(filename, bmp, dr.CorrectRect);
				_angle = new AutoAngle(dr.ListPoint); //或者导入时 设置
				if(templatefilename!="" && File.Exists(templatefilename)){
					_artemplate.Load(templatefilename);
				}
				
				this.Hide();
				_artemplate.SetFeaturePoint(dr.ListFeatureRectangle, dr.CorrectRect);
				ARTemplate.FormTemplate f = new ARTemplate.FormTemplate(_artemplate);
				f.ShowDialog();
				this.Show();
			}
		}
		private void DetectAllImgs(MyDetectFeatureRectAngle dr, List<string> nameList)
		{
			FileInfo fi = new FileInfo(nameList[0]);
			string dir = fi.Directory.FullName.Replace("LJH\\", "LJH\\Correct\\");
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			
			InitRundt(_artemplate);
			dgv.DataSource = _rundt;
			InitDgvUI();
			_rundr=dr;
			_runnameList = nameList;
			
			Thread thread=new Thread(new ThreadStart(RunDetectAllImg));
			thread.Start();
		}
		private StringBuilder DetectAllImg(MyDetectFeatureRectAngle dr, string s,List<string> title=null)
		{
			if(dr==null ){
				title.Clear();
				title.Add("文件名");
				title.Add("CorrectRect");
				title.Add("校验角度");
				title.Add("选择题");
				if(_artemplate.Dic.ContainsKey("考号") && _artemplate.Dic["考号"].Count>0)
					title.Add("考号");
				return null;
			}
			Bitmap bmp = (Bitmap)Bitmap.FromFile(s);
			string str = s.Substring(s.Length - 7, 3);
			//MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(bmp);
			Rectangle CorrectRect = dr.Detected(bmp);
			StringBuilder sb = new StringBuilder();
			sb.Append(s + "," +  CorrectRect.ToString("-") + ",");// 文件名 CorrectRect
			if (CorrectRect.Width > 0)
			{
				//TODO: debug r1 in 001
				Rectangle cr1 = new Rectangle(CorrectRect.Right - 60, CorrectRect.Top - 20, 80, 80);
				Rectangle r1 = dr.Detected(cr1, bmp);
				Rectangle cr2 = new Rectangle(CorrectRect.Left - 20, CorrectRect.Bottom - 60, 80, 80);
				Rectangle r2 = dr.Detected(cr2, bmp);

				
				sb.Append( _angle.SetPaper(CorrectRect.Location, r1.Location, r2.Location)+"," ); //校验角度
				Bitmap nbmp = (Bitmap)bmp.Clone(CorrectRect, bmp.PixelFormat);
				nbmp.Save(s.Replace("LJH\\", "LJH\\Correct\\"));
				
				//计算选择题
				AutoComputeXZT acx = new AutoComputeXZT(_artemplate, _angle, nbmp);
				sb.Append(acx.ComputeXZT(str)); //选择题
				//计算二维码
				
				if(_artemplate.Dic.ContainsKey("考号") && _artemplate.Dic["考号"].Count>0){
					KaoHaoChoiceArea kha = (KaoHaoChoiceArea)(_artemplate.Dic["考号"][0]);
					if(kha.Type=="条形码"){
						Rectangle Ir = kha.ImgArea;
//						Ir.Offset(CorrectRect.Location);
						Bitmap barmap = (Bitmap)nbmp.Clone( kha.ImgArea,nbmp.PixelFormat);
						barmap.Save("f:\\aa.tif");
						ZXing.Result rs = _br.Decode(barmap);
						if(rs!=null){
							sb.Append(","+rs.Text);
						}
					}
				}


			}
			else
			{
				//检测失败
			}
			sb.AppendLine();
			return sb;
			//MessageBox.Show(sb.ToString());
		}
		public void RunDetectAllImg(){
			StringBuilder sb = new StringBuilder();
			_titlepos = ConstructTitlePos(ref _xztpos);
			foreach (string s in _runnameList)
			{
				_runmsg = DetectAllImg(_rundr, s).ToString();
				sb.Append(_runmsg);
				this.Invoke(new MyInvoke(ShowMsg));
				Thread.Sleep(100);
			}
			_exportdata = sb.ToString();
			this.Invoke(new MyInvoke(ExportData));
		}
		private void ExportData()
		{
			FileInfo fi = new FileInfo(_artemplate.Filename);
			string path = fi.Directory.Parent.Parent.FullName + "\\template\\";
			string filename = path + fi.Directory.Name + "_" + _artemplate.GetTemplateName() + ".txt";
			if (File.Exists(filename))
			{
				SaveFileDialog saveFileDialog2 = new SaveFileDialog();
				saveFileDialog2.FileName = filename;
				saveFileDialog2.Filter = "txt files (*.txt)|*.txt";
				saveFileDialog2.Title = "Save Data file";
				if (saveFileDialog2.ShowDialog() == DialogResult.OK)
				{
					filename = saveFileDialog2.FileName;
				}
				else
				{
					filename = "allimportdata.txt";
				}
			}
			
			File.WriteAllText(filename, string.Join(",",_exporttitle) + "\r\n"+ _exportdata);
			_exportdata = "";
		}
		private void InitDgvUI()
		{
			foreach (DataGridViewColumn dc in dgv.Columns)
				if (dc.Name.StartsWith("x"))
					dc.Width = 20;
		}
		private void InitRundt(Template _artemplate)
		{
			int xztcnt = 0;
			if (_artemplate.Dic.ContainsKey("选择题")) {
				foreach (Area I in _artemplate.Dic["选择题"]) {
					xztcnt += ((SingleChoiceArea)I).Count;
				}
			}
			List<string> colnames = new List<string> {"序号","姓名"};
			//init _exporttitle
			if(_exporttitle==null)
				_exporttitle = new List<string>();
			DetectAllImg(null,"",_exporttitle);
			
			colnames.AddRange( _exporttitle);
			colnames.Remove("选择题");
			for (int i = 0; i < xztcnt; i++)
				colnames.Add("x" + (i + 1));
			_rundt = Tools.DataTableTools.ConstructDataTable(colnames.ToArray());
		}
		private void InitDgvData(string dataname)
		{
			int xztpos = 0;
			string[] ls = File.ReadAllLines(dataname);
			// 应该根据第一行标题来进行
			Dictionary<string, int> titlepos = ConstructTitlePos(ref xztpos);
			for (int i = 1; i < ls.Length; i++) {
				string[] ss = ls[i].Split(',');
				DataRow dr = _rundt.NewRow();
				
				MsgToDr(titlepos, xztpos, ss, ref dr);
				_rundt.Rows.Add(dr);
			}
		}
		private Dictionary<string, int> ConstructTitlePos(ref int xztpos)
		{
			Dictionary<string, int> titlepos = new Dictionary<string, int>();
			xztpos = 0;
			for (int i = 0; i < _exporttitle.Count; i++) {
				if (_exporttitle[i] == "选择题") {
					xztpos = i;
				} else {
					titlepos[_exporttitle[i]] = i;
				}
			}
			return titlepos;
		}
		private void MsgToDr(Dictionary<string, int> titlepos, int xztpos, string[] ss, ref DataRow dr)
		{
			dr["序号"] = _rundt.Rows.Count + 1;
			if (ss.Length == _exporttitle.Count) {
				foreach (KeyValuePair<string, int> kv in titlepos) {
					if (kv.Key.Contains("校验"))
						dr[kv.Key] = Convert.ToDouble(ss[kv.Value]);
					else
						dr[kv.Key] = ss[kv.Value];
				}
				if(xztpos>0){
					string[] xx = ss[xztpos].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
					for (int ii = 0; ii < xx.Length; ii++)
						dr["x" + (ii + 1)] = xx[ii];
				}
			}
		}
		private void AddDataToDt(DataRow dr, Bitmap bmp, DataTable dt)
		{
			double angle = (double)( dr["校验角度"]);
			if (_angle != null)
				_angle.SetPaper(angle);
			for (int i = 0; i < _artemplate.XztRect.Count; i++) {
				string value = dr["x" + (i + 1)].ToString();
				if (value.Length != 1 || !"ABCD".Contains(value)) {
					DataRow ndr = dt.NewRow();
					ndr["学号"] = new ValueTag(dr["考号"].ToString(), dr);
					ndr["题号"] = "x" + (i + 1);
					Rectangle r = _artemplate.XztRect[i];
					//r.Location = _angle.GetCorrectPoint(r.X,r.Y);
					Bitmap nbmp = bmp.Clone(r, bmp.PixelFormat);
					ndr["图片"] = nbmp;
					ndr["你的答案"] = value;
					ndr["是否多选"] = false;
					ndr["是否修改"] = false;
					dt.Rows.Add(ndr);
				}
			}
		}
		public void ShowMsg(){
			string[] ss = _runmsg.Split(',');

			DataRow dr = _rundt.NewRow();
			MsgToDr(_titlepos, _xztpos, ss, ref dr);
//			dr["文件名"] = ss[0];
//			dr["校验"] = Convert.ToDouble( ss[2] );
//			dr["序号"]=_rundt.Rows.Count+1;
//			if(ss.Length>4)
//				dr["考号"] = ss[4];
//
//			string[] xx = ss[3].Split(new string[]{"|"},StringSplitOptions.RemoveEmptyEntries);
//			for(int i=0; i<xx.Length; i++)
//				dr[ "x"+(i+1)] = xx[i];
			_rundt.Rows.Add(dr);
		}
		public static List<string> NameListFromDir(string fidir,string ext =".tif" )
		{
			List<string> namelist = new List<string>();
			DirectoryInfo dirinfo = new DirectoryInfo(fidir);
			//string ext = fi.Extension;
			foreach (FileInfo f in dirinfo.GetFiles())
				if (f.Extension.ToLower() == ext)
					namelist.Add(f.FullName);
			return namelist;
		}
		private List<string> NameListFromFile(string filename)
		{
			if (File.Exists(filename))
			{
				FileInfo fi = new FileInfo(filename);
				string fidir = fi.Directory.FullName;
				return NameListFromDir(fidir);
			}
			return new List<string>();
		}
		private static string GetLastestSubDirectory(string path)
		{
			List<string> sudirects = GetLastestSubDirectorys(path);
			if (sudirects.Count > 0)
				return sudirects.Max();
			return "";
		}
		private static List<string> GetLastestSubDirectorys(string path)
		{
			Regex r = new Regex("[0-9]{8}-[0-9]+");
			if (Directory.Exists(path))
			{
				DirectoryInfo dir = new DirectoryInfo(path);
				List<string> sudirects = new List<string>();
				foreach (DirectoryInfo d in dir.GetDirectories())
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
	}
	public class ValueTag
	{
		public ValueTag(string value, Object tag)
		{
			this.Value = value;
			this.Tag = tag;
		}
		public Object Tag;
		public String Value;
		public override string ToString()
		{
			return Value;
		}
	}
	public class IntValueTag
	{
		public IntValueTag(int value, Object tag)
		{
			this.Value = value;
			this.Tag = tag;
		}
		public Object Tag;
		public int Value;
		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
