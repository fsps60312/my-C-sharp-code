using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class ArmoredVehicleFactoryTask:ProduceArmsTask
    {
        public ArmoredVehicleFactoryTask():base("Armored Vehicle Factory",new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(4,10,0) ,new TimeSpan(0,50,0)))
        {
            buildingLocation = new KeyValuePair<Point, Point>(new Point(556, 137), new Point(482, 222));
            itemList.Add(new KeyValuePair<int, int>(7, 5));
            itemList.Add(new KeyValuePair<int, int>(6, 5));
            itemList.Add(new KeyValuePair<int, int>(5, 5));
            itemList.Add(new KeyValuePair<int, int>(4, 5));
            itemList.Add(new KeyValuePair<int, int>(3, 5));
            itemList.Add(new KeyValuePair<int, int>(2, 5));
            itemList.Add(new KeyValuePair<int, int>(1, 5));
            itemList.Add(new KeyValuePair<int, int>(0, 5));
        }
    }
}
