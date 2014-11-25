using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Model;
using Planets.Controller.PhysicsRules;

namespace PlanetsTest.Controller.PhysicsRules
{
    [TestClass]
    public class ElasticCollissionRuleTest
    {
        [TestMethod]
        public void UnitTest_ElasticCollissionRule_Working()
        {
            // Create two new GameObjects
            var go1 = new GameObject(new Vector(100, 100), new Vector(10, 10), 300);
            var go2 = new GameObject(new Vector(110, 110), new Vector(-5, -5), 300);

            // Playfield with the new two GameObjects
            var field = new Playfield(300, 300);
            field.GameObjects.Add(go1);
            field.GameObjects.Add(go2);

            // Save old DeltaV's
            var OldDV1 = go1.DV;
            var OldDV2 = go2.DV;

            // Execute Collission Rule
            var ElasticRule = new ElasticCollisionRule();
            ElasticRule.Execute(field, 17);

            // Check if old Delta V is not equal to new Delta V, if so they have collided
            Assert.AreNotEqual(OldDV1.X, go1.DV.X, 0.001);
        }
    }
}
