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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            label1.Text = "x^2+";
            label2.Text = "x+";
            label3.Text = "=0";
            label4.Text = "";
            label5.Text = "";
        }
        private void textbox_Changed(object sender,EventArgs e)
        {
            double a, b, c;
            try
            {
                a = double.Parse(textBox1.Text);
                b = double.Parse(textBox2.Text);
                c = double.Parse(textBox3.Text);
            }
            catch(FormatException)
            {
                return;
            }
            if(Math.Pow(b,2)-4*a*c>0)
            {
                label4.Text = "x1 = "+((-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (a * 2)).ToString();
                label5.Text = "x2 = " + ((-b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (a * 2)).ToString();
            }
            else if (Math.Pow(b, 2) - 4 * a * c == 0)
            {
                label4.Text = "x = " + ((-b) / (a * 2)).ToString();
                label5.Text = "";
            }
            else if (Math.Pow(b, 2) - 4 * a * c < 0)
            {
                label4.Text = "無實數解";
                label5.Text = "";
            }
        }
    }
}
