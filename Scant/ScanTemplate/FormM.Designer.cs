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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCreateYJData = new System.Windows.Forms.Button();
            this.buttonVerify = new System.Windows.Forms.Button();
            this.buttonUnScanNameList = new System.Windows.Forms.Button();
            this.buttonVerifyname = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTemplate = new System.Windows.Forms.ComboBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.listBoxScantData = new System.Windows.Forms.ListBox();
            this.buttonReScan = new System.Windows.Forms.Button();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonMatchTemplate = new System.Windows.Forms.Button();
            this.buttonCreateTemplate = new System.Windows.Forms.Button();
            this.listBoxUnScanDir = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonworkpath = new System.Windows.Forms.Button();
            this.textBoxWorkPath = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOpenTemplate = new System.Windows.Forms.Button();
            this.buttonLeftHide = new System.Windows.Forms.Button();
            this.buttonRightHide = new System.Windows.Forms.Button();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.checkBoxDebug = new System.Windows.Forms.CheckBox();
            this.buttonOutTextImage = new System.Windows.Forms.Button();
            this.buttonPreDealImage = new System.Windows.Forms.Button();
            this.checkBoxShowUnScanNamelist = new System.Windows.Forms.CheckBox();
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
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
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
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.buttonCreateYJData);
            this.panel1.Controls.Add(this.buttonVerify);
            this.panel1.Controls.Add(this.buttonUnScanNameList);
            this.panel1.Controls.Add(this.buttonVerifyname);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.comboBoxTemplate);
            this.panel1.Controls.Add(this.buttonRefresh);
            this.panel1.Controls.Add(this.listBoxScantData);
            this.panel1.Controls.Add(this.buttonReScan);
            this.panel1.Controls.Add(this.buttonScan);
            this.panel1.Controls.Add(this.buttonMatchTemplate);
            this.panel1.Controls.Add(this.buttonCreateTemplate);
            this.panel1.Controls.Add(this.listBoxUnScanDir);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonworkpath);
            this.panel1.Controls.Add(this.textBoxWorkPath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 607);
            this.panel1.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "未扫描目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 247);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "已扫描的数据";
            // 
            // buttonCreateYJData
            // 
            this.buttonCreateYJData.Location = new System.Drawing.Point(184, 569);
            this.buttonCreateYJData.Name = "buttonCreateYJData";
            this.buttonCreateYJData.Size = new System.Drawing.Size(53, 38);
            this.buttonCreateYJData.TabIndex = 9;
            this.buttonCreateYJData.Text = "生成阅卷数据";
            this.buttonCreateYJData.UseVisualStyleBackColor = true;
            this.buttonCreateYJData.Click += new System.EventHandler(this.buttonCreateYJData_Click);
            // 
            // buttonVerify
            // 
            this.buttonVerify.Location = new System.Drawing.Point(49, 569);
            this.buttonVerify.Name = "buttonVerify";
            this.buttonVerify.Size = new System.Drawing.Size(43, 38);
            this.buttonVerify.TabIndex = 4;
            this.buttonVerify.Text = "校验";
            this.buttonVerify.UseVisualStyleBackColor = true;
            this.buttonVerify.Click += new System.EventHandler(this.buttonVerify_Click);
            // 
            // buttonUnScanNameList
            // 
            this.buttonUnScanNameList.Location = new System.Drawing.Point(139, 569);
            this.buttonUnScanNameList.Name = "buttonUnScanNameList";
            this.buttonUnScanNameList.Size = new System.Drawing.Size(43, 38);
            this.buttonUnScanNameList.TabIndex = 10;
            this.buttonUnScanNameList.Text = "未交名单";
            this.buttonUnScanNameList.UseVisualStyleBackColor = true;
            this.buttonUnScanNameList.Click += new System.EventHandler(this.buttonUnScanNameList_Click);
            // 
            // buttonVerifyname
            // 
            this.buttonVerifyname.Location = new System.Drawing.Point(94, 569);
            this.buttonVerifyname.Name = "buttonVerifyname";
            this.buttonVerifyname.Size = new System.Drawing.Size(43, 38);
            this.buttonVerifyname.TabIndex = 10;
            this.buttonVerifyname.Text = "核对姓名";
            this.buttonVerifyname.UseVisualStyleBackColor = true;
            this.buttonVerifyname.Click += new System.EventHandler(this.buttonVerifyname_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "模板";
            // 
            // comboBoxTemplate
            // 
            this.comboBoxTemplate.FormattingEnabled = true;
            this.comboBoxTemplate.Location = new System.Drawing.Point(52, 191);
            this.comboBoxTemplate.Name = "comboBoxTemplate";
            this.comboBoxTemplate.Size = new System.Drawing.Size(184, 20);
            this.comboBoxTemplate.TabIndex = 13;
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
            // listBoxScantData
            // 
            this.listBoxScantData.FormattingEnabled = true;
            this.listBoxScantData.ItemHeight = 12;
            this.listBoxScantData.Location = new System.Drawing.Point(3, 265);
            this.listBoxScantData.Name = "listBoxScantData";
            this.listBoxScantData.Size = new System.Drawing.Size(239, 292);
            this.listBoxScantData.TabIndex = 10;
            this.listBoxScantData.SelectedIndexChanged += new System.EventHandler(this.listBoxData_SelectedIndexChanged);
            this.listBoxScantData.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxScantData_KeyUp);
            // 
            // buttonReScan
            // 
            this.buttonReScan.Location = new System.Drawing.Point(4, 569);
            this.buttonReScan.Name = "buttonReScan";
            this.buttonReScan.Size = new System.Drawing.Size(43, 38);
            this.buttonReScan.TabIndex = 9;
            this.buttonReScan.Text = "重新扫描";
            this.buttonReScan.UseVisualStyleBackColor = true;
            this.buttonReScan.Click += new System.EventHandler(this.buttonReScan_Click);
            // 
            // buttonScan
            // 
            this.buttonScan.Location = new System.Drawing.Point(160, 215);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(79, 29);
            this.buttonScan.TabIndex = 9;
            this.buttonScan.Text = "扫描";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.ButtonScanClick);
            // 
            // buttonMatchTemplate
            // 
            this.buttonMatchTemplate.Location = new System.Drawing.Point(79, 215);
            this.buttonMatchTemplate.Name = "buttonMatchTemplate";
            this.buttonMatchTemplate.Size = new System.Drawing.Size(78, 29);
            this.buttonMatchTemplate.TabIndex = 7;
            this.buttonMatchTemplate.Text = "套用模板";
            this.buttonMatchTemplate.UseVisualStyleBackColor = true;
            this.buttonMatchTemplate.Click += new System.EventHandler(this.buttonMatchTemplate_Click);
            // 
            // buttonCreateTemplate
            // 
            this.buttonCreateTemplate.Location = new System.Drawing.Point(9, 215);
            this.buttonCreateTemplate.Name = "buttonCreateTemplate";
            this.buttonCreateTemplate.Size = new System.Drawing.Size(69, 29);
            this.buttonCreateTemplate.TabIndex = 6;
            this.buttonCreateTemplate.Text = "创建模板";
            this.buttonCreateTemplate.UseVisualStyleBackColor = true;
            this.buttonCreateTemplate.Click += new System.EventHandler(this.buttonCreateTemplate_Click);
            // 
            // listBoxUnScanDir
            // 
            this.listBoxUnScanDir.FormattingEnabled = true;
            this.listBoxUnScanDir.ItemHeight = 12;
            this.listBoxUnScanDir.Location = new System.Drawing.Point(10, 76);
            this.listBoxUnScanDir.Name = "listBoxUnScanDir";
            this.listBoxUnScanDir.Size = new System.Drawing.Size(224, 112);
            this.listBoxUnScanDir.TabIndex = 5;
            this.listBoxUnScanDir.SelectedIndexChanged += new System.EventHandler(this.listBoxUnScanDir_SelectedIndexChanged);
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
            this.textBoxWorkPath.Text = "E:\\Scan\\s1025";
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
            this.tableLayoutPanel2.ColumnCount = 9;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel2.Controls.Add(this.buttonOpenTemplate, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonLeftHide, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonRightHide, 8, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxMsg, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxDebug, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonOutTextImage, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonPreDealImage, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxShowUnScanNamelist, 6, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 622);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1258, 28);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // buttonOpenTemplate
            // 
            this.buttonOpenTemplate.Location = new System.Drawing.Point(53, 3);
            this.buttonOpenTemplate.Name = "buttonOpenTemplate";
            this.buttonOpenTemplate.Size = new System.Drawing.Size(43, 22);
            this.buttonOpenTemplate.TabIndex = 10;
            this.buttonOpenTemplate.Text = "模板";
            this.buttonOpenTemplate.UseVisualStyleBackColor = true;
            this.buttonOpenTemplate.Click += new System.EventHandler(this.buttonOpenTemplate_Click);
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
            this.buttonRightHide.Location = new System.Drawing.Point(1210, 3);
            this.buttonRightHide.Name = "buttonRightHide";
            this.buttonRightHide.Size = new System.Drawing.Size(44, 17);
            this.buttonRightHide.TabIndex = 3;
            this.buttonRightHide.Text = ">>";
            this.buttonRightHide.UseVisualStyleBackColor = true;
            this.buttonRightHide.Click += new System.EventHandler(this.buttonRightHide_Click);
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.BackColor = System.Drawing.SystemColors.InfoText;
            this.textBoxMsg.ForeColor = System.Drawing.Color.Red;
            this.textBoxMsg.Location = new System.Drawing.Point(254, 3);
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.Size = new System.Drawing.Size(389, 21);
            this.textBoxMsg.TabIndex = 4;
            // 
            // checkBoxDebug
            // 
            this.checkBoxDebug.AutoSize = true;
            this.checkBoxDebug.ForeColor = System.Drawing.Color.Red;
            this.checkBoxDebug.Location = new System.Drawing.Point(1131, 3);
            this.checkBoxDebug.Name = "checkBoxDebug";
            this.checkBoxDebug.Size = new System.Drawing.Size(54, 16);
            this.checkBoxDebug.TabIndex = 11;
            this.checkBoxDebug.Text = "Debug";
            this.checkBoxDebug.UseVisualStyleBackColor = true;
            this.checkBoxDebug.CheckedChanged += new System.EventHandler(this.checkBoxDebug_CheckedChanged);
            // 
            // buttonOutTextImage
            // 
            this.buttonOutTextImage.Location = new System.Drawing.Point(103, 3);
            this.buttonOutTextImage.Name = "buttonOutTextImage";
            this.buttonOutTextImage.Size = new System.Drawing.Size(43, 22);
            this.buttonOutTextImage.TabIndex = 10;
            this.buttonOutTextImage.Text = "输出IMG";
            this.buttonOutTextImage.UseVisualStyleBackColor = true;
            this.buttonOutTextImage.Click += new System.EventHandler(this.buttonOutTextImage_Click);
            // 
            // buttonPreDealImage
            // 
            this.buttonPreDealImage.Location = new System.Drawing.Point(154, 3);
            this.buttonPreDealImage.Name = "buttonPreDealImage";
            this.buttonPreDealImage.Size = new System.Drawing.Size(70, 22);
            this.buttonPreDealImage.TabIndex = 10;
            this.buttonPreDealImage.Text = "预处理IMG";
            this.buttonPreDealImage.UseVisualStyleBackColor = true;
            this.buttonPreDealImage.Click += new System.EventHandler(this.buttonPreDealImage_Click);
            // 
            // checkBoxShowUnScanNamelist
            // 
            this.checkBoxShowUnScanNamelist.AutoSize = true;
            this.checkBoxShowUnScanNamelist.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxShowUnScanNamelist.ForeColor = System.Drawing.Color.Red;
            this.checkBoxShowUnScanNamelist.Location = new System.Drawing.Point(1017, 3);
            this.checkBoxShowUnScanNamelist.Name = "checkBoxShowUnScanNamelist";
            this.checkBoxShowUnScanNamelist.Size = new System.Drawing.Size(108, 22);
            this.checkBoxShowUnScanNamelist.TabIndex = 11;
            this.checkBoxShowUnScanNamelist.Text = "显示文件名列表";
            this.checkBoxShowUnScanNamelist.UseVisualStyleBackColor = true;
            this.checkBoxShowUnScanNamelist.CheckedChanged += new System.EventHandler(this.checkBoxDebug_CheckedChanged);
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
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.Button buttonMatchTemplate;

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
        private System.Windows.Forms.ListBox listBoxUnScanDir;
        private System.Windows.Forms.Button buttonCreateTemplate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListBox listBoxScantData;
        private System.Windows.Forms.Button buttonVerify;
        private System.Windows.Forms.Button buttonCreateYJData;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ComboBox comboBoxTemplate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonVerifyname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonReScan;
        private System.Windows.Forms.TextBox textBoxMsg;
        private System.Windows.Forms.Button buttonOpenTemplate;
        private System.Windows.Forms.CheckBox checkBoxDebug;
        private System.Windows.Forms.Button buttonUnScanNameList;
        private System.Windows.Forms.Button buttonOutTextImage;
        private System.Windows.Forms.Button buttonPreDealImage;
        private System.Windows.Forms.CheckBox checkBoxShowUnScanNamelist;        
    }
}