using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Controller.GameRules.Impl;
using Planets.Model;
using Planets.Model.GameObjects;

namespace PlanetsTest.Controller.PhysicsRules
{
    [TestClass]
    public class ElasticCollissionRuleTest
    {
        [TestMethod]
        public void UnitTest_ElasticCollissionRule_Working()
        {
            // Create two new BOT
            var go1 = new GameObject(new Vector(100, 100), new Vector(10, 10), 300);
            var go2 = new GameObject(new Vector(110, 110), new Vector(-5, -5), 300);

            // Playfield with the new two BOT
            var field = new Playfield(300, 300);
            field.GameObjects.Add(go1);
            field.GameObjects.Add(go2);

            // Save old DeltaV's
            var oldDv1 = go1.Dv;

            // Execute Collission Rule
            var elasticRule = new ElasticCollisionRule();
            elasticRule.Execute(field, 17);

            // Check if old Delta V is not equal to new Delta V, if so they have collided
            Assert.AreNotEqual(oldDv1.X, go1.Dv.X, 0.001);
        }
    }
}
