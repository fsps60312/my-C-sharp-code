using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class PrimeFinanceCenterTask:MultiContractTask
    {
        protected override bool enabled { get { return false; } }
        public PrimeFinanceCenterTask():base("Prime Finance Center",new TimeSpan(21,0,0))
        {
            locationOnMap = new Point(574, 257);
            contractLocations.Add(new Point(418, 110));
            orderContract.Add(new Point(424, 244));
        }
    }
}
