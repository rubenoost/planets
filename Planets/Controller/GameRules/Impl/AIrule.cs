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
        Playfield pfcopy;
        int AI;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Check for playfield change
            CheckPlayfieldChange(pf);

            // Find anta
            if (antagonist == null) antagonist = FindAntagonist(pf);
            closest = FindClosest(pf, (Antagonist)antagonist);

            TimeSpan tijd = DateTime.Now - begin;
            if (tijd.TotalMilliseconds < 1000) return;
            begin = DateTime.Now;
            if (closest.Radius > antagonist.Radius && closest.Ai == false)
            {
                //Move away from bigger object
                GameObject projectiel = ((Antagonist)antagonist).ShootProjectile(pf, (antagonist.Location - closest.Location));
                antagonist.DV = antagonist.DV * 1.2;
                projectiel.Ai = true;
                Console.WriteLine("REN");
            }
            else if (closest.Ai == false)
            {
                //Move towards smaller object
                GameObject projectiel = ((Antagonist)antagonist).ShootProjectile(pf, (closest.Location - antagonist.Location));
                AI++;
                Console.WriteLine(AI);
                antagonist.DV = antagonist.DV * 1.2;
                projectiel.Ai = true;
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

        private GameObject FindClosest(Playfield pf, Antagonist anta)
        {
            double distance = double.MaxValue;
            double newdistance;
            GameObject newclosest = null;
            pf.BOT.Iterate(go => 
            {
                newdistance = (go.Location - anta.Location).Length();
                if (go.Ai) Console.WriteLine("AI FOUND");
                if(distance > newdistance && go.GetType() == typeof(GameObject))//HIER ZIT DE SHIT
                {
                    distance = newdistance;
                    newclosest = go;
                }
            });
            return newclosest;
        }
    }
}