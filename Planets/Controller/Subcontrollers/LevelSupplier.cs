﻿using Planets.Model;

namespace Planets.Controller.Subcontrollers
{
    public class LevelSupplier
    {
        public enum Mode
        {
            Campaign,
            Random
        }

        private const string LevelFolder = "Data/Levels/";

        private Mode _propLevelMode = Mode.Random;
        public Mode LevelMode
        {
            get
            {
                return _propLevelMode;
            }
            set
            {
                if (value != _propLevelMode)
                    LevelNumber = 0;
                _propLevelMode = value;
            }
        }

        public int LevelNumber { get; private set; }

        public Playfield GenerateLevel()
        {
            LevelNumber++;
            return LevelMode == Mode.Random ? RandomLevelGenerator.GenerateRandomLevel() : LoadLevel(LevelNumber);
        }

        private Playfield LoadLevel(int i)
        {
            var pf = new Playfield();
            pf.DeserializeFromFile(LevelFolder + i + ".lvl");
            return pf;
        }

    }
}
