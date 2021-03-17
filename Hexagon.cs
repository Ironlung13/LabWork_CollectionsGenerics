using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LabWork_CollectionsGenerics
{
    [Serializable]
    public class Hexagon : IComparable<Hexagon>, ISerializable
    {
        public Hexagon(double side)
        {
            if (side <= 0)
            {
                throw new ArgumentException("Tried to create hexagon with invalid Side value.");
            }
            Side = side;
        }
        public Hexagon(SerializationInfo info, StreamingContext context)
        {
            Side = (double)info.GetValue("Side", typeof(double));
        }
        public double Side { get; }
        public double Perimeter { get => GetPerimeter(); }
        public double Area { get => GetArea(); }
        public override string ToString()
        {
            return $"Side: {Side}, Perimeter: {Perimeter}, Area: {Area:F2}";
        }
        public static bool CanExist(Hexagon hex)
        {
            if (hex.Side >= 0)
                return true;
            else
                return false;
        }
        public double GetPerimeter()
        {
            //x - длина ребра
            //Периметр шестиугольника = 6 * x
            return 6d * Side;
        }
        public double GetArea()
        {
            //x - длина ребра
            //Площадь шестиугольника = (3 * √3 * x²)/2
            return 3d * Math.Sqrt(3) * Math.Pow(Side, 2d) / 2d;
        }

        public int CompareTo([AllowNull] Hexagon other) //Сравнение по длине ребра
        {
            if (other is null)
                return 1;
            return Side.CompareTo(other.Side);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Side", Side);
        }

        public static bool operator >(Hexagon hex1, Hexagon hex2)
        {
            return hex1.CompareTo(hex2) > 0;
        }
        public static bool operator <(Hexagon hex1, Hexagon hex2)
        {
            return hex1.CompareTo(hex2) < 0;
        }
        public static bool operator >=(Hexagon hex1, Hexagon hex2)
        {
            return hex1.CompareTo(hex2) >= 0;
        }
        public static bool operator <=(Hexagon hex1, Hexagon hex2)
        {
            return hex1.CompareTo(hex2) <= 0;
        }
        public static bool operator !=(Hexagon hex1, Hexagon hex2)
        {
            return hex1.CompareTo(hex2) != 0;
        }
        public static bool operator ==(Hexagon hex1, Hexagon hex2)
        {
            return hex1.CompareTo(hex2) == 0;
        }
    }
}
