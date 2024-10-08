using ElevatorManagementSystem.Base.Enums;

namespace ElevatorManagementSystem.Base.Models
{
    public abstract class Request
    {
        public int OriginFloor { get; set; }
        public RequestDirection Direction { get; set; }
    }
}
