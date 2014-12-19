namespace Planets.Model.GameObjects
{
    public class Bonus : GameObject
    {
        // ReSharper disable once UnusedMember.Global
        public Bonus()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }

        public Bonus(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.Collides)
        {
            Traits = Traits & ~Rule.Collides & ~Rule.HasScore;
        }
    }
}
