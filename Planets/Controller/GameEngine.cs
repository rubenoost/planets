using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
        private AbstractGameRule[] _gameRules = {new MoveRule(), new StayInFieldRule(1920, 1080), new ElasticCollisionRule()};

        // Variables
        private bool running;
        private Thread GameThread;

        public GameEngine(MainEngine HostEngine, PlanetsForm HostForm)
        {
            this.HostEngine = HostEngine;
            this.HostForm = HostForm;
            this.field = new Playfield();
            this.field.CurrentPlayer = new Player(200, 200, 0, 0, Utils.StartMass);

            this.GameView = new GameView(this.field);

            // Create new ShootProjectileController
            spc = new ShootProjectileController(field, GameView);

            this.HostEngine.SetView(GameView);

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
            TimeSpan DeltaT = new TimeSpan(1000 / 60);

            int loopcount = 0;

            while (true)
            {
                while (running)
                {


                    DeltaT = DateTime.Now - LoopBegin;


                    LoopBegin = DateTime.Now;

                    // MOCHT GAMELOOP SNELLER ZIJN DAN +- 17MS -> DAN WACHTEN MET UPDATEN TOT 17MS is bereikt! ANDERS MEER DAN 60 FPS!!
                    while (DeltaT.Milliseconds > 1000 / 60)
                    {
                        Thread.Sleep(1);
                    }

                    // Lock GameObjects
                    lock (field.GameObjects)
                    {
                        // Execute game rules
                        foreach (AbstractGameRule agr in _gameRules)
                        {
                            agr.Execute(field, DeltaT.TotalMilliseconds);
                        }
                    }

                    // PLAATS GAMELOOP HIER, voor allereerste loop is DELTA T niet beschikbaar! Bedenk dus een vaste waarde voor eerste loop!?

                    // Update shizzle hier.
                    GameView.Invalidate();
                }
                loopcount = 0;
                Thread.Sleep(1);
            }
        }
    }
}
