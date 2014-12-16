namespace Planets.Model.GameObjects
{
    class Tardis : GameObject
    {
        public Tardis()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }

        public Tardis(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.COLLIDES)
        {
            Traits = Traits & ~Rule.COLLIDES;
            Traits = Traits & ~Rule.HAS_SCORE;
        }
    }
}
