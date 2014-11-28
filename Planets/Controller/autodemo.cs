using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Planets.Controller.Subcontrollers;

namespace Planets.Controller
{
    public class Autodemo
    {
        /// <summary>
        /// The shootprojectilecontroller used by this autodemo
        /// </summary>
        public readonly ShootProjectileController Spc;

        /// <summary>
        /// The GameView
        /// </summary>
        internal Control Gv;

        /// <summary>
        /// Thread for the autodemo
        /// </summary>
        private Thread adthread;

        /// <summary>
        /// Time of last activity
        /// </summary>
        private DateTime lastActivityTime = DateTime.MinValue;

        public int WaitTimeBetweenClick = 400;

        public int WaitTimeBetweenClicks = 1500;

        /// <summary>
        /// Create new autodemo on given playfield that uses given ShootProjectileController
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        public Autodemo(ShootProjectileController s, GameEngine ge)
        {
            Spc = s;
            Gv = Spc.InternalControl;

            // Register keys for auto-demo
            Gv.KeyUp += delegate(object sender, KeyEventArgs kea) { if(kea.KeyData == Keys.K) StartDemo(); };
            Gv.KeyUp += delegate(object sender, KeyEventArgs kea)
            {
                if (kea.KeyData == Keys.L) StopDemo();
            };
            Gv.Click += delegate
            {
                StopDemo();
            };

            // Register gamehookloop
            ge.GameLoopEvent += delegate
            {
                if ((DateTime.Now - lastActivityTime).TotalSeconds > 60)
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
            private set;
        }

        /// <summary>
        /// Run the autodemo
        /// </summary>
        private void Run()
        {
            // Create vars
            var p = new Point();
            var r = new Random();

            this.Spc.InternalControl.IsAiming = true;
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
                            Thread.Sleep(WaitTimeBetweenClick);
                        }
                    }
                    Thread.Sleep(WaitTimeBetweenClicks);
                }

                // Set running to false
                Running = false;

                Thread.Sleep(100);
            }
            this.Spc.InternalControl.IsAiming = false;
        }

        /// <summary>
        /// Stop the auto-demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void StopDemo()
        {
            lastActivityTime = DateTime.Now;
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
                Spc.InternalPlayfield.CurrentPlayer.mass = 1;
            }
        }

        /// <summary>
        /// Start the demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void StartDemo()
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
