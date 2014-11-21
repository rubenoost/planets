using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class ElasticCollisionRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            for (int i = 0; i < pf.GameObjects.Count; i++)
            {
                for (int j = i + 1; j < pf.GameObjects.Count; j++)
                {
                    CheckObjectCollission(pf.GameObjects[i], pf.GameObjects[j]);
                }
            }
        }

        private void CheckObjectCollission(GameObject c1, GameObject c2)
        {
            if (c1.IntersectsWith(c2))
            {
                // Move back
                c2.Location = c1.Location + (c2.Location - c1.Location).ScaleToLength(c2.radius + c1.radius);
                
                // Calculate new speeds
                var t = c1.mass + c2.mass;

                var h1 = c1.Location - c2.Location;
                var h2 = c2.Location - c1.Location;

                var t2 = h1.Length()*h1.Length();
                var t3 = (c1.DV - c2.DV).InnerProduct(h1);

                var f1 = c1.DV - (2.0 * c2.mass / t) * t3 / t2 * h1;
                var f2 = c2.DV - (2.0 * c1.mass / t) * t3 / t2 * h2;

                // Apply speeds
                c1.DV = f1;
                c2.DV = f2;
            }
        }
    }
}
