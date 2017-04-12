using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class AtmosphericProbeTask:AssemblingLineTask
    {
        protected override bool enabled { get { return false; } }
        public AtmosphericProbeTask() : base("Atmospheric Probe", new TimeSpan(5, 0, 0))
        {
            launchLocation = new KeyValuePair<Point, Point>(new Point(331, 180), new Point(465, 267));
            assemblyPlantLocation = new KeyValuePair<Point, Point>(new Point(346, 190), new Point(380, 306));
            locationOnMap = new Point(366, 200);
            contractLocations.Add(new Point(384, 285));
            contractLocations.Add(new Point(549, 166));
            orderContract.Add(new Point(618, 323));
        }
    }
}
