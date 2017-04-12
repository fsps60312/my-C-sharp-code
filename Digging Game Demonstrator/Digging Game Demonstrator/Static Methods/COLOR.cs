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
    static class Color_Extensions
    {
        public static Color Add_RGB(this Color c,int dis)
        {
            return Color.FromArgb(c.A, (c.R + dis).ToByte(), (c.G + dis).ToByte(), (c.B + dis).ToByte());
        }
        public static Color Multiply_RGB(this Color c,double ratio)
        {
            return Color.FromArgb(c.A, (c.R * ratio).ToByte(), (c.G * ratio).ToByte(), (c.B * ratio).ToByte());
        }
        public static Color Add_A(this Color c,int dis)
        {
            return Color.FromArgb((c.A + dis).ToByte(), c.R, c.G, c.B);
        }
        public static Color Multiply_A(this Color c,double ratio)
        {
            return Color.FromArgb((c.A * ratio).ToByte(), c.R, c.G, c.B);
        }
        public static Color Merge(this Color c,Color t,double ratio)
        {
            return Color.FromArgb(c.A.Merge(t.A, ratio), c.R.Merge(t.R, ratio), c.G.Merge(t.G, ratio), c.B.Merge(t.B, ratio));
        }
        public static int RGBsum(this Color c) { return c.R + c.G + c.B; }
    }
    class COLOR
    {
        public static Color FromRToG(double ratio)
        {
            if (ratio <= 0.5) return Color.FromArgb(255, (255 * (ratio / 0.5)).ToByte(), 0);
            else return Color.FromArgb((255 * ((1.0 - ratio) / 0.5)).ToByte(), 255, 0);
        }
        private COLOR() { }
    }
}
