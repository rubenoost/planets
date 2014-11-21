namespace Planets.Model
{
	class BlackHole : GameObject
	{
		private double Strength;

		public BlackHole(double x, double y, double dx, double dy, double mass, double strength) : base(x, y, dx, dy, mass, false)
		{
			Strength = strength;
		}

		public BlackHole(Vector location, Vector velocity,  double mass, double strength) : base(location, velocity, mass, false)
		{
			Strength = strength;
		}

		public double Gravity() {
			double GravGrootte = Strength + mass;
			return GravGrootte;
		}
	}
}
