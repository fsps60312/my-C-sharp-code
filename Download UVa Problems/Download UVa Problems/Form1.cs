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
using System.Net;
using System.Diagnostics;
using System.IO;

namespace Download_UVa_Problems
{
    public partial class Form1 : Form
    {
        public TextBox folder = new TextBox();
        public FolderBrowserDialog chosefolder = new FolderBrowserDialog();
        public Button Open = new Button();
        public Button Start = new Button();
        public TextBox message1 = new TextBox();
        public TextBox message2 = new TextBox();
        public TableLayoutPanel tlp = new TableLayoutPanel();
        public Form1()
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.Controls.Add(tlp);
            {
                tlp.Dock = DockStyle.Fill;
                tlp.ColumnCount = 2;
                tlp.RowCount = 1;
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                tlp.Controls.Add(message1); tlp.SetCellPosition(message1, new TableLayoutPanelCellPosition(0, 0));
                {
                    message1.Dock = DockStyle.Fill;
                    message1.Multiline = true;
                    message1.ScrollBars = ScrollBars.Both;
                    message1.DoubleClick += message_DoubleClick;
                }
                tlp.Controls.Add(message2); tlp.SetCellPosition(message2, new TableLayoutPanelCellPosition(1, 0));
                {
                    message2.Dock = DockStyle.Fill;
                    message2.Multiline = true;
                    message2.ScrollBars = ScrollBars.Both;
                    message2.DoubleClick += message_DoubleClick;
                }
            }
            this.Controls.Add(Open);
            {
                Open.Dock = DockStyle.Left;
                Open.Text = "Open";
                Open.Click += Open_Click;
            }
            this.Controls.Add(Start);
            {
                Start.Dock = DockStyle.Left;
                Start.Text = "Start";
                Start.Click += Start_Click;
            }
            this.Controls.Add(folder);
            folder.Dock = DockStyle.Top;
            folder.Text = @"C:\Users\Burney\Documents\Online Judges\Uva Online Judge\Problems";
            this.FormClosing += Form1_FormClosing;
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        void message_DoubleClick(object sender, EventArgs e)
        {
            message1.Clear();
        }
        public string ProblemUrl(int a)
        {
            return "https://uva.onlinejudge.org/external/" + (a / 100).ToString() + "/" + a.ToString() + ".pdf";
        }
        public string convertsize(long a)
        {
            double b = (double)a;
            int i;
            for (i = 0; i <= 4; i++)
            {
                if (b < 1024) break;
                b /= 1024;
            }
            string ans = b.ToString("F3");
            switch (i)
            {
                case 0: return ans + "B";
                case 1: return ans + "KB";
                case 2: return ans + "MB";
                case 3: return ans + "GB";
                default: return ans + "TB";
            }
        }
        public void DownloadProblem(int i)
        {
            string filename = "Q" + i.ToString() + ".pdf";
            this.Text = "Downloading " + filename;
            WebClient web = new WebClient();
            try
            {
                FileInfo a = new FileInfo(folder.Text + @"\" + filename);
                if (a.Exists && a.Length > 0) return;
                web.DownloadFile(ProblemUrl(i), a.FullName);
                a.Refresh();
                if (a.Length == 0) throw new Exception();
                message2.AppendText(filename + "\t" + convertsize(a.Length)+"\r\n");
            }
            catch (Exception)
            {
                filename = "Q" + i.ToString() + ".pdf";
                this.Text = "Retrying " + filename;
                try
                {
                    FileInfo a = new FileInfo(folder.Text + @"\" + filename);
                    if (a.Exists && a.Length > 0) return;
                    web.DownloadFile(ProblemUrl(i), a.FullName);
                    a.Refresh();
                    if (a.Length == 0) throw new Exception();
                    message2.AppendText("succeed downloading " + filename + "\t" + convertsize(a.Length) + "\r\n");
                }
                catch (Exception)
                {
                    FileInfo a = new FileInfo(folder.Text + @"\" + filename);
                    if (a.Exists) { message1.AppendText("file length="+a.Length.ToString()+"\t"); a.Delete(); }
                    message1.AppendText(a.Name+"\r\n");
                }
            }
        }
        void Start_Click(object sender, EventArgs e)
        {
            Thread thr = new Thread(() =>
            {
                Start.Enabled = false;
                for (int i = 100; i <= 1721; i++) DownloadProblem(i);
                for (int i = 10000; i <= 12928; i++) DownloadProblem(i);
                    Start.Enabled = true;
            });
            thr.IsBackground = true;
            thr.Start();
        }

        void Open_Click(object sender, EventArgs e)
        {
            chosefolder.ShowNewFolderButton = true;
            chosefolder.ShowDialog();
            folder.Text = chosefolder.SelectedPath;
        }
    }
}
