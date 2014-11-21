using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class BlackHoleRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            //foreach (var obj in pf.GameObjects)
            //{
            //    if(obj is BlackHole) {
            //        if(obj.Pull(pf.GameObjects)){
            //            foreach(GameObject g in pf.GameObjects){
            //                if(obj.IntersectsWith(g)){
            //                    pf.GameObjects.Remove(g);
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}
