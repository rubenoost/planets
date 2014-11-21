using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Planets.Controller.PhysicsRules;
using Planets.Controller.Subcontrollers;
using Planets.View;
using Planets.Model;

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

        // Model Data
        private Playfield field;

        // Game rules
        private AbstractGameRule[] _gameRules =
        {
            // Movement Rules (will change location of GameObjects)
            new MoveRule(),

            // Collision Rules (can change location of GameObjects)
            new BlackHoleRule(),
            

            // Do not touch the next rules, these should have the final word about movement
            new ElasticCollisionRule(),

            // Effect rules (cannot change location of GameObjects)
            new StayInFieldRule(), 

            // Reset rule
            new ResetRule(), 
        };

        // Variables
        private bool running;
        private Thread GameThread;

        public GameEngine(MainEngine HostEngine, PlanetsForm HostForm)
        {
            this.HostEngine = HostEngine;
            this.HostForm = HostForm;
            this.field = new Playfield(1920, 1080);
            this.field.CurrentPlayer = new Player(200, 200, 0, 0, Utils.StartMass);

            GameView = new GameView(this.field);

            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.R) field.CurrentPlayer.mass = 1.0; };
            GameView.KeyDown += delegate(object sender, KeyEventArgs args) { if (args.KeyData == Keys.K) { new Thread(delegate() { var p = new Point(); var r = new Random(); while (true) { p = new Point(r.Next(0, field.Size.Width), r.Next(0, field.Size.Height)); for (int i = 0; i < 3; i++) { spc.Clicked(p); field.LastAutoClickLocation = p; field.LastAutoClickMoment = DateTime.Now; Thread.Sleep(400); } Thread.Sleep(1500); } }).Start(); } };


            // Create new ShootProjectileController
            spc = new ShootProjectileController(field, GameView);

            this.HostEngine.SetView(GameView);

            // Adjust playfield
            field.Size = GameView.Size;

            running = false;
            GameThread = new Thread(GameLoop);
            GameThread.Start();
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
                        foreach (AbstractGameRule agr in _gameRules)
                        {
                            if (agr.Activated) agr.Execute(field, DeltaT.TotalMilliseconds);
                        }
                    }

                    // Update shizzle hier.
                    GameView.Invalidate();
                }
            }
        }
    }
}
