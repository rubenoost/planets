using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class ResetRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Reset if too low
            if (pf.CurrentPlayer.Mass < 10)
            {
                pf.BOT.Clear();

                pf.CurrentPlayer = new Player(new Vector(200, 200), new Vector(0, 0), Utils.StartMass);
                pf.CurrentPlayer.Location = new Vector(pf.Size.Width / 2, pf.Size.Height / 2);

                //pf.BOT.Add(new GameObject(new Vector(600, 600), new Vector(0, 0), Utils.StartMass - 0.05 * Utils.StartMass));

                pf.BOT.Add(new BlackHole(new Vector(50, 50), new Vector(0, 0), 1000000));
                pf.BOT.Add(new BlackHole(new Vector(50, 1030), new Vector(0, 0), 1000000));
                pf.BOT.Add(new BlackHole(new Vector(1870, 50), new Vector(0, 0), 1000000));
                pf.BOT.Add(new BlackHole(new Vector(1870, 1030), new Vector(0, 0), 1000000));
            }
        }
    }
}
