using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ARTemplate;
using System.IO;

namespace ScanTemplate
{
	public partial class FormVerify : Form
	{
        private ScanConfig _sc;
		private DataTable  _dt;
		private List<int> _ColState;
        private string _type;
		public FormVerify(ScanConfig sc,DataTable  dt,string type)
		{
            this.Changed = false;
            this._sc = sc;
			this._dt = dt;
            this._type = type;
            if (type == "选择题")
            {
                _ColState = new List<int>() { 0, 0, 0, 0, 1, 2, 3, 4, -5, -6 };
            }
            else if (type == "考号")
            {
                _ColState = new List<int>() { 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0 };
            }
            else if (type == "核对姓名")
            {
                _ColState = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
			InitializeComponent();
			dgv.DataSource = dt;
		}
		private void FormVerifyLoad(object sender, EventArgs e)
        {
        	InitDgvUI();
            InitNameFinds();
        }
        private void InitNameFinds()
        {
            if (_sc.Studentbases.HasStudentBase)
            {
                items = new List<string>();
                items.AddRange(
                    _sc.Studentbases.Studentbase.Select(
                   s => s.Classid+ "-"+s.KH+ "_" + s.Name + "(" + s.PYCode + ")").ToArray());

                string importtext = "600221";
                if (File.Exists("select.txt"))
                    importtext = File.ReadAllText("select.txt").Trim();
                List<string> find = items.FindAll(s => importtext.Contains(s.Substring(2, 6)));  //s.Contains(importtext.ToUpper())
                if (find.Count > 0)
                {
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(find.ToArray());
                    listBox1.Items.Clear();
                }  
            }
        }
		private void InitDgvUI( )
		{
            if (_type == "选择题")
            {
                //((DataGridViewImageColumn)(dgv.Columns["图片"])).ImageLayout = DataGridViewImageCellLayout.Zoom;
			    dgv.RowTemplate.Height = 30;
                dgv.DataSource = null;
                dgv.DataSource = _dt;
                foreach (DataGridViewColumn dc in dgv.Columns)
                    if (dc.Name.Contains("图片"))
                        ((DataGridViewImageColumn)(dc)).ImageLayout = DataGridViewImageCellLayout.Zoom;

                dgv.Columns["学号"].Width = 80;
                dgv.Columns["题号"].Width = 30;
                dgv.Columns["图片"].Width = 200;
                dgv.Columns["你的答案"].Width = 30;
                dgv.Columns["A"].Width = 30;
                dgv.Columns["B"].Width = 30;
                dgv.Columns["C"].Width = 30;
                dgv.Columns["D"].Width = 30;
                dgv.Columns["是否多选"].Width = 30;
                dgv.Columns["是否修改"].Width = 0;
            }
            else if(_type == "考号")
            {
			    dgv.RowTemplate.Height = 90;
                dgv.DataSource = null;
                dgv.DataSource = _dt;
                //((DataGridViewImageColumn)(dgv.Columns["图片"])).ImageLayout = DataGridViewImageCellLayout.Zoom;
                foreach (DataGridViewColumn dc in dgv.Columns)
                    if (dc.Name.Contains("图片"))
                        ((DataGridViewImageColumn)(dc)).ImageLayout = DataGridViewImageCellLayout.Zoom;

                dgv.Columns["OID考号"].Width = 60;
                dgv.Columns["图片"].Width = 300;
                dgv.Columns["图片姓名"].Width = 100;
                dgv.Columns["图片考号"].Width = 120;
                dgv.Columns["姓名"].Width = 60;
                dgv.Columns["新考号"].Width = 40;
                dgv.Columns["是否修改"].Width = 0;
            }
            else if (_type == "核对姓名")
            {
                dgv.RowTemplate.Height = 30;
                dgv.DataSource = null;
                dgv.DataSource = _dt;
                //((DataGridViewImageColumn)(dgv.Columns["图片"])).ImageLayout = DataGridViewImageCellLayout.Zoom;
                foreach (DataGridViewColumn dc in dgv.Columns)
                    if (dc.Name.Contains("图片"))
                        ((DataGridViewImageColumn)(dc)).ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgv.Columns["OID考号"].Width = 60;
                dgv.Columns["图片姓名"].Width = 100;
                dgv.Columns["姓名"].Width = 60;
                dgv.Columns["新考号"].Width = 40;
                dgv.Columns["是否修改"].Width = 0;
            }
		}
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string importtext = textBox1.Text;
            List<string> find = items.FindAll(s => s.Contains(importtext.ToUpper()));
            if (find.Count > 0)
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(find.ToArray());
            }
        }
		private void DgvCellClick(object sender, DataGridViewCellEventArgs e)
		{
			if ( e.ColumnIndex == -1) return;
			if (_ColState[e.ColumnIndex] > 0 && _type == "选择题")
			{
				int score = _ColState[e.ColumnIndex] - 1;
				int scoreindex = e.ColumnIndex - score -1;
				
				Char c = Convert.ToChar( 'A'+score) ;
				bool multiselect =Convert.ToBoolean( dgv[scoreindex+5,e.RowIndex].Value );
				dgv[scoreindex+6,e.RowIndex].Value = true;
				if(multiselect){
					if(dgv[scoreindex,e.RowIndex].Value == null)
						dgv[scoreindex,e.RowIndex].Value = "";
					string result = dgv[scoreindex,e.RowIndex].Value.ToString();
					if(result.Contains(c))
						result.Remove( result.IndexOf(c),1);
					else
						result+=c;
					dgv[scoreindex,e.RowIndex].Value = result;
				}else{
					dgv[scoreindex,e.RowIndex].Value = c.ToString();
				}
				//                if (e.RowIndex == -1)
				//                {
				//                    for(int i=0; i<dgvs.Rows.Count; i++)
				//                        dgvs.Rows[i].Cells[scoreindex].Value = score;
				//                    dgvs.Invalidate();
				//                }
				//                else
				//                {
				//                    dgvs.Rows[e.RowIndex].Cells[scoreindex].Value = score;
				//                    dgvs.InvalidateRow(e.RowIndex);
				//                }
			}
		}
		private void DgvCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if(e.ColumnIndex==-1 || e.RowIndex == -1) return;
            if (_ColState[e.ColumnIndex] > 0 && _type == "选择题")
			{
				int score = _ColState[e.ColumnIndex] - 1;
				int scoreindex = e.ColumnIndex - score -1;
				try{
					if(dgv.Rows[e.RowIndex].Cells[scoreindex].Value == null ) return;    	//|| is DBNull
					string str = dgv.Rows[e.RowIndex].Cells[scoreindex].Value.ToString();
					Char value =Convert.ToChar( 'A'+score);
					if (str.Contains(value))
					{
						e.CellStyle.BackColor =  Color.Red;
					}
				}catch(Exception ee){
					MessageBox.Show(ee.Message+""+ e.RowIndex +" "+e.ColumnIndex) ;
					;
				}
			}
		}
		private void buttonImportData_Click(object sender, EventArgs e)
		{
            if (_type == "选择题")
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    if ((bool)dr["是否修改"])
                    {
                        DataRow origindr = (DataRow)(((ValueTag)dr["学号"]).Tag);
                        string th = dr["题号"].ToString();
                        if (origindr[th].ToString() != dr["你的答案"].ToString())
                            origindr[th] = dr["你的答案"];
                        Changed = true;
                    }
                }
            }
            else if (_type == "考号")
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    if ((bool)dr["是否修改"])
                    {
                        Changed = true;
                        break;
                    }
                }
            }
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_type == "考号")
                {
                    int kh = Convert.ToInt32(dgv[e.ColumnIndex, e.RowIndex].Value);
                    DataRow dr = ((DataTable)dgv.DataSource).Rows[e.RowIndex];
                    DataRow origindr = (DataRow)(((ValueTag)dr["OID考号"]).Tag);

                    if (_sc.Studentbases.HasStudentBase)
                    {
                        string name =_sc.Studentbases.GetName(kh);
                        dr["OID考号"] = new ValueTag(kh.ToString(), origindr);
                        dr["姓名"] = name;
                        origindr["考号"] = kh;
                        origindr["姓名"] = name;
                        dr["是否修改"] = true;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }
        private void dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if(! (_type == "考号" && _ColState[e.ColumnIndex] > 10))
            {
                e.Cancel = true;
            }
        }
        public bool Changed { get; set; }
        public List<string> items { get; set; }
    }
}
