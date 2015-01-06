using System.Drawing;
using System.Windows.Forms;

namespace Planets.Controller
{
    public class MainEngine
    {

        private readonly PlanetsForm _host;
        private readonly GameEngine _gameEngine;

        public MainEngine(PlanetsForm host)
        {
            this._host = host;
            _gameEngine = new GameEngine(this);

            _gameEngine.Start();
        }

        // Set the Form view to a new UserControl
        public void SetView(UserControl uc)
        {
            uc.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            _host.Controls.Clear();
            _host.Controls.Add(uc);
        }
    }
}
