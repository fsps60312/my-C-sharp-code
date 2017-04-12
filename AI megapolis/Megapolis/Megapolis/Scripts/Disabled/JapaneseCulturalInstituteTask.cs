using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class JapaneseCulturalInstituteTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public JapaneseCulturalInstituteTask():base("Japanese Cultural Institute",new TimeSpan(12,0,0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(511, 137), new Point(482, 210));
            orderContract.Add(new Point(727, 236));
            orderContract.Add(new Point(420, 241));
            locations.Add(new KeyValuePair<Point, Point>(new Point(500, 136), new Point(524, 305)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(500, 136), new Point(628, 255)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(500, 136), new Point(377, 248)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(500, 136), new Point(279, 85)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(497, 128), new Point(166, 233)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(497, 128), new Point(656, 178)));
        }
    }
}
