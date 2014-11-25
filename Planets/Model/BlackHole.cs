namespace Planets.Model
{
    public class BlackHole : GameObject
    {
        private double Strength;

        public BlackHole(Vector location, Vector velocity, double mass, double strength)
            : base(location, velocity, mass, Rule.EAT)
        {
            Strength = strength;
            // Radius of the hole is always 50
            Radius = 50;
        }

        public double Gravity()
        {
            double GravGrootte = Strength + mass;
            return GravGrootte;
        }
    }
}
