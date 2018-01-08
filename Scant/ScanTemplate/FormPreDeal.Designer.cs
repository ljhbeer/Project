namespace ScanTemplate
{
    partial class FormPreDeal
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxUnScanDir = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCutbySelection = new System.Windows.Forms.Button();
            this.buttonApplyAll = new System.Windows.Forms.Button();
            this.buttonOutRectWhite = new System.Windows.Forms.Button();
            this.listBoxfilename = new System.Windows.Forms.ListBox();
            this.listBoxNewfilename = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.ll = new System.Windows.Forms.Label();
            this.textBoxfilenamereplacedst = new System.Windows.Forms.TextBox();
            this.textBoxfilenamereplacesrc = new System.Windows.Forms.TextBox();
            this.buttonReName = new System.Windows.Forms.Button();
            this.buttonModifyNewFilename = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonSaveActiveImage = new System.Windows.Forms.Button();
            this.buttonApplyCutToAllImage = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.listBoxUnScanDir, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBox2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.listBoxfilename, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBoxNewfilename, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(682, 397);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "扫描结果";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Exam";
            this.label2.Visible = false;
            // 
            // listBoxUnScanDir
            // 
            this.listBoxUnScanDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUnScanDir.FormattingEnabled = true;
            this.listBoxUnScanDir.ItemHeight = 12;
            this.listBoxUnScanDir.Location = new System.Drawing.Point(3, 23);
            this.listBoxUnScanDir.Name = "listBoxUnScanDir";
            this.listBoxUnScanDir.Size = new System.Drawing.Size(94, 226);
            this.listBoxUnScanDir.TabIndex = 1;
            this.listBoxUnScanDir.SelectedIndexChanged += new System.EventHandler(this.listBoxUnScanDir_SelectedIndexChanged);
            // 
            // listBox2
            // 
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(3, 322);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(94, 52);
            this.listBox2.TabIndex = 2;
            this.listBox2.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.pictureBox1.Location = new System.Drawing.Point(3, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(264, 208);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonApplyCutToAllImage);
            this.panel1.Controls.Add(this.buttonCutbySelection);
            this.panel1.Controls.Add(this.buttonSaveActiveImage);
            this.panel1.Controls.Add(this.buttonApplyAll);
            this.panel1.Controls.Add(this.buttonOutRectWhite);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(403, 322);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 52);
            this.panel1.TabIndex = 5;
            // 
            // buttonCutbySelection
            // 
            this.buttonCutbySelection.Location = new System.Drawing.Point(54, 0);
            this.buttonCutbySelection.Name = "buttonCutbySelection";
            this.buttonCutbySelection.Size = new System.Drawing.Size(37, 37);
            this.buttonCutbySelection.TabIndex = 3;
            this.buttonCutbySelection.Text = "剪切选区";
            this.buttonCutbySelection.UseVisualStyleBackColor = true;
            this.buttonCutbySelection.Click += new System.EventHandler(this.buttonCutbySelection_Click);
            // 
            // buttonApplyAll
            // 
            this.buttonApplyAll.Location = new System.Drawing.Point(142, 0);
            this.buttonApplyAll.Name = "buttonApplyAll";
            this.buttonApplyAll.Size = new System.Drawing.Size(62, 37);
            this.buttonApplyAll.TabIndex = 2;
            this.buttonApplyAll.Text = "应用白色到所有图片";
            this.buttonApplyAll.UseVisualStyleBackColor = true;
            this.buttonApplyAll.Click += new System.EventHandler(this.buttonApplyAll_Click);
            // 
            // buttonOutRectWhite
            // 
            this.buttonOutRectWhite.Location = new System.Drawing.Point(3, 0);
            this.buttonOutRectWhite.Name = "buttonOutRectWhite";
            this.buttonOutRectWhite.Size = new System.Drawing.Size(51, 37);
            this.buttonOutRectWhite.TabIndex = 1;
            this.buttonOutRectWhite.Text = "选区外侧白色";
            this.buttonOutRectWhite.UseVisualStyleBackColor = true;
            this.buttonOutRectWhite.Click += new System.EventHandler(this.buttonOutRectWhite_Click);
            // 
            // listBoxfilename
            // 
            this.listBoxfilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxfilename.FormattingEnabled = true;
            this.listBoxfilename.ItemHeight = 12;
            this.listBoxfilename.Location = new System.Drawing.Point(103, 23);
            this.listBoxfilename.Name = "listBoxfilename";
            this.tableLayoutPanel1.SetRowSpan(this.listBoxfilename, 3);
            this.listBoxfilename.Size = new System.Drawing.Size(144, 351);
            this.listBoxfilename.TabIndex = 6;
            this.listBoxfilename.SelectedIndexChanged += new System.EventHandler(this.listBoxfilename_SelectedIndexChanged);
            // 
            // listBoxNewfilename
            // 
            this.listBoxNewfilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxNewfilename.FormattingEnabled = true;
            this.listBoxNewfilename.ItemHeight = 12;
            this.listBoxNewfilename.Location = new System.Drawing.Point(253, 23);
            this.listBoxNewfilename.Name = "listBoxNewfilename";
            this.tableLayoutPanel1.SetRowSpan(this.listBoxNewfilename, 3);
            this.listBoxNewfilename.Size = new System.Drawing.Size(144, 351);
            this.listBoxNewfilename.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.ll);
            this.panel2.Controls.Add(this.textBoxfilenamereplacedst);
            this.panel2.Controls.Add(this.textBoxfilenamereplacesrc);
            this.panel2.Controls.Add(this.buttonReName);
            this.panel2.Controls.Add(this.buttonModifyNewFilename);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(403, 255);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(276, 61);
            this.panel2.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "->";
            // 
            // ll
            // 
            this.ll.AutoSize = true;
            this.ll.Location = new System.Drawing.Point(7, 6);
            this.ll.Name = "ll";
            this.ll.Size = new System.Drawing.Size(47, 12);
            this.ll.TabIndex = 1;
            this.ll.Text = "Replace";
            // 
            // textBoxfilenamereplacedst
            // 
            this.textBoxfilenamereplacedst.Location = new System.Drawing.Point(175, 3);
            this.textBoxfilenamereplacedst.Name = "textBoxfilenamereplacedst";
            this.textBoxfilenamereplacedst.Size = new System.Drawing.Size(100, 21);
            this.textBoxfilenamereplacedst.TabIndex = 0;
            // 
            // textBoxfilenamereplacesrc
            // 
            this.textBoxfilenamereplacesrc.Location = new System.Drawing.Point(56, 3);
            this.textBoxfilenamereplacesrc.Name = "textBoxfilenamereplacesrc";
            this.textBoxfilenamereplacesrc.Size = new System.Drawing.Size(100, 21);
            this.textBoxfilenamereplacesrc.TabIndex = 0;
            // 
            // buttonReName
            // 
            this.buttonReName.Location = new System.Drawing.Point(94, 21);
            this.buttonReName.Name = "buttonReName";
            this.buttonReName.Size = new System.Drawing.Size(85, 26);
            this.buttonReName.TabIndex = 0;
            this.buttonReName.Text = "重命名文件名";
            this.buttonReName.UseVisualStyleBackColor = true;
            this.buttonReName.Click += new System.EventHandler(this.buttonReName_Click);
            // 
            // buttonModifyNewFilename
            // 
            this.buttonModifyNewFilename.Location = new System.Drawing.Point(3, 21);
            this.buttonModifyNewFilename.Name = "buttonModifyNewFilename";
            this.buttonModifyNewFilename.Size = new System.Drawing.Size(85, 26);
            this.buttonModifyNewFilename.TabIndex = 0;
            this.buttonModifyNewFilename.Text = "更新文件名";
            this.buttonModifyNewFilename.UseVisualStyleBackColor = true;
            this.buttonModifyNewFilename.Click += new System.EventHandler(this.buttonModifyNewFilename_Click);
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(403, 23);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(276, 226);
            this.panel3.TabIndex = 9;
            // 
            // buttonSaveActiveImage
            // 
            this.buttonSaveActiveImage.Location = new System.Drawing.Point(91, 0);
            this.buttonSaveActiveImage.Name = "buttonSaveActiveImage";
            this.buttonSaveActiveImage.Size = new System.Drawing.Size(51, 37);
            this.buttonSaveActiveImage.TabIndex = 2;
            this.buttonSaveActiveImage.Text = "保存当前图片";
            this.buttonSaveActiveImage.UseVisualStyleBackColor = true;
            this.buttonSaveActiveImage.Click += new System.EventHandler(this.buttonSaveActiveImage_Click);
            // 
            // buttonApplyCutToAllImage
            // 
            this.buttonApplyCutToAllImage.Location = new System.Drawing.Point(205, 0);
            this.buttonApplyCutToAllImage.Name = "buttonApplyCutToAllImage";
            this.buttonApplyCutToAllImage.Size = new System.Drawing.Size(62, 37);
            this.buttonApplyCutToAllImage.TabIndex = 4;
            this.buttonApplyCutToAllImage.Text = "应用剪切到所有图片";
            this.buttonApplyCutToAllImage.UseVisualStyleBackColor = true;
            this.buttonApplyCutToAllImage.Click += new System.EventHandler(this.buttonApplyCutToAllImage_Click);
            // 
            // FormPreDeal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 397);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormPreDeal";
            this.Text = "FormPreDeal";
            this.Load += new System.EventHandler(this.FormPreDeal_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxUnScanDir;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonCutbySelection;
        private System.Windows.Forms.Button buttonApplyAll;
        private System.Windows.Forms.Button buttonOutRectWhite;
        private System.Windows.Forms.Button buttonModifyNewFilename;
        private System.Windows.Forms.ListBox listBoxfilename;
        private System.Windows.Forms.ListBox listBoxNewfilename;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label ll;
        private System.Windows.Forms.TextBox textBoxfilenamereplacesrc;
        private System.Windows.Forms.Button buttonReName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxfilenamereplacedst;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonSaveActiveImage;
        private System.Windows.Forms.Button buttonApplyCutToAllImage;
    }
}