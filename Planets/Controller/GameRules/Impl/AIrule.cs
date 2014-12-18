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
        GameObject closest = null;
        GameObject player;
        Playfield pfcopy;
        double distance = 0.0;
        double newdistance;
        bool run;
        bool eat;
        bool hug;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Check for playfield change
            CheckPlayfieldChange(pf);

            // Find antagonist
            if(antagonist == null) antagonist = FindAntagonist(pf);

            // Iterate through all gameobjects
            pf.BOT.Iterate(g =>
                {
                    if(g.GetType() == typeof(GameObject))
                    {
                        //Let's find the closest Gameobject
                        if(g == FindClosest((GameObject)g, (Antagonist)antagonist))
                        {
                            if(g.Radius > antagonist.Radius)
                            {
                                //if the closest Gameobject is bigger
                                run = true;
                            }
                            else if(closest.Ai == false)
                            {
                                //if the closest Gameobject is smaller
                                eat = true;
                            }
                        }
                    }
                    else if(g.GetType() == typeof(Player) && g == FindClosest(g, (Antagonist)antagonist))
                    {
                        //If the closest gameobject is a player ANNOY HIM!!!
                        eat = true;
                    }

                    /*
                    if (antagonist == null && g is Antagonist)
                    {
                        //find and bind the antagonist
                        antagonist = g;
                        Console.WriteLine("Antagonist!");
                        return;
                    }
                    else if (g is Antagonist)
                    {
                        //find and bind new antagonist location
                        antagonist = g;
                        return;
                    }
                    else if (antagonist == null) return;
                    if (closest == null)
                    {
                        closest = FindClosest(g, (Antagonist)antagonist, pf);
                    }
                    else
                    {
                        closest = FindClosest(g, (Antagonist)antagonist, pf);
                    }
                    if (antagonist != null && g.GetType() == typeof(GameObject) && g.Ai == false && g == closest)
                    {
                        if ( g.Radius > antagonist.Radius)
                        {
                            //Move away from bigger object
                            run = true;
                            eat = false;
                            hug = false;
                        }
                        else
                        {
                            //Move towards smaller object
                            run = false;
                            eat = true;
                            hug = false;
                        }
                    }
                    else if( g == closest)
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
                     */

                });
            TimeSpan tijd = DateTime.Now - begin;
            if (tijd.TotalMilliseconds < 1000) return;
            begin = DateTime.Now;
            if(run == true)
            {
                //Move away from bigger object
                ((Antagonist)antagonist).ShootProjectile(pf, (closest.Location - antagonist.Location));
                Console.WriteLine("REN");
                run = false;
                distance = 0.0;
            }
            else if(eat == true)
            {
                //Move towards smaller object or player
                ((Antagonist)antagonist).ShootProjectile(pf, (antagonist.Location - closest.Location));
                Console.WriteLine(closest.Ai);
                eat = false;
                distance = 0.0;
            }
        }

        #region Helper Methods

        private void CheckPlayfieldChange(Playfield pf)
        {
            // Code for keeping track of the playfield
            if (pfcopy == null)
            {
                pfcopy = pf;
            }
            else if (pf != pfcopy)
            {
                antagonist = null;
                player = null;
                distance = 0.0;
                run = false;
                eat = false;
                hug = false;
                pfcopy = pf;
            }
        }

        private GameObject FindAntagonist(Playfield pf)
        {
            GameObject anta = null;
            pf.BOT.Iterate(g => { if (g is Antagonist) anta = g; });
            return anta;
        }

        #endregion

        private GameObject FindClosest(GameObject go, Antagonist a)
        {
            if(distance == 0.0)
            {
                distance = (go.Location - a.Location).Length();
                closest = go;
                return closest;
            }
            else if(distance > (go.Location - a.Location).Length())
            {
                distance = (go.Location - a.Location).Length();
                closest = go;
                return closest;
            }
            else
            {
                return closest;
            }
        }
    }
}