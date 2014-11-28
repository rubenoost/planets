using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class CollidewithSmaller : AbstractCollisionRule
    {
        protected override void DoCollision(Playfield pf, GameObject go1, GameObject go2, double ms)
        {
            if (go1.IntersectsWith(go2))
            {
                if (go1.mass > go2.mass)
                {
                    go1.mass += go2.mass;
                    pf.BOT.Remove(go2);
                }
                else if (go2.mass > go1.mass)
                {
                    go2.mass += go1.mass;
                    pf.BOT.Remove(go1);
                }
            }
        }
    }
}
