using ElevatorManagementSystem.Base.Models;
using ElevatorManagementSystem.Managers;
using System.Threading.Tasks;

namespace ElevatorManagementSystem
{
    public static class Program
    {
        public async static Task Main(string[] args)
        {
            var elevatorManager = new BuildingElevatorsManager();

            elevatorManager.ProcessRequest(new InternalRequest(0, 10));

            elevatorManager.ProcessRequest(new ExternalRequest(2, Base.Enums.RequestDirection.Down));
            elevatorManager.ProcessRequest(new InternalRequest(2, 0));

            await elevatorManager.StartOperation();
        }
    }
}
