using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class HoustonTask : MultiContractTask
    {
        const int correctionX = 0, correctionY = 5;
        public HoustonTask() : base("Houston", new TimeSpan(12, 0, 0))
        {
            locationOnMap = new Point(653 + correctionX, 224+correctionY);
            orderContract.Add(new Point(430, 247));
            contractLocations.Add(new Point(318, 151));
            contractLocations.Add(new Point(383, 188));
            contractLocations.Add(new Point(462, 213));
            contractLocations.Add(new Point(515, 249));
            contractLocations.Add(new Point(587, 281));
            contractLocations.Add(new Point(648, 312));
            contractLocations.Add(new Point(723, 350));
        }
    }
}
