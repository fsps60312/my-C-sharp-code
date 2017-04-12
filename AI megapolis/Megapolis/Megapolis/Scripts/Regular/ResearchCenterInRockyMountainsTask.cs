using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class ResearchCenterInRockyMountainsTask:MultiContractTask
    {
        public ResearchCenterInRockyMountainsTask():base("Research Center In Rocky Mountains",new TimeSpan(22,0,0))
        {
            locationOnMap = new Point(559, 65);
            orderContract.Add(new Point(540, 248));
            for (int i = 5; i >= 0; i--) contractLocations.Add(new Point((645 * i + 321 * (5 - i)) / 5, (306 * i + 146 * (5 - i)) / 5));
            for (int i = 7; i >= 0; i--) contractLocations.Add(new Point((582 * i + 125 * (7 - i)) / 7, (341 * i + 115 * (7 - i)) / 7));
        }
    }
}
