using System.Drawing;
using System.Windows.Forms;

namespace Planets.Controller
{
    public class MainEngine
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
            uc.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            host.Controls.Clear();
            host.Controls.Add(uc);
        }
    }
}
