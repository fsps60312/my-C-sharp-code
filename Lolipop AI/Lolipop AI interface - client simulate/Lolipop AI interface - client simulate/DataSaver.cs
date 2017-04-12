using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lolipop_AI_interface___client_simulate
{
    class DataSaver
    {
        static StreamWriter writer = new StreamWriter("PlayData.txt", true, Encoding.UTF8);
        public static void WriteLine(string s)
        {
            writer.WriteLine(s);
        }
        public static void Close()
        {
            writer.Close();
        }
    }
}
