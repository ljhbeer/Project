﻿using System;
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
		private DataTable  _dt;
		private List<int> _ColState;
        private string _type;
		public FormVerify(DataTable  dt,string type)
		{
			this._dt = dt;
            this._type = type;
            if (type == "选择题")
            {
                _ColState = new List<int>() { 0, 0, 0, 0, 1, 2, 3, 4, -5, -6 };
            }
            else if (type == "考号")
            {
                _ColState = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
			InitializeComponent();
			dgv.DataSource = dt;
		}
		private void FormVerifyLoad(object sender, EventArgs e)
        {
        	InitDgvUI();
        }
		private void InitDgvUI( )
		{
            if (_type == "选择题")
            {
                ((DataGridViewImageColumn)(dgv.Columns["图片"])).ImageLayout = DataGridViewImageCellLayout.Zoom;
			    dgv.RowTemplate.Height = 30;
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
                ((DataGridViewImageColumn)(dgv.Columns["图片"])).ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgv.Columns["考号"].Width = 60;
                dgv.Columns["图片"].Width = 300;
                dgv.Columns["姓名"].Width = 40;
                dgv.Columns["新考号"].Width = 40;
                dgv.Columns["是否修改"].Width = 0;
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
                    }
                }
            }
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
