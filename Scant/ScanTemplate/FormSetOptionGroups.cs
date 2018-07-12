using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScanTemplate
{
    public partial class FormSetOptionGroups : Form
    {
        public FormSetOptionGroups(ARTemplate.SingleChoiceAreas singleChoiceAreas)
        {
            InitializeComponent();
            this._singleChoiceAreas = singleChoiceAreas;
            _Ogs = new ScOptionGroups();

            _BeginIndex = 0;
            _EndIndex = singleChoiceAreas.Count-1;
            _indexlist = new List<int>();
            _NameList = new List<string>();
            for (int i = 0; i < singleChoiceAreas.Count; i++ )
                _indexlist.Add(i);
            foreach (int i in _indexlist)
                _NameList.Add("x"+(i + 1));

            InitBeginTextBox();
            InitComboBox();
        }
        public void InitBeginTextBox()
        {
           if(ValidIndex())
                textBoxBeginID.Text = _NameList[_BeginIndex];
        }
        public bool ValidIndex()
        {
            return _BeginIndex > -1 && _BeginIndex < _NameList.Count && _BeginIndex <= _EndIndex;
        }
        public void InitComboBox()
        {
            comboBoxEndID.Items.Clear();
            for (int i = _BeginIndex; i < _NameList.Count; i++)
                comboBoxEndID.Items.Add(_NameList[i]);
            if(comboBoxEndID.Items.Count>0)
                comboBoxEndID.SelectedIndex = 0;
        }
        private void buttonAddToOptionGroups_Click(object sender, EventArgs e)
        {
            if (!ValidIndex())
            {
                MessageBox.Show("选择题已全部加入题组");
                return;
            }
            string groupname = textBoxName.Text;
            if(groupname=="")
            {
                MessageBox.Show("请先输入题组名称");
                return;
            }
            if (_Ogs.HasItemName(groupname))
            {
                MessageBox.Show("题组名称重复，请先更改题组名称");
                return;
            }
            _EndIndex = comboBoxEndID.SelectedIndex+_BeginIndex;
            ScOptionGroup O = new ScOptionGroup(groupname);
            _Ogs.list.Add(O);
            List<int> indexlist = _indexlist.Skip(_BeginIndex).Take(_EndIndex - _BeginIndex + 1).ToList();
            O.AddIndexList(indexlist);
            listBoxOptiongroups.Items.Add(O);
            _BeginIndex = _EndIndex = _EndIndex + 1;
            InitBeginTextBox();
            InitComboBox();
        }
        private void buttonSetOK_Click(object sender, EventArgs e)
        {
            if (ValidIndex())
            {
                MessageBox.Show("还有选择题没有编入题组，无法提交,如果要放弃本次设置，点击取消");
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close(); 
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Cancel");
        }
        private void listBoxOptiongroups_KeyUp(object sender, KeyEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedIndex == -1)
                return;
            if (e.KeyCode == Keys.Delete)
            {
                if (lb.SelectedIndex != lb.Items.Count - 1)
                {
                    MessageBox.Show("只能从最后一个题组开始删除");
                    return;
                }
                ScOptionGroup O = _Ogs.list[lb.SelectedIndex];
                _Ogs.list.Remove(O);
                lb.Items.RemoveAt(lb.SelectedIndex);
                if (lb.Items.Count > 0)
                    lb.SelectedIndex = lb.Items.Count - 1;
                _BeginIndex = _EndIndex = O.BeginIndex();
                InitBeginTextBox();
                InitComboBox();
            }
        }
        private ARTemplate.SingleChoiceAreas _singleChoiceAreas;      
        private int _BeginIndex;
        private int _EndIndex;
        private List<int> _indexlist;
        private List<string> _NameList;
        private ScOptionGroups _Ogs;
        public List<ScOptionGroup> OGlist()
        { return  _Ogs.list;}

    }

    public class ScOptionGroups
    {
        public ScOptionGroups()
        {
            list = new List<ScOptionGroup>();
        }
        public bool HasItemName(string groupname)
        {
            if (list == null)
                return false;
            return list.Exists(r => r.Name == groupname);
        }
        public List<ScOptionGroup> list;
    }
    public class ScOptionGroup
    {
        public ScOptionGroup(string name)
        {
            this.Name = name.Trim();
            _indexlist = new List<int>();
        }       
        public void AddIndexList(List<int> indexlist)
        {
            _indexlist = indexlist;
        }
        public override string ToString()
        {
            return Name+ "[" + (_indexlist[0]+1) + "-"+(_indexlist[ _indexlist.Count-1]+1)+"]";
        }
        private List<int> _indexlist;
        public string Name { get; set; }

        public int BeginIndex()
        {
            if (_indexlist.Count > 0)
                return _indexlist[0];
            return -1;
        }

        public List<int> IndexList { get { return _indexlist; } }
    }
}
