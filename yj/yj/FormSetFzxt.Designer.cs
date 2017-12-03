namespace yj
{
    partial class FormSetFzxt
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
        	this.dgv = new System.Windows.Forms.DataGridView();
        	this.textBox1 = new System.Windows.Forms.TextBox();
        	this.buttonSetOK = new System.Windows.Forms.Button();
        	this.tableLayoutPanel1.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// tableLayoutPanel1
        	// 
        	this.tableLayoutPanel1.ColumnCount = 3;
        	this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        	this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        	this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
        	this.tableLayoutPanel1.Controls.Add(this.dgv, 1, 1);
        	this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 0);
        	this.tableLayoutPanel1.Controls.Add(this.buttonSetOK, 1, 2);
        	this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        	this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        	this.tableLayoutPanel1.RowCount = 3;
        	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
        	this.tableLayoutPanel1.Size = new System.Drawing.Size(1007, 575);
        	this.tableLayoutPanel1.TabIndex = 0;
        	// 
        	// dgv
        	// 
        	this.dgv.AllowUserToAddRows = false;
        	this.dgv.AllowUserToDeleteRows = false;
        	this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        	this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.dgv.Location = new System.Drawing.Point(33, 23);
        	this.dgv.Name = "dgv";
        	this.dgv.RowTemplate.Height = 23;
        	this.dgv.Size = new System.Drawing.Size(885, 522);
        	this.dgv.TabIndex = 0;
        	// 
        	// textBox1
        	// 
        	this.textBox1.Location = new System.Drawing.Point(33, 3);
        	this.textBox1.Name = "textBox1";
        	this.textBox1.ReadOnly = true;
        	this.textBox1.Size = new System.Drawing.Size(885, 21);
        	this.textBox1.TabIndex = 1;
        	this.textBox1.Text = "只能更改题组名称 和 最大分值 所以题组都要改完\r\n";
        	// 
        	// buttonSetOK
        	// 
        	this.buttonSetOK.Location = new System.Drawing.Point(33, 551);
        	this.buttonSetOK.Name = "buttonSetOK";
        	this.buttonSetOK.Size = new System.Drawing.Size(83, 21);
        	this.buttonSetOK.TabIndex = 2;
        	this.buttonSetOK.Text = "设置完成";
        	this.buttonSetOK.UseVisualStyleBackColor = true;
        	this.buttonSetOK.Click += new System.EventHandler(this.buttonSetOK_Click);
        	// 
        	// FormSetFzxt
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(1007, 575);
        	this.Controls.Add(this.tableLayoutPanel1);
        	this.Name = "FormSetFzxt";
        	this.Text = "FormSetFzxt";
        	this.tableLayoutPanel1.ResumeLayout(false);
        	this.tableLayoutPanel1.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonSetOK;
    }
}