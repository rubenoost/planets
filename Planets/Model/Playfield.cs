using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using Planets.Model.GameObjects;

namespace Planets.Model
{
    public class Playfield
    {
        [XmlIgnore] private Player _currentPlayer;

        [XmlIgnore] private Antagonist _currentAntagonist;

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
            ScoreBoard = new ScoreBoard();

            // Create GameObject list
            GameObjects = new BinaryObjectTree(new Rectangle(0, 0, 1920, 1080));
        }

        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                GameObjects.Remove(_currentPlayer);
                _currentPlayer = value;
                GameObjects.Add(_currentPlayer);
            }
        }

        public Antagonist CurrentAntagonist
        {
            get { return _currentAntagonist; }
            set
            {
                GameObjects.Remove(_currentAntagonist);
                _currentAntagonist = value;
                GameObjects.Add(_currentAntagonist);
            }
        }

        [XmlIgnore]
        public readonly ScoreBoard ScoreBoard;

        [XmlIgnore]
        public readonly BinaryObjectTree GameObjects;

        [XmlArray("GameObjects")]
        public GameObject[]
            DoNotUseThisVariableThisIsOnlyUsedForSerializationSoAbsolutelyDoNotTouchThisNoReallyDontJustDont
        {
            get
            {
                List<GameObject> result = new List<GameObject>();
                GameObjects.Iterate(g => { if (g != CurrentPlayer) result.Add(g); });
                return result.ToArray();
            }
            set
            {
                GameObjects.Clear();
                foreach (var go in value)
                    GameObjects.Add(go);
            }
        }
    }
}
