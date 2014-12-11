namespace Planets.Model.GameObjects
{
    class Explosion : GameObject
    {
        public Explosion(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.NONE)
        {
        }
    }
}
