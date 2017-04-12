using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class TreasuryInMegapolisTask:CollectionTask
    {
        public TreasuryInMegapolisTask():base("Treasury In Megapolis",new TimeSpan(23,0,0))
        {
            locations.Add(new KeyValuePair<Point, Point>(new Point(626, 246), new Point(413, 230)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(698, 169), new Point(441, 252)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(656, 217), new Point(376, 231)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(734, 197), new Point(515, 271)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(754, 156), new Point(438,317)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(705, 132), new Point(418, 317)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(626, 202), new Point(475, 297)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(608, 151), new Point(555, 309)));
        }
    }
}
