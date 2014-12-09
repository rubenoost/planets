using Planets.Model;

namespace Planets.Controller.GameRules
{
    class ScoreRule : AbstractGameRule
    {

        protected override void ExecuteRule(Playfield pf, double ms)
        {

            pf.BOT.Iterate(go =>
                {
                    // first GameObject



                });

        }

    }
}
