using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;

namespace yj
{
	public partial class FormFullScreenYJ : Form
	{
		public FormFullScreenYJ(Db.ConnDb  db, List<subject> sublist,int activefloorid,string workpath)
		{
			InitializeComponent();
			_db = db;
			_sublist = sublist;
			_activefloorid = activefloorid;
			_workpath = workpath;
			comboBox1.Items.AddRange(sublist.ToArray());
			Init();
		}
		private void Init(){
			_dgvsize = dgvs.ClientSize;
			_cntx = 1;
			_cnty = 1;
			_activesubject = null;
			_drlist = new List<DataRow>();
			_ColState = new List<int>();
			_colstatetemplate = new List<int>();
			_itemsize = new Size(1,1);            
		}
        private void InitColState(int MaxScore)
        {
            _colstatetemplate.Clear();
            _colstatetemplate.AddRange(new int[] { -1, -1, 0 });
            for (int i = 0; i <= MaxScore; i++)
                _colstatetemplate.Add(i + 1);
            _ColState.Clear();
            for (int i = 0; i < 10; i++)
                _ColState.AddRange(_colstatetemplate);
        }				
		///
		private void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == -1) return;
			_activesubject =(subject)comboBox1.SelectedItem;
            InitColState( (int)_activesubject.MaxResult );
			string sql = "select kh,tk[subid] from subjectscore_[floorid] where tk[subid]=-1 order by id"
				.Replace("[floorid]", _activefloorid.ToString())
				.Replace("[subid]", _activesubject.Subid.ToString());
			_activedt = _db.query(sql).Tables[0];
			textBoxShow.Text = "本题未完成阅卷份数" + _activedt.Rows.Count + " 满分为" + _activesubject.MaxResult + "分";
			//            done.Clear();	
			//int _imgsize _itemsize	 _cntx _cnty		
			_imgsize = _activesubject.Rect.Size;
//			_imgsize.Height = _imgsize.Height/3;
//			_imgsize.Width = _imgsize.Width/3;	
			
			_itemsize.Width = _imgsize.Width/3 + ((int)_activesubject.MaxResult+2)*27;
			_itemsize.Height = _imgsize.Height/3;
			_cntx = (dgvs.Size.Width-15) / _itemsize.Width;
			_cnty = (dgvs.Size.Height-30)/_itemsize.Height;
			InitDtshow(_cntx);
			InitDgvUI();
			InitLoadBmpData();	
			YueJuan();
		}
		private void ButtonSubmitMultiClick(object sender, EventArgs e)
		{
			if (checkallsetscore())
            {
                string sql1 = "update subjectscore_[floorid]  set  tk[subid] = [score] where kh=[kh]"
                    .Replace("[floorid]", _activefloorid.ToString())
                    .Replace("[subid]", _activesubject.Subid.ToString());

                int sum = 0;
                List<int> scoreindex = new List<int>();
				for (int i = 0; i < dgvs.Columns.Count; i++){
					if(_ColState[i]==0)
						scoreindex.Add(i);
				}

                bool bbreak = false;
                foreach(int index in scoreindex){
                    for (int i = 0; i < dgvs.Rows.Count; i++)
                    {
                        if (dgvs.Rows[i].Cells[index - 2].Value is DBNull)
                        {
                            bbreak = true;
                            break;
                        }
	                    string s = sql1.Replace("[score]", dgvs.Rows[i].Cells[index].Value.ToString())
	                        .Replace("[kh]", dgvs.Rows[i].Cells[index-2].Value.ToString());
	                    sum += _db.update(s);
                	}
                    if (bbreak) break;
                } //MessageBox.Show("已更新" + sum + "条数据");
                LoadNext();
            }
            else
            {
                MessageBox.Show("还有试题没有给分");
            }
		}
		private void YueJuan()
        {
			ShowItemsInDgv();
        }      
		private void DgvsCellClick(object sender, DataGridViewCellEventArgs e)
		{
			if ( e.ColumnIndex == -1) return;
			if (_ColState[e.ColumnIndex] > 0)
            {
				int score = _ColState[e.ColumnIndex] - 1;        
				int scoreindex = e.ColumnIndex - score -1;      
                if (e.RowIndex == -1)
                {
                    for(int i=0; i<dgvs.Rows.Count; i++)
                        dgvs.Rows[i].Cells[scoreindex].Value = score;
                    dgvs.Invalidate();
                }
                else
                {
                    dgvs.Rows[e.RowIndex].Cells[scoreindex].Value = score;
                    dgvs.InvalidateRow(e.RowIndex); 
                }
            }
		}
		private void DgvsCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if(e.ColumnIndex==-1 || e.RowIndex == -1) return;
			if (_ColState[e.ColumnIndex] > 0)
            {
				int score = _ColState[e.ColumnIndex] - 1;        
				int scoreindex = e.ColumnIndex - score -1; 
				if(dgvs.Rows[e.RowIndex].Cells[scoreindex].Value is DBNull) return;                
                string str = dgvs.Rows[e.RowIndex].Cells[scoreindex].Value.ToString();
                if (str == score.ToString())
                {                    
        			e.CellStyle.BackColor =  Color.Red;
                }
			}
		}		
		///////////////////////////
		private void InitLoadBmpData()
		{
			if (_loadbmpdata == null) {
				string bmpdatapath = _workpath + "floor[fid]bitmapdata\\".Replace("[fid]", _activefloorid.ToString());
				_loadbmpdata = new LoadBitmapData(bmpdatapath, _activefloorid, _sublist);
			}
			_loadbmpdata.SetActiveSubject(_activesubject);
		}
		private void InitDtshow(int cntx){
			List<string> titles = new List<string>();		
			for(int x=0; x<cntx; x++){
				string xx = x.ToString();
				titles.Add("kh"+xx);
				titles.Add("图片"+xx);
				titles.Add(	"得分"+xx);				
				for (int i = 0; i <= (int)_activesubject.MaxResult; i++)
					titles.Add(i + "分"+xx);
			}
			_dtshow = Tools.DataTableTools.ConstructDataTable(titles.ToArray());
			dgvs.DataSource = null;
			dgvs.DataSource = _dtshow;		
//			dgvs.SortedColumn.SortMode = DataGridViewColumnSortMode.NotSortable;			
		}
		private void InitDgvUI()
		{
			int index = 0;
			foreach (DataGridViewColumn dc in dgvs.Columns) {
				if (dc.Name.Contains("分")) {
					dgvs.Columns[index].Width = 27;
					if(!dc.Name.EndsWith("分"))
						dgvs.Columns[index].HeaderText = dc.Name.Substring(0,dc.Name.Length-1);
				} else if (dc.Name.ToUpper().Contains( "KH")) {
					;
					dgvs.Columns[index].Visible = false;
				} else if (dc.Name.Contains("图片")) {
					((DataGridViewImageColumn)(dgvs.Columns[index])).ImageLayout = DataGridViewImageCellLayout.Zoom;
					dc.Width = _imgsize.Width / 3;
				}
				index++;
			}
			dgvs.RowTemplate.Height = _imgsize.Height / 3;
		}
		private void ShowItemsInDgv(){
			int cntx = _cntx;
			int cnty = _cnty;
			_dtshow.Rows.Clear();
			string kh="";
			for(int x=0; x<cntx; x++){
				string xx = x.ToString();
				for(int y=0; y<cnty; y++){
					kh = GetNextKh();
					if(kh=="") break;
					if(x==0){
						DataRow drt = _dtshow.NewRow();
						_dtshow.Rows.Add(drt);
					}
					DataRow dr = _dtshow.Rows[y];
					dr["kh"+xx] = kh;
					dr["图片"+xx] = _loadbmpdata.GetBitmap(kh);
				}				
				if(kh=="") break;
			}			
		}		
		private void LoadNext()
        {
            try
            {
//                foreach (DataRow dr in _drlist)
//                    _activedt.Rows.Remove(dr);
                _drlist.Clear(); //done.Add(activekh);
                textBoxShow.Text = "本题未完成阅卷份数" + _activedt.Rows.Count + " 满分为" + _activesubject.MaxResult + "分";
                YueJuan();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
		private string GetNextKh(){
			if(_activedt.Rows.Count>0){
				string kh = _activedt.Rows[0]["kh"].ToString();
				_drlist.Add(_activedt.Rows[0]);
				_activedt.Rows.Remove(_activedt.Rows[0]);
				return kh;
			}
			return "";
		}		
		private bool checkallsetscore() // 还有 其他行
        {
			List<int> scoreindex = new List<int>();
			for (int i = 0; i < dgvs.Columns.Count; i++){
				if(_ColState[i]==0)
					scoreindex.Add(i);
			}
            foreach(int index in scoreindex){
                for (int i = 0; i < dgvs.Rows.Count; i++)
                {
                    if (dgvs.Rows[i].Cells[index].Value.ToString() == "")
                    {
                        if (dgvs.Rows[i].Cells[index - 2].Value is DBNull)
                            return true;
                        return false;
                    }
            	}
            }
            return true;
        }

        private Db.ConnDb _db;
        private List<subject> _sublist;
        private int _activefloorid;
        private string _workpath;
        //        // for multiyj
        private subject _activesubject;
        private Size _dgvsize;
        private Size _itemsize;
        private Size _imgsize;
        private DataTable _dtshow;
        private DataTable _activedt;
        private int _cntx;
        private int _cnty;
        private List<int> _ColState;
        private List<int> _colstatetemplate;
        private List<DataRow> _drlist;
        public LoadBitmapData _loadbmpdata;
	}
}
