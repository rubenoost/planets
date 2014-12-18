using Planets.Model;

namespace Planets.Controller.GameRules.Abstract
{
    public abstract class AbstractGameRule : INativeGameRule
    {
        internal bool Activated { get; set; }

        protected AbstractGameRule()
        {
            Activated = true;
        }

        public void Execute(GameEngine ge, double ms)
        {
            if (Activated)
                ExecuteRule(ge.field, ms);
        }

        protected abstract void ExecuteRule(Playfield pf, double ms);
    }
}
