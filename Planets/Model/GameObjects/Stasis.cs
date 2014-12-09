namespace Planets.Model {
    public class Stasis : GameObject {
        public Stasis(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.NONE) {
            // Radius of the stasis is always 50
            Radius = 200;
        }
    }
}
