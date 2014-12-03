using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlanetsTest.Controller.PhysicsRules {
    [TestClass]
    public class BlackHoleRuleTest {
        /*[TestMethod]
        public void BlackHoleRuleGetsExecuted() {
            //Arrange
            var a = new BlackHoleRuleTest();
            var field = new Playfield(1920, 1080);

            //Act
            bool succes = true;
            try {
                foreach(GameObject g in field.BOT) {
                    if(g is BlackHole) {
                        foreach(GameObject g2 in field.BOT.Where(p => p.Traits.HasFlag(Rule.AFFECTED_BY_BH))) {
                            if(g != g2 && !(g2 is Player)) {
                                Vector V = g.Location - g2.Location;
                                double Fg = 10000.1 * ((g2.mass * g.mass) / (V.Length() * V.Length()));
                                g2.DV += V.ScaleToLength(Fg * (100 / 1000.0));
                            }
                        }
                    }
                }
            } catch(Exception){
                succes = false;
            }

            //Assert
            Assert.IsTrue(succes, "Succesfully executed");
        }*/
    }
}
