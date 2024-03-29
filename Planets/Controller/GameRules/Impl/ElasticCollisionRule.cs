﻿using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    public class ElasticCollisionRule : AbstractCollisionRule
    {
        protected override void DoCollision(Playfield pf, ScoreBoard sb, GameObject c1, GameObject c2)
        {
            if (!c1.Is(Rule.Collides) || !c2.Is(Rule.Collides)) return;
            if (c1.Mass > c2.Mass && c2.Is(Rule.Eatable) && c1.Is(c2 is Player ? Rule.EatPlayer : Rule.Eats)) return;
            if (c2.Mass > c1.Mass && c1.Is(Rule.Eatable) && c2.Is(c1 is Player ? Rule.EatPlayer : Rule.Eats)) return;
            if (!c1.IntersectsWith(c2)) return;

            if (c1.Is(Rule.Move) && c2.Is(Rule.Move))
            {
                var totalMass = c1.Mass + c2.Mass;

                // Move back
                Vector v1 = (c2.Location - c1.Location);
                if (v1.Length() < 0.1) return;
                double l1 = v1.Length();
                double l2 = c2.Radius + c1.Radius;
                double diff = l2 - l1;

                c2.Location = c2.Location + v1.ScaleToLength(c1.Mass / totalMass * diff);
                c1.Location = c1.Location - v1.ScaleToLength(c2.Mass / totalMass * diff);

                // Calculate new speeds
                var h1 = c1.Location - c2.Location;
                var h2 = c2.Location - c1.Location;

                var t2 = h1.Length() * h1.Length();
                var t3 = (c1.Dv - c2.Dv).InnerProduct(h1);

                c1.Dv = c1.Dv - (2.0 * c2.Mass / totalMass) * t3 / t2 * h1;
                c2.Dv = c2.Dv - (2.0 * c1.Mass / totalMass) * t3 / t2 * h2;
            }
            else if (c1.Is(Rule.Move))
            {
                // Move back
                c1.Location = c2.Location + (c1.Location - c2.Location).ScaleToLength(c1.Radius + c2.Radius);

                // Calculate new speeds
                var t = c1.Mass + c2.Mass;

                var h1 = c1.Location - c2.Location;

                var t2 = h1.Length() * h1.Length();
                var t3 = (c1.Dv - c2.Dv).InnerProduct(h1);

                c1.Dv = c1.Dv - (2.0 * c2.Mass / t) * t3 / t2 * h1;
            }
            else if (c2.Is(Rule.Move))
            {
                // Move back
                c2.Location = c1.Location + (c2.Location - c1.Location).ScaleToLength(c2.Radius + c1.Radius);

                // Calculate new speeds
                var t = c1.Mass + c2.Mass;

                var h2 = c2.Location - c1.Location;

                var t2 = h2.Length() * h2.Length();
                var t3 = (c2.Dv - c1.Dv).InnerProduct(h2);

                c2.Dv = c2.Dv - (2.0 * c1.Mass / t) * t3 / t2 * h2;
            }
        }
    }
}
