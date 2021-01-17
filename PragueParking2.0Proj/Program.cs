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

            //Creating table for parkingSpots
            string sAttr = ConfigurationManager.AppSettings.Get("NrOfPSpots");
            int garageSize = Int32.Parse(sAttr);
            //Console.WriteLine("Garage size is: " + garageSize);

            int nrOfColumn = 0;
            int nrOfRow = 0;

            if (garageSize >= 10)
            {
                nrOfRow = garageSize / 10 + 1;
                nrOfColumn = 10;
            } else
            {
                nrOfColumn = garageSize;
                nrOfRow = 1;
            }
       
            
            //Printing table using Spectre.Console
            Console.WriteLine();

            var table = new Table();
            table.Border = TableBorder.HeavyEdge;
            table.BorderColor(new Color(0, 95, 255));
            table.Centered();
            //Adding table columns
            for (int i = 0; i <= nrOfColumn; i++)
            {
                table.AddColumn("Column");
            }

            //Adding rows to the table

            for (int it = 0; it < nrOfRow; it++)
            {

                for (int i = 0; i <= nrOfColumn; i++)
                {
                    //table.AddColumn("Column");
                    //int counter = it * 10 + i; //position of a object in a list
                }
            }


            /*
            foreach (var parkingSpot in parkingSpotsList)
            {
                table.AddColumn($"[bold dodgerblue2]{parkingSpot.regNr}[/]");
            }*/

            table.Expand();
            AnsiConsole.Render(table);


        }//end of PrintParkingSpots


    }//end of class program 
}//end of namespace
