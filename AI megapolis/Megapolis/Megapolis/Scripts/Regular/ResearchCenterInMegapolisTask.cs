using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class ResearchCenterInMegapolisTask : MultiContractTask
    {
        public ResearchCenterInMegapolisTask() : base("Research Center In Megapolis", new TimeSpan(22, 0, 0))
        {
            locationOnMap = new Point(664, 217 + 5);
            orderContract.Add(new Point(540, 248));
            for (int i = 3; i >= 0; i--) contractLocations.Add(new Point((584 * i + 388 * (3 - i)) / 3, (180 * i + 79 * (3 - i)) / 3 + 27));
            for (int i = 7; i >= 0; i--) contractLocations.Add(new Point((682 * i + 225 * (7 - i)) / 7, (293 * i + 66 * (7 - i)) / 7 + 27));
            for (int i = 7; i >= 0; i--) contractLocations.Add(new Point((617 * i + 158 * (7 - i)) / 7, (324 * i + 96 * (7 - i)) / 7 + 27));
        }
    }
}
