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
    public partial class Form7 : Form
    {
        int n = 0;
        public Form7()
        {
            InitializeComponent();
            button1.Text = "加一";
            button2.Text = "減一";
            button3.Text = "加十";
            button4.Text = "減十";
            label1.Text = "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            n++;
            label1.Text = n.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            n--;
            label1.Text = n.ToString();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            n+=10;
            label1.Text = n.ToString();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            n -= 10;
            label1.Text = n.ToString();
        }
    }
}
