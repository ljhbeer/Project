namespace ScanTemplate
{
    partial class FormSetOptionGroups
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBeginID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxEndID = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonAddToOptionGroups = new System.Windows.Forms.Button();
            this.listBoxOptiongroups = new System.Windows.Forms.ListBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonSetOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始题号";
            // 
            // textBoxBeginID
            // 
            this.textBoxBeginID.Location = new System.Drawing.Point(78, 16);
            this.textBoxBeginID.Name = "textBoxBeginID";
            this.textBoxBeginID.ReadOnly = true;
            this.textBoxBeginID.Size = new System.Drawing.Size(55, 21);
            this.textBoxBeginID.TabIndex = 1;
            this.textBoxBeginID.Text = "1";
            this.textBoxBeginID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "结束题号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(97, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "↓";
            // 
            // comboBoxEndID
            // 
            this.comboBoxEndID.FormattingEnabled = true;
            this.comboBoxEndID.Location = new System.Drawing.Point(79, 68);
            this.comboBoxEndID.Name = "comboBoxEndID";
            this.comboBoxEndID.Size = new System.Drawing.Size(53, 20);
            this.comboBoxEndID.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "题组名称";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(64, 112);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(69, 21);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.Text = "选择题组1";
            this.textBoxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonAddToOptionGroups
            // 
            this.buttonAddToOptionGroups.Location = new System.Drawing.Point(7, 152);
            this.buttonAddToOptionGroups.Name = "buttonAddToOptionGroups";
            this.buttonAddToOptionGroups.Size = new System.Drawing.Size(125, 38);
            this.buttonAddToOptionGroups.TabIndex = 3;
            this.buttonAddToOptionGroups.Text = "添加到选择题题组";
            this.buttonAddToOptionGroups.UseVisualStyleBackColor = true;
            this.buttonAddToOptionGroups.Click += new System.EventHandler(this.buttonAddToOptionGroups_Click);
            // 
            // listBoxOptiongroups
            // 
            this.listBoxOptiongroups.FormattingEnabled = true;
            this.listBoxOptiongroups.ItemHeight = 12;
            this.listBoxOptiongroups.Location = new System.Drawing.Point(212, 28);
            this.listBoxOptiongroups.Name = "listBoxOptiongroups";
            this.listBoxOptiongroups.Size = new System.Drawing.Size(216, 148);
            this.listBoxOptiongroups.TabIndex = 4;
            this.listBoxOptiongroups.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxOptiongroups_KeyUp);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(212, 8);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(216, 21);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "选择题题组";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSetOK
            // 
            this.buttonSetOK.Location = new System.Drawing.Point(93, 217);
            this.buttonSetOK.Name = "buttonSetOK";
            this.buttonSetOK.Size = new System.Drawing.Size(98, 38);
            this.buttonSetOK.TabIndex = 3;
            this.buttonSetOK.Text = "设置完成";
            this.buttonSetOK.UseVisualStyleBackColor = true;
            this.buttonSetOK.Click += new System.EventHandler(this.buttonSetOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(210, 217);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(98, 38);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消设置";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(212, 175);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(216, 36);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "   如果更改题组设置，请从最后一项，按 Delete 键删除后，重新添加";
            // 
            // FormSetOptionGroups
            // 
            this.AcceptButton = this.buttonSetOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(431, 255);
            this.Controls.Add(this.listBoxOptiongroups);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSetOK);
            this.Controls.Add(this.buttonAddToOptionGroups);
            this.Controls.Add(this.comboBoxEndID);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBoxBeginID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "FormSetOptionGroups";
            this.Text = "设置选择题题组名称";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxBeginID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxEndID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonAddToOptionGroups;
        private System.Windows.Forms.ListBox listBoxOptiongroups;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonSetOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBox1;
    }
}