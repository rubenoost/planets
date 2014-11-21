using System.Collections.Generic;
using System.Configuration;
using System.Drawing;

namespace Planets.Model
{
    public class Playfield
    {
        internal Player _currentPlayer;

        internal Size Size { get; set; }

        public Playfield(int width, int height)
        {
            // Save variables
            Size = new Size(width, height);

            // Create GameObject list
            this.GameObjects = new List<GameObject>();
        }

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
    }
}
