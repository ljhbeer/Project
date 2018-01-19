namespace ScanTemplate
{
    partial class FormPreDealImage
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
            this.listBoxfilename = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxOut = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonSetSeletion = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonAutoRorate = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonApplyCutToAllImage = new System.Windows.Forms.Button();
            this.buttonApplyAll = new System.Windows.Forms.Button();
            this.buttonOutRectWhite = new System.Windows.Forms.Button();
            this.buttonCutbySelection = new System.Windows.Forms.Button();
            this.buttonSelectionBlack = new System.Windows.Forms.Button();
            this.textBoxAngle = new System.Windows.Forms.TextBox();
            this.buttonAngle = new System.Windows.Forms.Button();
            this.buttonTo2bpp = new System.Windows.Forms.Button();
            this.buttonSaveActiveImage = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel1.Controls.Add(this.listBoxfilename, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxOut, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(755, 446);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // listBoxfilename
            // 
            this.listBoxfilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxfilename.FormattingEnabled = true;
            this.listBoxfilename.ItemHeight = 12;
            this.listBoxfilename.Location = new System.Drawing.Point(3, 46);
            this.listBoxfilename.Name = "listBoxfilename";
            this.listBoxfilename.Size = new System.Drawing.Size(140, 397);
            this.listBoxfilename.TabIndex = 6;
            this.listBoxfilename.SelectedIndexChanged += new System.EventHandler(this.listBoxfilename_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(149, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 397);
            this.panel1.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.pictureBox1.Location = new System.Drawing.Point(0, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(484, 381);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // textBoxOut
            // 
            this.textBoxOut.Location = new System.Drawing.Point(3, 3);
            this.textBoxOut.Multiline = true;
            this.textBoxOut.Name = "textBoxOut";
            this.textBoxOut.Size = new System.Drawing.Size(107, 37);
            this.textBoxOut.TabIndex = 13;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.buttonSetSeletion);
            this.flowLayoutPanel3.Controls.Add(this.button1);
            this.flowLayoutPanel3.Controls.Add(this.buttonAutoRorate);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(149, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(497, 37);
            this.flowLayoutPanel3.TabIndex = 14;
            // 
            // buttonSetSeletion
            // 
            this.buttonSetSeletion.Location = new System.Drawing.Point(3, 3);
            this.buttonSetSeletion.Name = "buttonSetSeletion";
            this.buttonSetSeletion.Size = new System.Drawing.Size(43, 34);
            this.buttonSetSeletion.TabIndex = 12;
            this.buttonSetSeletion.Text = "设定选区";
            this.buttonSetSeletion.UseVisualStyleBackColor = true;
            this.buttonSetSeletion.Click += new System.EventHandler(this.buttonSetSeletion_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(52, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 34);
            this.button1.TabIndex = 13;
            this.button1.Text = "自动检测";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonAutoRorate
            // 
            this.buttonAutoRorate.Location = new System.Drawing.Point(101, 3);
            this.buttonAutoRorate.Name = "buttonAutoRorate";
            this.buttonAutoRorate.Size = new System.Drawing.Size(73, 34);
            this.buttonAutoRorate.TabIndex = 14;
            this.buttonAutoRorate.Text = "自动检测并修正";
            this.buttonAutoRorate.UseVisualStyleBackColor = true;
            this.buttonAutoRorate.Click += new System.EventHandler(this.buttonAutoRorate_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonApplyCutToAllImage);
            this.flowLayoutPanel1.Controls.Add(this.buttonApplyAll);
            this.flowLayoutPanel1.Controls.Add(this.buttonOutRectWhite);
            this.flowLayoutPanel1.Controls.Add(this.buttonCutbySelection);
            this.flowLayoutPanel1.Controls.Add(this.buttonSelectionBlack);
            this.flowLayoutPanel1.Controls.Add(this.textBoxAngle);
            this.flowLayoutPanel1.Controls.Add(this.buttonAngle);
            this.flowLayoutPanel1.Controls.Add(this.buttonTo2bpp);
            this.flowLayoutPanel1.Controls.Add(this.buttonSaveActiveImage);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(652, 46);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(100, 397);
            this.flowLayoutPanel1.TabIndex = 10;
            this.flowLayoutPanel1.Visible = false;
            // 
            // buttonApplyCutToAllImage
            // 
            this.buttonApplyCutToAllImage.Location = new System.Drawing.Point(3, 3);
            this.buttonApplyCutToAllImage.Name = "buttonApplyCutToAllImage";
            this.buttonApplyCutToAllImage.Size = new System.Drawing.Size(89, 33);
            this.buttonApplyCutToAllImage.TabIndex = 4;
            this.buttonApplyCutToAllImage.Text = "应用剪切到所有图片";
            this.buttonApplyCutToAllImage.UseVisualStyleBackColor = true;
            this.buttonApplyCutToAllImage.Click += new System.EventHandler(this.buttonApplyCutToAllImage_Click);
            // 
            // buttonApplyAll
            // 
            this.buttonApplyAll.Location = new System.Drawing.Point(3, 42);
            this.buttonApplyAll.Name = "buttonApplyAll";
            this.buttonApplyAll.Size = new System.Drawing.Size(89, 34);
            this.buttonApplyAll.TabIndex = 2;
            this.buttonApplyAll.Text = "应用白色到所有图片";
            this.buttonApplyAll.UseVisualStyleBackColor = true;
            this.buttonApplyAll.Click += new System.EventHandler(this.buttonApplyAll_Click);
            // 
            // buttonOutRectWhite
            // 
            this.buttonOutRectWhite.Location = new System.Drawing.Point(3, 82);
            this.buttonOutRectWhite.Name = "buttonOutRectWhite";
            this.buttonOutRectWhite.Size = new System.Drawing.Size(43, 36);
            this.buttonOutRectWhite.TabIndex = 1;
            this.buttonOutRectWhite.Text = "选区外侧白色";
            this.buttonOutRectWhite.UseVisualStyleBackColor = true;
            this.buttonOutRectWhite.Click += new System.EventHandler(this.buttonOutRectWhite_Click);
            // 
            // buttonCutbySelection
            // 
            this.buttonCutbySelection.Location = new System.Drawing.Point(52, 82);
            this.buttonCutbySelection.Name = "buttonCutbySelection";
            this.buttonCutbySelection.Size = new System.Drawing.Size(43, 35);
            this.buttonCutbySelection.TabIndex = 3;
            this.buttonCutbySelection.Text = "剪切选区";
            this.buttonCutbySelection.UseVisualStyleBackColor = true;
            this.buttonCutbySelection.Click += new System.EventHandler(this.buttonCutbySelection_Click);
            // 
            // buttonSelectionBlack
            // 
            this.buttonSelectionBlack.Location = new System.Drawing.Point(3, 124);
            this.buttonSelectionBlack.Name = "buttonSelectionBlack";
            this.buttonSelectionBlack.Size = new System.Drawing.Size(89, 21);
            this.buttonSelectionBlack.TabIndex = 1;
            this.buttonSelectionBlack.Text = "选区黑色";
            this.buttonSelectionBlack.UseVisualStyleBackColor = true;
            this.buttonSelectionBlack.Click += new System.EventHandler(this.buttonSelectionBlack_Click);
            // 
            // textBoxAngle
            // 
            this.textBoxAngle.Location = new System.Drawing.Point(3, 151);
            this.textBoxAngle.Name = "textBoxAngle";
            this.textBoxAngle.Size = new System.Drawing.Size(86, 21);
            this.textBoxAngle.TabIndex = 6;
            this.textBoxAngle.Text = "0";
            // 
            // buttonAngle
            // 
            this.buttonAngle.Location = new System.Drawing.Point(3, 178);
            this.buttonAngle.Name = "buttonAngle";
            this.buttonAngle.Size = new System.Drawing.Size(89, 21);
            this.buttonAngle.TabIndex = 5;
            this.buttonAngle.Text = "旋转";
            this.buttonAngle.UseVisualStyleBackColor = true;
            this.buttonAngle.Click += new System.EventHandler(this.buttonAngle_Click);
            // 
            // buttonTo2bpp
            // 
            this.buttonTo2bpp.Location = new System.Drawing.Point(3, 205);
            this.buttonTo2bpp.Name = "buttonTo2bpp";
            this.buttonTo2bpp.Size = new System.Drawing.Size(89, 21);
            this.buttonTo2bpp.TabIndex = 5;
            this.buttonTo2bpp.Text = "二值化";
            this.buttonTo2bpp.UseVisualStyleBackColor = true;
            this.buttonTo2bpp.Click += new System.EventHandler(this.buttonTo2bpp_Click);
            // 
            // buttonSaveActiveImage
            // 
            this.buttonSaveActiveImage.Location = new System.Drawing.Point(3, 232);
            this.buttonSaveActiveImage.Name = "buttonSaveActiveImage";
            this.buttonSaveActiveImage.Size = new System.Drawing.Size(89, 21);
            this.buttonSaveActiveImage.TabIndex = 2;
            this.buttonSaveActiveImage.Text = "保存当前图片";
            this.buttonSaveActiveImage.UseVisualStyleBackColor = true;
            this.buttonSaveActiveImage.Click += new System.EventHandler(this.buttonSaveActiveImage_Click);
            // 
            // FormPreDealImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 446);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormPreDealImage";
            this.Text = "FormPreDealImage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox listBoxfilename;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBoxOut;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button buttonSetSeletion;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonApplyCutToAllImage;
        private System.Windows.Forms.Button buttonApplyAll;
        private System.Windows.Forms.Button buttonOutRectWhite;
        private System.Windows.Forms.Button buttonCutbySelection;
        private System.Windows.Forms.Button buttonSelectionBlack;
        private System.Windows.Forms.TextBox textBoxAngle;
        private System.Windows.Forms.Button buttonAngle;
        private System.Windows.Forms.Button buttonTo2bpp;
        private System.Windows.Forms.Button buttonSaveActiveImage;
        private System.Windows.Forms.Button buttonAutoRorate;
    }
}