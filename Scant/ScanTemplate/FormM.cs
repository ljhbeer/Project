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
            //TODO: FormM_Load 使用类，显示相关信息
			//AutoLoadLatestImg(_workpath);
            listBox1.Items.Clear();
			foreach (string s in Tools.FileTools. GetLastestSubDirectorys(_workpath))
			{
				listBox1.Items.Add(s);
			}
			string templatepath = _workpath.Substring( 0,_workpath.LastIndexOf("\\"))+"\\Template";

            comboBoxTemplate.Items.Clear();
            foreach (string s in FileTools.NameListFromDir(templatepath, ".xml"))
			{
				string value = s.Substring(s.LastIndexOf("\\")+1);
                comboBoxTemplate.Items.Add(new ValueTag(value, s));
			}            
            string exampath = _workpath.Substring(0, _workpath.LastIndexOf("\\")) + "\\Exam";
            g_cfg.SetWorkPath(exampath);
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
            List<string> nameList = FileTools.NameListFromDir(path);
			if (nameList.Count == 0) return;
			string filename = nameList[0];
			CreateTemplate(filename);
		}
		private void ButtonUseTemplateClick(object sender, System.EventArgs e)
		{
			if (listBox1.SelectedIndex == -1) return;
			if(comboBoxTemplate.SelectedIndex==-1){
				MessageBox.Show("还未选择模板");
				return ;
			}
			
			string path = listBox1.SelectedItem.ToString();
            List<string> nameList = FileTools.NameListFromDir(path);
			if (nameList.Count == 0) return;
			string filename = nameList[0];

            string templatefile = ((ValueTag)comboBoxTemplate.SelectedItem).Tag.ToString();
			CreateTemplate(filename,templatefile);
		}
		private void ButtonScanClick(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1) return;
            if (comboBoxTemplate.SelectedIndex == -1)
            {
				MessageBox.Show("还未选择模板");
				return ;
			}
			string path = listBox1.SelectedItem.ToString();
            List<string> nameList = FileTools.NameListFromDir(path);
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
            if (!_artemplate.Dic.ContainsKey("考号") || _artemplate.Dic["考号"].Count == 0)
            {
                return;
            }
			DataTable dt = Tools.DataTableTools.ConstructDataTable(new string[] {
			                                                       	"考号",
			                                                       	"图片",
                                                                    "姓名",
			                                                       	"新考号",
			                                                       	"是否修改"
			                                                       });
            Rectangle r = _artemplate.Dic["考号"][0].ImgArea;
			foreach (DataRow dr in _rundt.Rows) {
				if (dr["考号"].ToString().Contains("-")) {
					string fn = dr["文件名"].ToString().Replace("LJH\\", "LJH\\Correct\\");
					if (File.Exists(fn)) {
						double angle = (double)(dr["校验角度"]);
						Bitmap bmp = (Bitmap)Bitmap.FromFile(fn);
						if (_angle != null)
							_angle.SetPaper(angle);
						DataRow ndr = dt.NewRow();
						ndr["考号"] = new ValueTag(dr["考号"].ToString(), dr);

						Bitmap nbmp = bmp.Clone(r, bmp.PixelFormat);
                        ndr["图片"] = nbmp;
                        ndr["姓名"] = "";
                        ndr["新考号"] = "";
						ndr["是否修改"] = false;
						dt.Rows.Add(ndr);
					}
				}
			}
			if(dt.Rows.Count>0){
				MessageBox.Show("暂未实现，待修改");
				FormVerify f = new FormVerify(dt,"考号");
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
						FormVerify f = new FormVerify(dt,"选择题");
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
            AutoDetectRectAnge.FeatureSetPath = templatename;
			bool unselected = true;
            for (int i = 0; i < comboBoxTemplate.Items.Count; i++)
            {
                if (comboBoxTemplate.Items[i].ToString().StartsWith(txt))
                {
                    comboBoxTemplate.SelectedIndex = i;
                    string templatefilename = ((ValueTag)comboBoxTemplate.SelectedItem).Tag.ToString();
					if (_artemplate == null)
						InitTemplate(templatefilename);
					InitListBoxData(templatefilename );
					unselected = false;
					break;
				}
			}
			if(unselected)
                comboBoxTemplate.SelectedIndex = -1;
		}
		private void InitTemplate(string templatefilename)
		{
			{
				Template t = new Template(templatefilename);
				if (t.Image != null)
				{
					_artemplate = t;
					List<Rectangle> listrect = new List<Rectangle>();
					foreach (Area I in t.Dic["特征点"])
					{
						listrect.Add(I.ImgArea );
					}
                    if (listrect.Count == 3)
                    {
                        AutoDetectRectAnge adr = new AutoDetectRectAnge();
                        adr.ComputTBO(listrect);
                        _angle = new AutoAngle(adr.TBO());
                    }
				}
			}
		}
		private void InitListBoxData(string templatefilename)
		{
			string dataname = templatefilename.Replace(".xml", ".txt");
			string dir = templatefilename.Substring(0, templatefilename.LastIndexOf("\\"));
            List<string> data = FileTools.NameListFromDir(dir, ".txt");
			data = data.Where(r => r.Contains(templatefilename.Replace(".xml", ""))).ToList();
			listBoxData.Items.Clear();
			foreach (string s in data)
			{
				string value = s.Substring(s.LastIndexOf("\\")+1);
				listBoxData.Items.Add(new ValueTag(value,s));
			}
		}

        private void comboBoxTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTemplate.SelectedIndex == -1) return;
            string templatefilename = ((ValueTag)comboBoxTemplate.SelectedItem).Tag.ToString();
            InitTemplate(templatefilename);
            InitListBoxData(templatefilename);
        }
        private void listBoxData_KeyUp(object sender, KeyEventArgs e)
        {
            if (listBoxData.SelectedIndex == -1) return;
            string dataname = ((ValueTag)listBoxData.SelectedItem).Tag.ToString();
            //TODO： 检测是否导入已有数据
            if (!File.Exists(dataname))
                return;
            if (e.KeyCode == Keys.H) //输出 F:\\out\\img.html
            {
                StringBuilder sb = new StringBuilder();
                string img = "<img src=\"[img]\"  /> \r\n";
                string[] ss = File.ReadAllLines(dataname);
                if (ss.Length > 1 && ss[0].StartsWith("文件名"))
                {
                    for (int i = 1; i < ss.Length; i++)
                    {
                        string f = ss[i].Substring(0, ss[i].IndexOf("."));
                        f = f.Substring(f.LastIndexOf("\\"));
                        f = f.Substring(f.IndexOf("-"));
                        f = f.Substring(f.Length - 3);
                        f = Convert.ToInt32(f).ToString()+".jpg";

                        sb.Append(img.Replace("[img]", f));
                    }
                }
                File.WriteAllText("F:\\out\\ Img.html", sb.ToString());
                MessageBox.Show(" 已生成文件 F:\\out\\ Img.html");
            }
        }
		private void listBoxData_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxData.SelectedIndex == -1) return;
			string dataname = ((ValueTag)listBoxData.SelectedItem).Tag.ToString();
			//TODO： 检测是否导入已有数据
			if (!File.Exists(dataname))
				return;
            InitExportTitleAndRunDataTable(_artemplate);
			dgv.DataSource = _rundt;
			InitDgvUI();

            string[] ls = File.ReadAllLines(dataname);
            List<string> titles = ls[0].Split(',').ToList();

            List<string> except1 = _exporttitle.Except(titles).ToList();
            List<string> except2 = titles.Except(_exporttitle).ToList();
            // 应该根据第一行标题来进行
            if (except2.Count == 0)
            {
                //if (except1.Count > 0)
                    _exporttitle = titles;
                _titlepos = ConstructTitlePos(titles);
                if (_titlepos.ContainsKey("选择题"))
                    _xztpos =_titlepos["选择题"];
                _titlepos.Remove("选择题");
			    InitDgvData(ls);
            }
            else
            {
                MessageBox.Show("模版不匹配");
            }
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
		private void CreateTemplate(string filename, string templatefilename = ""){
			Bitmap bmp = (Bitmap)Bitmap.FromFile(filename);
			MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(bmp);
			if (dr.Detected())
			{
				if(_artemplate!=null)
					_artemplate.Clear();
				
				_artemplate = new ARTemplate.Template(filename, bmp, dr.CorrectRect);
                List<Point> zeroListPoint = new List<Point>();
                for(int i=0; i<dr.ListPoint.Count; i++){
                    zeroListPoint.Add(new Point(dr.ListPoint[i].X - dr.CorrectRect.X, dr.ListPoint[i].Y - dr.CorrectRect.Y));
                }
				_angle = new AutoAngle(zeroListPoint); //或者导入时 设置
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

            InitExportTitleAndRunDataTable(_artemplate);
			dgv.DataSource = _rundt;
			InitDgvUI();
			_rundr=dr;
			_runnameList = nameList;
			
			Thread thread=new Thread(new ThreadStart(RunDetectAllImg));
			thread.Start();
		}
		public void RunDetectAllImg(){
			StringBuilder sb = new StringBuilder();
			_titlepos = ConstructTitlePos(_exporttitle);

            if (_titlepos.ContainsKey("选择题"))
                _xztpos = _titlepos["选择题"];
            _titlepos.Remove("选择题");
			foreach (string s in _runnameList)
			{
				_runmsg = DetectAllImg(_rundr, s).ToString();
				sb.Append(_runmsg);
				this.Invoke(new MyInvoke(ShowMsg));
				Thread.Sleep(10);
			}
			_exportdata = sb.ToString();
			this.Invoke(new MyInvoke(ExportData));
		}
		private StringBuilder DetectAllImg(MyDetectFeatureRectAngle dr, string s)
		{			
			Bitmap bmp = (Bitmap)Bitmap.FromFile(s);
            string str = s.Substring(s.Length - 7, 3);		
            List<Rectangle> TBO = new List<Rectangle>();
			Rectangle CorrectRect = dr.Detected(bmp,TBO);
			StringBuilder sb = new StringBuilder();

			sb.Append(s + "," +  CorrectRect.ToString("-") );// 文件名 , CorrectRect
			if (CorrectRect.Width > 0 && TBO.Count==3)
            {
                Rectangle T = TBO[0];
                Rectangle B = dr.Detected(TBO[1], bmp);
                Rectangle O = new Rectangle();
                O = dr.Detected(TBO[2], bmp);
                if (B.Width == 0)
                {
                    Rectangle R = TBO[1];
                    R.Inflate(R.Width / 2, R.Height / 5);
                    //bmpB = (Bitmap)bmp.Clone(R, bmp.PixelFormat);
                    B = dr.Detected(R, bmp);
                    if (B.Width == 0)
                    {
                        MessageBox.Show("检测特征点B失败");
                        return new StringBuilder();
                    }
                }
                Point offset = new Point(-CorrectRect.X, -CorrectRect.Y);
                T.Offset(offset);
                B.Offset(offset);
                O.Offset(offset);

                sb.Append("," + _angle.SetPaper(T.Location, B.Location, O.Location)); //校验角度
                Bitmap nbmp = (Bitmap)bmp.Clone(CorrectRect, bmp.PixelFormat);
                nbmp.Save(s.Replace("LJH\\", "LJH\\Correct\\"));

                AutoComputeXZTKH acx = new AutoComputeXZTKH(_artemplate, nbmp); 
                if (_artemplate.Dic.ContainsKey("考号") && _artemplate.Dic["考号"].Count > 0)
                {
                    KaoHaoChoiceArea kha = (KaoHaoChoiceArea)(_artemplate.Dic["考号"][0]);
                    if (kha.Type == "条形码")
                    {
                        Rectangle Ir = kha.ImgArea;
                        Bitmap barmap = (Bitmap)nbmp.Clone(kha.ImgArea, nbmp.PixelFormat);
                        //barmap.Save("f:\\aa.tif");
                        //Ir.Offset(CorrectRect.Location);
                        ZXing.Result rs = _br.Decode(barmap);
                        if (rs != null)
                        {
                            sb.Append("," + rs.Text );  //考号-条形码 姓名-未知 MsgtoDr中处理
                            if(g_cfg.Studentbases.HasStudentBase)
                                sb.Append( ","+g_cfg.Studentbases.GetName( Convert.ToInt32(rs.Text)));
                            else
                                sb.Append(",-");
                        }
                    }
                    else if ("1023456789".Contains(kha.Type))
                    {
                        string kh = acx.ComputeKH(kha, _angle);
                        if(kh.Contains("-"))
                            sb.Append("," + kh+",-");   //考号-涂卡 姓名-未知 MsgtoDr中处理
                        else
                        {
                             if(g_cfg.Studentbases.HasStudentBase)
                                sb.Append( "," + kh+","+g_cfg.Studentbases.GetName( Convert.ToInt32(kh)));
                            else
                                sb.Append("," + kh+",-");
                        }
                    }
                }

                sb.Append(","+acx.ComputeXZT(str,_angle)); //选择题

                //计算座位号
                if (_artemplate.Dic.ContainsKey("自定义") && _artemplate.Dic["自定义"].Count > 0)
                {
                    StringBuilder tsb = new StringBuilder();
                    foreach (Area I in _artemplate.Dic["自定义"])
                    {
                        CustomArea ca = (CustomArea)I;
                        if ("1023456789".Contains(ca.Type))
                        {
                            AutoComputeXZTKH acxzdy = new AutoComputeXZTKH(_artemplate, nbmp);
                            //sb.Append("," + acx.ComputeCustomDF(ca, _angle, nbmp));
                            tsb.Append(acx.ComputeCustomDF(ca, _angle) + "|");
                        }
                    }
                    sb.Append("," + tsb); //自定义
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
                else if(dc.Name == "序号")
                    dc.Width = 30;
                else
                    dc.Width = 60;
		}
        private void InitDgvData( string[] ls)
        {
            for (int i = 1; i < ls.Length; i++)
            {
                string[] ss = ls[i].Split(',');
                DataRow dr = _rundt.NewRow();

                MsgToDr(_titlepos,_exporttitle, _xztpos, ss, ref dr);
                _rundt.Rows.Add(dr);
            }
        }
        private void InitExportTitleAndRunDataTable(Template t)
        {
            List<string> colnames = new List<string> { "序号"};          
            _exporttitle = TitlesFromTemplate(t); //not contains 非选择题
            colnames.AddRange(_exporttitle);
            if (colnames.Contains("选择题"))
            {
                colnames.Remove("选择题");
                for (int i = 0; i <t.XztRect.Count; i++)
                    colnames.Add("x" + (i + 1));
            }
            _rundt = Tools.DataTableTools.ConstructDataTable(colnames.ToArray());
        }
        private List<string> TitlesFromTemplate(Template t)
        {
            if (t == null)
                return null;
            List<string> titles = new List<string>();
            titles.Clear();
            titles.Add("文件名");
            titles.Add("CorrectRect");
            titles.Add("校验角度");
            string item = "考号";
            if (t.Dic.ContainsKey(item) && t.Dic[item].Count > 0)
            {
                titles.Add(item);
                titles.Add("姓名");
            }
            //item = "非选择题";
            //if (t.Dic.ContainsKey(item) && t.Dic[item].Count > 0)
            //    titles.Add(item);

            item = "选择题";
            if (t.Dic.ContainsKey(item) && t.Dic[item].Count > 0)
                titles.Add(item);

            item = "自定义";
            if (t.Dic.ContainsKey(item) && t.Dic[item].Count > 0)
                titles.Add(item);
            return titles;
        }		
		private Dictionary<string, int> ConstructTitlePos(List<string> Titles)
		{
			Dictionary<string, int> titlepos = new Dictionary<string, int>();
            for (int i = 0; i < Titles.Count; i++)
            {
                titlepos[Titles[i]] = i;
			}
			return titlepos;
		}
		private void MsgToDr(Dictionary<string, int> titlepos, List<string> Titles, int xztpos, string[] ss, ref DataRow dr)
		{
			dr["序号"] = _rundt.Rows.Count + 1;
			if (ss.Length == Titles.Count) {
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
                    string xuehao = "";
                    if(dr.Table.Columns.Contains("考号"))
                        xuehao =dr["考号"].ToString();
                    ndr["学号"] = new ValueTag(xuehao, dr);
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
			string[] ss = _runmsg.Trim().Split(',');

			DataRow dr = _rundt.NewRow();
			MsgToDr(_titlepos,_exporttitle, _xztpos, ss, ref dr);
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
    public class Config
    {
        public Config()
        {
            _examinfo = new List<ExamInfo>();
        }
        public  void SetWorkPath(string exampath)
        {
            this._workpath = exampath;
            Studentbases= new StudentBases(StudentBaseFileName());
            LoadConfig();
        }
        private void LoadConfig( )
        {
            string filename = _workpath + "\\config.json";
            if (File.Exists( filename))
            {
                this._filename = filename;
                Config f = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(_filename));
                _examinfo = f._examinfo;
                //_workpath = f._workpath;
            }
        }
        public void SaveConfig(string filename = "")
        {
            string fn = _filename;
            if (filename != "")
                fn = filename;
            if (fn != "")
            {
                if (!fn.Contains(":"))
                    fn = _workpath+"\\" + fn;
                _filename = fn;
                string str = Tools.JsonFormatTool.ConvertJsonString(Newtonsoft.Json.JsonConvert.SerializeObject(this));
                File.WriteAllText(_filename, str);
            }
        }
        public bool CheckExamInfoName(ExamInfo ei)
        {
            if( !_examinfo.Exists( r => r.Name == ei.Name))
               //|| !_examinfo.Exists( r=> r.Path == ei.Path) )
            {
                ei.Number = 1001;
                if(_examinfo.Count>0) 
                    ei.Number = _examinfo.Max(r => r.Number) + 1;
                ei.Path = _workpath + "\\" + ei.Number+"\\";
                return true;
            }
            return false;
        }
        public void AddExamInfo(ExamInfo ei)
        {
            _examinfo.Add(ei);
        }
        public string ExamPath
        {
            get { return _workpath; }
        }
        public List<ExamInfo> _examinfo;
        private string _filename;
        private string _workpath;

        private  string StudentBaseFileName()
        {
            return _workpath.Replace("Exam" , "StudentBaseList.txt");
        }

        public StudentBases Studentbases { get; set; }
    }
    public class ExamInfo
    {
        public string Name;
        public string Path;
        public int Number;
        public string TemplateFileName;
        public override string ToString()
        {
            return Name + "_" + Number + "_" + Path; ;
        }
    }
}
