namespace AR
{
    partial class FormSetscore
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
            this.buttonClearanswer = new System.Windows.Forms.Button();
            this.buttonSetScore = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxScore = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxEnd = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxBegin = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewAnswer = new System.Windows.Forms.DataGridView();
            this.buttonImportAnswer = new System.Windows.Forms.Button();
            this.textBoxAnswer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAnswer)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClearanswer
            // 
            this.buttonClearanswer.Location = new System.Drawing.Point(5, 67);
            this.buttonClearanswer.Name = "buttonClearanswer";
            this.buttonClearanswer.Size = new System.Drawing.Size(52, 25);
            this.buttonClearanswer.TabIndex = 50;
            this.buttonClearanswer.Text = "清空";
            this.buttonClearanswer.UseVisualStyleBackColor = true;
            this.buttonClearanswer.Click += new System.EventHandler(this.buttonClearanswer_Click);
            // 
            // buttonSetScore
            // 
            this.buttonSetScore.Location = new System.Drawing.Point(415, 59);
            this.buttonSetScore.Name = "buttonSetScore";
            this.buttonSetScore.Size = new System.Drawing.Size(37, 27);
            this.buttonSetScore.TabIndex = 49;
            this.buttonSetScore.Text = "设置";
            this.buttonSetScore.UseVisualStyleBackColor = true;
            this.buttonSetScore.Click += new System.EventHandler(this.buttonSetScore_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(392, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 48;
            this.label8.Text = "分";
            // 
            // textBoxScore
            // 
            this.textBoxScore.Location = new System.Drawing.Point(338, 63);
            this.textBoxScore.Name = "textBoxScore";
            this.textBoxScore.Size = new System.Drawing.Size(51, 21);
            this.textBoxScore.TabIndex = 47;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(304, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 46;
            this.label7.Text = "设置";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(430, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 45;
            this.label6.Text = "题";
            // 
            // comboBoxEnd
            // 
            this.comboBoxEnd.FormattingEnabled = true;
            this.comboBoxEnd.Location = new System.Drawing.Point(379, 36);
            this.comboBoxEnd.Name = "comboBoxEnd";
            this.comboBoxEnd.Size = new System.Drawing.Size(45, 20);
            this.comboBoxEnd.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(363, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 43;
            this.label5.Text = "到";
            // 
            // comboBoxBegin
            // 
            this.comboBoxBegin.FormattingEnabled = true;
            this.comboBoxBegin.Location = new System.Drawing.Point(319, 36);
            this.comboBoxBegin.Name = "comboBoxBegin";
            this.comboBoxBegin.Size = new System.Drawing.Size(45, 20);
            this.comboBoxBegin.TabIndex = 42;
            this.comboBoxBegin.SelectedIndexChanged += new System.EventHandler(this.comboBoxBegin_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(303, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 41;
            this.label4.Text = "从";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 40;
            this.label1.Text = "批量设置分值";
            // 
            // dataGridViewAnswer
            // 
            this.dataGridViewAnswer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAnswer.Location = new System.Drawing.Point(5, 96);
            this.dataGridViewAnswer.Name = "dataGridViewAnswer";
            this.dataGridViewAnswer.RowTemplate.Height = 23;
            this.dataGridViewAnswer.Size = new System.Drawing.Size(292, 417);
            this.dataGridViewAnswer.TabIndex = 39;
            // 
            // buttonImportAnswer
            // 
            this.buttonImportAnswer.Location = new System.Drawing.Point(4, 41);
            this.buttonImportAnswer.Name = "buttonImportAnswer";
            this.buttonImportAnswer.Size = new System.Drawing.Size(52, 25);
            this.buttonImportAnswer.TabIndex = 38;
            this.buttonImportAnswer.Text = "导入";
            this.buttonImportAnswer.UseVisualStyleBackColor = true;
            this.buttonImportAnswer.Click += new System.EventHandler(this.buttonImportAnswer_Click);
            // 
            // textBoxAnswer
            // 
            this.textBoxAnswer.Location = new System.Drawing.Point(62, 12);
            this.textBoxAnswer.Multiline = true;
            this.textBoxAnswer.Name = "textBoxAnswer";
            this.textBoxAnswer.Size = new System.Drawing.Size(235, 80);
            this.textBoxAnswer.TabIndex = 37;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 36;
            this.label3.Text = "导入答案";
            // 
            // FormSetscore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 516);
            this.Controls.Add(this.buttonClearanswer);
            this.Controls.Add(this.buttonSetScore);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxScore);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxEnd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxBegin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewAnswer);
            this.Controls.Add(this.buttonImportAnswer);
            this.Controls.Add(this.textBoxAnswer);
            this.Controls.Add(this.label3);
            this.Name = "FormSetscore";
            this.Text = "FormSetscore";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAnswer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClearanswer;
        private System.Windows.Forms.Button buttonSetScore;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxScore;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxBegin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewAnswer;
        private System.Windows.Forms.Button buttonImportAnswer;
        private System.Windows.Forms.TextBox textBoxAnswer;
        private System.Windows.Forms.Label label3;
    }
}