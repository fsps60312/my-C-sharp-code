using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using Motivation;

namespace Megapolis
{
    class ContractTask:CollectionTask
    {
        protected KeyValuePair<Point, Point> contractBuilding=new KeyValuePair<Point, Point>();
        protected List<Point> orderContract=new List<Point>();
        //protected List<KeyValuePair<Point, Point>> locations=new List<KeyValuePair<Point, Point>>();
        public override void RunScript()
        {
            GoToAndClick(contractBuilding);
            //base.RunScript();
            GoToMap(contractBuilding.Key);
            DoubleClickToOpenWindow(contractBuilding,()=>
            {
                foreach (var p in orderContract)
                {
                    Click(p);
                    Thread.Sleep(500);
                }
            });
        }
        public ContractTask(string name,TimeSpan timeSpan):base(name,timeSpan)
        {
        }
    }
}
