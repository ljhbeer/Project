using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARTemplate
{
    public partial class FormInputComboBox : Form
    {
        public string StrValue { get; set; }
        private string keyname;
        //private ComboBox.ObjectCollection objectCollection;

        public FormInputComboBox(string keyname, ComboBox.ObjectCollection objectCollection)
        {
            InitializeComponent();
            this.keyname = keyname;
            foreach(Object O in objectCollection)
                this.comboBox1.Items.Add(O);
        }
        public FormInputComboBox(string keyname, List<object> objectCollection)
        {
            InitializeComponent();
            this.keyname = keyname;
            foreach (Object O in objectCollection)
                this.comboBox1.Items.Add(O);
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            if (keyname == "模板" && comboBox1.SelectedIndex != -1)
            {
                StrValue = comboBox1.SelectedItem.ToString();
            }
            else if (keyname == "选择班级" && comboBox1.SelectedIndex != -1)
            {
                StrValue = comboBox1.SelectedItem.ToString();                
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            this.Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
