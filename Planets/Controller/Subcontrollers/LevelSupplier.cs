using Planets.Model;

namespace Planets.Controller.Subcontrollers
{
    public class LevelSupplier
    {
        // At this moment only Random is used, but when this is further developed by another team a campaign could be added with XML.
        public enum Mode
        {
            Campaign,
            Random
        }
        
        // Path
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

        // Generate random level
        public Playfield GenerateLevel()
        {
            LevelNumber++;
            return LevelMode == Mode.Random ? RandomLevelGenerator.GenerateRandomLevel() : LoadLevel(LevelNumber);
        }

        // Load a campaign level
        private Playfield LoadLevel(int i)
        {
            var pf = new Playfield();
            pf.DeserializeFromFile(LevelFolder + i + ".lvl");
            return pf;
        }

    }
}
