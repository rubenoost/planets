using System;
using System.Windows.Forms;

namespace Planets
{
    static class PlanetsLauncher
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PlanetsForm());

        }
    }
}
