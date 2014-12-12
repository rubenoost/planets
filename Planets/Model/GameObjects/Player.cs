using Planets.Controller.Subcontrollers;

namespace Planets.Model.GameObjects
{
    public class Player : GameObject
    {
        public Player(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass)
        {
            Traits = Traits & ~Rule.AFFECTED_BY_BH & ~Rule.EAT_PLAYER;
        }

        public void ShootProjectile(Playfield pf, Vector direction)
        {
            GameObject projectile = new GameObject(new Vector(0, 0), new Vector(0, 0), Mass * 0.05);
            Vector newSpeed = ShootProjectileController.CalcNewDV(this, projectile, Location - direction);
            Mass -= projectile.Mass;
            DV = newSpeed;
            pf.BOT.Add(projectile);
        }
        public void ShootProjectile(Playfield pf, Vector direction, bool Ai)
        {
            GameObject projectile = new GameObject(new Vector(0, 0), new Vector(0, 0), Mass * 0.05);
            projectile.Ai = true;
            Vector newSpeed = ShootProjectileController.CalcNewDV(this, projectile, Location + direction);
            Mass -= projectile.Mass;
            DV = newSpeed;
            pf.BOT.Add(projectile);
        }
    }
}
