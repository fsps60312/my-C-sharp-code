using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class MilitaryShipyardTask:ProduceArmsTask
    {
        public MilitaryShipyardTask():base("Military Shipyard",new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(8,40,0),new TimeSpan(5,20,0)))
        {
            buildingLocation = new KeyValuePair<Point, Point>(new Point(591, 347), new Point(438, 257));
            itemList.Add(new KeyValuePair<int, int>(0, 5));
            itemList.Add(new KeyValuePair<int, int>(3, 0));
            itemList.Add(new KeyValuePair<int, int>(2, 3));
            itemList.Add(new KeyValuePair<int, int>(1, 2));
        }
    }
}
