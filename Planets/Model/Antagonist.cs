using Planets.Model.GameObjects;

namespace Planets.Model
{
    public class Antagonist : Player
    {
        public Antagonist()
            : this(new Vector(), new Vector(), 5000)
        { }
        public Antagonist(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass)
        {
            Traits = Traits & ~Rule.AffectedByBh;
        }
    }
}
