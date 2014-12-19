namespace Planets.Model.GameObjects
{
    public class Stasis : GameObject
    {
        public Stasis()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }
        public Stasis(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.None)
        {
            // Radius of the stasis is always 50
            Radius = 200;
        }
    }
}
