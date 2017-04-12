using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
            label1.Text = "數值a";
            label2.Text = "數值b";
            button1.Text = "交換";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string a = textBox1.Text;
            textBox1.Text = textBox2.Text;
            textBox2.Text = a;
        }
    }
}
