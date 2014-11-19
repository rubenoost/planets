namespace Planets.Model
{
    public struct Vector
    {
        public readonly double X;

        public readonly double Y;

        public Vector(double x = 0, double y = 0)
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
