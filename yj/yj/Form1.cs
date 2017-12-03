using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using yj.formimg;

namespace yj
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitDatabase();
            ShowFloor();
            _activefloorid = -1;
            _workpath = "E:\\Project\\SWAR\\back\\C_IMAGES\\";
            _imgpathtemplate = _workpath + "[id]_00_1_p1.TIF";
            textBoxWorkPath.Text = _workpath;  
	        splitContainer2.Panel1Collapsed = true;
            _dtshow = null;
            _drlist = new List<DataRow>();
            loadbmpdata = null;
        }
        private void Form1Load(object sender, EventArgs e)
        {
        	if(File.Exists("cfg.ini"))
	        	InitYJ();
        }
        private void InitYJ(){
        	string s="";
            string path = "";
            string[] ss= File.ReadAllLines("cfg.ini");
            foreach(string str in ss)
                if (str.Trim().StartsWith("selectindex="))
                {
                    s = str;
                    break;
                }
            foreach (string str in ss)
                if (str.Trim().StartsWith("workpath="))
                {
                    path = str.Substring("workpath=".Length) ;
                    break;
                }
            if (path != "" && Directory.Exists(path))
            {
                if (!path.EndsWith("\\")) 
                    path+="\\";
                _imgpathtemplate = path + "[id]_00_1_p1.TIF";
                _workpath = path;

                textBoxWorkPath.Text = _workpath;
            }
        	if(s.StartsWith("selectindex=")){
        		try{
        			int index = Convert.ToInt32( s.Substring( "selectindex=".Length));
        			if(index>=0 && dgv.Rows.Count>index){
        				dgv.Focus();
        				dgv.Rows[index].Selected = true;
        				dgv.CurrentCell = dgv.Rows[index].Cells[0];  
        				ButtonBeginYJClick(null,null);
        			}
        		}catch{}
        	}
        }
        private bool InitDatabase()
        {
            while (true)
            {
                _dbfilename = "data\\yj.mdb";
                if (!File.Exists(_dbfilename))
                {
                    MessageBox.Show("数据库" + _dbfilename + "不存在");
                    if (MessageBox.Show("重新选择数据库", "重新选择数据库", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                        return false;
                    OpenFileDialog fd = new OpenFileDialog();
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        this._dbfilename = fd.FileName;
                    }
                }
                try
                {
                    if (_db != null)
                    {
                        _db.connClose();
                        _db = null;
                    }
                    FileInfo fi = new FileInfo(_dbfilename);
                    _db = new Db.ConnDb(fi.FullName);
//                    RefreshDatabasedata();
                    _db.connClose();
                    this.textBoxShow.Text = "当前数据数据库：" + _dbfilename;
                    break;
                }
                catch
                {
                    MessageBox.Show("数据库：" +  _dbfilename +" 格式不正确");
                    _dbfilename = "";
                }
            }
            return true;
        }
        private void ShowFloor()
        {
            string sql = "select * from floor";
            DataTable dt = _db.query(sql).Tables[0];
            dgv.DataSource = dt;
        }
        private void buttonInitData_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null)
            {
                int index = dgv.CurrentRow.Index;
                int FloorID = (int)(dgv.CurrentRow.Cells["ID"].Value);
                string Name = (string)(dgv.CurrentRow.Cells["floorname"].Value);
                string ImgPath = (string)(dgv.CurrentRow.Cells["path"].Value);
                TableTools tt = new TableTools(_db,FloorID.ToString(),Name);
                MessageBox.Show("请导入 扫描的文本数据 ");
                if(! tt.ImportScanData() ) return;
            }
        }

        private void buttonCreateEditFloor_Click(object sender, EventArgs e)
        {
            this.Hide();
            //TODO: 做一些收尾工作
            if (_db == null)
                return;
            FormCreateTemplate f = new FormCreateTemplate(_db,_workpath);
            f.ShowDialog();
            ShowFloor();
            this.Show();
            //TODO:  初始化一些操作
        }        
        private void ButtonBeginYJClick(object sender, EventArgs e)
        {
        	//初始化  非选择题 的 分值和名称
        	//update room  set roomname = "tk"+trim(id), maxscore = 10
        	 if (dgv.CurrentRow != null)
            {
                int index = dgv.CurrentRow.Index;
                int FloorID = (int)(dgv.CurrentRow.Cells["ID"].Value);
                string Name = (string)(dgv.CurrentRow.Cells["floorname"].Value);
                _ImgPath = (string)(dgv.CurrentRow.Cells["path"].Value);

//                TableTools tt = new TableTools(db,FloorID.ToString(),Name);
//                MessageBox.Show("请导入 扫描的文本数据 ");
//                if(! tt.ImportScanData() ) return;
//				if( 没有初始化 ） return;
                tableLayoutPanel2.Visible = true;
	        	splitContainer2.Panel1Collapsed = true;
	        	dgvs.Visible = false;
	        	this._activefloorid = FloorID;
	        	Init(FloorID);
            }
        }       
        private void buttonok_Click(object sender, EventArgs e)
        {
            string fs = textBoxFenshu.Text;
            try
            {
               
                int result = Convert.ToInt32(fs);
                if (result >= 0 && result <= _activesj.MaxResult)
                {
                    string sql = "update subjectscore_[floorid] set tk[subid] = [score] where kh = [kh]"
                    	.Replace("[floorid]",_activefloorid.ToString());
                    sql = sql.Replace("[score]", result.ToString())
                    	.Replace("[kh]", _activekh)
                    	.Replace("[subid]",_activesj.Subid.ToString());
                    
                    if (_db.update(sql) == 1)
                    {
                        _activedt.Rows.RemoveAt(0);
                        _done.Add(_activekh);
                        textBoxShow.Text = "本题未完成阅卷份数" + _activedt.Rows.Count + " 满分为" + _activesj.MaxResult + "分";
                        pictureBox1.Image = null;
                        pictureBox1.Invalidate();
                        if (checkBoxautoLoadNext.Checked)
                            YueJuan();
                        //this.Invalidate();

                    }
                }
                else
                {
                    _boutarea = true;
                    MessageBox.Show("已超过最大分值,请按 ‘Esc’键取消");
                    //textBoxFenshu.Focus();
                    textBoxFenshu.SelectAll();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonback_Click(object sender, EventArgs e)
        {
            if (_done.Count == 0)
            {
                MessageBox.Show("没有可以回评的试卷");
                return;
            }

            this.Hide();
            Formhp fhp = new Formhp(_db,_imgpathtemplate,_activesj,_done);
            fhp.ShowDialog();
            this.Show();
        }  
        private void ButtonSetWorkPathClick(object sender, EventArgs e)
        {
            if (Directory.Exists(textBoxWorkPath.Text))
            {
                DirectoryInfo di = new DirectoryInfo(textBoxWorkPath.Text);
                _workpath = di.FullName;
                this._imgpathtemplate = _workpath + "[id]_00_1_p1.TIF";
            }
        }
        private void ButtonExportScoreClick(object sender, EventArgs e)
        {
        	if(_activefloorid != -1 && _activesj!=null){
        		// 检测 是否改完
        		bool hasover = false;
        		foreach( subject s      in comboBox1.Items ){
        			string sql = "select count(*) as cnt from subjectscore_[floorid] where tk[tkid] = -1 "
        				.Replace("[tkid]",s.Subid.ToString()).Replace("[floorid]",_activefloorid.ToString());
        			DataTable dt = _db.query(sql).Tables[0];
        			int sum = (int)dt.Rows[0][0];
        			if(sum>0) {
        				MessageBox.Show("你还有试题未改完");
        				hasover = true;
        				break;
        			}
        		}
        		
        		///
        		hasover = false;
        		if(!hasover){
        			string sql = "select * from subjectscore_[floorid]".Replace("[floorid]",_activefloorid.ToString());
        			DataTable dt = _db.query(sql).Tables[0];
        			StringBuilder sb = new StringBuilder();
        			foreach(DataColumn dc in dt.Columns)
        				sb.Append( dc.ColumnName+",");
        			sb.AppendLine();        				
        			foreach(DataRow dr in dt.Rows){
        				string str = "";
        				for(int i=0; i<dt.Columns.Count; i++){
        					str+= dr[i]+",";
        				}
        				sb.AppendLine(str);
        			}
        			
        			File.WriteAllText( "导出成绩.txt",sb.ToString());
        			MessageBox.Show("已将成绩导出到   导出成绩.txt ");
        		}
        	}
        }
        private void buttonSetXztAnswer_Click(object sender, EventArgs e)
        {           
            string sql = "select * from subjectscore_[floorid] where 1=2 ".Replace("[floorid]", _activefloorid.ToString());
            DataTable dt = _db.query(sql).Tables[0];
            List<string> xztids = new List<string>();
            foreach (DataColumn dc in dt.Columns)
                if (dc.ColumnName.StartsWith("xz"))
                    xztids.Add(dc.ColumnName);
            FormSetscore f = new FormSetscore(xztids);
            f.ShowDialog();

            //TODO: 保存答案

            if (!f.CheckOK())
            {
                MessageBox.Show("选择题答案不完整，请从新设置");
                return;
            }
            string sqlt1 = "update subjectscore_[floorid] as A, subjectbase_[floorid]  as B set A.[xztid]=[score] where trim(A.kh) = B.kh and B.[xztid] = '[answer]'"
                .Replace("[floorid]", _activefloorid.ToString());
            string sqlt2 = "update subjectscore_[floorid] as A, subjectbase_[floorid]  as B set A.[xztid]=0 where trim(A.kh) = B.kh and B.[xztid] <> '[answer]'"
                 .Replace("[floorid]", _activefloorid.ToString());
           
            foreach (XztQuestion q in f.Xzt())
            {
                sql = sqlt1.Replace("[xztid]", "xz" + q.ID).Replace("[answer]", q.OptionAnswer).Replace("[score]",q.Score.ToString());
                _db.update(sql);
                sql = sqlt2.Replace("[xztid]", "xz" + q.ID).Replace("[answer]", q.OptionAnswer);
                _db.update(sql);
            }

        }
        private void buttonSubmitMulti_Click(object sender, EventArgs e)
        {
            if (checkallsetscore())
            {
                string sql1 = "update subjectscore_[floorid]  set  tk[subid] = [score] where kh=[kh]"
                    .Replace("[floorid]", _activefloorid.ToString())
                    .Replace("[subid]", _activesj.Subid.ToString());

                int sum = 0;
                for (int i = 0; i < dgvs.Rows.Count; i++)
                {
                    string s = sql1.Replace("[score]", dgvs.Rows[i].Cells["得分"].Value.ToString())
                        .Replace("[kh]", dgvs.Rows[i].Cells["kh"].Value.ToString());
                    sum += _db.update(s);
                } //MessageBox.Show("已更新" + sum + "条数据");
                LoadNext();
            }
            else
            {
                MessageBox.Show("还有试题没有给分");
            }
        }
        private void ButtonFullScreenYJClick(object sender, EventArgs e)
        {
        	//
        	if(_dtshow!=null)
        	_dtshow.Rows.Clear();
        	dgvs.Rows.Clear();
        	FormFullScreenYJ f = new FormFullScreenYJ( _db,GetSubjects(),_activefloorid,_workpath);
        	
        	this.Hide();
        	f.ShowDialog();
        	this.Show();
        }
        private void LoadNext()
        {
            try
            {
                foreach (DataRow dr in _drlist)
                    _activedt.Rows.Remove(dr);
                _drlist.Clear();
                //done.Add(activekh);
                textBoxShow.Text = "本题未完成阅卷份数" + _activedt.Rows.Count + " 满分为" + _activesj.MaxResult + "分";
                
                if (checkBoxautoLoadNext.Checked)
                    YueJuan();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool checkallsetscore()
        {
            for (int i = 0; i < dgvs.Rows.Count; i++)
            {
                if (dgvs.Rows[i].Cells["得分"].Value.ToString() == "")
                    return false;
            }
            return true;
        }
		private void DgvsCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
        	if(e.ColumnIndex==-1 || e.RowIndex == -1) return;
        	string colName = dgvs.Columns[e.ColumnIndex].Name;

            if (colName.EndsWith("分"))
            {
                colName = colName.Substring(0, colName.Length - 1);
                if (!"0123456789".Contains(colName)) return;
                if(dgvs.Rows[e.RowIndex].Cells["得分"].Value is DBNull) return;
                
                string str = dgvs.Rows[e.RowIndex].Cells["得分"].Value.ToString();                
                int score = Convert.ToInt32(colName);                
                if (str == colName)
                {                    
        			e.CellStyle.BackColor =  Color.Red;
                }
            }            
        }
        private void dgvs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( e.ColumnIndex == -1) return;
            string colName = dgvs.Columns[e.ColumnIndex].Name;

            if (colName.EndsWith("分"))
            {
                colName = colName.Substring(0, colName.Length - 1);
                if (!"0123456789".Contains(colName)) return;
                int score = Convert.ToInt32(colName);
                if (e.RowIndex == -1)
                {
                    for(int i=0; i<dgvs.Rows.Count; i++)
                        dgvs.Rows[i].Cells["得分"].Value = score;
                }
                else
                {
                    dgvs.Rows[e.RowIndex].Cells["得分"].Value = score;
//                    dgvs.InvalidateCell(e.ColumnIndex,e.RowIndex);
                    dgvs.InvalidateRow(e.RowIndex); //多栏时要更改
                }
            }

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;

            subject sj = (subject)comboBox1.SelectedItem;
            _activesj = sj;
            string sql = "select kh,tk[subid] from subjectscore_[floorid] where tk[subid]=-1 order by id"
                        .Replace("[floorid]", _activefloorid.ToString())
                        .Replace("[subid]", sj.Subid.ToString());
            _activedt = _db.query(sql).Tables[0];

            textBoxShow.Text = "本题未完成阅卷份数" + _activedt.Rows.Count + " 满分为" + sj.MaxResult + "分";
            _done.Clear();
            if (checkBoxdgvpic.Checked)
            {
            	dgvs.Visible = true;
                buttonSubmitMulti.Visible = true;
                splitdgvpic.Panel1Collapsed = true;
                splitdgvpic.Panel2Collapsed = false;

                textBoxFenshu.Visible = false;
                buttonok.Visible = false;
                checkBoxautoLoadNext.Visible = false;
                buttonback.Visible = false;

                int maxscore = (int)_activesj.MaxResult;
                List<string> titles = new List<string>(new string[] { "kh", "图片", "得分" });
                for (int i = 0; i <= maxscore; i++)
                {
                    titles.Add(i + "分");
                }
                _dtshow = Tools.DataTableTools.ConstructDataTable(titles.ToArray());
                dgvs.DataSource = _dtshow;

                int index = 0;
                foreach (DataGridViewColumn dc in dgvs.Columns)
                {
                    if (dc.Name.Contains("分"))
                    {
                        dgvs.Columns[index].Width = 30;
                    }
                    else if (dc.Name.ToUpper() == "KH")
                    {
                        dgvs.Columns[index].Visible = false;
                    }
                    else if (dc.Name == "图片")
                    {
                        ((DataGridViewImageColumn)(dgvs.Columns[index])).ImageLayout = DataGridViewImageCellLayout.Zoom;
                        dc.Width = _activesj.Rect.Width / 3;
                    }
                    index++;
                }
                dgvs.RowTemplate.Height = _activesj.Rect.Height / 3;

            }
            else
            {
            	dgvs.Visible = false;
                buttonSubmitMulti.Visible = false;
                splitdgvpic.Panel1Collapsed = false;
                splitdgvpic.Panel2Collapsed = true;

                textBoxFenshu.Visible = true;
                buttonok.Visible = true;
                checkBoxautoLoadNext.Visible = true;
                buttonback.Visible = true;
            }
            
            if(checkBoxLoadFromBitmapdata.Checked){
	            if(  loadbmpdata==null){            	           
	            	string bmpdatapath = "floor[fid]bitmapdata".Replace("[fid]",_activefloorid.ToString());
	            	bmpdatapath = _workpath.Replace("C_IMAGES",bmpdatapath);
	            	List<subject> sublist = GetSubjects();
	            	loadbmpdata = new LoadBitmapData(bmpdatapath,_activefloorid,sublist);
	            }
            	loadbmpdata.SetActiveSubject(_activesj);
            }
            YueJuan();


        }
        private void textBoxFenshu_KeyUp(object sender, KeyEventArgs e)
        {             
            if (e.KeyData == Keys.Return)
            {
                if(!_boutarea)
                buttonok.PerformClick();
                _boutarea = false;
            }
        }  
		private void ComboBox1KeyUp(object sender, KeyEventArgs e)
        {
        	 if (e.KeyData == Keys.Return )
            {
                string txt = comboBox1.Text;
                if(txt == "init"){
                	splitContainer2.Panel1Collapsed = false;
                }
            }
        }
        private void Init(int floorid)
        {            
            _boutarea = false;
            _done = new List<string>();            
            InitSubjectData(floorid.ToString());  
        	if(loadbmpdata!=null){
        		loadbmpdata.Clear();
        		loadbmpdata=null;
        	}
        }
        private void InitSubjectData(string floorid)
        {
        	comboBox1.Items.Clear();
        	string sql = "select * from room where floorid=[floorid] order by id ".Replace("[floorid]",floorid);
            DataTable dtsubject = _db.query(sql).Tables[0];
            foreach (DataRow dr in dtsubject.Rows)
            {
            	int x = Convert.ToInt32( dr["rx"].ToString());
            	int y = Convert.ToInt32( dr["ry"].ToString());
            	int w = Convert.ToInt32( dr["rw"].ToString());
            	int h = Convert.ToInt32( dr["rh"].ToString());
            	Rectangle r = new Rectangle( x,y,w,h);
                int index = comboBox1.Items.Add( new subject((int)dr["id"],(double)dr["maxscore"],(string)dr["roomname"] ,r ));               
            }
        }        
        private List<subject> GetSubjects(){
        	List<subject> sublist = new List<subject>();
        	foreach(object o in comboBox1.Items)
        		sublist.Add( (subject)o);
        	return sublist;
        }        
        private void YueJuan()
        {

            if (_activedt.Rows.Count > 0)
            {
                if (checkBoxdgvpic.Checked)
                {
                    int cnt = 0;
                    _dtshow.Rows.Clear();
                    _drlist.Clear();
                    foreach (DataRow dr in _activedt.Rows)
                    {
                        DataRow drt = _dtshow.NewRow();
                        drt["kh"] = dr["kh"];
                        //drt[""] = dr[""];
                        if(checkBoxLoadFromBitmapdata.Checked){
                        	drt["图片"] = loadbmpdata.GetBitmap( dr["kh"].ToString());
                        }else{
	                        string imgname = _imgpathtemplate.Replace("[id]", dr["kh"].ToString());
	                        Bitmap img = (Bitmap)Bitmap.FromFile(imgname);
	                        Bitmap imgc = img.Clone(_activesj.Rect, img.PixelFormat);
	                        drt["图片"] = imgc;
                        }

                        _dtshow.Rows.Add(drt);
                        _drlist.Add(dr);
                        if (++cnt == 10) break;
                    }
                }
                else
                {
                    DataRow dr = _activedt.Rows[0];
                    string imgname = _imgpathtemplate.Replace("[id]", dr["kh"].ToString());
                    Bitmap img = (Bitmap)Bitmap.FromFile(imgname);
                    Bitmap imgc = img.Clone(_activesj.Rect, img.PixelFormat);
                    pictureBox1.Image = imgc;
                    _activekh = dr["kh"].ToString();
                    textBoxFenshu.Focus();
                    textBoxFenshu.SelectAll();
                }
            }
            else
            {
                if (checkBoxdgvpic.Checked)
                    _dtshow.Rows.Clear();
            }
        }      

        private string _dbfilename;
        private string _activekh;
        private bool _boutarea;
        private Db.ConnDb _db;
        private List<string> _done;
        private DataTable _activedt;
        private DataTable _dtshow;
        private List<DataRow> _drlist;
        private subject _activesj;
        private int _activefloorid;
        private string _ImgPath;
        private string _imgpathtemplate;
        private string _workpath;        
        public  LoadBitmapData loadbmpdata;        
        
        
    }
    public class TableTools //负责表的检测和创建
    {
    	private Db.ConnDb db;
    	private string _floorid;
    	private string _floorname;
        private List<string> importtxt;
        public TableTools(Db.ConnDb db, string floorid,string floorname)
        {
        	this.db = db;
        	this._floorid = floorid;
        	this._floorname = floorname;
        	importtxt = new List<string>();
        }
        public bool ImportScanData(){
        	if(!OpenBaseXztPath()) return false;        	
        	CheckTableSubject("subjectbase_"+_floorid);
        	if(!CreateTableSubjectBase(importtxt[0],"subjectbase_"+_floorid)) return false;
        	ImportSubjectBaseData(importtxt,_floorid.ToString());
        	//导入选择题
        	List<string> roomids = GetRoomids(_floorid.ToString(),db);	
        	List<string> xztids = GetXZTids(importtxt[0]);
        	CheckTableSubject("subjectscore_"+_floorid);        	
        	CreateTableSubjectScore(roomids,importtxt[0],"subjectscore_"+_floorid);
        	ImportSubjectScoreData(roomids,xztids, _floorid.ToString());
        	return true;
        }        
		private void CheckTableSubject(string tablename)  //1 如果存在则删除   
		{
			string sql = "drop table [tablename]".Replace("tablename",tablename);
			try{
				db.update(sql);
			}catch{
				
			}			
		}       
        private bool OpenBaseXztPath()
        {
            OpenFileDialog OpenFileDialog2 = new OpenFileDialog();
            OpenFileDialog2.FileName = "OpenFileDialog2";
            OpenFileDialog2.Filter = "Txt Files(*.txt)|*.txt|All files (*.*)|*.*"; //"Picture files (*.gif)|*.gif"; 
            OpenFileDialog2.Title = "Open text file";
            if (OpenFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                	importtxt.Clear();
                	importtxt.AddRange( File.ReadAllLines(OpenFileDialog2.FileName) );
                }
                catch (Exception ex)
                {
                    MessageBox.Show("格式错误或者格式不支持" + ex.Message);
                    return false;
                }
            }
            return true;
        }          
        private bool CreateTableSubjectBase(string s,string dsttablename){
        	string[] ss = s.Split(new string[]{",","\""},StringSplitOptions.RemoveEmptyEntries);
        	
        	string DstIsSameToSrc = "True";
        	string dstpath = "";        
        	string DstIsCreateID = "False";        	
//        	if (DstIsCreate != "True") return;
            string sqltemp = "CREATE TABLE [;DATABASE=dstpath].[dsttablename](\r\n" +
                            "    ID COUNTER  CONSTRAINT PK_TVIPLevel26 PRIMARY KEY" +
                            "[OtherCol])";
            if (DstIsSameToSrc == "True")
                sqltemp = sqltemp.Replace("[;DATABASE=dstpath].", "");
            else
                sqltemp = sqltemp.Replace("dstpath", dstpath);
            if (dsttablename == "") return false;
            sqltemp = sqltemp.Replace("dsttablename", dsttablename);
            
            if (DstIsCreateID == "False")
                sqltemp = sqltemp.Replace("COUNTER  CONSTRAINT PK_TVIPLevel26 PRIMARY KEY", "INTEGER");
//            else
//                coltemp += ",\r\n    " + DstsaveSrcIDAs + "  INTEGER";
			string coltemp = "";
            foreach (string  xe in ss)
            {
                string colname = xe;
                coltemp += ",\r\n    " + "[" + colname + "]  ";              
                coltemp += "STRING";
            }
            sqltemp = sqltemp.Replace("[OtherCol]", coltemp);            
            if (db!=null)
            {
                try
                {
                    db.update(sqltemp);
                    MessageBox.Show("表格 "+dsttablename+" 已创建");
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
            return true;
        }        
        private bool CreateTableSubjectScore(List<string> roomids,string xztstr, string dsttablename){
        	// xz 选择题  tk 填空题 （非选择题）
        	string[] ts1 = xztstr.Split(new string[]{",","\""},StringSplitOptions.RemoveEmptyEntries);
        	List<string> ts2 = new List<string>();
        	foreach(string str in ts1)
        		if(str.Contains( "kh") || str.Contains("xz"))
        			ts2.Add(str);        	
        	foreach(string  roomid in roomids){ 
        		ts2.Add("tk"+roomid.ToString());
        	}
        	string[] ss = ts2.ToArray();
        	
        	string DstIsSameToSrc = "True";
        	string dstpath = "";        	
        	string DstIsCreateID = "False";
        	
//        	if (DstIsCreate != "True") return;
            string sqltemp = "CREATE TABLE [;DATABASE=dstpath].[dsttablename](\r\n" +
                            "    ID COUNTER  CONSTRAINT PK_TVIPLevel26 PRIMARY KEY" +
                            "[OtherCol])";
            if (DstIsSameToSrc == "True")
                sqltemp = sqltemp.Replace("[;DATABASE=dstpath].", "");
            else
                sqltemp = sqltemp.Replace("dstpath", dstpath);
            if (dsttablename == "") return false;
            sqltemp = sqltemp.Replace("dsttablename", dsttablename);
            
            if (DstIsCreateID == "False")
                sqltemp = sqltemp.Replace("COUNTER  CONSTRAINT PK_TVIPLevel26 PRIMARY KEY", "INTEGER");
//            else
//                coltemp += ",\r\n    " + DstsaveSrcIDAs + "  INTEGER";
			string coltemp = "";
            foreach (string  xe in ss)
            {
                string colname = xe;
                coltemp += ",\r\n    " + "[" + colname + "]  ";              
                coltemp += "DOUBLE";
            }
            sqltemp = sqltemp.Replace("[OtherCol]", coltemp);
            
            if (db!=null)
            {
                try
                {
                    db.update(sqltemp);
                    MessageBox.Show("表格"+dsttablename+"已创建");
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
            return true;
        }
        
        private void ImportSubjectBaseData(List<string> data,string floorid){ // data = importtxt
        	string sqlt = "insert into subjectbase_[floorid]([col]) values([values])"
        		.Replace("[col]", data[0].Replace("\"",""))
        		.Replace("[floorid]",floorid);
        	int sum = 0;
        	for(int i=1; i<data.Count; i++){
        		string sql = sqlt.Replace("[values]",data[i]);
        		sum+= db.update(sql);
        	}
        	MessageBox.Show("已插入" + sum+"条记录");
        }
        private void  ImportSubjectScoreData(List<string> roomids, List<string> xztids, string floorid){
        	string sql = "insert into subjectscore_[floorid](kh) select kh from subjectbase_[floorid]"
        		.Replace("[floorid]",floorid);
        	int sum = db.update(sql);
        	MessageBox.Show( "已插入表subjectscore_[floorid]  共[sum] 条记录".Replace("[floorid]",floorid).Replace("[sum]",sum.ToString()));
        	sql = "update subjectscore_[floorid] set ".Replace("[floorid]",floorid);
        	foreach(string id in xztids)
        		sql+= id +"=-1, ";
        	foreach(string id in roomids)
        		sql+="tk"+ id +"=-1, ";
        	if( xztids.Count+roomids.Count>0)
        		sql = sql.Substring( 0, sql.Length-2);
        	db.update(sql);
        }
        ///////forcheck file
        public static  List<string> GetRoomids(string floorid,Db.ConnDb db){
        	string sql = "select * from room where floorid = [floorid] order by id"
        		.Replace("[floorid]",floorid);
        	DataTable dt = db.query(sql).Tables[0];
        	List<string> roomids  = new List<string>();
        	foreach(DataRow dr in dt.Rows)
        		roomids.Add(dr["id"].ToString());
        	return roomids;
        }
        public static List<string> GetXZTids(string xztstr)
        {
        	string[] ts1 = xztstr.Split(new string[]{",","\""},StringSplitOptions.RemoveEmptyEntries);
        	List<string> ts2 = new List<string>();
        	foreach(string str in ts1)
        		if(str.Contains("xz"))
        			ts2.Add(str);    
        	return ts2;
        }
        private List<string> GetNameList(string activeimgfilename,string ext)
        {
            List<string> namelist = new List<string>();
            FileInfo fi = new FileInfo(activeimgfilename);
            DirectoryInfo dirinfo = fi.Directory;
            int cnt = 0;
            foreach (FileInfo f in dirinfo.GetFiles())
            {
                if (f.Extension.ToLower() == ext)
                {
                    cnt++;
                    namelist.Add(f.Name);
                }
            }
            return namelist;
        }
    }
   
	public class subject
    {
        public subject(int subid, double maxresult, string name, Rectangle rect)
        {
            this.Name = name;
            this.Subid = subid;
            this.MaxResult = maxresult;
            this.Rect = rect;
            this.BitmapdataLength = -1;
        }
        public int Subid;
        public Double MaxResult;
        public string Name;
        public Rectangle Rect;
        public int BitmapdataLength;
        public override string ToString()
        {
            return Name;
        }
    }

}
