using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    internal abstract class AbstractGameRule
    {
        internal bool Activated { get; set; }

        protected AbstractGameRule()
        {
            Activated = true;
        }

        internal void Execute(Playfield pf, double ms)
        {
            if(Activated)
                ExecuteRule(pf, ms);
        }

        protected abstract void ExecuteRule(Playfield pf, double ms);
    }
}
