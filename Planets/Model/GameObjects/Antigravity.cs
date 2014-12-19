namespace Planets.Model.GameObjects
{
    public class Antigravity : GameObject
    {
        public Antigravity()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }
        public Antigravity(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.None)
        {
            Radius = 50;
        }
    }
}
