using System.Drawing.Drawing2D;
using Planets.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Planets.View
{
    public partial class GameView : UserControl
    {

        Playfield field;

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
        

        public GameView(Playfield field)
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.field = field;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Draw background unscaled to improve performance.
            g.DrawImageUnscaled(b, new Point(0, 0));

            // Maak teken functie
            foreach(GameObject obj in field.GameObjects)
            {
                float radius = (float) obj.radius;
                float length = radius*2;
                int h = obj.GetHashCode();
                if (obj != field.CurrentPlayer)
                {
                    Brush brush = new SolidBrush(Colors[h%Colors.Length]);
                    g.FillEllipse(brush, (float) obj.Location.X - radius, (float) obj.Location.Y - radius, length,
                        length);
                }
                else
                {
                    Brush brush2 = new LinearGradientBrush(new Point(0, 0), new Point(1920, 1080), Color.GreenYellow, Color.Red);
                    g.FillEllipse(brush2, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length, length);
                    Brush brush = new LinearGradientBrush(new Point(0, 1080),new Point(1920, 0),Color.Magenta,Color.Blue);
                    g.FillEllipse(brush, (float)obj.Location.X - radius / 2, (float)obj.Location.Y - radius / 2, length/2, length/2);
                    
                }
            }
        }

    }
}
