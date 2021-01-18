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
            



        }//end of main


        public static ParkingSpot[] GetParkingSpotsArray()
        {
            //getting relative directory of the json file
            string directory = AppDomain.CurrentDomain.BaseDirectory + "//garage.json";

            //reading json file and saving the content in the string variable
            var jsonString = File.ReadAllText(directory);
            //Console.WriteLine("current directory" + jsonString);

            //deserialising a json string into C# object
            ParkingSpot[] parkingSpotsArray = JsonConvert.DeserializeObject<ParkingSpot[]>(jsonString);
            return parkingSpotsArray;
        }

        public static List<ParkingSpot> GetParkingSpotsList()
        {
            //getting relative directory of the json file
            string directory = AppDomain.CurrentDomain.BaseDirectory + "//garage.json";

            //reading json file and saving the content in the string variable
            var jsonString = File.ReadAllText(directory);
            //Console.WriteLine("current directory" + jsonString);

            //deserialising a json string into C# object
            List<ParkingSpot> parkingSpotsList = JsonConvert.DeserializeObject<List<ParkingSpot>>(jsonString);
            return parkingSpotsList;
        }

        public static void SaveListInJsonFile(List<ParkingSpot> parkingSpots)
        {
            ParkingSpot[] parkingSpotArray = parkingSpots.ToArray();
            var jsonString = JsonConvert.SerializeObject(parkingSpotArray);

            //getting relative directory of the json file
            string directory = AppDomain.CurrentDomain.BaseDirectory + "//garage.json";

            //Console.WriteLine(jsonString);

            if (!File.Exists(directory))
            {
                Console.WriteLine("file doesn't exist");
            } else
            {
                Console.WriteLine("file exist");
                //writing json string into json file, replacing the old file
                File.WriteAllText(directory, jsonString);
            }
        }

        public static void PrintParkingSpots(){
            //gets list of ParkingSpots from JSON file
            List<ParkingSpot> parkingSpotsList = GetParkingSpotsList();
            int listSize = parkingSpotsList.Count;
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


                    int index = i * 10 + it + 1;
                    //Console.WriteLine("index of cells: " + index);

                    if (index >= parkingSpotsList.Count)
                    {
                        ParkingSpot ps = new ParkingSpot(index);
                        parkingSpotsList.Add(ps);
                        //tableNew.AddColumn(new TableColumn(ps.parkingSpotNr.ToString()).Centered());
                        tableNew.AddColumn(new TableColumn(new Markup($"[bold]{ps.parkingSpotNr.ToString()}[/]")).Footer($"[bold]{ps.regNr}[/]").Centered());


                    }
                    else
                    {
                        string status = parkingSpotsList.ElementAt(index - 1).status;
                        switch (status)
                        {
                            case "taken":
                                tableNew.AddColumn(new TableColumn(new Markup($"[red bold]{parkingSpotsList.ElementAt(index - 1).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(index - 1).regNr}[/]").Centered());
                                break;
                            case "halffull":
                                tableNew.AddColumn(new TableColumn(new Markup($"[yellow1 bold]{parkingSpotsList.ElementAt(index - 1).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(index - 1).regNr}[/]").Centered());
                                break;
                            default:
                                tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(index - 1).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(index - 1).regNr}[/]").Centered());
                                break;
                        }

                        //tableNew.AddColumn(new TableColumn(parkingSpotsList.ElementAt(index - 1).parkingSpotNr.ToString()).Centered());
                    }//end of if
                }//end of inner for
                tableNew.Expand();
                AnsiConsole.Render(tableNew);
            }//end of for

            int lastRowNrOfCol = garageSize % 10;
            //Console.WriteLine(" last row: " + lastRowNrOfCol);

            if (lastRowNrOfCol >= 1)
            {
                    var tableNew = new Table();
                    tableNew.Border = TableBorder.HeavyEdge;
                    tableNew.BorderColor(new Color(0, 95, 255));
                    tableNew.Alignment(Justify.Left);
                for (int it = 0; it < lastRowNrOfCol; it++)
                {
                    int index = nrOfRow * 10 + it + 1;
                    //Console.WriteLine("index of cells: " + index);
                    if (index >= parkingSpotsList.Count)
                    {
                        ParkingSpot ps = new ParkingSpot(index);
                        parkingSpotsList.Add(ps);
                        //tableNew.AddColumn(new TableColumn(ps.parkingSpotNr.ToString()).Footer("EDC").Centered());
                        tableNew.AddColumn(new TableColumn(new Markup($"[bold]{ps.parkingSpotNr.ToString()}[/]")).Footer($"[bold]{ps.regNr}[/]").Centered());
                    }
                    else
                    {
                        string status = parkingSpotsList.ElementAt(index - 1).status;
                        switch (status)
                        {
                            case "taken":
                                tableNew.AddColumn(new TableColumn(new Markup($"[red bold]{parkingSpotsList.ElementAt(index - 1).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(index - 1).regNr}[/]").Centered());
                                break;
                            case "halffull":
                                tableNew.AddColumn(new TableColumn(new Markup($"[yellow1 bold]{parkingSpotsList.ElementAt(index - 1).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(index - 1).regNr}[/]").Centered());
                                break;
                            default:
                                tableNew.AddColumn(new TableColumn(new Markup($"[bold]{parkingSpotsList.ElementAt(index - 1).parkingSpotNr.ToString()}[/]")).Footer($"[bold]{parkingSpotsList.ElementAt(index - 1).regNr}[/]").Centered());
                                break;
                        }
                        //tableNew.AddColumn(new TableColumn(parkingSpotsList.ElementAt(index - 1).parkingSpotNr.ToString()).Centered());
                        
                    }//end of if
                }//end of inner for
                    tableNew.Collapse();
                    AnsiConsole.Render(tableNew);
                    //table.AddColumn($"[bold dodgerblue2]{parkingSpot.regNr}[/]");
            }//end of if


            //Save ParkingSpotsList into a json file
            SaveListInJsonFile(parkingSpotsList);

        }//end of PrintParkingSpots


    }//end of class program 
}//end of namespace
