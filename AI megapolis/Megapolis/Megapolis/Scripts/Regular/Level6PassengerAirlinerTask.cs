using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class Level6PassengerAirlinerTask : MultiContractTask
    {
        const int correctionX = 0, correctionY = 5;
        const int borrectionX = 0, borrectionY = 0;
        public Level6PassengerAirlinerTask() : base("Level 6 Passenger Airliner", new TimeSpan(22, 0, 0))
        {
            locationOnMap = new Point(494 + correctionX, 251 + correctionY);
            contractLocations.Add(new Point(302 + borrectionX, 165 + borrectionY));
            contractLocations.Add(new Point(724 + borrectionX, 182 + borrectionY));
            contractLocations.Add(new Point(698 + borrectionX, 182 + borrectionY));
            contractLocations.Add(new Point(343 + borrectionX, 216 + borrectionY));
            contractLocations.Add(new Point(631 + borrectionX, 213 + borrectionY));
            contractLocations.Add(new Point(165 + borrectionX, 232 + borrectionY));
            contractLocations.Add(new Point(140 + borrectionX, 256 + borrectionY));
            contractLocations.Add(new Point(613 + borrectionX, 264 + borrectionY));
            contractLocations.Add(new Point(338 + borrectionX, 285 + borrectionY));
            contractLocations.Add(new Point(84 + borrectionX, 293 + borrectionY));
            orderContract.Add(new Point(615, 190));
        }
    }
}
