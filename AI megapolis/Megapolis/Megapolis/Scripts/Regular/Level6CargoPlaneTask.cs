using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class Level6CargoPlaneTask:MultiContractTask
    {
        const int correctionX = 0, correctionY = 5;
        public Level6CargoPlaneTask():base("Level 6 Cargo Plane",new TimeSpan(22,0,0))
        {
            locationOnMap = new Point(512+correctionX, 252+correctionY);
            orderContract.Add(new Point(622, 191));
            contractLocations.Add(new Point(694, 284));
            contractLocations.Add(new Point(540, 307));
            contractLocations.Add(new Point(484, 366));
            contractLocations.Add(new Point(285, 262));
            contractLocations.Add(new Point(383, 214));
            contractLocations.Add(new Point(475, 175));
            contractLocations.Add(new Point(421, 70));
        }
    }
}
