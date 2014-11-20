using System.Collections.Generic;

namespace Planets.Model
{
    public class Playfield
    {
        internal Player _currentPlayer;

        internal Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                GameObjects.Remove(_currentPlayer);
                _currentPlayer = value;
                GameObjects.Add(_currentPlayer);
            }
        }

        public List<GameObject> GameObjects;

        public Playfield()
        {
            this.GameObjects = new List<GameObject>();
        }

    }
}
