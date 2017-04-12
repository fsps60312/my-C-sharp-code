using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;

namespace Digging_Game_Problems_Demonstrator
{
    partial class PictureBox_Performance_Stability : Form
    {
        PictureBox PBX = new PictureBox();
        bool AttemptRun = false;
        //bool Running = false;
        bool ToRight = true;
        int EXC = 500;
        int DIS = 1;
        static string PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)+"\\";
        static Bitmap P1=BITMAP.FromFile(PATH + "科學班成果發表專題報告-Pictures\\1.png");
        public PictureBox_Performance_Stability()
        {
            this.Size = new Size(583, 440);
            this.Controls.Add(PBX);
            {
                PBX.Image = P1;
                //PBX.BackColor = Color.FromArgb(128, 0, 0, 0);
                PBX.SizeMode = PictureBoxSizeMode.StretchImage;
                PBX.Size = new Size(300, 200);
                PBX.Location = new Point(-EXC, this.Height / 2 - P1.Height);
            }
            PictureBox pbx = new PictureBox();
            this.Controls.Add(pbx);
            {
                pbx.Image = BITMAP.New(this.Width, this.Height / 2, Color.FromArgb(185, 122, 87));
                pbx.SizeMode = PictureBoxSizeMode.AutoSize;
                pbx.Location = new Point(0, this.Height / 2);
            }
            this.Click += PictureBox_Performance_Stability_Click;
        }
        void PictureBox_Performance_Stability_Click(object sender, EventArgs e)
        {
            this.Text = "Running";
            AttemptRun = true;
            ToRight = true;
            PBX.Left = -EXC;
            while (AttemptRun)
            {
                //Application.DoEvents();
                this.Update();
                if (ToRight)
                {
                    PBX.Left += DIS;
                    if (PBX.Left + PBX.Width - this.Width > EXC) ToRight = false;
                }
                else
                {
                    PBX.Left -= DIS;
                    if (PBX.Left < -EXC)
                    {
                        ToRight = true;
                        break;
                    }
                }
            }
            this.Text = "Finished";
        }
    }
}
