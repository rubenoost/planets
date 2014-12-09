namespace Planets.Model.GameObjects
{
    class Tardis : GameObject
    {
        public Tardis(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.NONE)
        { }
    }
}
