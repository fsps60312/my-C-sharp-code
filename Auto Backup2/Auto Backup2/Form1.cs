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

namespace Auto_Backup2
{
    public partial class Form1 : Form
    {
        public TableLayoutPanel Tlp1 = new TableLayoutPanel();
        public TableLayoutPanel Tlp2 = new TableLayoutPanel();
        public TableLayoutPanel1 Panel1 = new TableLayoutPanel1();
        public TableLayoutPanel2 Panel2 = new TableLayoutPanel2();
        public TableLayoutPanel3 Panel3 = new TableLayoutPanel3();
        public Panel Pan1 = new Panel();
        public Panel Pan2 = new Panel();
        public Panel Pan3 = new Panel();
        public ProgressBar Pgbar = new ProgressBar();
        public TextBox Txb1 = new TextBox();
        public FileInfo SettingFile = new FileInfo("Auto Backup2 Settings.ini");
        public static Color[] BtnColor = new Color[4];
        public string ThisText = "";
        public partial class TableLayoutPanel1:TableLayoutPanel
        {
            public Button Add = new Button();
            public Button Remove = new Button();
            private TableLayoutPanel Tlp = new TableLayoutPanel();
            public partial class CheckBox1:CheckBox
            {
                public int index;
                public CheckBox1(string name,int a)
                {
                    this.Text = name;
                    this.Checked = true;
                    this.Dock = DockStyle.Top;
                    index = a;
                }
                public Thread ShineThread = null;
                public Thread ColorThread=null;
                private void ChangeColor(Color a)
                {
                    if (ColorThread != null) ColorThread.Abort();
                    ColorThread = new Thread(() =>
                    {
                        while (this.BackColor != a)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                this.BackColor = ApproachColor(this.BackColor, a);
                            }
                            Thread.Sleep(10);
                        }
                    });
                    ColorThread.IsBackground = true;
                    ColorThread.Priority = ThreadPriority.Lowest;
                    ColorThread.Start();
                }
                public void Shine(bool start)
                {
                    if(!start)
                    {
                        if (ShineThread != null) ShineThread.Abort();
                        ChangeColor(DefaultBackColor);
                        return;
                    }
                    ShineThread = new Thread(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep(500);
                            ChangeColor(Color.FromArgb(0, 0, 255));
                            Thread.Sleep(500);
                            ChangeColor(Color.FromArgb(100, 100, 255));
                        }
                    });
                    ShineThread.IsBackground = true;
                    ShineThread.Priority = ThreadPriority.Lowest;
                    ShineThread.Start();
                }
            }
            public CheckBox1[] Dirs = new CheckBox1[0];
            public HashSet<string> DirNames = new HashSet<string>();
            private Panel Pan = new Panel();
            public bool Deleting = false;
            public TableLayoutPanel1()
            {
                this.Dock = DockStyle.Fill;
                this.ColumnCount = 1;
                this.RowCount = 2;
                this.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
                this.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                this.Controls.Add(Tlp); this.SetCellPosition(Tlp, new TableLayoutPanelCellPosition(0, 0));
                {
                    Tlp.Dock = DockStyle.Fill;
                    Tlp.ColumnCount = 2;
                    Tlp.RowCount = 1;
                    for (int i = 0; i < Tlp.ColumnCount; i++) Tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                    Tlp.Controls.Add(Add); Tlp.SetCellPosition(Add, new TableLayoutPanelCellPosition(0, 0));
                    {
                        Add.Dock = DockStyle.Fill;
                        Add.Text = "Add";
                    }
                    Tlp.Controls.Add(Remove); Tlp.SetCellPosition(Remove, new TableLayoutPanelCellPosition(1, 0));
                    {
                        Remove.Dock = DockStyle.Fill;
                        Remove.Text = "Remove";
                    }
                }
                this.Controls.Add(Pan); this.SetCellPosition(Pan, new TableLayoutPanelCellPosition(0, 1));
                {
                    Pan.Dock = DockStyle.Fill;
                    Pan.HorizontalScroll.Enabled = true;
                    Pan.VerticalScroll.Enabled = true;
                }
            }
            public void AddDirectory(string fullname)
            {
                if(DirNames.Contains(fullname))
                {
                    MessageBox.Show("Please don't add two same directories.");
                    return;
                }
                //MessageBox.Show(fullname);
                int a = Dirs.Length;
                Array.Resize(ref Dirs, a + 1);
                Dirs[a] = new CheckBox1(fullname,a);
                //Dirs[a].Text = "hi";
                Dirs[a].Click += TableLayoutPanel1_Click;
                Pan.Controls.Add(Dirs[a]);
                DirNames.Add(fullname);
            }
            void TableLayoutPanel1_Click(object sender, EventArgs e)
            {
                if(Deleting)
                {
                    RemoveDirectory((sender as CheckBox1).index);
                }
            }
            public void RemoveDirectory(int index)
            {
                DirNames.Remove(Dirs[index].Text);
                Pan.Controls.Remove(Dirs[index]);
                for (int i = index + 1; i < Dirs.Length; i++)
                {
                    Dirs[i - 1] = Dirs[i];
                    Dirs[i - 1].index = i - 1;
                }
                Array.Resize(ref Dirs, Dirs.Length - 1);
            }
        }
        public partial class TableLayoutPanel2 : TableLayoutPanel
        {
            public Button Refres = new Button();
            private TableLayoutPanel Tlp = new TableLayoutPanel();
            private Panel Pan = new Panel();
            public Thread thr = null;
            public List<DriveInfo> ToCopy = new List<DriveInfo>();
            public List<DriveInfo> ToPaste = new List<DriveInfo>();
            public void Shine(DriveInfo tocopy,DriveInfo topaste)
            {
                if (thr != null) thr.Abort();
                DoRefresh();
                if(tocopy==null||topaste==null)
                {
                    return;
                }
                int a = GetDriveIndex(tocopy.Name[0]), b = GetDriveIndex(topaste.Name[0]);
                thr = new Thread(() =>
                {
                    while (true)
                    {
                        Drives[a].SetColor(4);
                        Thread.Sleep(100);
                        Drives[b].SetColor(5);
                        Thread.Sleep(300);
                        Drives[a].SetColor(Drives[a].state);
                        Thread.Sleep(100);
                        Drives[b].SetColor(Drives[b].state);
                        Thread.Sleep(300);
                    }
                });
                thr.IsBackground = true;
                thr.Priority = ThreadPriority.Lowest;
                thr.Start();
            }
            public partial class Label1:Label
            {
                public int state = 0;
                public Label1(DriveInfo a)
                {
                    this.Dock = DockStyle.Top;
                    this.Click += Label1_Click;
                    updateinfo(a);
                }
                public void updateinfo(DriveInfo a)
                {
                    this.Text = driveinfo(a);
                }
                private string driveinfo(DriveInfo a)
                {
                    string ans = ((char)(a.Name[0])).ToString() + @":\";
                    if (!a.IsReady) return ans;
                    try { ans += (a.VolumeLabel).PadRight(20); }
                    catch (Exception) { }
                    try { ans += (convertsize(a.TotalFreeSpace) + "/" + convertsize(a.TotalSize)).PadRight(30); }
                    catch (Exception) { }
                    try { ans += ("Format:" + a.DriveFormat + "," + a.DriveType.ToString()); }
                    catch (Exception) { }
                    return ans;
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
                private Color[] colors = new Color[]{
                    Color.FromArgb(0,0,0),Color.FromArgb(255,255,255)
                    ,Color.FromArgb(0,0,0),Color.FromArgb(255,255,0)
                    ,Color.FromArgb(255,255,255),Color.FromArgb(0,0,255)
                    ,Color.FromArgb(255,255,255),Color.FromArgb(0,255,0)
                    ,Color.FromArgb(0,0,0),Color.FromArgb(200,200,0)
                    ,Color.FromArgb(255,255,255),Color.FromArgb(0,0,200)
                };
                public Thread thr = null;
                public void SetColor(int a)
                {
                    if (thr != null) thr.Abort();
                    thr = new Thread(() =>
                    {
                        while (this.ForeColor != colors[a * 2] || this.BackColor != colors[a * 2 + 1])
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                this.ForeColor = ApproachColor(this.ForeColor, colors[a * 2]);
                                this.BackColor = ApproachColor(this.BackColor, colors[a * 2 + 1]);
                            }
                            Thread.Sleep(10);
                        }
                    });
                    thr.Priority = ThreadPriority.Lowest;
                    thr.IsBackground = true;
                    thr.Start();
                }
                void Label1_Click(object sender, EventArgs e)
                {
                    state = (state + 1) % 4;
                    SetColor(state);
                }
            }
            public Label1[] Drives = new Label1[26];
            public TableLayoutPanel2()
            {
                for (int i = 0; i < 26; i++) Drives[i] = new Label1(new DriveInfo(((char)('A'+i)).ToString()));
                this.Dock = DockStyle.Fill;
                this.ColumnCount = 1;
                this.RowCount = 2;
                this.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
                this.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                this.Controls.Add(Tlp); this.SetCellPosition(Tlp, new TableLayoutPanelCellPosition(0, 0));
                {
                    Tlp.ColumnCount = 1;
                    Tlp.RowCount = 1;
                    for (int i = 0; i < Tlp.ColumnCount; i++) Tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
                    Tlp.Controls.Add(Refres); Tlp.SetCellPosition(Refres, new TableLayoutPanelCellPosition(0, 0));
                    {
                        Refres.Dock = DockStyle.Fill;
                        Refres.Text = "Refresh";
                    }
                }
                this.Controls.Add(Pan); this.SetCellPosition(Pan, new TableLayoutPanelCellPosition(0, 1));
                {
                    Pan.Dock = DockStyle.Fill;
                    Pan.HorizontalScroll.Enabled = true;
                    Pan.VerticalScroll.Enabled = true;
                }
                DoRefresh();
            }
            public void DoRefresh()
            {
                Pan.Controls.Clear();
                ToCopy.Clear();
                ToPaste.Clear();
                foreach (DriveInfo a in DriveInfo.GetDrives())
                {
                    int drin = GetDriveIndex(a.Name[0]);
                    Label1 b = Drives[drin];
                    b.updateinfo(a);
                    b.SetColor(b.state);
                    Pan.Controls.Add(b);
                    if (b.state == 1 || b.state == 3)
                    {
                        ToCopy.Add(a);
                    }
                    if (b.state == 2 || b.state == 3)
                    {
                        ToPaste.Add(a);
                    }
                }
            }
            public int GetDriveIndex(char a)
            {
                if (IsUpper(a)) a = (char)(a - 'A' + 'a');
                return a - 'a';
            }
        }
        public static Color ApproachColor(Color a, Color b)
        {
            return Color.FromArgb(a.R == b.R ? a.R : (a.R < b.R ? a.R + 1 : a.R - 1), a.G == b.G ? a.G : (a.G < b.G ? a.G + 1 : a.G - 1), a.B == b.B ? a.B : (a.B < b.B ? a.B + 1 : a.B - 1));
        }
        public partial class TableLayoutPanel3 : TableLayoutPanel
        {
            public Button Start = new Button();
            public Button ClearText = new Button();
            public CheckBox StartBackupImmediately = new CheckBox();
            public CheckBox ShowErrorMessage = new CheckBox();
            public TableLayoutPanel3()
            {
                this.Dock = DockStyle.Fill;
                this.ColumnCount = 1;
                this.RowCount = 4;
                for (int i = 0; i < this.RowCount; i++) this.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
                this.Controls.Add(Start); this.SetCellPosition(Start, new TableLayoutPanelCellPosition(0, 0));
                {
                    Start.Dock = DockStyle.Fill;
                    Start.Text = "Start";
                }
                this.Controls.Add(ClearText); this.SetCellPosition(ClearText, new TableLayoutPanelCellPosition(0, 1));
                {
                    ClearText.Dock = DockStyle.Fill;
                    ClearText.Text = "Clear Text";
                }
                this.Controls.Add(StartBackupImmediately); this.SetCellPosition(StartBackupImmediately, new TableLayoutPanelCellPosition(0, 2));
                {
                    StartBackupImmediately.Dock = DockStyle.Fill;
                    StartBackupImmediately.Text = "Back up my files immediately at next run";
                    StartBackupImmediately.Checked = false;
                }
                this.Controls.Add(ShowErrorMessage); this.SetCellPosition(ShowErrorMessage, new TableLayoutPanelCellPosition(0, 3));
                {
                    ShowErrorMessage.Dock = DockStyle.Fill;
                    ShowErrorMessage.Text = "Show Error Messages";
                    ShowErrorMessage.Checked = true;
                }
            }
        }
        public Form1()
        {
            //Pgbar.Maximum = 0;
            Form1.CheckForIllegalCrossThreadCalls = false;
            BtnColor[0] = new Button().BackColor;
            BtnColor[1] = new Button().ForeColor;
            BtnColor[2] = Color.FromArgb(0, 0, 255);
            BtnColor[3] = Color.FromArgb(255, 255, 255);
            InitializeComponent();
            this.Controls.Add(Tlp1);
            Tlp1.Dock = DockStyle.Fill;
            Tlp1.RowCount = 3;
            Tlp1.ColumnCount = 1;
            Tlp1.RowStyles.Add(new RowStyle(SizeType.AutoSize, 1));
            Tlp1.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            Tlp1.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            Tlp1.Controls.Add(Pgbar); Tlp1.SetCellPosition(Pgbar, new TableLayoutPanelCellPosition(0, 0));
            {
                Pgbar.Dock = DockStyle.Fill;
            }
            Tlp1.Controls.Add(Txb1); Tlp1.SetCellPosition(Txb1, new TableLayoutPanelCellPosition(0, 1));
            {
                Txb1.Dock = DockStyle.Fill;
                Txb1.Multiline = true;
                Txb1.ScrollBars = ScrollBars.Both;
            }
            Tlp1.Controls.Add(Tlp2); Tlp1.SetCellPosition(Tlp2, new TableLayoutPanelCellPosition(0, 2));
            {
                Tlp2.Dock = DockStyle.Fill;
                Tlp2.RowCount = 1;
                Tlp2.ColumnCount = 3;
                Tlp2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));//Directories
                Tlp2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));//Drives
                Tlp2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));//Buttons
                Tlp2.Controls.Add(Panel1); Tlp2.SetCellPosition(Panel1, new TableLayoutPanelCellPosition(0, 0));
                {
                    Panel1.Add.Click += Add_Click;
                    Panel1.Remove.Click += Remove_Click;
                }
                Tlp2.Controls.Add(Panel2); Tlp2.SetCellPosition(Panel2, new TableLayoutPanelCellPosition(1, 0));
                {
                    Panel2.Refres.Click += Refresh_Click;
                }
                Tlp2.Controls.Add(Panel3); Tlp2.SetCellPosition(Panel3, new TableLayoutPanelCellPosition(2, 0));
                {
                    Panel3.Start.Click += Start_Click;
                    Panel3.ClearText.Click += ClearText_Click;
                }
            }
            this.FormClosing += Form1_FormClosing;
            LoadSetting();
            PgbarThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(200);
                    this.Text = ThisText;
                    Pgbar.Maximum = pgbarmax;
                    Pgbar.Value = pgbarvalue > pgbarmax ? pgbarmax : pgbarvalue;
                }
            });
            PgbarThread.IsBackground = true;
            PgbarThread.Priority = ThreadPriority.Lowest;
            PgbarThread.Start();
            if (Panel3.StartBackupImmediately.Checked) Start_Click(null, null);
        }
        void ClearText_Click(object sender, EventArgs e)
        {
            Txb1.Clear();
        }
        public Thread PgbarThread=null;
        public Thread BackupThread = null;
        public int pgbarvalue = 0;
        public int pgbarmax=0;
        public void CountFiles(DirectoryInfo a,DriveInfo source,DriveInfo destiny)
        {
            try
            {
                foreach(FileInfo i in a.GetFiles())
                {
                    string b = source.Name.Remove(1) + i.FullName.Substring(1), c = destiny.Name.Remove(1) + i.FullName.Substring(1);
                    //this.Text = pgbarmax.ToString()+b+c;
                    if (new FileInfo(c).Exists && new FileInfo(b).LastWriteTime.CompareTo(new FileInfo(c).LastWriteTime) <= 0) continue;
                    else pgbarmax++;
                }
                //if(pgbarmax>0)    MessageBox.Show(pgbarmax.ToString());
            }
            catch (Exception) { }
            try
            {
                foreach (DirectoryInfo i in a.GetDirectories())
                {
                    CountFiles(i,source,destiny);
                }
            }
            catch(Exception error)
            {
                AppendText(error);
            }
        }
        void Start_Click(object sender, EventArgs e)
        {
            Panel2.DoRefresh();
            if (BackupThread != null)
            {
                BackupThread.Abort();
                Panel3.Start.Text = "Restart";
                BackupThread = null;
                return;
            }
            Panel3.Start.Text = "Stop";
            BackupThread = new Thread(() =>
            {
                List<DriveInfo> tocopy = new List<DriveInfo>();
                List<DriveInfo> topaste = new List<DriveInfo>();
                for (int i = 0; i < Panel2.ToCopy.Count; i++) tocopy.Add(Panel2.ToCopy[i]);
                for (int i = 0; i < Panel2.ToPaste.Count; i++) topaste.Add(Panel2.ToPaste[i]);
                foreach (DriveInfo a in tocopy)
                {
                    foreach (DriveInfo b in topaste)
                    {
                        if (a == b) continue;
                        Panel2.Shine(a, b);
                        for (int i = 0; i < Panel1.Dirs.Length; i++)
                        {
                            Panel1.Dirs[i].Shine(true);
                            if (Panel1.Dirs[i].Checked)
                            {
                                string path = Panel1.Dirs[i].Text;
                                pgbarmax = pgbarvalue = 0;
                                ThisText = "Checking files...";
                                CountFiles(new DirectoryInfo(a.Name.Remove(1)+path.Substring(1)),a,b);
                                //MessageBox.Show(pgbarmax.ToString());
                                if (pgbarmax > 0)
                                    BackupFile(new DirectoryInfo(a.Name.Remove(1) + path.Substring(1))
                                    , new DirectoryInfo(b.Name.Remove(1) + path.Substring(1)));
                            }
                            Panel1.Dirs[i].Shine(false);
                        }
                    }
                }
                Panel2.Shine(null, null);
                ThisText = "Back up finished!";
                Txb1.AppendText("Back up finished!\r\n");
                Thread thr = new Thread(() =>
                {
                    Start_Click(null, null);
                });
                thr.IsBackground = true;
                thr.Start();
            });
            BackupThread.IsBackground = true;
            BackupThread.Start();
        }
        void Refresh_Click(object sender, EventArgs e)
        {
            Panel2.DoRefresh();
        }
        void Remove_Click(object sender, EventArgs e)
        {
            Button a = sender as Button;
            if(Panel1.Deleting)
            {
                a.BackColor = BtnColor[0];
                a.ForeColor = BtnColor[1];
                Panel1.Deleting = false;
            }
            else
            {
                a.BackColor = BtnColor[2];
                a.ForeColor = BtnColor[3];
                Panel1.Deleting = true;
            }
        }
        void Add_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog a=new FolderBrowserDialog();
            a.ShowDialog();
            if (a.SelectedPath.Length == 0) return;
            Panel1.AddDirectory(a.SelectedPath);
        }
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetting();
            Process.GetCurrentProcess().Kill();
        }
        public void LoadSetting()
        {
            if (!SettingFile.Exists)
            {
                Initial();
                return;
            }
            try
            {
                StreamReader reader = new StreamReader(SettingFile.FullName, Encoding.UTF8);
                string[] file = reader.ReadToEnd().Split('\n');
                reader.Close();
                for (int i = 0; i < file.Length; i++) file[i] = file[i].Trim('\r');
                int now = -1, a = file.Length;

                while (++now < a && file[now] != "//Panel1") ;

                while (++now < a && file[now] != "//Panel2") Panel1.AddDirectory(file[now]);

                now++;
                if (file[now].Length == 26)
                {
                    for (int i = 0; i < 26; i++)
                    {
                        Panel2.Drives[i].state = file[now][i] - '0';
                    }
                }
                Panel2.DoRefresh();

                while (++now < a && file[now] != "//Panel3") ;

                now++;
                if(file[now].Length==2)
                {
                    Panel3.StartBackupImmediately.Checked = file[now][0] == '1' ? true : false;
                    Panel3.ShowErrorMessage.Checked = file[now][1] == '1' ? true : false;
                }

                while (++now < a && file[now] != "//End") ;
            }
            catch(Exception error)
            {
                AppendText(error);
                Initial();
                return;
            }
        }
        public void Initial()
        {
            Panel1.AddDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            Panel1.AddDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            Panel1.AddDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            Panel1.AddDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            Panel1.AddDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            Panel1.AddDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Favorites));
        }
        public void SaveSetting()
        {
            StreamWriter writer = new StreamWriter(SettingFile.FullName, false, Encoding.UTF8);
            writer.WriteLine("The Programmer:余柏序(Burney)");
            writer.WriteLine("Facebook:https://www.facebook.com/fsps60312");
            writer.WriteLine("Email:fsps60312@yahoo.com.tw");
            writer.WriteLine("Cellphone:0919508359");
            writer.WriteLine("Telephone:075560220");
            writer.WriteLine("//Panel1");
            for(int i=0;i<Panel1.Dirs.Length;i++)
            {
                writer.WriteLine(Panel1.Dirs[i].Text);
            }
            writer.WriteLine("//Panel2");
            for(int i=0;i<26;i++) writer.Write(Panel2.Drives[i].state.ToString());
            writer.WriteLine();
            writer.WriteLine("//Panel3");
            writer.Write(Panel3.StartBackupImmediately.Checked ? "1" : "0");
            writer.Write(Panel3.ShowErrorMessage.Checked ? "1" : "0");
            writer.WriteLine();
            writer.WriteLine("//End");
            writer.Close();
        }
        public void AppendText(string a)
        {
            Txb1.AppendText(DateTime.Now.ToString() + "\t" + a + "\r\n");
        }
        public void AppendText(Exception a)
        {
            if (Panel3.ShowErrorMessage.Checked) AppendText(a.ToString());
        }
        public void CopyFile(string source,string destiny)
        {
            //MessageBox.Show(source + "\r\n" + destiny);
            if (new FileInfo(destiny).Exists && new FileInfo(source).LastWriteTime.CompareTo(new FileInfo(destiny).LastWriteTime) <= 0) return;
            try
            {
                File.Copy(source, destiny, true);
                AppendText(source + " -> " + destiny);
                pgbarvalue++;
            }
            catch(Exception error)
            {
                AppendText(error);
            }
        }
        public void BackupFile(DirectoryInfo source,DirectoryInfo destiny)
        {
            if (!source.Exists)
            {
                MessageBox.Show("Can't find \"" + source + "\"");
            }
            if(!destiny.Exists) destiny.Create();
            FileInfo[] a = null;
            try
            {
                a = source.GetFiles();
            }
            catch (Exception error)
            {
                AppendText(error);
                return;
            }
            foreach(FileInfo i in a)
            {
                ThisText = "Copying " + i.FullName;
                CopyFile(i.FullName, destiny.FullName + "\\" + i.Name);
            }
            DirectoryInfo[] b=source.GetDirectories();
            foreach(DirectoryInfo i in b)
            {
                BackupFile(i, new DirectoryInfo(destiny.FullName + "\\" + i.Name));
            }
        }
        public static bool IsUpper(char a) { return a >= 'A' && a <= 'Z'; }
    }
}
