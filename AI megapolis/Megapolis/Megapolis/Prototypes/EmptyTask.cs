using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megapolis
{
    class EmptyTask:MyTask
    {
        public override void RunScript()
        {
            throw new NotImplementedException();
        }
        public EmptyTask():base("empty",new TimeSpan())
        {
        }
    }
}
