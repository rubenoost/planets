using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model
{
	class Tardis : GameObject
	{
		public Tardis(Vector location, Vector velocity, double mass) : base(location, velocity, mass, Rule.COLLIDES)
		{
            Traits = Traits & ~Rule.COLLIDES;
            Traits = Traits & ~Rule.HAS_SCORE;
		}
	}
}
