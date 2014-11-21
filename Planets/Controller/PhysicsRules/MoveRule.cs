using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class MoveRule : AbstractGameRule
    {
        internal override void Execute(Playfield pf, double ms)
        {
            foreach (GameObject go in pf.GameObjects)
            {
                go.UpdateLocation(ms);
            }
        }
    }
}
