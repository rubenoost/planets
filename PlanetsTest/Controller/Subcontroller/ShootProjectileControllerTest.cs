using System;
using System.Drawing;
using System.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using Planets.View;

namespace PlanetsTest.Controller.Subcontroller
{
    [TestClass]
    public class ShootProjectileControllerTest
    {
        /// <summary>
        /// Create new ShootProjectileController, check setting of given vars.
        /// </summary>
        [TestMethod]
        public void UnitTest_ShootProjectileController_Constructor()
        {
            // Arrange
            var pf = new Playfield(1920, 1080);
            var gv = new GameView(pf);

            // Act
            var spc = new ShootProjectileController(pf, gv);

            // Assert
            Assert.AreEqual(pf, spc.InternalPlayfield, "Playfield correctly saved");
            Assert.AreEqual(gv, spc.InternalControl, "Control correctly saved");
        }

        /// <summary>
        /// Click once on the field, and the amount of GameObjects should be one higher.
        /// </summary>
        [TestMethod]
        public void UnitTest_ShootProjectileController_Click_Once()
        {
            // Arrange
            var pf = new Playfield(1920, 1080) {CurrentPlayer = new Player(0, 0, 0, 0, 100)};
            var gv = new GameView(pf);
            var spc = new ShootProjectileController(pf, gv);
            var objectCount = pf.GameObjects.Count;

            // Act
            spc.Clicked(new Point());

            // Assert
            Assert.AreEqual(objectCount + 1, pf.GameObjects.Count, "Shooting once");
        }

        /// <summary>
        /// Click multiple times on the field, and the amount of GameObjects should be that many higher.
        /// </summary>
        [TestMethod]
        public void UnitTest_ShootProjectileController_Click_Multiple()
        {
            // Arrange
            var pf = new Playfield(1920, 1080) {CurrentPlayer = new Player(0, 0, 0, 0, 100)};
            var gv = new GameView(pf);
            var spc = new ShootProjectileController(pf, gv);
            var objectCount = pf.GameObjects.Count;
            var count = new Random().Next(20);

            // Act
            for(int i = 0; i < count; i++)
                spc.Clicked(new Point());

            // Assert
            Assert.AreEqual(objectCount + count, pf.GameObjects.Count, "Shooting " + count + " times");
        }
    }
}
