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
        GameObject _closest = null;
        Playfield _pfcopy;
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Check for playfield change
            CheckPlayfieldChange(pf);

            // Find antagonist
            if (_antagonist == null) _antagonist = FindAntagonist(pf);
            _closest = FindClosest(pf, (Antagonist)_antagonist);
            TimeSpan tijd = DateTime.Now - _begin;
            if (tijd.TotalMilliseconds < 1000) return;
            _begin = DateTime.Now;
            if (_closest != null && _closest.Radius > _antagonist.Radius && _closest.Ai == false)
            {
                //Move away from bigger object
                GameObject projectiel = ((Antagonist)_antagonist).ShootProjectile(pf, (_antagonist.Location - _closest.Location));
                projectiel.Ai = true;
            }
            else if (_closest != null && _closest.Ai == false)
            {
                //Move towards smaller object
                GameObject projectiel = ((Antagonist)_antagonist).ShootProjectile(pf, (_closest.Location - _antagonist.Location));
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
            pf.GameObjects.Iterate(g => { if (g is Antagonist) anta = g; });
            return anta;
        }

        private GameObject FindClosest(Playfield pf, Antagonist antagonist)
        {
            double distance = double.MaxValue;
            double newdistance;
            GameObject newclosest = null;
            pf.GameObjects.Iterate(go => 
            {
                newdistance = (go.Location - antagonist.Location).Length();
                if (go.Ai == true) return;
                if(distance > newdistance && go.GetType() == typeof(GameObject))
                {
                    distance = newdistance;
                    newclosest = go;
                }
            });
            return newclosest;
        }
        #endregion
    }
}