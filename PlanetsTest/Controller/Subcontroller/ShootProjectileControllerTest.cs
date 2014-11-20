using System;
using System.Drawing;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using Planets.View;

namespace PlanetsTest.Controller.Subcontroller
{
    [TestClass]
    public class ShootProjectileControllerTest
    {
        [TestMethod]
        public void UnitTest_ShootProjectileController_Constructor()
        {
            // Arrange
            Playfield pf = new Playfield();
            GameView gv = new GameView(pf);

            // Act
            ShootProjectileController spc = new ShootProjectileController(pf, gv);

            // Assert
            Assert.AreEqual(pf, spc.InternalPlayfield);
            Assert.AreEqual(gv, spc.InternalControl);
        }

        [TestMethod]
        public void UnitTest_ShootProjectileController_Click()
        {
            // Arrange
            Playfield pf = new Playfield();
            GameView gv = new GameView(pf);
            ShootProjectileController spc = new ShootProjectileController(pf, gv);
            int objectCount = pf.GameObjects.Count;

            // Act
            spc.Clicked(new Point());

            // Assert
            Assert.AreEqual(objectCount + 1, pf.GameObjects.Count);
        }
    }
}
