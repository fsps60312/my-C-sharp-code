using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Motivation;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Megapolis
{
    public partial class Form1 : Form
    {
        private static MainTask mainTask = new MainTask(new TimeSpan(0, 0, 1));
        TaskScheduler taskScheduler = new TaskScheduler(mainTask);
        private void AppendText(string status)
        {
            status = DateTime.Now.ToLongTimeString().Replace(" ", "").TrimEnd('M') + "  " + status + "\r\n";
            lengthOfNotSentText += status.Length;
            TXBlog.AppendText(status);
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Left = 0;// Screen.PrimaryScreen.Bounds.Width / 2;
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
            InitializeControls();
            taskScheduler.Stopped += delegate ()
             {
                 Do(() =>
                 {
                     BTNstart.Enabled = true;
                     BTNstop.Enabled = false;
                 });
             };
            var logChanged = new Action<string>((string log) =>
            {
                Do(() =>
                {
                    LBLstatus.Text = log;
                    AppendText(log);
                });
            });
            taskScheduler.LogChanged += delegate (string log) { logChanged(log); };
            NetworkCommunicator.LogChanged += delegate (string log) { logChanged(log); };
            NetworkCommunicator.StatusChanged += delegate (string status) { Do(() => { LBLstatus.Text = status; }); };
            NetworkCommunicator.MessageReceived += MessageReceived;
            NetworkCommunicator.Start();
        }
        private void MessageReceived(string msg,out string reply)
        {
            if (msg.IndexOf("Click") == 0)
            {
                msg = msg.Substring(6);
                foreach (var b in BTNs)
                {
                    if(b.Text==msg)
                    {
                        if(b.Enabled)
                        {
                            b.PerformClick();
                            reply = $"Button \"{b.Text}\" Clicked";
                        }
                        else
                        {
                            reply = $"Button \"{b.Text}\" not enabled";
                        }
                        return;
                    }
                }
                reply = $"No such button: {msg}";
                return;
            }
            switch(msg)
            {
                case "Get TXBlog.Text":
                    {
                        reply = TXBlog.Text;
                    }break;
                case "Is BlueStacks Started":
                    {
                        reply = $"{MyTask.PublicIsBlueStacksStarted()}";
                    }break;
                case "Close BlueStacks":
                    {
                        mainTask.PublicCloseBlueStacks();
                        reply = $"BlueStacks Closed";
                    }break;
                case "Get TXBlog.Text Updates":
                    {
                        string s = TXBlog.Text;
                        reply =s.Substring(s.Length - Math.Min(s.Length, lengthOfNotSentText));
                        if (lengthOfNotSentText > s.Length) reply += "(Some long-time-ago information was lost)\r\n";
                        lengthOfNotSentText = 0;
                    }break;
                default:
                    {
                        reply = $"No such instruction: {msg}";
                    }
                    break;
            }
        }
        private void BTNstop_Click(object sender, EventArgs e)
        {
            BTNstop.Enabled = false;
            Thread thread = new Thread(() =>
            {
                taskScheduler.Stop();
            });
            thread.IsBackground = true;
            thread.Start();
        }
        private void BTNstart_Click(object sender, EventArgs e)
        {
            BTNstart.Enabled = false;
            BTNstop.Enabled = true;
            Thread thread = new Thread(() =>
            {
                taskScheduler.Start();
            });
            thread.IsBackground = true;
            thread.Start();
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (true||e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
                LBLstatus.Text = $"Release mouse to save capture from the image file";
            }
            else
            {
                LBLstatus.Text = $"Wrong format: {string.Join(", ",e.Data.GetFormats())}";
            }
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                try
                {
                    string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                    List<int> valids = new List<int>();
                    List<Bitmap> bmps = new List<Bitmap>();
                    LBLstatus.Text = "Checking..."; Application.DoEvents();
                    for (int i=0;i<fileNames.Length;i++)
                    {
                        Bitmap bmp;
                        try
                        {
                            bmp = new Bitmap(fileNames[i]);
                            valids.Add(i);
                            bmps.Add(bmp);
                        }
                        catch(Exception error)
                        {
                            LBLstatus.Text=$"error:\r\n{error}\r\n"; Application.DoEvents();
                        }
                    }
                    LBLstatus.Text = $"get:\r\n{string.Join("\r\n", fileNames)}"; Application.DoEvents();
                    if (valids.Count!=fileNames.Length)
                    {
                        var result=MessageBox.Show($"Only {valids.Count} of {fileNames.Length} files would be processed correctly. Continue?", "Warning", MessageBoxButtons.OKCancel);
                        if(result!=DialogResult.OK)
                        {
                            LBLstatus.Text = "Canceled";
                            return;
                        }
                    }
                    for (int i = 0; i < valids.Count; i++)
                    {
                        LBLstatus.Text = $"Converting...({i+1}/{valids.Count})"; Application.DoEvents();
                        FileInfo info = new FileInfo(fileNames[valids[i]]);
                        //MessageBox.Show($"Name: {info.Name}\r\nExtension: {info.Extension}\r\nFull Name: {info.FullName}\r\nDirectory Name: {info.DirectoryName}");
                        try
                        {
                            mainTask.SaveCaptureFromImageFile(bmps[i], info.Name.Remove(info.Name.Length - info.Extension.Length) + " - captured" + info.Extension);
                        }
                        catch (Exception error)
                        {
                            LBLstatus.Text = $"error:\r\n{error}\r\n"; Application.DoEvents();
                        }
                    }
                }
                catch(Exception error)
                {
                    LBLstatus.Text = $"error:\r\n{error}";
                }
            }
            else
            {
                MessageBox.Show($"Wrong format: {string.Join(", ", e.Data.GetFormats())}");
            }
        }
        private void Do(Action a)
        {
            if (this.InvokeRequired) this.Invoke(a);
            else a.Invoke();
        }
        private void InitializeControls()
        {
            this.Size = new Size(800, 800);
            {
                TLPmain = new MyTableLayoutPanel(4, 1, "PAAA", "P");
                {
                    TXBlog = new MyTextBox(true);
                    TLPmain.AddControl(TXBlog, 0, 0);
                }
                {
                    LBLstatus = new MyLabel("");
                    LBLstatus.MaximumSize = new Size(this.Width, this.Height / 3);
                    TLPmain.AddControl(LBLstatus, 1, 0);
                }
                {
                    TLPbtn = new MyTableLayoutPanel(1, 7, "P", "AAAAAAA");
                    var btnFont = new Font("Consolas", 12, FontStyle.Regular);
                    {
                        BTNstart = new MyButton("Start");
                        BTNstart.Font = btnFont;
                        BTNstart.Click += BTNstart_Click;
                        TLPbtn.AddControl(BTNstart, 0, 0);
                    }
                    {
                        BTNstop = new MyButton("Stop");
                        BTNstop.Font = btnFont;
                        BTNstop.Enabled = false;
                        BTNstop.Click += BTNstop_Click;
                        TLPbtn.AddControl(BTNstop, 0, 1);
                    }
                    {
                        BTNxButton = new MyButton("XButton");
                        BTNxButton.Font = btnFont;
                        BTNxButton.Click += delegate
                        {
                            BTNxButton.Enabled = false;
                            XButton.ShowWindow();
                        };
                        XButton.SetRecoverButton(BTNxButton);
                        TLPbtn.AddControl(BTNxButton, 0, 2);
                    }
                    {
                        BTNrunManual = new MyButton("Run Manual");
                        BTNrunManual.Font = btnFont;
                        BTNrunManual.Click += delegate
                        {
                            mainTask.RunManualSelectedTasks();
                        };
                        TLPbtn.AddControl(BTNrunManual, 0, 3);
                    }
                    {
                        BTNshowNextRunTime = new MyButton("Show Next Run Time");
                        BTNshowNextRunTime.Font = btnFont;
                        BTNshowNextRunTime.Click += delegate
                          {
                              mainTask.ShowNextRunTime();
                          };
                        TLPbtn.AddControl(BTNshowNextRunTime, 0, 4);
                    }
                    {
                        BTNshowTaskInfo = new MyButton("Show Task Info");
                        BTNshowTaskInfo.Font = btnFont;
                        BTNshowTaskInfo.Click += delegate
                          {
                              mainTask.ShowTaskInfo();
                          };
                        TLPbtn.AddControl(BTNshowTaskInfo, 0, 5);
                    }
                    {
                        CHBshowCursorLocation = new MyCheckBox("Show Cursor Location");
                        CHBshowCursorLocation.Font = btnFont;
                        TLPbtn.AddControl(CHBshowCursorLocation, 0, 6);
                    }
                    TLPmain.AddControl(TLPbtn, 2, 0);
                }
                {
                    TLPctrl = new MyTableLayoutPanel(1, 4, "A", "P2P1P2P1");
                    {
                        IFDclick = new MyInputField();
                        IFDclick.AddField("X,Y");
                        TLPctrl.AddControl(IFDclick, 0, 0);
                    }
                    {
                        BTNclick = new MyButton("Click");
                        BTNclick.Click += delegate
                          {
                              try
                              {
                                  string[] s = IFDclick.GetField("X,Y").Split(',');
                                  MyTask.PublicClick(new Point(int.Parse(s[0]), int.Parse(s[1])));
                              }
                              catch (Exception error)
                              {
                                  MessageBox.Show($"Error:\r\n{error}");
                              }
                          };
                        TLPctrl.AddControl(BTNclick,0,1);
                    }
                    {
                        IFDgoToMap = new MyInputField();
                        IFDgoToMap.AddField("X,Y");
                        TLPctrl.AddControl(IFDgoToMap, 0, 2);
                    }
                    {
                        BTNgoToMap = new MyButton("Go to map");
                        BTNgoToMap.Click += delegate
                          {
                              try
                              {
                                  string[] s = IFDgoToMap.GetField("X,Y").Split(',');
                                  MyTask.PublicGoToMap(new Point(int.Parse(s[0]), int.Parse(s[1])));
                              }
                              catch (Exception error)
                              {
                                  MessageBox.Show($"Error:\r\n{error}");
                              }
                          };
                        TLPctrl.AddControl(BTNgoToMap, 0, 3);
                    }
                    TLPmain.AddControl(TLPctrl, 3, 0);
                }
                this.Controls.Add(TLPmain);
                {
                    Thread thread = new Thread(() =>
                    {
                        Func<Point> f = new Func<Point>(() =>
                        {
                            Point answer = MyTask.Locate();
                            return answer;
                        });
                        while (true)
                        {
                            if (CHBshowCursorLocation.Checked)
                            {
                                Point a = f();
                                if (a != MyBitmap.failedPoint)
                                {
                                    a.X = MyCursor.Position.X - a.X;
                                    a.Y = MyCursor.Position.Y - a.Y;
                                }
                                Do(() => { this.Text = $"Cursor = {a}"; });
                            }
                            Thread.Sleep(100);
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }
        private int lengthOfNotSentText = 0;
        MyTableLayoutPanel TLPmain, TLPbtn,TLPctrl;
        MyLabel LBLstatus;
        MyTextBox TXBlog;
        MyButton BTNstart, BTNstop, BTNxButton,BTNrunManual,BTNshowNextRunTime,BTNshowTaskInfo,BTNclick,BTNgoToMap;
        MyButton[] BTNs { get { return new MyButton[] { BTNstart, BTNstop, BTNxButton, BTNrunManual, BTNshowNextRunTime, BTNshowTaskInfo, BTNclick, BTNgoToMap }; } }
        MyCheckBox CHBshowCursorLocation;
        MyInputField IFDclick, IFDgoToMap;
        public Form1()
        {
            this.Shown += Form1_Shown;
            this.FormClosing += Form1_FormClosing;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            NextRunTimeDataSaver.Save();
            Process.GetCurrentProcess().Kill();
        }
    }
}
