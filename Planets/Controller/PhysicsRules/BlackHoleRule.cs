using System.Collections.Generic;
using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class BlackHoleRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Check for collisions with black hole

            var removed = new List<GameObject>();

            foreach (GameObject go in pf.GameObjects)
            {
                if (go is BlackHole)
                {
                    //go.Pull(pf.GameObjects);
                    foreach (GameObject go2 in pf.GameObjects)
                    {

                        if (go != go2 && go.IntersectsWith(go2))
                        {
                            if (!(go2 is Player))
                                removed.Add(go2);
                        }
                    }
                }
            }
            removed.ForEach(g => pf.GameObjects.Remove(g));

            // Update speed to black hole

        }
    }
}
