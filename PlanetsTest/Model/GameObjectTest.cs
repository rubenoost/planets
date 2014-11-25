using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Controller.Subcontrollers;
using Planets.Model;
using System.Drawing;
using System.Windows.Forms;

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

        [TestMethod]
        public void UnitTest_GameObject_SpeedTest_True()
        {
            // Arrange
            var go1 = new GameObject(new Vector(0, 0), new Vector(0, 0), 400);
            var pf1 = new Playfield(300, 300);
            var p = new Player(new Vector(10, 10), new Vector(0, 0), 30);
            var control = new Control();

            pf1.CurrentPlayer = p;
            ShootProjectileController shootController = new ShootProjectileController(pf1, control);

            // Act
            shootController.Clicked(new Point(100, 100));

            bool valid = (p.DV.Length() > 0 && go1.DV.Length() > 0);

            // Assert
            Assert.IsTrue(valid, "Check default speed.");
        }
    }
}
