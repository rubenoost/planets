using System.Collections.Generic;

namespace Planets.Model
{
    public class Playfield
    {
        public IEnumerable<GameObject> GameObjects
        {
            get { yield return new Player(10, 10, 1, 1, 300); }
        }
    }
}
