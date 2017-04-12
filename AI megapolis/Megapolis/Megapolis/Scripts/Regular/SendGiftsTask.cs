using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Megapolis
{
    class SendGiftsTask:MyTask
    {
        public override void RunScript()
        {
            CloseWindows();
            ClickToOpenWindow(new Point(25, 370), () =>
             {
                 Click(new Point(583, 106));
                 Thread.Sleep(1000);
                 Click(new Point(682, 380));
                 Thread.Sleep(2000);
                 DateTime startTime = DateTime.Now;
                 for (; (DateTime.Now - startTime).TotalMilliseconds <= 5000;)
                 {
                     if (ClickIfMatch(Properties.Resources.sendGiftButtonInMegapolis, new Point(224, 340))) startTime = DateTime.Now;
                     ClickIfMatch(Properties.Resources.okButtonAfterSendingGiftInMegapolis, new Point(413, 269));
                     Thread.Sleep(250);
                 }
             });
            //{
            //    log = "Failed to send gifts, ignored";
            //    return;
            //}
            //while (true)
            //{
            //    //if (FindImage(Properties.Resources.openSendGiftWindowButtonWhenNoGiftInMegapolis) != failedPoint) { log = "no gifts to send";return; }
            //    Click(new Point(25, 370));
            //    if (WaitForWindowToOpen()) break;
            //    else log = "Failed to open the window, retrying...";
            //}
        }
        public SendGiftsTask():base("Send Gifts",new TimeSpan(3,0,0))
        {
        }
    }
}
