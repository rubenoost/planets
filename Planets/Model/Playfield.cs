using System;
using System.Drawing;

namespace Planets.Model
{
    public class Playfield
    {
        internal Player _currentPlayer;

        internal Size Size { get; set; }

        internal Point LastAutoClickGameLocation { get; set; }

        internal DateTime LastAutoClickMoment { get; set; }

        public Playfield(int width, int height)
        {
            // Save variables
            Size = new Size(width, height);
            this.sb = new ScoreBoard();

            // Create GameObject list
            BOT = new BinaryObjectTree(null, new Rectangle(0, 0, 1920, 1080), 1, 12, 0);
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

        public ScoreBoard sb;

        public BinaryObjectTree BOT;
    }
}
