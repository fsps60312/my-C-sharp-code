using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digging_Game_3_Camera_Practice
{
    class Point3D
    {
        public double _x, _y, _z;
        public Point3D(double x, double y, double z)
        {
            _x = x; _y = y; _z = z;
        }
        public static Point3D operator+(Point3D p,Vector3D v)
        {
            return new Point3D(p._x + v._x, p._y + v._y, p._z + v._z);
        }
    }
}
