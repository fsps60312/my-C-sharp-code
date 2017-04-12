using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class MilitaryAircraftFactoryTask:ProduceArmsTask
    {
        public MilitaryAircraftFactoryTask():base("Military Aircraft Factory",new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(6,10,0),new TimeSpan(2,30,0)))
        {
            buildingLocation = new KeyValuePair<Point, Point>(new Point(-474, 344), new Point(454, 214));
            itemList.Add(new KeyValuePair<int, int>(0, 5));
            itemList.Add(new KeyValuePair<int, int>(4, 2));
            itemList.Add(new KeyValuePair<int, int>(1, 4));
            itemList.Add(new KeyValuePair<int, int>(2, 3));
            itemList.Add(new KeyValuePair<int, int>(3, 1));
        }
    }
}
