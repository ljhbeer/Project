namespace ScanTemplate
{
    partial class FormM
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TLP = new System.Windows.Forms.TableLayoutPanel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBoxData = new System.Windows.Forms.ListBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.listBoxTemplate = new System.Windows.Forms.ListBox();
            this.buttonUseTemplate = new System.Windows.Forms.Button();
            this.buttonGo = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonworkpath = new System.Windows.Forms.Button();
            this.textBoxWorkPath = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonLeftHide = new System.Windows.Forms.Button();
            this.buttonRightHide = new System.Windows.Forms.Button();
            this.buttonCreateYJData = new System.Windows.Forms.Button();
            this.buttonVerify = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.TLP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1264, 682);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1264, 25);
            this.panel2.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.TLP, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1264, 653);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // TLP
            // 
            this.TLP.ColumnCount = 3;
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TLP.Controls.Add(this.dgv, 1, 0);
            this.TLP.Controls.Add(this.panel1, 0, 0);
            this.TLP.Controls.Add(this.panel3, 2, 0);
            this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP.Location = new System.Drawing.Point(3, 3);
            this.TLP.Name = "TLP";
            this.TLP.RowCount = 1;
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP.Size = new System.Drawing.Size(1258, 613);
            this.TLP.TabIndex = 0;
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(254, 3);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(748, 607);
            this.dgv.TabIndex = 1;
            this.dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.buttonRefresh);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.listBoxData);
            this.panel1.Controls.Add(this.buttonScan);
            this.panel1.Controls.Add(this.listBoxTemplate);
            this.panel1.Controls.Add(this.buttonUseTemplate);
            this.panel1.Controls.Add(this.buttonGo);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonworkpath);
            this.panel1.Controls.Add(this.textBoxWorkPath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 607);
            this.panel1.TabIndex = 2;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(65, 6);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(42, 22);
            this.buttonRefresh.TabIndex = 12;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 389);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(228, 21);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "已导入数据";
            // 
            // listBoxData
            // 
            this.listBoxData.FormattingEnabled = true;
            this.listBoxData.ItemHeight = 12;
            this.listBoxData.Location = new System.Drawing.Point(8, 416);
            this.listBoxData.Name = "listBoxData";
            this.listBoxData.Size = new System.Drawing.Size(228, 100);
            this.listBoxData.TabIndex = 10;
            this.listBoxData.SelectedIndexChanged += new System.EventHandler(this.listBoxData_SelectedIndexChanged);
            // 
            // buttonScan
            // 
            this.buttonScan.Location = new System.Drawing.Point(8, 522);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(135, 32);
            this.buttonScan.TabIndex = 9;
            this.buttonScan.Text = "使用现有模板扫描";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.ButtonScanClick);
            // 
            // listBoxTemplate
            // 
            this.listBoxTemplate.FormattingEnabled = true;
            this.listBoxTemplate.ItemHeight = 12;
            this.listBoxTemplate.Location = new System.Drawing.Point(8, 247);
            this.listBoxTemplate.Name = "listBoxTemplate";
            this.listBoxTemplate.Size = new System.Drawing.Size(228, 136);
            this.listBoxTemplate.TabIndex = 8;
            this.listBoxTemplate.SelectedIndexChanged += new System.EventHandler(this.listBoxTemplate_SelectedIndexChanged);
            // 
            // buttonUseTemplate
            // 
            this.buttonUseTemplate.Location = new System.Drawing.Point(101, 209);
            this.buttonUseTemplate.Name = "buttonUseTemplate";
            this.buttonUseTemplate.Size = new System.Drawing.Size(135, 29);
            this.buttonUseTemplate.TabIndex = 7;
            this.buttonUseTemplate.Text = "使用现有配套模板";
            this.buttonUseTemplate.UseVisualStyleBackColor = true;
            this.buttonUseTemplate.Click += new System.EventHandler(this.ButtonUseTemplateClick);
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(8, 209);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(87, 29);
            this.buttonGo.TabIndex = 6;
            this.buttonGo.Text = "创建新模板";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(8, 58);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(228, 148);
            this.listBox1.TabIndex = 5;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "工作目录";
            // 
            // buttonworkpath
            // 
            this.buttonworkpath.Location = new System.Drawing.Point(146, 6);
            this.buttonworkpath.Name = "buttonworkpath";
            this.buttonworkpath.Size = new System.Drawing.Size(90, 22);
            this.buttonworkpath.TabIndex = 4;
            this.buttonworkpath.Text = "更改工作目录";
            this.buttonworkpath.UseVisualStyleBackColor = true;
            this.buttonworkpath.Click += new System.EventHandler(this.buttonworkpath_Click);
            // 
            // textBoxWorkPath
            // 
            this.textBoxWorkPath.Location = new System.Drawing.Point(8, 31);
            this.textBoxWorkPath.Name = "textBoxWorkPath";
            this.textBoxWorkPath.Size = new System.Drawing.Size(228, 21);
            this.textBoxWorkPath.TabIndex = 3;
            this.textBoxWorkPath.Text = "E:\\Scan\\LJH\\s1025";
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(1008, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(247, 607);
            this.panel3.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(3, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(247, 581);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.Controls.Add(this.buttonLeftHide, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonRightHide, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonCreateYJData, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonVerify, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 622);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1258, 28);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // buttonLeftHide
            // 
            this.buttonLeftHide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLeftHide.Location = new System.Drawing.Point(3, 3);
            this.buttonLeftHide.Name = "buttonLeftHide";
            this.buttonLeftHide.Size = new System.Drawing.Size(44, 22);
            this.buttonLeftHide.TabIndex = 0;
            this.buttonLeftHide.Text = "<<";
            this.buttonLeftHide.UseVisualStyleBackColor = true;
            this.buttonLeftHide.Click += new System.EventHandler(this.buttonLeftHide_Click);
            // 
            // buttonRightHide
            // 
            this.buttonRightHide.Location = new System.Drawing.Point(1211, 3);
            this.buttonRightHide.Name = "buttonRightHide";
            this.buttonRightHide.Size = new System.Drawing.Size(44, 17);
            this.buttonRightHide.TabIndex = 3;
            this.buttonRightHide.Text = ">>";
            this.buttonRightHide.UseVisualStyleBackColor = true;
            this.buttonRightHide.Click += new System.EventHandler(this.buttonRightHide_Click);
            // 
            // buttonCreateYJData
            // 
            this.buttonCreateYJData.Location = new System.Drawing.Point(103, 3);
            this.buttonCreateYJData.Name = "buttonCreateYJData";
            this.buttonCreateYJData.Size = new System.Drawing.Size(135, 22);
            this.buttonCreateYJData.TabIndex = 9;
            this.buttonCreateYJData.Text = "生成阅卷数据";
            this.buttonCreateYJData.UseVisualStyleBackColor = true;
            this.buttonCreateYJData.Click += new System.EventHandler(this.buttonCreateYJData_Click);
            // 
            // buttonVerify
            // 
            this.buttonVerify.Location = new System.Drawing.Point(53, 3);
            this.buttonVerify.Name = "buttonVerify";
            this.buttonVerify.Size = new System.Drawing.Size(44, 22);
            this.buttonVerify.TabIndex = 4;
            this.buttonVerify.Text = "校验";
            this.buttonVerify.UseVisualStyleBackColor = true;
            this.buttonVerify.Click += new System.EventHandler(this.buttonVerify_Click);
            // 
            // FormM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormM";
            this.Text = "FormM";
            this.Load += new System.EventHandler(this.FormM_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.TLP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.Button buttonUseTemplate;
        private System.Windows.Forms.ListBox listBoxTemplate;

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel TLP;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonLeftHide;
        private System.Windows.Forms.Button buttonRightHide;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxWorkPath;
        private System.Windows.Forms.Button buttonworkpath;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBoxData;
        private System.Windows.Forms.Button buttonVerify;
        private System.Windows.Forms.Button buttonCreateYJData;
        private System.Windows.Forms.Button buttonRefresh;        
    }
}