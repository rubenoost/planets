﻿using System;
using System.Drawing.Drawing2D;
using Planets.Model;
using System.Drawing;
using System.Windows.Forms;
using Planets.View.Imaging;

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
        private Bitmap cursor = new Bitmap(Properties.Resources.Cursors_Red);

        // Aiming Settings
        /// <summary>
        /// If true, a vector will be drawn to show the current trajectory
        /// </summary>
        public bool IsAiming;

        // Aiming pen buffer
        private Pen CurVecPen = new Pen(Color.Red, 2);
        private Pen NewVecPen = new Pen(Color.Green, 2);
        private Pen AimVecPen = new Pen(Color.White, 2);

        public GameView(Playfield field)
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.field = field;
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
            this.CurVecPen.CustomEndCap = bigArrow;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Teken achtergrond
            g.DrawImageUnscaled(sp.GetSprite(Sprite.Background, ClientSize.Width, ClientSize.Height, 0), 0, 0);

            // Maak teken functie
            lock (field.GameObjects)
            {
                foreach (GameObject obj in field.GameObjects)
                {
                    float radius = (float)obj.Radius;
                    int length = (int)(radius * 2);
                    int h = obj.GetHashCode();
                    int x = (int)(obj.Location.X - radius);
                    int y = (int)(obj.Location.Y - radius);

                    if (obj == field.CurrentPlayer)
                    {
                        if (IsAiming)
                        {
                            Vector NewPoint = obj.CalcNewLocation(17);
                            Vector CurVec = obj.Location + obj.DV.ScaleToLength(100.0);
                            g.DrawLine(CurVecPen, obj.Location + obj.DV.ScaleToLength(obj.Radius + 1), CurVec);
                        }
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
                }

                // Drawing the autodemo
                double f = (DateTime.Now - field.LastAutoClickMoment).TotalMilliseconds;
                if (f < 1000)
                {
                    int radius = 30 + (int)(f / 10);
                    g.FillEllipse(new SolidBrush(Color.FromArgb((int)(255 - f / 1000 * 255), 255, 0, 0)), field.LastAutoClickLocation.X - radius / 2, field.LastAutoClickLocation.Y - radius / 2, radius, radius);
                    g.DrawImage(cursor, field.LastAutoClickLocation.X - 4, field.LastAutoClickLocation.Y - 10);
                }
            }
        }

    }
}
