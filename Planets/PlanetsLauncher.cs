using System;
using System.Windows.Forms;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using Planets.Model.GameObjects;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            HostForm = new PlanetsForm();

            Application.Run(HostForm);
        }
    }
}
