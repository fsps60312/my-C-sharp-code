using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class InstituteOfNaturalPhenomenaTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public InstituteOfNaturalPhenomenaTask():base("Institute of Natural Phenomena", new TimeSpan(9,0,0))
        {
            locations.Add(new KeyValuePair<Point, Point>(new Point(-486, 205), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-474, 204), new Point(425, 240)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-460, 207), new Point(425, 240)));
            contractBuilding = new KeyValuePair<Point, Point>(new Point(-467, 216), new Point(425, 240));
            orderContract.Add(new Point(726, 239));
            orderContract.Add(new Point(422, 239));
        }
    }
}
