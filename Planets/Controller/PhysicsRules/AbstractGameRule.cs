using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    internal abstract class AbstractGameRule
    {
        internal abstract void Execute(Playfield pf, double ms);
    }
}
