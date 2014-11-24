namespace Planets.Model
{
    public class Player : GameObject
    {
        public Player(double x, double y, double dx, double dy, double mass)
            : base(x, y, dx, dy, mass)
        { }

        public Player(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass)
        {
            IsAffectedByBlackHole = false;
        }
    }
}
