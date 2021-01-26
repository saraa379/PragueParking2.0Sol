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
                        string type = "empty";
                        type = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("[bold paleturquoise1]Please choose type of your vehicle?[/]")
                        .PageSize(3)
                        .AddChoices(new[] {
                                "Car",
                                "MC"
                        }));

                        Console.WriteLine("");
                        Console.WriteLine("You have chosen: " + type);
                        Console.WriteLine("");

                        Vehicle vh = new Vehicle();

                        if (type == "MC")
                        {
                            MC motorcycle = new MC(regNr);
                            vh = motorcycle;
                        } else
                        {
                            Car car = new Car(regNr);
                            vh = car;
                        }

                        AddVehicle(vh);

                        break;
                    case '2':
                        Console.WriteLine("You chose to change parkingspot for your vehicle");

                        string regNrInput = "empty";
                        int parkingNrInput = 0;
                        int position = -1;

                        //checks if registration nr is valid
                        bool isValidregNrInput = false;


                        while (!isValidregNrInput)
                        {
                            string strRegNrInput = AnsiConsole.Ask<string>("[paleturquoise1]Please enter your vehicle's registration number: [/]");
                            bool isRegnrValid = IsInputRegnrValid(strRegNrInput);
                            position = IsRegnrAvailable(strRegNrInput);
                            if (isRegnrValid && position != -1)
                            {
                                    regNrInput = strRegNrInput;
                                    isValidregNrInput = true;
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration number is not found in the garage");
                                Console.WriteLine("");
                            }

                        }


                        //checks if new parking nr is valid
                        bool isValidParkingNrInput = false;

                        while (!isValidParkingNrInput)
                        {
                            Console.WriteLine("");
                            string strNr = AnsiConsole.Ask<string>("[paleturquoise1]Please enter parkingspot number where you want to move your vehicle: [/]");
                            int intNr = IsNewParkingNrValid(strNr, position); //returns position of a new parkingspot

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
                        Console.WriteLine("");
                        Console.WriteLine("3. You chose to get your vehicle");
                        Console.WriteLine("");

                        string regNrInputG = "empty";
                        int positionG = -1;

                        //checks if registration nr is valid
                        bool isValidregNrInputG = false;

                        while (!isValidregNrInputG)
                        {
                            string strRegNrInputG = AnsiConsole.Ask<string>("[paleturquoise1]Please enter your vehicle's registration number: [/]");
                            bool isRegnrValid = IsInputRegnrValid(strRegNrInputG);
                            positionG = IsRegnrAvailable(strRegNrInputG);
                            if (isRegnrValid && positionG != -1)
                            {
                                regNrInputG = strRegNrInputG;
                                isValidregNrInputG = true;
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration number is not found in the garage");
                                Console.WriteLine("");
                            }

                        }

                        //Console.WriteLine("Reg nr available. You can get your car" + regNrInputG);
                        RemoveVehicle(regNrInputG, positionG);

                        break;
                    case '4':
                        Console.WriteLine("");
                        Console.WriteLine("4. You chose to search your vehicle");
                        Console.WriteLine("");


                        //checks if registration nr is valid
                        bool isValidregNrInputS = false;

                        while (!isValidregNrInputS)
                        {
                            string strRegNrInputS = AnsiConsole.Ask<string>("[paleturquoise1]Please enter your vehicle's registration number: [/]");
                            bool isRegnrValidS = IsInputRegnrValid(strRegNrInputS);
                            int positionS = IsRegnrAvailable(strRegNrInputS);
                            if (isRegnrValidS && positionS != -1)
                            {
                                //regNrInputS = strRegNrInputS;
                                isValidregNrInputS = true;
                                int psNr = positionS + 1;
                                Console.WriteLine("Your vehicle with registration number " + strRegNrInputS + " is in the parkingspot " + psNr);
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration number is not found in the garage");
                                Console.WriteLine("");
                            }

                        }

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
                        tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).vh.RegNr}[/]").Centered());
                    } else
                    {
                        //Console.WriteLine("index is inside list");
                        //Console.WriteLine("ps nr: " + parkingSpotsList.ElementAt(position).parkingSpotNr);
                        string status = parkingSpotsList.ElementAt(position).status;
                        switch (status)
                        {
                            case "taken":
                                tableNew.AddColumn(new TableColumn(new Markup($"[red bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).vh.RegNr}[/]").Centered());
                                break;
                            case "halffull":
                                tableNew.AddColumn(new TableColumn(new Markup($"[yellow1 bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).vh.RegNr}[/]").Centered());
                                break;
                            default:
                                tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).vh.RegNr}[/]").Centered());
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
                        tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).vh.RegNr}[/]").Centered());
                    }
                    else
                    {
                        //Console.WriteLine("index is inside list");
                        //Console.WriteLine("ps nr: " + parkingSpotsList.ElementAt(position).parkingSpotNr);
                        string status = parkingSpotsList.ElementAt(position).status;
                        switch (status)
                        {
                            case "taken":
                                tableNew.AddColumn(new TableColumn(new Markup($"[red bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).vh.RegNr}[/]").Centered());
                                break;
                            case "halffull":
                                tableNew.AddColumn(new TableColumn(new Markup($"[yellow1 bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).vh.RegNr}[/]").Centered());
                                break;
                            default:
                                tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(position).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(position).vh.RegNr}[/]").Centered());
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
        public static void AddVehicle(Vehicle vh)
        {
            //gets list of ParkingSpots from JSON file
            List<ParkingSpot> parkingSpotsList = GetParkingSpotsList();
            ParkingSpot[] parkingSpotsArray = parkingSpotsList.ToArray();
            string regNrTrimmed = vh.RegNr; // Ignore white space on either side.
                                            //convert to lower case
            string lowerRegNr = regNrTrimmed.ToLower();

            if (vh.Type == "mc")
            {
                //Console.WriteLine("MC is gonna saved");
                string sAttr = ConfigurationManager.AppSettings.Get("NrOfMCPerPSpot");
                int mcSize = Int32.Parse(sAttr);
                
                bool mcSaved = false;
                
                //try to find parking spots where 1 mc is saved
                for (int i = 0; i < parkingSpotsArray.Length; i++)
                {
                    if (parkingSpotsArray[i].status.Equals("halffull"))
                    {
                        int nrOfVehicle = parkingSpotsArray[i].nrOfVehicle;
                        string joinedRegNr = parkingSpotsArray[i].vh.RegNr;
                        joinedRegNr += "," + lowerRegNr;
                        parkingSpotsArray[i].vh.RegNr = joinedRegNr;
                        parkingSpotsArray[i].nrOfVehicle = nrOfVehicle + 1;
                        //status
                        if (parkingSpotsArray[i].nrOfVehicle == mcSize)
                        {
                            parkingSpotsArray[i].status = "taken";
                        } else
                        {
                            parkingSpotsArray[i].status = "halffull";
                        }
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
                            parkingSpotsArray[i].vh.RegNr = lowerRegNr;
                            parkingSpotsArray[i].nrOfVehicle = 1;
                            //status
                            if (parkingSpotsArray[i].nrOfVehicle >= mcSize)
                            {
                                parkingSpotsArray[i].status = "taken";
                            }
                            else
                            {
                                parkingSpotsArray[i].status = "halffull";
                            }
                            DateTime localDate = DateTime.Now;
                            string date_str = localDate.ToString();
                            parkingSpotsArray[i].dateCheckedIn = date_str;
                            parkingSpotsArray[i].vh.Type = "mc";
                            parkingSpotsArray[i].vh.Size = mcSize;

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

                string sAttrCar = ConfigurationManager.AppSettings.Get("NrOfCarPerPSpot");
                int carSize = Int32.Parse(sAttrCar);
                ////try to find empty parking spots
                if (!carSaved)
                {
                    for (int i = 0; i < parkingSpotsArray.Length; i++)
                    {
                        if (parkingSpotsArray[i].status.Equals("empty"))
                        {
                            parkingSpotsArray[i].vh.RegNr = lowerRegNr;
                            parkingSpotsArray[i].status = "taken";
                            parkingSpotsArray[i].nrOfVehicle = 1;
                            DateTime localDate = DateTime.Now;
                            string date_str = localDate.ToString();
                            parkingSpotsArray[i].dateCheckedIn = date_str;
                            parkingSpotsArray[i].vh.Type = "car";
                            parkingSpotsArray[i].vh.Size = carSize;

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
        public static void ChangeParkingSpot(int oldPosition, int newPosition, string regnr)
        {
            //Console.WriteLine("parking spot will be changed");
            //gets list of ParkingSpots from JSON file
            List<ParkingSpot> parkingSpotsList = GetParkingSpotsList();
            ParkingSpot[] parkingSpotsArray = parkingSpotsList.ToArray();
            //Console.WriteLine("MC is gonna saved");
            string sAttr = ConfigurationManager.AppSettings.Get("NrOfMCPerPSpot");
            int mcSize = Int32.Parse(sAttr);

            if (parkingSpotsArray[oldPosition].nrOfVehicle == 2) //when there are 2 vehicles in the parking spot
            {
                //separating regnr from joined regnr
                string joinedRegNr = parkingSpotsArray[oldPosition].vh.RegNr;
                int pos = joinedRegNr.IndexOf(regnr); //gets position of the regnr
                string newStr = joinedRegNr.Remove(pos, regnr.Length); //removes the regnr from the string
                string remainingRegnr = newStr.Trim(new Char[] { ' ', ',' }); //removes komma och white space from remaining reg

                Console.WriteLine("position of the regnr in the joined regnr" + pos); //0

                //separating CheckedInDate from joined dates
                string joinedDate = parkingSpotsArray[oldPosition].dateCheckedIn;
                string[] dates = joinedDate.Split(',');
                //Console.WriteLine("First date: " + dates[0]);
                //Console.WriteLine("Second date: " + dates[1]);
                string dateTobeMoved;
                string dateToStay;

                if (pos == 0) //first date belongs to the vehicle to be removed
                {
                    dateTobeMoved = dates[0];
                    dateToStay = dates[1];
                } else
                {
                    dateTobeMoved = dates[1];
                    dateToStay = dates[0];
                }

                //moves the vehicle into new parking spot
                
                if (parkingSpotsArray[newPosition].nrOfVehicle >= 1 && parkingSpotsArray[newPosition].nrOfVehicle < mcSize) //there is mc in the new ps
                {
                    parkingSpotsArray[newPosition].vh.RegNr += "," + regnr;
                    int nrOfMC = parkingSpotsArray[newPosition].nrOfVehicle;
                    parkingSpotsArray[newPosition].nrOfVehicle = nrOfMC + 1;
                    if (parkingSpotsArray[newPosition].nrOfVehicle == mcSize)
                    {
                        parkingSpotsArray[newPosition].status = "taken";
                    } else
                    {
                        parkingSpotsArray[newPosition].status = "halffull";
                    }
                    parkingSpotsArray[newPosition].dateCheckedIn += "," + dateTobeMoved;
                 
                } else //new ps is empty
                {
                    MC mc = new MC(regnr);
                    parkingSpotsArray[newPosition].vh = mc;
                    if (parkingSpotsArray[newPosition].nrOfVehicle >= mcSize)
                    {
                        parkingSpotsArray[newPosition].status = "taken";
                    }
                    else
                    {
                        parkingSpotsArray[newPosition].status = "halffull";
                    }
                    parkingSpotsArray[newPosition].dateCheckedIn = dateTobeMoved;
                    parkingSpotsArray[newPosition].nrOfVehicle = 1;
                }


                //free the old parking spot

                if (parkingSpotsArray[oldPosition].nrOfVehicle == 2) //there are 2 mc in the old ps
                {
                    parkingSpotsArray[oldPosition].vh.RegNr = remainingRegnr;
                    parkingSpotsArray[oldPosition].status = "halffull";
                    parkingSpotsArray[oldPosition].dateCheckedIn = dateToStay;
                    parkingSpotsArray[oldPosition].nrOfVehicle = 1;
                }
                else //old ps shall be empty
                {
                    parkingSpotsArray[oldPosition].vh.RegNr = "empty";
                    parkingSpotsArray[oldPosition].vh.Type = "empty";
                    parkingSpotsArray[oldPosition].status = "empty";
                    parkingSpotsArray[oldPosition].dateCheckedIn = "empty";
                    parkingSpotsArray[oldPosition].nrOfVehicle = 0;
                    parkingSpotsArray[oldPosition].vh.Size = 0;
                }




            }
            else //when there is only 1 car need to be moved
            {
                //moves the vehicle into new parking spot
                Car car = new Car(regnr);
                parkingSpotsArray[newPosition].vh = car;
                parkingSpotsArray[newPosition].status = "taken";
                parkingSpotsArray[newPosition].dateCheckedIn = parkingSpotsArray[oldPosition].dateCheckedIn;
                parkingSpotsArray[newPosition].nrOfVehicle = 1;

                //free the old parking spot
                parkingSpotsArray[oldPosition].vh.RegNr = "empty";
                parkingSpotsArray[oldPosition].vh.Type = "empty";
                parkingSpotsArray[oldPosition].status = "empty";
                parkingSpotsArray[oldPosition].dateCheckedIn = "empty";
                parkingSpotsArray[oldPosition].nrOfVehicle = 0;
                parkingSpotsArray[oldPosition].vh.Size = 0;

            }


            Console.WriteLine("Your vehicle is moved into the new parkingspot");

            SaveArrayInJsonFile(parkingSpotsArray);
            PrintParkingSpots();
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
                if (parkingSpotsArray[i].vh.RegNr.Contains(lowerRegNr, StringComparison.OrdinalIgnoreCase))
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
                Console.WriteLine("Reg number is available in the system");
                return position;
            }

        }//end of IsRegnrAvailable method

        //checks if input parking nr is valid number
        public static int IsNewParkingNrValid(string positionNew, int positionOld)
        {
            //gets list of ParkingSpots from JSON file
            List<ParkingSpot> parkingSpotsList = GetParkingSpotsList();
            ParkingSpot[] parkingSpotsArray = parkingSpotsList.ToArray();

            string s = positionNew.Trim(); // Ignore white space on either side.
            int number = Convert.ToInt32(s);

            string sAttr = ConfigurationManager.AppSettings.Get("NrOfPSpots");
            int garageSize = Int32.Parse(sAttr);

            //finding out type of the vehicle that gonna move
            string type = parkingSpotsArray[positionOld].vh.Type;
            Console.WriteLine("Type of a vehicle that gonna move is: " + type);


            if (number < garageSize) //checks nr is not more than garage size
            {
                if (type == "car" && String.Equals(parkingSpotsArray[number - 1].status, "empty"))
                {
                    return number - 1;
                } else if (type == "mc" && String.Equals(parkingSpotsArray[number - 1].status, "empty")){
                    return number - 1;
                } else if (type == "mc" && String.Equals(parkingSpotsArray[number - 1].status, "halffull"))
                {
                    return number - 1;
                } else
                {
                    Console.WriteLine("The new parkingspot is already taken");
                    return - 1;
                }
                
            }
            else
            {
                Console.WriteLine("Your parking slot number is not valid. Please enter number between 0-99");
                return -1;
            }

        }//end of IsNewParkingNrValid method

        public static void RemoveVehicle(string regNr, int position)
        {
            //Console.WriteLine("Your vehicle will be removed: " + regNr + ", " + position);
            //gets list of ParkingSpots from JSON file
            List<ParkingSpot> parkingSpotsList = GetParkingSpotsList();
            ParkingSpot[] parkingSpotsArray = parkingSpotsList.ToArray();


            if (parkingSpotsArray[position].nrOfVehicle == 2) //remove mc
            {
                Console.WriteLine("mc will be removed");

                //separating regnr from joined regnr
                string joinedRegNr = parkingSpotsArray[position].vh.RegNr;
                int pos = joinedRegNr.IndexOf(regNr); //gets position of the regnr
                string newStr = joinedRegNr.Remove(pos, regNr.Length); //removes the regnr from the string
                string remainingRegnr = newStr.Trim(new Char[] { ' ', ',' }); //removes komma och white space from remaining reg

                //Console.WriteLine("position of the regnr in the joined regnr" + pos); //0

                //separating CheckedInDate from joined dates
                string joinedDate = parkingSpotsArray[position].dateCheckedIn;
                string[] dates = joinedDate.Split(',');
                //Console.WriteLine("First date: " + dates[0]);
                //Console.WriteLine("Second date: " + dates[1]);
                string dateTobeMoved;
                string dateToStay;

                if (pos == 0) //first date belongs to the vehicle to be removed
                {
                    dateTobeMoved = dates[0];
                    dateToStay = dates[1];
                }
                else
                {
                    dateTobeMoved = dates[1];
                    dateToStay = dates[0];
                }

                int mcFee = GetParkingCharge(dateTobeMoved, "mc");

                //remove vehicle from parkingspot
                parkingSpotsArray[position].vh.RegNr = remainingRegnr;
                parkingSpotsArray[position].status = "halffull";
                parkingSpotsArray[position].dateCheckedIn = dateToStay;
                parkingSpotsArray[position].nrOfVehicle = 1;

                Console.WriteLine("Your mc is removed");

            } else {

                int carFee = GetParkingCharge(parkingSpotsArray[position].dateCheckedIn, "car");

                //free the old parking spot
                Vehicle vh = new Vehicle();
                parkingSpotsArray[position].vh = vh;
                parkingSpotsArray[position].status = "empty";
                parkingSpotsArray[position].dateCheckedIn = "empty";
                parkingSpotsArray[position].nrOfVehicle = 0;

            }

            SaveArrayInJsonFile(parkingSpotsArray);
            PrintParkingSpots();

        }//end of RemoveVehicle

        public static int GetParkingCharge(string dateCheckedIn, string type)
        {

            DateTime dateIn = DateTime.Parse(dateCheckedIn);
            DateTime dateStart = dateIn.AddMinutes(10); //first 10 minutes are free
            DateTime dateOut = DateTime.Now;
            Console.WriteLine("chekced in date: " + dateStart);
            Console.WriteLine("chekced out date: " + dateOut);

            TimeSpan timeDiff = dateOut - dateStart;
            int elapsedTime = Convert.ToInt32(timeDiff.TotalHours);
            elapsedTime = elapsedTime + 1;
            //Console.WriteLine("Elapsed time in hours: " + elapsedTime);

            if (String.Equals(type, "mc"))
            {
                //Getting fee information from config file
                string mcFeeStr = ConfigurationManager.AppSettings.Get("MCPrice");
                int mcFee = Int32.Parse(mcFeeStr);
                int totalFee = elapsedTime * mcFee;

                Console.WriteLine("");
                Console.WriteLine("Total elapsed time including started time: " + elapsedTime);
                Console.WriteLine("Your fee is: " + totalFee);
                Console.WriteLine("");

                return totalFee;

            } else
            {
                //Getting fee information from config file
                string mcFeeStr = ConfigurationManager.AppSettings.Get("CarPrice");
                int carFee = Int32.Parse(mcFeeStr);
                int totalFeeCar = elapsedTime * carFee;

                Console.WriteLine("");
                Console.WriteLine("Total elapsed time including started time: " + elapsedTime);
                Console.WriteLine("Your fee is: " + totalFeeCar);
                Console.WriteLine("");

                return totalFeeCar;
            }
        }//end of GetParkingCharge


    }//end of class program 
}//end of namespace
