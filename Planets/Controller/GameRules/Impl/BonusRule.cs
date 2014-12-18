using System;
using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class BonusRule : AbstractGameRule
    {
        private Random randX = new Random();
        private Random randY = new Random();

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Update speed to black hole
            pf.BOT.Iterate(g =>
            {
                if (!(g is Bonus))
                    return;
                pf.BOT.Iterate(g2 =>
                {
                    if (g2 is Player)
                    {
                        if (g2.IntersectsWith(g))
                        {
                            if(g2 is Antagonist){
                                pf.sb.AddScore(new Score(100, DateTime.Now, g.Location, false));
                            } else {
                                pf.sb.AddScore(new Score(100, DateTime.Now, g.Location, true));
                            }

                            g.Location = new Vector(randX.Next(0, 1920), randY.Next(0, 1080));
                        }
                    }
                });
            });
        }
    }
}
