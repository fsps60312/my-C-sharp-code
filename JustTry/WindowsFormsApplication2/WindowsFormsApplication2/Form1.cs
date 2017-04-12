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
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Drawing.Imaging;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 SendInput(Int32 cInputs, ref INPUT pInputs, Int32 cbSize);
        [DllImport("User32.dll",EntryPoint="FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName,string lpWindowName);
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 28)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public INPUTTYPE dwType;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBOARDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MOUSEINPUT
        {
            public Int32 dx;
            public Int32 dy;
            public Int32 mouseData;
            public MOUSEFLAG dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct KEYBOARDINPUT
        {
            public Int16 wVk;
            public Int16 wScan;
            public KEYBOARDFLAG dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HARDWAREINPUT
        {
            public Int32 uMsg;
            public Int16 wParamL;
            public Int16 wParamH;
        }

        public enum INPUTTYPE : int
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags()]
        public enum MOUSEFLAG : int
        {
            MOVE = 0x1,
            LEFTDOWN = 0x2,
            LEFTUP = 0x4,
            RIGHTDOWN = 0x8,
            RIGHTUP = 0x10,
            MIDDLEDOWN = 0x20,
            MIDDLEUP = 0x40,
            XDOWN = 0x80,
            XUP = 0x100,
            VIRTUALDESK = 0x400,
            WHEEL = 0x800,
            ABSOLUTE = 0x8000
        }

        [Flags()]
        public enum KEYBOARDFLAG : int
        {
            EXTENDEDKEY = 1,
            KEYUP = 2,
            UNICODE = 4,
            SCANCODE = 8
        }
        static public void LeftDown()
        {
            INPUT leftdown = new INPUT();

            leftdown.dwType = 0;
            leftdown.mi = new MOUSEINPUT();
            leftdown.mi.dwExtraInfo = IntPtr.Zero;
            leftdown.mi.dx = 0;
            leftdown.mi.dy = 0;
            leftdown.mi.time = 0;
            leftdown.mi.mouseData = 0;
            leftdown.mi.dwFlags = MOUSEFLAG.LEFTDOWN;

            SendInput(1, ref leftdown, Marshal.SizeOf(typeof(INPUT)));
        }
        static public void LeftUp()
        {
            INPUT leftup = new INPUT();

            leftup.dwType = 0;
            leftup.mi = new MOUSEINPUT();
            leftup.mi.dwExtraInfo = IntPtr.Zero;
            leftup.mi.dx = 0;
            leftup.mi.dy = 0;
            leftup.mi.time = 0;
            leftup.mi.mouseData = 0;
            leftup.mi.dwFlags = MOUSEFLAG.LEFTUP;

            SendInput(1, ref leftup, Marshal.SizeOf(typeof(INPUT)));
        }
        static public void LeftClick()
        {
            LeftDown();
            Thread.Sleep(200);
            LeftUp();
        }
        public Form1()
        {
            InitializeComponent();
            Form1.CheckForIllegalCrossThreadCalls = false;
            this.TopMost = true;
        }
        public void setclipboard(string a)
        {
            try
            {
                Clipboard.SetText(a);
            }
            catch(Exception)
            {
                Application.DoEvents();
                setclipboard(a);
            }
        }
        public void saveforecaseweb(int month,int day,int time)
        {
            IDataObject clipboard = Clipboard.GetDataObject();
            const int webindex = 2000, saveweb = 2000, saveform = 5000,refresh=10000,bigrefresh=30000;
            for(int i=1;i<=3;i++)
            {
                while (findpicture(new Bitmap("360se6 address bar.png")) == new Point(-1, -1))
                {
                    this.Text = "fail to find \"360se6 address bar.png\"";
                    if (this.Top >= Screen.PrimaryScreen.Bounds.Height - 100) this.Top = 100;
                    else this.Top = Screen.PrimaryScreen.Bounds.Height - 100;
                }
                mypoint = findpicture(new Bitmap("360se6 address bar.png"));
                Cursor.Position = new Point(mypoint.X + 90, mypoint.Y + 10);
                Thread.Sleep(webindex);
                LeftClick();
                Thread.Sleep(webindex);
                setclipboard(website[i - 1]);
                SendKeys.SendWait("^a");
                SendKeys.Flush();
                Thread.Sleep(200);
                SendKeys.SendWait(Clipboard.GetText() + "{Enter}");
                this.Text = website[i - 1];
                Thread.Sleep(refresh);
                SendKeys.SendWait("^s");
                SendKeys.Flush();
                Thread.Sleep(saveform);
                setclipboard(month.ToString() + "-" + (DateTime.Now.Hour < 5 ? (day - 1).ToString() : day.ToString()) + " " + time.ToString("D2") + "00-" + i.ToString());
                SendKeys.SendWait("^a");
                Thread.Sleep(saveweb);
                SendKeys.SendWait(Clipboard.GetText() + "{ENTER}y");
                while (findpicture(new Bitmap("360se6 new tab.png")) == new Point(-1, -1))
                {
                    this.Text = "fail to find \"360se6 new tab.png\"";
                    if (this.Top >= Screen.PrimaryScreen.Bounds.Height - 100) this.Top = 100;
                    else this.Top = Screen.PrimaryScreen.Bounds.Height - 100;
                }
                mypoint = findpicture(new Bitmap("360se6 new tab.png"));
                Cursor.Position = new Point(mypoint.X + 10, mypoint.Y + 10);
                Thread.Sleep(webindex);
                LeftClick();
                Thread.Sleep(webindex);
            }
            while (findpicture(new Bitmap("360se6 address bar.png")) == new Point(-1, -1))
            {
                this.Text = "fail to find \"360se6 address bar.png\"";
                if (this.Top >= Screen.PrimaryScreen.Bounds.Height - 100) this.Top = 100;
                else this.Top = Screen.PrimaryScreen.Bounds.Height - 100;
            }
            mypoint = findpicture(new Bitmap("360se6 address bar.png"));
            Cursor.Position = new Point(mypoint.X + 90, mypoint.Y + 10);
            Thread.Sleep(webindex);
            LeftClick();
            Thread.Sleep(webindex);
            Clipboard.Clear();
            setclipboard(website[3]);
            SendKeys.SendWait("^a");
            SendKeys.Flush();
            Thread.Sleep(200);
            SendKeys.SendWait(Clipboard.GetText() + "{Enter}");
            Thread.Sleep(refresh);
            SendKeys.SendWait("^s");
            SendKeys.Flush();
            Thread.Sleep(saveform);
            Clipboard.Clear();
            setclipboard("week" + month.ToString() + "-" + (DateTime.Now.Hour < 5 ? (day - 1).ToString() : day.ToString()) + " " + time.ToString("D2") + "00");
            SendKeys.SendWait("^a");
            SendKeys.Flush();
            Thread.Sleep(200);
            SendKeys.SendWait(Clipboard.GetText() + "{ENTER}y");
            Thread.Sleep(saveweb);

            while (findpicture(new Bitmap("360se6 new tab.png")) == new Point(-1, -1))
            {
                this.Text = "fail to find \"360se6 new tab.png\"";
                if (this.Top >= Screen.PrimaryScreen.Bounds.Height - 100) this.Top = 100;
                else this.Top = Screen.PrimaryScreen.Bounds.Height - 100;
            }
            mypoint = findpicture(new Bitmap("360se6 new tab.png"));
            Cursor.Position = new Point(mypoint.X + 10, mypoint.Y + 10);
            Thread.Sleep(webindex);
            LeftClick();
            Thread.Sleep(webindex);
            while (findpicture(new Bitmap("360se6 address bar.png")) == new Point(-1, -1))
            {
                this.Text = "fail to find \"360se6 address bar.png\"";
                if (this.Top >= Screen.PrimaryScreen.Bounds.Height - 100) this.Top = 100;
                else this.Top = Screen.PrimaryScreen.Bounds.Height - 100;
            }
            mypoint = findpicture(new Bitmap("360se6 address bar.png"));
            Cursor.Position = new Point(mypoint.X + 90, mypoint.Y + 10);
            Thread.Sleep(webindex);
            LeftClick();
            Thread.Sleep(webindex);
            Clipboard.Clear();
            setclipboard(website[4]);
            SendKeys.SendWait("^a");
            SendKeys.Flush();
            Thread.Sleep(200);
            SendKeys.SendWait(Clipboard.GetText() + "{Enter}");
            Thread.Sleep(bigrefresh);
            SendKeys.SendWait("^s");
            SendKeys.Flush();
            Thread.Sleep(saveform);
            Clipboard.Clear();
            setclipboard("rain" + month.ToString() + "-" + (DateTime.Now.Hour == 0 ? (day - 1).ToString() : day.ToString()) + "-" + ((DateTime.Now.Hour + 23) % 24).ToString("D2"));
            SendKeys.SendWait("^a");
            SendKeys.Flush();
            Thread.Sleep(200);
            SendKeys.SendWait(Clipboard.GetText() + "{ENTER}y");
            Clipboard.Clear();
            while(true)
            {
                try
                {
                    Clipboard.SetDataObject(clipboard);
                }
                catch(Exception)
                {
                    Application.DoEvents();
                    continue;
                }
                break;
            }
            this.Text = "等待中";
        }
        public int timestate(int a)
        {
            if ((a >= 23 && a < 24) || (a >= 0 && a < 5)) return 23;
            else if (a >= 5 && a < 11) return 5;
            else if (a >= 11 && a < 17) return 11;
            else if (a >= 17 && a < 23) return 17;
            else
            {
                MessageBox.Show("timestate 錯誤");
                this.Close();
                return -1;
            }
        }
        public void theloop()
        {
            while(true)
            {
                time = timestate(DateTime.Now.Hour);
                if (DateTime.Now.Hour == time)
                {
                    while (DateTime.Now.Minute < 30) Thread.Sleep(5000);
                }
                this.TopMost = true;
                if (FindWindow("360se6_Frame", null) != IntPtr.Zero)
                {
                    windowtarget = FindWindow("360se6_Frame", null);
                    this.Text = "找到了";
                    ShowWindow(windowtarget,1);
                    SetForegroundWindow(windowtarget);
                    while (findpicture(new Bitmap("360se6 new tab.png"))==new Point(-1,-1))
                    {
                        this.Text = "fail to find \"360se6 new tab.png\"";
                        if (this.Top >= Screen.PrimaryScreen.Bounds.Height - 100) this.Top = 100;
                        else this.Top = Screen.PrimaryScreen.Bounds.Height - 100;
                    }
                    mypoint = findpicture(new Bitmap("360se6 new tab.png"));
                    Cursor.Position = new Point(mypoint.X + 10, mypoint.Y + 10);
                    LeftClick();
                }
                else
                {
                    Interaction.Shell(@"C:\Users\ney\AppData\Roaming\360se6\Application\360se.exe");
                    while(FindWindow("360se6_Frame", null)==null) Thread.Sleep(1000);
                    windowtarget = FindWindow("360se6_Frame", null);
                    this.Text = "找到了";
                    ShowWindow(windowtarget, 1);
                    SetForegroundWindow(windowtarget);
                }
                this.Text = "new tab";
                thread[0] = new Thread(()=>{saveforecaseweb(DateTime.Now.Month, DateTime.Now.Day, time);});
                thread[0].IsBackground = true;
                thread[0].SetApartmentState(ApartmentState.STA);
                thread[0].Start();
                this.TopMost = false;
                while(time==timestate(System.DateTime.Now.Hour)) Thread.Sleep(10000);
            }
        }
        public Thread[] thread=new Thread[2];
        public int time;
        public string[] website = new string[] { "http://www.cwb.gov.tw/V7/forecast/index.htm", "http://www.cwb.gov.tw/V7/forecast/index2.htm", "http://www.cwb.gov.tw/V7/forecast/index3.htm", "http://www.cwb.gov.tw/V7/forecast/week/week.htm", "http://www.cwb.gov.tw/V7/observe/rainfall/Rain_Hr/22.htm" };
        public Point mypoint;
        public IntPtr windowtarget;
        public Point findpicture(Bitmap a)
        {
            Bitmap b = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics gdest = Graphics.FromImage(b);
            Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero);
            BitBlt(gdest.GetHdc(), 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, gsrc.GetHdc(), 0, 0, (int)CopyPixelOperation.SourceCopy);
            gdest.ReleaseHdc();
            gsrc.ReleaseHdc();
            Rectangle a_rec=new Rectangle(0,0,a.Width,a.Height);
            BitmapData a_bmp = a.LockBits(a_rec, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr a_ptr = a_bmp.Scan0;
            byte[] a_byte = new byte[a_rec.Width * a_rec.Height * 4];
            Marshal.Copy(a_ptr, a_byte, 0, a_byte.Length);
            Rectangle b_rec = new Rectangle(0, 0, b.Width, b.Height);
            BitmapData b_bmp = b.LockBits(b_rec, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr b_ptr = b_bmp.Scan0;
            byte[] b_byte = new byte[b_rec.Width * b_rec.Height * 4];
            Marshal.Copy(b_ptr, b_byte, 0, b_byte.Length);

            int width=Screen.PrimaryScreen.Bounds.Width-a.Width;
            int height=Screen.PrimaryScreen.Bounds.Height-a.Height;
            for(int i=0;i<width;i++)
            {
                for(int j=0;j<height;j++)
                {
                    bool escape = false;
                    for(int k=0;k<a.Width&&!escape;k++)
                        for (int l = 0; l < a.Height && !escape; l++)
                        {
                            if (a_byte[(l * a.Width + k) * 4] != b_byte[((j + l) * b.Width + (i + k)) * 4] || a_byte[(l * a.Width + k) * 4] != b_byte[((j + l) * b.Width + (i + k)) * 4])
                            {
                                escape = true;
                                break;
                            }
                        }
                    if (!escape)
                    {
                        a.UnlockBits(a_bmp);
                        b.UnlockBits(b_bmp);
                        return new Point(i, j);
                    }
                }
                //this.Text = i.ToString();
            }
            a.UnlockBits(a_bmp);
            b.UnlockBits(b_bmp);
            return new Point(-1, -1);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            //Cursor.Position = new Point(-1, -1);
            //MessageBox.Show(Cursor.Position.X.ToString() + " " + Cursor.Position.Y.ToString());
            //mypoint=findpicture(new Bitmap("360se6 new tab.png"));
            //Cursor.Position = new Point(mypoint.X + 10, mypoint.Y + 10);
            //MessageBox.Show(mypoint.X.ToString()+" "+mypoint.Y.ToString());
            this.Text = "等待中";
            thread[1] = new Thread(theloop);
            thread[1].Start();
        }
    }
}
