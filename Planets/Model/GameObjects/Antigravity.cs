namespace Planets.Model.GameObjects
{
    class Antigravity : GameObject
    {
        public Antigravity(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.NONE)
        {
            Radius = 50;
        }
    }
}
