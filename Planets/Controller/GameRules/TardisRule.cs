﻿using System;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules {
    class TardisRule : AbstractGameRule {
        private Random randX = new Random();
        private Random randY = new Random();

        protected override void ExecuteRule(Playfield pf, double ms) {
            // Update speed to black hole
            pf.BOT.Iterate(g => {
                if(!(g is Tardis))
                    return;
                pf.BOT.Iterate(g2 => {
                    if(g2 is Player) {
                        if(g.IntersectsWith(g2)) {
                            g.Location = new Vector(randX.Next(0, 1920), randY.Next(0, 1080));
                        }
                    }
                });
            });
        }
    }
}
