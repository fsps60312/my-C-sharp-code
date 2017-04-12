using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class Level6BusinessJetVIPJetTask:MultiContractTask
    {
        const int correctionX = 0, correctionY = 5;
        public Level6BusinessJetVIPJetTask():base("Level 6 Business Jet & VIP Jet",new TimeSpan(20,0,0))
        {
            locationOnMap = new Point(501 + correctionX, 246 + correctionY);
            orderContract.Add(new Point(619, 193));
            contractLocations.Add(new Point(528, 124));
            contractLocations.Add(new Point(452, 170));
            contractLocations.Add(new Point(367, 227));
            contractLocations.Add(new Point(499, 236));
            contractLocations.Add(new Point(572, 306));
            contractLocations.Add(new Point(576, 361));
        }
    }
}
