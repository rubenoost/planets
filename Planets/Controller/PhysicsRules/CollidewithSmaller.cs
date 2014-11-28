using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class CollidewithSmaller : AbstractCollisionRule
    {

        public void change(GameObject go1, GameObject go2, Playfield pf)
        {
            if (!(go2 is Player))
            {
                go1.mass += go2.mass;
                go2.mass = 1;
                go1.Location += ((go2.Location - go1.Location)*(go2.mass/(go1.mass+ go2.mass)));
                go1.DV = ((go1.mass*go1.DV) + (go2.mass*go2.DV))/(go1.mass + go2.mass);
                pf.BOT.Remove(go2);
            }
        }


        protected override void DoCollision(Playfield pf, GameObject go1, GameObject go2, double ms)
        {
            if (go1.IntersectsWith(go2))
            {
                if (go1.mass >= go2.mass)
                {
                    change(go1,go2,pf);
                }
                else if (go2.mass > go1.mass)
                {
                    change(go2,go1,pf);
                }
            }
        }
    }
}
