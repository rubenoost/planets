using Planets.Controller.GameRules.Abstract;
using Planets.Controller.GameRules.GameTargets;
using Planets.Model;

namespace Planets.Controller.GameRules.Impl
{
    public class ResetRule : INativeGameRule
    {
        private static readonly double MassTreshold = 500.0d;

        private IGameTarget _currentGameTarget = new GameTargetGetLargest();

        public void Execute(GameEngine ge, double ms)
        {
            // Check if player is too small
            if (ge.field.CurrentPlayer.Mass < MassTreshold)
            {
                // Show death screen
                return;
            }

            // Check if target is reached
            if (_currentGameTarget.IsTargetReached(ge))
            {
                // Show level won screen
                return;
            }

            // Else do nothing
        }


    }
}
