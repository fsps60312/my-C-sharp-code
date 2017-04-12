using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Motivation;
using System.Threading;
using System.Drawing;

namespace Megapolis
{
    class TaskScheduler
    {
        private MyTask task;
        public void Start()
        {
            try
            {
                task.RunScript();
            }
            catch(Exception error)
            {
                log = error.ToString();
            }
            OnStopped();
        }
        public void Stop()
        {
            task.Stop();
            OnStopped();
        }
        public TaskScheduler(MyTask _task)
        {
            task = _task;
            task.LogChanged += delegate (string log) { OnLogChanged(log); };
        }
        public delegate void StoppedEventHandler();
        public delegate void LogChangedEventHandler(string status);
        public StoppedEventHandler Stopped;
        public LogChangedEventHandler LogChanged;
        private void OnStopped() { Stopped?.Invoke(); }
        private void OnLogChanged(string log) { LogChanged?.Invoke(log); }
        private string log { set { OnLogChanged(value); } }
    }
}
