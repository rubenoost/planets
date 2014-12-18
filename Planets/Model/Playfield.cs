using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using Planets.Model.GameObjects;

namespace Planets.Model
{
    public class Playfield
    {
        [XmlIgnore]
        internal Player _currentPlayer;

        public Size Size { get; set; }

        [XmlIgnore]
        internal Point LastAutoClickGameLocation;

        [XmlIgnore]
        internal DateTime LastAutoClickMoment;

        public Playfield()
            : this(1920, 1080)
        { }

        public Playfield(int width, int height)
        {
            // Save variables
            Size = new Size(width, height);
            sb = new ScoreBoard();

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

        [XmlIgnore]
        public ScoreBoard sb;

        [XmlIgnore]
        public BinaryObjectTree BOT;

        [XmlArray("GameObjects")]
        public GameObject[]
            DoNotUseThisVariableThisIsOnlyUsedForSerializationSoAbsolutelyDoNotTouchThisNoReallyDontJustDont
        {
            get
            {
                List<GameObject> result = new List<GameObject>();
                BOT.Iterate(g => { if (g != CurrentPlayer) result.Add(g); });
                return result.ToArray();
            }
            set
            {
                BOT.Clear();
                foreach (var go in value)
                    BOT.Add(go);
            }
        }
    }
}
