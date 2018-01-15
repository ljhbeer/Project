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
        private ScanConfig _sc;
        private DataTable _rundt;
        private Scan _scan;
        private bool _bReScan;
        private bool _bSingleTestScan;
		public FormM()
		{
			InitializeComponent();
            _bReScan = false;
            _rundt = null;
		}
		private void FormM_Load(object sender, EventArgs e)
		{            
            if ( Directory.Exists(textBoxWorkPath.Text))
            {
                _sc = new ScanConfig(textBoxWorkPath.Text);
                listBoxUnScanDir.Items.Clear();
                comboBoxTemplate.Items.Clear();
                listBoxScantData.Items.Clear();
                listBoxUnScanDir.Items.AddRange(_sc.Unscans.Unscans.ToArray());
                comboBoxTemplate.Items.AddRange(_sc.CommonTemplates.CommonTemplates.ToArray());
                listBoxScantData.Items.AddRange(_sc.Scandatas.Scandatas.ToArray());
                //foreach (ScanData sd in _sc.Scandatas.Scandatas)
                //{
                //    string path = sd.Fullpath;
                //    if (!File.Exists(path + "\\desktop.ini"))
                //        global.SaveDirectoryMemo(path,  sd.ExamName);
                //}
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
                _sc.Templateshow = new TemplateShow(dir.FullPath, dir.DirName, nameList[0]);
                _sc.Templateshow.Template.FileName = _sc.Baseconfig.TemplatePath ;
                if (_sc.Templateshow.OK)
                {
                    this.Hide();
                    FormTemplate f = new FormTemplate(_sc.Templateshow);
                    f.ShowDialog();
                    f.Clear();
                    f = null;
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
                _sc.Templateshow = new TemplateShow(dir.FullPath, dir.DirName, nameList[0], ti,true);
                if (_sc.Templateshow.OK)
                {
                    this.Hide();
                    FormTemplate f =new FormTemplate(_sc.Templateshow);
                    f.ShowDialog();
                    f.Clear();
                    f = null;
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
            InitDoScan();
		}
        private void InitDoScan(string filename = "")
        {
            TemplateInfo ti = (TemplateInfo)comboBoxTemplate.SelectedItem;
            UnScan dir = (UnScan)listBoxUnScanDir.SelectedItem;
            List<string> nameList = dir.ImgList();
            if (nameList.Count > 0)
            {
                //TODO: add Detect
                _bReScan = false;
                _bSingleTestScan = false;
                if(filename!="" && File.Exists(filename)){
                    nameList.Clear();
                    nameList.Add(filename);
                    _bSingleTestScan = true;
                }
                _scan = new Scan(_sc, ti.TemplateFileName, nameList, dir.FullPath);
                _rundt = Tools.DataTableTools.ConstructDataTable(_scan.ColNames.ToArray());
                dgv.DataSource = _rundt;
                InitDgvUI();
                _scan.DgSaveScanData = new DelegateSaveScanData(ExportData);
                _scan.DgShowScanMsg = new DelegateShowScanMsg(ShowMsg);
                _scan.DoScan();
            }
        }
        private void buttonReScan_Click(object sender, EventArgs e)
        {
            /////// ReScan
            if (_scan == null || _rundt == null || _rundt.Rows.Count == 0 || listBoxScantData.SelectedIndex == -1)
                return;
            InitReDoScan();
        }
        private void InitReDoScan(string filename = "")
        {
            ScanData sd = (ScanData)listBoxScantData.SelectedItem;
            TemplateInfo ti = new TemplateInfo(sd.TemplateFileName, sd.Fullpath);
            List<string> nameList = sd.ImgList;
            if (nameList.Count > 0)
            {//TODO: add Detect
                _bReScan =true;
                _bSingleTestScan = false;
                if (filename != "" && File.Exists(filename))
                {
                    nameList.Clear();
                    nameList.Add(filename);
                    _bSingleTestScan = true;
                }

                _scan = new Scan(_sc, ti.TemplateFileName, nameList, sd.Fullpath);
                _rundt = Tools.DataTableTools.ConstructDataTable(_scan.ColNames.ToArray());
                dgv.DataSource = _rundt;
                InitDgvUI();
                _scan.DgSaveScanData = new DelegateSaveScanData(ExportData);
                _scan.DgShowScanMsg = new DelegateShowScanMsg(ShowMsg);
                _scan.DoScan();
            }
        }
        private void buttonOutTextImage_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormOutTxtToImg f = new FormOutTxtToImg( _sc );
            f.ShowDialog();

            this.Show();
        }
        private void buttonPreDealImage_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormPreDeal f = new FormPreDeal(_sc);
            f.ShowDialog();
            f.Clear();
            f = null;
            this.Show();
        }
		private void listBoxData_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxScantData.SelectedIndex == -1) return;
            ScanData sd = (ScanData)listBoxScantData.SelectedItem;
            if (File.Exists(sd.DataFullName))
            {
                string[] ls = File.ReadAllLines(sd.DataFullName);
                List<string> titles = ls[0].Split(',').ToList();

                if(_scan!=null){
                    _scan.Clear();
                }
                _scan = new Scan(_sc,sd.TemplateFileName,sd.ImgList,sd.Fullpath,false);
                _rundt = Tools.DataTableTools.ConstructDataTable(_scan.ColNames.ToArray());
                dgv.DataSource = _rundt;
                InitDgvUI();
                InitDgvData(ls);
            }
            else
            {
                MessageBox.Show("没有发现扫描数据");
            }
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
            if (e.RowIndex == -1  || e.ColumnIndex == -1 )
                return;
            if (checkBoxShowUnScanNamelist.Checked)
            {
                if (dgv.Columns.Contains("FileName"))
                {
                    string filename = dgv.Rows[e.RowIndex].Cells["FileName"].Value.ToString();
                    if (listBoxUnScanDir.SelectedIndex == -1 || comboBoxTemplate.SelectedIndex == -1)
                        return;
                    InitDoScan(filename);
                    return;
                }
            }

            if (e.RowIndex == -1 || _rundt == null || e.ColumnIndex == -1 || _scan == null)
                return;
            if (dgv.Columns.Contains("文件名") && dgv.Columns[e.ColumnIndex].Name == "文件名" && checkBoxReSingleScan.Checked)
            {
                string filename = dgv.Rows[e.RowIndex].Cells["文件名"].Value.ToString();
                InitReDoScan(filename);
                return;
            }
            string fn = _rundt.Rows[e.RowIndex]["文件名"].ToString().Replace("LJH\\", "LJH\\Correct\\").Replace("\\img","");
            if (File.Exists(fn))
            {
                double angle = (double)(_rundt.Rows[e.RowIndex]["校验角度"]);
                Bitmap bmp = (Bitmap)Bitmap.FromFile(fn);

                AutoAngle Angle = _scan.Template.Angle;
                if (Angle != null)
                    Angle.SetPaper(angle);
                pictureBox1.Image = ARTemplate.TemplateTools.DrawInfoBmp(bmp, _scan.Template, Angle);
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
        private void InitDgvData(string[] ls)
        {
            for (int i = 1; i < ls.Length; i++)
            {
                string[] ss = ls[i].Split(',');
                DataRow dr = _rundt.NewRow();

                MsgToDr( ss, ref dr); 
                _rundt.Rows.Add(dr);
            }
            _rundt.AcceptChanges();
        }
        public void ShowMsg(string  msg)
        {
            string[] ss = msg.Trim().Split(',');
            DataRow dr = _rundt.NewRow();
            MsgToDr(ss, ref dr);
            _rundt.Rows.Add(dr);
            this.Invoke(new MyInvoke(MyRefreshDgv));
        }
        private void ExportData(string exportdata)
        {
            if (_bSingleTestScan)
            {
                if (_bReScan)
                {
                    _bReScan = false;
                }
              //
                {
                    _bSingleTestScan = false;
                    this.Invoke(new MyInvoke(MyRefreshDgvAll));
                }
            }else if (_bReScan)
            {
                _bReScan = false;
                string Datafilename = _scan.ScanDataPath + "\\data.txt";
                File.WriteAllText(Datafilename, string.Join(",", _scan.ExportTitles) + "\r\n" + exportdata);
                this.Invoke(new MyInvoke(MyRefresh));
            }
            else
            {
                if (InputBox.Input("考试名称"))
                {
                    string examname = InputBox.strValue;
                    string Datafilename = _scan.ScanDataPath + "\\data.txt";
                    string NewTemplatename = _scan.ScanDataPath + "\\template.json";
                    string NewImgsPath = _scan.ScanDataPath + "\\img";
                    if (!Directory.Exists(_scan.ScanDataPath))    //文件是否被使用
                        Directory.CreateDirectory(_scan.ScanDataPath);
                    Directory.Move(_scan.SourcePath, NewImgsPath);
                    File.Copy(_scan.TemplateName, NewTemplatename, true);
                    File.WriteAllText(_scan.ScanDataPath + "\\" + examname + ".exam", examname);
                    exportdata = exportdata.Replace(_scan.SourcePath, _scan.ScanDataPath + "\\img");
                    File.WriteAllText(Datafilename, string.Join(",", _scan.ExportTitles) + "\r\n" + exportdata);
                    this.Invoke(new MyInvoke(MyRefresh));
                    global.SaveDirectoryMemo(_scan.ScanDataPath,examname);
                }
            }
            if (_scan != null && _scan.Msg != "")
            {
                MessageBox.Show("未扫描名单\r\n" + _scan.Msg);
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
            Template _artemplate = _scan.Template;
            AutoAngle _angle = _scan.Angle;
            double angle = (double)(dr["校验角度"]);
            if (_angle != null)
                _angle.SetPaper(angle);
            for (int i = 0; i < _artemplate.Manageareas.SinglechoiceAreas.Count; i++)
            {
                string value = dr["x" + (i + 1)].ToString();
                if (value.Length != 1 || !"ABCD".Contains(value))
                {
                    DataRow ndr = dt.NewRow();
                    string xuehao = "";
                    if (dr.Table.Columns.Contains("考号"))
                        xuehao = dr["考号"].ToString();
                    ndr["学号"] = new ValueTag(xuehao, dr);
                    ndr["题号"] = "x" + (i + 1);
                    Rectangle r = _artemplate.Manageareas.SinglechoiceAreas.SingleRectangle(i);
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
        public void MyRefresh()
        {
            buttonRefresh.PerformClick();
        }
        public void MyRefreshDgvAll()
        {
            dgv.Invalidate();
        }
        public void MyRefreshDgv()
        {
            //cnt = dgv.RowCount;
            int cnt = _rundt.Rows.Count;
            dgv.InvalidateRow(cnt - 1);
            //dgv.FirstDisplayedScrollingRowIndex = cnt - 1;
            textBoxMsg.Text = "扫描第" + cnt + "号， 考号：" + _rundt.Rows[cnt - 1]["考号"] + " 姓名：" + _rundt.Rows[cnt - 1]["姓名"];
        }
        private void buttonVerify_Click(object sender, EventArgs e)
        {
            if (_scan == null || _rundt == null || _rundt.Rows.Count == 0)
                return;
            this.Hide();
            VerifyKaoHao();
            VerifyXzt();
            this.Show();
        }
        private void buttonVerifyname_Click(object sender, EventArgs e)
        {
            if (_scan == null || _rundt == null || _rundt.Rows.Count == 0)
                return;
            this.Hide();
            VerifyName();
            this.Show();
        }
        private void buttonCreateYJData_Click(object sender, EventArgs e)
        {
            if (_scan == null || _rundt == null || _rundt.Rows.Count == 0 || listBoxScantData.SelectedIndex==-1)
                return;
            this.Hide();
            _sc.Examconfig = new ExamConfig();
            _sc.Examconfig.SetWorkPath(_sc.Baseconfig.ExamPath);
            ScanData sd = (ScanData)listBoxScantData.SelectedItem;
            FormYJ.FormYJInit f = new FormYJ.FormYJInit(_sc.Examconfig, _scan.Template, _rundt,_sc.Baseconfig.ScanDataPath,sd.ExamName,sd.Fullpath);
            f.ShowDialog();
            this.Show();
        }
        private void buttonOpenTemplate_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormTemplate().ShowDialog();
            this.Show();
        }
        private void checkBoxDebug_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxDebug.Checked)
               global.Debug = true;
            else
                global.Debug = false;;
        }
        private void VerifyName()
        {
            Template _artemplate = _scan.Template;
            AutoAngle _angle = _scan.Angle;
            if (!_artemplate.Dic.ContainsKey("考号") || _artemplate.Dic["考号"].Count == 0)
            {
                return;
            }
            DataTable dt = Tools.DataTableTools.ConstructDataTable(new string[] {
			                                                       	"OID考号",			                                                  
			                                                       	"图片姓名",
                                                                    "姓名",
			                                                       	"新考号",
			                                                       	"是否修改"
			                                                       });
            Rectangle r = _artemplate.Dic["考号"][0].ImgArea;
            Rectangle rxm = new Rectangle();
            if (_artemplate.Dic["校对"].Count > 0)
            {
                foreach (Area I in _artemplate.Dic["校对"])
                {
                    if (I.ToString().Contains("姓名"))
                    {
                        rxm = I.Rect;
                    }
                }
            }
            foreach (DataRow dr in _rundt.Rows)
            {
                if (!dr["考号"].ToString().Contains("-"))
                {
                    string fn = dr["文件名"].ToString().Replace("LJH\\", "LJH\\Correct\\").Replace("\\img","");
                    if (File.Exists(fn))
                    {
                        double angle = (double)(dr["校验角度"]);
                        Bitmap bmp = (Bitmap)Bitmap.FromFile(fn);
                        if (_angle != null)
                            _angle.SetPaper(angle);
                        DataRow ndr = dt.NewRow();
                        ndr["OID考号"] = new ValueTag(dr["考号"].ToString(), dr);
                        int kh = Convert.ToInt32(dr["考号"].ToString());
                        if (_sc.Studentbases.HasStudentBase)
                        {
                            string name = _sc.Studentbases.GetName(kh);
                            ndr["姓名"] = name;
                        }
                        ndr["是否修改"] = false;
                        if (rxm.Width > 0)
                        {
                            ndr["图片姓名"] = bmp.Clone(rxm, bmp.PixelFormat);
                        }
                        dt.Rows.Add(ndr);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                //MessageBox.Show("暂未实现，待修改");
                FormVerify f = new FormVerify(_sc,dt, "核对姓名");
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    MessageBox.Show("未校验");
                }
                else
                {
                    if (f.Changed)
                    {
                        string filename = ((ValueTag)listBoxScantData.SelectedItem).Tag.ToString();
                        string[] ss = File.ReadAllLines(filename);
                        try
                        {
                            for (int i = 1; i < ss.Length; i++)
                            {
                                string[] item = ss[i].Split(',');
                                if (item[3].Contains("-"))
                                {
                                    DataRow[] drs = _rundt.Select("文件名='" + item[0] + "'");
                                    if (drs.Length == 1)
                                    {
                                        item[3] = drs[0]["考号"].ToString();
                                        item[4] = drs[0]["姓名"].ToString();
                                        ss[i] = string.Join(",", item);
                                    }
                                }
                            }
                            //File.WriteAllText(filename + "_1", string.Join("\r\n", ss));
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("未保存更改，因为" + ee.Message);
                        }
                    }
                }
                //修改之后
                dt.Rows.Clear();
            }
        }
        private void VerifyKaoHao()
        {
            Template _artemplate = _scan.Template;
            AutoAngle _angle = _scan.Angle;
            if (!_artemplate.Dic.ContainsKey("考号") || _artemplate.Dic["考号"].Count == 0)
            {
                return;
            }
            DataTable dt = Tools.DataTableTools.ConstructDataTable(new string[] {
			                                                       	"OID考号",
			                                                       	"图片",
			                                                       	"图片姓名",
			                                                       	"图片考号",
                                                                    "姓名",
			                                                       	"新考号",
			                                                       	"是否修改"
			                                                       });
            Rectangle r = _artemplate.Dic["考号"][0].ImgArea;
            Rectangle rxm = new Rectangle();
            Rectangle rkh = new Rectangle();
            if (_artemplate.Dic["校对"].Count > 0)
            {
                foreach (Area I in _artemplate.Dic["校对"])
                {
                    if (I.ToString().Contains("姓名"))
                    {
                        rxm = I.Rect;
                    }
                    else
                        if (I.ToString().Contains("学号"))
                        {
                            rkh = I.Rect;
                        }
                }
            }
            foreach (DataRow dr in _rundt.Rows)
            {
                string skh = dr["考号"].ToString();
                string name = dr["姓名"].ToString();
                int kh = -1;
                if (!skh.Contains("-"))
                    kh = Convert.ToInt32(skh);
                if ( kh<0 || !_sc.Studentbases.ContainsKey(kh) || name.Contains("-") || name.Trim()==""  )
                {
                    string fn = dr["文件名"].ToString().Replace("LJH\\", "LJH\\Correct\\").Replace("\\img","");
                    if (File.Exists(fn))
                    {
                        double angle = (double)(dr["校验角度"]);
                        Bitmap bmp = (Bitmap)Bitmap.FromFile(fn);
                        if (_angle != null)
                            _angle.SetPaper(angle);
                        DataRow ndr = dt.NewRow();
                        ndr["OID考号"] = new ValueTag(dr["考号"].ToString(), dr);
                        if (r.Width > 0)
                        {
                            ndr["图片"] = bmp.Clone(r, bmp.PixelFormat);
                        }
                        ndr["姓名"] = "";
                        ndr["新考号"] = "";
                        ndr["是否修改"] = false;

                        if (kh > 0 && name.Contains("-"))
                        {
                            dr["姓名"] = ndr["姓名"] = _sc.Studentbases.GetName(kh);
                            ndr["新考号"] = skh;
                            ndr["是否修改"] = true;
                            //ndr.SetModified();
                        }
                            

                        if (rxm.Width > 0)
                        {
                            ndr["图片姓名"] = bmp.Clone(rxm, bmp.PixelFormat);
                        }
                        if (rkh.Width > 0)
                        {
                            ndr["图片考号"] = bmp.Clone(rkh, bmp.PixelFormat);
                        }
                        dt.Rows.Add(ndr);
                        //if (kh > 0 && name.Contains("-"))
                        //{
                        //    dt.Rows[dt.Rows.Count - 1].SetModified();
                        //}
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                FormVerify f = new FormVerify(_sc, dt, "考号");
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    MessageBox.Show("未校验");
                }
                else
                {
                    if (f.Changed)
                    {
                        
                        ScanData sd = (ScanData)listBoxScantData.SelectedItem;
                        string[] ss = File.ReadAllLines(sd.DataFullName);// sd.ImgList.ToArray();
                        //TODO: 应避免出现空行
                        try
                        {
                            for (int i = 1; i < ss.Length; i++)
                            {
                                string[] item = ss[i].Split(',');
                                string skh =  item[3];
                                string name = item[4];
                                int kh = -1;
                                if (!skh.Contains("-"))
                                    kh = Convert.ToInt32(skh);

                                if ( kh<0 || !_sc.Studentbases.ContainsKey(kh) || name.Contains("-") || name.Trim()==""  )
                                {
                                    DataRow[] drs = _rundt.Select("文件名='" + item[0] + "'");
                                    if (drs.Length == 1)
                                    {
                                        item[3] = drs[0]["考号"].ToString();
                                        item[4] = drs[0]["姓名"].ToString();
                                        ss[i] = string.Join(",", item);
                                    }
                                }
                            }
                            File.WriteAllText(sd.DataFullName, string.Join("\r\n", ss));
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("未保存更改，因为" + ee.Message);
                        }
                    }
                }
                //修改之后
                dt.Rows.Clear();
            }
        }
        private void VerifyXzt()
        {
            Template _artemplate = _scan.Template;
            AutoAngle _angle = _scan.Angle;
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
            int xztcnt = _artemplate.Manageareas.SinglechoiceAreas.Count;
            int runcnt = 0;
            foreach (DataRow dr in _rundt.Rows)
            {
                runcnt++;
                bool b = false;
                int xi = 0;
                for (; xi < xztcnt; xi++)
                {
                    if (!"ABCD".Contains(dr["x" + (xi + 1)].ToString()) || dr["x" + (xi + 1)].ToString().Length > 1)
                    {
                        b = true;
                        break;
                    }
                }
                if (b)
                {
                    string fn = dr["文件名"].ToString().Replace("LJH\\", "LJH\\Correct\\").Replace("\\img", ""); ;
                    if (File.Exists(fn))
                    {
                        double angle = (double)(dr["校验角度"]);
                        Bitmap bmp = (Bitmap)Bitmap.FromFile(fn);
                        if (_angle != null)
                            _angle.SetPaper(angle);
                        AddDataToDt(dr, bmp, dt);
                    }
                }
                if (b || runcnt == _rundt.Rows.Count)
                {
                    if (dt.Rows.Count > 20 || (runcnt == _rundt.Rows.Count && dt.Rows.Count > 0))
                    {
                        FormVerify f = new FormVerify(_sc, dt, "选择题");
                        if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            MessageBox.Show("校验失败");
                        }
                        //修改之后
                        else
                        {
                            if (f.Changed)
                            {

                                bool bsave = false;
                                ScanData sd = (ScanData)listBoxScantData.SelectedItem;
                                string[] ss = File.ReadAllLines(sd.DataFullName);// sd.ImgList.ToArray();
                                //TODO: 应避免出现空行
                                try
                                {
                                    for (int i = 1; i < ss.Length; i++)
                                    {
                                        string[] item = ss[i].Split(',');
                                        string skh = item[3];
                                        string name = item[4];
                                        int kh = -1;
                                        if (!skh.Contains("-"))
                                            kh = Convert.ToInt32(skh);
                                        string xzt = item[5];
                                        if(xzt.Contains("-|") || xzt.Length != xztcnt*2)
                                        {
                                            DataRow[] drs = _rundt.Select("文件名='" + item[0] + "'");
                                            if (drs.Length == 1 && drs[0].RowState == DataRowState.Modified)
                                            {
                                                string strxzt = "";
                                                for (int k = 0; k < xztcnt; k++)
                                                {
                                                    strxzt += drs[0]["x" + (k + 1)]+"|";
                                                }

                                                if (strxzt != xzt && strxzt.Length >= xztcnt*2)
                                                {
                                                    item[5] = strxzt;
                                                    ss[i] = string.Join(",", item);
                                                    bsave = true;
                                                }
                                            }
                                        }
                                    }
                                    if(bsave)
                                    File.WriteAllText(sd.DataFullName, string.Join("\r\n", ss));
                                }
                                catch (Exception ee)
                                {
                                    MessageBox.Show("未保存更改，因为" + ee.Message);
                                }
                            }
                        }
                        dt.Rows.Clear();
                    }
                }
            }
        }

        private void buttonUnScanNameList_Click(object sender, EventArgs e)
        {
            if (_scan == null || _rundt == null || _rundt.Rows.Count == 0)
                return;
            List<Student> students = new List<Student>();
            List<StudentBase> studentbases = new List<StudentBase>();
           
             foreach (DataRow dr in _rundt.Rows)
            {
                string skh = dr["考号"].ToString();
                int kh = -1;
                if (!skh.Contains("-"))
                    kh = Convert.ToInt32(skh);
                if ( kh>=0  )
                {
                    studentbases.Add(  _sc.Studentbases.GetStudent(kh));
                }
             }
             List<int> Lclassid =
             studentbases.Select(r => r.Classid ).Distinct().ToList();
             Lclassid.Remove(0);
             if (Lclassid.Count > 1)
                 return;
             int cid = Lclassid[0];
             string str = string.Join("\r\n",
             _sc.Studentbases.GetClassStudent(cid).Where(r => ! studentbases.Exists(rr => rr.KH == r.KH))
                 .Select(r1 => r1.Name).ToList());

             MessageBox.Show("导出未交名单");
             SaveFileDialog saveFileDialog2 = new SaveFileDialog();
             saveFileDialog2.FileName = "_未交名单";
             saveFileDialog2.Filter = "txt files (*.txt)|*.txt";
             saveFileDialog2.Title = "导出未交名单";
             if (saveFileDialog2.ShowDialog() == DialogResult.OK)
             {
                 File.WriteAllText(saveFileDialog2.FileName, str);
             }
        }
        private void listBoxScantData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                if (listBoxScantData.SelectedIndex == -1) return;
                ScanData sd = (ScanData)listBoxScantData.SelectedItem;
                if (File.Exists(sd.DataFullName))
                {
                    string[] ls = File.ReadAllLines(sd.DataFullName);
                    List<string> titles = ls[0].Split(',').ToList();

                    Scan  _scan = new Scan(_sc, sd.TemplateFileName, sd.ImgList, sd.Fullpath, false);
                    FileInfo fi = new FileInfo(sd.TemplateFileName);
                    string path= fi.FullName.Substring(0, fi.FullName.Length - fi.Name.Length-1);
                    TemplateInfo ti = new TemplateInfo(fi.FullName ,path);
                    _sc.Templateshow = new TemplateShow("", "", sd.ImgList[0], ti,true);
                    if (_sc.Templateshow.OK)
                    {
                        this.Hide();
                        FormTemplate f = new FormTemplate(_sc.Templateshow);
                        f.ShowDialog();
                        f.Clear();
                        f = null;
                        this.Show();
                    }
                }
            }
        }
        private void listBoxUnScanDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBoxShowUnScanNamelist.Checked)
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
                    List<nameobj> ls = nameList.Select(r => new nameobj(r)).ToList();
                    dgv.DataSource = ls;
                }
            }
        }


        private class nameobj
        {
            public nameobj(string s)
            {
                this.FileName = s;
                this.Length = s.Length;
            }
            public string FileName { get; set; }
            public int Length { get; set; }
        }
	}	
}
