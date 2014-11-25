namespace Planets.Model
{
    public class Player : GameObject
    {
        public Player(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass)
        {
            Traits = base.Traits & ~Rule.AFFECTED_BY_BH;
        }
    }
}
