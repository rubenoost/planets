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

            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.R) field.CurrentPlayer.mass = 1.0; };
            GameView.KeyUp += OnGameViewOnKeyUp;
            GameView.KeyUp += OnViewOnKeyUp;
            GameView.Click += delegate { OnViewOnKeyUp(null, new KeyEventArgs(Keys.L)); };

            running = false;
            GameThread = new Thread(GameLoop);
            GameThread.Start();

            //Hier komt code voor automatisch opstarten auto-demo
            OnGameViewOnKeyUp(null, new KeyEventArgs(Keys.K));
        }

        private void OnViewOnKeyUp(object sender, KeyEventArgs args)
        {
            if (args.KeyData == Keys.L && ad.Kpressed == true && ad.running == true)
            {
                Debug.AddMessage("Stopping demo");
                ad.Stop();

                // Little hack
                field.CurrentPlayer.mass = 0;
            }
        }

        private void OnGameViewOnKeyUp(object sender, KeyEventArgs args)
        {
            if (args.KeyData == Keys.K && ad.Kpressed == false && ad.running == false)
            {
                Debug.AddMessage("Starting demo");
                adthread = new Thread(ad.Run);
                ad.Start();
                adthread.Start();
            }
        }

        public void Start()
        {
            this.running = true;
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
