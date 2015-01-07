using Planets.Controller.GameRules.Abstract;
using Planets.Model;

namespace Planets.Controller.GameRules.Impl
{
    public class BugfixRule : AbstractGameRule
    {
        /// <summary>
        /// Maximum speed of a GameObject.
        /// </summary>
        private const double MaxSpeed = 1920.0d;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Iterate through all gameobjects
            pf.GameObjects.Iterate(g =>
            {
                // If speed is to high, scale down to maximum speed
                if (g.Dv.Length() > MaxSpeed)
                    g.Dv = g.Dv.ScaleToLength(MaxSpeed);
            });
        }
    }
}
