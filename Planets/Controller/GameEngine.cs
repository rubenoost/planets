using System;
using System.Threading;
using System.Windows.Forms;
using Planets.Controller.PhysicsRules;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using Planets.View;
using Planets.View.Imaging;

namespace Planets.Controller
{
    public class GameEngine
    {
        // Hosts
        private MainEngine HostEngine;
        private PlanetsForm HostForm;

        // Views
        private GameView GameView;

        // Controllers
        private ShootProjectileController spc;
        private Autodemo ad;

        // Model Data
        public Playfield field;

        // Events
        public event Action<double> GameLoopEvent;

        // Game rules
        private AbstractGameRule[] _gameRules =
        {
            // ========== [ ANTAGONIST BEHAVIOUR ] ==========
            new AIrule(),
            // ========== [ CHANGE SPEED ] ==========
            new BlackHoleRule(),
			//new AntiGravityRule(),

            // ========== [ CHANGE LOCATION ] ==========
            new MoveRule(),

            // ========== [ REMOVING OBJECTS ] ==========
            //new CollidewithSmaller(),
            new DynamicEatRule(),
			new ExplosionRule(),

            // ========== [ CHANGE SPEED ON COLLISION RULE ] ==========
            new ElasticCollisionRule(),

            // ========== [ SLOW OBJECT ] ==========
            //new StasisRule(),

            // ========== [ TARDIS ] ==========
            new TardisRule(),

            // ========== [ DO NOT TOUCH NEXT RULES ] ==========
            new StayInFieldRule()
        };

        private Thread GameThread;
        private bool running = true;

        public GameEngine(MainEngine HostEngine, PlanetsForm HostForm)
        {
            this.HostEngine = HostEngine;
            this.HostForm = HostForm;
            field = RandomLevelGenerator.GenerateRandomLevel();

            // Create view
            GameView = new GameView(field);

            // Create controllers
            spc = new ShootProjectileController(field, GameView);
            ad = new Autodemo(spc, this);

            // Set gameview
            this.HostEngine.SetView(GameView);

            // Adjust playfield
            field.Size = GameView.Size;

            // Register keys for resetting
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.R) field.CurrentPlayer.Mass = 0.0; };
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.B) Debug.ShowWindow();};

            // Increase mass
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.T) field.CurrentPlayer.Mass *= 1.2; };
            // Decrease mass
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.G) field.CurrentPlayer.Mass /= 1.2; };
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.Z) GameView.Zoom *= 1.25f; };
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.X) GameView.Zoom *= 0.8f; };

            // Create new GameThread
            GameThread = new Thread(GameLoop);

            // Start GameThread
            GameThread.Start();
        }

        public void Start()
        {
            running = true;
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

                    // Lock BOT
                    lock (field.BOT)
                    {
                        // ExecuteRule game rules
                        foreach (var agr in _gameRules)
                        {
                            agr.Execute(field, dt);
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
