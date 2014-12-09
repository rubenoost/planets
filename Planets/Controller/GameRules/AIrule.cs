using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Model;
using Planets.Controller.Subcontrollers;

namespace Planets.Controller.PhysicsRules
{
    class AIrule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.Iterate(g =>
                {
                    if(!(g is Antagonist)) return;
                    pf.BOT.Iterate(g2 =>
                        {
                            if (g2 is GameObject && !(g2 is Player) && !(g2 is BlackHole))
                            {
                                if ((g.Radius < g2.Radius))
                                {
                                    Vector click = (g.Location - g2.Location).ScaleToLength(g.Radius + 100);
                                    GameObject projectiel = new GameObject(new Vector(), new Vector(), 0.05 * g.Mass);

                                    g.DV = ShootProjectileController.CalcNewDV(g, projectiel, click);
                                    g.Mass -= projectiel.Mass;
                                    pf.BOT.Add(projectiel);
                                }
                                else
                                {
                                    Vector click = (g2.Location - g.Location).ScaleToLength(g.Radius + 100);
                                    GameObject projectiel = new GameObject(new Vector(), new Vector(), 0.05 * g.Mass);

                                    g.DV = ShootProjectileController.CalcNewDV(g, projectiel, click);
                                    g.Mass -= projectiel.Mass;
                                    pf.BOT.Add(projectiel);
                                }
                            }
                            else
                                return;
                                                
                        });
                    pf.BOT.Iterate(g3 =>
                    {
                        if (g3 is Player && !(g3 is Antagonist))
                        {
                            if ((g3.Location - g.Location).Length() < 600)
                            {
                                g.DV = (g3.Location - g.Location).ScaleToLength(100);
                            }

                        }
                    });   
                });
        }
    }
}
