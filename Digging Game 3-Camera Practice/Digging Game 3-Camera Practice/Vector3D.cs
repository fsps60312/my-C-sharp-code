using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digging_Game_3_Camera_Practice
{
    class Vector3D
    {
        #region Constructors
        public double _x, _y, _z;
        public Vector3D(double x, double y, double z)
        {
            _x = x; _y = y; _z = z;
        }
        #endregion
        #region Operators
        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            return new Vector3D(a._x + b._x, a._y + b._y, a._z + b._z);
        }
        #endregion
        public Vector3D UnitVector()
        {
            double l = Math.Sqrt(_x * _x + _y * _y + _z * _z);
            if (l == 0.0) throw new Exception("Can't get unit vector from zero vector!");
            return new Vector3D(_x / l, _y / l, _z / l);
        }
    }
}
