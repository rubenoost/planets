using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planets.Controller
{
    class Autodemo
    {
        internal Model.Playfield field;
        private Controller.Subcontrollers.ShootProjectileController spc; 

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

        public void Run()
        {
            var p = new Point(); var r = new Random();
            while (Kpressed)
            {
                p = new Point(r.Next(0, field.Size.Width), r.Next(0, field.Size.Height));
                for (int i = 0; i < 3; i++)
                {
                    spc.Clicked(p); field.LastAutoClickLocation = p; field.LastAutoClickMoment = DateTime.Now; Thread.Sleep(400);
                }
                Thread.Sleep(1500);
            }
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
