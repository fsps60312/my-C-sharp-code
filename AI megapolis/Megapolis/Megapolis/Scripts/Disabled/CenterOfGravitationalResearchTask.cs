using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class CenterOfGravitationalResearchTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public CenterOfGravitationalResearchTask():base("Center Of Gravitational Research",new TimeSpan(10,0,0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(-533, 212), new Point(594, 219));
            orderContract.Add(new Point(725, 235));
            orderContract.Add(new Point(426, 244));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-545, 208), new Point(559, 280)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-545, 208), new Point(315, 187)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-550, 200), new Point(383, 190)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-550, 200), new Point(564, 312)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-532, 197), new Point(479, 197)));
        }
    }
}
