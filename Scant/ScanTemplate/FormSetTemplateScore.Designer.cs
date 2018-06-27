namespace ScanTemplate
{
    partial class FormSetTemplateScore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetTemplateScore));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.panelxzt = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonClearanswer = new System.Windows.Forms.Button();
            this.buttonImportMultiAnswer = new System.Windows.Forms.Button();
            this.buttonImportAnswer = new System.Windows.Forms.Button();
            this.textBoxAnswer = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.panelxzt.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 141F));
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgv, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelxzt, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(651, 408);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(76, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(96, 20);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(76, 28);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(431, 377);
            this.dgv.TabIndex = 2;
            this.dgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellValueChanged);
            // 
            // panelxzt
            // 
            this.panelxzt.Controls.Add(this.textBox1);
            this.panelxzt.Controls.Add(this.buttonOK);
            this.panelxzt.Controls.Add(this.buttonClearanswer);
            this.panelxzt.Controls.Add(this.buttonImportMultiAnswer);
            this.panelxzt.Controls.Add(this.buttonImportAnswer);
            this.panelxzt.Controls.Add(this.textBoxAnswer);
            this.panelxzt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelxzt.Location = new System.Drawing.Point(513, 28);
            this.panelxzt.Name = "panelxzt";
            this.panelxzt.Size = new System.Drawing.Size(135, 377);
            this.panelxzt.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(4, 170);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(232, 176);
            this.textBox1.TabIndex = 74;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.Visible = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(2, 346);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(97, 28);
            this.buttonOK.TabIndex = 73;
            this.buttonOK.Text = "设置完成";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonClearanswer
            // 
            this.buttonClearanswer.Location = new System.Drawing.Point(175, 126);
            this.buttonClearanswer.Name = "buttonClearanswer";
            this.buttonClearanswer.Size = new System.Drawing.Size(52, 25);
            this.buttonClearanswer.TabIndex = 72;
            this.buttonClearanswer.Text = "清空";
            this.buttonClearanswer.UseVisualStyleBackColor = true;
            // 
            // buttonImportMultiAnswer
            // 
            this.buttonImportMultiAnswer.Location = new System.Drawing.Point(62, 89);
            this.buttonImportMultiAnswer.Name = "buttonImportMultiAnswer";
            this.buttonImportMultiAnswer.Size = new System.Drawing.Size(70, 33);
            this.buttonImportMultiAnswer.TabIndex = 61;
            this.buttonImportMultiAnswer.Text = "格式化";
            this.buttonImportMultiAnswer.UseVisualStyleBackColor = true;
            this.buttonImportMultiAnswer.Click += new System.EventHandler(this.buttonImportMultiAnswer_Click);
            // 
            // buttonImportAnswer
            // 
            this.buttonImportAnswer.Location = new System.Drawing.Point(5, 89);
            this.buttonImportAnswer.Name = "buttonImportAnswer";
            this.buttonImportAnswer.Size = new System.Drawing.Size(51, 33);
            this.buttonImportAnswer.TabIndex = 60;
            this.buttonImportAnswer.Text = "导入";
            this.buttonImportAnswer.UseVisualStyleBackColor = true;
            this.buttonImportAnswer.Click += new System.EventHandler(this.buttonImportAnswer_Click);
            // 
            // textBoxAnswer
            // 
            this.textBoxAnswer.Location = new System.Drawing.Point(3, 3);
            this.textBoxAnswer.Multiline = true;
            this.textBoxAnswer.Name = "textBoxAnswer";
            this.textBoxAnswer.Size = new System.Drawing.Size(132, 80);
            this.textBoxAnswer.TabIndex = 59;
            // 
            // FormSetTemplateScore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 408);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormSetTemplateScore";
            this.Text = "FormSetTemplateScore";
            this.Load += new System.EventHandler(this.FormSetTemplateScore_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.panelxzt.ResumeLayout(false);
            this.panelxzt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Panel panelxzt;
        private System.Windows.Forms.Button buttonImportAnswer;
        private System.Windows.Forms.TextBox textBoxAnswer;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonClearanswer;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonImportMultiAnswer;
    }
}