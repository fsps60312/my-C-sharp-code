using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Megapolis
{
    class AssemblingLineTask:MultiContractTask
    {
        protected KeyValuePair<Point, Point> assemblyPlantLocation, launchLocation;
        public override void RunScript()
        {
            GoToAndClick(launchLocation);
            Click(launchLocation.Value);
            GoToAndClick(assemblyPlantLocation);
            base.RunScript();
            GoToMap(assemblyPlantLocation.Key);
            DoubleClickToOpenWindow(assemblyPlantLocation,()=> { Click(buildButtonLocation); });
        }
        public AssemblingLineTask(string name,TimeSpan timeSpan):base(name,timeSpan)
        {
        }
        private static Point buildButtonLocation = new Point(589, 377);
    }
}
