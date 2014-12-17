using Planets.Controller.Subcontrollers;
using Planets.Model;

namespace Planets.Controller.GameRules
{
    class ResetRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {

            if (pf.CurrentPlayer.Mass < 500)
            {
                Playfield PFNew = RandomLevelGenerator.GenerateRandomLevel();

                PFNew.BOT.Iterate(g =>
                {
                    if (g != PFNew.CurrentPlayer)
                        pf.BOT.Add(g);
                });

                pf.CurrentPlayer = PFNew.CurrentPlayer;
            }
        }
    }
}
