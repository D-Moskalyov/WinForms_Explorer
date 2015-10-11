using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace _2014._04._27_Explorer
{
    public partial class Form2 : Form
    {
        string path;
        string creation;
        string access;
        string write;
        DirectoryInfo dI;
        FileInfo fI;
        
        public Form2()
        {
            InitializeComponent();
        }

        string GetPath(TreeNode tNode)
        {
            string str = "";
            string pred = "";
            if ((tNode.FullPath != "Desktop") && (tNode.FullPath != "My Computer"))
            {
                int it = tNode.FullPath.IndexOf("\\");
                char[] chArray = tNode.FullPath.ToArray();
                if (tNode.FullPath.First() == 'D')
                {
                    pred = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                }
                IEnumerable<char> ch = chArray.Skip(it + 1);
                char[] chr = new char[(ch.Count())];
                for (int k = 0; k < ch.Count(); k++)
                {
                    chr[k] = ch.ElementAt(k);
                }
                str = new string(chr);
                if (pred != "")
                {
                    str = string.Concat(pred, "\\", str);
                }
            }
            else
            {
                if (tNode.FullPath == "Desktop")
                {
                    str = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                }
                else
                {
                    str = "MyComp";
                }
            }
            return str;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (((Button)(((Form1)Owner).ActiveControl)).Name == "button5")
            {
                path = GetPath(((Form1)Owner).TreeView1.SelectedNode);
            }
            else
            {
                path = GetPath(((Form1)Owner).TreeView2.SelectedNode);
            }

            if (path == "MyComp")
            {
                label2.Text = "My Computer";
                label4.Text = "My Computer";
                label6.Text = "My Computer";
                label8.Text = "My Computer";
            }
            else if (Directory.Exists(path))
            {
                dI = new DirectoryInfo(path);
                label2.Text = dI.FullName;
                creation = string.Format("{0} {1}", dI.CreationTime.ToShortDateString(), dI.CreationTime.ToShortTimeString());
                label4.Text = creation;
                access = string.Format("{0} {1}", dI.LastAccessTime.ToShortDateString(), dI.LastAccessTime.ToShortTimeString());
                label6.Text = access;
                write = string.Format("{0} {1}", dI.LastWriteTime.ToShortDateString(), dI.LastWriteTime.ToShortTimeString());
                label8.Text = write;
            }
            else if (File.Exists(path))
            {
                fI = new FileInfo(path);
                label2.Text = fI.FullName;
                creation = string.Format("{0} {1}", fI.CreationTime.ToShortDateString(), fI.CreationTime.ToShortTimeString());
                label4.Text = creation;
                access = string.Format("{0} {1}", fI.LastAccessTime.ToShortDateString(), fI.LastAccessTime.ToShortTimeString());
                label6.Text = access;
                write = string.Format("{0} {1}", fI.LastWriteTime.ToShortDateString(), fI.LastWriteTime.ToShortTimeString());
                label8.Text = write;
            }
            else
            {
                MessageBox.Show("Object not exist");
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

    }
}
