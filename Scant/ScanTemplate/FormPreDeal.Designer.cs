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
            this.listBoxfilename = new System.Windows.Forms.ListBox();
            this.listBoxNewfilename = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonPredealImage = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ll = new System.Windows.Forms.Label();
            this.textBoxfilenamereplacedst = new System.Windows.Forms.TextBox();
            this.textBoxfilenamereplacesrc = new System.Windows.Forms.TextBox();
            this.buttonReName = new System.Windows.Forms.Button();
            this.buttonModifyNewFilename = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.listBoxUnScanDir = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxScantData = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBoxfilename, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBoxNewfilename, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(834, 433);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "未扫描数据";
            // 
            // listBoxfilename
            // 
            this.listBoxfilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxfilename.FormattingEnabled = true;
            this.listBoxfilename.ItemHeight = 12;
            this.listBoxfilename.Location = new System.Drawing.Point(103, 19);
            this.listBoxfilename.Name = "listBoxfilename";
            this.listBoxfilename.Size = new System.Drawing.Size(144, 411);
            this.listBoxfilename.TabIndex = 6;
            this.listBoxfilename.SelectedIndexChanged += new System.EventHandler(this.listBoxfilename_SelectedIndexChanged);
            // 
            // listBoxNewfilename
            // 
            this.listBoxNewfilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxNewfilename.FormattingEnabled = true;
            this.listBoxNewfilename.ItemHeight = 12;
            this.listBoxNewfilename.Location = new System.Drawing.Point(253, 19);
            this.listBoxNewfilename.Name = "listBoxNewfilename";
            this.listBoxNewfilename.Size = new System.Drawing.Size(144, 411);
            this.listBoxNewfilename.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(403, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 411);
            this.panel1.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.pictureBox1.Location = new System.Drawing.Point(0, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(310, 395);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonPredealImage);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.ll);
            this.panel2.Controls.Add(this.textBoxfilenamereplacedst);
            this.panel2.Controls.Add(this.textBoxfilenamereplacesrc);
            this.panel2.Controls.Add(this.buttonReName);
            this.panel2.Controls.Add(this.buttonModifyNewFilename);
            this.panel2.Location = new System.Drawing.Point(739, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(92, 411);
            this.panel2.TabIndex = 8;
            // 
            // buttonPredealImage
            // 
            this.buttonPredealImage.Location = new System.Drawing.Point(0, 130);
            this.buttonPredealImage.Name = "buttonPredealImage";
            this.buttonPredealImage.Size = new System.Drawing.Size(85, 26);
            this.buttonPredealImage.TabIndex = 2;
            this.buttonPredealImage.Text = "处理图片";
            this.buttonPredealImage.UseVisualStyleBackColor = true;
            this.buttonPredealImage.Click += new System.EventHandler(this.buttonPredealImage_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 39);
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
            this.textBoxfilenamereplacedst.Location = new System.Drawing.Point(3, 51);
            this.textBoxfilenamereplacedst.Name = "textBoxfilenamereplacedst";
            this.textBoxfilenamereplacedst.Size = new System.Drawing.Size(82, 21);
            this.textBoxfilenamereplacedst.TabIndex = 0;
            // 
            // textBoxfilenamereplacesrc
            // 
            this.textBoxfilenamereplacesrc.Location = new System.Drawing.Point(3, 18);
            this.textBoxfilenamereplacesrc.Name = "textBoxfilenamereplacesrc";
            this.textBoxfilenamereplacesrc.Size = new System.Drawing.Size(82, 21);
            this.textBoxfilenamereplacesrc.TabIndex = 0;
            // 
            // buttonReName
            // 
            this.buttonReName.Location = new System.Drawing.Point(0, 98);
            this.buttonReName.Name = "buttonReName";
            this.buttonReName.Size = new System.Drawing.Size(85, 26);
            this.buttonReName.TabIndex = 0;
            this.buttonReName.Text = "重命名文件名";
            this.buttonReName.UseVisualStyleBackColor = true;
            this.buttonReName.Click += new System.EventHandler(this.buttonReName_Click);
            // 
            // buttonModifyNewFilename
            // 
            this.buttonModifyNewFilename.Location = new System.Drawing.Point(0, 72);
            this.buttonModifyNewFilename.Name = "buttonModifyNewFilename";
            this.buttonModifyNewFilename.Size = new System.Drawing.Size(85, 26);
            this.buttonModifyNewFilename.TabIndex = 0;
            this.buttonModifyNewFilename.Text = "更新文件名";
            this.buttonModifyNewFilename.UseVisualStyleBackColor = true;
            this.buttonModifyNewFilename.Click += new System.EventHandler(this.buttonModifyNewFilename_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.listBoxUnScanDir);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.listBoxScantData);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 19);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(94, 411);
            this.flowLayoutPanel2.TabIndex = 11;
            // 
            // listBoxUnScanDir
            // 
            this.listBoxUnScanDir.FormattingEnabled = true;
            this.listBoxUnScanDir.ItemHeight = 12;
            this.listBoxUnScanDir.Location = new System.Drawing.Point(3, 3);
            this.listBoxUnScanDir.Name = "listBoxUnScanDir";
            this.listBoxUnScanDir.Size = new System.Drawing.Size(91, 160);
            this.listBoxUnScanDir.TabIndex = 1;
            this.listBoxUnScanDir.SelectedIndexChanged += new System.EventHandler(this.listBoxUnScanDir_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "已扫描数据";
            this.label2.Visible = false;
            // 
            // listBoxScantData
            // 
            this.listBoxScantData.FormattingEnabled = true;
            this.listBoxScantData.ItemHeight = 12;
            this.listBoxScantData.Location = new System.Drawing.Point(3, 181);
            this.listBoxScantData.Name = "listBoxScantData";
            this.listBoxScantData.Size = new System.Drawing.Size(91, 220);
            this.listBoxScantData.TabIndex = 2;
            this.listBoxScantData.SelectedIndexChanged += new System.EventHandler(this.listBoxScantData_SelectedIndexChanged);
            // 
            // FormPreDeal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 433);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormPreDeal";
            this.Text = "FormPreDeal";
            this.Load += new System.EventHandler(this.FormPreDeal_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxUnScanDir;
        private System.Windows.Forms.ListBox listBoxScantData;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonModifyNewFilename;
        private System.Windows.Forms.ListBox listBoxfilename;
        private System.Windows.Forms.ListBox listBoxNewfilename;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label ll;
        private System.Windows.Forms.TextBox textBoxfilenamereplacesrc;
        private System.Windows.Forms.Button buttonReName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxfilenamereplacedst;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button buttonPredealImage;
    }
}