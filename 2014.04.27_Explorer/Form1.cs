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
    public partial class Form1 : Form
    {
        int index1 = 0;
        int index2 = 0;
        TreeNode tN1;
        TreeNode tN2;
        long sizeToCopy = 0;

        public long SizeToCopy
        {
            get { return sizeToCopy; }
            set { sizeToCopy = value; }
        }

        public Form1()
        {
            InitializeComponent();

            treeView1.Nodes.Add("Desktop");
            treeView1.Nodes[0].ImageIndex = 0;
            treeView1.Nodes.Add("My Computer");
            treeView1.Nodes[1].ImageIndex = 1;
            treeView1.Nodes[1].SelectedImageIndex = treeView1.Nodes[1].ImageIndex;

            treeView2.Nodes.Add("Desktop");
            treeView2.Nodes[0].ImageIndex = 0;
            treeView2.Nodes.Add("My Computer");
            treeView2.Nodes[1].ImageIndex = 1;
            treeView2.Nodes[1].SelectedImageIndex = treeView2.Nodes[1].ImageIndex;

            DirectoryInfo dI = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            List<FileSystemInfo> fSI = new List<FileSystemInfo>();
            fSI.AddRange(dI.GetFiles().ToList());
            fSI.AddRange(dI.GetDirectories().ToList());

            string[] disks = Environment.GetLogicalDrives();

            for (int j = 0; j < fSI.Count; j++)
            {
                //MessageBox.Show(fSI[j].Name);
				if(File.Exists(fSI[j].FullName))
				{
                    treeView1.Nodes[0].Nodes.Add(fSI[j].Name, fSI[j].Name, 4);
                    treeView1.Nodes[0].Nodes[j].SelectedImageIndex = treeView1.Nodes[0].Nodes[j].ImageIndex;
                    treeView2.Nodes[0].Nodes.Add(fSI[j].Name, fSI[j].Name, 4);
                    treeView2.Nodes[0].Nodes[j].SelectedImageIndex = treeView2.Nodes[0].Nodes[j].ImageIndex;
				}
				else
				{
                    treeView1.Nodes[0].Nodes.Add(fSI[j].Name, fSI[j].Name, 3);
                    treeView1.Nodes[0].Nodes[j].SelectedImageIndex = treeView1.Nodes[0].Nodes[j].ImageIndex;
                    treeView2.Nodes[0].Nodes.Add(fSI[j].Name, fSI[j].Name, 3);
                    treeView2.Nodes[0].Nodes[j].SelectedImageIndex = treeView2.Nodes[0].Nodes[j].ImageIndex;
				}
            }

            for (int i = 0; i < disks.Count(); i++)
            {
                treeView1.Nodes[1].Nodes.Add(disks[i], disks[i], 2);
                treeView1.Nodes[1].Nodes[i].SelectedImageIndex = treeView1.Nodes[1].Nodes[i].ImageIndex;
                treeView2.Nodes[1].Nodes.Add(disks[i], disks[i], 2);
                treeView2.Nodes[1].Nodes[i].SelectedImageIndex = treeView2.Nodes[1].Nodes[i].ImageIndex;
            }

        }

        void Expend(TreeNodeCollection tNCol, int lev)
        {
            lev--;
            for (int i = 0; i < tNCol.Count; i++)
            {
                tNCol[i].Expand();
                
                for (int j = 0; j < lev; j++)
                {
                    Expend(tNCol[i].Nodes, lev);
                }
            }
        }
		
		void DeleteDirectory(DirectoryInfo dI)
		{
			DirectoryInfo[] subDIArray = dI.GetDirectories().ToArray();
			foreach(DirectoryInfo subDI in subDIArray)
			{
				DeleteDirectory(subDI);
			}
			dI.Delete(true);
		}

        void GetSize(string path)
        {
            List<FileInfo> fI = new List<FileInfo>();
            List<DirectoryInfo> dI = new List<DirectoryInfo>();
            if (Directory.Exists(path))
            {
                DirectoryInfo dID = new DirectoryInfo(path);
                fI.AddRange(dID.GetFiles());
                foreach(FileInfo fIF in fI)
                {
                    sizeToCopy += fIF.Length;
                }
                dI.AddRange(dID.GetDirectories());
                foreach(DirectoryInfo dInfo in dI)
                {
                    GetSize(dInfo.FullName);
                }
            }
            else
            {
                FileInfo fInfo = new FileInfo(path);
                sizeToCopy += fInfo.Length;
            }
        }

        string GetPath(TreeNode tNode)
        {
            //MessageBox.Show(String.Format("{0}, {1}", tNode.Index, tNode.Name));
            string str = "";
            string pred = "";
            //MessageBox.Show(tNode.FullPath);
            if ((tNode.FullPath != "Desktop") && (tNode.FullPath != "My Computer"))
            //if(false)
            {
                int it = tNode.FullPath.IndexOf("\\");
                //MessageBox.Show(tNode.FullPath.ToString());
                char[] chArray = tNode.FullPath.ToArray();
                if (tNode.FullPath.First() == 'D')
                {
                    pred = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    //MessageBox.Show(pred);
                }
                //char[] chArray2 = new char[(chArray.Count())];
                //chArray.CopyTo(chArray2, it);
                //MessageBox.Show(chArray[0].ToString());
                IEnumerable<char> ch = chArray.Skip(it + 1);
                char[] chr = new char[(ch.Count())];
                for (int k = 0; k < ch.Count(); k++)
                {
                    chr[k] = ch.ElementAt(k);
                }
                str = new string(chr);
                //string str = new string(chArray);
                //MessageBox.Show(str);
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
            //MessageBox.Show(str);
            return str;
        }

        void CopyDirectory(string from, string to)
        {
            string pathFrom;
            string pathTo;
            //pathTo = string.Concat(to, "\\", from);
            DirectoryInfo dI = new DirectoryInfo(from);
            List<FileInfo> fIList = new List<FileInfo>();
            List<DirectoryInfo> dIList = new List<DirectoryInfo>();
            fIList.AddRange(dI.GetFiles());
            dIList.AddRange(dI.GetDirectories());
            foreach (FileInfo fIToCopy in fIList)
            {
                pathFrom = string.Concat(from, "\\", fIToCopy.Name);
                CopyFile(pathFrom, to);
            }
            foreach (DirectoryInfo dITocopy in dIList)
            {
                pathTo = string.Concat(to, "\\", dITocopy.Name);
                Directory.CreateDirectory(pathTo);
                pathFrom = string.Concat(from, "\\", dITocopy.Name);
                CopyDirectory(pathFrom, pathTo);
            }
        }

        void MoveDirectory(string from, string to)
        {
            //string pathFrom;
            //string pathTo;
            int ind = from.LastIndexOf("\\");
            string fromDir = from.Substring(ind);
            string toDir = string.Concat(to, "\\", fromDir);
            Directory.CreateDirectory(toDir);

            DirectoryInfo dI = new DirectoryInfo(from); 

            List<DirectoryInfo> dIList = new List<DirectoryInfo>();
            dIList.AddRange(dI.GetDirectories());
            foreach (DirectoryInfo dITocopy in dIList)
            {
                MoveDirectory(dITocopy.FullName, toDir);
            }

            List<FileInfo> fIList = new List<FileInfo>();
            fIList.AddRange(dI.GetFiles());
            foreach (FileInfo fIToCopy in fIList)
            {
                MoveFile(fIToCopy.FullName, toDir);
            }

            Directory.Delete(from);
            //int ind = pathOne.LastIndexOf("\\");
            //string pathOneDir = pathOne.Substring(ind);
            //string pathTwoDir = string.Concat(pathTwo, "\\", pathOneDir);
            //File.Move(pathOne, pathTwoDir);
            //Directory.Move(pathOne, pathTwoDir);
        }

        void CopyFile(string from, string to)
        {
            int ind = from.LastIndexOf("\\");
            string from1 = from.Substring(ind);
            string to1 = string.Concat(to, "\\", from1);
            File.Copy(from, to1);
        }

        void MoveFile(string from, string to)
        {
            //MessageBox.Show(from);
            //MessageBox.Show(to);
            int ind = from.LastIndexOf("\\");
            string fromFile = from.Substring(ind);
            string toFile = string.Concat(to, "\\", fromFile);
            File.Move(from, toFile);
        }

        void Filling(TreeNodeCollection tNodeCol)
        {

            if (tNodeCol[0].Parent.FullPath == "Desktop" || tNodeCol[0].Parent.FullPath == "My Computer" || Directory.Exists(GetPath(tNodeCol[0].Parent)))
            {
                int iter;
                int deviation;
                string path = GetPath(tNodeCol[0].Parent);
                if (path != "MyComp")
                {
                    DirectoryInfo dI = new DirectoryInfo(path);
                    
                    List<FileSystemInfo> fSI = new List<FileSystemInfo>();
                    fSI.AddRange(dI.GetFiles().ToList());
                    fSI.AddRange(dI.GetDirectories().ToList());
                    deviation = fSI.Count - tNodeCol.Count;
                    for (int i = 0; i < tNodeCol.Count; i++)
                    {
                        string str = GetPath(tNodeCol[i]);
                        if (!(Directory.Exists(str) || File.Exists(str)))
                        {
                            tNodeCol[i].Remove();
                            deviation++;
                            i--;
                        }
                    }

                    while (deviation != 0)
                    {
                        bool forAdd = true;
                        for (int i = 0; i < fSI.Count; i++)
                        {
                            for (iter = 0; iter < tNodeCol.Count; iter++)
                            {
                                //MessageBox.Show(tNodeCol[iter].Text);
                                if (tNodeCol[iter].Text == fSI[i].Name)
                                {
                                    forAdd = false;
                                    break;
                                }
                            }
                            if (forAdd)
                            {
                                if (File.Exists(fSI[i].FullName))   tNodeCol[0].Parent.Nodes.Add(fSI[i].Name, fSI[i].Name, 4, 4);
                                else                                tNodeCol[0].Parent.Nodes.Add(fSI[i].Name, fSI[i].Name, 3, 3);
                                deviation--;
                                if (deviation == 0) { break; }
                            }
                            //iter = 0;
                            forAdd = true;
                        }
                    }
                }
                else
                {
                    string[] disks = Environment.GetLogicalDrives();
                    deviation = disks.Count() - tNodeCol.Count;
                    //MessageBox.Show(deviation.ToString());
                    for (int i = 0; i < tNodeCol.Count; i++)
                    {
                        string str = GetPath(tNodeCol[i]);
                        if (!(Directory.Exists(str) || File.Exists(str)))
                        //if (disks[i] == str)
                        {
                            bool yes = false;
                            foreach (string st in disks)
                            {
                                if (st == str) yes = true;
                            }
                            if (!yes)
                            {
                                //MessageBox.Show("Ky");
                                tNodeCol[i].Remove();
                                deviation++;
                                i--;
                            }
                        }
                    }
                    while (deviation != 0)
                    {
                        //MessageBox.Show("Ky-Ky");
                        bool forAdd = true;
                        for (int i = 0; i < disks.Count(); i++)
                        {
                            for (iter = 0; iter < tNodeCol.Count; iter++)
                            {
                                if (tNodeCol[iter].Text == disks[i])
                                {
                                    forAdd = false;
                                    break;
                                }
                            }
                            if (forAdd)
                            {
                                tNodeCol[0].Parent.Nodes.Add(disks[i], disks[i], 2, 2);
                                deviation--;
                                if (deviation == 0) { break; }
                            }
                            forAdd = true;
                        }
                    }
                }
                for (int i = 0; i < tNodeCol.Count; i++)
                {
                    if (tNodeCol[i].FirstNode == null)
                    {
                        string str = GetPath(tNodeCol[i]);
                        //MessageBox.Show(str);
                        DirectoryInfo dI2 = new DirectoryInfo(str);
                        if (dI2.Exists)
                        {
                            try
                            {
                                List<FileSystemInfo> fSI2 = new List<FileSystemInfo>();
                                fSI2.AddRange(dI2.GetFiles().ToList());
                                fSI2.AddRange(dI2.GetDirectories().ToList());

                                for (int j = 0; j < fSI2.Count; j++)
                                {
                                    if (File.Exists(fSI2[j].FullName))  tNodeCol[i].Nodes.Add(fSI2[j].Name, fSI2[j].Name, 4, 4);
                                    else                                tNodeCol[i].Nodes.Add(fSI2[j].Name, fSI2[j].Name, 3, 3);
                                }
                            }
                            catch (Exception e) { }
                        }
                    }
                    //MessageBox.Show(dI.FullName.ToString()); 
                }
                
            }
            else
            {
                Filling(tNodeCol[0].Parent.Parent.Nodes);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNodeCollection tNC;
            //if (e.Node.Parent != null)
            //{
            //    //MessageBox.Show("Ky");
            //    tNC = e.Node.Parent.Nodes;
            //    Filling(tNC);
            //}
            tNC = e.Node.Nodes;
            Filling(tNC);
        }

        private void treeView2_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNodeCollection tNC;
            //if (e.Node.Parent != null)
            //{
            //    tNC = e.Node.Parent.Nodes;
            //    Filling(tNC);
            //}
            tNC = e.Node.Nodes;
            Filling(tNC);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string str = GetPath(treeView1.SelectedNode);
            //string pred = "";
            TreeNode tN = treeView1.SelectedNode.Parent;
            TreeNodeCollection toReDraw = treeView1.SelectedNode.Parent.Nodes;
            if (File.Exists(str))
            {
                FileInfo fI = new FileInfo(str);
                fI.Delete();
            }
            else if (Directory.Exists(str))
            {
                DirectoryInfo dI = new DirectoryInfo(str);
				DeleteDirectory(dI);
                //dI.Delete(true);
            }
            //MessageBox.Show(toReDraw[0].ToString());
            //Filling(toReDraw);
            tN.Collapse();
            tN.Expand();
        }//delete

        private void button8_Click(object sender, EventArgs e)
        {
            string str = GetPath(treeView2.SelectedNode);
            TreeNode tN = treeView2.SelectedNode.Parent;
            TreeNodeCollection toReDraw = treeView2.SelectedNode.Parent.Nodes;

            if (File.Exists(str))
            {
                FileInfo fI = new FileInfo(str);
                fI.Delete();
            }
            else if (Directory.Exists(str))
            {
                DirectoryInfo dI = new DirectoryInfo(str);
				DeleteDirectory(dI);
            }
            //Filling(toReDraw);
            tN.Collapse();
            tN.Expand();
        }//delete

        private void button10_Click(object sender, EventArgs e)//collapse all
        {
            treeView1.CollapseAll();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int pos = treeView1.SelectedNode.Level;
            TreeNodeCollection tNCol = treeView1.Nodes;
            Expend(tNCol, pos);
        }//expend

        private void button12_Click(object sender, EventArgs e)//collapse all
        {
            treeView2.CollapseAll();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (treeView2.SelectedNode != null)
            {
                int pos = treeView2.SelectedNode.Level;
                TreeNodeCollection tNCol = treeView2.Nodes;
                Expend(tNCol, pos);
            }
        }//expend

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 info = new Form2();
            info.Show(this);
            //info.label
        }//info

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 info = new Form2();
            info.Show(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<TreeNode> toRedraw = new List<TreeNode>(); 

            if (treeView1.SelectedNode.Text == "My Computer" || treeView2.SelectedNode.Text == "My Computer" || (treeView1.SelectedNode.Level == 1 && treeView1.SelectedNode.Parent.Text == "My Computer") )
            {
                MessageBox.Show("Uncorrect path");
            }
            else
            {
                string pathOne = GetPath(treeView1.SelectedNode);
                string pathTwo = GetPath(treeView2.SelectedNode);

                if (!(Directory.Exists(pathOne) || File.Exists(pathOne)))
                {
                    MessageBox.Show("Uncorrect path");
                }
                else 
                {
                    if (pathTwo.IndexOf(pathOne) == 0)
                    {
                        MessageBox.Show("!Dir in subdir!");
                    }
                    else
                    {
                        if (!(Directory.Exists(pathTwo) || File.Exists(pathTwo)))
                        {
                            MessageBox.Show("Appointment not exist");
                        }
                        else
                        {
                            if (File.Exists(GetPath(TreeView2.SelectedNode)))
                            {
                                toRedraw.Add(treeView2.SelectedNode.Parent);
                                if (treeView2.SelectedNode.Parent.Parent != null)
                                {
                                    toRedraw.Add(treeView2.SelectedNode.Parent.Parent);
                                }
                            }
                            else
                            {
                                toRedraw.Add(treeView2.SelectedNode);
                                if (treeView2.SelectedNode.Parent != null)
                                {
                                    toRedraw.Add(treeView2.SelectedNode.Parent);
                                }
                            }

                            if (File.Exists(GetPath(TreeView1.SelectedNode)))
                            {
                                toRedraw.Add(treeView1.SelectedNode.Parent);
                                if (treeView1.SelectedNode.Parent.Parent != null)
                                {
                                    toRedraw.Add(treeView1.SelectedNode.Parent.Parent);
                                }
                            }
                            else
                            {
                                toRedraw.Add(treeView1.SelectedNode);
                                if (treeView1.SelectedNode.Parent != null)
                                {
                                    toRedraw.Add(treeView1.SelectedNode.Parent);
                                }
                            }

                            GetSize(pathOne);
                            //Form3 progressBar = new Form3();
                            //progressBar.Show(this);
                            if (File.Exists(pathTwo))
                            {
                                pathTwo = GetPath(treeView2.SelectedNode.Parent);
                            }
                            if (File.Exists(pathOne))
                            {
                                CopyFile(pathOne, pathTwo);
                            }
                            else
                            {
                                int ind = pathOne.LastIndexOf("\\");
                                string pathOneDir = pathOne.Substring(ind);
                                pathTwo = string.Concat(pathTwo, "\\", pathOneDir);
                                Directory.CreateDirectory(pathTwo);
                                CopyDirectory(pathOne, pathTwo);
                            }
                        }
                        foreach (TreeNode tN in toRedraw)
                        {
                            tN.Collapse();
                            tN.Expand();
                        }
                    }
                }
            }
        }//copy

        private void button3_Click(object sender, EventArgs e)
        {
            List<TreeNode> toRedraw = new List<TreeNode>(); 

            if (treeView1.SelectedNode.Text == "My Computer" || treeView2.SelectedNode.Text == "My Computer" || (treeView2.SelectedNode.Level == 1 && treeView2.SelectedNode.Parent.Text == "My Computer") )
            {
                MessageBox.Show("Uncorrect path");
            }
            else
            {
                string pathOne = GetPath(treeView2.SelectedNode);
                string pathTwo = GetPath(treeView1.SelectedNode);

                if (!(Directory.Exists(pathOne) || File.Exists(pathOne)))
                {
                    MessageBox.Show("Uncorrect path");
                }
                else 
                {
                    if (pathTwo.IndexOf(pathOne) == 0)
                    {
                        MessageBox.Show("!Dir in subdir!");
                    }
                    else
                    {
                        if (!(Directory.Exists(pathTwo) || File.Exists(pathTwo)))
                        {
                            MessageBox.Show("Appointment not exist");
                        }
						else
						{
							if (File.Exists(GetPath(TreeView2.SelectedNode)))
							{
								toRedraw.Add(treeView2.SelectedNode.Parent);
								if(treeView2.SelectedNode.Parent.Parent != null)
								{
									toRedraw.Add(treeView2.SelectedNode.Parent.Parent);
								}
							}
							else
							{
								toRedraw.Add(treeView2.SelectedNode);
								if(treeView2.SelectedNode.Parent != null)
								{
									toRedraw.Add(treeView2.SelectedNode.Parent);
								}
							}
							
							if (File.Exists(GetPath(TreeView1.SelectedNode)))
							{
								toRedraw.Add(treeView1.SelectedNode.Parent);
								if(treeView1.SelectedNode.Parent.Parent != null)
								{
									toRedraw.Add(treeView1.SelectedNode.Parent.Parent);
								}
							}
							else
							{
								toRedraw.Add(treeView1.SelectedNode);
								if(treeView1.SelectedNode.Parent != null)
								{
									toRedraw.Add(treeView1.SelectedNode.Parent);
								}
							}
							
							GetSize(pathOne);
							//Form3 progressBar = new Form3();
							//progressBar.Show(this);
							if (File.Exists(pathTwo))
							{
								pathTwo = GetPath(treeView1.SelectedNode.Parent);
							}
							if (File.Exists(pathOne))
							{
								CopyFile(pathOne, pathTwo);
							}
							else
							{
								int ind = pathOne.LastIndexOf("\\");
								string pathOneDir = pathOne.Substring(ind);
								pathTwo = string.Concat(pathTwo, "\\", pathOneDir);
								Directory.CreateDirectory(pathTwo);
								CopyDirectory(pathOne, pathTwo);
							}
						}
						foreach (TreeNode tN in toRedraw)
						{
							tN.Collapse();
							tN.Expand();
						}
					}
                }
            }
        }//copy

        private void button2_Click(object sender, EventArgs e)
        {
            List<TreeNode> toRedraw = new List<TreeNode>();

            if (treeView1.SelectedNode.Text == "My Computer" || treeView2.SelectedNode.Text == "My Computer" || (treeView1.SelectedNode.Level == 1 && treeView1.SelectedNode.Parent.Text == "My Computer") )
            {
                MessageBox.Show("Uncorrect path");
            }
            else
            {
                string pathOne = GetPath(treeView1.SelectedNode);
                string pathTwo = GetPath(treeView2.SelectedNode);

                if (!(Directory.Exists(pathOne) || File.Exists(pathOne)))
                {
                    MessageBox.Show("Uncorrect path");
                }
                else 
                {
                    if (pathTwo.IndexOf(pathOne) == 0)
                    {
                        MessageBox.Show("!Dir in subdir!");
                    }
                    else
                    {
                        if (!(Directory.Exists(pathTwo) || File.Exists(pathTwo)))
                        {
                            MessageBox.Show("Appointment not exist");
                        }
						else
						{
							if (File.Exists(GetPath(TreeView2.SelectedNode)))
							{
								toRedraw.Add(treeView2.SelectedNode.Parent);
								if(treeView2.SelectedNode.Parent.Parent != null)
								{
									toRedraw.Add(treeView2.SelectedNode.Parent.Parent);
								}
							}
							else
							{
								toRedraw.Add(treeView2.SelectedNode);
								if(treeView2.SelectedNode.Parent != null)
								{
									toRedraw.Add(treeView2.SelectedNode.Parent);
								}
							}
							
							if (File.Exists(GetPath(TreeView1.SelectedNode)))
							{
								toRedraw.Add(treeView1.SelectedNode.Parent);
								if(treeView1.SelectedNode.Parent.Parent != null)
								{
									toRedraw.Add(treeView1.SelectedNode.Parent.Parent);
								}
							}
							else
							{
								toRedraw.Add(treeView1.SelectedNode);
								if(treeView1.SelectedNode.Parent != null)
								{
									toRedraw.Add(treeView1.SelectedNode.Parent);
								}
							}
							
							GetSize(pathOne);
							//Form3 progressBar = new Form3();
							//progressBar.Show(this);
							if (File.Exists(pathTwo))
							{
								pathTwo = GetPath(treeView2.SelectedNode.Parent);
							}
							if (File.Exists(pathOne))
							{
								MoveFile(pathOne, pathTwo);
							}
							else
							{
								MoveDirectory(pathOne, pathTwo);
								//int ind = pathOne.LastIndexOf("\\");
								//string pathOneDir = pathOne.Substring(ind);
								//string pathTwoDir = string.Concat(pathTwo, "\\", pathOneDir);
								//File.Move(pathOne, pathTwoDir);
								//Directory.Move(pathOne, pathTwoDir);
								//int ind = pathOne.LastIndexOf("\\");
								//string pathOneDir = pathOne.Substring(ind);
								//pathTwo = string.Concat(pathTwo, "\\", pathOneDir);
								//Directory.CreateDirectory(pathTwo);
								//CopyDirectory(pathOne, pathTwo);
							}
							foreach (TreeNode tN in toRedraw)
							{
								tN.Collapse();
								tN.Expand();
							}
						}
                    }
                }
            }
        }//move

        private void button4_Click(object sender, EventArgs e)
        {
            List<TreeNode> toRedraw = new List<TreeNode>();

            if (treeView1.SelectedNode.Text == "My Computer" || treeView2.SelectedNode.Text == "My Computer" || (treeView2.SelectedNode.Level == 1 && treeView2.SelectedNode.Parent.Text == "My Computer") )
            {
                MessageBox.Show("Uncorrect path");
            }
            else
            {
                string pathOne = GetPath(treeView2.SelectedNode);
                string pathTwo = GetPath(treeView1.SelectedNode);

                if (!(Directory.Exists(pathOne) || File.Exists(pathOne)))
                {
                    MessageBox.Show("Uncorrect path");
                }
                else 
                {
                    if (pathTwo.IndexOf(pathOne) == 0)
                    {
                        MessageBox.Show("!Dir in subdir!");
                    }
                    else
                    {
                        if (!(Directory.Exists(pathTwo) || File.Exists(pathTwo)))
                        {
                            MessageBox.Show("Appointment not exist");
                        }
						else
						{
						
							if (File.Exists(GetPath(TreeView2.SelectedNode)))
							{
								toRedraw.Add(treeView2.SelectedNode.Parent);
								if(treeView2.SelectedNode.Parent.Parent != null)
								{
									toRedraw.Add(treeView2.SelectedNode.Parent.Parent);
								}
							}
							else
							{
								toRedraw.Add(treeView2.SelectedNode);
								if(treeView2.SelectedNode.Parent != null)
								{
									toRedraw.Add(treeView2.SelectedNode.Parent);
								}
							}
							
							if (File.Exists(GetPath(TreeView1.SelectedNode)))
							{
								toRedraw.Add(treeView1.SelectedNode.Parent);
								if(treeView1.SelectedNode.Parent.Parent != null)
								{
									toRedraw.Add(treeView1.SelectedNode.Parent.Parent);
								}
							}
							else
							{
								toRedraw.Add(treeView1.SelectedNode);
								if(treeView1.SelectedNode.Parent != null)
								{
									toRedraw.Add(treeView1.SelectedNode.Parent);
								}
							}
							
							GetSize(pathOne);
							//Form3 progressBar = new Form3();
							//progressBar.Show(this);
							if (File.Exists(pathTwo))
							{
								pathTwo = GetPath(treeView1.SelectedNode.Parent);
							}
							if (File.Exists(pathOne))
							{
								MoveFile(pathOne, pathTwo);
							}
							else
							{
								MoveDirectory(pathOne, pathTwo);
								//int ind = pathOne.LastIndexOf("\\");
								//string pathOneDir = pathOne.Substring(ind);
								//string pathTwoDir = string.Concat(pathTwo, "\\", pathOneDir);
								//File.Move(pathOne, pathTwoDir);
								//Directory.Move(pathOne, pathTwoDir);
								//int ind = pathOne.LastIndexOf("\\");
								//string pathOneDir = pathOne.Substring(ind);
								//pathTwo = string.Concat(pathTwo, "\\", pathOneDir);
								//Directory.CreateDirectory(pathTwo);
								//CopyDirectory(pathOne, pathTwo);
							}
							foreach (TreeNode tN in toRedraw)
							{
								tN.Collapse();
								tN.Expand();
							}
						}
                    }
                }
            }
        }

        private void treeView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.LShiftKey)
            {
                e.IsInputKey = false;
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("ShiftDown");
            if (e.KeyCode == Keys.LShiftKey)
            {
                MessageBox.Show("ShiftDown");
                tN1 = new TreeNode();
                tN1 = treeView1.SelectedNode;
                while (tN1.Parent != null)
                {
                    while (tN1.PrevNode != null)
                    {
                        tN1 = tN1.PrevNode;
                        index1 = tN1.GetNodeCount(true) + 1;
                    }
                    index1++;
                    tN1 = tN1.Parent;
                }
                while (tN1.PrevNode != null)
                {
                    tN1 = tN1.PrevNode;
                    index1 = tN1.GetNodeCount(true) + 1;
                }
            }
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LShiftKey)
            {
                MessageBox.Show("ShiftUp");
                tN2 = new TreeNode();
                tN2 = treeView1.SelectedNode;
                while (tN2.Parent != null)
                {
                    while (tN2.PrevNode != null)
                    {
                        tN2 = tN2.PrevNode;
                        index1 = tN2.GetNodeCount(true) + 1;
                    }
                    index1++;
                    tN2 = tN2.Parent;
                }
                while (tN2.PrevNode != null)
                {
                    tN2 = tN2.PrevNode;
                    index1 = tN2.GetNodeCount(true) + 1;
                }
            }
            //MessageBox.Show(tN2.Text);
        }
    }
}