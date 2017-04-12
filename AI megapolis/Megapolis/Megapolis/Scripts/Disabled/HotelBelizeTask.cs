using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class HotelBelizeTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public HotelBelizeTask() : base("Hotel Belize", new TimeSpan(12, 0, 0))
        {
            locations.Add(new KeyValuePair<Point, Point>(new Point(508, 321), new Point(321, 323)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(508, 321), new Point(355, 190)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(508, 321), new Point(562, 232)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(513, 330), new Point(340, 149)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(513, 330), new Point(538, 274)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(523, 341), new Point(472, 307)));
            contractBuilding = new KeyValuePair<Point, Point>(new Point(523, 341), new Point(717, 222));
            orderContract.Add(new Point(725, 238));
            orderContract.Add(new Point(424, 242));
        }
    }
}
