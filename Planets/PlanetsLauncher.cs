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
            // Setup application parameters
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create HostForm
            HostForm = new PlanetsForm();

            // Run application
            Application.Run(HostForm);
        }

        // ReSharper disable once UnusedMember.Local
        private static void Gen()
        {
            var pf = new Playfield();
            // Set player
            Player p = new Player(new Vector((double)pf.Size.Width / 2, (double)pf.Size.Height / 2), new Vector(), 1000);
            pf.CurrentPlayer = p;

            // Generate objects
            double maxMass = 10000.0d;
            double minMass = 100.0d;
            int variance = 10000;
            int saturation = 100000;
            double scale = 1.0d - 1.0d / variance;

            // Create random
            Random r = new Random();
            double currentMass = maxMass;
            for (int i = 0; i < saturation; i++)
            {
                // Get valid gamepoint
                int radius = (int)Math.Sqrt(currentMass / Math.PI);
                int x = r.Next(radius, pf.Size.Width - radius);
                int y = r.Next(radius, pf.Size.Height - radius);
                GameObject go = new GameObject(new Vector(x, y), new Vector(), currentMass);

                // Check if valid
                bool intersects = false;
                pf.GameObjects.Iterate(g =>
                {
                    if (intersects)
                        return;
                    if ((g.Location - go.Location).Length() < g.Radius + go.Radius + 1)
                        intersects = true;
                });

                // Add
                if (!intersects)
                    pf.GameObjects.Add(go);

                // Change mass
                currentMass = Math.Max(minMass, currentMass * scale);
            }

            // Save playfield
            pf.SerializeToFile("C:/DebugData/2.lvl");
        }
    }
}
