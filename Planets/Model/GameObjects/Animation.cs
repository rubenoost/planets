using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model.GameObjects
{
    class Animation : GameObject
    {
        private int duration;
        private int starttijd;

                public Animation(Vector location, Vector velocity, double mass, int d, int s)
            : base(location, velocity, mass, Rule.NONE)

                {
                    d = duration;
                    s = starttijd;
                }
    }
}
