using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        public Form1()
        {
            InitializeComponent();
            button1.Text = "Form";
            button2.Text = "Label";
            button3.Text = "TextBox";
            button4.Text = "Button-a";
            button5.Text = "Button-b";
            button6.Text = "Button-c";
            button7.Text = "Button-d";
            button8.Text = "輸出入對話框";
            button9.Text = "PictureBox";
            button10.Text = "Timer";
            button11.Text = "LineShape-a";
            button12.Text = "LineShape-b";
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            //MessageBox.Show("螢幕解析度為 " + screenWidth.ToString() + "*" + screenHeight.ToString());
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "3.基本輸入與輸出";
            //var c = GetColorAt(0,0);
            //this.BackColor = c;
            //MessageBox.Show(SystemInformation.FrameBorderSize.ToString());
            //MessageBox.Show(c.R.ToString() + " " + c.G.ToString() + " " + c.B.ToString());
            //MessageBox.Show(c.ToString());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 childform = new Form2 { };
            childform.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 childform = new Form3 { };
            childform.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form4 childform = new Form4 { };
            childform.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Form5 childform = new Form5 { };
            childform.Show();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Form6 childform = new Form6 { };
            childform.Show();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Form7 childform = new Form7 { };
            childform.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form8 childform = new Form8 { };
            childform.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form9 childform = new Form9 { };
            childform.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form10 childform = new Form10 { };
            childform.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form11 childform = new Form11 { };
            childform.Show();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            Bitmap screenPixel = new Bitmap(mybitmap.Width, mybitmap.Height, PixelFormat.Format32bppArgb);
            Graphics gdest = Graphics.FromImage(screenPixel);
            Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero);
            BitBlt(gdest.GetHdc(), 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, gsrc.GetHdc(), 696, 158, (int)CopyPixelOperation.SourceCopy);
            gdest.ReleaseHdc();
            gsrc.ReleaseHdc();
            pictureBox1.Image = screenPixel;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            if (screenPixel==mybitmap)
            {
                MessageBox.Show("same");
            }
            else
            {
                MessageBox.Show("differ");
                for(int i=0;i<100;i++)
                    for(int j=0;j<100;j++)
                    {
                        if(mybitmap.GetPixel(i,j)!=screenPixel.GetPixel(i,j))
                        {
                            MessageBox.Show(i.ToString() + " " + j.ToString());
                        }
                    }
                MessageBox.Show("same");
            }
            //pictureBox1.Left = int.Parse(textBox5.Text);
            //pictureBox1.Top = int.Parse(textBox6.Text);
            //MessageBox.Show(screenPixel.Width.ToString()+" "+screenPixel.Height.ToString());
        }
        public Bitmap mybitmap = new Bitmap("C:\\Users\\ney\\Pictures\\未命名.png");
        /*public Color GetColorAt(int X, int Y)
        {
            return screenPixel.GetPixel(0, 0);
        }*/
    }
}
