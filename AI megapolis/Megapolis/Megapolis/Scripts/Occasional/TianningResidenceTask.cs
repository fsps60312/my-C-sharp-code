using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Megapolis
{
    class TianningResidenceTask:CollectionTask
    {
        protected override bool enabled { get { return false; } }
        public override void RunScript()
        {
            base.RunScript();
            log = "Waiting for 10 seconds...";
            Thread.Sleep(10000);
        }
        public TianningResidenceTask():base("Tianning Residence",new TimeSpan(1,0,0))
        {
            for(int i=0;i<=3;i++)
            {
                for(int j=0;j<=4;j++)
                {
                    locations.Add(new KeyValuePair<Point, Point>(new Point(634, 229),
                        new Point(((359 * (3 - i) + 162 * i) * (4 - j) + (619 * (3 - i) + 425 * i) * j) / (3 * 4)
                                , ((109 * (3 - i) + 206 * i) * (4 - j) + (242 * (3 - i) + 339 * i) * j) / (3 * 4))));//359, 109 162,206 | 619,242 425,339
                }
            }
        }
    }
}
