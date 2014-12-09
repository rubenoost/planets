using Planets.Model;

namespace Planets.Controller.GameRules
{
    class StayInFieldRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            pf.BOT.Iterate(obj =>
            {
                if (obj.Location.X < obj.Radius)
                {
                    obj.Location = new Vector(obj.Radius, obj.Location.Y);
                    obj.InvertObjectX();
                }
                if (obj.Location.X > pf.Size.Width - obj.Radius)
                {
                    obj.Location = new Vector(pf.Size.Width - obj.Radius, obj.Location.Y);
                    obj.InvertObjectX();
                }
                if (obj.Location.Y < obj.Radius)
                {
                    obj.Location = new Vector(obj.Location.X, obj.Radius);
                    obj.InvertObjectY();
                }
                if (obj.Location.Y > pf.Size.Height - obj.Radius)
                {
                    obj.Location = new Vector(obj.Location.X, pf.Size.Height - obj.Radius);
                    obj.InvertObjectY();
                }
            });
        }
    }
}
