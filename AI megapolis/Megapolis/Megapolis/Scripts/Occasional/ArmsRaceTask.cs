using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Megapolis
{
    class ArmsRaceTask:MyTask
    {
        //protected override bool enabled { get { return false; } }
        public override void RunScript()
        {
            for (int i = 0; ; i++)
            {
                var p = new KeyValuePair<Point, Point>(new Point(614, 204), new Point(437, 318));
                GoToMap(p.Key);
                DoubleClickToOpenWindow(p,()=>
                {
                    Click(new Point(354, 171));
                    Thread.Sleep(500);
                    if (i == 3) Click(new Point(527, 355));
                    else Click(new Point(585, 355));
                });
                if (i == 3) break;
            }
        }
        public ArmsRaceTask():base("Arms Race",new TimeSpan(0,15,0))
        {
            if (!enabled) nextRunTime = DateTime.MaxValue;
        }
    }
}
