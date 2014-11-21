using System.Collections.Generic;

namespace Planets.Model
{
    public class Playfield
    {
        internal Player _currentPlayer;

        internal double Height { get; private set; }

        internal double Width { get; private set; }

        public Playfield(double width, double height)
        {
            // Save variables
            Width = width;
            Height = height;

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
