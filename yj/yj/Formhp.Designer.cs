namespace yj
{
    partial class Formhp
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonback = new System.Windows.Forms.Button();
            this.checkBoxautoLoadNext = new System.Windows.Forms.CheckBox();
            this.buttonok = new System.Windows.Forms.Button();
            this.textBoxFenshu = new System.Windows.Forms.TextBox();
            this.textBoxShow = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxShow, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(725, 544);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(31, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(661, 460);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonback);
            this.panel1.Controls.Add(this.checkBoxautoLoadNext);
            this.panel1.Controls.Add(this.buttonok);
            this.panel1.Controls.Add(this.textBoxFenshu);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(31, 498);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 43);
            this.panel1.TabIndex = 1;
            // 
            // buttonback
            // 
            this.buttonback.Location = new System.Drawing.Point(3, 10);
            this.buttonback.Name = "buttonback";
            this.buttonback.Size = new System.Drawing.Size(82, 30);
            this.buttonback.TabIndex = 3;
            this.buttonback.Text = "结束回评";
            this.buttonback.UseVisualStyleBackColor = true;
            // 
            // checkBoxautoLoadNext
            // 
            this.checkBoxautoLoadNext.AutoSize = true;
            this.checkBoxautoLoadNext.Location = new System.Drawing.Point(684, 24);
            this.checkBoxautoLoadNext.Name = "checkBoxautoLoadNext";
            this.checkBoxautoLoadNext.Size = new System.Drawing.Size(108, 16);
            this.checkBoxautoLoadNext.TabIndex = 2;
            this.checkBoxautoLoadNext.Text = "自动加载下一份";
            this.checkBoxautoLoadNext.UseVisualStyleBackColor = true;
            this.checkBoxautoLoadNext.Visible = false;
            // 
            // buttonok
            // 
            this.buttonok.Location = new System.Drawing.Point(438, 8);
            this.buttonok.Name = "buttonok";
            this.buttonok.Size = new System.Drawing.Size(82, 30);
            this.buttonok.TabIndex = 1;
            this.buttonok.Text = "确定";
            this.buttonok.UseVisualStyleBackColor = true;
            this.buttonok.Click += new System.EventHandler(this.buttonok_Click);
          
            // 
            // textBoxFenshu
            // 
            this.textBoxFenshu.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxFenshu.Location = new System.Drawing.Point(328, 4);
            this.textBoxFenshu.Name = "textBoxFenshu";
            this.textBoxFenshu.Size = new System.Drawing.Size(95, 35);
            this.textBoxFenshu.TabIndex = 0;
            // 
            // textBoxShow
            // 
            this.textBoxShow.Location = new System.Drawing.Point(31, 3);
            this.textBoxShow.Name = "textBoxShow";
            this.textBoxShow.Size = new System.Drawing.Size(661, 21);
            this.textBoxShow.TabIndex = 9;
            // 
            // Formhp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 544);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Formhp";
            this.Text = "Formhp";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonback;
        private System.Windows.Forms.CheckBox checkBoxautoLoadNext;
        private System.Windows.Forms.Button buttonok;
        private System.Windows.Forms.TextBox textBoxFenshu;
        private System.Windows.Forms.TextBox textBoxShow;
    }
}