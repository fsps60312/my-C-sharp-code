using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Imaging;

namespace Megapolis
{
    class DeleteInactiveFriendsTask:MyTask
    {
        protected override bool enabled { get { return false; } }
        private bool IsActiveFriendsSwitchOn()
        {
            bool a = false, b = false;
            DateTime startTime = DateTime.Now;
            do
            {
                Trace.Assert((DateTime.Now - startTime).TotalSeconds < 5.0);
                a = IsMatch(Properties.Resources.activatedActiveFriendsSwitchInNeighborsListInMegapolis, activeFriendsSwitchLocation);
                b = IsMatch(Properties.Resources.disactivatedActiveFriendsSwitchInNeighborsListInMegapolis, activeFriendsSwitchLocation);
            } while (!a && !b);
            Trace.Assert(a != b);
            return a;
        }
        private void SetActiveFriendsSwitch(bool targetState)
        {
            while(IsActiveFriendsSwitchOn()!=targetState)
            {
                Click(MiddleOfImage(Properties.Resources.activatedActiveFriendsSwitchInNeighborsListInMegapolis, activeFriendsSwitchLocation));
                Thread.Sleep(500);
            }
            //Thread.Sleep(1000);
            //Trace.Assert(IsActiveFriendsSwitchOn() == targetState);
        }
        private void ClickTrashCan(int index)//228,168 701,168
        {
            Point p = new Point((228 * (4 - index) + 701 * index) / 4, 168);
            //Motivation.MyCursor.Position = p;
            Click(p);
            WaitClick(Properties.Resources.deleteButtonInNeighborsListInMegapolis, deleteButtonLocation);
        }
        private int findDisactiveFriends()
        {
            SetActiveFriendsSwitch(true);
            Bitmap bmp1 = Capture();
            SetActiveFriendsSwitch(false);
            Bitmap bmp2 = Capture();
            Trace.Assert(bmp1.Size == bmp2.Size);
            BitmapData bd1 = bmp1.LockBits(captureRegion, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData bd2 = bmp2.LockBits(captureRegion, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int w = captureRegion.Width, h = captureRegion.Height;
            int answer = -1;
            unsafe
            {
                Trace.Assert(bd1.Stride == bd2.Stride);
                int stride = bd1.Stride;
                byte* p1 = (byte*)bd1.Scan0.ToPointer(), p2 = (byte*)bd2.Scan0.ToPointer();
                for (int x = 0; x < w && answer==-1; x++)
                {
                    for (int y = 0; y < h && answer==-1; y++)
                    {
                        int i = y * stride + x * 4;
                        if(false
                            ||p1[i+0]!=p2[i+0]
                            ||p1[i+1]!=p2[i+1]
                            ||p1[i+2]!=p2[i+2]
                            ||p1[i+3]!=p2[i+3])
                        {
                            //System.Windows.Forms.MessageBox.Show($"x:{x},y:{y}");
                            //ShowImage(bmp1);
                            //ShowImage(bmp2);
                            answer = (x + 10) * 5 / w;
                            Trace.Assert(0 <= answer && answer < 5);
                        }
                    }
                }
            }
            bmp1.UnlockBits(bd1);
            bmp2.UnlockBits(bd2);
            bmp1.Dispose();
            bmp2.Dispose();
            return answer;
        }
        public override void RunScript()
        {
            while (!IsMatch(Properties.Resources.neighborsIconInMegapolis, neighborIconLocation)) CloseWindows();
            Trace.Assert(ClickToOpenWindow(MiddleOfImage(Properties.Resources.neighborsIconInMegapolis, neighborIconLocation), new Action(() =>
            {
                do
                {
                    for(int missed=2;(missed--)>0;)
                    {
                        //clicked = false;
                        //int pre;
                        //if((pre=findDisactiveFriends())!=-1)
                        //{
                        //    bool really = true;
                        //    for(int i=0;i<3;i++)
                        //    {
                        //        if(findDisactiveFriends()!=pre)
                        //        {
                        //            really = false;
                        //            break;
                        //        }
                        //    }
                        //    if(really)
                        //    {
                        //        ClickTrashCan(pre);
                        //        clicked = true;
                        //    }
                        //}
                        int pre;
                        if((pre=findDisactiveFriends())!=-1)
                        {
                            ClickTrashCan(pre);
                            missed = 2;
                        }
                    }
                } while (ClickIfMatch(Properties.Resources.nextPageButtonInNeighborsListInMegapolis, nextPageButtonLocation));
            })));
        }
        public DeleteInactiveFriendsTask():base("Delete Inactive Friends",TimeSpan.MaxValue)
        {
            if (!enabled) nextRunTime = DateTime.MaxValue;
        }
        private static Point neighborIconLocation { get { return new Point(513, 410); } }
        private static Point activeFriendsSwitchLocation { get { return new Point(692,374); } }
        private static Point nextPageButtonLocation { get { return new Point(722, 229); } }
        private static Point deleteButtonLocation { get { return new Point(456, 311); } }
        private static Rectangle captureRegion { get { return new Rectangle(136, 155, 576, 38); } }
    }
}
