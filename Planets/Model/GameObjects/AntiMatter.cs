namespace Planets.Model.GameObjects
{
    class AntiMatter : GameObject
    {
        public AntiMatter()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }
        public AntiMatter(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.COLLIDES | Rule.EATABLE | Rule.MOVE | Rule.EATS | Rule.DYNAMIC_RADIUS)
        { }

    }
}
