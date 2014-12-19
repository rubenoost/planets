namespace Planets.Model.GameObjects
{
    public class BlackHole : GameObject
    {
        public BlackHole()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }

        public BlackHole(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.Eats)
        {
            // Radius of the hole is always 50
            Radius = 50;
        }
    }
}
