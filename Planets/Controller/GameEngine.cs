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
using System.Drawing;

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


                    DeltaT = DateTime.Now - LoopBegin;


                    LoopBegin = DateTime.Now;

                    // MOCHT GAMELOOP SNELLER ZIJN DAN +- 17MS -> DAN WACHTEN MET UPDATEN TOT 17MS is bereikt! ANDERS MEER DAN 60 FPS!!
                    while (DeltaT.Milliseconds > 1000 / 60)
                    {
                        Thread.Sleep(1);
                    }

                    // Check every obj for field limits
                    lock (field.GameObjects)
                    {
                        for (int i = 0; i < field.GameObjects.Count; i++)
                        {
                            GameObject obj = field.GameObjects[i];
                            if (obj == null) continue; // TODO Remove hack
                            Vector newLoc = obj.CalcNewLocation(DeltaT.TotalMilliseconds);
                            if (!FieldXCollission(newLoc, obj.radius))
                                obj.InvertObjectX();
                            if (!FieldYCollission(newLoc, obj.radius))
                                obj.InvertObjectY();

                            for (int j = i + 1; j < this.field.GameObjects.Count; j++)
                            {
                                GameObject SecondObj = this.field.GameObjects[j];
                                if (SecondObj == null) continue;
                                if (obj == SecondObj)
                                    continue;

                                CheckObjectCollission(obj, SecondObj);
                            }
                            obj.UpdateLocation(DeltaT.TotalMilliseconds);
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

        private bool FieldXCollission(Vector location, double radius)
        {
            return (location.X > radius && location.X + radius < this.HostForm.Size.Width);
        }

        private bool FieldYCollission(Vector location, double radius)
        {
            return (location.Y > radius && location.Y + radius < this.HostForm.Size.Height);
        }

        private void CheckObjectCollission(GameObject CurObj, GameObject CheckObj)
        {
            if (CurObj.IntersectsWith(CheckObj))
            {
                Vector i = (CheckObj.Location - CurObj.Location).ScaleToLength((CheckObj.DV - CurObj.DV).Length());
                double totalMass = CheckObj.mass + CurObj.mass;
                Vector i2 = CheckObj.DV + i * (2.0 * CurObj.mass / totalMass);
                Vector i3 = CurObj.DV - i * (CheckObj.mass / totalMass);

                CheckObj.Location = CurObj.Location + i.ScaleToLength(CheckObj.radius + CurObj.radius + 1); // TODO Remove hack

                CheckObj.DV = i2;
                CurObj.DV = i3;
            }
        }
    }
}
