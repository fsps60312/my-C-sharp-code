using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class MissileConstructionFactoryTask:ProduceArmsTask
    {
        public MissileConstructionFactoryTask():base("Missile Construction Factory",new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(4,50,0),new TimeSpan(2,10,0)))
        {
            buildingLocation = new KeyValuePair<Point, Point>(new Point(382, 183), new Point(419,258));
            itemList.Add(new KeyValuePair<int, int>(1, 1));
            itemList.Add(new KeyValuePair<int, int>(0, 2));
        }
    }
}
