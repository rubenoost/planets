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
        double workingdist = double.MaxValue;
        bool run;
        bool eat;
        bool hug;
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.Iterate(g =>
                {
                    if (antagonist == null && g is Antagonist)
                    {
                        //find and bind the antagonist
                        antagonist = g;
                        Console.WriteLine("Antagonist!");
                    }
                    else if (g is Antagonist)
                    {
                        //find and bind new antagonist location
                        antagonist = g;
                    }
                    else if (antagonist == null) return;
                    if (closest == null)
                    {
                        closest = FindClosest(g, (Antagonist)antagonist);
                    }
                    else
                    {
                        closest = FindClosest(g, (Antagonist)antagonist);
                    }
                    if (antagonist != null && g.GetType() == typeof(GameObject) && g.Ai == false && g == closest)
                    {
                        if (g.Radius > antagonist.Radius)
                        {
                            //Move away from bigger object
                            run = true;
                            eat = false;
                            hug = false;
                        }
                        else if (g.Ai == false)
                        {
                            //Move towards smaller object
                            run = false;
                            eat = true;
                            hug = false;
                        }
                    }
                    else if (g == closest)
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
            TimeSpan tijd = DateTime.Now - begin;
            if (tijd.TotalMilliseconds < 1000) return;
            begin = DateTime.Now;
            if (run)
            {
                //Move away from bigger object
                ((Antagonist)antagonist).ShootProjectile(pf, (closest.Location - antagonist.Location));
                Console.WriteLine("REN");
                run = false;
            }
            else if (eat)
            {
                //Move towards smaller object
                ((Antagonist)antagonist).ShootProjectile(pf, (antagonist.Location - closest.Location));
                Console.WriteLine("EET");
                eat = false;
            }
            else if (!(player == null) && hug)
            {
                ((Antagonist)antagonist).ShootProjectile(pf, (antagonist.Location - player.Location));
                Console.WriteLine("KNUFFEL");
                hug = false;
                //Move towards player if there is no other option 
            }
        }
        private GameObject FindClosest(GameObject go, Antagonist a)
        {
            //find closest gameobject
            GameObject closest = null;
            double dist = (go.Location - a.Location).Length();

            if (dist < workingdist)
            {
                workingdist = dist;
                closest = go;
            }
            return closest;
        }
    }
}