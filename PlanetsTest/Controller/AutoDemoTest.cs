using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets;
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
            PlanetsForm pform = new PlanetsForm();

            // Act
            var ad = new Autodemo(spc,new GameEngine(new MainEngine(pform), pform));

            // Assert
            Assert.AreEqual(pf, ad.Spc.InternalPlayfield, "Playfield correctly saved");
            Assert.AreEqual(spc, ad.Spc, "ShootProjectileController correctly saved");
        }

        [TestMethod]
        public void UnitTestUnitTest_AutoDemo_TestDemo()
        {
            // Arrange
            var pf = new Playfield(1920, 1080) {CurrentPlayer = new Player(new Vector(500, 500), new Vector(1, 1), 100)};
            var gv = new GameView(pf);
            var spc = new ShootProjectileController(pf, gv);
            PlanetsForm pform = new PlanetsForm();

            var ad = new Autodemo(spc, new GameEngine(new MainEngine(pform), pform))
            {
                WaitTimeBetweenClick = 1,
                WaitTimeBetweenClicks = 1
            };
            int startCount = pf.GameObjects.Count;

            // Act
            Thread.Sleep(10);

            // Assert
            Assert.IsTrue(ad.Spc.InternalPlayfield.GameObjects.Count > startCount, "GameObjects spawned");
        }

        [TestMethod]
        public void UnitTestUnitTest_AutoDemo_StopDemo()
        {
            // Arrange
            var pf = new Playfield(1920, 1080) { CurrentPlayer = new Player(new Vector(500, 500), new Vector(1, 1), 100) };
            var gv = new GameView(pf);
            var spc = new ShootProjectileController(pf, gv);
            PlanetsForm pform = new PlanetsForm();

            var ad = new Autodemo(spc, new GameEngine(new MainEngine(pform), pform))
            {
                WaitTimeBetweenClick = 1,
                WaitTimeBetweenClicks = 1
            };

            // Act
            Thread.Sleep(10);
            ad.StopDemo();
            Thread.Sleep(10);
            int startCount = pf.GameObjects.Count;
            Thread.Sleep(10);

            // Assert
            Assert.AreEqual(startCount, ad.Spc.InternalPlayfield.GameObjects.Count, "GameObjects spawned");
        }
    }
}
