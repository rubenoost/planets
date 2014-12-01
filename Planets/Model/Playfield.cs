using System;
using System.Drawing;

namespace Planets.Model
{
    public class Playfield
    {
        internal Player _currentPlayer;

        internal Size Size { get; set; }

        internal Point LastAutoClickLocation { get; set; }

        internal DateTime LastAutoClickMoment { get; set; }

        public Playfield(int width, int height)
        {
            // Save variables
            Size = new Size(width, height);

            // Create GameObject list
            BOT = new BinaryObjectTree(null, new Rectangle(0, 0, 1920, 1080), 0, 7, 0);
        }

        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                BOT.Remove(_currentPlayer);
                _currentPlayer = value;
                BOT.Add(_currentPlayer);
            }
        }

        public BinaryObjectTree BOT;
    }
}
