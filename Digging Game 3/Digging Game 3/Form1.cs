using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Digging_Game_3
{
    public partial class Form1 : Form
    {
        void Do(Control ctrl,Action action)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(action);
            else action.Invoke();
        }
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            Thread thread = new Thread(() =>
            {
                for (int i = 0,cnt=0; ; i ^= 1)
                {
                    Bitmap bmp = new Bitmap(20000, 2000);
                    BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    unsafe
                    {
                        byte* bmp_ptr = (byte*)bmp_data.Scan0.ToPointer();
                        for(int j=0;j<bmp_data.Height;j++)
                        {
                            for(int k=0;k<bmp_data.Width;k++)
                            {
                                bmp_ptr[k * 4] = (byte)(i * 255);
                                bmp_ptr[k * 4 + 1] = (byte)(i * 255);
                                bmp_ptr[k * 4 + 2] = (byte)(i * 255);
                                bmp_ptr[k * 4+3] =  255;
                            }
                            bmp_ptr += bmp_data.Stride;
                        }
                    }
                    bmp.UnlockBits(bmp_data);
                    Do(this, new Action(() => { if (this.BackgroundImage != null)this.BackgroundImage.Dispose(); this.BackgroundImage = bmp; this.Text = (cnt++).ToString(); }));
                    //Thread.Sleep(300);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
