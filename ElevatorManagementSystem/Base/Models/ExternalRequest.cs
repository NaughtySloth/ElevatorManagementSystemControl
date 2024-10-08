using ElevatorManagementSystem.Base.Enums;

namespace ElevatorManagementSystem.Base.Models
{
    /// <summary>
    /// External Request class represents a request made by a passenger from outside the elevator calling the elevator to their floor
    /// We don't know which exact floor they are going to, but we know which floor they are on and the direction they are going to go
    /// </summary>
    public class ExternalRequest : Request
    {
        public ExternalRequest(int originFloor, RequestDirection direction)
        {
            OriginFloor = originFloor;
            Direction = direction;
        }
    }
}
