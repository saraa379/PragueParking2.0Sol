/* Sarantsetseg Hedenfalk
 * Inlämnings uppgift C# del 2
 * Prague Parking 2.0
 */




using System;
using Spectre.Console;
using System.Configuration; //for the XML config file
using System.Collections.Specialized;//for the XML config file
using System.Collections.Generic;//for List
using System.IO;

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

            Console.WriteLine("Parking spots will be printed here");
            var jsonString = File.ReadAllText("garage.json"); //file not found exception

        }//end of PrintParkingSpots
        /*{
            

            private readonly string _path = $"garage.json";

        //var sr = new StreamReader("garage.json");



            //string sAttr = ConfigurationManager.AppSettings.Get("CarPrice");
           //Console.WriteLine("Value for config file" + sAttr);
        }//end of PrintParkingSpots

        */


    }//end of class program 
}//end of namespace
