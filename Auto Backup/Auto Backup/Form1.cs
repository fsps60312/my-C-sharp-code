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
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace Auto_Backup
{
    public partial class Form1 : Form
    {
        public TextBox Txb1 = new TextBox();
        public TextBox Txb2 = new TextBox();
        public Button Btn1 = new Button();
        public TableLayoutPanel Tlp1 = new TableLayoutPanel();
        public TableLayoutPanel Tlp2 = new TableLayoutPanel();
        public TableLayoutPanel Tlp3 = new TableLayoutPanel();
        public TableLayoutPanel Tlp4 = new TableLayoutPanel();
        public Panel pan = new Panel();
        public CheckBox[] chbox = new CheckBox[5];
        public CheckBox copynewfile = new CheckBox();
        public long totalsize = 0;
        public long movedsize = 0;
        public Form1()
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Controls.Add(Tlp1);
            Tlp1.Dock = DockStyle.Fill;
            Tlp1.RowCount = 2;
            Tlp1.ColumnCount = 1;
            Tlp1.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            Tlp1.RowStyles.Add(new RowStyle(SizeType.AutoSize, 50));
            Tlp1.Controls.Add(Txb1); Tlp1.SetCellPosition(Txb1, new TableLayoutPanelCellPosition(0, 0));
            {
                Txb1.Dock = DockStyle.Fill;
                Txb1.Multiline = true;
                Txb1.ScrollBars = ScrollBars.Both;
                Txb1.Font = new Font("新細明體", 10);
                Txb1.WordWrap = false;
            }
            Tlp1.Controls.Add(Tlp4); Tlp1.SetCellPosition(Tlp4, new TableLayoutPanelCellPosition(0, 1));
            {
                Tlp4.Dock = DockStyle.Fill;
                Tlp4.ColumnCount = 2;
                Tlp4.RowCount = 1;
                Tlp4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4));
                Tlp4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                Tlp4.Controls.Add(Tlp2); Tlp4.SetCellPosition(Tlp2, new TableLayoutPanelCellPosition(0, 0));
                {
                    Tlp2.Dock = DockStyle.Fill;
                    Tlp2.RowCount =2;
                    Tlp2.ColumnCount = 1;//document, video, picture, desktop, music
                    Tlp2.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                    Tlp2.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
                    Tlp2.Controls.Add(pan); Tlp2.SetCellPosition(pan, new TableLayoutPanelCellPosition(0, 0));
                    {
                        pan.Dock = DockStyle.Fill;
                        pan.AutoSize = true;
                        pan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        string[] chboxstring = { "Desktop", "Documents", "Pictures", "Videos", "Music" };
                        for (int i = 0; i < 5; i++)
                        {
                            chbox[i] = new CheckBox();
                            pan.Controls.Add(chbox[i]);
                            {
                                chbox[i].Dock = DockStyle.Left;
                                chbox[i].BringToFront();
                                chbox[i].Checked = false;
                                chbox[i].Text = chboxstring[i];
                                chbox[i].AutoSize = true;
                            }
                        }
                        pan.Controls.Add(copynewfile);
                        {
                            copynewfile.Dock = DockStyle.Left;
                            copynewfile.BringToFront();
                            copynewfile.Checked = false;
                            copynewfile.Text = "Copy when no file exists";
                            copynewfile.AutoSize = true;
                        }
                    }
                    Tlp2.Controls.Add(Txb2); Tlp2.SetCellPosition(Txb2, new TableLayoutPanelCellPosition(0, 1));
                    {
                        Txb2.Dock = DockStyle.Fill;
                        Txb2.Text = "";
                    }
                }
                Tlp4.Controls.Add(Btn1); Tlp4.SetCellPosition(Btn1, new TableLayoutPanelCellPosition(1, 0));
                {
                    Btn1.Dock = DockStyle.Fill;
                    Btn1.AutoSize = true;
                    Btn1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    Btn1.Text = "Start";
                    Btn1.Click += Btn1_Click;
                }
            }
            LoadSettings();
            this.FormClosing+=Form1_FormClosing;
            chboxcolor[0] = copynewfile.BackColor;
            chboxcolor[1] = copynewfile.ForeColor;
        }
        void LoadSettings()
        {
            if (!SettingInfo.Exists)
            {
                SaveSettings();
            }
            StreamReader a = new StreamReader(SettingInfo.FullName, Encoding.UTF8);
            string[] b = a.ReadToEnd().Split('\n');
            for (int i = 0; i < b.Length; i++) b[i] = b[i].Trim(' ').Trim('\r');
            a.Close();
            for(int i=0;i<b[0].Length&&i<chbox.Length;i++)
            {
                if (b[0][i] == '1') chbox[i].Checked = true;
                else chbox[i].Checked = false;
            }
            if (b[1][0] == '1') copynewfile.Checked = true;
            else copynewfile.Checked = false;
            Txb2.Text = b[2];
            a.Close();
        }
        void SaveSettings()
        {
            StreamWriter b = new StreamWriter(SettingInfo.FullName,false,Encoding.UTF8);
            for(int i=0;i<chbox.Length;i++)
            {
                if (chbox[i].Checked) b.Write("1");
                else b.Write("0");
            }
            b.WriteLine();
            if (copynewfile.Checked) b.WriteLine("1");
            else b.WriteLine("0");
            b.WriteLine(Txb2.Text);
            b.Close();
        }
        public FileInfo SettingInfo = new FileInfo("Auto Backup Settings.ini");
        void Btn1_Click(object sender, EventArgs e)
        {
            if (thread != null) thread.Abort();
            thread = new Thread(() =>
            {
                checkchange();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            Process.GetCurrentProcess().Kill();
        }
        public HashSet<DriveInfo> driveinfo = new HashSet<DriveInfo>();
        public string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public string newestdir = null;
        public Thread thread = null;
        public Dictionary<FileInfo, HashSet<FileInfo>> failed = new Dictionary<FileInfo,HashSet< FileInfo>>();
        public int successed = 0;
        public Color[] chboxcolor = new Color[] { Color.FromArgb(100, 100, 255), Color.FromArgb(0, 0, 0), Color.FromArgb(100, 100, 255), Color.FromArgb(0, 0, 0) };
        public void restore(Exception e, FileInfo b,FileInfo d)
        {
            //if (d.Exists) d.Delete();
            if (!failed.ContainsKey(b)) failed[b] = new HashSet<FileInfo>();
            failed[b].Add(d);
            Txb1.AppendText(DateTime.Now.ToString() + "\tFailed: " + b.FullName.Remove(3) + " -> " + d.FullName.Remove(3) + "\t" + b.FullName.Substring(path.Length) + "\r\n"/*\t" +
                e.ToString()+"\r\n"*/);
        }
        public void restore(Exception e,DirectoryInfo a)
        {
            Txb1.AppendText(DateTime.Now.ToString() + "\tFailed: " + a.FullName+"\r\n"/*\t" + e.ToString()+"\r\n"*/);
        }
        public void dfs(DirectoryInfo a)
        {
            if (a.Attributes.ToString().IndexOf("System")!=-1)
            {
                Txb1.AppendText(DateTime.Now.ToString() + "\tSkipped: " + a.FullName + "\r\n");
                return;
            }
            this.Text = a.FullName;
            try
            {
                FileInfo[] b = a.GetFiles();
            }
            catch(Exception e)
            {
                restore(e, a);
                return;
            }
            foreach(FileInfo b in a.GetFiles())
            {
                totalsize += b.Length;
                foreach(DriveInfo c in driveinfo)//search each drive
                {
                    DirectoryInfo f=new DirectoryInfo(c.Name+a.FullName.Substring(3));
                    if (!f.Exists) f.Create();
                    if (c.Name != a.FullName.Remove(3))
                    {
                        FileInfo d = new FileInfo(c.Name + b.FullName.Substring(3));
                        try
                        {
                            if ((copynewfile.Checked && !d.Exists) || d.LastWriteTime < b.LastWriteTime)
                            {
                                File.Copy(b.FullName, d.FullName, true);
                                d.LastWriteTime = b.LastWriteTime;
                                d.Refresh();
                                Txb1.AppendText(DateTime.Now.ToString() + "\t" + b.FullName.Remove(3) + " -> " + d.FullName.Remove(3) + "\t" + b.FullName.Substring(path.Length) + "\r\n");
                                movedsize += b.Length;
                            }
                            successed++;
                        }
                        catch (Exception e)
                        {
                            restore(e, b, d);
                        }
                    }
                }
            }
            foreach(DirectoryInfo b in a.GetDirectories())
            {
                dfs(b);
            }
        }
        public void checkchange()
        {
            driveinfo.Clear();
            bool todfs = true;
            foreach(DriveInfo a in DriveInfo.GetDrives())
            {
                if(a.DriveType==DriveType.Removable||a.DriveType==DriveType.Fixed)
                {
                    if (a.DriveType == DriveType.Removable) todfs = false;
                    driveinfo.Add(a);
                }
            }
            if (todfs) return;
            todfs = true;
            if (todfs)
            {
                successed = 0;
                Environment.SpecialFolder[] chboxpath ={Environment.SpecialFolder.Desktop,Environment.SpecialFolder.MyDocuments,Environment.SpecialFolder.MyPictures,
                                                      Environment.SpecialFolder.MyVideos,Environment.SpecialFolder.MyMusic};
                if(Txb2.Text.Length>0)
                {
                    DirectoryInfo dirinfo = null;
                    try
                    {
                        dirinfo = new DirectoryInfo(Txb2.Text);
                    }
                    catch(ArgumentException)
                    {
                        MessageBox.Show("The Directory Name is Not Correct");
                        return;
                    }
                    if(!dirinfo.Exists)
                    {
                        MessageBox.Show("The following path doesn't exist:\r\n" + Txb2.Text);
                    }
                    else
                    {
                        path = dirinfo.FullName;
                        foreach (DriveInfo a in driveinfo)
                        {
                            newestdir = a.Name;
                            dfs(new DirectoryInfo(newestdir + path.Substring(3)));
                        }
                    }
                }
                for (int i = 0; i < chbox.Length;i++ )
                {
                    for (int j = 0; j < chbox.Length; j++)
                    {
                        if(j==i)
                        {
                            chbox[j].BackColor = chboxcolor[2];
                            chbox[j].ForeColor = chboxcolor[3];
                        }
                        else
                        {
                            chbox[j].BackColor = chboxcolor[0];
                            chbox[j].ForeColor = chboxcolor[1];
                        }
                    }
                    if(chbox[i].Checked)
                    {
                        path = Environment.GetFolderPath(chboxpath[i]);
                        foreach (DriveInfo a in driveinfo)
                        {
                            newestdir = a.Name;
                            dfs(new DirectoryInfo(newestdir + path.Substring(3)));
                        }
                    }
                }
                for (int i = 0; i < chbox.Length; i++)
                {
                    chbox[i].BackColor = chboxcolor[0];
                    chbox[i].ForeColor = chboxcolor[1];
                }
                this.Text = "Successed: " + successed.ToString() + ", Failed: " + failed.Count.ToString()+", Moved: "+convertsize(movedsize)+", Total: "+convertsize(totalsize);
            }
            //this.Text = "Completed";
        }
        public string convertsize(long a)
        {
            double b = (double)a;
            int i;
            for(i=0;i<=4;i++)
            {
                if (b < 1024) break;
                b /= 1024;
            }
            string ans = b.ToString("F3");
            switch(i)
            {
                case 0: return ans + "B";
                case 1: return ans + "KB";
                case 2: return ans + "MB";
                case 3: return ans + "GB";
                default: return ans+"TB";
            }
        }
    }
}
