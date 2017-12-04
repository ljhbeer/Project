namespace ScanTemplate
{
    partial class FormVerify
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
        	this.listBoxXH = new System.Windows.Forms.ListBox();
        	this.listBoxXZT = new System.Windows.Forms.ListBox();
        	this.buttonConfirm = new System.Windows.Forms.Button();
        	this.dgv = new System.Windows.Forms.DataGridView();
        	((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// listBoxXH
        	// 
        	this.listBoxXH.FormattingEnabled = true;
        	this.listBoxXH.ItemHeight = 12;
        	this.listBoxXH.Location = new System.Drawing.Point(-2, 12);
        	this.listBoxXH.Name = "listBoxXH";
        	this.listBoxXH.Size = new System.Drawing.Size(10, 40);
        	this.listBoxXH.TabIndex = 0;
        	this.listBoxXH.Visible = false;
        	// 
        	// listBoxXZT
        	// 
        	this.listBoxXZT.FormattingEnabled = true;
        	this.listBoxXZT.ItemHeight = 12;
        	this.listBoxXZT.Location = new System.Drawing.Point(-2, 58);
        	this.listBoxXZT.Name = "listBoxXZT";
        	this.listBoxXZT.Size = new System.Drawing.Size(10, 112);
        	this.listBoxXZT.TabIndex = 0;
        	this.listBoxXZT.Visible = false;
        	// 
        	// buttonConfirm
        	// 
        	this.buttonConfirm.Location = new System.Drawing.Point(594, 651);
        	this.buttonConfirm.Name = "buttonConfirm";
        	this.buttonConfirm.Size = new System.Drawing.Size(46, 34);
        	this.buttonConfirm.TabIndex = 2;
        	this.buttonConfirm.Text = "确认";
        	this.buttonConfirm.UseVisualStyleBackColor = true;
        	this.buttonConfirm.Click += new System.EventHandler(this.buttonImportData_Click);
        	// 
        	// dgv
        	// 
        	this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        	this.dgv.Location = new System.Drawing.Point(14, -2);
        	this.dgv.Name = "dgv";
        	this.dgv.RowTemplate.Height = 23;
        	this.dgv.Size = new System.Drawing.Size(582, 687);
        	this.dgv.TabIndex = 3;
        	this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCellClick);
        	this.dgv.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvCellPainting);
        	// 
        	// FormVerify
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(642, 686);
        	this.Controls.Add(this.dgv);
        	this.Controls.Add(this.buttonConfirm);
        	this.Controls.Add(this.listBoxXZT);
        	this.Controls.Add(this.listBoxXH);
        	this.Name = "FormVerify";
        	this.Text = "FormVerify";
        	this.Load += new System.EventHandler(this.FormVerifyLoad);
        	((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
        	this.ResumeLayout(false);
        }
        private System.Windows.Forms.DataGridView dgv;

        #endregion

        private System.Windows.Forms.ListBox listBoxXH;
        private System.Windows.Forms.ListBox listBoxXZT;
        private System.Windows.Forms.Button buttonConfirm;
    }
}