using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class GoldDragonCasinoTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public GoldDragonCasinoTask():base("Gold Dragon Casino",new TimeSpan(11,0,0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(167, 74), new Point(661, 283));
            orderContract.Add(new Point(726, 240));
            orderContract.Add(new Point(420, 241));
            locations.Add(new KeyValuePair<Point, Point>(new Point(167, 74), new Point(692, 140)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(167, 74), new Point(539, 77)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(167, 74), new Point(288, 100)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(167, 74), new Point(484, 194)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(167, 74), new Point(271, 267)));
        }
    }
}
