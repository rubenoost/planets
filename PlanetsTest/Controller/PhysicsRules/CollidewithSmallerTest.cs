using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Controller.GameRules.Impl;
using Planets.Model;
using Planets.Model.GameObjects;

namespace PlanetsTest.Controller.PhysicsRules
{
    [TestClass]
    public class CollidewithSmallerTest
    {
        [TestMethod]
        public void UnitTest_CollidewithSmaller_Change()
        {
            var go1 = new Player(new Vector(100, 100), new Vector(10, 10), 300);
            var go2 = new GameObject(new Vector(110, 110), new Vector(-5, -5), 300);
            var pf = new Playfield(300, 300);

            pf.GameObjects.Add(go1);
            pf.GameObjects.Add(go2);

            var oldGo1Mass = go1.Mass;

            CollidewithSmaller.Change(go1, go2, pf);

            Assert.AreEqual(oldGo1Mass + 300.00, go1.Mass, "Collided right");
        }
    }
}
