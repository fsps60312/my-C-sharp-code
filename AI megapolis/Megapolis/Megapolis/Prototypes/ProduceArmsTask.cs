using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Megapolis
{
    class ProduceArmsTask : MyTask
    {
        private static bool produceOnlyFirstOne { get { return false; } }
        public override void RunScript()
        {
            GoToMap(buildingLocation.Key);
            DoubleClickToOpenWindow(buildingLocation,()=>
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    Click(tabLocations[itemList[i].Key]);
                    for (DateTime startTime = DateTime.Now; (DateTime.Now - startTime).TotalMilliseconds < 2000;)
                    {
                        if (IsMatch(sendButton.Key, sendButton.Value))
                        {
                            Click(new Point(sendButton.Value.X + sendButton.Key.Width / 2, sendButton.Value.Y + sendButton.Key.Height / 2));
                            Thread.Sleep(500);
                            startTime = DateTime.Now;
                        }
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        Click(orderLocations[itemList[i].Value]);
                        Thread.Sleep(250);
                    }
                    if (produceOnlyFirstOne) break;
                }
            });
        }
        protected KeyValuePair<Point, Point> buildingLocation;
        protected List<KeyValuePair<int, int>> itemList = new List<KeyValuePair<int, int>>();
        public ProduceArmsTask(string name,KeyValuePair<TimeSpan,TimeSpan>probableTimeSpan):base(name,new TimeSpan())
        {
            timeSpan = (produceOnlyFirstOne ? probableTimeSpan.Key : probableTimeSpan.Value);
            timeSpan = timeSpan + timeSpan + timeSpan;//production queue has 3 slots
        }
        private static List<Point> _tabLocations, _orderLocations;
        private static List<Point> tabLocations { get { return _tabLocations; } }
        private static List<Point> orderLocations { get { return _orderLocations; } }
        private static Point productionQueueLocation { get { return new Point(402, 316); } }
        static ProduceArmsTask()
        {
            _tabLocations = new List<Point>();
            for (int i = 7; i >= 0; i--) _tabLocations.Add(new Point((254 * i + 596 * (7 - i)) / 7, 102));
            _orderLocations = new List<Point>();
            for (int i = 5; i >= 0; i--) _orderLocations.Add(new Point((243 * i + 608 * (5 - i)) / 5, 213));
        }
        private static KeyValuePair< Bitmap,Point> sendButton { get { return new KeyValuePair<Bitmap, Point>( Properties.Resources.sendButtonInArmsProductionInMegapolis, new Point(394, 332)); } }
    }
}
