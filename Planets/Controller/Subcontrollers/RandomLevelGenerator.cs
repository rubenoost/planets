using System;
using System.Drawing;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.Subcontrollers
{
    static class RandomLevelGenerator
    {
        private static Playfield pf;

        public static Playfield GenerateRandomLevel()
        {
            pf = new Playfield(1920, 1080);

            Random rnd = new Random();
            int AmntObstacles = rnd.Next(7, 15);
            int normalCount = rnd.Next(10, 15);

            int[] RndObstacles = new int[AmntObstacles];

            bool tardisAvbl = false;
            bool AntagonistAvbl = false;
            bool StasisFieldAvbl = false;

            for (int i = 0; i < AmntObstacles; i++)
            {
                int NextObj = rnd.Next(0, 7);

                while (NextObj == 5 && tardisAvbl)
                {
                    NextObj = rnd.Next(0, 7);
                }

                if (NextObj == 5 && !tardisAvbl)
                {
                    tardisAvbl = true;
                }

                while (NextObj == 4 && StasisFieldAvbl)
                {
                    NextObj = rnd.Next(0, 7);
                }

                if (NextObj == 4 && !StasisFieldAvbl)
                {
                    StasisFieldAvbl = true;
                }

                RndObstacles[i] = NextObj;


                while (NextObj == 6 && AntagonistAvbl)
                {
                    NextObj = rnd.Next(0, 6);
                }

                if (NextObj == 6 && !AntagonistAvbl)
                {
                    AntagonistAvbl = true;
                }

                RndObstacles[i] = NextObj;

            }

            Point[] UsedPoints = new Point[AmntObstacles];
            Point NextPoint = new Point(0, 0);

            foreach (int obj in RndObstacles)
            {
                bool NotOK = false;
                while (!NotOK)
                {
                    bool PointFound = false;
                    bool PointOK = false;
                    int UsedPointCount = 0;

                    while (!PointOK)
                    {
                        Point RndPoint = new Point(rnd.Next(0, 1920), rnd.Next(0, 1080));

                        foreach (Point point in UsedPoints)
                            if (point == RndPoint)
                                PointFound = true;

                        if (!PointFound)
                        {
                            NextPoint = RndPoint;
                            PointOK = true;
                        }
                    }

                    GameObject NewObj = new GameObject(new Vector(0, 0), new Vector(0, 0), 0);

                    switch (obj)
                    {
                        case 0: // AntiGravity
                            NewObj = (new Antigravity(NextPoint, new Vector(0, 0), 10000));
                            break;
                        case 1: // AntiMatter
                            NewObj = (new AntiMatter(NextPoint, new Vector(0, 0), 100));
                            break;
                        case 2: // BlackHole
                            NewObj = (new BlackHole(NextPoint, new Vector(0, 0), 100));
                            break;
                        case 3: // Mine
                            NewObj = (new Mine(NextPoint, new Vector(0, 0), 50));
                            break;
                        case 4: // Stasis
                            NewObj = (new Stasis(NextPoint, new Vector(0, 0), 800));
                            break;
                        case 5: // Tardis
                            NewObj = (new Bonus(NextPoint, new Vector(0, 0), 0));
                            break;
                        //case 6: //Antagonist
                        //    NewObj = (new Antagonist(NextPoint, new Vector(0, 0), 10000));
                        //    break;
                    }

                    bool FoundIntersect = false;
                    pf.BOT.Iterate(g =>
                        {
                            if (g.IntersectsWith(NewObj))
                                FoundIntersect = true;
                        });

                    if (!FoundIntersect)
                    {
                        UsedPoints[UsedPointCount] = NextPoint;
                        pf.BOT.Add(NewObj);
                        NotOK = true;
                    }
                }
            }

            pf.CurrentPlayer = new Player(new Vector(0, 0), new Vector(0, 0), Utils.StartMass);
            pf.CurrentAntagonist = new Antagonist(new Vector(600, 500), new Vector(0,0), Utils.StartMass);

            for (int i = 0; i < normalCount; i++)
            {
                var normalObject = new GameObject(new Vector(rnd.Next(100, 1800), rnd.Next(100, 900)), new Vector(rnd.Next(0, 10), rnd.Next(0, 10)), rnd.Next(1000, 5000));
                pf.BOT.Add(normalObject);
            }

            //pf.BOT.Add(new Mine(new Vector(50, 50), new Vector(0, 0), Utils.StartMass / 2));
            return pf;
        }


    }
}
