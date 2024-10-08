using ElevatorManagementSystem.Base.Enums;
using ElevatorManagementSystem.Base.Models;
using ElevatorManagementSystem.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ElevatorManagementSystem.Tests
{
    [TestClass]
    public class ElevatorManagerTests
    {
        private BuildingElevatorsManager ElevatorManager;

        [TestInitialize]
        public virtual void Setup()
        {
            ElevatorManager = new BuildingElevatorsManager();
        }

        [TestMethod]
        public void AssignsRequestCorrectly()
        {
            var buildingElevatorsManager = new BuildingElevatorsManager();

            var firstRequestElevator = buildingElevatorsManager.ProcessRequest(new ExternalRequest(2, RequestDirection.Up));

            Assert.AreEqual("Bottom elevator", firstRequestElevator.Name);
            Assert.IsTrue(firstRequestElevator.UpRequests.Any(), "Bottom elevator should have at least one Up request");

            var secondRequestElevator = buildingElevatorsManager.ProcessRequest(new InternalRequest(2, 3));

            Assert.AreEqual("Bottom elevator", secondRequestElevator.Name);
            Assert.AreEqual(2, secondRequestElevator.UpRequests.Count, "Bottom elevator should have two Up requests");

            var thirdRequestElevator = buildingElevatorsManager.ProcessRequest(new ExternalRequest(8, RequestDirection.Up));

            Assert.AreEqual("Top elevator", thirdRequestElevator.Name);
            Assert.AreEqual(1, thirdRequestElevator.UpRequests.Count, "Top elevator should have one Up request");
        }
    }
}
