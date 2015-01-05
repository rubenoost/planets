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
            pf.GameObjects.Iterate(g =>
            {
                if(g.Mass < MinMass)
                    pf.GameObjects.Remove(g);
                if (g.Dv.Length() > MaxSpeed)
                    g.Dv = g.Dv.ScaleToLength(MaxSpeed);
            });
        }
    }
}
