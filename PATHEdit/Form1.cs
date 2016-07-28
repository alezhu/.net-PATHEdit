using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PATHEdit
{
    public partial class Form1 : Form
    {
        private PathHolder pathUser;
        private PathHolder pathSystem;

        public Form1()
        {
            InitializeComponent();
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            Trace.TraceInformation("tab changed to:" + tabControl1.TabIndex);
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            Trace.TraceInformation("tab selected:" + e.TabPage.Name);
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            Trace.TraceInformation("tab enter:" + (sender as TabPage).Name);
            if (sender == tabPage1)
            {
                if (this.pathUser == null)
                {

                    this.pathUser = new PathHolder(PathHolder.PathType.User);
                    Refresh(checkedListBox1, this.pathUser);
                }

            }
            else if (sender == tabPage2)
            {
                if (this.pathSystem == null)
                {
                    this.pathSystem = new PathHolder(PathHolder.PathType.System);
                    Refresh(checkedListBox2, this.pathSystem);
                }
            }
        }

        private void checkedAll(CheckedListBox checkedListBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, true);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabIndex == 0)
            {
                Refresh(checkedListBox1, pathUser);
            }
            else
            {
                Refresh(checkedListBox2, pathSystem);
            }
        }

        private void Refresh(CheckedListBox checkedListBox, PathHolder pathHolder)
        {
            pathHolder.Reload();
            checkedListBox.Items.Clear();
            checkedListBox.Items.AddRange(pathHolder.Values.ToArray());
            checkedAll(checkedListBox);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Save(checkedListBox1, pathUser);
            }
            else
            {
                Save(checkedListBox2, pathSystem);
            }
        }

        private void Save(CheckedListBox checkedListBox, PathHolder pathHolder)
        {
            for (int i = checkedListBox.Items.Count - 1; i >= 0; i--)
            {
                if (!checkedListBox.GetItemChecked(i))
                {
                    pathHolder.Values.RemoveAt(i);
                }
            }
            pathHolder.Save();

            Refresh(checkedListBox, pathHolder);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            var fod = new OpenFileDialog();
            fod.CheckPathExists = true;
            fod.CheckFileExists = false;
            fod.Multiselect = false;
            fod.ValidateNames = false;
            fod.FileName = "Folder Selection";
            if (fod.ShowDialog(this) == DialogResult.OK)
            {
                var path = Path.GetDirectoryName(fod.FileName);
                if (tabControl1.SelectedIndex == 0)
                {
                    Add(path, checkedListBox1, pathUser);
                }
                else
                {
                    Add(path, checkedListBox2, pathSystem);
                }
            }


        }

        private void Add(string path, CheckedListBox checkedListBox, PathHolder pathHolder)
        {
            pathHolder.Values.Add(path);
            checkedListBox.Items.Add(path, true);
        }
    }
}
