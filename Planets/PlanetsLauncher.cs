using System;
using System.Windows.Forms;

namespace Planets
{
    static class PlanetsLauncher
    {
        public static Form HostForm { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Setup application parameters
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create HostForm
            HostForm = new PlanetsForm();

            // Run application
            Application.Run(HostForm);
        }
    }
}
