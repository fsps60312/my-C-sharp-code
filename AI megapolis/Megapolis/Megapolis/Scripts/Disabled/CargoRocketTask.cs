using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class CargoRocketTask:AssemblingLineTask
    {
        protected override bool enabled { get { return false; } }
        public CargoRocketTask():base("Cargo Rocket",new TimeSpan(5,0,0))
        {
            launchLocation = new KeyValuePair<Point, Point>(new Point(607, 106), new Point(507, 256));
            assemblyPlantLocation = new KeyValuePair<Point, Point>(new Point(630, 118), new Point(559, 177));
            locationOnMap = new Point(653, 131);
            contractLocations.Add(new Point(407, 250));
            contractLocations.Add(new Point(463, 128));
            orderContract.Add(new Point(624, 196));
        }
    }
}
