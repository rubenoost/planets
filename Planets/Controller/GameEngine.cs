using System;
using System.Windows.Forms;

namespace Planets.Controller
{
    class GameEngine
    {

        private MainEngine HostEngine;
        private PlanetsForm HostForm;

        private int MouseX;
        private int MouseY;

        public GameEngine(MainEngine HostEngine, PlanetsForm HostForm)
        {
            this.HostEngine = HostEngine;
            this.HostForm = HostForm;

            this.HostForm.Click += Form_Click;
            this.HostForm.MouseDown += Form_MouseDown;
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

        public void GameLoop()
        {

        }
    }
}
