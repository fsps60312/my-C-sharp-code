using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;

namespace Digging_Game_Demonstrator
{
    class PublicVariables
    {
        public static bool TEST_MODE = false;
        public static PictureBox PBX;
        public static Form1 THIS;
        public static double TIME = 0.0;
        public static string TEXT
        {
            get
            {
                if (THIS == null) return null;
                return THIS.Text;
            }
            set
            {
                if (THIS == null) return;
                THIS.Text = value;
            }
        }
        private PublicVariables() { }
    }
}
