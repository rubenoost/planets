using System;
using System.Drawing;
using System.Threading;

namespace Planets.Controller
{
    public class Autodemo
    {
        public Model.Playfield field;
        public Controller.Subcontrollers.ShootProjectileController spc;

        public Autodemo(Model.Playfield p, Controller.Subcontrollers.ShootProjectileController s)
        {
            field = p;
            spc = s;
        }

        public bool Kpressed
        {
            get;
            private set;
        }

        public bool running
        {
            get;
            set;
        }

        public void Run()
        {
            var p = new Point(); var r = new Random();
            while (Kpressed)
            {
                this.running = true;
                p = new Point(r.Next(0, field.Size.Width), r.Next(0, field.Size.Height));
                for (int i = 0; i < 3; i++)
                {
                    spc.Clicked(p); field.LastAutoClickLocation = p; field.LastAutoClickMoment = DateTime.Now; Thread.Sleep(400);
                }
                Thread.Sleep(1500);
            }
            this.running = false;
        }

        public void Stop()
        {
            Kpressed = false;
        }
        public void Start()
        {
            Kpressed = true;
        }

    }
}
