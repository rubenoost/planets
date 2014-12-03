using System;
using System.Diagnostics;
using System.Windows.Forms;
using Planets.Controller;

namespace Planets
{
    public partial class PlanetsForm : Form
    {

        MainEngine engine;

        public PlanetsForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            engine = new MainEngine(this);

            // Event handlers
            Closed += (sender, args) => Process.GetCurrentProcess().Kill();
        }
    }
}
