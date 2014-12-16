using System;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules
{
    class AIrule : AbstractGameRule
    {
        GameObject antagonist;
        DateTime begin;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            TimeSpan tijd = DateTime.Now - begin;
            if (tijd.TotalMilliseconds < 1000) return;
            begin = DateTime.Now;
            pf.BOT.Iterate(g =>
                {
                    if (antagonist == null && g is Antagonist)
                    {
                        //find and bind the antagonist
                        antagonist = g;
                        Console.WriteLine("Antagonist!");

                    }
                    else if (antagonist != null && g.GetType() == typeof(GameObject) && !g.Ai)
                    {
                        GameObject closest = FindClosest(g, (Antagonist)antagonist, pf);
                        if (g == closest && g.Radius > antagonist.Radius)
                        {
                            //Move away from bigger object
                            ((Antagonist)antagonist).ShootProjectile(pf, (g.Location - antagonist.Location));
                        }
                        else if (g == closest && !g.Ai)
                        {
                            //Move towards smaller object
                            ((Antagonist)antagonist).ShootProjectile(pf, (antagonist.Location - g.Location));
                        }
                    }
                    else
                    {
                        if (antagonist != null && g is Player && g.GetType() == typeof(Player))
                        {
                            if ((g.Location - antagonist.Location).Length() < 1000)
                            {
                                //Move towards player if there is no other option
                                antagonist.DV = (g.Location - antagonist.Location).ScaleToLength(100);
                                //insert shoot&move logic here
                                //
                            }

                        }
                    }
                });
        }
        private GameObject FindClosest(GameObject go, Antagonist a, Playfield pf)
        {
            //find closest gameobject
            double workingdist = double.MaxValue;
            GameObject closest = null;
            double olddist = (go.Location - a.Location).Length();

            if (olddist < workingdist)
            {
                workingdist = olddist;
                closest = go;
            }
            return closest;
        }
    }
}
