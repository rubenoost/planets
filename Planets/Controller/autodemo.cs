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
    class autodemo
    {
        internal Model.Playfield field;
        private Controller.Subcontrollers.ShootProjectileController spc; 

        public autodemo(Model.Playfield p, Controller.Subcontrollers.ShootProjectileController s)
        {
            this.field = p;
            this.spc = s;
        }

        public bool kpressed
        {
            get;
            private set;
        }

        public void run()
        {
            var p = new Point(); var r = new Random();
            while (kpressed)
            {
                p = new Point(r.Next(0, field.Size.Width), r.Next(0, field.Size.Height));
                for (int i = 0; i < 3; i++)
                {
                    this.spc.Clicked(p); this.field.LastAutoClickLocation = p; this.field.LastAutoClickMoment = DateTime.Now; Thread.Sleep(400);
                }
                Thread.Sleep(1500);
            }
        }

        public void stop()
        {
            kpressed = false;
        }
        public void start()
        {
            kpressed = true;
        }

    }
}
