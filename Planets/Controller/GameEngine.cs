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
        private MainEngine HostEngine;

        // Views
        private GameView GameView;

        // Controllers
        private ShootProjectileController spc;
        private LevelSupplier ls = new LevelSupplier();

        // Model Data
        public Playfield Field;

        // Events
        public event Action<double> GameLoopEvent;

        // Game rules
        private INativeGameRule[] _gameRules =
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
            HostEngine = hostEngine;
            Field = ls.GenerateLevel();

            // Create view
            GameView = new GameView(this);

            // Create controllers
            spc = new ShootProjectileController(this, GameView);
            new Autodemo(spc, this);

            // Set gameview
            HostEngine.SetView(GameView);

            // Adjust playfield
            Field.Size = GameView.Size;

            // Register keys for resetting
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.R) Field.CurrentPlayer.Mass = 1.0; };

            // Increase mass
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.T) Field.CurrentPlayer.Mass *= 1.2; };

            // Decrease mass
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.G) Field.CurrentPlayer.Mass /= 1.2; };
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.Z) GameView.Zoom *= 1.25f; };
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.X) GameView.Zoom *= 0.8f; };

            // Level stuff
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.N) LoadNextLevel(); };
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.M) ls.LevelMode = ls.LevelMode == LevelSupplier.Mode.Random ? LevelSupplier.Mode.Campaign : LevelSupplier.Mode.Random; };

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
            Field = ls.GenerateLevel();
            Running = true;
        }

        public void GameLoop()
        {
            DateTime loopBegin = DateTime.Now;
            TimeSpan DeltaT;

            while (true)
            {
                while (Running)
                {
                    // Bereken DeltaT
                    DeltaT = DateTime.Now - loopBegin;
                    loopBegin = DateTime.Now;

                    // Converteer DeltaT naar ms
                    double dt = DeltaT.TotalMilliseconds;

                    // MOCHT GAMELOOP SNELLER ZIJN DAN +- 17MS -> DAN WACHTEN MET UPDATEN TOT 17MS is bereikt! ANDERS MEER DAN 60 FPS!!
                    double temp1 = dt * 60 / 1000;
                    double temp2 = temp1 - (int)temp1;
                    double temp3 = temp2 * 1000 / 60;
                    Thread.Sleep((int)temp3);

                    Field.sb.CheckStamps();

                    // Lock BOT
                    lock (Field.BOT)
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
                    GameView.Invalidate();
                }
            }
        }
    }
}
