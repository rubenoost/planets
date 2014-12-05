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
            pf = new Playfield(1920, 1080);

            bool TARDIS = false;
            Random rnd = new Random();
            int AmntObstacles = rnd.Next(4, 8);

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

                int UsedPointCount = 0;

                switch(obj)
                {
                    case 0: // AntiGravity
                        pf.BOT.Add(new Antigravity(NextPoint, new Vector(0,0), -1000000));
                        break;
                    case 1: // AntiMatter
                        pf.BOT.Add(new AntiMatter(NextPoint, new Vector(0, 0), 100));
                        break;
                    case 2: // BlackHole
                        pf.BOT.Add(new BlackHole(NextPoint, new Vector(0, 0), 1000000));
                        break;
                    case 3: // Mine
                        pf.BOT.Add(new Mine(NextPoint, new Vector(0, 0), 50));
                        break;
                    case 4: // Stasis
                        pf.BOT.Add(new Stasis(NextPoint, new Vector(0, 0), 800));
                        break;
                    case 5: // Tardis
                        pf.BOT.Add(new Tardis(NextPoint, new Vector(0, 0), 0));
                        break;
                }

                UsedPoints[UsedPointCount] = NextPoint;
            }

            pf.CurrentPlayer = new Player(new Vector(0, 0), new Vector(0, 0), Utils.StartMass);
            return pf;
        }


    }
}
