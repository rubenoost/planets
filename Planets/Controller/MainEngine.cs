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
        }

        public void SetView(UserControl uc)
        {
            this.host.Controls.Clear();
            this.host.Controls.Add(uc);
        }
    }
}
