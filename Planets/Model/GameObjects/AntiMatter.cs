namespace Planets.Model.GameObjects
{
    public class AntiMatter : GameObject
    {
        public AntiMatter()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }
        public AntiMatter(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.Collides | Rule.Eatable | Rule.Move | Rule.Eats | Rule.DynamicRadius)
        { }

    }
}
