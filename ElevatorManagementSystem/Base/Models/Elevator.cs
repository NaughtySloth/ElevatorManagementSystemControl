using ElevatorManagementSystem.Base.Enums;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ElevatorManagementSystem.Base.Models
{
    /// <summary>
    /// The Elevator class has no complex logic in it by design. All of that should be handled by the ElevatorManager.
    /// </summary>
    public class Elevator
    {
        private System.Threading.Timer _idleTimer;
        private const int IdleTimeThreshold = 10000; // Time in ms before an elevator is considered idle (adjust as needed)

        public int CurrentFloor { get; set; }
        public int DestinationFloor { get; set; }
        public string Name { get; set; }
        public ElevatorStatus Status { get; set; }

        public Queue<Request> UpRequests { get; set; } = new Queue<Request>();
        public Queue<Request> DownRequests { get; set; } = new Queue<Request>();

        // Event to trigger when the elevator becomes idle
        public event EventHandler ElevatorIdleThreshold;

        public Elevator(int initialFloor, string name)
        {
            CurrentFloor = initialFloor;
            Name = name;
            Status = ElevatorStatus.Idle;
        }

        // Method to start the idle timer if it's not already running
        public void StartElevatorIdleTimerIfNotRunning()
        {
            if (_idleTimer == null)
            {
                Console.WriteLine($"{Name} starting idle timer.");

                _idleTimer = new System.Threading.Timer(OnElevatorIdleThresholdReached, null, IdleTimeThreshold, Timeout.Infinite);
            }
            else
            {
                Console.WriteLine($"{Name} idle timer is already running.");
            }
        }

        // Method to handle what happens when the idle timer reaches its threshold
        private void OnElevatorIdleThresholdReached(object state)
        {
            // Raise the event to notify listeners (e.g., the manager) that the elevator is idle for too long
            ElevatorIdleThreshold?.Invoke(this, EventArgs.Empty);

            // Stop the timer after it's triggered
            _idleTimer.Dispose();
            _idleTimer = null;
        }
    }
}
