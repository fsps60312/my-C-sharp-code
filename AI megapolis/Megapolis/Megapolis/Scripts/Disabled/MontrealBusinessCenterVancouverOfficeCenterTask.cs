using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class MontrealBusinessCenterVancouverOfficeCenterTask:MultiContractTask
    {
        protected override bool enabled { get { return false; } }
        public MontrealBusinessCenterVancouverOfficeCenterTask():base("Montreal Business Center & Vancouver Office Center",new TimeSpan(22,0,0))
        {
            locationOnMap = new Point(574, 257);
            contractLocations.Add(new Point(321, 303));
            contractLocations.Add(new Point(716, 212));
            orderContract.Add(new Point(424, 244));
        }
    }
}
