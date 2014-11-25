using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Model;
using Planets.Controller;
using Planets.Controller.Subcontrollers;
using Planets.View;

namespace PlanetsTest.Controller
{
    [TestClass]
    public class AutoDemoTest
    {
        [TestMethod]
        public void UnitTestUnitTest_AutoDemo_Constructor()
        {
            // Arrange
            var pf = new Playfield(1920, 1080);
            var gv = new GameView(pf);
            var spc = new ShootProjectileController(pf, gv);

            // Act
            /*var ad = new Autodemo(pf,spc);

            // Assert
            Assert.AreEqual(pf, ad.field, "Playfield correctly saved");
            Assert.AreEqual(spc, ad.spc, "ShootProjectileController correctly saved");*/
        }


    }
}
