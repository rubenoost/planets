namespace Planets.Model.GameObjects
{
    public class BlackHole : GameObject
    {
        public BlackHole()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }

        public BlackHole(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.EATS)
        {
            // Radius of the hole is always 50
            Radius = 50;
        }
    }
}
