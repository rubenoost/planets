using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Model;

namespace PlanetsTest.Model {
    [TestClass]
    public class BlackHoleTest {
        [TestMethod]
        public void BlackHoleIsBlackHole() {
            BlackHole b = new BlackHole(new Vector(0, 0), new Vector(0, 0), 10.0);
            if(b is BlackHole) {
                Assert.IsTrue(true, "Black Hole is Black Hole");
            } else {
                Assert.IsTrue(false, "Black Hole is no Black Hole");
            }
        }
    }
}
