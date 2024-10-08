ASSIGNMENT:
How would you program 2 elevators for a 10-storey building?
Please draw some lines for the high-level algorithm you would use.
In order to express how you would do this, you can use:
- code or pseudocode
- diagrams to help express the logic on how it will work.
Additionally you should include your classes design, your assumptions, etc.
Please note that not a complete solution is expected, just a high level direction, maybe with details in the parts that you consider important.

elevator-management-system
Simple design for a two elevator system in a 10 story building

Note: Class diagram PDF is included in the root of the solution 

Assumptions:
- ground floor is marked as 0, top floor is marked as 10
- all the floors above the ground one have the same number of apartments and people living
in them. meaning the "demand" for each floor except the ground one is the same.
- the elevators move at the same speed relative to the other one.
- the number of people/weight of the elevator is ignored as a variable
- all the floors are above ground (positive integers 0-10)
- the ground floor has the highest demand so there should be an elevator close to it whenever possible
- operating the doors opening/closing is ignored
- elevator emergency stop is ignored


Logic to figure out:
- Default floor:
	considering we have two elevators, there is an optimal default floor for each of them during downtime (when they are not moving people).
	The default floors should be a combination of floors which results in minimum wait time for an elevator when calling it from any other floor
	the elevators should move to one of the default floors after finishing a passenger ride if no other calls are made
	only one elevator should rest on one of the default floors

- Handling calling an elevator while both are resting:
	figure out which of the two elevators is closer to the caller and send it to the passenger floor 
	(take the current position of the elevator and get the absolute number of floors from it to the caller)
	the other elevator should stay where it is while the other one is doing the pick up 
	possible optimization: after one of the elevators moves, re-calculate the optimal position of the other one
	for example if the default floors are 7 and 0 and a call is made from the ground floor to the top floor, the other resting elevator should move to the ground floor
	
- Handling calling an elevator while one is moving:
	to calculate which elevator should take the call, we need to take into consideration current floor of the resting elevator and the destination floor of the moving one.
	when we have those two floors, we should calculate which of the two is closer to the caller.
		if the moving elevator's destination is closer it should be the one that picks up the other passenger
			possible optimization: 
			if the elevator is going down and the call is made on the way down to the destination, it should stop and pick up the caller before completing
			the original ride
			if the elevator is moving up, it should first finish the original ride and then go pick up the caller
		if the resting elevator's position is closer it should pick up the caller
		
- Handling calling an elevator while both are carrying passengers (new passenger calling while both elevators are moving):
	to calculate which elevator should take the call, we need to take into consideration the destination floors of both elevators and calculate which one will be closer
	to the new passenger at the end of its current ride and send that one
	
- Handling passengers selecting multiple floors from within the elevator:
	first stop at the floor which is closer to the current floor(either up or down) and then proceed to final destination 
	(unless new inputs come in which case repeat same logic)
	
