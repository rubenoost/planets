using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Planets.Controller
{
    class GameEngine
    {

        private MainEngine HostEngine;
        private PlanetsForm HostForm;

        private int MouseX;
        private int MouseY;

		private bool running;
        private Thread GameThread;

        public GameEngine(MainEngine HostEngine, PlanetsForm HostForm)
        {
            this.HostEngine = HostEngine;
            this.HostForm = HostForm;

            this.HostForm.Click += Form_Click;
            this.HostForm.MouseDown += Form_MouseDown;

			running = false;
            GameThread = new Thread(GameLoop);
            GameThread.Start();
        }

        private void Form_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Form Clicked = " + MouseX + " / " + MouseY);
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            MouseX = e.X;
            MouseY = e.Y;
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

                    if(loopcount > 0)
                    {
                        DeltaT = DateTime.Now - LoopBegin;
                    }

                    LoopBegin = DateTime.Now;

                    // MOCHT GAMELOOP SNELLER ZIJN DAN +- 17MS -> DAN WACHTEN MET UPDATEN TOT 17MS is bereikt! ANDERS MEER DAN 60 FPS!!

                    // PLAATS GAMELOOP HIER, voor allereerste loop is DELTA T niet beschikbaar! Bedenk dus een vaste waarde voor eerste loop!?

                    loopcount++;
                }
                loopcount = 0;
                Thread.Sleep(50);
            }
        }
    }
}
