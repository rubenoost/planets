﻿using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    public class ElasticCollisionRule : AbstractCollisionRule
    {
        protected override void DoCollision(Playfield pf, GameObject c1, GameObject c2, double ms)
        {
            if (c1.IntersectsWith(c2))
            {
                if (c1.Traits.HasFlag(Rule.MOVE) && c2.Traits.HasFlag(Rule.MOVE))
                {
                    var totalMass = c1.mass + c2.mass;

                    // Move back
                    Vector v1 = (c2.Location - c1.Location);
                    if (v1.Length() < 0.1) return;
                    double l1 = v1.Length();
                    double l2 = c2.Radius + c1.Radius;
                    double diff = l2 - l1;

                    c2.Location = c2.Location + v1.ScaleToLength(c1.mass / totalMass * diff);
                    c1.Location = c1.Location - v1.ScaleToLength(c2.mass / totalMass * diff);

                    // Calculate new speeds
                    var h1 = c1.Location - c2.Location;
                    var h2 = c2.Location - c1.Location;

                    var t2 = h1.Length() * h1.Length();
                    var t3 = (c1.DV - c2.DV).InnerProduct(h1);

                    c1.DV = c1.DV - (2.0 * c2.mass / totalMass) * t3 / t2 * h1;
                    c2.DV = c2.DV - (2.0 * c1.mass / totalMass) * t3 / t2 * h2;
                }
                else if (c1.Traits.HasFlag(Rule.MOVE))
                {
                    // Move back
                    c1.Location = c2.Location + (c1.Location - c2.Location).ScaleToLength(c1.Radius + c2.Radius);

                    // Calculate new speeds
                    var t = c1.mass + c2.mass;

                    var h1 = c1.Location - c2.Location;

                    var t2 = h1.Length() * h1.Length();
                    var t3 = (c1.DV - c2.DV).InnerProduct(h1);

                    c1.DV = c1.DV - (2.0 * c2.mass / t) * t3 / t2 * h1;
                }
                else if (c2.Traits.HasFlag(Rule.MOVE))
                {
                    // Move back
                    c2.Location = c1.Location + (c2.Location - c1.Location).ScaleToLength(c2.Radius + c1.Radius);

                    // Calculate new speeds
                    var t = c1.mass + c2.mass;

                    var h2 = c2.Location - c1.Location;

                    var t2 = h2.Length() * h2.Length();
                    var t3 = (c2.DV - c1.DV).InnerProduct(h2);

                    c2.DV = c2.DV - (2.0 * c1.mass / t) * t3 / t2 * h2;
                }
            }
        }
    }
}
