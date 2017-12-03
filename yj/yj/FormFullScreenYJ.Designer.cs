/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2017-10-25
 * 时间: 9:46
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace yj
{
	partial class FormFullScreenYJ
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.dgvs = new System.Windows.Forms.DataGridView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.textBoxShow = new System.Windows.Forms.TextBox();
			this.buttonSubmitMulti = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvs)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
			this.tableLayoutPanel1.Controls.Add(this.dgvs, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonSubmitMulti, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(762, 438);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// dgvs
			// 
			this.dgvs.AllowUserToAddRows = false;
			this.dgvs.AllowUserToDeleteRows = false;
			this.dgvs.AllowUserToResizeRows = false;
			this.dgvs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvs.Location = new System.Drawing.Point(15, 33);
			this.dgvs.Name = "dgvs";
			this.dgvs.RowHeadersVisible = false;
			this.dgvs.RowTemplate.Height = 23;
			this.dgvs.Size = new System.Drawing.Size(732, 375);
			this.dgvs.TabIndex = 1;
			this.dgvs.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvsCellClick);
			this.dgvs.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvsCellPainting);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.comboBox1);
			this.panel1.Controls.Add(this.textBoxShow);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(15, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(732, 24);
			this.panel1.TabIndex = 13;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(3, 3);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(83, 20);
			this.comboBox1.TabIndex = 11;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1SelectedIndexChanged);
			// 
			// textBoxShow
			// 
			this.textBoxShow.Location = new System.Drawing.Point(296, 3);
			this.textBoxShow.Name = "textBoxShow";
			this.textBoxShow.Size = new System.Drawing.Size(418, 21);
			this.textBoxShow.TabIndex = 12;
			// 
			// buttonSubmitMulti
			// 
			this.buttonSubmitMulti.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonSubmitMulti.Location = new System.Drawing.Point(15, 414);
			this.buttonSubmitMulti.Name = "buttonSubmitMulti";
			this.buttonSubmitMulti.Size = new System.Drawing.Size(732, 21);
			this.buttonSubmitMulti.TabIndex = 15;
			this.buttonSubmitMulti.Text = "提交";
			this.buttonSubmitMulti.UseVisualStyleBackColor = true;
			this.buttonSubmitMulti.Click += new System.EventHandler(this.ButtonSubmitMultiClick);
			// 
			// FormFullScreenYJ
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(762, 438);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "FormFullScreenYJ";
			this.Text = "FormFullScreenYJ";
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvs)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button buttonSubmitMulti;
		private System.Windows.Forms.TextBox textBoxShow;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.DataGridView dgvs;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
