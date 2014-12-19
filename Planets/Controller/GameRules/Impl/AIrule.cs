using System;
using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class AIrule : AbstractGameRule
    {
        GameObject _antagonist;
        DateTime _begin;
        GameObject _closest;
        Playfield _pfcopy;
        int _ai;

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Check for playfield change
            CheckPlayfieldChange(pf);

            // Find anta
            if (_antagonist == null) _antagonist = FindAntagonist(pf);
            _closest = FindClosest(pf, (Antagonist)_antagonist);

            TimeSpan tijd = DateTime.Now - _begin;
            if (tijd.TotalMilliseconds < 1000) return;
            _begin = DateTime.Now;
            if (_closest.Radius > _antagonist.Radius && _closest.Ai == false)
            {
                //Move away from bigger object
                GameObject projectiel = ((Antagonist)_antagonist).ShootProjectile(pf, (_antagonist.Location - _closest.Location));
                _antagonist.DV = _antagonist.DV * 1.2;
                projectiel.Ai = true;
                Console.WriteLine("REN");
            }
            else if (_closest.Ai == false)
            {
                //Move towards smaller object
                GameObject projectiel = ((Antagonist)_antagonist).ShootProjectile(pf, (_closest.Location - _antagonist.Location));
                _ai++;
                Console.WriteLine(_ai);
                _antagonist.DV = _antagonist.DV * 1.2;
                projectiel.Ai = true;
            }
        }

        #region Helper Methods

        private void CheckPlayfieldChange(Playfield pf)
        {
            // Code for keeping track of the playfield
            if (_pfcopy == null)
            {
                _pfcopy = pf;
            }
            else if (pf != _pfcopy)
            {
                _antagonist = null;
                _pfcopy = pf;
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