using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class RailroadTask:CollectionTask
    {
        const int correctionX = 0, correctionY = 5;
        public RailroadTask():base("Railroad",new TimeSpan(2,0,0))
        {
            locations.Add(new KeyValuePair<Point, Point>(new Point(537 + correctionX, 274 + correctionY), new Point(416, 273)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(482 + correctionX, 181 + correctionY), new Point(388, 228)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(471 + correctionX, 237 + correctionY), new Point(458, 252)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(577 + correctionX, 239 + correctionY), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(672 + correctionX, 240 + correctionY), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(715 + correctionX, 263 + correctionY), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(684 + correctionX, 306 + correctionY), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(645 + correctionX, 288 + correctionY), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(529 + correctionX, 210 + correctionY), new Point(400, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(713 + correctionX, 185 + correctionY), new Point(500, 250)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(640 + correctionX, 157 + correctionY), new Point(580, 240)));
        }
    }
}
