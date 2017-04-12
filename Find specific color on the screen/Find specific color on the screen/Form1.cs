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

namespace Find_specific_color_on_the_screen
{
    public partial class Form1 : Form
    {
        Color targetColor = Color.FromArgb(255, 244, 180);
        List<Point> points = new List<Point>();
        bool[][] vis
        private void updatePoints()
        {
            using (Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
                    IntPtr dc1 = g.GetHdc();
                    g.ReleaseHdc(dc1);
                }
                BitmapData bd = bmp.LockBits(new Rectangle(new Point(0, 0), bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                Parallel.For(0, bd.Height, (int y) =>
                {
                    unsafe
                    {
                        byte* p = (byte*)bd.Scan0.ToPointer();
                        for(int x=0;x<bd.Width;x++,p++)
                        {
                            bool fit = true;
                            if(*p++!=)
                        }
                    }
                });
                bmp.UnlockBits(bd);
            }
        }
        public Form1()
        {

        }
    }
}
