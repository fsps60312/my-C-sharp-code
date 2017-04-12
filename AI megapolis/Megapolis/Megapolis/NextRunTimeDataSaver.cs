using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Megapolis
{
    static class NextRunTimeDataSaver
    {
        private static string saveFile = "NextRunTimeData.ini";
        public static Dictionary<string, DateTime> nextRunTime = new Dictionary<string, DateTime>();
        public static void Load()
        {
            if (!new FileInfo(saveFile).Exists) new StreamWriter(saveFile).Close();
            StreamReader reader = new StreamReader(saveFile, Encoding.UTF8);
            try
            {
                nextRunTime.Clear();
                for (string a,b;(a=reader.ReadLine())!=null;)
                {
                    b = reader.ReadLine();
                    nextRunTime[a] = DateTime.Parse(b);
                }
            }
            catch(Exception error)
            {
                MessageBox.Show($"{error}");
                nextRunTime.Clear();
            }
            reader.Close();
        }
        public static void Save()
        {
            StreamWriter writer = new StreamWriter(saveFile, false, Encoding.UTF8);
            foreach(var p in nextRunTime)
            {
                writer.WriteLine(p.Key);
                writer.WriteLine(p.Value.ToString());
            }
            writer.Close();
        }
        static NextRunTimeDataSaver() { Load(); }
    }
}
