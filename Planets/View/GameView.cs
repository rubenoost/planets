using Planets.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Planets.View
{
    public partial class GameView : UserControl
    {

        Playfield field;

        public GameView(Playfield field)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.field = field;
            this.BackgroundImage = Properties.Resources.LogoFinal_Inv;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            // Maak teken functie
            foreach(GameObject obj in field.GameObjects)
            {
                g.FillEllipse(new SolidBrush(Color.Blue), (float)obj.Location.X, (float)obj.Location.Y, Utils.CalcRadius(obj.Mass), Utils.CalcRadius(obj.Mass));
            }
        }

    }
}
