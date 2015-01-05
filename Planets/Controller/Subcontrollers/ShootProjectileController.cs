using System;
using System.Drawing;
using System.Windows.Forms;
using Planets.Model;
using Planets.Model.GameObjects;
using Planets.View;

namespace Planets.Controller.Subcontrollers
{
    /// <summary>
    /// ShootProjectileController is used to let the Player inside a playfield shoot a projectile by clicking.
    /// </summary>
    public class ShootProjectileController
    {
        private readonly GameEngine _ge;

        /// <summary>
        /// The playfield used by this controller to shoot projectiles.
        /// </summary>
        public Playfield InternalPlayfield { get { return _ge.Field; } }

        /// <summary>
        /// The control used by this controller to listen on for mouse clicks.
        /// </summary>
        public GameView InternalControl { get; private set; }

        /// <summary>
        /// Create new ShootProjectileController.
        /// </summary>
        /// <param name="ge">The gameengine to shoot projectiles in.</param>
        /// <param name="listenControl">The control to listen on for clicks.</param>
        public ShootProjectileController(GameEngine ge, GameView listenControl)
        {
            // Save variables
            this._ge = ge;
            InternalControl = listenControl;

            // Register event handlers
            listenControl.MouseClick += (sender, args) => Clicked(InternalControl.ScreenToGame(args.Location));

            InternalControl.MouseDown += MouseDownEvent;
            InternalControl.MouseUp += MouseUpEvent;
        }

        private void MouseUpEvent(object sender, MouseEventArgs e)
        {
            if (InternalPlayfield.CurrentPlayer.GameOver || InternalPlayfield.CurrentPlayer.GameWon)
            {
                if (InternalControl.PrevClickNext)
                {
                    InternalControl.PrevClickNext = false;
                    InternalControl.ClickOnNextButton = false;
                    _ge.LoadNextLevel();
                    InternalControl.Invalidate();
                }
            }
            else
                InternalControl.IsAiming = false;
        }

        private void MouseDownEvent(object sender, MouseEventArgs e)
        {
            if (InternalPlayfield.CurrentPlayer.GameOver || InternalPlayfield.CurrentPlayer.GameWon)
            {
                Rectangle mouseRec = new Rectangle(new Point(e.X, e.Y), new Size(10, 10));
                Rectangle levelButtonRec = new Rectangle(new Point(175, 400), new Size(430, 100));

                InternalControl.ClickOnNextButton = mouseRec.IntersectsWith(levelButtonRec);

                InternalControl.Invalidate();
            }
            else
            {
                InternalControl.IsAiming = true;
                InternalControl.ClickOnNextButton = false;
            }
        }

        public static Vector CalcNewDv(GameObject player, GameObject projectile, Point p)
        {
            //Projectile being shot
            GameObject proj = projectile;
            GameObject o = player;

            //set velocity projectile
            Vector temp1 = p - o.Location;
            temp1 = temp1.ScaleToLength(100.0);
            proj.Dv = o.Dv + temp1;

            //Set projectile location
            proj.Location = o.Location + temp1.ScaleToLength(o.Radius + proj.Radius + 1);

            //set the velocity of the new player
            return o.Dv - temp1 * Math.Sqrt(proj.Mass / o.Mass);
        }

        /// <summary>
        /// Method called when click happens in listenControl.
        /// </summary>
        /// <param name="gamePoint"></param>
        public void Clicked(Point gamePoint)
        {
            GameObject p;
            //Player
            GameObject o = InternalPlayfield.CurrentPlayer;

            bool isBlackhole = false;
            bool isAntiMatter = false;

            //Projectile being shot
            Random rnd = new Random();
            int rndint = rnd.Next(0, 100);
            if (rndint == 58)
            {
                p = new BlackHole(new Vector(0, 0), new Vector(0, 0), 0) { Mass = 100 };
                isBlackhole = true;
            }
            else if (rndint == 20 || rndint == 25 || rndint == 30 || rndint == 35 || rndint == 40 || rndint == 50 || rndint == 55 || rndint == 60)
            {
                p = new AntiMatter(new Vector(0, 0), new Vector(0, 0), 0) { Mass = o.Mass * 0.05 };
                o.Mass -= p.Mass;
                isAntiMatter = true;
            }
            else
            {
                p = new GameObject(new Vector(0, 0), new Vector(0, 0), 0) { Mass = 0.05 * o.Mass };
                o.Mass = o.Mass - p.Mass;
            }

            lock (InternalPlayfield.GameObjects)
            {
                o.Dv = CalcNewDv(o, p, gamePoint);
                if (isBlackhole)
                    p.Mass = 100;

                if (isAntiMatter)
                    p.Mass = 100;

                //set the velocity of the new player
                InternalPlayfield.GameObjects.Add(p);
            }
        }
    }
}
