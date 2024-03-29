﻿using Planets.Controller.Subcontrollers;

namespace Planets.Model.GameObjects
{
    public class Player : GameObject
    {
        public bool GameOver;
        public bool GameWon;

        public Player() : this(new Vector(), new Vector(), Utils.StartMass) { }

        public Player(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass)
        {
            Traits = Traits & ~Rule.AffectedByBh & ~Rule.EatPlayer;
            GameOver = false;
            GameWon = false;
        }

        public GameObject ShootProjectile(Playfield pf, Vector direction)
        {
            GameObject projectile = new GameObject(new Vector(0, 0), new Vector(0, 0), Mass * 0.05);
            Vector newSpeed = ShootProjectileController.CalcNewDv(this, projectile, Location - direction);
            Mass -= projectile.Mass;
            Dv = newSpeed;
            pf.GameObjects.Add(projectile);
            return projectile;
        }
        public double CalcDistance(GameObject g) {
            double distance;
            distance = (Location - g.Location).Length();
            return distance;
        }
    }
}
