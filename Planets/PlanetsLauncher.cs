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
            
            Playfield pf = new Playfield(1920, 1080);
            pf.BOT.Add(new GameObject(new Vector(), new Vector(), Utils.StartMass));
            pf.BOT.Add(new Antagonist(new Vector(), new Vector(), Utils.StartMass));
            pf.BOT.Add(new BlackHole(new Vector(), new Vector(), Utils.StartMass));
            pf.CurrentPlayer = new Player(new Vector(), new Vector(), Utils.StartMass);
            pf.SerializeToFile("C:/DebugData/Planets/pf.xml");

            var p = new Playfield();
            p.DeserializeFromFile("C:/DebugData/Planets/pf.xml");

            p.SerializeToFile("C:/DebugData/Planets/pf2.xml");

            Application.Run(HostForm);
        }
    }
}
