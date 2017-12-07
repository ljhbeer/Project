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
            this.buttonModifyData = new System.Windows.Forms.Button();
            this.buttonBeginYJ = new System.Windows.Forms.Button();
            this.textBoxWorkPath = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.buttonShowXztSet = new System.Windows.Forms.Button();
            this.buttonShowFXztSet = new System.Windows.Forms.Button();
            this.buttonImportOptionAnswerScore = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.buttonRefresh, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonModifyData, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonBeginYJ, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxWorkPath, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(197, 406);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRefresh.Location = new System.Drawing.Point(101, 3);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(93, 24);
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
            this.listBox1.Size = new System.Drawing.Size(191, 340);
            this.listBox1.TabIndex = 6;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // buttonModifyData
            // 
            this.buttonModifyData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonModifyData.Location = new System.Drawing.Point(3, 379);
            this.buttonModifyData.Name = "buttonModifyData";
            this.buttonModifyData.Size = new System.Drawing.Size(92, 24);
            this.buttonModifyData.TabIndex = 7;
            this.buttonModifyData.Text = "更改数据";
            this.buttonModifyData.UseVisualStyleBackColor = true;
            this.buttonModifyData.Click += new System.EventHandler(this.buttonModifyData_Click);
            // 
            // buttonBeginYJ
            // 
            this.buttonBeginYJ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBeginYJ.Location = new System.Drawing.Point(101, 379);
            this.buttonBeginYJ.Name = "buttonBeginYJ";
            this.buttonBeginYJ.Size = new System.Drawing.Size(93, 24);
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
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.dgv, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonShowXztSet, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonShowFXztSet, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonImportOptionAnswerScore, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(208, 400);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel3.SetColumnSpan(this.dgv, 3);
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(3, 27);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 50;
            this.dgv.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.Size = new System.Drawing.Size(202, 370);
            this.dgv.TabIndex = 2;
            // 
            // buttonShowXztSet
            // 
            this.buttonShowXztSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowXztSet.Location = new System.Drawing.Point(3, 3);
            this.buttonShowXztSet.Name = "buttonShowXztSet";
            this.buttonShowXztSet.Size = new System.Drawing.Size(63, 18);
            this.buttonShowXztSet.TabIndex = 3;
            this.buttonShowXztSet.Text = "选择题";
            this.buttonShowXztSet.UseVisualStyleBackColor = true;
            this.buttonShowXztSet.Click += new System.EventHandler(this.buttonShowXztSet_Click);
            // 
            // buttonShowFXztSet
            // 
            this.buttonShowFXztSet.Location = new System.Drawing.Point(72, 3);
            this.buttonShowFXztSet.Name = "buttonShowFXztSet";
            this.buttonShowFXztSet.Size = new System.Drawing.Size(63, 18);
            this.buttonShowFXztSet.TabIndex = 4;
            this.buttonShowFXztSet.Text = "非选择题";
            this.buttonShowFXztSet.UseVisualStyleBackColor = true;
            this.buttonShowFXztSet.Click += new System.EventHandler(this.buttonShowFXztSet_Click);
            // 
            // buttonImportOptionAnswerScore
            // 
            this.buttonImportOptionAnswerScore.Location = new System.Drawing.Point(141, 3);
            this.buttonImportOptionAnswerScore.Name = "buttonImportOptionAnswerScore";
            this.buttonImportOptionAnswerScore.Size = new System.Drawing.Size(64, 18);
            this.buttonImportOptionAnswerScore.TabIndex = 4;
            this.buttonImportOptionAnswerScore.Text = "导入选择题答案";
            this.buttonImportOptionAnswerScore.UseVisualStyleBackColor = true;
            this.buttonImportOptionAnswerScore.Click += new System.EventHandler(this.buttonImportOptionAnswerScore_Click);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonModifyData;
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
    }
}