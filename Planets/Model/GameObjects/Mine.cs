namespace Planets.Model.GameObjects
{
    public class Mine : GameObject
    {
        public Mine()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }
        public Mine(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.EXPLODES | Rule.COLLIDES)
        {
            Radius = 50;
        }
    }
}
