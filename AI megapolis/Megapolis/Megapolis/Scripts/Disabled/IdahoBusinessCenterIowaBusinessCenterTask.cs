using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class IdahoBusinessCenterIowaBusinessCenterTask:MultiContractTask
    {
        protected override bool enabled { get { return false; } }
        public IdahoBusinessCenterIowaBusinessCenterTask():base("Idaho Business Center & Iowa Business Center",new TimeSpan(23,0,0))
        {
            locationOnMap = new Point(574, 257);
            contractLocations.Add(new Point(160, 227));
            contractLocations.Add(new Point(538, 313));
            orderContract.Add(new Point(424, 244));
        }
    }
}
