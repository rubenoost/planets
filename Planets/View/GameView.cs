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
        private Bitmap b = new Bitmap(Properties.Resources.LogoFinal_Inv, new Size(1920, 1080));
        private Brush b2 = new TextureBrush(Properties.Resources.LogoFinal);
        private Brush b3 = new SolidBrush(Color.Magenta);

        /// <summary>
        /// Main user character image
        /// </summary>
        private Image newImage = new Bitmap(Planets.Properties.Resources.Pluto);


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
            lock (field.GameObjects)
            {
                foreach (GameObject obj in field.GameObjects)
                {
                    float radius = (float)obj.radius;
                    float length = radius * 2;
                    int h = obj.GetHashCode();


                    if (obj == field.CurrentPlayer)
                    {
                        g.DrawImage(newImage, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length,
                            length);
                    }
                    else if (obj is BlackHole)
                    {
                        g.FillEllipse(b3, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length,length);
                    }
                    else
                    {
                        //Brush brush = new SolidBrush(Colors[h%Colors.Length]);
                        g.FillEllipse(b2, (float)obj.Location.X - radius, (float)obj.Location.Y - radius, length,
                            length);
                    }
                }
            }
        }

    }
}
