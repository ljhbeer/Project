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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSaveTemplate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImportTemplate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDP = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDId = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonName = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDX = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDF = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomout = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomin = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomMouse = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWhite = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonToDark = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSetGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCustomDefine = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonCloseAndOutImages = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomNone = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBoxKHFormat = new System.Windows.Forms.ToolStripComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip2.SuspendLayout();
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
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSaveTemplate,
            this.toolStripButtonImportTemplate,
            this.toolStripButtonDP,
            this.toolStripButtonDId,
            this.toolStripButtonName,
            this.toolStripButtonDX,
            this.toolStripButtonDF,
            this.toolStripButtonZoomout,
            this.toolStripButtonZoomin,
            this.toolStripButtonZoomMouse,
            this.toolStripButtonWhite,
            this.toolStripButtonToDark,
            this.toolStripButtonSetGroup,
            this.toolStripButtonCustomDefine});
            this.toolStrip1.Location = new System.Drawing.Point(3, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(697, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // toolStripButtonSaveTemplate
            // 
            this.toolStripButtonSaveTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSaveTemplate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveTemplate.Image")));
            this.toolStripButtonSaveTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveTemplate.Name = "toolStripButtonSaveTemplate";
            this.toolStripButtonSaveTemplate.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonSaveTemplate.Text = "保存模板";
            this.toolStripButtonSaveTemplate.Click += new System.EventHandler(this.toolStripButtonSaveTemplate_Click);
            // 
            // toolStripButtonImportTemplate
            // 
            this.toolStripButtonImportTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonImportTemplate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImportTemplate.Image")));
            this.toolStripButtonImportTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImportTemplate.Name = "toolStripButtonImportTemplate";
            this.toolStripButtonImportTemplate.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonImportTemplate.Text = "导入模板";
            this.toolStripButtonImportTemplate.Click += new System.EventHandler(this.toolStripButtonImportTemplate_Click);
            // 
            // toolStripButtonDP
            // 
            this.toolStripButtonDP.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDP.Name = "toolStripButtonDP";
            this.toolStripButtonDP.Size = new System.Drawing.Size(63, 22);
            this.toolStripButtonDP.Text = "DF特征点";
            this.toolStripButtonDP.Click += new System.EventHandler(this.toolStripButtonDP_Click);
            // 
            // toolStripButtonDId
            // 
            this.toolStripButtonDId.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDId.Name = "toolStripButtonDId";
            this.toolStripButtonDId.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonDId.Text = "考号";
            this.toolStripButtonDId.Click += new System.EventHandler(this.toolStripButtonDId_Click);
            // 
            // toolStripButtonName
            // 
            this.toolStripButtonName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonName.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonName.Image")));
            this.toolStripButtonName.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonName.Name = "toolStripButtonName";
            this.toolStripButtonName.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonName.Text = "校对";
            this.toolStripButtonName.Click += new System.EventHandler(this.toolStripButtonName_Click);
            // 
            // toolStripButtonDX
            // 
            this.toolStripButtonDX.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDX.Name = "toolStripButtonDX";
            this.toolStripButtonDX.Size = new System.Drawing.Size(48, 22);
            this.toolStripButtonDX.Text = "选择题";
            this.toolStripButtonDX.Click += new System.EventHandler(this.toolStripButtonDX_Click);
            // 
            // toolStripButtonDF
            // 
            this.toolStripButtonDF.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDF.Name = "toolStripButtonDF";
            this.toolStripButtonDF.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonDF.Text = "非选择题";
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
            // toolStripButtonSetGroup
            // 
            this.toolStripButtonSetGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSetGroup.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSetGroup.Image")));
            this.toolStripButtonSetGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSetGroup.Name = "toolStripButtonSetGroup";
            this.toolStripButtonSetGroup.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonSetGroup.Text = "设置题组";
            this.toolStripButtonSetGroup.Click += new System.EventHandler(this.toolStripButtonSetGroup_Click);
            // 
            // toolStripButtonCustomDefine
            // 
            this.toolStripButtonCustomDefine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCustomDefine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCustomDefine.Image")));
            this.toolStripButtonCustomDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCustomDefine.Name = "toolStripButtonCustomDefine";
            this.toolStripButtonCustomDefine.Size = new System.Drawing.Size(48, 22);
            this.toolStripButtonCustomDefine.Text = "自定义";
            this.toolStripButtonCustomDefine.Click += new System.EventHandler(this.toolStripButtonCustomDefine_Click);
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
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
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
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(786, 619);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
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
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonCloseAndOutImages,
            this.toolStripButtonZoomNone,
            this.toolStripComboBoxKHFormat});
            this.toolStrip2.Location = new System.Drawing.Point(3, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(231, 25);
            this.toolStrip2.TabIndex = 2;
            // 
            // toolStripButtonCloseAndOutImages
            // 
            this.toolStripButtonCloseAndOutImages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCloseAndOutImages.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCloseAndOutImages.Image")));
            this.toolStripButtonCloseAndOutImages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCloseAndOutImages.Name = "toolStripButtonCloseAndOutImages";
            this.toolStripButtonCloseAndOutImages.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonCloseAndOutImages.Text = "保存退出";
            this.toolStripButtonCloseAndOutImages.Click += new System.EventHandler(this.toolStripButtonCloseAndOutImages_Click);
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
            // toolStripComboBoxKHFormat
            // 
            this.toolStripComboBoxKHFormat.Items.AddRange(new object[] {
            "考号-条形码",
            "考号-涂卡3位",
            "考号-涂卡4位",
            "考号-涂卡5位",
            "考号-涂卡6位",
            "考号-涂卡7位",
            "考号-涂卡8位",
            "考号-涂卡9位",
            "考号-涂卡10位"});
            this.toolStripComboBoxKHFormat.Name = "toolStripComboBoxKHFormat";
            this.toolStripComboBoxKHFormat.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxKHFormat.Text = "考号格式";
            // 
            // FormTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 686);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "FormTemplate";
            this.Text = "阅卷";
            this.Load += new System.EventHandler(this.FormTemplate_Load);
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
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
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
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonWhite;
        private System.Windows.Forms.ToolStripButton toolStripButtonToDark;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomMouse;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveTemplate;
        private System.Windows.Forms.ToolStripButton toolStripButtonImportTemplate;
        private System.Windows.Forms.ToolStripButton toolStripButtonName;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetGroup;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButtonCloseAndOutImages;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomNone;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxKHFormat;
        private System.Windows.Forms.ToolStripButton toolStripButtonCustomDefine;
    }
}

