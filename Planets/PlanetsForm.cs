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
            Timer t = new Timer { Enabled = true };
            t.Interval = 5000;
            t.Tick += t_Tick;

            t.Start();

            InitializeComponent();
            DoubleBuffered = true;
            engine = new MainEngine(this);

            // Event handlers
            Closed += (sender, args) => Process.GetCurrentProcess().Kill();
        }

        void t_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
