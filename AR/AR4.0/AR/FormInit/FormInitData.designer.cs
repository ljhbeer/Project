namespace AR
{
    partial class FormInitData
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
            this.BtnSaveReadData = new System.Windows.Forms.Button();
            this.linkLabel1ChooseTemplate = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel3SetAnswerScore = new System.Windows.Forms.LinkLabel();
            this.linkLabel2ChoosePicturePath = new System.Windows.Forms.LinkLabel();
            this.linkLabel4LoadNameList = new System.Windows.Forms.LinkLabel();
            this.checkBoxCreateStaticImage = new System.Windows.Forms.CheckBox();
            this.textBoxTemplateName = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonScan = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnSaveReadData
            // 
            this.BtnSaveReadData.Location = new System.Drawing.Point(29, 231);
            this.BtnSaveReadData.Name = "BtnSaveReadData";
            this.BtnSaveReadData.Size = new System.Drawing.Size(118, 29);
            this.BtnSaveReadData.TabIndex = 1;
            this.BtnSaveReadData.Text = "保存项目数据";
            this.BtnSaveReadData.UseVisualStyleBackColor = true;
            this.BtnSaveReadData.Click += new System.EventHandler(this.BtnSaveReadData_Click);
            // 
            // linkLabel1ChooseTemplate
            // 
            this.linkLabel1ChooseTemplate.AutoSize = true;
            this.linkLabel1ChooseTemplate.Location = new System.Drawing.Point(27, 31);
            this.linkLabel1ChooseTemplate.Name = "linkLabel1ChooseTemplate";
            this.linkLabel1ChooseTemplate.Size = new System.Drawing.Size(71, 12);
            this.linkLabel1ChooseTemplate.TabIndex = 2;
            this.linkLabel1ChooseTemplate.TabStop = true;
            this.linkLabel1ChooseTemplate.Text = "1、选择模板";
            this.linkLabel1ChooseTemplate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1ChooseTemplate_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "当前模板为：";
            // 
            // linkLabel3SetAnswerScore
            // 
            this.linkLabel3SetAnswerScore.AutoSize = true;
            this.linkLabel3SetAnswerScore.Location = new System.Drawing.Point(27, 127);
            this.linkLabel3SetAnswerScore.Name = "linkLabel3SetAnswerScore";
            this.linkLabel3SetAnswerScore.Size = new System.Drawing.Size(107, 24);
            this.linkLabel3SetAnswerScore.TabIndex = 4;
            this.linkLabel3SetAnswerScore.TabStop = true;
            this.linkLabel3SetAnswerScore.Text = "3、设置客观题答案\r\n   与分值（可选）";
            this.linkLabel3SetAnswerScore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3SetAnswerScore_LinkClicked);
            // 
            // linkLabel2ChoosePicturePath
            // 
            this.linkLabel2ChoosePicturePath.AutoSize = true;
            this.linkLabel2ChoosePicturePath.Location = new System.Drawing.Point(27, 96);
            this.linkLabel2ChoosePicturePath.Name = "linkLabel2ChoosePicturePath";
            this.linkLabel2ChoosePicturePath.Size = new System.Drawing.Size(95, 12);
            this.linkLabel2ChoosePicturePath.TabIndex = 5;
            this.linkLabel2ChoosePicturePath.TabStop = true;
            this.linkLabel2ChoosePicturePath.Text = "2、选择图片目录";
            this.linkLabel2ChoosePicturePath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2ChoosePicturePath_LinkClicked);
            // 
            // linkLabel4LoadNameList
            // 
            this.linkLabel4LoadNameList.AutoSize = true;
            this.linkLabel4LoadNameList.Location = new System.Drawing.Point(27, 162);
            this.linkLabel4LoadNameList.Name = "linkLabel4LoadNameList";
            this.linkLabel4LoadNameList.Size = new System.Drawing.Size(143, 12);
            this.linkLabel4LoadNameList.TabIndex = 6;
            this.linkLabel4LoadNameList.TabStop = true;
            this.linkLabel4LoadNameList.Text = "4、导入学生名单（可选）";
            this.linkLabel4LoadNameList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4LoadNameList_LinkClicked);
            // 
            // checkBoxCreateStaticImage
            // 
            this.checkBoxCreateStaticImage.AutoSize = true;
            this.checkBoxCreateStaticImage.Location = new System.Drawing.Point(0, 279);
            this.checkBoxCreateStaticImage.Name = "checkBoxCreateStaticImage";
            this.checkBoxCreateStaticImage.Size = new System.Drawing.Size(192, 16);
            this.checkBoxCreateStaticImage.TabIndex = 7;
            this.checkBoxCreateStaticImage.Text = "静态生成阅卷图片（耗时较长）";
            this.checkBoxCreateStaticImage.UseVisualStyleBackColor = true;
            // 
            // textBoxTemplateName
            // 
            this.textBoxTemplateName.Location = new System.Drawing.Point(29, 68);
            this.textBoxTemplateName.Name = "textBoxTemplateName";
            this.textBoxTemplateName.ReadOnly = true;
            this.textBoxTemplateName.Size = new System.Drawing.Size(140, 21);
            this.textBoxTemplateName.TabIndex = 8;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonScan);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxTemplateName);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxCreateStaticImage);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.linkLabel3SetAnswerScore);
            this.splitContainer1.Panel1.Controls.Add(this.linkLabel1ChooseTemplate);
            this.splitContainer1.Panel1.Controls.Add(this.linkLabel2ChoosePicturePath);
            this.splitContainer1.Panel1.Controls.Add(this.BtnSaveReadData);
            this.splitContainer1.Panel1.Controls.Add(this.linkLabel4LoadNameList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(952, 686);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 9;
            // 
            // buttonScan
            // 
            this.buttonScan.Location = new System.Drawing.Point(29, 196);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(118, 29);
            this.buttonScan.TabIndex = 9;
            this.buttonScan.Text = "扫描试卷";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgv1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 686);
            this.panel1.TabIndex = 0;
            // 
            // dgv1
            // 
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv1.Location = new System.Drawing.Point(0, 0);
            this.dgv1.Name = "dgv1";
            this.dgv1.RowTemplate.Height = 23;
            this.dgv1.Size = new System.Drawing.Size(763, 686);
            this.dgv1.TabIndex = 0;
            // 
            // FormInitData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 686);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormInitData";
            this.Text = "初始化阅卷数据";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1ChooseTemplate;
        private System.Windows.Forms.LinkLabel linkLabel3SetAnswerScore;
        private System.Windows.Forms.LinkLabel linkLabel2ChoosePicturePath;
        private System.Windows.Forms.LinkLabel linkLabel4LoadNameList;
        private System.Windows.Forms.CheckBox checkBoxCreateStaticImage;
        private System.Windows.Forms.TextBox textBoxTemplateName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.Button BtnSaveReadData;

    }
}