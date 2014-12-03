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

			pf.BOT.Add(new GameObject(new Vector(goPlayer.Location.X + 100, goPlayer.Location.Y + 100), new Vector(goPlayer.DV.X, goPlayer.DV.Y), lostMass / 2));
			pf.BOT.Add(new GameObject(new Vector(goPlayer.Location.X - 100, goPlayer.Location.Y - 100), new Vector(goPlayer.DV.X, goPlayer.DV.Y), lostMass / 2));

			goPlayer.Mass -= lostMass;

			// TODO: We should probably create an epic explosion before removing the object.
			pf.BOT.Remove(goExplodes);
		}
	}
}
