using Planets.Controller.GameRules.Abstract;
using Planets.Model;

namespace Planets.Controller.GameRules.Impl
{
    public class BugfixRule : AbstractGameRule
    {
        private const double MaxSpeed = 1920.0d;
        private const double MinMass = 30.0d;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.Iterate(g =>
            {
                if(g.Mass < MinMass)
                    pf.BOT.Remove(g);
                if (g.DV.Length() > MaxSpeed)
                    g.DV = g.DV.ScaleToLength(MaxSpeed);
            });
        }
    }
}
