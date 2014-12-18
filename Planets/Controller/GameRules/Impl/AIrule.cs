using System;
using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class AIrule : AbstractGameRule
    {
        GameObject antagonist;
        DateTime begin;
        GameObject closest;
        GameObject player;
        bool run;
        bool eat;
        bool hug;
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
                        closest = FindClosest(g, (Antagonist)antagonist, pf);
                        if (g == closest && g.Radius > antagonist.Radius)
                        {
                            //Move away from bigger object
                            run = true;
                            eat = false;
                            hug = false;
                        }
                        else if (g == closest && !g.Ai)
                        {
                            //Move towards smaller object
                            run = false;
                            eat = true;
                            hug = false;
                        }
                    }
                    else
                    {
                        if (antagonist != null && g is Player && g.GetType() == typeof(Player))
                        {
                            player = g;
                            if ((g.Location - antagonist.Location).Length() < 1000)
                            {
                                run = false;
                                eat = false;
                                hug = true;
                                //Move towards player if there is no other option                                
                            }
                        }
                    }
                });

            if(run == true)
            {
                ((Antagonist)antagonist).ShootProjectile(pf, (closest.Location - antagonist.Location));
                Console.WriteLine("REN");
                run = false;
            }
            else if(eat == true)
            {
                ((Antagonist)antagonist).ShootProjectile(pf, (antagonist.Location - closest.Location));
                Console.WriteLine("EET");
                eat = false;
            }
            else if(!(player == null) && hug == true)
            {
                ((Antagonist)antagonist).ShootProjectile(pf, (antagonist.Location - player.Location));
                Console.WriteLine("KNUFFEL");
                hug = false;
            }
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