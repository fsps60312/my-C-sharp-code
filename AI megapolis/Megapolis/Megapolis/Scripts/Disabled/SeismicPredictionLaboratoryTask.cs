using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class SeismicPredictionLaboratoryTask:ContractTask
    {
        protected override bool enabled { get { return false; } }
        public SeismicPredictionLaboratoryTask() : base("Seismic Prediction Laboratory", new TimeSpan(10, 0, 0))
        {
            contractBuilding = new KeyValuePair<Point, Point>(new Point(-450, 172), new Point(209, 130));
            orderContract.Add(new Point(726, 239));
            orderContract.Add(new Point(425, 244));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-460, 178), new Point(455, 118)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-460, 178), new Point(576, 276)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-450, 172), new Point(737, 104)));
            locations.Add(new KeyValuePair<Point, Point>(new Point(-450, 172), new Point(440, 262)));
        }
    }
}
