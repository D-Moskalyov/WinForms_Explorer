using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2014._04._27_Explorer
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            long size = ((Form1)Owner).SizeToCopy;
            label4.Text = size.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
