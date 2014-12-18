﻿using System;
using Planets.Controller.GameRules.Abstract;
using Planets.Model;
using Planets.Model.GameObjects;

namespace Planets.Controller.GameRules.Impl
{
    class ExplosionRule : AbstractCollisionRule
    {
        protected override void DoCollision(Playfield pf, ScoreBoard sb, GameObject go1, GameObject go2, double ms)
        {
            // Check de afstand tot de objecten.
            if (!go1.IntersectsWith(go2)) return;

            // Check for explosion flags
            if (!go1.Is(Rule.EXPLODES) && !go2.Is(Rule.EXPLODES))
                return;

            GameObject goExplodes, goPlayer;

            // Determine which object is the exploding object.
            if (go1.Is(Rule.EXPLODES))
            {
                goExplodes = go1;
                goPlayer = go2;
            }
            else
            {
                goPlayer = go1;
                goExplodes = go2;
            }

            // Go boom!
            //pf.sb.AddScore(new Score(-50, DateTime.Now, go1.Location, true));
            double lostMass = goPlayer.Mass / 2;
            goPlayer.Mass -= lostMass;

			AnimatedGameObject explosion = new AnimatedGameObject(goExplodes.Location, new Vector(0, 0), 500);
            pf.BOT.Add(explosion);
            
			pf.BOT.Remove(goExplodes);

            Random random = new Random();

            double massPool = lostMass;
            while (massPool > 0.1)
            {
                // We don't want to generate more mass than is actually lost by the player.
                double mass = massPool >= 1000 ? random.Next(10, (int)massPool) : Math.Max(massPool, random.Next(500, 1000));

                Vector createLocation = goExplodes.Location + new Vector(random.Next(-50, 100), random.Next(-50, 100));
                GameObject debris = new GameObject(createLocation, new Vector(random.Next(-500, 500), random.Next(-500, 500)), mass);
                debris.Radius = goExplodes.Radius*3.0;
                pf.BOT.Add(debris);
                massPool -= mass;
            }
        }
    }
}