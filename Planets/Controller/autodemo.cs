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
        /// Time of last activity
        /// </summary>
        private DateTime _lastActivityTime = DateTime.MinValue;

        public int WaitTimeBetweenClick = 400;

        public int WaitTimeBetweenClicks = 1500;

        /// <summary>
        /// Create new autodemo on given playfield that uses given ShootProjectileController
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ge"></param>
        public Autodemo(ShootProjectileController s, GameEngine ge)
        {
            Spc = s;
            var gv = Spc.InternalControl;

            // Register keys for auto-demo
            gv.KeyUp += delegate(object sender, KeyEventArgs kea) { if (kea.KeyData == Keys.K) StartDemo(); };
            gv.KeyUp += delegate(object sender, KeyEventArgs kea)
            {
                if (kea.KeyData == Keys.L) StopDemo();
            };
            gv.Click += delegate
            {
                StopDemo();
            };

            // Register gamehookloop
            ge.GameLoopEvent += delegate
            {
                if ((DateTime.Now - _lastActivityTime).TotalSeconds > 60)
                {
                    StartDemo();
                }
            };

            // Create thread
            var adthread = new Thread(Run);
            adthread.Start();

            // Start autodemo
            StartDemo();
        }

        /// <summary>
        /// Whether this autodemo is running
        /// </summary>
        private bool Running;

        /// <summary>
        /// Run the autodemo
        /// </summary>
        private void Run()
        {
            // Create vars
            var r = new Random();

            while (true)
            {
                // While key is pressed
                while (Running)
                {
                    Spc.InternalControl.IsAiming = true;
                    // Determine next click
                    var p = new Point(r.Next(0, Spc.InternalPlayfield.Size.Width), r.Next(0, Spc.InternalPlayfield.Size.Height));

                    // Click 3 times and wait
                    for (int i = 0; i < 3; i++)
                    {
                        // Click
                        if (Running)
                        {
                            Spc.Clicked(p);
                            Spc.InternalPlayfield.LastAutoClickGameLocation = p;
                            Spc.InternalPlayfield.LastAutoClickMoment = DateTime.Now;
                            Thread.Sleep(WaitTimeBetweenClick);
                        }
                    }
                    Thread.Sleep(WaitTimeBetweenClicks);
                    Spc.InternalControl.IsAiming = false;
                }

                // Set running to false
                Running = false;

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Stop the auto-demo
        /// </summary>
        public void StopDemo()
        {
            _lastActivityTime = DateTime.Now;
            // If keys ok
            if (Running)
            {
                // Stop demo
                Running = false;

                // Change lastautoclicklocation
                Spc.InternalPlayfield.LastAutoClickMoment = DateTime.MinValue;
            }
        }

        /// <summary>
        /// Start the demo
        /// </summary>
        private void StartDemo()
        {
            // If keys ok
            if (!Running)
            {
                // Start demo
                Running = true;
            }
        }

    }
}
