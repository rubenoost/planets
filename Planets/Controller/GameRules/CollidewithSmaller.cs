using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules
{
    public class CollidewithSmaller : AbstractCollisionRule
    {

        public void change(GameObject go1, GameObject go2, Playfield pf)
        {
            if (!(go2 is Player))
            {
                go1.Mass += go2.Mass;
                go2.Mass = 1;
                go1.Location += ((go2.Location - go1.Location)*(go2.Mass/(go1.Mass+ go2.Mass)));
                go1.DV = ((go1.Mass*go1.DV) + (go2.Mass*go2.DV))/(go1.Mass + go2.Mass);
                pf.BOT.Remove(go2);
            }
        }


        protected override void DoCollision(Playfield pf, GameObject go1, GameObject go2, double ms)
        {
            if (go1.IntersectsWith(go2))
            {
                if (go1.Mass >= go2.Mass)
                {
                    change(go1,go2,pf);
                }
                else if (go2.Mass > go1.Mass)
                {
                    change(go2,go1,pf);
                }
            }
        }
    }
}
