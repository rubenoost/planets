using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Model;
using System.Drawing;

namespace Planets.Controller.Subcontrollers
{
    static class RandomLevelGenerator
    {
        private static Playfield pf;
        public static Playfield GenerateRandomLevel()
        {
            /*pf = new Playfield(1920, 1080);

            bool TARDIS = false;
            Random rnd = new Random();
            int AmntObstacles = rnd.Next(4, 12);

            int[] RndObstacles = new int[AmntObstacles];
            int previous = -1;

            for(int i = 0; i < AmntObstacles; i++)
            {
                int NextObj = rnd.Next(0, 6);
                while (NextObj == previous)
                {
                    if(NextObj == 6)
                        TARDIS = true;

                    if(!TARDIS)
                        NextObj = rnd.Next(0, 6);
                }

                RndObstacles[i] = NextObj;

                previous = RndObstacles[i];
            }

            Point[] UsedPoints = new Point[AmntObstacles];
            Point NextPoint = new Point(0, 0);

            foreach(int obj in RndObstacles)
            {
                bool PointFound = false;
                bool PointOK = false;
                bool NotOK = false;
                int UsedPointCount = 0;

                while (!NotOK)
                {
                    while(!PointOK)
                    {
                        Point RndPoint = new Point(rnd.Next(0, 1920), rnd.Next(0, 1080));

                        foreach(Point point in UsedPoints)
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
                            NewObj = (new Antigravity(NextPoint, new Vector(0, 0), -1000000));
                            break;
                        case 1: // AntiMatter
                            NewObj = (new AntiMatter(NextPoint, new Vector(0, 0), 100));
                            break;
                        case 2: // BlackHole
                            NewObj = (new BlackHole(NextPoint, new Vector(0, 0), 1000000));
                            break;
                        case 3: // Mine
                            NewObj = (new Mine(NextPoint, new Vector(0, 0), 50));
                            break;
                        case 4: // Stasis
                            NewObj = (new Stasis(NextPoint, new Vector(0, 0), 800));
                            break;
                        case 5: // Tardis
                            NewObj = (new Tardis(NextPoint, new Vector(0, 0), 0));
                            break;
                    }

                    bool FoundIntersect = false;
                    pf.BOT.Iterate(g =>
                        {
                            if (g.IntersectsWith(NewObj))
                                FoundIntersect = true;
                        });

                    if (!FoundIntersect)
                    {
                        pf.BOT.Add(NewObj);
                        NotOK = true;
                    }
                }
                UsedPoints[UsedPointCount] = NextPoint;
            }

            pf.CurrentPlayer = new Player(new Vector(0, 0), new Vector(0, 0), Utils.StartMass);
            return pf;*/
            Playfield pf = new Playfield(1920, 1080);
            pf.CurrentPlayer = new Player(new Vector(200, 200), new Vector(0, 0), Utils.StartMass);
            pf.CurrentPlayer.Location = new Vector(pf.Size.Width / 2, pf.Size.Height / 2);

            //pf.BOT.Add(new GameObject(pf.CurrentPlayer.Location + new Vector(200, 0), new Vector(0, 0), Utils.StartMass / 2));
            //pf.BOT.Add(new GameObject(pf.CurrentPlayer.Location + new Vector(-200, 0), new Vector(0, 0), Utils.StartMass / 2));
            //pf.BOT.Add(new GameObject(pf.CurrentPlayer.Location + new Vector(0, 200), new Vector(0, 0), Utils.StartMass / 2));
            //pf.BOT.Add(new GameObject(pf.CurrentPlayer.Location + new Vector(0, -200), new Vector(0, 0), Utils.StartMass / 2));
            pf.BOT.Add(new Antagonist(pf.CurrentPlayer.Location + new Vector(200, 200), new Vector(0, 0), Utils.StartMass));
            // Anti Gravity
            pf.BOT.Add(new Mine(new Vector(500, 300), new Vector(0, 0), Utils.StartMass / 2));

            // Black holes
            pf.BOT.Add(new BlackHole(new Vector(50, 50), new Vector(0, 0), 1000000));
            pf.BOT.Add(new BlackHole(new Vector(50, 1030), new Vector(0, 0), 1000000));
            pf.BOT.Add(new BlackHole(new Vector(1870, 50), new Vector(0, 0), 1000000));
            pf.BOT.Add(new BlackHole(new Vector(1870, 1030), new Vector(0, 0), 1000000));

            pf.BOT.Add(new Stasis(new Vector(1200, 800), new Vector(0, 0), 800));
            pf.BOT.Add(new Tardis(new Vector(800, 200), new Vector(0, 0), 0));
            return pf;
        }


    }
}
