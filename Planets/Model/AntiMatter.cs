using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model
{
    class AntiMatter : GameObject
    {

        public AntiMatter(Vector location, Vector velocity, double mass) : base(location, velocity, mass, Rule.COLLIDES | Rule.EATABLE | Rule.EATS | Rule.MOVE)
        {
            Radius = 50;
        }

    }
}
