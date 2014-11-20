using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Model;

namespace PlanetsTest.Model
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UnitTest_GameObject_IntersectWith_True()
        {
            // Arrange
            var go1 = new GameObject(0, 0, 0, 0, 400, false);
            var go2 = new GameObject(0, 20, 0, 0, 400, false);
            
            // Act
            bool valid = go1.IntersectsWith(go2);

            // Assert
            Assert.IsTrue(valid, "Colliding objects.");
        }
    }
}
