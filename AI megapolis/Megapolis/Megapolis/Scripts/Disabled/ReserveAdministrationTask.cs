using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class ReserveAdministrationTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public ReserveAdministrationTask():base("Reserve Administration",new TimeSpan(11,0,0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(408, 158), new Point(553, 119));
            orderContract.Add(new Point(726, 238));
            orderContract.Add(new Point(430, 246));
            locations.Add(new KeyValuePair<Point, Point>(new Point(408, 158), new Point(311, 269)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(404, 142), new Point(257, 147)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(404, 142), new Point(603, 324)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(386, 143), new Point(579, 115)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(386, 143), new Point(383, 239)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(395, 151), new Point(401, 244)));
        }
    }
}
