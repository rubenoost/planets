using System;
using System.Drawing;
using System.Windows.Forms;
using Planets.Model;

namespace Planets.Controller.Subcontrollers
{
    /// <summary>
    /// ShootProjectileController is used to let the Player inside a playfield shoot a projectile by clicking.
    /// </summary>
    public class ShootProjectileController
    {
        /// <summary>
        /// The playfield used by this controller to shoot projectiles.
        /// </summary>
        public Playfield InternalPlayfield { get; private set; }

        /// <summary>
        /// The control used by this controller to listen on for mouse clicks.
        /// </summary>
        public Control InternalControl { get; private set; }

        /// <summary>
        /// Create new ShootProjectileController.
        /// </summary>
        /// <param name="pf">The playfield to shoot projectiles in.</param>
        /// <param name="listenControl">The control to listen on for clicks.</param>
        public ShootProjectileController(Playfield pf, Control listenControl)
        {
            // Save variables
            InternalPlayfield = pf;
            InternalControl = listenControl;

            // Register event handlers
            listenControl.MouseClick += (sender, args) => Clicked(args.Location);
        }

        /// <summary>
        /// Method called when click happens in listenControl.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Clicked(Point p)
        {
            //Projectile being shot
            GameObject P = new GameObject(0, 0, 0, 0, 0, true);

            //Player
            GameObject O = InternalPlayfield.CurrentPlayer;

            //set the mass of the projectile
            P.mass = 0.05 * O.mass;

            //set velocity projectile
            Vector temp1 = p - O.Location;
            temp1 = temp1.ScaleToLength(100.0);
            P.DV = O.DV + temp1;

            //Set projectile location
            P.Location = O.Location + temp1.ScaleToLength(O.radius + P.radius + 1);

            lock (InternalPlayfield.GameObjects)
            {
                //Set mass of the player
                O.mass = O.mass - P.mass;

                //set the velocity of the new player
                O.DV = O.DV - temp1 * Math.Sqrt(P.mass / O.mass);

                InternalPlayfield.GameObjects.Add(P);

                // Reset if too low
                if (O.mass < 25)
                {
                    InternalPlayfield.GameObjects.Clear();
                    InternalPlayfield.CurrentPlayer = new Player(200, 200, 0, 0, Utils.StartMass);
                }
            }
        }
    }
}
