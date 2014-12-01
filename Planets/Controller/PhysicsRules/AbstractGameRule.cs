using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    public abstract class AbstractGameRule
    {
        internal bool Activated { get; set; }

        protected AbstractGameRule()
        {
            Activated = true;
        }

        public void Execute(Playfield pf, double ms)
        {
            if(Activated)
                ExecuteRule(pf, ms);
        }

        protected abstract void ExecuteRule(Playfield pf, double ms);
    }
}
