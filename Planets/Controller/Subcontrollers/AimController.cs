using Planets.Model;
using Planets.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planets.Controller.Subcontrollers
{
    class AimController
    {

        private Playfield pf;
        private GameView listenControl;

        private bool PrevMouseDown;
        private bool MouseUp;

        public AimController(Playfield pf, GameView listenControl)
        {
            this.pf = pf;
            this.listenControl = listenControl;

            this.listenControl.MouseDown += MouseDownEvent;
            this.listenControl.MouseUp += MouseUpEvent;
        }

        private void MouseUpEvent(object sender, MouseEventArgs e)
        {
            this.listenControl.IsAiming = false;
        }

        private void MouseDownEvent(object sender, MouseEventArgs e)
        {
            this.listenControl.IsAiming = true;
        }

    }
}
