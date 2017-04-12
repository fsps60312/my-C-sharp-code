using System;
using System.Collections.Generic;
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
using System.Threading;
using System.Runtime.InteropServices;

namespace IPSC2016_D_Hard
{
    public partial class Form1 : Form
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        TableLayoutPanel TLP_MAIN = new TableLayoutPanel();
        TableLayoutPanel TLP_BUTN = new TableLayoutPanel();
        TextBox TXB = new TextBox();
        Button BTN_SAVE = new Button();
        Button BTN_RUNN = new Button();
        Button BTN_COPY = new Button();
        bool RECORDING = false;
        public Form1()
        {
            {
                TLP_MAIN.Dock = DockStyle.Fill;
                TLP_MAIN.RowCount = 2;
                TLP_MAIN.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
                TLP_MAIN.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                {
                    TLP_BUTN.Dock = DockStyle.Fill;
                    TLP_BUTN.AutoSize = true;
                    TLP_BUTN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    TLP_BUTN.ColumnCount = 5;
                    for (int i = 0; i < TLP_BUTN.ColumnCount; i++) TLP_BUTN.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                    int id = 0;
                    {
                        BTN_SAVE.Dock = DockStyle.Fill;
                        BTN_SAVE.Text = "Save";
                        BTN_SAVE.Click += BTN_SAVE_Click;
                        BTN_SAVE.AutoSize = true;
                        BTN_SAVE.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    }
                    TLP_BUTN.Controls.Add(BTN_SAVE); TLP_BUTN.SetCellPosition(BTN_SAVE, new TableLayoutPanelCellPosition(id++, 0));
                    {
                        BTN_RUNN.Dock = DockStyle.Fill;
                        BTN_RUNN.Text = "Run";
                        BTN_RUNN.Click += BTN_RUNN_Click;
                        BTN_RUNN.AutoSize = true;
                        BTN_RUNN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    }
                    TLP_BUTN.Controls.Add(BTN_RUNN); TLP_BUTN.SetCellPosition(BTN_RUNN, new TableLayoutPanelCellPosition(id++, 0));
                    {
                        BTN_COPY.Dock = DockStyle.Fill;
                        BTN_COPY.Text = "Copy";
                        BTN_COPY.Click += BTN_COPY_Click;
                        BTN_COPY.AutoSize = true;
                        BTN_COPY.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    }
                    TLP_BUTN.Controls.Add(BTN_COPY); TLP_BUTN.SetCellPosition(BTN_COPY, new TableLayoutPanelCellPosition(id++, 0));
                }
                TLP_MAIN.Controls.Add(TLP_BUTN); TLP_MAIN.SetCellPosition(TLP_BUTN, new TableLayoutPanelCellPosition(0, 0));
                {
                    TXB.Dock = DockStyle.Fill;
                    TXB.Multiline = true;
                    TXB.ScrollBars = ScrollBars.Both;
                }
                TLP_MAIN.Controls.Add(TXB); TLP_MAIN.SetCellPosition(TXB, new TableLayoutPanelCellPosition(0, 1));
            }
            this.Controls.Add(TLP_MAIN);
            P.Add(new Point(258, 376));
            P.Add(new Point(354, 376));
            P.Add(new Point(459, 376));
            P.Add(new Point(648, 376));
            P.Add(new Point(258, 480));
            P.Add(new Point(354, 480));
            P.Add(new Point(459, 480));
            P.Add(new Point(648, 480));
            P.Add(new Point(258, 572));
            P.Add(new Point(354, 572));
            P.Add(new Point(459, 572));
            P.Add(new Point(648, 572));
            for (int i = 0; i < P.Count; i++) LAST_COLOR.Add(Color.FromArgb(0, 0, 0));
            this.Shown += Form1_Shown;
            this.FormClosed += Form1_FormClosed;
        }

        void Form1_Shown(object sender, System.EventArgs e)
        {
            TheLoop();
        }

        void BTN_COPY_Click(object sender, System.EventArgs e)
        {
            Clipboard.SetText(ANSWER.ToString());
            MessageBox.Show("Text copied");
        }

        void BTN_RUNN_Click(object sender, System.EventArgs e)
        {
            RECORDING = true;
            for (int i = 0; i < P.Count; i++) LAST_COLOR[i] = GetColor(P[i]);
        }

        void BTN_SAVE_Click(object sender, System.EventArgs e)
        {
            Point p = this.Location;
            P.Add(p);
            LAST_COLOR.Add(Color.FromArgb(0, 0, 0));
            AppendText(p.ToString() + "\r\n");
        }
        List<Point> P = new System.Collections.Generic.List<Point>();
        List<Color> LAST_COLOR = new System.Collections.Generic.List<Color>();
        StringBuilder ANSWER = new StringBuilder();
        void AppendText(string s)
        {
            ANSWER.Append(s);
        }
        public void TheLoop()
        {
            Color color;
            DateTime changedtime = DateTime.Now.AddDays(-1.0);
            //DateTime updatedtime = DateTime.Now.AddDays(-1.0);
            while (true)
            {
                Point p = this.Location;
                color = GetColor(p);
                this.Text = "X: " + p.X + " Y: " + p.Y
                +", (" + color.A.ToString()
                +"," + color.R.ToString()
                +"," + color.G.ToString()
                +"," + color.B.ToString()+")";
                if(RECORDING)
                {
                    this.Text = "Rerording "+ANSWER.Length.ToString();
                    bool changed=false;
                    for (int i = 0; i < P.Count; i++)
                    {
                        Color c=GetColor(P[i]);
                        if (c != LAST_COLOR[i]) { changed = true; }
                        LAST_COLOR[i] = c;
                    }
                    if(changed)
                    {
                        if ((DateTime.Now - changedtime).TotalMilliseconds > 500.0)
                        {
                            for (int i = 0; i < P.Count; i++)
                            {
                                Color c = LAST_COLOR[i];
                                AppendText(c.R.ToString().PadLeft(3, '0'));
                                AppendText(c.G.ToString().PadLeft(3, '0'));
                                AppendText(c.B.ToString().PadLeft(3, '0'));
                            }
                            AppendText("\r\n");
                        }
                        changedtime = DateTime.Now;
                    }
                }
                Application.DoEvents();
            }
        }
        public Color GetColor(Point point)
        {
        index: ;
            try
            {
                Bitmap b = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
                Graphics gdest = Graphics.FromImage(b);
                Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero);
                BitBlt(
                    gdest.GetHdc()
                    , 0
                    , 0
                    , 1
                    , 1
                    , gsrc.GetHdc()
                    , point.X
                    , point.Y
                    , (int)CopyPixelOperation.SourceCopy
                    );
                gdest.ReleaseHdc();
                gsrc.ReleaseHdc();
                Color c=b.GetPixel(0, 0);
                gdest.Dispose();
                gsrc.Dispose();
                b.Dispose();
                return c;
            }
            catch(Exception error)
            {
                TXB.AppendText(DateTime.Now.ToString()+"\r\n"+ error.ToString()+"\r\n");
                Application.DoEvents();
                Thread.Sleep(50);
                Application.DoEvents();
                Thread.Sleep(50);
                Application.DoEvents();
                goto index;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
