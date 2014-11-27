using System;
using System.Drawing.Drawing2D;
using Planets.Model;
using System.Drawing;
using System.Windows.Forms;

namespace Planets.View
{
    public partial class GameView : UserControl
    {

        Playfield field;

        // Wordt gebruikt voor bewegende achtergrond
        private int angle = 0;

        private Color[] Colors =
        {
            Color.FromArgb(127, 255, 0, 0),
            Color.FromArgb(127, 255, 255, 0),
            Color.FromArgb(127, 0, 255, 0),
            Color.FromArgb(127, 0, 255, 255),
            Color.FromArgb(127, 0, 0, 255),
            Color.FromArgb(127, 255, 0, 255)
        };

        /// <summary>
        /// Buffer bitmap
        /// </summary>
        private Bitmap b = new Bitmap(Properties.Resources.LogoFinal_Inv, new Size(1920, 1080));
        private Bitmap h1 = new Bitmap(Properties.Resources.Hole1, new Size(100, 100));
        private Bitmap cursor = new Bitmap(Properties.Resources.Cursors_Red);
        private Brush b2 = new TextureBrush(Properties.Resources.LogoFinal);
        private Brush b3 = new SolidBrush(Color.Magenta);

        /// <summary>
        /// Main user character image
        /// </summary>
        private Image newImage = new Bitmap(Planets.Properties.Resources.Pluto);

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
        }

        /// <summary>
        /// Functie die een BMP input kan roteren
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Bitmap RotateImg(Bitmap bmp, float angle)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            Bitmap tempImg = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(tempImg);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);
            Bitmap newImg = new Bitmap(Convert.ToInt32(rct.Width), Convert.ToInt32(rct.Height));
            g = Graphics.FromImage(newImg);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tempImg, 0, 0);
            g.Dispose();
            tempImg.Dispose();
            return newImg;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw background unscaled to improve performance.
            g.DrawImageUnscaled(b, new Point(0, 0));
            
            // Bewegende achtergrond testcode
            //g.DrawImageUnscaled(b, new Point(angle/4, angle/4));
            //angle -= 1;

            // Maak teken functie
            lock (field.GameObjects)
            {
                foreach (GameObject obj in field.GameObjects)
                {
                    float radius = (float)obj.Radius;
                    float length = radius * 2;
                    int h = obj.GetHashCode();


                    if (obj == field.CurrentPlayer)
                    {
                        g.DrawImage(newImage, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length,length);

                        if(IsAiming)
                        {
                            // if deltaV x = - dan gaat ie naar links bij + naar rechts
                            // if deltaV y = - dan gaat ie naar boven bij + naar beneden
                            Vector NewPoint = obj.CalcNewLocation(17);

                            Vector CurVec = obj.Location + obj.DV.ScaleToLength(100.0);
                            g.DrawLine(CurVecPen, obj.Location + obj.DV.ScaleToLength(obj.Radius + 1), CurVec);
                        }
                    }
                    else if (obj is BlackHole)
                    {
                        angle -= 1;
                        Bitmap test = RotateImg(h1, angle);
                        g.DrawImage(test, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length, length);

                        //g.FillEllipse(b3, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length,length);
                    }
                    else
                    {
                        //Brush brush = new SolidBrush(Colors[h%Colors.Length]);
                        g.FillEllipse(b2, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length, length);
                    }
                }

                double f = (DateTime.Now - field.LastAutoClickMoment).TotalMilliseconds;
                if (f < 1000)
                {
                    int radius = 30 + (int)(f/10);
                    g.FillEllipse(new SolidBrush(Color.FromArgb((int) (255 - f / 1000 * 255), 255, 0, 0)), field.LastAutoClickLocation.X - radius / 2, field.LastAutoClickLocation.Y - radius / 2, radius, radius);
                    g.DrawImage(cursor, field.LastAutoClickLocation.X - 4, field.LastAutoClickLocation.Y - 10);
                }
            }
        }

    }
}
