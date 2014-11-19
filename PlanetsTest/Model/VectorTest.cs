using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Model;

namespace PlanetsTest.Model
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void UnitTest_Vector_Constructor_Empty()
        {
            // Arrange
            double expectedX = 0;
            double expectedY = 0;

            // Act
            var v = new Vector();

            // Assert
            Assert.AreEqual(expectedX, v.X, "Comparing x of vector (empty constructor)");
            Assert.AreEqual(expectedY, v.Y, "Comparing y of vector (empty constructor)");
        }

        [TestMethod]
        public void UnitTest_Vector_Constructor_Parameters()
        {
            // Arrange
            double expectedX = 1.3;
            double expectedY = 154782.2;

            // Act
            var v = new Vector(expectedX, expectedY);

            // Assert
            Assert.AreEqual(expectedX, v.X, "Comparing x of vector (filled constructor)");
            Assert.AreEqual(expectedY, v.Y, "Comparing y of vector (filled constructor)");
        }
    }
}
