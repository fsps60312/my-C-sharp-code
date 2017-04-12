using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class TourAdministrationTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public TourAdministrationTask():base("Tour Administration", new TimeSpan(11,0,0))
        {
            locations.Add(new KeyValuePair<Point, Point>(new Point(-495, 202), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-486, 196), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-490, 190), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-502, 188), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-510, 192), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-496, 194), new Point(425, 240)));
            contractBuilding = new KeyValuePair<Point, Point>(new Point(-506, 196), new Point(425, 240));
            orderContract.Add(new Point(725, 235));
            orderContract.Add(new Point(425, 242));
        }
    }
}
