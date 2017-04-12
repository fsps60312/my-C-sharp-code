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
using System.IO;

namespace Clipboard_Image_Transparenter
{
    unsafe static class Bitmap_Extensions
    {
        public static bool EqualsTo(this Bitmap a,Bitmap b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Size != b.Size) return false;
            BitmapData da = a.GetBitmapData();
            BitmapData db = b.GetBitmapData();
            byte* pa = da.GetPointer();
            byte* pb = db.GetPointer();
            bool ans = true;
            Parallel.For(0, da.Height, h =>
            {
                if (!ans) return;
                int ia = da.Stride * h;
                int ib = db.Stride * h;
                for (int w = 0; w < da.Width; w++)
                {
                    if (pa[ia] != pb[ib]) { ans = false; break; } ia++; ib++;
                    if (pa[ia] != pb[ib]) { ans = false; break; } ia++; ib++;
                    if (pa[ia] != pb[ib]) { ans = false; break; } ia++; ib++;
                    if (pa[ia] != pb[ib]) { ans = false; break; } ia++; ib++;
                }
            });
            a.UnlockBits(da);
            b.UnlockBits(db);
            return ans;
        }
        public static unsafe byte* GetPointer(this BitmapData data_bmp)
        {
            return (byte*)data_bmp.Scan0.ToPointer();
        }
        public static BitmapData GetBitmapData(this Bitmap bmp) { return bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb); }
    }
    public partial class Form1 : Form
    {
        PictureBox PBX = new PictureBox();
        int CNT = 0;
        public Form1()
        {
            InitializeComponent();
            this.Controls.Add(PBX);
            PBX.Dock = DockStyle.Fill;
            PBX.SizeMode = PictureBoxSizeMode.Zoom;
            this.Text = (CNT++).ToString();
            this.FormClosing += Form1_FormClosing;
            this.Shown += Form1_Shown;
        }
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        void Form1_Shown(object sender, EventArgs e)
        {
            Bitmap bmp = null;
            while (true)
            {
                IDataObject data = Clipboard.GetDataObject();
                if (data.GetFormats().Contains("PNG"))
                {
                    try
                    {
                        MemoryStream ms = (System.IO.MemoryStream)data.GetData("PNG");
                        Bitmap ans = (Bitmap)Bitmap.FromStream(ms, true);
                        if (!bmp.EqualsTo(ans))
                        {
                            bmp = new Bitmap(ans);
                            Make_TrueOrFalse(bmp);
                        }
                    }
                    catch (Exception) { }
                }
                Application.DoEvents();
            }
        }
        unsafe Bitmap Make_TrueOrFalse(Bitmap bmp)
        {
            BitmapData data_bmp = bmp.GetBitmapData();
            byte* ptr_bmp = data_bmp.GetPointer();
            byte cmp = 128;
            Parallel.For(0, data_bmp.Height, h =>
            {
                int i = data_bmp.Stride * h;
                for (int w = 0; w < data_bmp.Width; w++, i++)
                {
                    i += 3;
                    ptr_bmp[i] = ((ptr_bmp[i] >= cmp) ? byte.MaxValue : byte.MinValue);
                    //if (ptr_bmp[i] == byte.MinValue) MessageBox.Show("");
                }
            });
            bmp.UnlockBits(data_bmp);
            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Png);
            var data = new DataObject("PNG", stream);
            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);
            PBX.Image = new Bitmap(bmp);
            this.Text = (CNT++).ToString();
            return bmp;
        }
    }
}
