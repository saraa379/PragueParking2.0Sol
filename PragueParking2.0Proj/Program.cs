/* Sarantsetseg Hedenfalk
 * Inlämnings uppgift C# del 2
 * Prague Parking 2.0
 */




using System;
using Spectre.Console;
using System.Configuration; //for the XML config file
using System.Collections.Specialized;//for the XML config file
using System.Collections.Generic;//for List
using System.IO; //for file reading and writing
using System.Linq;
using Newtonsoft.Json;

namespace PragueParking2._0Proj
{
    class Program
    {
        static void Main(string[] args)
        {

            //Welcome text
            Console.WriteLine();
            var table = new Table();
            table.Border = TableBorder.None;
            table.AddColumn(new TableColumn("[bold dodgerblue2]Welcome to the Prague Parking![/]").Centered());
            table.AddRow("[bold dodgerblue2]---------------------------------------------[/]");
            table.Expand();
            AnsiConsole.Render(table);

            Console.WriteLine();
            Console.WriteLine();

            PrintParkingSpots();

            Console.WriteLine("");
            Console.WriteLine("");
            //Menu starts here
            bool isOpen = true;

            while (isOpen)
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                // Ask for the user's favorite fruit

                var menuItem = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold paleturquoise1]Please choose from the menu below?[/]")
                        .PageSize(5)
                        .AddChoices(new[] {
                                "1. Leave a vehicle for parking",
                                "2. Change a vehicle's parkingspot",
                                "3. Get your vehicle",
                                "4. Search for a vehicle"
                }));

                Console.WriteLine("");


                // Echo the fruit back to the terminal
                char charFirst = menuItem[0];
                //Console.WriteLine("You have chosen: " + charFirst);

                switch (charFirst)
                {
                    case '1':
                        //Leaving vehicle to the garage
                        Console.WriteLine("");
                        Console.WriteLine("You chose to leave your vehicle for parking");
                        Console.WriteLine("");
                        string regNr = "empty";
                        string type = "empty";


                        //checks if registration nr is valid
                        bool isValidRegNr = false;


                        while (!isValidRegNr)
                        {
                            string strRegNr = AnsiConsole.Ask<string>("[paleturquoise1]Please enter your vehicle's registration number: [/]");
                            bool isRegnrValid = IsInputRegnrValid(strRegNr);
                            if (isRegnrValid)
                            {
                                regNr = strRegNr;
                                isValidRegNr = true;
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration number is not valid");
                                Console.WriteLine("");
                            }

                        }


                        Console.WriteLine("");
                        Console.WriteLine("Your reg number is: " + regNr);
                        Console.WriteLine("");



                        //Getting vehicle's type
                        type = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("[bold paleturquoise1]Please choose to change your vehicle's parkingspot?[/]")
                        .PageSize(3)
                        .AddChoices(new[] {
                                "Car",
                                "MC"
                        }));

                        Console.WriteLine("");
                        Console.WriteLine("You have chosen: " + type);
                        Console.WriteLine("");
                        AddVehicle(regNr, type);

                        break;
                    case '2':
                        Console.WriteLine("You chose to leave your vehicle for parking");

                        string regNrInput = "empty";
                        int parkingNrInput = 0;
                        int position = -1;

                        //checks if registration nr is valid
                        bool isValidregNrInput = false;


                        while (!isValidregNrInput)
                        {
                            string strRegNrInput = AnsiConsole.Ask<string>("[paleturquoise1]Please enter your vehicle's registration number: [/]");
                            bool isRegnrValid = IsInputRegnrValid(strRegNrInput);
                            if (isRegnrValid)
                            {
                                position = IsRegnrAvailable(strRegNrInput);
                                if (position != -1)
                                {
                                    regNrInput = strRegNrInput;
                                    isValidregNrInput = true;
                                }
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration number is not valid");
                                Console.WriteLine("");
                            }

                        }


                        //checks if new parking nr is valid
                        bool isValidParkingNrInput = false;

                        while (!isValidParkingNrInput)
                        {
                            Console.WriteLine("");
                            string strNr = AnsiConsole.Ask<string>("[paleturquoise1]Please enter parkingspot number where you want to move your vehicle: [/]");
                            int intNr = IsNewParkingNrValid(strNr);

                            if (intNr != -1)
                            {
                                parkingNrInput = intNr;
                                isValidParkingNrInput = true;
                            }
                        }//end of while



                        Console.WriteLine("");
                        Console.WriteLine("Your reg number is: " + regNrInput);
                        Console.WriteLine("Position of your vehicle is: " + position);
                        Console.WriteLine("Your new ps nummer is valid: " + parkingNrInput);
                        Console.WriteLine("");


                        ChangeParkingSpot(position, parkingNrInput, regNrInput);


                        break;
                    case '3':
                        Console.WriteLine("3. Get your vehicle");
                        break;
                    case '4':
                        Console.WriteLine("4. Search for a vehicle");
                        break;
                    default:
                        Console.WriteLine("Please choose from the menu");
                        break;
                }



            }//end of while menu



        }//end of main


        public static ParkingSpot[] CreateParkingSpotsList()
        {
            string sAttr = ConfigurationManager.AppSettings.Get("NrOfPSpots");
            int garageSize = Int32.Parse(sAttr);
            ParkingSpot[] parkingSpotsArray = new ParkingSpot[garageSize];

            //create a array of parkingspots
            for (int i = 0; i < garageSize; i++)
            {
                ParkingSpot ps = new ParkingSpot(i+1);
                parkingSpotsArray[i] = ps;
            }
            /*
            foreach (ParkingSpot ps in parkingSpotsArray)
            {
                Console.WriteLine("array: " + ps.parkingSpotNr);
            }*/

            SaveArrayInJsonFile(parkingSpotsArray);
            return parkingSpotsArray;
        }

        public static void SaveArrayInJsonFile(ParkingSpot[] parkingSpots)
        {
            string jsonString = JsonConvert.SerializeObject(parkingSpots);
            //Console.WriteLine("array: " + jsonString);
            //getting relative directory of the project folder
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\garage.json"));
            File.WriteAllText(path, jsonString);
            //Console.WriteLine("Array is saved in json file");
        }

        public static List<ParkingSpot> GetParkingSpotsList()
        {
            //getting relative directory of the json file
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\garage.json"));

            List<ParkingSpot> parkingSpotsList;

            //check if file exist before reading from it
            if (!File.Exists(path))
            {
                Console.WriteLine("file doesn't exist");
                ParkingSpot[] parkingSpotsArray = CreateParkingSpotsList();
                parkingSpotsList = parkingSpotsArray.ToList();
            }
            else
            {
                //reading json file and saving the content in the string variable
                var jsonString = File.ReadAllText(path);
                parkingSpotsList = JsonConvert.DeserializeObject<List<ParkingSpot>>(jsonString);
                Console.WriteLine("File exist");
            }

            
            return parkingSpotsList;
        }

       
        public static void PrintParkingSpots(){
            //gets list of ParkingSpots from JSON file
            List<ParkingSpot> parkingSpotsList = GetParkingSpotsList();
            int listSize = parkingSpotsList.Count;
            Console.WriteLine();
            Console.WriteLine();
            //Console.WriteLine("Size of the list: " + listSize);

            //Creating table for parkingSpots
            string sAttr = ConfigurationManager.AppSettings.Get("NrOfPSpots");
            int garageSize = Int32.Parse(sAttr);
            Console.WriteLine("Garage size is: " + garageSize);
            Console.WriteLine("Parkingspots with red numbers are taken");
            Console.WriteLine("Parkingspots with yellow numbers have a place for 1 MC");

            int nrOfColumn = 0;
            int nrOfRow = 0;

            if (garageSize >= 10)
            {
                nrOfRow = garageSize / 10;
                nrOfColumn = 10;
            } else
            {
                nrOfColumn = garageSize;
                nrOfRow = 1;
            }

            //Console.WriteLine("columns: " + nrOfColumn);
            //Console.WriteLine("rows: " + nrOfRow);


            //Printing table using Spectre.Console
            Console.WriteLine();

            for (int i = 0; i < nrOfRow; i++)
            {
                var tableNew = new Table();
                tableNew.Border = TableBorder.HeavyEdge;
                tableNew.BorderColor(new Color(0, 95, 255));
                tableNew.Centered();

                for (int it = 0; it < nrOfColumn; it++)
                {

                    int position = i * 10 + it;
                    //Console.WriteLine("position of cells: " + position);
                    //

                    if (position >= parkingSpotsList.Count)
                    {
                        //Console.WriteLine("index is outside list");
                        //Console.WriteLine("ps nr: " + parkingSpotsList.ElementAt(position).parkingSpotNr);
                        ParkingSpot ps = new ParkingSpot(position+1);
                        parkingSpotsList.Add(ps);
                        //Console.WriteLine("ps nr: " + parkingSpotsList.ElementAt(position).parkingSpotNr);
                        tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).regNr}[/]").Centered());
                    } else
                    {
                        //Console.WriteLine("index is inside list");
                        //Console.WriteLine("ps nr: " + parkingSpotsList.ElementAt(position).parkingSpotNr);
                        string status = parkingSpotsList.ElementAt(position).status;
                        switch (status)
                        {
                            case "taken":
                                tableNew.AddColumn(new TableColumn(new Markup($"[red bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).regNr}[/]").Centered());
                                break;
                            case "halffull":
                                tableNew.AddColumn(new TableColumn(new Markup($"[yellow1 bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).regNr}[/]").Centered());
                                break;
                            default:
                                tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).regNr}[/]").Centered());
                                break;
                        }

                    }//end of if

                }//end of inner for
                tableNew.Expand();
                AnsiConsole.Render(tableNew);
            }//end of outer for


            //Printing last row which is less than 10
            int lastRowNrOfCol = garageSize % 10;

            //If there is a row left, then print
            if (lastRowNrOfCol >= 1)
            {
                var tableNew = new Table();
                tableNew.Border = TableBorder.HeavyEdge;
                tableNew.BorderColor(new Color(0, 95, 255));
                tableNew.Alignment(Justify.Left);

                for (int it = 0; it < lastRowNrOfCol; it++)
                {
                    int position = nrOfRow * 10 + it;
                    //Console.WriteLine("index of last row: " + position);
                    if (position >= parkingSpotsList.Count)
                    {
                        //Console.WriteLine("index is outside list");
                        //Console.WriteLine("ps nr: " + parkingSpotsList.ElementAt(position).parkingSpotNr);
                        ParkingSpot ps = new ParkingSpot(position + 1);
                        parkingSpotsList.Add(ps);
                        //Console.WriteLine("ps nr: " + parkingSpotsList.ElementAt(position).parkingSpotNr);
                        tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).regNr}[/]").Centered());
                    }
                    else
                    {
                        //Console.WriteLine("index is inside list");
                        //Console.WriteLine("ps nr: " + parkingSpotsList.ElementAt(position).parkingSpotNr);
                        string status = parkingSpotsList.ElementAt(position).status;
                        switch (status)
                        {
                            case "taken":
                                tableNew.AddColumn(new TableColumn(new Markup($"[red bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).regNr}[/]").Centered());
                                break;
                            case "halffull":
                                tableNew.AddColumn(new TableColumn(new Markup($"[yellow1 bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).regNr}[/]").Centered());
                                break;
                            default:
                                tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).regNr}[/]").Centered());
                                break;
                        }

                    }//end of if
                }//end of for


                tableNew.Collapse();
                AnsiConsole.Render(tableNew);
            }


            //Save ParkingSpotsList into a json file
            ParkingSpot[] parkingSpotArray = parkingSpotsList.ToArray();
            SaveArrayInJsonFile(parkingSpotArray);
            /*
            foreach (ParkingSpot ps in parkingSpotsList)
            {
                Console.WriteLine("List at the end: " + ps.parkingSpotNr);
            }*/


        }//end of PrintParkingSpots

        //checks if input for regnr is valid
        public static bool IsInputRegnrValid(string regnr)
        {
            bool isStrEmpty = String.IsNullOrEmpty(regnr);
            if (!isStrEmpty)
            {
                string trimmed = regnr.Trim(); // Ignore white space on either side.
                //convert to lower case
                if (trimmed.Length <= 10)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Registration number is too long. It should be no longer than 10 characters");
                    Console.WriteLine("");
                    return false;
                }//end of inner if

            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Please enter a valid registration number");
                Console.WriteLine("");
                return false; //input is empty string
            }//end of outer if

        }//end of IsInputTypeValid method

        //add vehicle to parking
        public static void AddVehicle(string regNr, string type)
        {
            //gets list of ParkingSpots from JSON file
            List<ParkingSpot> parkingSpotsList = GetParkingSpotsList();
            ParkingSpot[] parkingSpotsArray = parkingSpotsList.ToArray();
            string regNrTrimmed = regNr; // Ignore white space on either side.
                                            //convert to lower case
            string lowerRegNr = regNrTrimmed.ToLower();

            if (type == "MC")
            {
                //Console.WriteLine("MC is gonna saved");
                bool mcSaved = false;
                
                //try to find parking spots where 1 mc is saved
                for (int i = 0; i < parkingSpotsArray.Length; i++)
                {
                    if (parkingSpotsArray[i].status.Equals("halffull"))
                    {
                        string joinedRegNr = parkingSpotsArray[i].regNr;
                        joinedRegNr += "," + lowerRegNr;
                        parkingSpotsArray[i].regNr = joinedRegNr;
                        parkingSpotsArray[i].status = "taken";
                        parkingSpotsArray[i].nrOfVehicle = 2;
                        DateTime localDate = DateTime.Now;
                        string joinedDateTime = parkingSpotsArray[i].dateCheckedIn;
                        joinedDateTime += "," + localDate.ToString();
                        parkingSpotsArray[i].dateCheckedIn = joinedDateTime;

                        mcSaved = true;
                        Console.WriteLine("found halffull ps, mc is saved in the parking spot nr: " + parkingSpotsArray[i].parkingSpotNr);
                        break;
                    }
                }//end of for

                ////try to find empty parking spots
                if (!mcSaved)
                {
                    for (int i = 0; i < parkingSpotsArray.Length; i++)
                    {
                        if (parkingSpotsArray[i].status.Equals("empty"))
                        {
                            parkingSpotsArray[i].regNr = lowerRegNr;
                            parkingSpotsArray[i].status = "halffull";
                            parkingSpotsArray[i].nrOfVehicle = 1;
                            DateTime localDate = DateTime.Now;
                            string date_str = localDate.ToString();
                            parkingSpotsArray[i].dateCheckedIn = date_str;
                            parkingSpotsArray[i].typeOfVehicle = "mc";

                            mcSaved = true;
                            Console.WriteLine("found empty ps, mc is saved in the parking spot: " + parkingSpotsArray[i].parkingSpotNr);
                            break;
                        }
                    }//end of for
                }//end of if

                if (mcSaved)
                {
                    SaveArrayInJsonFile(parkingSpotsArray);
                    PrintParkingSpots();

                } else
                {
                    Console.WriteLine("MC is not saved");
                }

                
            } else
            {
                //Console.WriteLine("Car is gonna saved");

                bool carSaved = false;
                ////try to find empty parking spots
                if (!carSaved)
                {
                    for (int i = 0; i < parkingSpotsArray.Length; i++)
                    {
                        if (parkingSpotsArray[i].status.Equals("empty"))
                        {
                            parkingSpotsArray[i].regNr = lowerRegNr;
                            parkingSpotsArray[i].status = "taken";
                            parkingSpotsArray[i].nrOfVehicle = 1;
                            DateTime localDate = DateTime.Now;
                            string date_str = localDate.ToString();
                            parkingSpotsArray[i].dateCheckedIn = date_str;
                            parkingSpotsArray[i].typeOfVehicle = "car";

                            carSaved = true;
                            Console.WriteLine("found empty ps, car is saved in the parking spot: " + parkingSpotsArray[i].parkingSpotNr);
                            break;
                        }
                    }//end of for
                }//end of if

                if (carSaved)
                {
                    SaveArrayInJsonFile(parkingSpotsArray);
                    PrintParkingSpots();

                }
                else
                {
                    Console.WriteLine("Car is not saved");
                }//end of inner if else

            }//end of outer if else


        }//end of AddVehicle method

        //changes vehicle's parking spot by reg nr
        public static void ChangeParkingSpot(int position, int newNr, string regnr)
        {
            Console.WriteLine("parking spot will be changed");
            /*

            string regNr = ParkingSpots.parkingSpotsArray[oldNr].RegNr;
            string type = ParkingSpots.parkingSpotsArray[oldNr].VehicleType;
            int nrOfVehicles = ParkingSpots.parkingSpotsArray[oldNr].NrOfVehicle;

            if (nrOfVehicles == 2)
            {
                //when there are 2 vehicles in the parking spot
                int pos = regNr.IndexOf(regnr); //gets position of the regnr
                string newStr = regNr.Remove(pos, regnr.Length); //removes the regnr from the string
                string remainingRegnr = newStr.Trim(new Char[] { ' ', ',' }); //removes komma och white space from remaining reg

                //moves the vehicle into new parking spot
                ParkingSpots.parkingSpotsArray[newNr].RegNr = regnr;
                ParkingSpots.parkingSpotsArray[newNr].VehicleType = type;
                ParkingSpots.parkingSpotsArray[newNr].NrOfVehicle = 1;

                //free the old parking spot
                ParkingSpots.parkingSpotsArray[oldNr].RegNr = remainingRegnr;
                ParkingSpots.parkingSpotsArray[oldNr].VehicleType = type;
                ParkingSpots.parkingSpotsArray[oldNr].NrOfVehicle = 1;

            }
            else
            {

                //when there is only 1 vehicle in the parking spot
                ParkingSpots.parkingSpotsArray[newNr].RegNr = regNr;
                ParkingSpots.parkingSpotsArray[newNr].VehicleType = type;
                ParkingSpots.parkingSpotsArray[newNr].NrOfVehicle = nrOfVehicles;

                //free the old parking spot
                ParkingSpots.parkingSpotsArray[oldNr].RegNr = "empty";
                ParkingSpots.parkingSpotsArray[oldNr].VehicleType = "empty";
                ParkingSpots.parkingSpotsArray[oldNr].NrOfVehicle = 0;
            }





            Console.WriteLine("Your vehicle is moved into new parking spot");
            Console.WriteLine("Your vehicle's parking spot number is: " + newNr);

            */
        }//end of ChangeParkingSpot method


        //checks if input parking nr is valid
        public static int IsRegnrAvailable(string regnr)
        {

            //gets list of ParkingSpots from JSON file
            List<ParkingSpot> parkingSpotsList = GetParkingSpotsList();
            ParkingSpot[] parkingSpotsArray = parkingSpotsList.ToArray();
            string regNrTrimmed = regnr.Trim(); // Ignore white space on either side.
                                         //convert to lower case
            string lowerRegNr = regNrTrimmed.ToLower();

            int position = -1;
            bool isFound = false;

            //Search for vehicle by regnr
            for (int i = 0; i < parkingSpotsArray.Length; i++)
            {
                if (parkingSpotsArray[i].regNr.Contains(lowerRegNr, StringComparison.OrdinalIgnoreCase))
                {

                    isFound = true;
                    position = i;
                    break;

                }
            }//end of for

            if (!isFound)
            {
                Console.WriteLine("Entered registration number is not found");
                return -1;
            }
            else
            {
                return position;
            }

        }//end of IsRegnrAvailable method

        //checks if input parking nr is valid number
        public static int IsNewParkingNrValid(string raw)
        {
            string s = raw.Trim(); // Ignore white space on either side.
            int number = Convert.ToInt32(s);

            string sAttr = ConfigurationManager.AppSettings.Get("NrOfPSpots");
            int garageSize = Int32.Parse(sAttr);


            if (number <= garageSize) //checks nr is not more than garage size
            {
                return number;
            }
            else
            {
                Console.WriteLine("Your parking slot number is not valid. Please enter number between 0-99");
                return -1;
            }

        }//end of IsNewParkingNrValid method


    }//end of class program 
}//end of namespace
