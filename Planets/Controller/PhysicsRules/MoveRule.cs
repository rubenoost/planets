using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class MoveRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            foreach (GameObject go in pf.GameObjects)
            {
                go.UpdateLocation(ms);
            }
        }
    }
}
