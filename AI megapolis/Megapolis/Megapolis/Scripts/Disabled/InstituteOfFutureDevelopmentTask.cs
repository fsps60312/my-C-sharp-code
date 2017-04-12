using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class InstituteOfFutureDevelopmentTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public InstituteOfFutureDevelopmentTask():base("Institute Of Future Development",new TimeSpan(12,0,0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(-523, 253), new Point(460, 250));
            orderContract.Add(new Point(726, 237));
            orderContract.Add(new Point(421, 244));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-527, 264), new Point(335, 209)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-527, 264), new Point(651, 357)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-508, 263), new Point(308, 183)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-508, 263), new Point(571, 320)));
        }
    }
}
