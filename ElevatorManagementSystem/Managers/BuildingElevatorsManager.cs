using ElevatorManagementSystem.Base.Enums;
using ElevatorManagementSystem.Base.Exceptions;
using ElevatorManagementSystem.Base.Interfaces;
using ElevatorManagementSystem.Base.Models;
using System;
using System.Threading.Tasks;

namespace ElevatorManagementSystem.Managers
{
    /// <summary>
    /// Class responsible for controlling all the elevators in a building, instantiating individual elevators and their managers
    /// </summary>
    public class BuildingElevatorsManager : IBuildingElevatorsManager
    {
        private const int _numberOfFloors = 10;
        private readonly int TopOptimalIdleFloor = 7;
        private readonly int BottomOptimalIdleFloor = 0;

        private readonly Elevator _topElevator;
        private readonly Elevator _bottomElevator;

        private readonly ElevatorManager _topElevatorManager;
        private readonly ElevatorManager _bottomElevatorManager;

        public BuildingElevatorsManager()
        {
            // we start off the elevators at optimal resting positions
            _topElevator = new Elevator(TopOptimalIdleFloor, "Top elevator");
            _bottomElevator = new Elevator(BottomOptimalIdleFloor, "Bottom elevator");

            _topElevator.ElevatorIdleThreshold += HandleIdleElevatorTimer;
            _bottomElevator.ElevatorIdleThreshold += HandleIdleElevatorTimer;

            _topElevatorManager = new ElevatorManager(_topElevator);
            _bottomElevatorManager = new ElevatorManager(_bottomElevator);

        }

        /// <summary>
        /// Execute this after adding all desired requests via ProcessRequest method
        /// </summary>
        public async Task StartOperation()
        {
            Console.WriteLine("Starting Building Elevator Manager and all the Elevators");

            await Task.WhenAll(_topElevatorManager.Start(), _bottomElevatorManager.Start());
        }

        /// <summary>
        /// This method compares the status of the elevators and passes either the destination floor if the elevator is on the move
        /// or the current floor if the elevator is idling
        /// Depending on those conditions we send different parameters to the SendIdleElevatorToOptimalFloor method
        /// </summary>
        /// <param name="request"></param>
        public Elevator ProcessRequest(Request request)
        {
            int topElevatorFloor = _topElevator.Status == ElevatorStatus.Idle ? _topElevator.CurrentFloor : _topElevator.DestinationFloor;
            int bottomElevatorFloor = _bottomElevator.Status == ElevatorStatus.Idle ? _bottomElevator.CurrentFloor : _bottomElevator.DestinationFloor;

            return AssignRequestToOptimalElevator(topElevatorFloor, bottomElevatorFloor, request);
        }

        private Elevator AssignRequestToOptimalElevator(int topElevatorFloor, int bottomElevatorFloor, Request request)
        {
            var requestedFloor = request is InternalRequest internalRequest ? internalRequest.DestinationFloor : request.OriginFloor;

            if (requestedFloor > _numberOfFloors)
            {
                throw new InvalidRequestException("Building isn't tall enough for this request!!!");
            }

            var closerElevator = GetCloserElevator(topElevatorFloor, bottomElevatorFloor, requestedFloor);

            AssignRequest(closerElevator, request);

            return closerElevator;

            // I would add here the logic to optimize the location of the resting elevator to ensure minimum wait time for any future passengers
            // to do this I would call the SendIdleElevatorToOptimalFloor with the idle elevator (if any is idle now)
        }

        private Elevator GetCloserElevator(int topElevatorFloor, int bottomElevatorFloor, int requestedFloor)
        {
            return Math.Abs(topElevatorFloor - requestedFloor) > Math.Abs(bottomElevatorFloor - requestedFloor)
                ? _bottomElevator
                : _topElevator;
        }

        private void HandleIdleElevatorTimer(object sender, EventArgs e)
        {
            Console.WriteLine("Handling Idle Elevator after timer threshold reached.");

            SendIdleElevatorToOptimalFloor(sender as Elevator);
        }

        private void SendIdleElevatorToOptimalFloor(Elevator idleElevator)
        {
            // here I would put logic which would assign an elevator to one of the default floors to rest in order to ensure minimal wait time
            // to do this I would take the current floor of the idle elevator and the destination floor of the moving elevator
            // with these two floors I could calculate which of the two default floors (0,7) is better to rest at for the idle elevator

            // for the time being we will just send it to rest at ground floor

            Console.WriteLine("Sending idle elevator to ground floor.");

            AssignRequest(idleElevator, new InternalRequest(idleElevator.CurrentFloor, 0));
        }

        private void AssignRequest(Elevator elevator, Request request)
        {
            if (request.Direction == RequestDirection.Up)
            {
                elevator.UpRequests.Enqueue(request);
            }
            else
            {
                elevator.DownRequests.Enqueue(request);
            }
        }
    }
}
