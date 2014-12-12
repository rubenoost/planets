namespace Planets.Model.GameObjects
{
    class Antigravity : GameObject
    {
        public Antigravity(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.COLLIDES)
        {
            Radius = 50;
        }
    }
}
