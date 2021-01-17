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

        public static void PrintParkingSpots(){
            //getting relative directory of the json file
            string directory = AppDomain.CurrentDomain.BaseDirectory + "//garage.json";
            
            //reading json file and saving the content in the string variable
            var jsonString = File.ReadAllText(directory);
            Console.WriteLine("current directory" + jsonString);

            //deserialaising a json string into C# object
            List<ParkingSpot> parkingSpotsList = JsonConvert.DeserializeObject<List<ParkingSpot>>(jsonString);

        }//end of PrintParkingSpots


    }//end of class program 
}//end of namespace
