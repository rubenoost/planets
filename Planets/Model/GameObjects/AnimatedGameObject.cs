using System;

namespace Planets.Model.GameObjects
{
    public class AnimatedGameObject : GameObject
    {
        public DateTime Begin = DateTime.Now;

        public TimeSpan Duration = new TimeSpan(0, 0, 0, 2);

        public AnimatedGameObject()
            : this(new Vector(), new Vector(), Utils.StartMass)
        { }

        public AnimatedGameObject(Vector location, Vector velocity, double mass)
            : base(location, velocity, mass, Rule.None)
        {
        }
    }
}
