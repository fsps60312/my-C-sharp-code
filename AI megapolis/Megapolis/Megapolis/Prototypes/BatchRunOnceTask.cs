using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megapolis
{
    class BatchRunOnceTask:MyTask
    {
        public override void RunScript()
        {
            //List<DateTime> newNextRunTime = new List<DateTime>();
            for (int i = 0; i < tasks.Count; i++)
            {
                //newNextRunTime.Add(tasks[i].nextRunTime + tasks[i].timeSpan);
                tasks[i].nextRunTime = DateTime.MinValue;
            }
            var errorScripts = new HashSet<string>();
            StartBlueStacksIfDidnt();
            StartMegapolisIfDidnt();
            RunExpiredScripts(ref errorScripts);
            for (int i = 0; i < tasks.Count; i++)
            {
                //tasks[i].nextRunTime = newNextRunTime[i];
                tasks[i].nextRunTime = DateTime.Now + tasks[i].timeSpan;
            }
            CloseBlueStacks();
            log = "Batch Run Finished";
        }
        public BatchRunOnceTask(List<MyTask>_tasks):base("Batch",new TimeSpan())
        {
            foreach (var t in _tasks) tasks.Add(t);
        }
    }
}
