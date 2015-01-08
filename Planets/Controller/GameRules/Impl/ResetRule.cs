using System.Timers;
using System.Windows.Forms;
using Planets.Controller.GameRules.Abstract;
using Planets.Controller.GameRules.GameTargets;
using Timer = System.Timers.Timer;

namespace Planets.Controller.GameRules.Impl
{
    public class ResetRule : INativeGameRule
    {
        private static readonly double MassTreshold = 500.0d;

        private readonly IGameTarget _currentGameTarget = new GameTargetGetLargest();

        private readonly Timer _timer;

        private GameEngine _ge;

        public ResetRule()
        {
            _timer = new Timer();
            _timer.Elapsed += TimerCallback;
            _timer.Interval = 25000;
            _timer.AutoReset = false;
        }

        public void Execute(GameEngine ge, double ms)
        {
            _ge = ge;
            // Check if player is too small
            if (ge.Field.CurrentPlayer.Mass < MassTreshold)
            {
                if (ge.Autodemo.Running)
                {
                    ge.LoadNextLevel();
                    return;
                }
                else
                {
                    ge.Field.CurrentPlayer.GameOver = true;
                    ge.Running = false;
                    _timer.Start();
                    return;
                }
            }

            // Check if target is reached
            if (_currentGameTarget.IsTargetReached(ge))
            {
                if (ge.Autodemo.Running)
                {
                    ge.LoadNextLevel();
                }
                else
                {
                    ge.Field.CurrentPlayer.GameWon = true;
                    ge.Running = false;
                    _timer.Start();
                    return;
                }
            }

            // Else do nothing
        }

        private void TimerCallback(object state, ElapsedEventArgs elapsedEventArgs)
        {
            _timer.Stop();
            _ge.Running = true;
            _ge.Field.CurrentPlayer.GameOver = false;
            _ge.Field.CurrentPlayer.GameWon = false;
            _ge._gameView.PrevClickNext = false;
            _ge._gameView.ClickOnNextButton = false;
        }
    }
}
