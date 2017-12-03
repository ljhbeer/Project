using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AR
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();           
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 354);
            this.Name = "InputBox";
            this.Text = "InputBox";
            buttonCancel = new Button();
            buttonOK = new Button();
            buttonOK.Text = "确定(&O)";
            buttonCancel.Text = "取消(&O)";
            this.Controls.Add(buttonOK);
            this.Controls.Add(buttonCancel);
            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
            buttonOK.Click += new EventHandler(buttonOK_Click);
            buttonCancel.Click += new EventHandler(buttonCancel_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public static bool Input(string Title, string textTitle, ref string textValue,string numTitle,ref float numValue)
        {
            List<string> subTitle = new List<string>();
            List<string> subValue = new List<string>();
            subTitle.Add(textTitle);
            subTitle.Add(numTitle);
            subValue.AddRange(new string[]{textValue,""});
            if(Input(Title,ref subTitle,ref subValue,2))
            {
                textValue = subValue[0];
                numValue = float.Parse(subValue[1]);
                return true;
            }
            return false;
        }
        public static bool Input(string Title,ref List<string> subTitle, ref List< string> subValue, int Numflag)
        {
            InputBox inputBox = new InputBox();
            inputBox.Text = Title;
            inputBox.numflag = Numflag;
            inputBox.Init(subTitle,subValue);
            DialogResult result = inputBox.ShowDialog();
            if (result == DialogResult.OK)
            {
                if( inputBox.GetValue(subValue))                    
                    return true;
            }
            return false;
        }

        private bool GetValue( List<string> subValue)
        {
            int flag = 1;
            float value;
            for (int i = 0; i < label.Count(); i++)
            {
                subValue[i] = textBox[i].Text;
                if (subValue[i] == "")
                    return false;
                if ((numflag & flag) != 0)
                {
                    if (!float.TryParse(subValue[i], out value))
                        return false;
                }
                flag *= 2;
            }
            return true;
        }

        private void Init(List<string> subTitle, List<string> subValue)
        {
            if (subValue.Count != subValue.Count || subValue.Count==0)
                throw new SyntaxErrorException();
            this.SuspendLayout();     
            label = new Label[subValue.Count];
            textBox = new TextBox[subValue.Count];
            int flag = 1;
            for (int i = 0; i < subValue.Count; i++)
            {
                label[i] = new Label();
                textBox[i] = new TextBox();
                if (subTitle[i].Length > 8)
                    subTitle[i] = subTitle[i].Substring(subTitle[i].Length - 8);
                label[i].Text = subTitle[i];
                label[i].Location = new Point(60, 33+i*30);
                textBox[i].Location = new Point(160, 33+i*30);
                textBox[i].Size = new  Size(200, 21);

                if(  (numflag & flag)!=0)
                    textBox[i].KeyPress += textboxValue_KeyPress;
                flag *= 2;

            }
            buttonOK.Location = new Point(100, 33 + 30 * subValue.Count);
            buttonCancel.Location = new Point(220, 33 + 30 * subValue.Count);
            this.Controls.AddRange(label);
            this.Controls.AddRange(textBox);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        void textboxValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < (char)Keys.D0 || e.KeyChar > (char)Keys.D9) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' || e.KeyChar == '-')
            {
                e.Handled = false ;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label[] label;
        private System.Windows.Forms.TextBox[] textBox;
        private Button buttonOK;
        private Button buttonCancel;
        private int numflag;
    }
    public partial  class FormShowImg : Form
    {
        public FormShowImg()
        {
            InitializeComponent();
        }
        internal void SetImg(Image cropimg)
        {
            pictureBox1.Image = cropimg;
        }
    }
    partial class FormShowImg
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(7, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(643, 526);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 552);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form3";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
