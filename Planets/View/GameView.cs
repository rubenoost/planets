using Planets.Model;
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
        private Bitmap b = new Bitmap(Properties.Resources.Background, new Size(1920, 1080));

        /// <summary>
        /// Main user character image
        /// </summary>
        private Image newImage = Planets.Properties.Resources.Pluto;


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
            foreach (GameObject obj in field.GameObjects)
            {
                float radius = (float)obj.radius;
                float length = radius * 2;
                int h = obj.GetHashCode();
                if (obj != field.CurrentPlayer)
                {
                    Brush brush = new SolidBrush(Colors[h % Colors.Length]);
                    g.FillEllipse(brush, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length,
                        length);
                }
                else
                {
                    g.DrawImage(newImage, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length, length);
                }
            }
        }

    }
}
