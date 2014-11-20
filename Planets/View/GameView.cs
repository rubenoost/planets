using Planets.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Planets.View
{
    public partial class GameView : UserControl
    {

        Playfield field;

        /// <summary>
        /// Buffer bitmap
        /// </summary>
        private Bitmap b = new Bitmap(Properties.Resources.LogoFinal_Inv, new Size(1920, 1080));

        /// <summary>
        /// Buffer brush
        /// </summary>
        private Brush brush = new SolidBrush(Color.Blue);

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
                float radius = (float) obj.radius * 2;
                g.FillEllipse(brush, (float)obj.Location.X - radius, (float)obj.Location.Y-radius, radius, radius);
            }
        }

    }
}
