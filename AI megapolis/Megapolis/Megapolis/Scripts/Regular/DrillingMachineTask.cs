using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class DrillingMachineTask:AssemblingLineTask
    {
        const int correctionX = 0, correctionY = 5;
        public DrillingMachineTask():base("Drilling Machine",new TimeSpan(18,30,0))
        {
            launchLocation = new KeyValuePair<Point, Point>(new Point(416 + correctionX, 229 + correctionY), new Point(398, 288));
            assemblyPlantLocation = new KeyValuePair<Point, Point>(new Point(402 + correctionX, 207 + correctionY), new Point(535, 330));
            locationOnMap = new Point(377 + correctionX, 220 + correctionY);
            contractLocations.Add(new Point(435,158));
            contractLocations.Add(new Point(214,198));
            contractLocations.Add(new Point(657,257));
            contractLocations.Add(new Point(483,296));
            orderContract.Add(new Point(618, 323));
        }
    }
}
