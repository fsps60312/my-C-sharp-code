using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class GeocryologyInstituteTask : ContractTask
    {
        protected override bool enabled { get { return false; } }
        public GeocryologyInstituteTask():base("Geocryology Institute", new TimeSpan(15,0,0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(732, 121), new Point(310, 220));
            orderContract.Add(new Point(725, 234));
            orderContract.Add(new Point(424, 238));
        }
    }
}
