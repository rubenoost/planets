using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using Planets.Controller.Subcontrollers;
using Planets.View;
using Planets.Model;
using System.Runtime.InteropServices;

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

                    if (loopcount > 0)
                    {
                        DeltaT = DateTime.Now - LoopBegin;
                    }

                    LoopBegin = DateTime.Now;

                    // MOCHT GAMELOOP SNELLER ZIJN DAN +- 17MS -> DAN WACHTEN MET UPDATEN TOT 17MS is bereikt! ANDERS MEER DAN 60 FPS!!
                    if (DeltaT.Milliseconds > 1000 / 60)
                    {
                        Thread.Sleep(1);
                    }

                    // Check every obj for field limits

                    for (int i = 0; i < field.GameObjects.Count; i++)
                    {
                        GameObject obj = field.GameObjects[i];
                        if (obj == null) continue; // TODO Remove hack
                        Vector newLoc = obj.CalcNewLocation();
                        if (!CheckXCollision(newLoc))
                            obj.InvertObjectX();
                        if (!CheckYCollision(newLoc))
                            obj.InvertObjectY();

                        obj.UpdateLocation();
                    }

                    // PLAATS GAMELOOP HIER, voor allereerste loop is DELTA T niet beschikbaar! Bedenk dus een vaste waarde voor eerste loop!?

                    // Update shizzle hier.
                    GameView.Invalidate();
                }
                loopcount = 0;
                Thread.Sleep(1);
            }
        }

        private bool CheckXCollision(Vector location)
        {
            return (location.X > 0 && location.X < this.HostForm.Size.Width);
        }

        private bool CheckYCollision(Vector location)
        {
            return (location.Y > 0 && location.Y < this.HostForm.Size.Height);
        }
    }
}
