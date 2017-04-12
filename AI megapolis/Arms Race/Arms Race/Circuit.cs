using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Arms_Race
{
    class Circuit
    {
        private void locateFireworkShopLocations()
        {
            fireworkShopLocations.Add(locate("firework shop #1"));
            fireworkShopLocations.Add(locate("firework shop #2"));
            fireworkShopLocations.Add(locate("firework shop #3"));
        }
        private void locateBoxOfFireworksXButton()
        {
            boxOfFireworksButton = locate("box of firework button");
            XButton = locate("X button");
        }
        private void locateThings()
        {
            locateFireworkShopLocations();
            locateBoxOfFireworksXButton();
        }
        private void wait(int miliseconds)
        {
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < miliseconds) Application.DoEvents();
        }
        private Point locate(string name)
        {
            AppendMsg($"Move cursor to the middle of \"{name}\" and then press Ctrl+L.");
            Point answer = new Point(-1, -1);
            bool ok = false;
            HotKey hotKey = new HotKey(Form1.getHandle(), Keys.L, Keys.Control);
            hotKey.OnHotkey += (sender, e) =>
            {
                ok = true;
                answer = Cursor.Position;
            };
            while (!ok) Application.DoEvents();
            hotKey.Dispose();
            AppendMsg($"Recorded \"{name}\"'s position: {answer}");
            Debug.Assert(answer.X != -1);
            return answer;
        }
        private void AppendMsg(string msg)
        {
            OnMessageAppended(msg);
        }
        public delegate void MessageAppendedHandler(string msg);
        public MessageAppendedHandler MessageAppended;
        private void OnMessageAppended(string msg)
        {
            MessageAppended?.Invoke(msg);
        }
        public void start()
        {
            locateThings();
            Debug.Assert(fireworkShopLocations.Count == 3);
            AppendMsg("circuit has been started...");
            bool firstTime = true;
            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    wait(5000);
                    MyCursor.LeftClick(fireworkShopLocations[i]);
                    if (!firstTime)
                    {
                        wait(5000);
                        MyCursor.LeftClick(fireworkShopLocations[i]);
                    }
                    wait(5000);
                    MyCursor.LeftClick(boxOfFireworksButton);
                    wait(1000);
                    MyCursor.LeftClick(XButton);
                }
                int waitTime = 16;
                AppendMsg($"The process will wait for {waitTime} minutes and then restart the cycle...");
                wait(waitTime * 60 * 1000);
                firstTime = false;
            }
        }
        private List<Point> fireworkShopLocations = new List<Point>();
        private Point boxOfFireworksButton, XButton;
    }
}
