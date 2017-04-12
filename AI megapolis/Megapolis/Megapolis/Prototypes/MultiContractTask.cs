using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Megapolis
{
    class MultiContractTask:MyTask
    {
        protected Point locationOnMap;
        protected List<Point> contractLocations = new List<Point>();
        protected List<Point> orderContract = new List<Point>();
        public override void RunScript()
        {
            GoToMap(locationOnMap);
            foreach (Point p in contractLocations)
            {
                Click(p);
                Thread.Sleep(250);
            }
            GoToMap(locationOnMap);
            foreach (Point p in contractLocations)
            {
                DoubleClickToOpenWindow(new KeyValuePair<Point, Point>(locationOnMap, p),()=>
                {
                    foreach (Point q in orderContract)
                    {
                        Click(q);
                        Thread.Sleep(500);
                    }
                });
                CloseWindows();
            }
        }
        public MultiContractTask(string name, TimeSpan timeSpan) : base(name, timeSpan)
        {
        }
    }
}
