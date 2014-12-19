namespace Planets.Model.GameObjects
{
    public class Mine : GameObject
    {
        public Mine()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }
        public Mine(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.Explodes | Rule.Collides)
        {
            Radius = 50;
        }
    }
}
