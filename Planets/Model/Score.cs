using System;
using System.Collections.Generic;
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

        public bool CurrentPlayer { get; private set; }

        public Score(int Value, DateTime Stamp, Vector Location, bool CurrentPlayer)
        {
            this.Value = Value;
            this.Stamp = Stamp;
            this.Location = Location;
            this.CurrentPlayer = CurrentPlayer;
        }

    }
}
