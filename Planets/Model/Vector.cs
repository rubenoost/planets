namespace Planets.Model
{
    public class Vector
    {
        public double X { get; private set; }

        public double Y { get; private set; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector operator *(Vector v1, double scalar)
        {
            return new Vector(v1.X * scalar, v1.Y * scalar);
        }
    }
}
