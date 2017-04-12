using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class OrbitalShuttleTask:AssemblingLineTask
    {
        protected override bool enabled { get { return false; } }
        public OrbitalShuttleTask():base("Orbital Shuttle",new TimeSpan(19,30,0))
        {
            launchLocation = new KeyValuePair<Point, Point>(new Point(646, 86), new Point(712, 227));
            assemblyPlantLocation = new KeyValuePair<Point, Point>(new Point(666, 96), new Point(360, 275));
            locationOnMap = new Point(692, 108);
            contractLocations.Add(new Point(514, 133));
            contractLocations.Add(new Point(698, 140));
            contractLocations.Add(new Point(245, 218));
            contractLocations.Add(new Point(441, 260));
            orderContract.Add(new Point(618, 323));
        }
    }
}
