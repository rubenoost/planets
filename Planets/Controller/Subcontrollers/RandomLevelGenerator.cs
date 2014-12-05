using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Model;

namespace Planets.Controller.Subcontrollers
{
    class RandomLevelGenerator
    {
        private Playfield pf;
        public RandomLevelGenerator(GameEngine ge)
        {
            ge.field = pf;
        }


    }
}
