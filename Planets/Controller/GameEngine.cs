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
        private Autodemo ad;
        private LevelSupplier ls = new LevelSupplier();

        // Model Data
        public Playfield field;

        // Events
        public event Action<double> GameLoopEvent;

        // Game rules
        private INativeGameRule[] _gameRules =
        {
            // ========== [ ANTAGONIST BEHAVIOUR ] ==========
            //new AIrule(),

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
            new ResetRule()
        };

        public Thread GameThread;
        public bool running = true;

        public GameEngine(MainEngine HostEngine)
        {
            this.HostEngine = HostEngine;
            field = ls.GenerateLevel();

            // Create view
            GameView = new GameView(this);

            // Create controllers
            spc = new ShootProjectileController(this, GameView);
            ad = new Autodemo(spc, this);

            // Set gameview
            this.HostEngine.SetView(GameView);

            // Adjust playfield
            field.Size = GameView.Size;

            // Register keys for resetting
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.R) field.CurrentPlayer.Mass = 1.0; };

            // Increase mass
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.T) field.CurrentPlayer.Mass *= 1.2; };

            // Decrease mass
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.G) field.CurrentPlayer.Mass /= 1.2; };
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
            running = true;
        }

        public void LoadNextLevel()
        {
            field = ls.GenerateLevel();
        }

        public void GameLoop()
        {
            DateTime LoopBegin = DateTime.Now;
            TimeSpan DeltaT;

            while (true)
            {
                while (running)
                {
                    // Bereken DeltaT
                    DeltaT = DateTime.Now - LoopBegin;
                    LoopBegin = DateTime.Now;

                    // Converteer DeltaT naar ms
                    double dt = DeltaT.TotalMilliseconds;

                    // MOCHT GAMELOOP SNELLER ZIJN DAN +- 17MS -> DAN WACHTEN MET UPDATEN TOT 17MS is bereikt! ANDERS MEER DAN 60 FPS!!
                    double temp1 = dt * 60 / 1000;
                    double temp2 = temp1 - (int)temp1;
                    double temp3 = temp2 * 1000 / 60;
                    Thread.Sleep((int)temp3);

                    field.sb.CheckStamps();

                    // Lock BOT
                    lock (field.BOT)
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
