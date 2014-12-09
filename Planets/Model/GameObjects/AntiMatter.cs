namespace Planets.Model.GameObjects
{
    class AntiMatter : GameObject
    {

        public AntiMatter(Vector location, Vector velocity, double mass) : base(location, velocity, mass, Rule.COLLIDES | Rule.EATABLE | Rule.MOVE | Rule.EATS)
        {
            Radius = 20;
        }

    }
}
