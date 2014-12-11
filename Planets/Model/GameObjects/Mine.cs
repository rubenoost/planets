namespace Planets.Model.GameObjects
{
    class Mine : GameObject
    {
        public Mine(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.EXPLODES | Rule.COLLIDES)
        {
            Radius = 50;
        }
    }
}
