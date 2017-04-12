using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class DeepSubmergenceVehicleTask:AssemblingLineTask
    {
        const int correctionX=0,correctionY = 5;
        public DeepSubmergenceVehicleTask() : base("Deep-submergence Vehicle", new TimeSpan(18, 0, 0))
        {
            launchLocation = new KeyValuePair<Point, Point>(new Point(344+ correctionX, 234 + correctionY), new Point(613, 284));
            assemblyPlantLocation = new KeyValuePair<Point, Point>(new Point(363+ correctionX, 229 + correctionY), new Point(622, 239));
            locationOnMap = new Point(345+ correctionX, 222 + correctionY);
            contractLocations.Add(new Point(223, 268));
            contractLocations.Add(new Point(399, 190));
            contractLocations.Add(new Point(669, 210));
            contractLocations.Add(new Point(402, 334));
            orderContract.Add(new Point(618, 323));
        }
    }
}
