using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace Megapolis
{
    class TreasureChestTask:MyTask
    {
        protected override bool enabled { get { return false; } }
        public override void RunScript()
        {
            foreach (Point p in treasureChestButtonInMegapolisLocation)
            {
                CloseWindows();
                bool found = false;
                ClickToOpenWindow(p, new Action(() =>
                 {
                     Thread.Sleep(500);
                     if (ClickIfMatch(Properties.Resources.unlockForFreeButtonInTreasureChestInMegapolis, unlockForFreeButtonInTreasureChestInMegapolisLocation))
                     {
                         log = "Found chests!";
                         found = true;
                         //foreach (MyTask t in tasks) t.RunScript();
                         //CloseWindows();
                         //ClickToOpenWindow(p, new Action(() =>
                         //{
                         //    if (!ClickIfMatch(Properties.Resources.unlockForFreeButtonInTreasureChestInMegapolis, unlockForFreeButtonInTreasureChestInMegapolisLocation))
                         //    {
                         //        log = "Still no chests left!";
                         //    }
                         //}));
                     }
                     else
                     {
                         log = "No chests...";
                     }
                     Thread.Sleep(500);
                 }));
                if (found) break;
            }
        }
        public TreasureChestTask():base("Treasure Chest",new TimeSpan(1,0,0))
        {
            Add(new RailroadTask());
        }
        private Point[] treasureChestButtonInMegapolisLocation { get { return new Point[] { new Point(30, 143), new Point(30, 200) }; } }
        private Point unlockForFreeButtonInTreasureChestInMegapolisLocation { get { return new Point(299, 358); } }
    }
}
