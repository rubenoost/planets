using System.Drawing;
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

        [TestMethod]
        public void UnitTest_Vector_Addition()
        {
            // Arrange
            var v1 = new Vector(15.5, -12.7);
            var v2 = new Vector(-.3, .3);
            var expected = new Vector(15.2, -12.4);

            // Act
            var actual = v1 + v2;

            // Assert
            Assert.AreEqual(expected.X, actual.X, 0.00001, "Adding 2 vectors (X)");
            Assert.AreEqual(expected.Y, actual.Y, 0.00001, "Adding 2 vectors (Y)");
        }

        [TestMethod]
        public void UnitTest_Vector_Subtraction()
        {
            // Arrange
            var v1 = new Vector(15.5, -12.7);
            var v2 = new Vector(-.3, .3);
            var expected = new Vector(15.8, -13.0);

            // Act
            var actual = v1 - v2;

            // Assert
            Assert.AreEqual(expected.X, actual.X, 0.00001, "Subtracting 2 vectors (X)");
            Assert.AreEqual(expected.Y, actual.Y, 0.00001, "Subtracting 2 vectors (Y)");
        }

        [TestMethod]
        public void UnitTest_Vector_Multiplication_Lefthand()
        {
            // Arrange
            var v1 = new Vector(15.5, -12.7);
            var v2 = 2.0;
            var expected = new Vector(31.0, -25.4);

            // Act
            var actual = v2*v1;

            // Assert
            Assert.AreEqual(expected.X, actual.X, 0.00001, "Multiply scalar with vectors lefthanded (X)");
            Assert.AreEqual(expected.Y, actual.Y, 0.00001, "Multiply scalar with vectors lefthanded (Y)");
        }

        [TestMethod]
        public void UnitTest_Vector_Multiplication_Righthand()
        {
            // Arrange
            var v1 = new Vector(15.5, -12.7);
            var v2 = 2.0;
            var expected = new Vector(31.0, -25.4);

            // Act
            var actual = v1 * v2;

            // Assert
            Assert.AreEqual(expected.X, actual.X, 0.00001, "Multiply scalar with vectors righthanded (X)");
            Assert.AreEqual(expected.Y, actual.Y, 0.00001, "Multiply scalar with vectors righthanded (Y)");
        }

        [TestMethod]
        public void UnitTest_Vector_ScaleToLength()
        {
            // Arrange
            var v1 = new Vector(3.0, 4.0);
            var l = 10.0;
            var expected = new Vector(6.0, 8.0);

            // Act
            var actual = v1.ScaleToLength(l);

            // Assert
            Assert.AreEqual(expected.X, actual.X, 0.00001, "Scale vector to length (X)");
            Assert.AreEqual(expected.Y, actual.Y, 0.00001, "Scale vector to length (Y)");
        }

        [TestMethod]
        public void UnitTest_Vector_InnerProduct()
        {
            // Arrange
            var v1 = new Vector(3.0, 4.0);
            var v2 = new Vector(-5.0, 12.0);
            var expected = 33.0;

            // Act
            var actual = v1.InnerProduct(v2);

            // Assert
            Assert.AreEqual(expected, actual, 0.00001, "Inner product");
        }

        [TestMethod]
        public void UnitTest_Vector_ToString()
        {
            // Arrange
            var v = new Vector(3.0, 4.0);
            var expected = "3.000,4.000";

            // Act
            var actual = v.ToString();

            // Assert
            Assert.AreEqual(expected, actual, "Inner product");
        }

        [TestMethod]
        public void UnitTest_Vector_Casting_From_Point()
        {
            // Arrange
            var p = new Point(3, 4);
            var expected = new Vector(3.0, 4.0);

            // Act
            var actual = p;

            // Assert
            Assert.AreEqual(expected.X, actual.X, 0.00001, "Casting from point (X)");
            Assert.AreEqual(expected.Y, actual.Y, 0.00001, "Casting from point (Y)");
        }
    }
}
