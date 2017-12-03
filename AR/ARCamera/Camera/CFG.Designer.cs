namespace Camera
{
    partial class CFGForm
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
            this.btnImportXmlCfg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImportXmlCfg
            // 
            this.btnImportXmlCfg.Location = new System.Drawing.Point(12, 13);
            this.btnImportXmlCfg.Name = "btnImportXmlCfg";
            this.btnImportXmlCfg.Size = new System.Drawing.Size(71, 20);
            this.btnImportXmlCfg.TabIndex = 0;
            this.btnImportXmlCfg.Text = "导入配置";
            this.btnImportXmlCfg.UseVisualStyleBackColor = true;
            this.btnImportXmlCfg.Click += new System.EventHandler(this.btnImportXmlCfg_Click);
            // 
            // CFG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 391);
            this.Controls.Add(this.btnImportXmlCfg);
            this.Name = "CFG";
            this.Text = "CFG";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImportXmlCfg;
    }
}