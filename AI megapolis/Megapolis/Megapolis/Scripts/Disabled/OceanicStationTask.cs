using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class OceanicStationTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public OceanicStationTask() : base("Oceanic Station", new TimeSpan(12, 0, 0))
        {
            locations.Add(new KeyValuePair<Point, Point>(new Point(402, 299), new Point(402, 299)));
            contractBuilding = new KeyValuePair<Point, Point>(new Point(393, 316), new Point(331, 241));
            orderContract.Add(new Point(726, 311));
            orderContract.Add(new Point(307, 316));
        }
    }
}
