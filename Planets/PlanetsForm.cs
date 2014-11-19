using System;
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
            engine = new MainEngine(this);
        }

        private void PlanetsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
