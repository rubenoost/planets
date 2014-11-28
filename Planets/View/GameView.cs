using System;
using System.Drawing.Drawing2D;
using Planets.Model;
using System.Drawing;
using System.Windows.Forms;
using Planets.View.Imaging;
using Planets.Controller.Subcontrollers;

namespace Planets.View
{
    public partial class GameView : UserControl
    {

        Playfield field;

        private SpritePool sp = new SpritePool();

        // Wordt gebruikt voor bewegende achtergrond
        private int angle = 0;

        /// <summary>
        /// Buffer bitmap
        /// </summary>

        // Aiming Settings
        /// <summary>
        /// If true, a vector will be drawn to show the current trajectory
        /// </summary>
        public bool IsAiming;
        public Vector AimPoint;
        public Vector MousePoint;

        // Aiming pen buffer
        private Pen CurVecPen = new Pen(Color.Red, 2);
        private Pen NextVecPen = new Pen(Color.Green, 2);
        private Pen AimVecPen = new Pen(Color.Black, 2);

        public GameView(Playfield field)
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.field = field;
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
            this.CurVecPen.CustomEndCap = bigArrow;
            this.NextVecPen.CustomEndCap = bigArrow;
            this.AimVecPen.DashPattern = new float[] { 10 };
            this.AimVecPen.DashStyle = DashStyle.Dash;
            this.AimVecPen.CustomEndCap = bigArrow;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // Teken achtergrond
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Background, ClientSize.Width, ClientSize.Height, 0), 0, 0);

            // Maak teken functie
            lock (field.BOT)
            {
                field.BOT.Iterate(obj =>
                {
                    float radius = (float)obj.Radius;
                    int length = (int)(radius * 2);
                    int h = obj.GetHashCode();
                    int x = (int)(obj.Location.X - radius);
                    int y = (int)(obj.Location.Y - radius);

                    // Calculate player angle
                    /*if (obj.DV.Length() > 1.0)
                    {
                        int angleO = 0;
                        angleO = (int)(Math.Atan2(obj.DV.X, obj.DV.Y) / Math.PI * 180.0);
                        // Retrieve sprites
                        Sprite cometSprite = sp.GetSprite(Sprite.CometTail, length * 4, length * 4, angleO + 180);
                        g.DrawImageUnscaled(cometSprite, (int)(obj.Location.X - cometSprite.Width / 2), (int)(obj.Location.Y - cometSprite.Height / 2));

                    }*/



                    if (obj == field.CurrentPlayer)
                    {
                        if (IsAiming)
                        {
                            Vector CursorPosition = Cursor.Position;
                            AimPoint = obj.Location - CursorPosition;

                            Vector CurVec = obj.Location + obj.DV.ScaleToLength(obj.DV.Length());
                            // Draw current direction vector
                            g.DrawLine(CurVecPen, obj.Location + obj.DV.ScaleToLength(obj.Radius + 1), CurVec);

                            // Draw aim direction vector
                            g.DrawLine(AimVecPen, obj.Location + AimPoint.ScaleToLength(obj.Radius + 1),
                                obj.Location + AimPoint.ScaleToLength(obj.DV.Length()));

                            // Draw next direction vector
                            Vector NextVec = ShootProjectileController.CalcNewDV(obj,
                                new GameObject(new Vector(0, 0), new Vector(0, 0), 0.05 * obj.mass), Cursor.Position);
                            g.DrawLine(NextVecPen, obj.Location + NextVec.ScaleToLength(obj.Radius + 1),
                                obj.Location + NextVec.ScaleToLength(obj.DV.Length()));
                        }

                        // Draw player
                        Sprite s = sp.GetSprite(Sprite.Player, length, length);
                        g.DrawImageUnscaled(s, (int)(obj.Location.X - s.Width / 2), (int)(obj.Location.Y - s.Height / 2));
                    }
                    else if (obj is BlackHole)
                    {
                        angle -= 1;
                        Sprite s = sp.GetSprite(Sprite.BlackHole, length, length, angle);
                        g.DrawImageUnscaled(s, (int)(obj.Location.X - s.Width / 2), (int)(obj.Location.Y - s.Height / 2));
                    }
                    else
                    {
                        Sprite s = sp.GetSprite(Sprite.Player, length, length);
                        g.DrawImageUnscaled(s, (int)(obj.Location.X - s.Width / 2), (int)(obj.Location.Y - s.Height / 2));
                    }


                    // Drawing the autodemo
                    double f = (DateTime.Now - field.LastAutoClickMoment).TotalMilliseconds;
                    if (f < 1000)
                    {
                        int r = 30 + (int)(f / 10);
                        g.FillEllipse(new SolidBrush(Color.FromArgb((int)(255 - f / 1000 * 255), 255, 0, 0)),
                            field.LastAutoClickLocation.X - r / 2, field.LastAutoClickLocation.Y - r / 2, r,
                            r);
                        g.DrawImageUnscaled(sp.GetSprite(Sprite.Cursor, 100, 100, 0), field.LastAutoClickLocation.X - 4,
                            field.LastAutoClickLocation.Y - 10);
                    }
                });

                if (Debug.Enabled)
                {
                    using (Pen p = new Pen(Color.OrangeRed, 2.0f))
                    {
                        field.BOT.DoCollisions((go1, go2, ms) => g.DrawLine(p, go1.Location, go2.Location), 0);
                    }

                    int d = field.BOT.Count;
                    int d2 = (d - 1) * d / 2;

                    using (Brush b = new SolidBrush(Color.Magenta))
                    {
                        Font f = new Font(FontFamily.GenericSansSerif, 16.0f, FontStyle.Bold);
                        g.DrawString("Regular Collision Detection: " + d2, f, b, 100, 300);
                        g.DrawString("Binary Tree Collision Detection: " + (field.BOT.colCount), f, b, 100, 320);
                        g.DrawString("Collision Detection Pimp rovement: " + (d2 - field.BOT.colCount) * 100 / d2 + "%", f, b, 100, 340);
                    }
                }
            }
        }
    }
}
