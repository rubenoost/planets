using System;
using System.Drawing;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.Subcontrollers
{
    static class RandomLevelGenerator
    {
        // Playfield to fill and return.
        private static Playfield _pf;

        public static Playfield GenerateRandomLevel()
        {
            _pf = new Playfield(1920, 1080);

            // Spawn between 7 and 14 obstacles
            Random rnd = new Random();
            int amntObstacles = rnd.Next(7, 15);
            // Spawn between 10 and 14 GameObjects that can be eaten to gain mass.
            int normalCount = rnd.Next(10, 15);

            int[] rndObstacles = new int[amntObstacles];

            bool tardisAvbl = false;
            bool antagonistAvbl = false;
            bool stasisFieldAvbl = false;

            // Check if some obstacles that can only occur once are already in the game
            for (int i = 0; i < amntObstacles; i++)
            {
                int nextObj = rnd.Next(0, 7);

                while (nextObj == 5 && tardisAvbl)
                    nextObj = rnd.Next(0, 7);

                if (nextObj == 5 && !tardisAvbl)
                    tardisAvbl = true;

                while (nextObj == 4 && stasisFieldAvbl)
                    nextObj = rnd.Next(0, 7);

                if (nextObj == 4 && !stasisFieldAvbl)
                    stasisFieldAvbl = true;

                rndObstacles[i] = nextObj;


                while (nextObj == 6 && antagonistAvbl)
                    nextObj = rnd.Next(0, 6);

                if (nextObj == 6 && !antagonistAvbl)
                    antagonistAvbl = true;

                rndObstacles[i] = nextObj;

            }

            // Check for usedpoints, gameobjects are not allowed to overlap.
            Point[] usedPoints = new Point[amntObstacles];
            Point nextPoint = new Point(0, 0);

            foreach (int obj in rndObstacles)
            {
                bool notOk = false;
                while (!notOk)
                {
                    bool pointFound = false;
                    bool pointOk = false;
                    int usedPointCount = 0;

                    while (!pointOk)
                    {
                        Point rndPoint = new Point(rnd.Next(0, 1920), rnd.Next(0, 1080));

                        foreach (Point point in usedPoints)
                            if (point == rndPoint)
                                pointFound = true;

                        if (!pointFound)
                        {
                            nextPoint = rndPoint;
                            pointOk = true;
                        }
                    }

                    GameObject newObj = new GameObject(new Vector(0, 0), new Vector(0, 0), 0);

                    switch (obj)
                    {
                        case 0: // AntiGravity
                            newObj = (new Antigravity(nextPoint, new Vector(0, 0), 10000));
                            break;
                        case 1: // AntiMatter
                            newObj = (new AntiMatter(nextPoint, new Vector(0, 0), 100));
                            break;
                        case 2: // BlackHole
                            newObj = (new BlackHole(nextPoint, new Vector(0, 0), 100));
                            break;
                        case 3: // Mine
                            newObj = (new Mine(nextPoint, new Vector(0, 0), 50));
                            break;
                        case 4: // Stasis
                            newObj = (new Stasis(nextPoint, new Vector(0, 0), 800));
                            break;
                        case 5: // Tardis
                            newObj = (new Bonus(nextPoint, new Vector(0, 0), 0));
                            break;
                    }

                    bool foundIntersect = false;
                    _pf.GameObjects.Iterate(g =>
                        {
                            if (g.IntersectsWith(newObj))
                                foundIntersect = true;
                        });

                    if (!foundIntersect)
                    {
                        usedPoints[usedPointCount] = nextPoint;
                        _pf.GameObjects.Add(newObj);
                        notOk = true;
                    }
                }
            }

            _pf.CurrentPlayer = new Player(new Vector(0, 0), new Vector(0, 0), Utils.StartMass);
            _pf.CurrentAntagonist = new Antagonist(new Vector(600, 500), new Vector(0,0), Utils.StartMass);

            for (int i = 0; i < normalCount; i++)
            {
                var normalObject = new GameObject(new Vector(rnd.Next(100, 1800), rnd.Next(100, 900)), new Vector(rnd.Next(0, 10), rnd.Next(0, 10)), rnd.Next(1000, 5000));
                _pf.GameObjects.Add(normalObject);
            }

            return _pf;
        }


    }
}
