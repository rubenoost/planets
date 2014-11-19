using Planets.Model;
using System.Collections.Generic;
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
        }

        protected override void OnPaint(PaintEventArgs e)
        {



        }

    }
}
