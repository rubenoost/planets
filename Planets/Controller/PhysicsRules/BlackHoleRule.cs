﻿using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class BlackHoleRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            foreach (var obj in pf.GameObjects)
            {
                if (obj.GetType().Name == "BlackHole")
                    obj.Pull(pf.GameObjects);
            }
        }
    }
}