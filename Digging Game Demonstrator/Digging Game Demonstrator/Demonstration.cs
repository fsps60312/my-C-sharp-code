using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Digging_Game_Demonstrator
{
    abstract class Demonstration
    {
        public static List<Demonstration> DEMONSTRATIONS = new List<Demonstration>();
        public static GetMergedImage(int width,int height)
        {
            Bitmap bmp=BITMAP.New(width,height);
            foreach(var d in DEMONSTRATIONS)
            {

            }
        }
        public double Time = 0.0;
        public double Length = 0.0;
        public PointD TopLeftPoint=new PointD(0.0,0.0);
        public PointD BottomRightPoint=new PointD(1.0,1.0);
        public abstract Bitmap GetImage();
        public virtual void DrawImage(BitmapData data_bac)
        {
            Bitmap bmp=GetImage();
            data_bac.Paste(bmp,)
        }
    }
}
