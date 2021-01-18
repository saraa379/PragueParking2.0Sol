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

        public static void PrintParkingSpots(){
            //gets list of ParkingSpots from JSON file
            List<ParkingSpot> parkingSpotsList = GetParkingSpotsList();
            int listSize = parkingSpotsList.Count;
            Console.WriteLine("Size of the list: " + listSize);

            //Creating table for parkingSpots
            string sAttr = ConfigurationManager.AppSettings.Get("NrOfPSpots");
            int garageSize = Int32.Parse(sAttr);
            Console.WriteLine("Garage size is: " + garageSize);

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

            Console.WriteLine("columns: " + nrOfColumn);
            Console.WriteLine("rows: " + nrOfRow);


            //Printing table using Spectre.Console
            Console.WriteLine();

            for (int i = 0; i <= nrOfRow; i++)
            {
                var tableNew  = new Table();
                tableNew.Border = TableBorder.HeavyEdge;
                tableNew.BorderColor(new Color(0, 95, 255));
                tableNew.Centered();
                for (int it = 0; it <= nrOfColumn; it++)
                {
                    tableNew.AddColumn("Column");
                }
                tableNew.Expand();
                AnsiConsole.Render(tableNew);
            }

            int lastRowNrOfCol = garageSize % 10;
            //Console.WriteLine(" last row: " + lastRowNrOfCol);

            if (lastRowNrOfCol >= 1)
            {
                    var tableNew = new Table();
                    tableNew.Border = TableBorder.HeavyEdge;
                    tableNew.BorderColor(new Color(0, 95, 255));
                    tableNew.Alignment(Justify.Left);
                for (int it = 0; it <= lastRowNrOfCol; it++)
                    {
                        tableNew.AddColumn("Column");
                    }
                    tableNew.Collapse();
                    AnsiConsole.Render(tableNew);
                
            }
            //Adding rows to the table
            /*
            for (int it = 0; it < nrOfRow; it++)
            {
                //string rowValues = "$";
                for (int i = 1; i <= nrOfColumn; i++)
                {
                    int index = it * 10 + i;
                    if (index < listSize)
                    {
                        Console.WriteLine("index inside loop: " + index);
                        //rowValues += "\"" + parkingSpotsList.ElementAt(index-1).regNr + "\", ";
                    } else
                    {
                        ParkingSpot newParkingSpot = new ParkingSpot();
                        parkingSpotsList.Add(newParkingSpot);
                        //rowValues += "\"empty\", ";
                    }
                    
                    //Console.WriteLine("Index: " + index);
                }
                //rowValues = rowValues.Substring(0, rowValues.Length - 2);
                //Console.WriteLine("rowValues values: " + rowValues);
                //table.AddRow(rowValues);
            }*/


            /*
            foreach (var parkingSpot in parkingSpotsList)
            {
                table.AddColumn($"[bold dodgerblue2]{parkingSpot.regNr}[/]");
            }*/

            //table.Expand();
            //AnsiConsole.Render(table);


        }//end of PrintParkingSpots


    }//end of class program 
}//end of namespace
