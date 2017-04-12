using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form10 : Form
    {
        public Thread t;
        public Form10()
        {
            InitializeComponent();
            Form.CheckForIllegalCrossThreadCalls = false;
            pictureBox1.Image = Properties.Resources.DSC00770;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            button1.Text = "←";
            button2.Text = "↓";
            button3.Text = "→";
            button4.Text = "↓\n↑";
            button5.Text = "←→";
            button6.Text = "↑";
            button7.Text = "→←";
            button8.Text = "↑\n↓";
        }
        public int buttonnumber(object buttontext)
        {
            string a = (buttontext as Button).Text;
            if(a=="←") return 0;
            else if(a=="↓") return 1;
            else if(a=="→") return 2;
            else if (a == "↓\n↑") return 3;
            else if(a=="←→") return 4;
            else if(a=="↑") return 5;
            else if(a=="→←") return 6;
            else if (a == "↑\n↓") return 7;
            else
            {
                MessageBox.Show("buttonnumber input error");
                return -1;
            }
        }
        public void movepicture(int a)
        {
            switch (a)
            {
                case 0: while (true) pictureBox1.Left--;
                case 1: while (true) pictureBox1.Top++;
                case 2: while (true) pictureBox1.Left++;
                case 3: while (true) pictureBox1.Height--;
                case 4: while (true) pictureBox1.Width++;
                case 5: while (true) pictureBox1.Top--;
                case 6: while (true) pictureBox1.Width--;
                case 7: while (true) pictureBox1.Height++;
                default: this.Close(); break;
            }
        }
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            t = new Thread(() => { movepicture(buttonnumber(sender)); });
            t.IsBackground = true;
            t.Start();
        }
        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            t.Abort();
        }
    }
}
