namespace SoftRegTools
{
    partial class FormRegShow
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxReadMe = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMachineCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxRegCodeHash = new System.Windows.Forms.TextBox();
            this.textBoxRegstate = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonReg = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxJG = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxReadMe, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxMachineCode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxRegCodeHash, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxRegstate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxJG, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(604, 354);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "注册说明";
            // 
            // textBoxReadMe
            // 
            this.textBoxReadMe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReadMe.Location = new System.Drawing.Point(67, 98);
            this.textBoxReadMe.Multiline = true;
            this.textBoxReadMe.Name = "textBoxReadMe";
            this.textBoxReadMe.ReadOnly = true;
            this.textBoxReadMe.Size = new System.Drawing.Size(514, 188);
            this.textBoxReadMe.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "机器码";
            // 
            // textBoxMachineCode
            // 
            this.textBoxMachineCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMachineCode.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxMachineCode.Location = new System.Drawing.Point(67, 3);
            this.textBoxMachineCode.Name = "textBoxMachineCode";
            this.textBoxMachineCode.ReadOnly = true;
            this.textBoxMachineCode.Size = new System.Drawing.Size(514, 29);
            this.textBoxMachineCode.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "注册代码";
            // 
            // textBoxRegCodeHash
            // 
            this.textBoxRegCodeHash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRegCodeHash.Location = new System.Drawing.Point(67, 38);
            this.textBoxRegCodeHash.Name = "textBoxRegCodeHash";
            this.textBoxRegCodeHash.Size = new System.Drawing.Size(514, 21);
            this.textBoxRegCodeHash.TabIndex = 5;
            // 
            // textBoxRegstate
            // 
            this.textBoxRegstate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRegstate.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxRegstate.ForeColor = System.Drawing.Color.Red;
            this.textBoxRegstate.Location = new System.Drawing.Point(67, 63);
            this.textBoxRegstate.Name = "textBoxRegstate";
            this.textBoxRegstate.ReadOnly = true;
            this.textBoxRegstate.Size = new System.Drawing.Size(514, 34);
            this.textBoxRegstate.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonReg);
            this.flowLayoutPanel1.Controls.Add(this.buttonClose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(67, 292);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(514, 34);
            this.flowLayoutPanel1.TabIndex = 10;
            // 
            // buttonReg
            // 
            this.buttonReg.Location = new System.Drawing.Point(3, 3);
            this.buttonReg.Name = "buttonReg";
            this.buttonReg.Size = new System.Drawing.Size(97, 34);
            this.buttonReg.TabIndex = 8;
            this.buttonReg.Text = "注册";
            this.buttonReg.UseVisualStyleBackColor = true;
            this.buttonReg.Click += new System.EventHandler(this.buttonReg_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(106, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(97, 34);
            this.buttonClose.TabIndex = 9;
            this.buttonClose.Text = "暂不注册";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxJG
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxJG, 3);
            this.textBoxJG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxJG.Location = new System.Drawing.Point(3, 332);
            this.textBoxJG.Name = "textBoxJG";
            this.textBoxJG.ReadOnly = true;
            this.textBoxJG.Size = new System.Drawing.Size(598, 21);
            this.textBoxJG.TabIndex = 1;
            // 
            // FormRegShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 354);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormRegShow";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxReadMe;
        private System.Windows.Forms.TextBox textBoxMachineCode;
        private System.Windows.Forms.TextBox textBoxRegstate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxRegCodeHash;
        private System.Windows.Forms.Button buttonReg;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxJG;
    }
}

