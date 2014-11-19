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
    }
}
