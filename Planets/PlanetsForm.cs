using System.Diagnostics;
using System.Windows.Forms;
using Planets.Controller;

namespace Planets
{
    public partial class PlanetsForm : Form
    {

        public PlanetsForm()
        {
            InitializeComponent();
            DoubleBuffered = true;

            // Event handlers
            Closed += (sender, args) => Process.GetCurrentProcess().Kill();
        }
    }
}
