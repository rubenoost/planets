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
        public GameEngine(MainEngine HostEngine, PlanetsForm HostForm)
        {
            this.HostEngine = HostEngine;
            this.HostForm = HostForm;

            this.HostForm.Click += Form_Click;
            this.HostForm.MouseDown += Form_MouseDown;

			running = false;
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
			Stopwatch timer = new Stopwatch();
			timer.Start();

			double lastTime = timer.Elapsed.TotalMilliseconds;
			double delta = 0;
			double unproccessedTime = 0;

			while (running) {
				double startTime = timer.Elapsed.TotalMilliseconds;
				double passedTime = startTime - lastTime;
				lastTime = startTime;

				unproccessedTime += passedTime / 10000;

				while (unproccessedTime > 60) {
					unproccessedTime -= 60;

					// Update shizzle hier..
				}
			}

			timer.Stop();
        }
    }
}
