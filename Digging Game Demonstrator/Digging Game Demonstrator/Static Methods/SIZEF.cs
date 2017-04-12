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
    static class SizeF_Extensions
    {
        public static Size Round(this SizeF szf) { return new Size(szf.Width.Round(), szf.Height.Round()); }
        public static Size Floor(this SizeF szf) { return new Size(szf.Width.Floor(), szf.Height.Floor()); }
        public static Size Ceiling(this SizeF szf) { return new Size(szf.Width.Ceiling(), szf.Height.Ceiling()); }
    }
    class SIZEF
    {
    }
}
