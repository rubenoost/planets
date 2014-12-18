using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.View.Imaging;

namespace PlanetsTest.View
{
    [TestClass]
    public class SpriteTest
    {

        [TestMethod, Description("Tests the first constructor with filling all paramaters the constructor needs")]
        public void UnitTest_Sprite_Constructor()
        {
            // Arrange
            var bm = new List<Bitmap>();
            const int columns = new int();
            const int rows = new int();

            const int expectedbmcount = 0;
            const int expectedc = columns;
            const int expectedr = rows;
            const bool expectedcy = true;

            // Act
            var sp = new Sprite(bm, columns, rows, true);

            // Assert
            Assert.AreEqual(expectedbmcount,sp.Images.Count);
            Assert.AreEqual(expectedc,sp.Columns);
            Assert.AreEqual(expectedr,sp.Rows);
            Assert.AreEqual(expectedcy,sp.Cyclic);
        }

        [TestMethod, Description("Tests the second constructor with filling all paramaters the constructor needs")]
        public void UnitTest_Sprite_Constructor2()
        {
            // Arrange
            var bm = new Bitmap(200,200);
            const int columns = 1;
            const int rows = 2;

            const int expectedcount = 2;
            const int expectedc = columns;
            const int expectedr = rows;
            const bool expectedcy = true;

            // Act
            var sp = new Sprite(bm, columns, rows, true);

            // Assert
            Assert.AreEqual(expectedcount, sp.Images.Count);
            Assert.AreEqual(expectedc, sp.Columns);
            Assert.AreEqual(expectedr, sp.Rows);
            Assert.AreEqual(expectedcy, sp.Cyclic);
        }

        [TestMethod, Description("Tests the first constructor with filling only the bm parameter")]
        public void UnitTest_Sprite_Constructor3()
        {
            // Arrange
            var bm = new List<Bitmap>();

            const int expectedbmcount = 0;
            const int expectedc = 1;
            const int expectedr = 1;
            const bool expectedcy = false;

            // Act
            var sp = new Sprite(bm);

            // Assert
            Assert.AreEqual(expectedbmcount, sp.Images.Count);
            Assert.AreEqual(expectedc, sp.Columns);
            Assert.AreEqual(expectedr, sp.Rows);
            Assert.AreEqual(expectedcy, sp.Cyclic);
        }

        [TestMethod, Description("Tests the second constructor with filling only the bm parameter")]
        public void UnitTest_Sprite_Constructor4()
        {
            // Arrange
            var bm = new Bitmap(200, 200);

            var expectedbmcount = 1;
            const int expectedc = 1;
            const int expectedr = 1;
            const bool expectedcy = false;

            // Act
            var sp = new Sprite(bm);

            // Assert
            Assert.AreEqual(expectedbmcount, sp.Images.Count);
            Assert.AreEqual(expectedc, sp.Columns);
            Assert.AreEqual(expectedr, sp.Rows);
            Assert.AreEqual(expectedcy, sp.Cyclic);
        }

        [TestMethod]
        public void UnitTest_Sprite_GetFrame()
        {
            // Arrange
            int frame = 0;
            var bm = new Bitmap(100, 100);
            var sp = new Sprite(bm);

            var expectedbm = new Bitmap(100,100);
            // Act
            bm = sp.GetFrame(frame);

            // Assert
            Assert.AreEqual(expectedbm.Height,bm.Height);
            Assert.AreEqual(expectedbm.Width,bm.Width);
        }
    }
}
