using System.Drawing;
using System.Windows.Forms;

namespace Planets.Controller
{
    class MainEngine
    {

        private PlanetsForm host;
        private GameEngine GameEngine;

        public MainEngine(PlanetsForm host)
        {
            this.host = host;
            GameEngine = new GameEngine(this, this.host);

            GameEngine.Start();
        }

        public void SetView(UserControl uc)
        {
            uc.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            this.host.Controls.Clear();
            this.host.Controls.Add(uc);
        }
    }
}
