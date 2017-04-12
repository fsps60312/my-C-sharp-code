using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digging_Game_3_Camera_Practice
{
    class Plane
    {
        double _a, _b, _c, _d;//ax+by+cz+d=0
        Plane(double a,double b,double c,double d)
        {
            _a = a; _b = b; _c = c; _d = d;
        }
        Plane(Point3D p,Vector3D v)
        {
            _a = v._x; _b = v._y; _c = v._z;
            _d = 0.0 - _a * p._x - _b * p._y - _c * p._z;
        }
    }
}
