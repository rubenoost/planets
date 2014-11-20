using System.Collections.Generic;

namespace Planets.Model
{
    public class Playfield
    {
        internal Player CurrentPlayer { get; set; }

        public List<GameObject> GameObjects;

        public Playfield()
        {
            this.GameObjects = new List<GameObject>();
        }

    }
}
