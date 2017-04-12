using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class CreativeProjectCenterTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public CreativeProjectCenterTask():base("Creative Project Center", new TimeSpan(10,0,0))
        {
            locations.Add(new KeyValuePair<Point, Point>(new Point(410, 352), new Point(805, 283)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(410, 352), new Point(119, 306)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(418, 368), new Point(231, 315)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(418, 368), new Point(531, 136)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(434, 367), new Point(378, 266)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(433, 358), new Point(668, 143)));
            contractBuilding = new KeyValuePair<Point, Point>(new Point(433, 358), new Point(334, 256));
            orderContract.Add(new Point(725, 237));
            orderContract.Add(new Point(428, 235));
        }
    }
}
