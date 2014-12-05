using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model
{
    public class Antagonist : Player
    {
        public Antagonist(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass)
        {
            Traits = Traits & ~Rule.AFFECTED_BY_BH;
        }
    }
}
