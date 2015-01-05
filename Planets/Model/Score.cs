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

        private int _alpha = 255;

        public Score(int value, DateTime stamp, Vector location, bool currentPlayer)
        {
            this.Value = value;
            this.Stamp = stamp;
            this.Location = location;

            Color = currentPlayer ? Color.White : Color.Red;
        }

        public void UpdateLocation()
        {
            Location -= new Vector(0, 3);
            Color = Color.FromArgb(_alpha, Color);
            if (_alpha > 50)
                _alpha -= 25;
            else
                if (_alpha > 0)
                    _alpha -= 5;
        }

    }
}
