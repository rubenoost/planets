using System;
using System.Drawing;
using System.Windows.Forms;
using Planets.Model;
using Planets.View;

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
        public GameView InternalControl { get; private set; }

        /// <summary>
        /// Create new ShootProjectileController.
        /// </summary>
        /// <param name="pf">The playfield to shoot projectiles in.</param>
        /// <param name="listenControl">The control to listen on for clicks.</param>
        public ShootProjectileController(Playfield pf, GameView listenControl)
        {
            // Save variables
            InternalPlayfield = pf;
            InternalControl = listenControl;

            // Register event handlers
            listenControl.MouseClick += (sender, args) => Clicked(args.Location);

            this.InternalControl.MouseDown += MouseDownEvent;
            this.InternalControl.MouseUp += MouseUpEvent;
        }

        private void MouseUpEvent(object sender, MouseEventArgs e)
        {
            this.InternalControl.IsAiming = false;
        }

        private void MouseDownEvent(object sender, MouseEventArgs e)
        {
            this.InternalControl.IsAiming = true;
        }

        public static Vector CalcNewDV(GameObject Player, GameObject Projectile, Point p)
        {
            //Projectile being shot
            GameObject P = Projectile;
            GameObject O = Player;

            //set velocity projectile
            Vector temp1 = p - O.Location;
            temp1 = temp1.ScaleToLength(100.0);
            P.DV = O.DV + temp1;

            //Set projectile location
            P.Location = O.Location + temp1.ScaleToLength(O.Radius + P.Radius + 1);

            //set the velocity of the new player
            return O.DV - temp1 * Math.Sqrt(P.mass / O.mass);
        }

        /// <summary>
        /// Method called when click happens in listenControl.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Clicked(Point p)
        {
            GameObject P;
            //Player
            GameObject O = InternalPlayfield.CurrentPlayer;

            bool IsBlackhole = false;

            //Projectile being shot
            Random rnd = new Random();
            int rndint = rnd.Next(0, 100);
            if (rndint == 56 || rndint == 57 || rndint == 58 || rndint == 42)
            {
                P = new BlackHole(new Vector(0, 0), new Vector(0, 0), 0);
                P.mass = 0.05 * O.mass;
                IsBlackhole = true;
            }
            else
            {
                P = new GameObject(new Vector(0, 0), new Vector(0, 0), 0);
                P.mass = 0.05 * O.mass;
                O.mass = O.mass - P.mass;
            }


            lock (InternalPlayfield.BOT)
            {
                O.DV = CalcNewDV(O, P, p);
                if(IsBlackhole)
                    P.mass = 1000000;

                //set the velocity of the new player
                InternalPlayfield.BOT.Add(P);
            }
        }
    }
}
