using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Model;

namespace PlanetsTest.Model
{
    [TestClass]
    public class GameObjectTest
    {
        [TestMethod]
        public void UnitTest_GameObject_IntersectWith_True()
        {
            // Arrange
            var go1 = new GameObject(new Vector(0, 0), new Vector(0, 0), 400);
            var go2 = new GameObject(new Vector(0, 20), new Vector(0, 0), 400);
            
            // Act
            bool valid = go1.IntersectsWith(go2);

            // Assert
            Assert.IsTrue(valid, "Colliding objects.");
        }
    }
}
