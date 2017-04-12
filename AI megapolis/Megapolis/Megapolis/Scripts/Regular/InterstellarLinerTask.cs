using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class InterstellarLinerTask:AssemblingLineTask
    {
        const int correctionX = 0, correctionY = 10;
        public InterstellarLinerTask():base("Interstellar Liner",new TimeSpan(22,0,0))
        {
            launchLocation = new KeyValuePair<Point, Point>(new Point(695+correctionX, 55+correctionY), new Point(492, 295));
            assemblyPlantLocation = new KeyValuePair<Point, Point>(new Point(719 + correctionX, 68 + correctionY), new Point(555, 217));
            locationOnMap = new Point(741 + correctionX, 83 + correctionY);
            contractLocations.Add(new Point(707, 225));
            contractLocations.Add(new Point(323, 167));
            contractLocations.Add(new Point(622, 334));
            contractLocations.Add(new Point(207, 281));
            orderContract.Add(new Point(618, 323));
        }
    }
}
