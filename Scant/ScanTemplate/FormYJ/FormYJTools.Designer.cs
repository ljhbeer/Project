namespace ScanTemplate.FormYJ
{
    partial class FormYJTools
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonBeginYJ = new System.Windows.Forms.Button();
            this.textBoxWorkPath = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.buttonShowXztSet = new System.Windows.Forms.Button();
            this.buttonShowFXztSet = new System.Windows.Forms.Button();
            this.buttonImportOptionAnswerScore = new System.Windows.Forms.Button();
            this.buttonShowStudents = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExportResult = new System.Windows.Forms.Button();
            this.buttonImportImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(629, 406);
            this.splitContainer1.SplitterDistance = 197;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.83756F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.16244F));
            this.tableLayoutPanel1.Controls.Add(this.buttonRefresh, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonBeginYJ, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxWorkPath, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(197, 406);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRefresh.Location = new System.Drawing.Point(112, 3);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(82, 24);
            this.buttonRefresh.TabIndex = 13;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // listBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.listBox1, 2);
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 33);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(191, 328);
            this.listBox1.TabIndex = 6;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // buttonBeginYJ
            // 
            this.buttonBeginYJ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBeginYJ.Location = new System.Drawing.Point(112, 367);
            this.buttonBeginYJ.Name = "buttonBeginYJ";
            this.buttonBeginYJ.Size = new System.Drawing.Size(82, 36);
            this.buttonBeginYJ.TabIndex = 7;
            this.buttonBeginYJ.Text = "开始阅卷";
            this.buttonBeginYJ.UseVisualStyleBackColor = true;
            this.buttonBeginYJ.Click += new System.EventHandler(this.buttonBeginYJ_Click);
            // 
            // textBoxWorkPath
            // 
            this.textBoxWorkPath.Location = new System.Drawing.Point(3, 3);
            this.textBoxWorkPath.Name = "textBoxWorkPath";
            this.textBoxWorkPath.ReadOnly = true;
            this.textBoxWorkPath.Size = new System.Drawing.Size(92, 21);
            this.textBoxWorkPath.TabIndex = 3;
            this.textBoxWorkPath.Text = "E:\\Scan\\LJH\\s1025";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(428, 406);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.dgv, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonShowXztSet, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonShowFXztSet, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonImportOptionAnswerScore, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonShowStudents, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(208, 400);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel3.SetColumnSpan(this.dgv, 4);
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(3, 32);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 50;
            this.dgv.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.Size = new System.Drawing.Size(202, 365);
            this.dgv.TabIndex = 2;
            this.dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellContentClick);
            // 
            // buttonShowXztSet
            // 
            this.buttonShowXztSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowXztSet.Location = new System.Drawing.Point(3, 3);
            this.buttonShowXztSet.Name = "buttonShowXztSet";
            this.buttonShowXztSet.Size = new System.Drawing.Size(46, 23);
            this.buttonShowXztSet.TabIndex = 3;
            this.buttonShowXztSet.Text = "选择题";
            this.buttonShowXztSet.UseVisualStyleBackColor = true;
            this.buttonShowXztSet.Click += new System.EventHandler(this.buttonShowXztSet_Click);
            // 
            // buttonShowFXztSet
            // 
            this.buttonShowFXztSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowFXztSet.Location = new System.Drawing.Point(55, 3);
            this.buttonShowFXztSet.Name = "buttonShowFXztSet";
            this.buttonShowFXztSet.Size = new System.Drawing.Size(46, 23);
            this.buttonShowFXztSet.TabIndex = 4;
            this.buttonShowFXztSet.Text = "非选择题";
            this.buttonShowFXztSet.UseVisualStyleBackColor = true;
            this.buttonShowFXztSet.Click += new System.EventHandler(this.buttonShowFXztSet_Click);
            // 
            // buttonImportOptionAnswerScore
            // 
            this.buttonImportOptionAnswerScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImportOptionAnswerScore.Location = new System.Drawing.Point(159, 3);
            this.buttonImportOptionAnswerScore.Name = "buttonImportOptionAnswerScore";
            this.buttonImportOptionAnswerScore.Size = new System.Drawing.Size(46, 23);
            this.buttonImportOptionAnswerScore.TabIndex = 4;
            this.buttonImportOptionAnswerScore.Text = "导入选择题答案";
            this.buttonImportOptionAnswerScore.UseVisualStyleBackColor = true;
            this.buttonImportOptionAnswerScore.Click += new System.EventHandler(this.buttonImportOptionAnswerScore_Click);
            // 
            // buttonShowStudents
            // 
            this.buttonShowStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowStudents.Location = new System.Drawing.Point(107, 3);
            this.buttonShowStudents.Name = "buttonShowStudents";
            this.buttonShowStudents.Size = new System.Drawing.Size(46, 23);
            this.buttonShowStudents.TabIndex = 5;
            this.buttonShowStudents.Text = "考生";
            this.buttonShowStudents.UseVisualStyleBackColor = true;
            this.buttonShowStudents.Click += new System.EventHandler(this.buttonShowStudents_Click);
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(217, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(208, 400);
            this.panel3.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(202, 394);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonImportImage);
            this.panel1.Controls.Add(this.buttonExportResult);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 367);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(103, 36);
            this.panel1.TabIndex = 14;
            // 
            // buttonExportResult
            // 
            this.buttonExportResult.Location = new System.Drawing.Point(0, 0);
            this.buttonExportResult.Name = "buttonExportResult";
            this.buttonExportResult.Size = new System.Drawing.Size(62, 33);
            this.buttonExportResult.TabIndex = 8;
            this.buttonExportResult.Text = "导出成绩";
            this.buttonExportResult.UseVisualStyleBackColor = true;
            this.buttonExportResult.Click += new System.EventHandler(this.buttonExportResult_Click);
            // 
            // buttonImportImage
            // 
            this.buttonImportImage.Location = new System.Drawing.Point(59, 3);
            this.buttonImportImage.Name = "buttonImportImage";
            this.buttonImportImage.Size = new System.Drawing.Size(44, 29);
            this.buttonImportImage.TabIndex = 9;
            this.buttonImportImage.Text = "图片";
            this.buttonImportImage.UseVisualStyleBackColor = true;
            this.buttonImportImage.Click += new System.EventHandler(this.buttonImportImage_Click);
            // 
            // FormYJTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 406);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormYJTools";
            this.Text = "FormYJTools";
            this.Load += new System.EventHandler(this.FormYJTools_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonBeginYJ;
        private System.Windows.Forms.TextBox textBoxWorkPath;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button buttonShowXztSet;
        private System.Windows.Forms.Button buttonShowFXztSet;
        private System.Windows.Forms.Button buttonImportOptionAnswerScore;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonShowStudents;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonImportImage;
        private System.Windows.Forms.Button buttonExportResult;
    }
}