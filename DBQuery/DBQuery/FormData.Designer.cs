namespace DBQuery
{
    partial class FormData
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
            this.textBoxData = new System.Windows.Forms.TextBox();
            this.textBoxSql = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCustom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxTab = new System.Windows.Forms.CheckBox();
            this.checkBoxComma = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBoxDataFromTxtFile = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxData
            // 
            this.textBoxData.Location = new System.Drawing.Point(12, 23);
            this.textBoxData.Multiline = true;
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxData.Size = new System.Drawing.Size(818, 495);
            this.textBoxData.TabIndex = 0;
            // 
            // textBoxSql
            // 
            this.textBoxSql.Location = new System.Drawing.Point(12, 550);
            this.textBoxSql.Multiline = true;
            this.textBoxSql.Name = "textBoxSql";
            this.textBoxSql.Size = new System.Drawing.Size(818, 63);
            this.textBoxSql.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 533);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "sqlCommand";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Data";
            // 
            // textBoxCustom
            // 
            this.textBoxCustom.Location = new System.Drawing.Point(846, 81);
            this.textBoxCustom.Multiline = true;
            this.textBoxCustom.Name = "textBoxCustom";
            this.textBoxCustom.Size = new System.Drawing.Size(115, 47);
            this.textBoxCustom.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(844, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "分隔符";
            // 
            // checkBoxTab
            // 
            this.checkBoxTab.AutoSize = true;
            this.checkBoxTab.Location = new System.Drawing.Point(847, 24);
            this.checkBoxTab.Name = "checkBoxTab";
            this.checkBoxTab.Size = new System.Drawing.Size(78, 16);
            this.checkBoxTab.TabIndex = 6;
            this.checkBoxTab.Text = "Tab分隔符";
            this.checkBoxTab.UseVisualStyleBackColor = true;
            // 
            // checkBoxComma
            // 
            this.checkBoxComma.AutoSize = true;
            this.checkBoxComma.Location = new System.Drawing.Point(847, 46);
            this.checkBoxComma.Name = "checkBoxComma";
            this.checkBoxComma.Size = new System.Drawing.Size(72, 16);
            this.checkBoxComma.TabIndex = 7;
            this.checkBoxComma.Text = "，分隔符";
            this.checkBoxComma.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(845, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "自定义分隔符";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(836, 236);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(157, 281);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "说明\r\n  data的每一行为一个记录，用分隔符分割为N个Item，\r\n for i 0 To N   \r\nsqlcommand.replace（“[id”+i+" +
                "\"]\",item[i])\r\n   执行命令";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(844, 548);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 73);
            this.button1.TabIndex = 10;
            this.button1.Text = "执行";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBoxDataFromTxtFile
            // 
            this.checkBoxDataFromTxtFile.AutoSize = true;
            this.checkBoxDataFromTxtFile.Location = new System.Drawing.Point(846, 177);
            this.checkBoxDataFromTxtFile.Name = "checkBoxDataFromTxtFile";
            this.checkBoxDataFromTxtFile.Size = new System.Drawing.Size(144, 16);
            this.checkBoxDataFromTxtFile.TabIndex = 11;
            this.checkBoxDataFromTxtFile.Text = "从文本文件中读取数据";
            this.checkBoxDataFromTxtFile.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(836, 199);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(157, 31);
            this.textBox2.TabIndex = 9;
            this.textBox2.Text = "左侧Data中为文本地址";
            // 
            // FormData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 625);
            this.Controls.Add(this.checkBoxDataFromTxtFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBoxComma);
            this.Controls.Add(this.checkBoxTab);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxCustom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSql);
            this.Controls.Add(this.textBoxData);
            this.Name = "FormData";
            this.Text = "FormData";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxData;
        private System.Windows.Forms.TextBox textBoxSql;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCustom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxTab;
        private System.Windows.Forms.CheckBox checkBoxComma;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxDataFromTxtFile;
        private System.Windows.Forms.TextBox textBox2;
    }
}