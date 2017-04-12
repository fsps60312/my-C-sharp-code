using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class ArchaeologicalResearchLabTask:ContractTask
    {
        public ArchaeologicalResearchLabTask():base("Archaeological Research Lab", new TimeSpan(12,0,0))
        {
            locations.Add(new KeyValuePair<Point, Point>(new Point(-535, 326), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-540, 315), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-547, 318), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-549, 326), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-536, 331), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-532, 321), new Point(425, 240)));
            contractBuilding = new KeyValuePair<Point, Point>(new Point(-521,327), new Point(515,279));
            orderContract.Add(new Point(724, 238));
            orderContract.Add(new Point(423, 245));
        }
    }
}
