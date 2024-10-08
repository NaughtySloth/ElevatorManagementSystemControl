using ElevatorManagementSystem.Base.Enums;
using ElevatorManagementSystem.Base.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ElevatorManagementSystem.Managers
{
    public class ElevatorManager
    {
        private readonly Elevator _elevator;
        private const int MovementDelay = 5000;

        public ElevatorManager(Elevator elevator)
        {
            _elevator = elevator;
        }

        public async Task Start()
        {
            while (_elevator.UpRequests.Any() || _elevator.DownRequests.Any())
            {
                Console.WriteLine($"{_elevator.Name} Processing requests...");

                await ProcessRequests();
            }

            Console.WriteLine($"{_elevator.Name} Processed all requests. Elevator Idle");

            SetElevatorIdle();
        }

        private async Task ProcessRequests()
        {
            // a real-life elevator should first process requests from passengers that need to go up before going down
            if (_elevator.Status == ElevatorStatus.GoingUp || _elevator.Status == ElevatorStatus.Idle)
            {
                await ProcessRequestsByDirection(RequestDirection.Up);
                await ProcessRequestsByDirection(RequestDirection.Down);
            }
            else
            {
                await ProcessRequestsByDirection(RequestDirection.Down);
                await ProcessRequestsByDirection(RequestDirection.Up);
            }
        }

        private async Task ProcessRequestsByDirection(RequestDirection direction)
        {
            var requests = direction == RequestDirection.Up ? _elevator.UpRequests : _elevator.DownRequests;

            while (requests.Any())
            {
                Request request = requests.Dequeue();
                _elevator.DestinationFloor = request is InternalRequest internalRequest ? internalRequest.DestinationFloor : request.OriginFloor;

                Console.WriteLine($"{_elevator.Name} going {direction.ToString().ToLower()} to floor {_elevator.DestinationFloor}");

                _elevator.Status = direction == RequestDirection.Up ? ElevatorStatus.GoingUp : ElevatorStatus.GoingDown;
                await Task.Delay(MovementDelay);

                Console.WriteLine($"{_elevator.Name} Processing {direction.ToString().ToLower()} requests. Stopped at floor {_elevator.CurrentFloor}");
            }

            // Change status to next possible state (e.g., from Up to Down or vice versa)
            SwitchElevatorStatus(direction);
        }

        private void SwitchElevatorStatus(RequestDirection currentDirection)
        {
            var oppositeDirection = currentDirection == RequestDirection.Up ? RequestDirection.Down : RequestDirection.Up;
            var oppositeRequests = oppositeDirection == RequestDirection.Up ? _elevator.UpRequests : _elevator.DownRequests;

            if (oppositeRequests.Any())
            {
                _elevator.Status = oppositeDirection == RequestDirection.Up ? ElevatorStatus.GoingUp : ElevatorStatus.GoingDown;
            }
            else
            {
                SetElevatorIdle();
            }
        }

        private void SetElevatorIdle()
        {
            _elevator.Status = ElevatorStatus.Idle;

            // Ensure idle timer is started if not already
            _elevator.StartElevatorIdleTimerIfNotRunning();
        }
    }
}
