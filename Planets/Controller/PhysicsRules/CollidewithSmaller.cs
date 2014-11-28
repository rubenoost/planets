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
                    if (!(go2 is Player))
                    {
                        go1.mass += go2.mass;
                        go2.mass = 1;
                        go1.Location = go2.Location;
                        pf.BOT.Remove(go2);
                    }
                }
                else if (go2.mass > go1.mass)
                {
                    if (!(go1 is Player))
                    {
                        go2.mass += go1.mass;
                        go1.mass = 1;
                        go2.Location = go1.Location;
                        pf.BOT.Remove(go1);
                    }
                }
                else if (go1.mass == go2.mass)
                {
                    if (!(go1 is Player))
                    {
                        go2.mass += go1.mass;
                        go1.mass = 1;
                        go1.Location = go2.Location;
                        pf.BOT.Remove(go1);
                    }
                }
                else if (go2.mass == go1.mass)
                {
                    if (!(go2 is Player))
                    {
                        go1.mass += go2.mass;
                        go2.mass = 1;
                        go1.Location = go2.Location;
                        pf.BOT.Remove(go2);
                    }
                }
            }
        }
    }
}
