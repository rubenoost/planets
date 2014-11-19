using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planets.Controller
{
    class MainEngine
    {

        private PlanetsForm host;

        public MainEngine(PlanetsForm host)
        {
            this.host = host;
        }

        public void SetView(UserControl uc)
        {
            this.host.Controls.Clear();
            this.host.Controls.Add(uc);
        }


    }
}
