# PragueParking2.0

Inlämning C# de2
Sarantsetseg Hedenfalk

The program saves Parkingspots in json file called garage.json which is in the project folder.
There is also App.config file which contains information such as number of parkingspots, fees, vehicles types
and size of vehcile types. The file is in the project folder too.



When the program is starts, it shows menu with 4 alternatives

    1. Leave a vehicle for parking
    2. Change a vehicle's parking spot
    3. Get your vehicle
    4. Search for a vehicle

Then user shall choose from options by using arrow buttons on a keyboard.

When entered
    option 1 to leave a vehicle for parking

    - user is asked to enter vehicle's registration number
      the system repeats the question untill user enters string (max length 10 characters)

    - next, user is asked to enter vehicle type
      the system repeats the question untill user enters 1 of 2 options: car or mc
    - when user chooses vehicle type, then the system saves vehicle in the parkering array
	  if it is a car, then it will be saved in a first empty parkingspots
	  if it is mc, then it will be saved in the first found parkingspot that has 1 mc saved
	               otherwise it will be saved in a first empty parkingspots 
    - the system prints updated parkingspot where changes has been made


    option 2 to change a vehicle's parking spot
    - user is asked to enter vehicle's registration number
      the system repeats the question untill user enters valid registration number 
	  (registration number that is saved in the system)
    - user is asked to enter parking spot number where user wants to move the vehicle
      the system repeats the question untill user enters parkingspot number that is empty or if user wants to move mc  
	  then destination parkingspot can have 1 mc saved
    - the system prints updated parkingspot where changes has been made
	

    option 3 to get your vehicle
    - user is asked to enter vehicle's registration number
    - the system repeats the question untill user enters string (max length 10 characters) and
      registration number that is saved in the system
    - when user enters the valid registration number, system will remove the vehicle from the system
    - system prints parkering charge of the vehicle
	- the system prints updated parkingspot where changes has been made
	

    option 4 to search for a vehicle
    - user is asked to enter vehicle's registration number
    - the system repeats the question untill user enters string (max length 10 characters) and
      registration number that is saved in the system
    - when user enters the valid registration number, system will prints its parkingspot number




