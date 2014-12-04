using Planets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Controller.PhysicsRules
{
	class ExplosionRule : AbstractCollisionRule
	{
		protected override void DoCollision(Playfield pf, GameObject go1, GameObject go2, double ms)
		{
			// Check de afstand tot de objecten.
			double L = (go1.Location - go2.Location).Length();

			if (go1.Radius + go2.Radius <= L) 
				return;

			// Check for explosion flags
			if (! go1.Traits.HasFlag(Rule.EXPLODES) && !go2.Traits.HasFlag(Rule.EXPLODES))
				return;

			GameObject goExplodes, goPlayer;

			// Determine which object is the exploding object.
			if (go1.Traits.HasFlag(Rule.EXPLODES)) {
				goExplodes = go1;
				goPlayer = go2;
			} else {
				goPlayer = go1;
				goExplodes = go2;
			}

			// Go boom!
			double lostMass = goPlayer.Mass / 2;

			// TODO: We should probably create an epic explosion before removing the object.
			pf.BOT.Remove(goExplodes);

			Random random = new Random();

			double massPool = lostMass;
			while (massPool > 0) {
				double mass = random.Next(1000, 3000);
				Vector createLocation = goExplodes.Location + new Vector(random.Next(-50, 100), random.Next(-50, 100));
				GameObject debris = new GameObject(createLocation, new Vector(random.Next(-500, 500), random.Next(-500, 500)), mass);

				pf.BOT.Add(debris);
				massPool -= mass;
			}

			goPlayer.Mass -= lostMass;
		}
	}
}
