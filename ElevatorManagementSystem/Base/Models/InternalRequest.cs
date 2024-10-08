using ElevatorManagementSystem.Base.Enums;
using ElevatorManagementSystem.Base.Exceptions;

namespace ElevatorManagementSystem.Base.Models
{
    /// <summary>
    /// Internal Request class represents a request made by a passenger from within the elevator, meaning that we know which floor they want to go to
    /// </summary>
    public class InternalRequest : Request
    {
        /// <summary>
        /// Create a new Internal Request
        /// </summary>
        /// <param name="originFloor">
        /// The origin floor should be the same as the Elevator's current floor
        /// </param>
        /// <param name="destinationFloor">
        /// Simulate user input by assigning any destination floor
        /// </param>
        public InternalRequest(int originFloor, int destinationFloor)
        {
            this.OriginFloor = originFloor;
            DestinationFloor = destinationFloor;

            Direction = GetDirection(originFloor, destinationFloor);
        }

        private static RequestDirection GetDirection(int originFloor, int destinationFloor)
        {
            if (destinationFloor == originFloor)
            {
                throw new InvalidRequestException("Destination floor must be different than origin floor");
            }

            return originFloor > destinationFloor ? RequestDirection.Down : RequestDirection.Up;
        }

        /// <summary>
        /// Since the passenger has to select a floor from inside the elevator, this type of Request has a Destination Floor
        /// </summary>
        public int DestinationFloor { get; set; }
    }
}
