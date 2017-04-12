using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Megapolis
{
    class CollectionTask:MyTask
    {
        protected List<KeyValuePair<Point, Point>> locations = new List<KeyValuePair<Point, Point>>();
        public override void RunScript()
        {
            //locations.Sort(new Comparison<KeyValuePair<Point, Point>>((KeyValuePair<Point, Point> a, KeyValuePair<Point, Point> b) =>
            //{
            //    if (a.Key.X != b.Key.X) return a.Key.X < b.Key.X ? 1 : -1;
            //    if (a.Key.Y != b.Key.Y) return a.Key.Y < b.Key.Y ? 1 : -1;
            //    //if (a.Value.X != b.Value.X) return a.Value.X < b.Value.X ? 1 : -1;
            //    //if (a.Value.Y != b.Value.Y) return a.Value.Y < b.Value.Y ? 1 : -1;
            //    return 0;
            //}));
            for(int i=0;i<locations.Count;i++)
            {
                if (i == 0 || locations[i - 1].Key != locations[i].Key)
                {
                    GoToAndClick(locations[i]);
                }
                else
                {
                    Thread.Sleep(500);
                    Click(locations[i].Value);
                }
            }
        }
        public CollectionTask(string name,TimeSpan timeSpan):base(name,timeSpan)
        {
        }
    }
}
