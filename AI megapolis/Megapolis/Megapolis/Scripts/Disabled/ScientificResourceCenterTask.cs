using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class ScientificResourceCenterTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public ScientificResourceCenterTask():base("Scientific Resource Center",new TimeSpan(9,0,0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(486, 61), new Point(406, 232));
            orderContract.Add(new Point(725, 236));
            orderContract.Add(new Point(306, 242));
        }
    }
}
