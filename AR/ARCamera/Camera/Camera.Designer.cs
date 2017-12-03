namespace Camera
{
    partial class Camera
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPX = new System.Windows.Forms.TextBox();
            this.textBoxPY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxPWidth = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPHeight = new System.Windows.Forms.TextBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.textBoxState = new System.Windows.Forms.TextBox();
            this.textBoxR = new System.Windows.Forms.TextBox();
            this.openimg = new System.Windows.Forms.Button();
            this.checkBoxImgRotate = new System.Windows.Forms.CheckBox();
            this.btncfg = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxdebug = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Location = new System.Drawing.Point(3, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 240);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(397, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(147, 20);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(341, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "摄像头：";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(397, 36);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(147, 20);
            this.comboBox2.TabIndex = 3;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(341, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "性能：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(348, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 25);
            this.button1.TabIndex = 4;
            this.button1.Text = "连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(397, 59);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(43, 25);
            this.button2.TabIndex = 5;
            this.button2.Text = "截图";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(446, 59);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(43, 25);
            this.button3.TabIndex = 6;
            this.button3.Text = "断开";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox2.Location = new System.Drawing.Point(3, 243);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(640, 480);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(338, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "X";
            // 
            // textBoxPX
            // 
            this.textBoxPX.Location = new System.Drawing.Point(350, 95);
            this.textBoxPX.Name = "textBoxPX";
            this.textBoxPX.Size = new System.Drawing.Size(49, 21);
            this.textBoxPX.TabIndex = 8;
            this.textBoxPX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // textBoxPY
            // 
            this.textBoxPY.Location = new System.Drawing.Point(350, 119);
            this.textBoxPY.Name = "textBoxPY";
            this.textBoxPY.Size = new System.Drawing.Size(49, 21);
            this.textBoxPY.TabIndex = 10;
            this.textBoxPY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(338, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(401, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "Width";
            // 
            // textBoxPWidth
            // 
            this.textBoxPWidth.Location = new System.Drawing.Point(438, 95);
            this.textBoxPWidth.Name = "textBoxPWidth";
            this.textBoxPWidth.Size = new System.Drawing.Size(51, 21);
            this.textBoxPWidth.TabIndex = 12;
            this.textBoxPWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(400, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 13;
            this.label6.Tag = "";
            this.label6.Text = "Height";
            // 
            // textBoxPHeight
            // 
            this.textBoxPHeight.Location = new System.Drawing.Point(438, 118);
            this.textBoxPHeight.Name = "textBoxPHeight";
            this.textBoxPHeight.Size = new System.Drawing.Size(51, 21);
            this.textBoxPHeight.TabIndex = 14;
            this.textBoxPHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(495, 102);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(49, 27);
            this.buttonApply.TabIndex = 15;
            this.buttonApply.Text = "应用";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // textBoxState
            // 
            this.textBoxState.Location = new System.Drawing.Point(342, 216);
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.ReadOnly = true;
            this.textBoxState.Size = new System.Drawing.Size(203, 21);
            this.textBoxState.TabIndex = 17;
            // 
            // textBoxR
            // 
            this.textBoxR.Location = new System.Drawing.Point(343, 193);
            this.textBoxR.Name = "textBoxR";
            this.textBoxR.ReadOnly = true;
            this.textBoxR.Size = new System.Drawing.Size(201, 21);
            this.textBoxR.TabIndex = 16;
            // 
            // openimg
            // 
            this.openimg.Location = new System.Drawing.Point(495, 160);
            this.openimg.Name = "openimg";
            this.openimg.Size = new System.Drawing.Size(49, 25);
            this.openimg.TabIndex = 18;
            this.openimg.Text = "打开";
            this.openimg.UseVisualStyleBackColor = true;
            this.openimg.Click += new System.EventHandler(this.openimg_Click);
            // 
            // checkBoxImgRotate
            // 
            this.checkBoxImgRotate.AutoSize = true;
            this.checkBoxImgRotate.Location = new System.Drawing.Point(417, 168);
            this.checkBoxImgRotate.Name = "checkBoxImgRotate";
            this.checkBoxImgRotate.Size = new System.Drawing.Size(72, 16);
            this.checkBoxImgRotate.TabIndex = 19;
            this.checkBoxImgRotate.Text = "图像旋转";
            this.checkBoxImgRotate.UseVisualStyleBackColor = true;
            // 
            // btncfg
            // 
            this.btncfg.Location = new System.Drawing.Point(495, 59);
            this.btncfg.Name = "btncfg";
            this.btncfg.Size = new System.Drawing.Size(50, 25);
            this.btncfg.TabIndex = 20;
            this.btncfg.Text = "配置";
            this.btncfg.UseVisualStyleBackColor = true;
            this.btncfg.Click += new System.EventHandler(this.btncfg_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(348, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 13;
            this.label7.Tag = "";
            this.label7.Text = "debug";
            // 
            // textBoxdebug
            // 
            this.textBoxdebug.Location = new System.Drawing.Point(348, 166);
            this.textBoxdebug.Name = "textBoxdebug";
            this.textBoxdebug.Size = new System.Drawing.Size(51, 21);
            this.textBoxdebug.TabIndex = 14;
            this.textBoxdebug.TextChanged += new System.EventHandler(this.textBoxdebug_TextChanged);
            this.textBoxdebug.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // Camera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 726);
            this.Controls.Add(this.btncfg);
            this.Controls.Add(this.checkBoxImgRotate);
            this.Controls.Add(this.openimg);
            this.Controls.Add(this.textBoxR);
            this.Controls.Add(this.textBoxState);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.textBoxdebug);
            this.Controls.Add(this.textBoxPHeight);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxPY);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxPWidth);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxPX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Camera";
            this.Text = "WebCamera";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Camera_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private long framecnt;
        private bool bcapture;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPX;
        private System.Windows.Forms.TextBox textBoxPY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPHeight;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.TextBox textBoxState;
        private System.Windows.Forms.TextBox textBoxR;
        private System.Windows.Forms.Button openimg;
        private System.Windows.Forms.CheckBox checkBoxImgRotate;
        private System.Windows.Forms.Button btncfg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxdebug;
    }
}

