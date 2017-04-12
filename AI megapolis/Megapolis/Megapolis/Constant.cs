using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Megapolis
{
    static class Constant
    {
        //public static KeyValuePair<Point, Point>[] Correction = { new KeyValuePair<Point, Point>(new Point(0, 9), new Point(0, 0)) };
        public const int MaxTimeToTryToOpenSettings = 20 * 1000;
        public const int MaxTimeIntervalBetweenTasksToNotCloseBlueStacks = 5 * 60 * 1000;
        public const int MaxAllowableTimeToRunOneSingleScript = 10 * 60 * 1000;
        public const int MaxTimeForStartingBlueStacksOrMegapolis = 10 * 60 * 1000;
        public const int TimeToSleepIfBlueStacksOrMegapolisDidntStartSuccessfully = 60 * 1000;
        public static Random random = new Random();
        public static Size MyTaskCaptureSize { get { return new Size(851, 478); } }
        public static Vector MyTaskVectorFromControlBoxesToCaptureArea { get { return new Vector(84, 41) - new Vector(820, 18); } }
        public static string BlueStacksExeFileFullName = @"C:\Program Files (x86)\Bluestacks\Bluestacks.exe";
    }
}
