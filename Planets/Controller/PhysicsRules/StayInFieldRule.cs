using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class StayInFieldRule : AbstractGameRule
    {
        private int _width;
        private int _height;

        public StayInFieldRule(int width, int height)
        {
            _width = width;
            _height = height;
        }

        internal override void Execute(Playfield pf, double ms)
        {
            foreach (GameObject obj in pf.GameObjects)
            {
                Vector newLoc = obj.CalcNewLocation(ms);
                if (!FieldXCollission(newLoc, obj.radius))
                    obj.InvertObjectX();
                if (!FieldYCollission(newLoc, obj.radius))
                    obj.InvertObjectY();
            }
        }

        private bool FieldXCollission(Vector location, double radius)
        {
            return (location.X > radius && location.X + radius < _width);
        }

        private bool FieldYCollission(Vector location, double radius)
        {
            return (location.Y > radius && location.Y + radius < _height);
        }
    }
}
