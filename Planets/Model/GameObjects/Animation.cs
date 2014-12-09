using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model.GameObjects
{
    class Animation : GameObject
    {
                public Animation(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.NONE)

        {

        }
    }
}
