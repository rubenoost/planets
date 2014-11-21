using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class ResetRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Reset if too low
            if (pf.CurrentPlayer.mass < 10)
            {
                pf.GameObjects.Clear();
                pf.CurrentPlayer = new Player(200, 200, 0, 0, Utils.StartMass);
                pf.GameObjects.Add(new BlackHole(600, 600, 0, 0, 1000000, 1));
            }
        }
    }
}
