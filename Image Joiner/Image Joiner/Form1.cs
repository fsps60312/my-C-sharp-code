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
using System.Drawing.Imaging;

namespace Image_Joiner
{
    public partial class Form1 : Form
    {
        string FILENAME = @"C:\Users\ney\Pictures\鑽礦遊戲程式碼\Image";
        List<Bitmap> BMP = new List<Bitmap>();
        PictureBox PBX = new PictureBox();
        TableLayoutPanel TLP_M = new TableLayoutPanel();
        TableLayoutPanel TLP_T = new TableLayoutPanel();
        TextBox[] TXB = new TextBox[7];
        Button BTN = new Button();
        int BMP_IDX = 0;
        Point P1, P2;
        int COLUMNS = 1;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1000, 750);
            this.Shown += Form1_Shown;
            this.Controls.Add(TLP_M);
            {
                TLP_M.Dock = DockStyle.Fill;
                TLP_M.ColumnCount = 2;
                TLP_M.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 1));
                TLP_M.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                TLP_M.Controls.Add(PBX); TLP_M.SetCellPosition(PBX, new TableLayoutPanelCellPosition(1, 0));
                {
                    PBX.Dock = DockStyle.Fill;
                    PBX.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                TLP_M.Controls.Add(TLP_T); TLP_M.SetCellPosition(TLP_T, new TableLayoutPanelCellPosition(0, 0));
                {
                    TLP_T.Dock = DockStyle.Fill;
                    TLP_T.AutoSize = true;
                    TLP_T.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    TLP_T.RowCount = TXB.Length + 1;
                    TLP_T.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                    TLP_T.Controls.Add(BTN); TLP_T.SetCellPosition(BTN, new TableLayoutPanelCellPosition(0, 0));
                    BTN.Dock = DockStyle.Fill;
                    BTN.Text = "Start";
                    BTN.Click += BTN_Click;
                    for (int i = 0; i < TXB.Length; i++)
                    {
                        TLP_T.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                        TXB[i] = new TextBox();
                        TLP_T.Controls.Add(TXB[i]); TLP_T.SetCellPosition(TXB[i], new TableLayoutPanelCellPosition(0, i + 1));
                        TXB[i].Dock = DockStyle.Fill;
                        TXB[i].Name = i.ToString();
                        TXB[i].TextChanged += Form1_TextChanged;
                    }
                }
            }
        }
        void BTN_Click(object sender, EventArgs e)
        {
            if (PBX.Image != null) PBX.Image.Dispose();
            PBX.Image = Image_Merge2();
            PBX.Image.Save("Result.bmp", ImageFormat.Bmp);
            this.Text = "Saved";
        }
        Bitmap bmp, ans;
        Bitmap Image_Merge2()
        {
            bmp = Image_Merge1();
            if(COLUMNS==1)return bmp;
            int W = bmp.Width;
            int H = bmp.Height;
            int h = bmp.Height / COLUMNS; if (bmp.Height % COLUMNS > 0) h++;
            ans = new Bitmap(W * COLUMNS, h);
            BitmapData data = ans.Get_BitmapData();
            for (int i = 0; i < COLUMNS - 1; i++)
            {
                Bitmap bbp=SubBitmap(bmp, new Rectangle(0, i * h, W, h));
                Regular(data,bbp , new Point(i * W, 0));
                bbp.Dispose();
                this.Text = String.Format("Processing2...{0}/{1}", i + 1, COLUMNS - 1);
            }
            Regular(data, SubBitmap(bmp, new Rectangle(0, h * (COLUMNS - 1), W, H - h * (COLUMNS - 1))), new Point(W * (COLUMNS - 1), 0));
            ans.UnlockBits(data);
            bmp.Dispose();
            this.Text = "Complete2";
            return ans;
        }
        Bitmap Image_Merge1()
        {
            int cnt = BMP.Count;
            Rectangle r = new Rectangle(P1, new Size(P2.X - P1.X + 1, P2.Y - P1.Y + 1));
            Bitmap ans = new Bitmap(r.Width, r.Height * cnt);
            BitmapData data = ans.Get_BitmapData();
            for (int i = 0; i < cnt; i++)
            {
                Bitmap bbp=SubBitmap(BMP[i], r);
                Regular(data, bbp, new Point(0, i * r.Height));
                bbp.Dispose();
                this.Text = String.Format("Processing1...{0}/{1}", i + 1, cnt);
            }
            ans.UnlockBits(data);
            this.Text = "Complete1";
            return ans;
        }
        void Show_Image()
        {
            if (PBX.Image != null) PBX.Image.Dispose();
            Bitmap bmp = new Bitmap(BMP[BMP_IDX]);
            PBX.Image = SubBitmap(bmp, new Rectangle(P1, new Size(P2.X - P1.X + 1, P2.Y - P1.Y + 1)));
        }
        void Form1_TextChanged(object sender, EventArgs e)
        {
            TextBox txb = sender as TextBox;
            switch (txb.Name)
            {
                case "0": P1.X = SetValue(txb.Text, 0); break;
                case "1": P2.X = SetValue(txb.Text, BMP[BMP_IDX].Width - 1); break;
                case "2": P1.Y = SetValue(txb.Text, 0); break;
                case "3": P2.Y = SetValue(txb.Text, BMP[BMP_IDX].Height - 1); break;
                case "4": BMP_IDX = SetValue(txb.Text, 0); break;
                case "5": COLUMNS = SetValue(txb.Text, 1); return;
                default: throw new Exception("No more than 5 TextBoxes");
            }
            Show_Image();
        }
        void Form1_Shown(object sender, EventArgs e)
        {
            string filename = FILENAME + "0.bmp";
            for (int i = 0; new FileInfo(filename).Exists; )
            {
                BMP.Add((Bitmap)Bitmap.FromFile(filename));
                this.Text = filename;
                i++;
                filename = FILENAME + i.ToString() + ".bmp";
            }
            P1 = new Point(0, 0);
            P2 = new Point(BMP[0].Width - 1, BMP[0].Height - 1);
            Show_Image();
        }
        int SetValue(string s, int def)
        {
            int i;
            if (!int.TryParse(s, out i)) return def;
            return i;
        }
        unsafe Bitmap SubBitmap(Bitmap bmp, Rectangle r)
        {
            if (r.X < 0 || r.Y < 0 || r.X + r.Width > bmp.Width || r.Y + r.Height > bmp.Height || r.Width <= 0 || r.Height <= 0) return null;
            BitmapData data_bmp = bmp.Get_BitmapData(r);
            Bitmap ans = new Bitmap(r.Width, r.Height);
            BitmapData data_ans = ans.Get_BitmapData();
            byte* ptr_bmp = data_bmp.Get_Pointer();
            byte* ptr_ans = data_ans.Get_Pointer();
            Parallel.For(0, r.Height, h =>
            {
                int i1 = data_ans.Stride * h;
                int i2 = data_bmp.Stride * h;
                for (int w = 0; w < r.Width; w++)
                {
                    ptr_ans[i1++] = ptr_bmp[i2++];
                    ptr_ans[i1++] = ptr_bmp[i2++];
                    ptr_ans[i1++] = ptr_bmp[i2++];
                    ptr_ans[i1++] = ptr_bmp[i2++];
                }
            });
            ans.UnlockBits(data_ans);
            bmp.UnlockBits(data_bmp);
            return ans;
        }
        unsafe public static void Regular(BitmapData data_bac, BitmapData data_bmp, Point p, Rectangle region = default(Rectangle))
        {
            if (region == default(Rectangle)) region = new Rectangle(0, 0, data_bac.Width, data_bac.Height);
            byte* ptr_bac = data_bac.Get_Pointer();
            byte* ptr_bmp = data_bmp.Get_Pointer();
            int w1 = Math.Max(Math.Max(-p.X, 0), region.X - p.X);
            int w2 = Math.Min(Math.Min(data_bmp.Width, data_bac.Width - p.X), region.X + region.Width - p.X);
            int h1 = Math.Max(Math.Max(-p.Y, 0), region.Y - p.Y);
            int h2 = Math.Min(Math.Min(data_bmp.Height, data_bac.Height - p.Y), region.Y + region.Height - p.Y);
            if (w1 >= w2 || h1 >= h2) return;
            ptr_bac += p.Y * data_bac.Stride + 4 * (w1 + p.X);
            ptr_bmp += 4 * w1;
            Parallel.For(h1, h2, h =>
            {
                int i1 = data_bac.Stride * h;
                int i2 = data_bmp.Stride * h;
                for (int w = w1; w < w2; w++)
                {
                    ptr_bac[i1++] = ptr_bmp[i2++];
                    ptr_bac[i1++] = ptr_bmp[i2++];
                    ptr_bac[i1++] = ptr_bmp[i2++];
                    ptr_bac[i1++] = ptr_bmp[i2++];
                }
            });
        }
        unsafe public static void Regular(BitmapData data_bac, Bitmap bmp, Point p, Rectangle region = default(Rectangle))
        {
            BitmapData data_bmp = bmp.Get_BitmapData();
            Regular(data_bac, data_bmp, p, region);
            bmp.UnlockBits(data_bmp);
        }
    }
    unsafe static class Bitmap_Extensions
    {
        public static BitmapData Get_BitmapData(this Bitmap bmp) { return bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb); }
        public static BitmapData Get_BitmapData(this Bitmap bmp, Rectangle r) { return bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb); }
    }
    unsafe static class BitmapData_Extensions
    {
        public static unsafe byte* Get_Pointer(this BitmapData data_bmp)
        {
            return (byte*)data_bmp.Scan0.ToPointer();
        }
    }
}
