using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class SpaceShipTask:AssemblingLineTask
    {
        protected override bool enabled { get { return false; } }
        public SpaceShipTask():base("Space Ship", new TimeSpan(21,0,0))
        {
            launchLocation = new KeyValuePair<Point, Point>(new Point(668, 75), new Point(718, 172));
            assemblyPlantLocation = new KeyValuePair<Point, Point>(new Point(688, 85), new Point(293, 250));
            locationOnMap = new Point(714, 98);
            contractLocations.Add(new Point(423, 94));
            contractLocations.Add(new Point(671, 121));
            contractLocations.Add(new Point(590, 261));
            contractLocations.Add(new Point(207, 216));
            orderContract.Add(new Point(618, 323));
        }
    }
}
