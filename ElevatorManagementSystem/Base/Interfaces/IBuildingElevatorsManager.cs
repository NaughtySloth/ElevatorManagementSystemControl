using ElevatorManagementSystem.Base.Models;
using System.Threading.Tasks;

namespace ElevatorManagementSystem.Base.Interfaces
{
    public interface IBuildingElevatorsManager
    {
        Elevator ProcessRequest(Request request);
        Task StartOperation();
    }
}
