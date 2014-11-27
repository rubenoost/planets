using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class CollidewithSmaller : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.DoCollisions((go1, go2, dt) => CheckObjectCollission(go1, go2), ms);
        }

        public void CheckObjectCollission(GameObject c1, GameObject c2)
        {

        }
    }
}
