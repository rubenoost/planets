using Planets.Model;

namespace Planets.Controller.PhysicsRules
{
    class ResetRule : AbstractGameRule
    {
        protected override void ExecuteRule(Playfield pf, double ms)
        {
            // Reset if too low
            if (pf.CurrentPlayer.Mass < 50)
            {
                pf.BOT.Clear();

                pf.CurrentPlayer = new Player(new Vector(200, 200), new Vector(0, 0), Utils.StartMass);
                pf.CurrentPlayer.Location = new Vector(pf.Size.Width / 2, pf.Size.Height / 2);

                //pf.BOT.Add(new GameObject(pf.CurrentPlayer.Location + new Vector(200, 0), new Vector(0, 0), Utils.StartMass / 2));
                //pf.BOT.Add(new GameObject(pf.CurrentPlayer.Location + new Vector(-200, 0), new Vector(0, 0), Utils.StartMass / 2));
                //pf.BOT.Add(new GameObject(pf.CurrentPlayer.Location + new Vector(0, 200), new Vector(0, 0), Utils.StartMass / 2));
                //pf.BOT.Add(new GameObject(pf.CurrentPlayer.Location + new Vector(0, -200), new Vector(0, 0), Utils.StartMass / 2));
                pf.BOT.Add(new Antagonist(pf.CurrentPlayer.Location + new Vector(200, 200), new Vector(0, 0), Utils.StartMass));
				// Anti Gravity
				pf.BOT.Add(new Mine(new Vector(500, 300), new Vector(0, 0), Utils.StartMass / 2));

                // Black holes
                pf.BOT.Add(new BlackHole(new Vector(50, 50), new Vector(0, 0), 1000000));
                pf.BOT.Add(new BlackHole(new Vector(50, 1030), new Vector(0, 0), 1000000));
                pf.BOT.Add(new BlackHole(new Vector(1870, 50), new Vector(0, 0), 1000000));
                pf.BOT.Add(new BlackHole(new Vector(1870, 1030), new Vector(0, 0), 1000000));

                pf.BOT.Add(new Stasis(new Vector(1200, 800), new Vector(0, 0), 800));
                pf.BOT.Add(new Tardis(new Vector(800, 200), new Vector(0, 0), 0));
            }

            if (pf.CurrentPlayer.Mass > 30100)
            {
                pf.BOT.Clear();

                pf.CurrentPlayer = new Player(new Vector(200, 200), new Vector(0, 0), Utils.StartMass);
                pf.CurrentPlayer.Location = new Vector(1000, pf.Size.Height / 2);
                pf.BOT.Add(new Antagonist(pf.CurrentPlayer.Location + new Vector(200, 200), new Vector(0, 0), Utils.StartMass));

            }
        }
    }
}
