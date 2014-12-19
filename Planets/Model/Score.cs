using System;
using System.Drawing;

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

            Color = CurrentPlayer ? Color.White : Color.Red;
        }

        public void UpdateLocation()
        {
            Location -= new Vector(0, 3);
            Color = Color.FromArgb(Alpha, Color);
            if (Alpha > 50)
                Alpha -= 25;
            else
                if (Alpha > 0)
                    Alpha -= 5;
        }

    }
}
