using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class AviationDesignCompanyTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public AviationDesignCompanyTask() : base("Aviation Design Company", new TimeSpan(10, 0, 0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(-530, 111), new Point(357, 306));
            orderContract.Add(new Point(725, 236));
            orderContract.Add(new Point(427, 239));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-530, 111), new Point(291, 77)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-530, 111), new Point(655, 125)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-530, 111), new Point(698, 354)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-520, 110), new Point(244, 174)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-520, 110), new Point(427, 224)));
        }
    }
}
