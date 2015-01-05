using System;
using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class BonusRule : AbstractGameRule
    {
        private readonly Random _randX = new Random();
        private readonly Random _randY = new Random();

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Update speed to black hole
            pf.GameObjects.Iterate(g =>
            {
                if (!(g is Bonus))
                    return;
                pf.GameObjects.Iterate(g2 =>
                {
                    if (g2 is Player)
                    {
                        if (g2.IntersectsWith(g))
                        {
                            if(g2 is Antagonist){
                                pf.ScoreBoard.AddScore(new Score(100, DateTime.Now, g.Location, false));
                            } else {
                                pf.ScoreBoard.AddScore(new Score(100, DateTime.Now, g.Location, true));
                            }

                            g.Location = new Vector(_randX.Next(0, 1920), _randY.Next(0, 1080));
                        }
                    }
                });
            });
        }
    }
}
