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
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Digging_Game_Problems_Demonstrator
{
    partial class BitmapBox:Form
    {
        PictureBox PBX = new PictureBox();
        public BitmapBox(Bitmap bmp)
        {
            MaximizeBox = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(PBX);
            {
                PBX.Dock = DockStyle.Fill;
                PBX.SizeMode = PictureBoxSizeMode.AutoSize;
                PBX.Image = bmp;
            }
        }
        public static void Show(Bitmap bmp, string text = null)
        {
            BitmapBox box = new BitmapBox(bmp);
            DateTime now = DateTime.Now;
            box.Show();
            while (!box.IsDisposed) Application.DoEvents();
            PublicVariables.ProcessTime += DateTime.Now - now;
        }
    }
}
