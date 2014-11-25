using System;
using System.Threading;
using System.Windows.Forms;
using Planets.Controller.PhysicsRules;
using Planets.Controller.Subcontrollers;
using Planets.View;
using Planets.Model;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace Planets.Controller
{
    class GameEngine
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
        private Playfield field;

        // Game rules
        private AbstractGameRule[] _gameRules =
        {
            // ========== [ CHANGE SPEED ] ==========
            new BlackHoleRule(),

            // ========== [ CHANGE LOCATION ] ==========
            new MoveRule(),

            // ========== [ REMOVING OBJECTS ] ==========
            new EatRule(), 

            // ========== [ CHANGE SPEED ON COLLISION RULE ] ==========
            new ElasticCollisionRule(),

            // ========== [ DO NOT TOUCH NEXT RULES ] ==========
            new StayInFieldRule(), 
            new ResetRule(), 
        };

        // Variables
        private bool running;
        private Thread GameThread;
        private Thread adthread;
        
        public GameEngine(MainEngine HostEngine, PlanetsForm HostForm)
        {
            this.HostEngine = HostEngine;
            this.HostForm = HostForm;
            this.field = new Playfield(1920, 1080);
            this.field.CurrentPlayer = new Player(new Vector(0, 0), new Vector(0, 0), 0);

            GameView = new GameView(this.field);

            // Create new ShootProjectileController
            spc = new ShootProjectileController(field, GameView);

            this.HostEngine.SetView(GameView);

            // Adjust playfield
            field.Size = GameView.Size;

            //Create a Auto demo refrence for the auto-demo
            this.ad = new Autodemo(this.field, this.spc);

            // Register keys for resetting
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.R) field.CurrentPlayer.mass = 0.0; };

            // Register keys for auto-demo
            GameView.KeyUp += StartDemo;
            GameView.KeyUp += StopDemo;
            GameView.Click += delegate { StopDemo(null, new KeyEventArgs(Keys.L)); };

            // Set running to false
            running = false;

            // Create new GameThread
            GameThread = new Thread(GameLoop);

            // Start GameThread
            GameThread.Start();

            //Hier komt code voor automatisch opstarten auto-demo
            StartDemo(null, new KeyEventArgs(Keys.K));
        }

        /// <summary>
        /// Stop the auto-demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StopDemo(object sender, KeyEventArgs args)
        {
            // If keys ok
            if (args.KeyData == Keys.L && ad.Kpressed && ad.running)
            {
                // Debug message
                Debug.AddMessage("Stopping demo");

                // Stop demo
                ad.Stop();

                // Little hack
                field.CurrentPlayer.mass = 0;
            }
        }

        /// <summary>
        /// Start the demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StartDemo(object sender, KeyEventArgs args)
        {
            // If keys ok
            if (args.KeyData == Keys.K && !ad.Kpressed && !ad.running)
            {
                // Debug message
                Debug.AddMessage("Starting demo");

                // Start demo
                adthread = new Thread(ad.Run);
                ad.Start();
                adthread.Start();
            }
        }

        public void Start()
        {
            running = true;
        }

        public void GameLoop()
        {
            DateTime LoopBegin = DateTime.Now;
            TimeSpan DeltaT;

            int loopcount = 0;

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

                    // Lock GameObjects
                    lock (field.GameObjects)
                    {
                        // ExecuteRule game rules
                        foreach (var agr in _gameRules)
                        {
                            agr.Execute(field, dt);
                        }
                    }

                    // Update shizzle hier.
                    GameView.Invalidate();
                }
            }
        }
    }
}
