using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class MannedRocketTask:AssemblingLineTask
    {
        protected override bool enabled { get { return false; } }
        public MannedRocketTask():base("Manned Rocket",new TimeSpan(19,0,0))
        {
            launchLocation = new KeyValuePair<Point, Point>(new Point(624, 97), new Point(627, 187));
            assemblyPlantLocation = new KeyValuePair<Point, Point>(new Point(649, 107), new Point(585, 153));
            locationOnMap = new Point(670, 120);
            contractLocations.Add(new Point(684, 129));
            contractLocations.Add(new Point(518, 144));
            contractLocations.Add(new Point(404, 220));
            contractLocations.Add(new Point(243, 266));
            orderContract.Add(new Point(618, 323));
        }
    }
}
