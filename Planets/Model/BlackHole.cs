using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model
{
	class BlackHole : GameObject
	{
		public BlackHole(double x, double y, double dx, double dy, double mass) : base(x, y, dx, dy, mass)
		{

		}

		public BlackHole(Vector location, Vector velocity, double mass) : base(location, velocity, mass)
		{

		}
	}
}
