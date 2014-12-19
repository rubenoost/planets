using Planets.Controller.GameRules.Abstract;
using Planets.Controller.GameRules.GameTargets;

namespace Planets.Controller.GameRules.Impl
{
    public class ResetRule : INativeGameRule
    {
        private static readonly double MassTreshold = 500.0d;

        private IGameTarget _currentGameTarget = new GameTargetGetLargest();

        public void Execute(GameEngine ge, double ms)
        {
            // Check if player is too small
            if (ge.Field.CurrentPlayer.Mass < MassTreshold)
            {
                ge.Field.CurrentPlayer.GameOver = true;
                ge.Running = false;
                return;
            }

            // Check if target is reached
            if (_currentGameTarget.IsTargetReached(ge))
            {
                ge.Field.CurrentPlayer.GameWon = true;
                ge.Running = false;
                return;
            }

            // Else do nothing
        }


    }
}
