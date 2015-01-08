using System;
using System.Threading;
using System.Windows.Forms;
using Planets.Controller.GameRules.Abstract;
using Planets.Controller.GameRules.Impl;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using Planets.View;

namespace Planets.Controller
{
    public class GameEngine
    {
        // Hosts
        private readonly MainEngine _hostEngine;

        // Views
        public readonly GameView _gameView;

        // Controllers
        private readonly ShootProjectileController _spc;
        private readonly LevelSupplier _ls = new LevelSupplier();
        public readonly Autodemo Autodemo;

        // Model Data
        public Playfield Field;

        // Events
        public event Action<double> GameLoopEvent;

        // Game rules
        private readonly INativeGameRule[] _gameRules =
        {
            // ========== [ ANTAGONIST BEHAVIOUR ] ==========
            new AIrule(),

            // ========== [ CHANGE SPEED ] ==========
            new BlackHoleRule(),
			new AntiGravityRule(),

            // ========== [ CHANGE LOCATION ] ==========
            new MoveRule(),

            // ========== [ REMOVING OBJECTS ] ==========
            //new CollidewithSmaller(),
            new DynamicEatRule(),
			new ExplosionRule(),
            new BlackHoleEatRule(), 

            // ========== [ CHANGE SPEED ON COLLISION RULE ] ==========
            new ElasticCollisionRule(),

            // ========== [ SLOW OBJECT ] ==========
            new StasisRule(),

            // ========== [ TARDIS ] ==========
            new BonusRule(),

            // ========== [ DO NOT TOUCH NEXT RULES ] ==========
            new StayInFieldRule(),
            new ResetRule(),
            new BugfixRule()
        };

        public Thread GameThread;
        public bool Running = true;

        public GameEngine(MainEngine hostEngine)
        {
            _hostEngine = hostEngine;
            Field = _ls.GenerateLevel();

            // Create view
            _gameView = new GameView(this);

            // Create controllers
            _spc = new ShootProjectileController(this, _gameView);
            Autodemo = new Autodemo(_spc, this);

            // Set gameview
            _hostEngine.SetView(_gameView);

            // Adjust playfield
            Field.Size = _gameView.Size;

            // Register keys for resetting
            _gameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.R) Field.CurrentPlayer.Mass = 1.0; };

            // Increase mass
            _gameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.T) Field.CurrentPlayer.Mass *= 1.2; };

            // Decrease mass
            _gameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.G) Field.CurrentPlayer.Mass /= 1.2; };
            _gameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.Z) _gameView.Zoom *= 1.25f; };
            _gameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.X) _gameView.Zoom *= 0.8f; };

            // Level stuff
            _gameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.N) LoadNextLevel(); };
            _gameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.M) _ls.LevelMode = _ls.LevelMode == LevelSupplier.Mode.Random ? LevelSupplier.Mode.Campaign : LevelSupplier.Mode.Random; };

            // Create new GameThread
            GameThread = new Thread(GameLoop);

            // Start GameThread
            GameThread.Start();
        }

        public void Start()
        {
            Running = true;
        }

        public void LoadNextLevel()
        {
            Field = _ls.GenerateLevel();
            Running = true;
        }

        public void GameLoop()
        {
            DateTime loopBegin = DateTime.Now;
            TimeSpan deltaT;

            while (true)
            {
                while (Running)
                {
                    // Bereken DeltaT
                    deltaT = DateTime.Now - loopBegin;
                    loopBegin = DateTime.Now;

                    // Converteer DeltaT naar ms
                    double dt = deltaT.TotalMilliseconds;

                    // MOCHT GAMELOOP SNELLER ZIJN DAN +- 17MS -> DAN WACHTEN MET UPDATEN TOT 17MS is bereikt! ANDERS MEER DAN 60 FPS!!
                    double temp1 = dt * 60 / 1000;
                    double temp2 = temp1 - (int)temp1;
                    double temp3 = temp2 * 1000 / 60;
                    Thread.Sleep((int)temp3);

                    Field.ScoreBoard.CheckStamps();

                    // Lock BOT
                    lock (Field.GameObjects)
                    {
                        // ExecuteRule game rules
                        foreach (var agr in _gameRules)
                        {
                            agr.Execute(this, dt);
                        }
                    }

                    // Execute gameloop hook
                    if (GameLoopEvent != null)
                        GameLoopEvent(dt);

                    // Update shizzle hier.
                    _gameView.Invalidate();
                }
            }
        }
    }
}
