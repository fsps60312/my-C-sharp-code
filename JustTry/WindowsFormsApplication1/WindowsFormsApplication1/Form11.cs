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
    public partial class Form11 : Form
    {
        double t=0;
        bool threadexecute=false;
        public Form11()
        {
            InitializeComponent();
            Form11.CheckForIllegalCrossThreadCalls = false;
            label1.Visible = false;
            button1.Text = "開始計時";
            button2.Text = "開始撞牆";
            pictureBox1.Visible = false;
            pictureBox1.Image = Properties.Resources.DSC00770;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Width = 0;
            pictureBox1.Height = 0;
            Thread mythread=new Thread(()=>{thethread();});
            mythread.IsBackground = true;
            mythread.Start();
        }
        public void thethread()
        {
            pictureBox1.Visible = true;
            for(int i=0;i<100;i++)
            {
                pictureBox1.Width ++;
                pictureBox1.Height ++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled)
            {
                timer1.Enabled = false;
                label1.Text = t.ToString()+" 秒";
                t = 0;
                label1.Visible = true;
                button1.Text = "開始計時";
                return;
            }
            else
            {
                timer1.Interval = 1;
                timer1.Enabled = true;
                button1.Text = "過多久了?";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            t+=0.001;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(threadexecute==false)
            {
                thread = new Thread(picture);
                thread.IsBackground = true;
                thread.Start();
                button2.Text = "別跑了!!!";
                threadexecute = true;
            }
            else
            {
                thread.Abort();
                button2.Text = "開始撞牆";
                threadexecute = false;
            }
        }
        public void picture()
        {
            while(true)
            {
                while (pictureBox1.Width + pictureBox1.Left != this.Size.Width)
                {
                    pictureBox1.Left++;
                }
                while (pictureBox1.Left!=0)
                {
                    pictureBox1.Left--;
                }
            }
        }
        public Thread thread;

    }
}
