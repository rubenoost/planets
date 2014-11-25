using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using Planets.View;

namespace Planets.Controller
{
    class Autodemo
    {
        /// <summary>
        /// The shootprojectilecontroller used by this autodemo
        /// </summary>
        internal ShootProjectileController Spc;

        /// <summary>
        /// The GameView
        /// </summary>
        internal GameView Gv;

        /// <summary>
        /// Thread for the autodemo
        /// </summary>
        private Thread adthread;

        /// <summary>
        /// Time of last activity
        /// </summary>
        private DateTime lastActivityTime = DateTime.MinValue;

        /// <summary>
        /// Create new autodemo on given playfield that uses given ShootProjectileController
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        public Autodemo(ShootProjectileController s, GameView gv, GameEngine ge)
        {
            Spc = s;
            Gv = gv;

            // Register keys for auto-demo
            gv.KeyUp += delegate(object sender, KeyEventArgs kea) { if(kea.KeyData == Keys.K) StartDemo(); };
            gv.KeyUp += delegate(object sender, KeyEventArgs kea) { if(kea.KeyData == Keys.L) StopDemo();
                                                                      lastActivityTime = DateTime.Now;
            };
            gv.Click += delegate
            {
                StopDemo();
                lastActivityTime = DateTime.Now;
            };

            // Register gamehookloop
            ge.GameLoopEvent += delegate(double ms)
            {
                if ((DateTime.Now - lastActivityTime).TotalSeconds > 5)
                {
                    StartDemo();
                }
            };

            // Create thread
            adthread = new Thread(Run);
            adthread.Start();

            // Start autodemo
            StartDemo();
        }

        /// <summary>
        /// Whether this autodemo is running
        /// </summary>
        public bool Running
        {
            get;
            set;
        }

        /// <summary>
        /// Run the autodemo
        /// </summary>
        public void Run()
        {
            // Create vars
            var p = new Point();
            var r = new Random();

            while (true)
            {
                // While key is pressed
                while (Running)
                {
                    // Determine next click
                    p = new Point(r.Next(0, Spc.InternalPlayfield.Size.Width),
                        r.Next(0, Spc.InternalPlayfield.Size.Height));

                    // Click 3 times and wait
                    for (int i = 0; i < 3; i++)
                    {
                        // Click
                        if (Running)
                        {
                            Spc.Clicked(p);
                            Spc.InternalPlayfield.LastAutoClickLocation = p;
                            Spc.InternalPlayfield.LastAutoClickMoment = DateTime.Now;
                            Thread.Sleep(400);
                        }
                    }
                    Thread.Sleep(1500);
                }

                // Set running to false
                Running = false;

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Stop the auto-demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StopDemo()
        {
            // If keys ok
            if (Running)
            {
                // Debug message
                Debug.AddMessage("Stopping demo");

                // Stop demo
                Running = false;

                // Change lastautoclicklocation
                Spc.InternalPlayfield.LastAutoClickMoment = DateTime.MinValue;

                // Little hack
                Spc.InternalPlayfield.CurrentPlayer.mass = 0;
            }
        }

        /// <summary>
        /// Start the demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StartDemo()
        {
            // If keys ok
            if (!Running)
            {
                // Debug message
                Debug.AddMessage("Starting demo");

                // Start demo
                Running = true;
            }
        }
    }
}
