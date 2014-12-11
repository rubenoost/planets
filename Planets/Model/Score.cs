using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model
{
    public class Score
    {

        public int Value { get; private set; }
        public DateTime Stamp { get; private set; }

        public Vector Location { get; private set; }

        public Color Color { get; private set; }

        private int Alpha = 255;

        public Score(int Value, DateTime Stamp, Vector Location, bool CurrentPlayer)
        {
            this.Value = Value;
            this.Stamp = Stamp;
            this.Location = Location;

            if (CurrentPlayer)
                Color = Color.White;
            else
                Color = Color.Red;
        }

        public void UpdateLocation()
        {
            this.Location -= new Vector(0, 3);
            Color = Color.FromArgb(this.Alpha, Color);
            if (this.Alpha > 50)
                this.Alpha -= 25;
            else
                if (this.Alpha > 0)
                    this.Alpha -= 5;
        }

    }
}
