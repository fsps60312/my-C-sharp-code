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
using System.Drawing.Imaging;
using System.Threading;


namespace AI_play_Megapolis_in_BlueStacks
{
    enum ExecuteResult { Success,Finished, Error};
    static class Script
    {
        public static bool stopOnError = true;
        private static Point pointFail = new Point(-1, -1);
        static Script()
        {
            actions.Add(new Func<string>(() =>
            {
                Point p = getImageLocation(Properties.Resources.mapIcon);
                if (p == pointFail) return "Can't find buttonX";
                Cursor.Position = p;
                return null;
            }));
            var openMap = new Func<string>(() =>
              {
                  for (DateTime startTime = DateTime.Now; (DateTime.Now - startTime).TotalSeconds < 5;)
                  {
                      MyForm.setText(String.Format("Waiting...({0} s)", (DateTime.Now - startTime).TotalSeconds));
                      Point p;
                          //don't know how to check whether it's in map state
                          {
                          p = getImageLocation(Properties.Resources.mapIcon);
                          if (p != pointFail)
                          {
                              MyCursor.LeftClick(p);
                              MyForm.setText("openMap succeeded!");
                              return null;
                          }
                          else
                          {
                              p = getImageLocation(Properties.Resources.buttonX);
                              if (p != pointFail)
                              {
                                  MyCursor.LeftClick(p);
                                  startTime = DateTime.Now;
                              }
                          }
                      }
                  }
                  return "openMap: try 20 seconds but still failed";
              });
            actions.Add(openMap);
        }
        /*actions.Add(new Func<string>(() =>
        {
            if (!updateMapIconLocation()) return "Can't find mapIconLocation";
            Cursor.Position = mapIconLocation;
            return null;
        }));
        actions.Add(new Func<string>(() =>
        {
            for(int i=0; ;i++)
            {
                Thread.Sleep(2000);
                MyCursor.LeftClick();
            }
        }));*/
        private static Point getButtonXLocation()
        {
            Point ans = pointFail;
            using (Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
                    IntPtr dc1 = g.GetHdc();
                    g.ReleaseHdc(dc1);
                }
                var isBW = new Func<Color, bool>((Color c) => { return c.R == c.G && c.R == c.B; });
                var isR = new Func<Color, bool>((Color c) => { return c.R > c.G && c.R > c.B; });
                for(int j=0;j+10<bmp.Height;j++)
                {
                    for(int i=0;i+10<bmp.Width;i++)
                    {
                        Color c = bmp.GetPixel(i, j);
                        if (isBW(bmp.GetPixel(i, j)) && isBW(bmp.GetPixel(i, j + 10)) && isBW(bmp.GetPixel(i + 10, j)) && isBW(bmp.GetPixel(i + 10, j + 10)) && isBW(bmp.GetPixel(i + 5, j + 5))
                            && isR(bmp.GetPixel(i, j + 5)) && isR(bmp.GetPixel(i + 5, j)) && isR(bmp.GetPixel(i + 5, j + 10)) && isR(bmp.GetPixel(i + 10, j + 5)))
                        {
                            if (ans != pointFail) return pointFail;
                            else ans = new Point(i, j);
                        }
                    }
                }
            }
            return ans;
        }
        private static Point getImageLocation(Bitmap img)
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
                BitmapData id = img.LockBits(new Rectangle(new Point(0, 0), img.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                int progress = 0;
                for (int i = 0; i + img.Height <= bmp.Height; i++)
                {
                    int nxtProgress = (i == (bmp.Height - img.Height) ? 100 : i * 100 / (bmp.Height - img.Height));
                    if (nxtProgress != progress)
                    {
                        progress = nxtProgress;
                        MyForm.setText(String.Format("Scanning screen...({0}%)", progress));
                    }
                    for (int j = 0; j + img.Width <= bmp.Width; j++)
                    {
                        if (MyBitmap.isMatch(bd, id, new Point(j, i)))
                        {
                            bmp.UnlockBits(bd);
                            img.UnlockBits(id);
                            return new Point(j, i);
                        }
                    }
                }
                bmp.UnlockBits(bd);
                img.UnlockBits(id);
                return pointFail;
            }
        }
        public static ExecuteResult Execute()
        {
            if (executeId == actions.Count) return ExecuteResult.Finished;
            else
            {
                string result=actions[executeId++].Invoke();
                if(result==null)return ExecuteResult.Success;
                else
                {
                    MessageBox.Show("Error: "+result);
                    return stopOnError ? ExecuteResult.Error : ExecuteResult.Success;
                }
            }
        }
        public static void Restart()
        {
            executeId = 0;
        }
        private static int executeId = 0;
        private static List<Func<string>> actions = new List<Func<string>>();
    }
    public partial class MyForm : Form
    {
        private void setThread()
        {
            thread = () =>
              {
                  stopping = false;
                  Script.Restart();
                  Do(() => { this.Text = "Running"; });
                  bool finished = false;
                  while (!stopping)
                  {
                      ExecuteResult result = Script.Execute();
                      switch (result)
                      {
                          case ExecuteResult.Finished:
                              {
                                  finished = true;
                                  stopping = true;
                              }
                              break;
                          case ExecuteResult.Error:
                              {
                                  stopping = true;
                              }
                              break;
                          case ExecuteResult.Success:
                              break;
                          default:
                              {
                                  Debug.Assert(false, "Unknown ExecuteResult: " + result.ToString());
                              }
                              break;
                      }
                  }
                  Do(() => { BTNstart.Enabled = true; BTNstop.Enabled = false; });
                  Do(() => { this.Text = finished ? "Finished" : "Stopped"; });
              };
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            setThread();
            TLPmain = new MyTableLayoutPanel(2, 1, "PP", "P");
            {
                BTNstart = new MyButton("Start");
                {
                    BTNstart.Click += BTNstart_Click;
                    TLPmain.AddControl(BTNstart, 0, 0);
                }
                BTNstop = new MyButton("Stop");
                {
                    BTNstop.Enabled = false;
                    BTNstop.Click += BTNstop_Click;
                    TLPmain.AddControl(BTNstop, 1, 0);
                }
                this.Controls.Add(TLPmain);
            }
        }
        private void BTNstop_Click(object sender, EventArgs e)
        {
            Do(() => { BTNstop.Enabled = false; });
            stopping = true;
            Do(() => { this.Text = "Stopping..."; });
        }
        private void Start()
        {
            Thread t = new Thread(new ThreadStart(thread));
            t.IsBackground = true;
            t.Start();
        }
        private void BTNstart_Click(object sender, EventArgs e)
        {
            BTNstart.Enabled = false;
            BTNstop.Enabled = true;
            Start();
        }
        public MyForm()
        {
            SELF = this;
            this.Shown += Form1_Shown;
            this.Size = new Size(500, 150);
        }
        public static void setText(string txt)
        {
            staticDo(() => { SELF.Text = txt; });
        }
        private static void staticDo(Action a)
        {
            if (SELF.InvokeRequired) SELF.Invoke(a);
            else a.Invoke();
        }
        private void Do(Action a)
        {
            if (this.InvokeRequired) this.Invoke(a);
            else a.Invoke();
        }
        private Action thread;
        private bool stopping = false;
        private MyButton BTNstart, BTNstop;
        private MyTableLayoutPanel TLPmain;
        public static MyForm SELF;
    }
    /*class ExecuteResult:IComparable
    {
        public static ExecuteResult Success;
        public static ExecuteResult Finished;
        public static ExecuteResult Error;
        static ExecuteResult()
        {
            var er = default(ExecuteResult);
            er.type = 0;
            Success = er;
            er.type++;
            Finished = er;
            er.type++;
            Error = er;
        }
        public int type;
        public string msg;
        ExecuteResult(ExecuteResult er)
        {
            type = er.type;
            msg = null;
        }
        ExecuteResult(ExecuteResult er,string msg)
        {
            type = er.type;
            this.msg = msg;
        }
        public int CompareTo(object _o)
        {
            var o = _o as ExecuteResult;
            return type.CompareTo(o.type);
        }
    }*/
}