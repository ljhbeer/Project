using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using ARTemplate;
using System.Linq;

namespace ScanTemplate.FormYJ
{
	public partial class FormFullScreenYJ : Form
	{
        public FormFullScreenYJ(FormYJ.Students _Students, FormYJ.Imgsubjects _Imgsubjects, string path)
        {
			InitializeComponent();
			this._workpath = path;            
            _SR = new StudentsResult(_Students, _Imgsubjects, _workpath);
         
			comboBox1.Items.AddRange(_Imgsubjects.Subjects.ToArray());
			Init();
        }
		private void Init(){
			_dgvsize = dgvs.ClientSize;
			_cntx = 1;
			_cnty = 1;
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
		private void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
            if (comboBox1.SelectedIndex == -1) return;
            Imgsubject S = (Imgsubject)comboBox1.SelectedItem;
            InitColState( S.Score);
            _SR.SetActiveSubject(S);            
            textBoxShow.Text = "本题未完成阅卷份数" + _SR.Students.Count + " 满分为" + S.Score + "分";         
            _imgsize = S.Rect.Size;
            _itemsize.Width = _imgsize.Width /2 + (S.Score + 2) * 27;
            _itemsize.Height = _imgsize.Height / 2;
            _cntx = (dgvs.Size.Width - 15) / _itemsize.Width;
            _cnty = (dgvs.Size.Height - 30) / _itemsize.Height;
            InitDtshow(_cntx);
            InitDgvUI();
            YueJuan();
		}
        //TODO: fullscreen.Debug
		private void ButtonSubmitMultiClick(object sender, EventArgs e)
		{
            if (_SR.ActiveSubject == null)
                return;
			if (checkallsetscore())
            {
                
                int sum = 0;
                List<int> scoreindex = new List<int>();
				for (int i = 0; i < dgvs.Columns.Count; i++){
					if(_ColState[i]==0)
						scoreindex.Add(i);
				}

                bool bbreak = false;
                foreach (int index in scoreindex)
                {
                    for (int i = 0; i < dgvs.Rows.Count; i++)
                    {
                        if (dgvs.Rows[i].Cells[index - 2].Value is DBNull)
                        {
                            bbreak = true;
                            break;
                        }
                        int Score = Convert.ToInt32(dgvs.Rows[i].Cells[index].Value.ToString());
                        Student S = (Student)dgvs.Rows[i].Cells[index - 2].Value;
                        _SR.SetScoreByKh(S, Score);
                    }
                    if (bbreak) break;
                } //MessageBox.Show("已更新" + sum + "条数据");
                _SR.LoadNextStudents();
                ShowItemsInDgv();
                textBoxShow.Text = "本题未完成阅卷份数" + _SR.Students.Count + " 满分为" +_SR.ActiveSubject.Score + "分";        
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
		private void InitDtshow(int cntx){
			List<string> titles = new List<string>();		
			for(int x=0; x<cntx; x++){
				string xx = x.ToString();
				titles.Add("object"+xx);
				titles.Add("图片"+xx);
				titles.Add(	"得分"+xx);
                for (int i = 0; i <= _SR.ActiveSubject.Score ; i++)
                    titles.Add(i + "分" + xx);
			}
			_dtshow = Tools.DataTableTools.ConstructDataTable(titles.ToArray());
			dgvs.DataSource = null;
			dgvs.DataSource = _dtshow;
            //dgvs.SortedColumn.SortMode = DataGridViewColumnSortMode.NotSortable;			
		}
		private void InitDgvUI()
		{
			int index = 0;
			foreach (DataGridViewColumn dc in dgvs.Columns) {
				if (dc.Name.Contains("分")) {
					dgvs.Columns[index].Width = 27;
					if(!dc.Name.EndsWith("分"))
						dgvs.Columns[index].HeaderText = dc.Name.Substring(0,dc.Name.Length-1);
				} else if (dc.Name.ToUpper().Contains( "OBJECT")) {
					dgvs.Columns[index].Visible = false;
				} else if (dc.Name.Contains("图片")) {
					((DataGridViewImageColumn)(dgvs.Columns[index])).ImageLayout = DataGridViewImageCellLayout.Zoom;
                    dc.Width = _imgsize.Width / 2;
				}
				index++;
			}
            dgvs.RowTemplate.Height = _itemsize.Height;
		}
		private void ShowItemsInDgv(){
			int cntx = _cntx;
			int cnty = _cnty;
			_dtshow.Rows.Clear();
            Stack<Student> stack = new Stack<Student>(_SR.Students);
            Student S = null;
            for (int x = 0; x < cntx; x++)
            {
                string xx = x.ToString();
                for (int y = 0; y < cnty; y++)
                {
                    if (stack.Count == 0)
                        break;
                    S = stack.Pop();
                    if (x == 0)
                    {
                        DataRow drt = _dtshow.NewRow();
                        _dtshow.Rows.Add(drt);
                    }
                    DataRow dr = _dtshow.Rows[y];
                    dr["object" + xx] = S;
                    dr["图片" + xx] = _SR.GetBitMap(S);
                }
                if (S==null) break;
            }			
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

        private Size _dgvsize;
        private Size _itemsize;
        private Size _imgsize;
        private int _cntx;
        private int _cnty;
        private List<int> _ColState;
        private List<int> _colstatetemplate;
        private List<DataRow> _drlist;
        private DataTable _dtshow;

        private string _workpath;
        private StudentsResult _SR;
	}
    public class StudentsResult
    {
        public Imgsubject ActiveSubject { get { return _activesubject; } }
        public List<Student> Students { get; set; }
        public StudentsResult(FormYJ.Students _Students, FormYJ.Imgsubjects _Imgsubjects, string _workpath)
        {
            this._Students = _Students;
            this._Imgsubjects = _Imgsubjects;
            this._workpath = _workpath;

            _Result = new List<List<int>>();
            for (int i = 0; i < _Imgsubjects.Subjects.Count; i++)
            {
                List<int> L = new List<int>();
                for (int index = 0; index < _Students.students.Count; index++)
                {
                    L.Add(-index-1);
                }
                _Result.Add(L);
            }
            _Ims = new ImgbinManagesubjects(_Students, _Imgsubjects);
            _Ims.InitLoadBindata(_workpath);
         
            if (!_Students.CheckIndex())
                MessageBox.Show("index Error");
        }
        public void SetActiveSubject(Imgsubject S)
        {
            this._activesubject = S;
            _Ims.SetActiveSubject(S);
            LoadNextStudents();
        }
        public void SetScoreByKh(Student S, int Score)
        {
            _Result[_activesubject.Index][S.Index] = Score;
        }
        public Bitmap  GetBitMap(Student S)
        {
           return  _Ims.ActiveSubjectBitmap(S);
        }
        public void LoadNextStudents()
        {
            Students = _Result[_activesubject.Index].Where( r=> r<0).Select(r =>_Students.students[-r - 1] ).ToList();
        }

        private FormYJ.Students _Students;
        private FormYJ.Imgsubjects _Imgsubjects;
        private string _workpath;
        private ImgbinManagesubjects _Ims;
        private Imgsubject _activesubject;
        private List<List<int>> _Result;

    }
}
