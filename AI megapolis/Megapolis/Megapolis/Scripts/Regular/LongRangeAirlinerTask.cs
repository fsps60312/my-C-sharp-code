using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Megapolis
{
    class LongRangeAirlinerTask:MultiContractTask
    {
        const int correctionX = 0, correctionY = 6;
        public LongRangeAirlinerTask():base("Long-Range Airliner",new TimeSpan(12,0,0))
        {
            locationOnMap = new Point(629 + correctionX, 218 + correctionY);
            orderContract.Add(new Point(422, 246));
            contractLocations.Add(new Point(205, 199));
            contractLocations.Add(new Point(316, 170));
            contractLocations.Add(new Point(349, 140));
            contractLocations.Add(new Point(459, 93));
            contractLocations.Add(new Point(546, 191));
            contractLocations.Add(new Point(601, 204));
            contractLocations.Add(new Point(785, 272));
            contractLocations.Add(new Point(716, 308));
            contractLocations.Add(new Point(645, 342));
            contractLocations.Add(new Point(597, 374));
        }
    }
}
