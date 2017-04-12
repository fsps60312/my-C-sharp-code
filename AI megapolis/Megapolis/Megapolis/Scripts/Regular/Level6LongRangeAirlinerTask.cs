using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class Level6LongRangeAirlinerTask:MultiContractTask
    {
        const int correctionX = 0, correctionY = 5;
        const int borrectionX = -27, borrectionY = -18;
        public Level6LongRangeAirlinerTask():base("Level 6 Long-range Airliner",new TimeSpan(21,0,0))
        {
            locationOnMap = new Point(500 + correctionX, 261 + correctionY);
            orderContract.Add(new Point(619, 193));
            contractLocations.Add(new Point(777 + borrectionX, 153 + borrectionY));
            contractLocations.Add(new Point(673 + borrectionX, 195 + borrectionY));
            contractLocations.Add(new Point(672 + borrectionX, 267 + borrectionY));
            contractLocations.Add(new Point(136 + borrectionX, 186 + borrectionY));
            contractLocations.Add(new Point(248 + borrectionX, 233 + borrectionY));
            contractLocations.Add(new Point(336 + borrectionX, 278 + borrectionY));
            contractLocations.Add(new Point(434 + borrectionX, 331 + borrectionY));
        }
    }
}
