﻿namespace DBQuery
{
    partial class Form1
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
            this.comboBoxtablename = new System.Windows.Forms.ComboBox();
            this.dataGridViewtablestruct = new System.Windows.Forms.DataGridView();
            this.textBoxtable = new System.Windows.Forms.TextBox();
            this.lable1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonrefreshtable = new System.Windows.Forms.Button();
            this.textBoxquery = new System.Windows.Forms.TextBox();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.buttonDone = new System.Windows.Forms.Button();
            this.textBoxShow = new System.Windows.Forms.TextBox();
            this.comboBoxSQLname = new System.Windows.Forms.ComboBox();
            this.buttonSaveSqlCmd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSQLname = new System.Windows.Forms.TextBox();
            this.btnimport = new System.Windows.Forms.Button();
            this.buttonDelSQL = new System.Windows.Forms.Button();
            this.buttonSetDatabase = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBoxaction = new System.Windows.Forms.ListBox();
            this.buttonsaveupdateaction = new System.Windows.Forms.Button();
            this.buttonsaveselectsingleaction = new System.Windows.Forms.Button();
            this.buttonsaveselectmultiaction = new System.Windows.Forms.Button();
            this.buttonSetSQLDatabase = new System.Windows.Forms.Button();
            this.checkBoxOutput = new System.Windows.Forms.CheckBox();
            this.checkBoxShowTopRc100 = new System.Windows.Forms.CheckBox();
            this.buttonFormData = new System.Windows.Forms.Button();
            this.buttonUpdateToDatabase = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewtablestruct)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxtablename
            // 
            this.comboBoxtablename.FormattingEnabled = true;
            this.comboBoxtablename.Location = new System.Drawing.Point(2, 38);
            this.comboBoxtablename.Name = "comboBoxtablename";
            this.comboBoxtablename.Size = new System.Drawing.Size(137, 20);
            this.comboBoxtablename.TabIndex = 0;
            this.comboBoxtablename.SelectedIndexChanged += new System.EventHandler(this.comboBoxtablename_SelectedIndexChanged);
            // 
            // dataGridViewtablestruct
            // 
            this.dataGridViewtablestruct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewtablestruct.Location = new System.Drawing.Point(162, 201);
            this.dataGridViewtablestruct.Name = "dataGridViewtablestruct";
            this.dataGridViewtablestruct.RowTemplate.Height = 23;
            this.dataGridViewtablestruct.Size = new System.Drawing.Size(648, 316);
            this.dataGridViewtablestruct.TabIndex = 1;
            // 
            // textBoxtable
            // 
            this.textBoxtable.Location = new System.Drawing.Point(1, 62);
            this.textBoxtable.Multiline = true;
            this.textBoxtable.Name = "textBoxtable";
            this.textBoxtable.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxtable.Size = new System.Drawing.Size(138, 169);
            this.textBoxtable.TabIndex = 2;
            this.textBoxtable.WordWrap = false;
            // 
            // lable1
            // 
            this.lable1.AutoSize = true;
            this.lable1.Location = new System.Drawing.Point(152, 20);
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size(47, 12);
            this.lable1.TabIndex = 3;
            this.lable1.Text = "查询SQL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "执行SQL";
            // 
            // buttonrefreshtable
            // 
            this.buttonrefreshtable.Location = new System.Drawing.Point(3, 6);
            this.buttonrefreshtable.Name = "buttonrefreshtable";
            this.buttonrefreshtable.Size = new System.Drawing.Size(93, 29);
            this.buttonrefreshtable.TabIndex = 5;
            this.buttonrefreshtable.Text = "刷新表结构";
            this.buttonrefreshtable.UseVisualStyleBackColor = true;
            this.buttonrefreshtable.Click += new System.EventHandler(this.buttonrefreshtable_Click);
            // 
            // textBoxquery
            // 
            this.textBoxquery.Location = new System.Drawing.Point(206, 23);
            this.textBoxquery.Multiline = true;
            this.textBoxquery.Name = "textBoxquery";
            this.textBoxquery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxquery.Size = new System.Drawing.Size(466, 176);
            this.textBoxquery.TabIndex = 6;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(152, 35);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(46, 23);
            this.buttonQuery.TabIndex = 8;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(152, 76);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(46, 23);
            this.buttonDone.TabIndex = 9;
            this.buttonDone.Text = "执行";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // textBoxShow
            // 
            this.textBoxShow.Location = new System.Drawing.Point(174, 519);
            this.textBoxShow.Name = "textBoxShow";
            this.textBoxShow.ReadOnly = true;
            this.textBoxShow.Size = new System.Drawing.Size(635, 21);
            this.textBoxShow.TabIndex = 10;
            // 
            // comboBoxSQLname
            // 
            this.comboBoxSQLname.FormattingEnabled = true;
            this.comboBoxSQLname.Location = new System.Drawing.Point(678, 50);
            this.comboBoxSQLname.MaxDropDownItems = 20;
            this.comboBoxSQLname.Name = "comboBoxSQLname";
            this.comboBoxSQLname.Size = new System.Drawing.Size(132, 20);
            this.comboBoxSQLname.TabIndex = 11;
            this.comboBoxSQLname.SelectedIndexChanged += new System.EventHandler(this.comboBoxSQLname_SelectedIndexChanged);
            // 
            // buttonSaveSqlCmd
            // 
            this.buttonSaveSqlCmd.Location = new System.Drawing.Point(678, 23);
            this.buttonSaveSqlCmd.Name = "buttonSaveSqlCmd";
            this.buttonSaveSqlCmd.Size = new System.Drawing.Size(131, 25);
            this.buttonSaveSqlCmd.TabIndex = 12;
            this.buttonSaveSqlCmd.Text = "保存SQL到数据库";
            this.buttonSaveSqlCmd.UseVisualStyleBackColor = true;
            this.buttonSaveSqlCmd.Click += new System.EventHandler(this.buttonSaveSqlCmd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(719, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "SQL名称";
            // 
            // textBoxSQLname
            // 
            this.textBoxSQLname.Location = new System.Drawing.Point(678, 87);
            this.textBoxSQLname.Name = "textBoxSQLname";
            this.textBoxSQLname.Size = new System.Drawing.Size(130, 21);
            this.textBoxSQLname.TabIndex = 14;
            // 
            // btnimport
            // 
            this.btnimport.Location = new System.Drawing.Point(1, 518);
            this.btnimport.Name = "btnimport";
            this.btnimport.Size = new System.Drawing.Size(79, 20);
            this.btnimport.TabIndex = 17;
            this.btnimport.Text = "导出";
            this.btnimport.UseVisualStyleBackColor = true;
            this.btnimport.Click += new System.EventHandler(this.btnimport_Click);
            this.btnimport.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btnimport_KeyUp);
            // 
            // buttonDelSQL
            // 
            this.buttonDelSQL.Location = new System.Drawing.Point(678, 110);
            this.buttonDelSQL.Name = "buttonDelSQL";
            this.buttonDelSQL.Size = new System.Drawing.Size(131, 25);
            this.buttonDelSQL.TabIndex = 20;
            this.buttonDelSQL.Text = "删除当前SQL";
            this.buttonDelSQL.UseVisualStyleBackColor = true;
            this.buttonDelSQL.Click += new System.EventHandler(this.buttonDelSQL_Click);
            // 
            // buttonSetDatabase
            // 
            this.buttonSetDatabase.Location = new System.Drawing.Point(80, 492);
            this.buttonSetDatabase.Name = "buttonSetDatabase";
            this.buttonSetDatabase.Size = new System.Drawing.Size(76, 20);
            this.buttonSetDatabase.TabIndex = 21;
            this.buttonSetDatabase.Text = "更改数据库";
            this.buttonSetDatabase.UseVisualStyleBackColor = true;
            this.buttonSetDatabase.Click += new System.EventHandler(this.buttonSetDatabase_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(212, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(120, 16);
            this.checkBox1.TabIndex = 22;
            this.checkBox1.Text = "执行语句含有文件";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 235);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "动作列表";
            // 
            // listBoxaction
            // 
            this.listBoxaction.FormattingEnabled = true;
            this.listBoxaction.ItemHeight = 12;
            this.listBoxaction.Location = new System.Drawing.Point(1, 251);
            this.listBoxaction.Name = "listBoxaction";
            this.listBoxaction.Size = new System.Drawing.Size(138, 232);
            this.listBoxaction.TabIndex = 24;
            this.listBoxaction.SelectedIndexChanged += new System.EventHandler(this.listBoxaction_SelectedIndexChanged);
            this.listBoxaction.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxaction_KeyUp);
            // 
            // buttonsaveupdateaction
            // 
            this.buttonsaveupdateaction.Location = new System.Drawing.Point(332, 1);
            this.buttonsaveupdateaction.Name = "buttonsaveupdateaction";
            this.buttonsaveupdateaction.Size = new System.Drawing.Size(87, 20);
            this.buttonsaveupdateaction.TabIndex = 25;
            this.buttonsaveupdateaction.Text = "保存执行动作";
            this.buttonsaveupdateaction.UseVisualStyleBackColor = true;
            this.buttonsaveupdateaction.Click += new System.EventHandler(this.buttonsaveupdateaction_Click);
            // 
            // buttonsaveselectsingleaction
            // 
            this.buttonsaveselectsingleaction.Location = new System.Drawing.Point(425, 1);
            this.buttonsaveselectsingleaction.Name = "buttonsaveselectsingleaction";
            this.buttonsaveselectsingleaction.Size = new System.Drawing.Size(121, 20);
            this.buttonsaveselectsingleaction.TabIndex = 26;
            this.buttonsaveselectsingleaction.Text = "保存查询单结果动作";
            this.buttonsaveselectsingleaction.UseVisualStyleBackColor = true;
            this.buttonsaveselectsingleaction.Click += new System.EventHandler(this.buttonsaveselectsingleaction_Click);
            // 
            // buttonsaveselectmultiaction
            // 
            this.buttonsaveselectmultiaction.Location = new System.Drawing.Point(552, 1);
            this.buttonsaveselectmultiaction.Name = "buttonsaveselectmultiaction";
            this.buttonsaveselectmultiaction.Size = new System.Drawing.Size(121, 20);
            this.buttonsaveselectmultiaction.TabIndex = 27;
            this.buttonsaveselectmultiaction.Text = "保存查询多结果动作";
            this.buttonsaveselectmultiaction.UseVisualStyleBackColor = true;
            this.buttonsaveselectmultiaction.Click += new System.EventHandler(this.buttonsaveselectmultiaction_Click);
            // 
            // buttonSetSQLDatabase
            // 
            this.buttonSetSQLDatabase.Location = new System.Drawing.Point(-3, 492);
            this.buttonSetSQLDatabase.Name = "buttonSetSQLDatabase";
            this.buttonSetSQLDatabase.Size = new System.Drawing.Size(83, 20);
            this.buttonSetSQLDatabase.TabIndex = 28;
            this.buttonSetSQLDatabase.Text = "更改SQL数据库";
            this.buttonSetSQLDatabase.UseVisualStyleBackColor = true;
            this.buttonSetSQLDatabase.Click += new System.EventHandler(this.buttonSetSQLDatabase_Click);
            // 
            // checkBoxOutput
            // 
            this.checkBoxOutput.AutoSize = true;
            this.checkBoxOutput.Location = new System.Drawing.Point(678, 156);
            this.checkBoxOutput.Name = "checkBoxOutput";
            this.checkBoxOutput.Size = new System.Drawing.Size(84, 16);
            this.checkBoxOutput.TabIndex = 29;
            this.checkBoxOutput.Text = "输出到文件";
            this.checkBoxOutput.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowTopRc100
            // 
            this.checkBoxShowTopRc100.AutoSize = true;
            this.checkBoxShowTopRc100.Location = new System.Drawing.Point(96, 5);
            this.checkBoxShowTopRc100.Name = "checkBoxShowTopRc100";
            this.checkBoxShowTopRc100.Size = new System.Drawing.Size(114, 16);
            this.checkBoxShowTopRc100.TabIndex = 30;
            this.checkBoxShowTopRc100.Text = "显示前100条记录";
            this.checkBoxShowTopRc100.UseVisualStyleBackColor = true;
            // 
            // buttonFormData
            // 
            this.buttonFormData.Location = new System.Drawing.Point(80, 518);
            this.buttonFormData.Name = "buttonFormData";
            this.buttonFormData.Size = new System.Drawing.Size(88, 20);
            this.buttonFormData.TabIndex = 31;
            this.buttonFormData.Text = "导入文本记录";
            this.buttonFormData.UseVisualStyleBackColor = true;
            this.buttonFormData.Click += new System.EventHandler(this.buttonFormData_Click);
            // 
            // buttonUpdateToDatabase
            // 
            this.buttonUpdateToDatabase.Location = new System.Drawing.Point(743, 175);
            this.buttonUpdateToDatabase.Name = "buttonUpdateToDatabase";
            this.buttonUpdateToDatabase.Size = new System.Drawing.Size(67, 24);
            this.buttonUpdateToDatabase.TabIndex = 31;
            this.buttonUpdateToDatabase.Text = "更新到数据库";
            this.buttonUpdateToDatabase.UseVisualStyleBackColor = true;
            this.buttonUpdateToDatabase.Click += new System.EventHandler(this.buttonUpdateToDatabase_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 540);
            this.Controls.Add(this.buttonFormData);
            this.Controls.Add(this.buttonUpdateToDatabase);
            this.Controls.Add(this.checkBoxShowTopRc100);
            this.Controls.Add(this.checkBoxOutput);
            this.Controls.Add(this.buttonSetSQLDatabase);
            this.Controls.Add(this.buttonsaveselectmultiaction);
            this.Controls.Add(this.buttonsaveselectsingleaction);
            this.Controls.Add(this.buttonsaveupdateaction);
            this.Controls.Add(this.listBoxaction);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.buttonSetDatabase);
            this.Controls.Add(this.buttonDelSQL);
            this.Controls.Add(this.btnimport);
            this.Controls.Add(this.textBoxSQLname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSaveSqlCmd);
            this.Controls.Add(this.comboBoxSQLname);
            this.Controls.Add(this.textBoxShow);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.textBoxquery);
            this.Controls.Add(this.buttonrefreshtable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lable1);
            this.Controls.Add(this.textBoxtable);
            this.Controls.Add(this.dataGridViewtablestruct);
            this.Controls.Add(this.comboBoxtablename);
            this.Name = "Form1";
            this.Text = "数据表工具";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewtablestruct)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxtablename;
        private System.Windows.Forms.DataGridView dataGridViewtablestruct;
        private System.Windows.Forms.TextBox textBoxtable;
        private System.Windows.Forms.Label lable1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonrefreshtable;
        private System.Windows.Forms.TextBox textBoxquery;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.Button buttonDone;
        private System.Windows.Forms.TextBox textBoxShow;
        private System.Windows.Forms.ComboBox comboBoxSQLname;
        private System.Windows.Forms.Button buttonSaveSqlCmd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSQLname;
        private System.Windows.Forms.Button btnimport;
        private System.Windows.Forms.Button buttonDelSQL;
        private System.Windows.Forms.Button buttonSetDatabase;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBoxaction;
        private System.Windows.Forms.Button buttonsaveupdateaction;
        private System.Windows.Forms.Button buttonsaveselectsingleaction;
        private System.Windows.Forms.Button buttonsaveselectmultiaction;
        private System.Windows.Forms.Button buttonSetSQLDatabase;
        private System.Windows.Forms.CheckBox checkBoxOutput;
        private System.Windows.Forms.CheckBox checkBoxShowTopRc100;
        private System.Windows.Forms.Button buttonFormData;
        private System.Windows.Forms.Button buttonUpdateToDatabase;
    }
}

