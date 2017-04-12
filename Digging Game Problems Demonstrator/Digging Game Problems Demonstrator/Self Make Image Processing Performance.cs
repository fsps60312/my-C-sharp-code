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
using System.Threading;
using System.Drawing.Imaging;

namespace Digging_Game_Problems_Demonstrator
{
    public partial class Self_Make_Image_Processing_Performance : Form
    {
        PictureBox P1 = new PictureBox();
        PictureBox P2 = new PictureBox();
        TableLayoutPanel TLP = new TableLayoutPanel();
        ProgressBar B1 = new ProgressBar();
        ProgressBar B2 = new ProgressBar();
        bool Running = false;
        public Self_Make_Image_Processing_Performance()
        {
            Self_Make_Image_Processing_Performance.CheckForIllegalCrossThreadCalls = false;
            this.KeyPreview = true;
            InitializeComponent();//"Record_2015_08_01_21_09_10_435.mp4"
            this.Size = new Size(583, 440);
            this.Controls.Add(TLP);
            {
                TLP.Dock = DockStyle.Fill;
                TLP.RowCount = 4;
                TLP.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                TLP.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                TLP.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
                TLP.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
                TLP.Controls.Add(P1); TLP.SetCellPosition(P1, new TableLayoutPanelCellPosition(0, 0));
                {
                    P1.Dock = DockStyle.Fill;
                    P1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                TLP.Controls.Add(P2); TLP.SetCellPosition(P2, new TableLayoutPanelCellPosition(0, 1));
                {
                    P2.Dock = DockStyle.Fill;
                    P2.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                TLP.Controls.Add(B1); TLP.SetCellPosition(B1, new TableLayoutPanelCellPosition(0, 2));
                {
                    B1.Dock = DockStyle.Fill;
                    B1.Value = 0;
                    B1.Maximum = 1000;
                }
                TLP.Controls.Add(B2); TLP.SetCellPosition(B2, new TableLayoutPanelCellPosition(0, 3));
                {
                    B2.Dock = DockStyle.Fill;
                    B2.Value = 0;
                    B2.Maximum = 1000;
                }
            }
            this.Click += Self_Make_Image_Processing_Performance_Click;
            P1.Click += Self_Make_Image_Processing_Performance_Click;
            P2.Click += Self_Make_Image_Processing_Performance_Click;
            B1.Click += Self_Make_Image_Processing_Performance_Click;
            B2.Click += Self_Make_Image_Processing_Performance_Click;
        }
        void ChangeColor1(ref Bitmap b, Color c)
        {
            for (int i = 0; i < b.Height; i++)
            {
                for (int j = 0; j < b.Width; j++)
                {
                    b.SetPixel(j, i, c);
                }
            }
        }
        unsafe void ChangeColor2(ref Bitmap b, Color c)
        {
            BitmapData data = b.GetBitmapData();
            byte* ptr = data.GetPointer();
            int idx = 0;
            for (int i = 0; i < data.Height; i++)
            {
                for (int j = 0; j < data.Width; j++, idx++)
                {
                    ptr[idx++] = c.B;
                    ptr[idx++] = c.G;
                    ptr[idx++] = c.R;
                }
                idx += data.Stride - 4 * data.Width;
            }
            b.UnlockBits(data);
        }
        void Self_Make_Image_Processing_Performance_Click(object sender, EventArgs e)
        {
            if (Running) return;
            Running = true;
            Bitmap b1 = BITMAP.New(1000, 1000, Color.FromArgb(0, 0, 0));
            Bitmap b2 = BITMAP.New(1000, 1000, Color.FromArgb(0, 0, 0));
            int v1 = 0, v2 = 0;
            this.Text = v1.ToString() + " : " + v2.ToString();
            bool r1 = true, r2 = true;
            DateTime start=DateTime.Now;
            Thread t1 = new Thread(() =>
            {
                B1.Value = 0;
                for (int i = 0; r2 && i < B1.Maximum; i++)
                {
                    B1.Value++;
                    v1++;
                    this.Text = v1.ToString() + " : " + v2.ToString();
                    if (i % 2 == 0) ChangeColor1(ref b1, Color.FromArgb(255, 255, 255));
                    else ChangeColor1(ref b1, Color.FromArgb(0, 0, 0));
                    P1.Image = b1;
                }
                bool tr = r2;
                if (!tr) Running = false;
                r1 = false;
                if (!tr) MessageBox.Show("Time Used: " + (DateTime.Now - start).TotalSeconds.ToString("F3") + " seconds");
            });
            Thread t2 = new Thread(() =>
            {
                B2.Value=0;
                for (int i = 0; r1 && i < B2.Maximum; i++)
                {
                    B2.Value++;
                    v2++;
                    this.Text = v1.ToString() + " : " + v2.ToString();
                    if (i % 2 == 0) ChangeColor2(ref b2, Color.FromArgb(255, 255, 255));
                    else ChangeColor2(ref b2, Color.FromArgb(0, 0, 0));
                    P2.Image = b2;
                }
                bool tr = r1;
                if (!tr) Running = false;
                r2 = false;
                if (!tr) MessageBox.Show("Time Used: " + (DateTime.Now - start).TotalSeconds.ToString("F3") + " seconds");
            });
            t1.IsBackground = true;
            t2.IsBackground = true;
            t1.Start();
            t2.Start();
        }
    }
}
