namespace ScanTemplate
{
    partial class FormReportTable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageByStudent = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvstudent = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxOnlyShowerror = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvstudenterrorsubjectid = new System.Windows.Forms.DataGridView();
            this.dgvstudentrightsubjectid = new System.Windows.Forms.DataGridView();
            this.tabPageBySubject = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvsubjects = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonShowOption = new System.Windows.Forms.Button();
            this.buttonShowUnChoose = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonShowRightStudentList = new System.Windows.Forms.Button();
            this.buttonShowErrorStudentList = new System.Windows.Forms.Button();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.dgvSubjectRightErrorStudentList = new System.Windows.Forms.DataGridView();
            this.buttonZoomout = new System.Windows.Forms.Button();
            this.buttonZoomin = new System.Windows.Forms.Button();
            this.buttonZoomMouse = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageByStudent.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvstudent)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvstudenterrorsubjectid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvstudentrightsubjectid)).BeginInit();
            this.tabPageBySubject.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvsubjects)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjectRightErrorStudentList)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(795, 460);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(789, 19);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageByStudent);
            this.tabControl1.Controls.Add(this.tabPageBySubject);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(789, 429);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageByStudent
            // 
            this.tabPageByStudent.Controls.Add(this.tableLayoutPanel2);
            this.tabPageByStudent.Location = new System.Drawing.Point(4, 22);
            this.tabPageByStudent.Name = "tabPageByStudent";
            this.tabPageByStudent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByStudent.Size = new System.Drawing.Size(781, 403);
            this.tabPageByStudent.TabIndex = 0;
            this.tabPageByStudent.Text = "按人";
            this.tabPageByStudent.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tableLayoutPanel2.Controls.Add(this.dgvstudent, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.splitContainer1, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(775, 397);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // dgvstudent
            // 
            this.dgvstudent.AllowUserToAddRows = false;
            this.dgvstudent.AllowUserToDeleteRows = false;
            this.dgvstudent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvstudent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvstudent.Location = new System.Drawing.Point(3, 31);
            this.dgvstudent.Name = "dgvstudent";
            this.dgvstudent.ReadOnly = true;
            this.dgvstudent.RowTemplate.Height = 23;
            this.dgvstudent.Size = new System.Drawing.Size(226, 363);
            this.dgvstudent.TabIndex = 0;
            this.dgvstudent.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvstudent_CellClick);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(226, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "试卷信息";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(235, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 363);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 1000);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseWheel +=new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseWheel);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.checkBoxOnlyShowerror);
            this.flowLayoutPanel2.Controls.Add(this.buttonZoomout);
            this.flowLayoutPanel2.Controls.Add(this.buttonZoomin);
            this.flowLayoutPanel2.Controls.Add(this.buttonZoomMouse);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(235, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(350, 22);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // checkBoxOnlyShowerror
            // 
            this.checkBoxOnlyShowerror.AutoSize = true;
            this.checkBoxOnlyShowerror.Location = new System.Drawing.Point(3, 3);
            this.checkBoxOnlyShowerror.Name = "checkBoxOnlyShowerror";
            this.checkBoxOnlyShowerror.Size = new System.Drawing.Size(84, 16);
            this.checkBoxOnlyShowerror.TabIndex = 0;
            this.checkBoxOnlyShowerror.Text = "仅显示错误";
            this.checkBoxOnlyShowerror.UseVisualStyleBackColor = true;
            this.checkBoxOnlyShowerror.CheckedChanged += new System.EventHandler(this.checkBoxOnlyShowerror_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(591, 31);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvstudenterrorsubjectid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvstudentrightsubjectid);
            this.splitContainer1.Size = new System.Drawing.Size(181, 363);
            this.splitContainer1.SplitterDistance = 194;
            this.splitContainer1.TabIndex = 4;
            // 
            // dgvstudenterrorsubjectid
            // 
            this.dgvstudenterrorsubjectid.AllowUserToAddRows = false;
            this.dgvstudenterrorsubjectid.AllowUserToDeleteRows = false;
            this.dgvstudenterrorsubjectid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvstudenterrorsubjectid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvstudenterrorsubjectid.Location = new System.Drawing.Point(0, 0);
            this.dgvstudenterrorsubjectid.Name = "dgvstudenterrorsubjectid";
            this.dgvstudenterrorsubjectid.ReadOnly = true;
            this.dgvstudenterrorsubjectid.RowTemplate.Height = 23;
            this.dgvstudenterrorsubjectid.Size = new System.Drawing.Size(181, 194);
            this.dgvstudenterrorsubjectid.TabIndex = 0;
            this.dgvstudenterrorsubjectid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgverrorid_CellClick);
            this.dgvstudenterrorsubjectid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvstudenterrorsubjectid_CellDoubleClick);
            // 
            // dgvstudentrightsubjectid
            // 
            this.dgvstudentrightsubjectid.AllowUserToAddRows = false;
            this.dgvstudentrightsubjectid.AllowUserToDeleteRows = false;
            this.dgvstudentrightsubjectid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvstudentrightsubjectid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvstudentrightsubjectid.Location = new System.Drawing.Point(0, 0);
            this.dgvstudentrightsubjectid.Name = "dgvstudentrightsubjectid";
            this.dgvstudentrightsubjectid.ReadOnly = true;
            this.dgvstudentrightsubjectid.RowTemplate.Height = 23;
            this.dgvstudentrightsubjectid.Size = new System.Drawing.Size(181, 165);
            this.dgvstudentrightsubjectid.TabIndex = 0;
            this.dgvstudentrightsubjectid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvcorrectid_CellClick);
            this.dgvstudentrightsubjectid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvstudentrightsubjectid_CellDoubleClick);
            // 
            // tabPageBySubject
            // 
            this.tabPageBySubject.Controls.Add(this.tableLayoutPanel3);
            this.tabPageBySubject.Location = new System.Drawing.Point(4, 22);
            this.tabPageBySubject.Name = "tabPageBySubject";
            this.tabPageBySubject.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBySubject.Size = new System.Drawing.Size(781, 403);
            this.tabPageBySubject.TabIndex = 1;
            this.tabPageBySubject.Text = "按题";
            this.tabPageBySubject.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel3.Controls.Add(this.dgvsubjects, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel3, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.dgvSubjectRightErrorStudentList, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(775, 397);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // dgvsubjects
            // 
            this.dgvsubjects.AllowUserToAddRows = false;
            this.dgvsubjects.AllowUserToDeleteRows = false;
            this.dgvsubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvsubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvsubjects.Location = new System.Drawing.Point(3, 35);
            this.dgvsubjects.Name = "dgvsubjects";
            this.dgvsubjects.ReadOnly = true;
            this.dgvsubjects.RowTemplate.Height = 23;
            this.dgvsubjects.Size = new System.Drawing.Size(149, 359);
            this.dgvsubjects.TabIndex = 0;
            this.dgvsubjects.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvsubjects_CellClick);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.buttonShowOption, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonShowUnChoose, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(149, 26);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // buttonShowOption
            // 
            this.buttonShowOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowOption.Location = new System.Drawing.Point(3, 3);
            this.buttonShowOption.Name = "buttonShowOption";
            this.buttonShowOption.Size = new System.Drawing.Size(68, 20);
            this.buttonShowOption.TabIndex = 0;
            this.buttonShowOption.Text = "选择题";
            this.buttonShowOption.UseVisualStyleBackColor = true;
            this.buttonShowOption.Click += new System.EventHandler(this.buttonShowOption_Click);
            // 
            // buttonShowUnChoose
            // 
            this.buttonShowUnChoose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowUnChoose.Location = new System.Drawing.Point(77, 3);
            this.buttonShowUnChoose.Name = "buttonShowUnChoose";
            this.buttonShowUnChoose.Size = new System.Drawing.Size(69, 20);
            this.buttonShowUnChoose.TabIndex = 0;
            this.buttonShowUnChoose.Text = "非选择题";
            this.buttonShowUnChoose.UseVisualStyleBackColor = true;
            this.buttonShowUnChoose.Click += new System.EventHandler(this.buttonShowUnChoose_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.buttonShowRightStudentList);
            this.flowLayoutPanel3.Controls.Add(this.buttonShowErrorStudentList);
            this.flowLayoutPanel3.Controls.Add(this.textBoxMsg);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(158, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(614, 26);
            this.flowLayoutPanel3.TabIndex = 2;
            // 
            // buttonShowRightStudentList
            // 
            this.buttonShowRightStudentList.Location = new System.Drawing.Point(3, 3);
            this.buttonShowRightStudentList.Name = "buttonShowRightStudentList";
            this.buttonShowRightStudentList.Size = new System.Drawing.Size(132, 26);
            this.buttonShowRightStudentList.TabIndex = 0;
            this.buttonShowRightStudentList.Text = "显示正确名单";
            this.buttonShowRightStudentList.UseVisualStyleBackColor = true;
            this.buttonShowRightStudentList.Click += new System.EventHandler(this.buttonShowRightStudentList_Click);
            // 
            // buttonShowErrorStudentList
            // 
            this.buttonShowErrorStudentList.Location = new System.Drawing.Point(141, 3);
            this.buttonShowErrorStudentList.Name = "buttonShowErrorStudentList";
            this.buttonShowErrorStudentList.Size = new System.Drawing.Size(139, 26);
            this.buttonShowErrorStudentList.TabIndex = 0;
            this.buttonShowErrorStudentList.Text = "显示错误名单";
            this.buttonShowErrorStudentList.UseVisualStyleBackColor = true;
            this.buttonShowErrorStudentList.Click += new System.EventHandler(this.buttonShowErrorStudentList_Click);
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.Location = new System.Drawing.Point(286, 3);
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.ReadOnly = true;
            this.textBoxMsg.Size = new System.Drawing.Size(318, 21);
            this.textBoxMsg.TabIndex = 1;
            // 
            // dgvSubjectRightErrorStudentList
            // 
            this.dgvSubjectRightErrorStudentList.AllowUserToAddRows = false;
            this.dgvSubjectRightErrorStudentList.AllowUserToDeleteRows = false;
            this.dgvSubjectRightErrorStudentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubjectRightErrorStudentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSubjectRightErrorStudentList.Location = new System.Drawing.Point(158, 35);
            this.dgvSubjectRightErrorStudentList.Name = "dgvSubjectRightErrorStudentList";
            this.dgvSubjectRightErrorStudentList.ReadOnly = true;
            this.dgvSubjectRightErrorStudentList.RowTemplate.Height = 23;
            this.dgvSubjectRightErrorStudentList.Size = new System.Drawing.Size(614, 359);
            this.dgvSubjectRightErrorStudentList.TabIndex = 3;
            this.dgvSubjectRightErrorStudentList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRightErrorStudentList_CellClick);
            this.dgvSubjectRightErrorStudentList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubjectRightErrorStudentList_CellDoubleClick);
            // 
            // buttonZoomout
            // 
            this.buttonZoomout.Location = new System.Drawing.Point(93, 3);
            this.buttonZoomout.Name = "buttonZoomout";
            this.buttonZoomout.Size = new System.Drawing.Size(32, 18);
            this.buttonZoomout.TabIndex = 1;
            this.buttonZoomout.Text = "-";
            this.buttonZoomout.UseVisualStyleBackColor = true;
            this.buttonZoomout.Click += new System.EventHandler(this.buttonZoomout_Click);
            // 
            // buttonZoomin
            // 
            this.buttonZoomin.Location = new System.Drawing.Point(131, 3);
            this.buttonZoomin.Name = "buttonZoomin";
            this.buttonZoomin.Size = new System.Drawing.Size(32, 18);
            this.buttonZoomin.TabIndex = 1;
            this.buttonZoomin.Text = "+";
            this.buttonZoomin.UseVisualStyleBackColor = true;
            this.buttonZoomin.Click += new System.EventHandler(this.buttonZoomin_Click);
            // 
            // buttonZoomMouse
            // 
            this.buttonZoomMouse.Location = new System.Drawing.Point(169, 3);
            this.buttonZoomMouse.Name = "buttonZoomMouse";
            this.buttonZoomMouse.Size = new System.Drawing.Size(48, 18);
            this.buttonZoomMouse.TabIndex = 1;
            this.buttonZoomMouse.Text = "zoom";
            this.buttonZoomMouse.UseVisualStyleBackColor = true;
            this.buttonZoomMouse.Click += new System.EventHandler(this.buttonZoomMouse_Click);
            // 
            // FormReportTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 460);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormReportTable";
            this.Text = "FormReportTable";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageByStudent.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvstudent)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvstudenterrorsubjectid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvstudentrightsubjectid)).EndInit();
            this.tabPageBySubject.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvsubjects)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjectRightErrorStudentList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageByStudent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView dgvstudent;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.CheckBox checkBoxOnlyShowerror;
        private System.Windows.Forms.TabPage tabPageBySubject;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvstudentrightsubjectid;
        private System.Windows.Forms.DataGridView dgvstudenterrorsubjectid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView dgvsubjects;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button buttonShowOption;
        private System.Windows.Forms.Button buttonShowUnChoose;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button buttonShowRightStudentList;
        private System.Windows.Forms.Button buttonShowErrorStudentList;
        private System.Windows.Forms.TextBox textBoxMsg;
        private System.Windows.Forms.DataGridView dgvSubjectRightErrorStudentList;
        private System.Windows.Forms.Button buttonZoomout;
        private System.Windows.Forms.Button buttonZoomin;
        private System.Windows.Forms.Button buttonZoomMouse;
    }
}