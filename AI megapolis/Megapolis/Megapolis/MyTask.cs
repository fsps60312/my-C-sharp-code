using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Motivation;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Megapolis
{
    class Vector
    {
        public int X, Y;
        public Vector(int _X, int _Y) { X = _X; Y = _Y; }
        public static Point operator +(Point a, Vector b) { return new Point(a.X + b.X, a.Y + b.Y); }
        public static Vector operator -(Vector a, Vector b) { return new Vector(a.X - b.X, a.Y - b.Y); }
    }
    abstract class MyTask
    {
        protected virtual bool enabled { get { return true; } }
        public override string ToString()
        {
            return name;
        }
        private const int findImageHuristicDataCountLimit = 20;
        #region publics
        public static void PublicClick(Point p)
        {
            temporaryMyTaskInstance.Click(p);
        }
        public static void PublicGoToMap(Point p)
        {
            temporaryMyTaskInstance.GoToMap(p);
        }
        public static bool PublicIsBlueStacksStarted()
        {
            return IsBlueStacksStarted();
        }
        public void PublicCloseBlueStacks()
        {
            CloseBlueStacks();
        }
        private static Point locateHuristicData = failedPoint;
        public static Point Locate()
        {
            MyScreen.TurnOn();
            if (stopping) throw new Exception("stopping");
            Point answer = failedPoint;
            if (locateHuristicData != failedPoint)
            {
                Bitmap bmp = MyScreen.CaptureScreen();
                if (IsMatch(bmp, Properties.Resources.controlBoxesOfBlueStacks, locateHuristicData))
                {
                    answer = locateHuristicData;
                }
                bmp.Dispose();
            }
            if (answer == failedPoint) answer = MyScreen.FindImage(Properties.Resources.controlBoxesOfBlueStacks);
            //if (answer == failedPoint) MyScreen.FindImage(Properties.Resources.controlBoxesOfBlueStacks1);
            if (answer == failedPoint) return answer;
            else
            {
                locateHuristicData = answer;
                return answer + vectorFromControlBoxesToCaptureArea;
            }
        }
        #endregion
        #region tasks level 2
        protected void GoToMap(Point p)
        {
            OpenMap();
            if (p.X>0)
            {
                GoToLeftOnMap(p);
            }
            else
            {
                if (p.X == 0) throw new Exception($"Map's X value can't be 0: p={p}");
                p.X *= -1;
                GoToRightOnMap(p);
            }
            Thread.Sleep(500);
            Click(p);
            log = "Waiting for map to close...";
            while (!IsMapIconOnTheScene()) Thread.Sleep(500);
            log = "Map closed";
            CloseWindows();
        }
        private void GoToLeftOnMap(Point p)
        {
            Drag(new Point(captureSize.Width / 6, captureSize.Height / 2), new Point(5 * captureSize.Width / 6, captureSize.Height / 2), 1000);
        }
        private void GoToRightOnMap(Point p)
        {
            Drag(new Point(5 * captureSize.Width / 6, captureSize.Height / 2), new Point(captureSize.Width / 6, captureSize.Height / 2),  1000);
        }
        private static Point goToAndClickHuristicData = failedPoint;
        protected void GoToAndClick(KeyValuePair<Point, Point> p)
        {
            //if (goToAndClickHuristicData != p.Key)
            //{
            GoToMap(p.Key);
            //    goToAndClickHuristicData = p.Key;
            //}
            //else
            //{
            //    CloseWindows();
            //}
            Click(p.Value);
        }
        protected bool ClickToOpenWindow(Point p, Action toDoAfterOpenWindow)
        {
            Click(p);
            if(!WaitForWindowToOpen())
            {
                log = "The window didn't open, retrying...";
                CloseWindows();
                Click(p);
                if(!WaitForWindowToOpen())
                {
                    log = "The window still didn't open, ignored";
                    return false;
                }
            }
            toDoAfterOpenWindow.Invoke();
            return true;
        }
        protected bool DoubleClickToOpenWindow(KeyValuePair<Point,Point>p,Action toDoAfterOpenWindow)
        {
            Click(p.Value);
            Click(p.Value);
            if(!WaitForWindowToOpen())
            {
                log = "The window didn't open, retrying...";
                GoToAndClick(p);
                Click(p.Value);
                if(!WaitForWindowToOpen())
                {
                    log = "The window still didn't open, ignored";
                    return false;
                }
            }
            toDoAfterOpenWindow.Invoke();
            return true;
        }
        #endregion
        #region tasks level 1
        public void SaveCaptureFromImageFile(Bitmap bmp, string fileName)
        {
            Point p = MyBitmap.FindImage(bmp, Properties.Resources.controlBoxesOfBlueStacks);
            //for(int i=0;i<bmp.Height; i++)for(int j=0;j<bmp.Width; j++)
            //    {
            //        Color c = bmp.GetPixel(j,i);
            //        if (c.A != 255) MessageBox.Show($"bmp.pixel({i},{j})={c}");
            //    }
            //{
            //    Bitmap bm = Properties.Resources.controlBoxesOfBlueStacks;
            //for (int i = 0; i < bm.Height; i++) for (int j = 0; j < bm.Width; j++)
            //        {
            //            Color c = bm.GetPixel(j, i);
            //            if (c.A != 255) MessageBox.Show($"bm.pixel({i},{j})={c}");
            //        }
            //}
            if (p == failedPoint)
            {
                log = "Location failed";
                throw new Exception("Location failed");
            }
            else log = "Location succeed";
            p += vectorFromControlBoxesToCaptureArea;
            MyBitmap.Capture(bmp, new Rectangle(p, captureSize)).Save(fileName);
            log = $"Saved as {fileName}";
        }
        protected void OpenMap()
        {
            CloseWindows();
            ClickImage(Properties.Resources.mapIconInMegapolis);
            log = "Waiting for map to open...";
            while (!IsMapOpened()) Thread.Sleep(100);
            log = "Map opened";
        }
        protected void CloseWindows()
        {
            Point p;
            DateTime disappearTime;
            index_retry:;
            log = "Closing windows...";
            disappearTime = DateTime.Now;
            while ((DateTime.Now - disappearTime).TotalSeconds < 2.0)
            {
                if ((p = FindXButton()) != failedPoint)
                {
                    Click(p);
                    disappearTime = DateTime.Now;
                }
                Thread.Sleep(100);
            }
            if (FindImage(Properties.Resources.mapIconInMegapolis) == failedPoint)
            {
                //string fileName = $"failCloseWindows{Constant.random.Next(0, 100)}.png";
                //Capture().Save(fileName);
                SaveErrorCapture("CloseWindows");
                log = $"Failed to close all the windows, retrying...";
                goto index_retry;
            }
            log = "Windows closed";
        }
        protected Point FindXButton()
        {
            Bitmap bmp = Capture();
            Point answer = XButton.GetXButtonLocation(bmp);
            bmp.Dispose();
            return answer;
        }
        #endregion
        #region tasks level 0
        protected bool StartBlueStacksIfDidnt()
        {
            MyScreen.TurnOn();
            log = "Checking if BlueStacks is started...";
            if (IsBlueStacksStarted()) return true;
            Process.Start(Constant.BlueStacksExeFileFullName);
            log = "Waiting for BlueStacks to start...";
            for (DateTime startTime=DateTime.Now; !IsBlueStacksStarted();)
            {
                if((DateTime.Now-startTime).TotalMilliseconds>Constant.MaxTimeForStartingBlueStacksOrMegapolis)
                {
                    log = "Failed to start BlueStacks: Time out";
                    SaveErrorCapture("StartBlueStacksIfDidnt-timeOut");
                    return false;
                }
                Thread.Sleep(500);
            }
            log = "BlueStacks has started";
            return true;
        }
        protected void CloseBlueStacks()
        {
            if (IsBlueStacksStarted())
            {
                if (IsMegapolisStarted())
                {
                    MegapolisSaveProgress();
                    CloseWindows();
                }
                log = "Closing BlueStacks...";
                //string msg = "";
                foreach(var p in Process.GetProcesses())
                {
                    try
                    {
                        //msg += p.MainModule.FileVersionInfo.FileDescription + "\r\n";
                        if (p.MainModule.FileVersionInfo.FileDescription.ToLower().IndexOf("bluestacks") != -1) p.Kill();
                    }
                    catch (Exception) { }
                }
                //MessageBox.Show(msg);
                //Click(new Point(captureSize.Width, -10));
                while (IsBlueStacksStarted())
                {
                    Thread.Sleep(1000);
                }
                log = "BlueStacks closed";
            }
            else
            {
                log = "BlueStacks isn't running";
            }
            MyScreen.TurnOff();
        }
        protected bool StartMegapolisIfDidnt()
        {
            log = "Checking if Megapolis is started...";
            if (!IsMegapolisStarted())
            {
                log = "Starting Megapolis...";
                for(DateTime startTime = DateTime.Now; !IsMegapolisStarted();)
                {
                    if ((DateTime.Now - startTime).TotalMilliseconds > Constant.MaxTimeForStartingBlueStacksOrMegapolis)
                    {
                        log = "Failed to start Megapolis: Time out at loop 1";
                        SaveErrorCapture("StartMegapolisIfDidnt-timeOutAtLoop1");
                        return false;
                    }
                    ClickIfMatch(Properties.Resources.allAppButtonInBlueStacks, new Point(400, 417));
                    ClickIfMatch(Properties.Resources.megapolisIconInBlueStacks, new Point(752, 35));
                    CheckAndCloseAdsInBlueStacksAppearedWhenOpenMegapolis();
                    Thread.Sleep(250);
                }
            }
            log = "Megapolis has started";
            log = "Waiting for megapolis to load...";
            for (DateTime startTime = DateTime.Now; !IsMapIconOnTheScene() && !IsMapOpened();)
            {
                if ((DateTime.Now - startTime).TotalMilliseconds > Constant.MaxTimeForStartingBlueStacksOrMegapolis)
                {
                    log = "Failed to start Megapolis: Time out at loop 2";
                    SaveErrorCapture("StartMegapolisIfDidnt-timeOutAtLoop2");
                    return false;
                }
                CheckAndCloseAdsInBlueStacksAppearedWhenOpenMegapolis();
                Thread.Sleep(250);
            }
            //log = "Waiting for 5 seconds to let window appear...";
            //Thread.Sleep(5000);
            log = "Megapolis is ready!";
            return true;
        }
        protected void CheckAndCloseAdsInBlueStacksAppearedWhenOpenMegapolis()
        {
            if (AdsInBlueStacksWhenOpenMegapolisAppeared()) ClickImage(Properties.Resources.adWhenOpenMegapolisInBlueStacks);
        }
        protected bool AdsInBlueStacksWhenOpenMegapolisAppeared()
        {
            return IsMatch(Properties.Resources.adWhenOpenMegapolisInBlueStacks, new Point(650-60, 359-35));
        }
        protected bool IsMapOpened()
        {
            return IsMatch(Properties.Resources.mapXButtonInMegapolis, new Point(821, 6));
        }
        protected bool IsMapIconOnTheScene()
        {
            return FindImage(Properties.Resources.darkenMapIconInMegapolis) != failedPoint || FindImage(Properties.Resources.mapIconInMegapolis) != failedPoint;
        }
        protected void ShowImage(Bitmap bmp)
        {
            log = "Showing image...";
            Form f = new Form();
            f.AutoSize = true;
            f.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            {
                PictureBox p = new PictureBox();
                p.Dock = DockStyle.Fill;
                p.SizeMode = PictureBoxSizeMode.AutoSize;
                p.Image = bmp;
                f.Controls.Add(p);
            }
            f.Show();
            log = "Image shown";
        }
        protected void ShowCapture() { ShowImage(Capture()); }
        protected void Drag(Point f,Point t,int miliseconds)
        {
            if (stopping) throw new Exception("stopping");
            log = $"Dragging from {f} to {t} in {miliseconds} ms...";
            Point b = Locate();
            f.X += b.X;t.X += b.X;
            f.Y += b.Y;t.Y += b.Y;
            MyCursor.Position = f;
            Thread.Sleep(500);
            MyCursor.LeftDown();
            Thread.Sleep(500);
            for (int i=0;i<miliseconds;i++)
            {
                MyCursor.Position = new Point((f.X * (miliseconds - i) + t.X * i) / miliseconds, (f.Y * (miliseconds - i) + t.Y * i) / miliseconds);
                Thread.Sleep(1);
            }
            MyCursor.Position = t;
            Thread.Sleep(500);
            MyCursor.LeftUp();
            Thread.Sleep(500);
            log = "Dragging completed";
        }
        protected void Click(Point p)
        {
            if (stopping) throw new Exception("stopping");
            Point b = Locate();
            MyCursor.LeftClick(new Point(b.X + p.X, b.Y + p.Y));
            log = $"Mouse clicked on {p}";
            Thread.Sleep(100);
        }
        protected Point MiddleOfImage(Bitmap bmp,Point p)
        {
            return new Point(p.X + bmp.Width / 2, p.Y + bmp.Height / 2);
        }
        protected bool ClickIfMatch(Bitmap bmp,Point p)
        {
            if (IsMatch(bmp, p))
            {
                Click(MiddleOfImage(bmp, p));
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void WaitClick(Bitmap bmp,Point p)
        {
            log = "Waiting for image to appear...";
            while (!ClickIfMatch(bmp, p)) Thread.Sleep(100);
        }
        protected void ClickImage(Bitmap bmp,bool ignoreIfImageDidntFind=false)
        {
            Point p = FindImage(bmp);
            if (p == failedPoint)
            {
                if (ignoreIfImageDidntFind) return;
                ShowImage(Capture());
                ShowImage(bmp);
                log = "Failed to find image to click";
                throw new Exception("failed to find image to click");
            }
            Click(new Point(p.X + bmp.Width / 2, p.Y + bmp.Height / 2));
        }
        protected void WaitClick(Bitmap bmp, Action whatToDoWhenIdling = null)
        {
            Point p;
            log = "Waiting image to appear...";
            hideLog = true;
            while ((p = FindImage(bmp)) == failedPoint)
            {
                whatToDoWhenIdling?.Invoke();
                Thread.Sleep(100);
            }
            hideLog = false;
            do
            {
                Click(new Point(p.X + bmp.Width / 2, p.Y + bmp.Height / 2));
                Thread.Sleep(100);
            } while ((p = FindImage(bmp)) != failedPoint);
        }
        protected Bitmap Capture()
        {
            //return MyScreen.CaptureScreen();
            Point p = Locate();
            if (p == failedPoint)
            {
                log = "Capture location failed";
                //ShowImage(MyScreen.CaptureScreen());
                return MyScreen.CaptureScreen();
                //throw new Exception("capture location failed");
            }
            Bitmap src = MyScreen.CaptureScreen();
            Bitmap ans = MyBitmap.Capture(src, new Rectangle(p, captureSize));
            src.Dispose();
            return ans;
        }
        protected bool IsMatch(Bitmap bmp, Point p)
        {
            if (stopping) throw new Exception("stopping");
            Bitmap src = Capture();
            bool answer = IsMatch(src, bmp, p);
            src.Dispose();
            return answer;
        }
        private static List<Point> findImageHuristicData = new List<Point>();
        protected Point FindImage(Bitmap bmp)
        {
            if (stopping) throw new Exception("stopping");
            Bitmap src = Capture();
            Point answer = failedPoint;
            foreach (Point p in findImageHuristicData)
            {
                if (p.X + bmp.Width <= src.Width && p.Y + bmp.Height <= src.Height && IsMatch(src, bmp, p))
                {
                    answer = p;
                    //status = $">Huristic succeed: Find Image at {answer}";
                    findImageHuristicData.Remove(p);
                    goto index_returnAnswer;
                }
            }
            answer = MyBitmap.FindImage(src, bmp);
            index_returnAnswer:;
            src.Dispose();
            if (answer != failedPoint)
            {
                findImageHuristicData.Insert(0, answer);
                while (findImageHuristicData.Count > findImageHuristicDataCountLimit) findImageHuristicData.RemoveAt(findImageHuristicData.Count - 1);
            }
            return answer;
        }
        private bool WaitForWindowToOpen()
        {
            log = "Waiting for the window to open...";
            for (int i = 0; FindImage(Properties.Resources.darkenMapIconInMegapolis) == failedPoint; i += 500)
            {
                if (i >= 5000)
                {
                    log = "The window didn't open";
                    SaveErrorCapture($"WaitForWindowToOpen");
                    return false;
                    //throw new Exception("The window didn't open");
                }
            }
            return true;
        }
        private bool IsMegapolisStarted()
        {
            //log = "Checking if megapolis is started...";
            Point p = Locate();
            return MyScreen.IsMatch(Properties.Resources.megapolisTabInBlueStacks, new Point(p.X+(1253-(1063+1)),p.Y+(25-(48+1))));
            //Point answer = MyScreen.FindImage(Properties.Resources.megapolisTabInBlueStacks);
            //if (answer != failedPoint) MessageBox.Show($"hi:{answer} locate:{Locate()}");
            //return answer != failedPoint;
        }
        private static bool IsBlueStacksStarted()
        {
            MyCursor.Position = new Point(0, 0);
            return Locate() != failedPoint;
        }
        private static bool IsMatch(Bitmap src, Bitmap bmp, Point p)
        {
            if (stopping) throw new Exception("stopping");
            return MyBitmap.IsMatch(src, bmp, p);
        }
        private void MegapolisSaveProgress()
        {
            log = "Saving progress...";
            CloseWindows();
            for (DateTime startTime=DateTime.Now;;)
            {
                if((DateTime.Now - startTime).TotalMilliseconds > Constant.MaxTimeToTryToOpenSettings)
                {
                    log = $"Tried to open settings for {Constant.MaxTimeToTryToOpenSettings / 1000} seconds but in vain";
                    return;
                }
                if (ClickIfMatch(Properties.Resources.settingButtonInMegapolis, new Point(804, 390)) && WaitForWindowToOpen()) break;
                else
                {
                    log = "Failed to open settings, retrying...";
                }
                Thread.Sleep(100);
            }
            Click(new Point(517, 91));
            while (!IsMatch(Properties.Resources.savedButtonInSettingsInMegapolis, new Point(504, 84))) Thread.Sleep(100);
            log = "Progress saved";
        }
        private void SaveErrorCapture(string type)
        {
            Capture().Save($"error-{name}-{type}{Constant.random.Next(0, 100)}.png");
        }
        #endregion
        public virtual void Stop()
        {
            stopping = true;
            log = "Stopping...";
            while (!stopped) Thread.Sleep(100);
            log = "Stopped";
        }
        protected KeyValuePair<DateTime,string>NextTaskToRun()
        {
            KeyValuePair<DateTime, string> answer = new KeyValuePair<DateTime, string>(DateTime.MaxValue, "(None)");
            foreach (MyTask t in tasks)
            {
                if (t.nextRunTime < answer.Key)
                {
                    answer = new KeyValuePair<DateTime, string>(t.nextRunTime, t.name);
                }
            }
            return answer;
        }
        protected void RunExpiredScripts(ref HashSet<string>errorScripts)
        {
            List<int> runable = new List<int>();
            for (int i = 0; i < tasks.Count; i++) if (tasks[i].nextRunTime <= DateTime.Now) runable.Add(i);
            if (runable.Count > 0)
            {
                {
                    StringBuilder msg = new StringBuilder();
                    foreach (int i in runable) msg.Append(tasks[i].name + ", ");
                    msg.Length -= 2;
                    log = $"{runable.Count} tasks will run:\r\n{msg}";
                }
                List<int> errorList = new List<int>();
                for(int i=0;i<runable.Count;i++)
                {
                    bool timeOuted = false;
                    try
                    {
                        log = $"Running...({i + 1}/{runable.Count}): {tasks[runable[i]].name}";
                        {
                            bool taskFinished;
                            Thread thread;
                            //index_retryDueToTimeOut:;
                            taskFinished = false;
                            Exception error = null;
                            thread = new Thread(() =>
                            {
                                try
                                {
                                    tasks[runable[i]].RunScript();
                                    CloseWindows();
                                    tasks[runable[i]].nextRunTime = DateTime.Now + tasks[runable[i]].timeSpan;
                                }
                                catch(Exception error1)
                                {
                                    error = error1;
                                }
                                taskFinished = true;
                            });
                            thread.IsBackground = true;
                            thread.Start();
                            for (DateTime startTime = DateTime.Now; !taskFinished && (DateTime.Now - startTime).TotalMilliseconds < Constant.MaxAllowableTimeToRunOneSingleScript;) Thread.Sleep(100);
                            if (!taskFinished)
                            {
                                thread.Abort();
                                //log = $"\"\" aborted due to time out, retrying...";
                                //goto index_retryDueToTimeOut;
                                timeOuted = true;
                                SaveErrorCapture("RunExpiredScripts-TimeOut");
                                throw new Exception($"Time out: \"{tasks[runable[i]].name}\"");
                            }
                            else
                            {
                                if (error != null) throw error;
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        log = $"Catch error when running: {tasks[runable[i]].name}\r\n{error}";
                        errorList.Add(runable[i]);
                        if(timeOuted)
                        {
                            log = $"A task timed out, restarting BlusStacks, quiting the loop and retrying...";
                            CloseBlueStacks();
                            break;
                        }
                    }
                    if (stopping)
                    {
                        log = $"Abrupted: {i} of {runable.Count} finished";
                        break;
                    }
                }
                if (errorList.Count == 0)
                {
                    log = $"All {runable.Count} tasks ran successfully!";
                }
                else
                {
                    StringBuilder msg = new StringBuilder();
                    foreach (int i in errorList)
                    {
                        msg.Append(tasks[i].name + ", ");
                        errorScripts.Add(tasks[i].name);
                    }
                    msg.Length -= 2;
                    log = $"{errorList.Count} tasks ran into error:\r\n{msg}";
                }
            }
            else throw new Exception("No task to run");
        }
        public abstract void RunScript();
        protected virtual void Add(MyTask task)
        {
            task.LogChanged += delegate (string _status) { OnLogChanged(">" + _status); };
            tasks.Add(task);
        }
        public MyTask(string _name, TimeSpan _timeSpan)
        {
            name = _name;
            timeSpan = _timeSpan;
            if (nextRunTime==DateTime.MinValue)nextRunTime = DateTime.Now + timeSpan;
            if (!enabled) nextRunTime = DateTime.MaxValue;
        }
        internal DateTime nextRunTime
        {
            get
            {
                if (NextRunTimeDataSaver.nextRunTime.ContainsKey(name)) return NextRunTimeDataSaver.nextRunTime[name];
                else return DateTime.MinValue;
            }
            set { NextRunTimeDataSaver.nextRunTime[name] = value; }
        }
        internal TimeSpan timeSpan;
        protected string name;
        #region variables
        protected List<MyTask> tasks = new List<MyTask>();
        protected bool hideLog = false;
        public static bool stopping = false, stopped = false;
        protected static Point failedPoint { get { return MyBitmap.failedPoint; } }
        private static EmptyTask temporaryMyTaskInstance = new EmptyTask();
        private static Size captureSize { get { return Constant.MyTaskCaptureSize; } }
        private static Vector vectorFromControlBoxesToCaptureArea { get { return Constant.MyTaskVectorFromControlBoxesToCaptureArea; } }
        #endregion
        #region log change event
        public delegate void LogChangedEventHandler(string log);
        public LogChangedEventHandler LogChanged;
        private void OnLogChanged(string log) { if (!hideLog) LogChanged?.Invoke(log); }
        protected string log { set { OnLogChanged(name + ": " + value); } }
        #endregion
    }
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
        public static bool IsWin64Emulator(this Process process)
        {
            if ((Environment.OSVersion.Version.Major > 5)
                || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
            {
                bool retVal;

                return NativeMethods.IsWow64Process(process.Handle, out retVal) && retVal;
            }

            return false; // not on 64-bit Windows Emulator
        }
    }
}