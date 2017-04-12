using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class SubmarineFactoryTask:ProduceArmsTask
    {
        public SubmarineFactoryTask():base("Submarine Factory",new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(4,20,0),new TimeSpan(3,0,0)))
        {
            buildingLocation = new KeyValuePair<Point, Point>(new Point(325, 308), new Point(377, 241));
            itemList.Add(new KeyValuePair<int, int>(0, 5));
            itemList.Add(new KeyValuePair<int, int>(1, 4));
            itemList.Add(new KeyValuePair<int, int>(2, 2));
        }
    }
}
