using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class StayInFieldRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            foreach (GameObject obj in pf.GameObjects)
            {
                Vector newLoc = obj.CalcNewLocation(ms);
                if (!FieldXCollission(newLoc, obj.Radius, pf))
                    obj.InvertObjectX();
                if (!FieldYCollission(newLoc, obj.Radius, pf))
                    obj.InvertObjectY();
            }
        }

        private bool FieldXCollission(Vector location, double radius, Playfield pf)
        {
            return (location.X > radius && location.X + radius < pf.Size.Width);
        }

        private bool FieldYCollission(Vector location, double radius, Playfield pf)
        {
            return (location.Y > radius && location.Y + radius < pf.Size.Height);
        }
    }
}
