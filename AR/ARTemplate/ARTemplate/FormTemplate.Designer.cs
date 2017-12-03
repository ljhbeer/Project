namespace ARTemplate
{
    partial class FormTemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTemplate));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入模板IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出模板OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.模板设计MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入图片IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExportImg = new System.Windows.Forms.ToolStripMenuItem();
            this.定义考号KToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.定义特征点TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.定义选择题XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.定义非选择题FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.联系我们CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.注册RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDP = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDId = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDX = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDF = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomout = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomin = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomMouse = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomNone = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWhite = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonToDark = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.文件FToolStripMenuItem,
            this.模板设计MToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(952, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入模板IToolStripMenuItem,
            this.导出模板OToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 导入模板IToolStripMenuItem
            // 
            this.导入模板IToolStripMenuItem.Name = "导入模板IToolStripMenuItem";
            this.导入模板IToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.导入模板IToolStripMenuItem.Text = "导入模板(&I)";
            this.导入模板IToolStripMenuItem.Click += new System.EventHandler(this.导入模板IToolStripMenuItem_Click);
            // 
            // 导出模板OToolStripMenuItem
            // 
            this.导出模板OToolStripMenuItem.Name = "导出模板OToolStripMenuItem";
            this.导出模板OToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.导出模板OToolStripMenuItem.Text = "导出模板(&O)";
            this.导出模板OToolStripMenuItem.Click += new System.EventHandler(this.导出模板OToolStripMenuItem_Click);
            // 
            // 模板设计MToolStripMenuItem
            // 
            this.模板设计MToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入图片IToolStripMenuItem,
            this.toolStripMenuItemExportImg,
            this.定义考号KToolStripMenuItem,
            this.定义特征点TToolStripMenuItem,
            this.定义选择题XToolStripMenuItem,
            this.定义非选择题FToolStripMenuItem});
            this.模板设计MToolStripMenuItem.Name = "模板设计MToolStripMenuItem";
            this.模板设计MToolStripMenuItem.Size = new System.Drawing.Size(88, 21);
            this.模板设计MToolStripMenuItem.Text = "模板设计(&M)";
            // 
            // 导入图片IToolStripMenuItem
            // 
            this.导入图片IToolStripMenuItem.Name = "导入图片IToolStripMenuItem";
            this.导入图片IToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.导入图片IToolStripMenuItem.Text = "导入图片(&P)";
            this.导入图片IToolStripMenuItem.Click += new System.EventHandler(this.导入图片IToolStripMenuItem_Click);
            // 
            // toolStripMenuItemExportImg
            // 
            this.toolStripMenuItemExportImg.Name = "toolStripMenuItemExportImg";
            this.toolStripMenuItemExportImg.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemExportImg.Text = "导出修改好的图片(&E)";
            this.toolStripMenuItemExportImg.Click += new System.EventHandler(this.toolStripMenuItemExportImg_Click);
            // 
            // 定义考号KToolStripMenuItem
            // 
            this.定义考号KToolStripMenuItem.Name = "定义考号KToolStripMenuItem";
            this.定义考号KToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.定义考号KToolStripMenuItem.Text = "定义考号(&K)";
            this.定义考号KToolStripMenuItem.Click += new System.EventHandler(this.定义考号KToolStripMenuItem_Click);
            // 
            // 定义特征点TToolStripMenuItem
            // 
            this.定义特征点TToolStripMenuItem.Name = "定义特征点TToolStripMenuItem";
            this.定义特征点TToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.定义特征点TToolStripMenuItem.Text = "定义特征点(&T)";
            this.定义特征点TToolStripMenuItem.Click += new System.EventHandler(this.定义特征点TToolStripMenuItem_Click);
            // 
            // 定义选择题XToolStripMenuItem
            // 
            this.定义选择题XToolStripMenuItem.Name = "定义选择题XToolStripMenuItem";
            this.定义选择题XToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.定义选择题XToolStripMenuItem.Text = "定义选择题(&X)";
            this.定义选择题XToolStripMenuItem.Click += new System.EventHandler(this.定义选择题XToolStripMenuItem_Click);
            // 
            // 定义非选择题FToolStripMenuItem
            // 
            this.定义非选择题FToolStripMenuItem.Name = "定义非选择题FToolStripMenuItem";
            this.定义非选择题FToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.定义非选择题FToolStripMenuItem.Text = "定义非选择题(&F)";
            this.定义非选择题FToolStripMenuItem.Click += new System.EventHandler(this.定义非选择题FToolStripMenuItem_Click);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.联系我们CToolStripMenuItem,
            this.注册RToolStripMenuItem,
            this.关于AToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 联系我们CToolStripMenuItem
            // 
            this.联系我们CToolStripMenuItem.Name = "联系我们CToolStripMenuItem";
            this.联系我们CToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.联系我们CToolStripMenuItem.Text = "联系我们(&C)";
            // 
            // 注册RToolStripMenuItem
            // 
            this.注册RToolStripMenuItem.Name = "注册RToolStripMenuItem";
            this.注册RToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.注册RToolStripMenuItem.Text = "注册(&R)";
            // 
            // 关于AToolStripMenuItem
            // 
            this.关于AToolStripMenuItem.Name = "关于AToolStripMenuItem";
            this.关于AToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.关于AToolStripMenuItem.Text = "关于(&A)";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDP,
            this.toolStripButtonDId,
            this.toolStripButtonDX,
            this.toolStripButtonDF,
            this.toolStripButtonZoomout,
            this.toolStripButtonZoomin,
            this.toolStripButtonZoomMouse,
            this.toolStripButtonZoomNone,
            this.toolStripButtonWhite,
            this.toolStripButtonToDark});
            this.toolStrip1.Location = new System.Drawing.Point(290, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(569, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // toolStripButtonDP
            // 
            this.toolStripButtonDP.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDP.Name = "toolStripButtonDP";
            this.toolStripButtonDP.Size = new System.Drawing.Size(72, 22);
            this.toolStripButtonDP.Text = "定义特征点";
            this.toolStripButtonDP.Click += new System.EventHandler(this.toolStripButtonDP_Click);
            // 
            // toolStripButtonDId
            // 
            this.toolStripButtonDId.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDId.Name = "toolStripButtonDId";
            this.toolStripButtonDId.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonDId.Text = "定义考号";
            this.toolStripButtonDId.Click += new System.EventHandler(this.toolStripButtonDId_Click);
            // 
            // toolStripButtonDX
            // 
            this.toolStripButtonDX.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDX.Name = "toolStripButtonDX";
            this.toolStripButtonDX.Size = new System.Drawing.Size(72, 22);
            this.toolStripButtonDX.Text = "定义选择题";
            this.toolStripButtonDX.Click += new System.EventHandler(this.toolStripButtonDX_Click);
            // 
            // toolStripButtonDF
            // 
            this.toolStripButtonDF.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDF.Name = "toolStripButtonDF";
            this.toolStripButtonDF.Size = new System.Drawing.Size(72, 22);
            this.toolStripButtonDF.Text = "定义非选择";
            this.toolStripButtonDF.Click += new System.EventHandler(this.toolStripButtonDF_Click);
            // 
            // toolStripButtonZoomout
            // 
            this.toolStripButtonZoomout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZoomout.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomout.Image")));
            this.toolStripButtonZoomout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomout.Name = "toolStripButtonZoomout";
            this.toolStripButtonZoomout.Size = new System.Drawing.Size(25, 22);
            this.toolStripButtonZoomout.Text = " - ";
            this.toolStripButtonZoomout.Click += new System.EventHandler(this.toolStripButtonZoomout_Click);
            // 
            // toolStripButtonZoomin
            // 
            this.toolStripButtonZoomin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZoomin.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomin.Image")));
            this.toolStripButtonZoomin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomin.Name = "toolStripButtonZoomin";
            this.toolStripButtonZoomin.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomin.Tag = "zoomin";
            this.toolStripButtonZoomin.Text = "+";
            this.toolStripButtonZoomin.Click += new System.EventHandler(this.toolStripButtonZoomin_Click);
            // 
            // toolStripButtonZoomMouse
            // 
            this.toolStripButtonZoomMouse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZoomMouse.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomMouse.Image")));
            this.toolStripButtonZoomMouse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomMouse.Name = "toolStripButtonZoomMouse";
            this.toolStripButtonZoomMouse.Size = new System.Drawing.Size(46, 22);
            this.toolStripButtonZoomMouse.Tag = "zoommouse";
            this.toolStripButtonZoomMouse.Text = "Zoom";
            this.toolStripButtonZoomMouse.Click += new System.EventHandler(this.toolStripButtonZoomMouse_Click);
            // 
            // toolStripButtonZoomNone
            // 
            this.toolStripButtonZoomNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZoomNone.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomNone.Image")));
            this.toolStripButtonZoomNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomNone.Name = "toolStripButtonZoomNone";
            this.toolStripButtonZoomNone.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonZoomNone.Text = "还原";
            this.toolStripButtonZoomNone.Click += new System.EventHandler(this.toolStripButtonZoomNone_Click);
            // 
            // toolStripButtonWhite
            // 
            this.toolStripButtonWhite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonWhite.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonWhite.Image")));
            this.toolStripButtonWhite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWhite.Name = "toolStripButtonWhite";
            this.toolStripButtonWhite.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonWhite.Text = "选区变白";
            this.toolStripButtonWhite.Click += new System.EventHandler(this.toolStripButtonWhite_Click);
            // 
            // toolStripButtonToDark
            // 
            this.toolStripButtonToDark.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonToDark.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonToDark.Image")));
            this.toolStripButtonToDark.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToDark.Name = "toolStripButtonToDark";
            this.toolStripButtonToDark.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonToDark.Text = "选区变黑";
            this.toolStripButtonToDark.Click += new System.EventHandler(this.toolStripButtonToDark_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tableLayoutPanel1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(952, 636);
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
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.92308F));
            this.tableLayoutPanel1.Controls.Add(this.treeView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxMessage, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.46468F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(952, 636);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 30);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(151, 603);
            this.treeView1.TabIndex = 1;
            this.treeView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyUp);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(160, 3);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(789, 630);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.pictureBox1.Location = new System.Drawing.Point(3, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(718, 622);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessage.Location = new System.Drawing.Point(3, 3);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(151, 21);
            this.textBoxMessage.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 686);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "阅卷";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入模板IToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出模板OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 模板设计MToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入图片IToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 定义考号KToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 定义特征点TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 定义选择题XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 定义非选择题FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 联系我们CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 注册RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于AToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDP;
        private System.Windows.Forms.ToolStripButton toolStripButtonDId;
        private System.Windows.Forms.ToolStripButton toolStripButtonDX;
        private System.Windows.Forms.ToolStripButton toolStripButtonDF;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomin;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomNone;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonWhite;
        private System.Windows.Forms.ToolStripButton toolStripButtonToDark;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomMouse;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExportImg;
    }
}

