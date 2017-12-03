namespace AR
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.模式选择CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入阅卷数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出阅卷数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.试卷初始化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.阅卷设计DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出成绩ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始阅卷ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.停止阅卷ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.联系我们CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.注册RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.panelyuejuan = new System.Windows.Forms.Panel();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonupdate = new System.Windows.Forms.Button();
            this.comboBoxSetScore = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.buttonPageDown = new System.Windows.Forms.Button();
            this.buttonPageUp = new System.Windows.Forms.Button();
            this.comboBoxQustiongroup = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.更改客观题答案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panelyuejuan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Image files (*.jpg,*.png,*.tif,*.bmp,*.gif)|*.jpg;*.png;*.tif;*.bmp;*.gif|JPG fil" +
                "es (*.jpg)|*.jpg|PNG files (*.png)|*.png|TIF files (*.tif)|*.tif|BMP files (*.bm" +
                "p)|*.bmp|GIF files (*.gif)|*.gif";
            this.openFileDialog1.Title = "Open image file";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.模式选择CToolStripMenuItem,
            this.阅卷设计DToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(952, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 模式选择CToolStripMenuItem
            // 
            this.模式选择CToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入阅卷数据ToolStripMenuItem,
            this.导出阅卷数据ToolStripMenuItem,
            this.试卷初始化ToolStripMenuItem});
            this.模式选择CToolStripMenuItem.Name = "模式选择CToolStripMenuItem";
            this.模式选择CToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.模式选择CToolStripMenuItem.Text = "模式选择(&C)";
            // 
            // 导入阅卷数据ToolStripMenuItem
            // 
            this.导入阅卷数据ToolStripMenuItem.Name = "导入阅卷数据ToolStripMenuItem";
            this.导入阅卷数据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.导入阅卷数据ToolStripMenuItem.Text = "导入阅卷数据";
            this.导入阅卷数据ToolStripMenuItem.Click += new System.EventHandler(this.导入阅卷数据ToolStripMenuItem_Click);
            // 
            // 导出阅卷数据ToolStripMenuItem
            // 
            this.导出阅卷数据ToolStripMenuItem.Name = "导出阅卷数据ToolStripMenuItem";
            this.导出阅卷数据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.导出阅卷数据ToolStripMenuItem.Text = "导出阅卷数据";
            this.导出阅卷数据ToolStripMenuItem.Click += new System.EventHandler(this.导出阅卷数据ToolStripMenuItem_Click);
            // 
            // 试卷初始化ToolStripMenuItem
            // 
            this.试卷初始化ToolStripMenuItem.Name = "试卷初始化ToolStripMenuItem";
            this.试卷初始化ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.试卷初始化ToolStripMenuItem.Text = "试卷初始化";
            this.试卷初始化ToolStripMenuItem.Click += new System.EventHandler(this.试卷初始化ToolStripMenuItem_Click);
            // 
            // 阅卷设计DToolStripMenuItem
            // 
            this.阅卷设计DToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始阅卷ToolStripMenuItem1,
            this.停止阅卷ToolStripMenuItem1,
            this.导出成绩ToolStripMenuItem,
            this.更改客观题答案ToolStripMenuItem});
            this.阅卷设计DToolStripMenuItem.Name = "阅卷设计DToolStripMenuItem";
            this.阅卷设计DToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.阅卷设计DToolStripMenuItem.Text = "阅卷设计(&D)";
            // 
            // 导出成绩ToolStripMenuItem
            // 
            this.导出成绩ToolStripMenuItem.Name = "导出成绩ToolStripMenuItem";
            this.导出成绩ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.导出成绩ToolStripMenuItem.Text = "导出成绩";
            this.导出成绩ToolStripMenuItem.Click += new System.EventHandler(this.导出成绩ToolStripMenuItem_Click);
            // 
            // 开始阅卷ToolStripMenuItem1
            // 
            this.开始阅卷ToolStripMenuItem1.Name = "开始阅卷ToolStripMenuItem1";
            this.开始阅卷ToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.开始阅卷ToolStripMenuItem1.Text = "开始阅卷";
            this.开始阅卷ToolStripMenuItem1.Click += new System.EventHandler(this.开始阅卷ToolStripMenuItem1_Click);
            // 
            // 停止阅卷ToolStripMenuItem1
            // 
            this.停止阅卷ToolStripMenuItem1.Name = "停止阅卷ToolStripMenuItem1";
            this.停止阅卷ToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.停止阅卷ToolStripMenuItem1.Text = "停止阅卷";
            this.停止阅卷ToolStripMenuItem1.Click += new System.EventHandler(this.停止阅卷ToolStripMenuItem1_Click);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.联系我们CToolStripMenuItem,
            this.注册RToolStripMenuItem,
            this.关于AToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 联系我们CToolStripMenuItem
            // 
            this.联系我们CToolStripMenuItem.Name = "联系我们CToolStripMenuItem";
            this.联系我们CToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.联系我们CToolStripMenuItem.Text = "联系我们(&C)";
            // 
            // 注册RToolStripMenuItem
            // 
            this.注册RToolStripMenuItem.Name = "注册RToolStripMenuItem";
            this.注册RToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.注册RToolStripMenuItem.Text = "注册(&R)";
            // 
            // 关于AToolStripMenuItem
            // 
            this.关于AToolStripMenuItem.Name = "关于AToolStripMenuItem";
            this.关于AToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.关于AToolStripMenuItem.Text = "关于(&A)";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panelyuejuan);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(952, 662);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(952, 686);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // panelyuejuan
            // 
            this.panelyuejuan.Controls.Add(this.buttonBack);
            this.panelyuejuan.Controls.Add(this.buttonupdate);
            this.panelyuejuan.Controls.Add(this.comboBoxSetScore);
            this.panelyuejuan.Controls.Add(this.label1);
            this.panelyuejuan.Controls.Add(this.pictureBox2);
            this.panelyuejuan.Controls.Add(this.buttonPageDown);
            this.panelyuejuan.Controls.Add(this.buttonPageUp);
            this.panelyuejuan.Controls.Add(this.comboBoxQustiongroup);
            this.panelyuejuan.Location = new System.Drawing.Point(3, 3);
            this.panelyuejuan.Name = "panelyuejuan";
            this.panelyuejuan.Size = new System.Drawing.Size(946, 632);
            this.panelyuejuan.TabIndex = 0;
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(652, 6);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 0;
            this.buttonBack.Text = "回评";
            // 
            // buttonupdate
            // 
            this.buttonupdate.Location = new System.Drawing.Point(557, 5);
            this.buttonupdate.Name = "buttonupdate";
            this.buttonupdate.Size = new System.Drawing.Size(89, 23);
            this.buttonupdate.TabIndex = 9;
            this.buttonupdate.Text = "保存当前成绩";
            this.buttonupdate.UseVisualStyleBackColor = true;
            // 
            // comboBoxSetScore
            // 
            this.comboBoxSetScore.FormattingEnabled = true;
            this.comboBoxSetScore.Location = new System.Drawing.Point(500, 6);
            this.comboBoxSetScore.Name = "comboBoxSetScore";
            this.comboBoxSetScore.Size = new System.Drawing.Size(51, 20);
            this.comboBoxSetScore.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(388, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "本页全部设为（分）";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(0, 30);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(950, 630);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // buttonPageDown
            // 
            this.buttonPageDown.Location = new System.Drawing.Point(267, 5);
            this.buttonPageDown.Name = "buttonPageDown";
            this.buttonPageDown.Size = new System.Drawing.Size(92, 23);
            this.buttonPageDown.TabIndex = 6;
            this.buttonPageDown.Text = "下一页";
            this.buttonPageDown.UseVisualStyleBackColor = true;
            // 
            // buttonPageUp
            // 
            this.buttonPageUp.Location = new System.Drawing.Point(160, 5);
            this.buttonPageUp.Name = "buttonPageUp";
            this.buttonPageUp.Size = new System.Drawing.Size(99, 23);
            this.buttonPageUp.TabIndex = 5;
            this.buttonPageUp.Text = "上一页";
            this.buttonPageUp.UseVisualStyleBackColor = true;
            // 
            // comboBoxQustiongroup
            // 
            this.comboBoxQustiongroup.FormattingEnabled = true;
            this.comboBoxQustiongroup.Location = new System.Drawing.Point(10, 5);
            this.comboBoxQustiongroup.Name = "comboBoxQustiongroup";
            this.comboBoxQustiongroup.Size = new System.Drawing.Size(134, 20);
            this.comboBoxQustiongroup.TabIndex = 4;
            // 
            // 更改客观题答案ToolStripMenuItem
            // 
            this.更改客观题答案ToolStripMenuItem.Name = "更改客观题答案ToolStripMenuItem";
            this.更改客观题答案ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.更改客观题答案ToolStripMenuItem.Text = "更改客观题答案";
            this.更改客观题答案ToolStripMenuItem.Click += new System.EventHandler(this.更改客观题答案ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 686);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "阅卷4.0";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.panelyuejuan.ResumeLayout(false);
            this.panelyuejuan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 阅卷设计DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出成绩ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 联系我们CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 注册RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 模式选择CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始阅卷ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 停止阅卷ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem 导出阅卷数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入阅卷数据ToolStripMenuItem;
        private System.Windows.Forms.Panel panelyuejuan;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonupdate;
        private System.Windows.Forms.ComboBox comboBoxSetScore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button buttonPageDown;
        private System.Windows.Forms.Button buttonPageUp;
        private System.Windows.Forms.ComboBox comboBoxQustiongroup;
        private System.Windows.Forms.ToolStripMenuItem 试卷初始化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更改客观题答案ToolStripMenuItem;
    }
}

