using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class NorthStationTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public NorthStationTask():base("North Station",new TimeSpan(6,0,0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(592, 190), new Point(521, 210));
            orderContract.Add(new Point(728, 243));
            orderContract.Add(new Point(191, 257));
            locations.Add(new KeyValuePair<Point, Point>(new Point(557, 183), new Point(439, 208)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(525, 204), new Point(475, 235)));
        }
    }
}
