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

            //PrintParkingSpots();
            GetParkingSpotsList();



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
            Console.WriteLine("array: " + jsonString);
            //getting relative directory of the project folder
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\garage.json"));
            File.WriteAllText(path, jsonString);
        }

        public static List<ParkingSpot> GetParkingSpotsList()
        {
            //getting relative directory of the json file
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\garage.json"));
            string path2 = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\garage3.json"));

            List<ParkingSpot> parkingSpotsList;

            //check if file exist before reading from it
            if (!File.Exists(path2))
            {
                Console.WriteLine("file doesn't exist");
                ParkingSpot[] parkingSpotsArray = CreateParkingSpotsList();
            }
            else
            {

                Console.WriteLine("File exist");
            }

            //reading json file and saving the content in the string variable
            var jsonString = File.ReadAllText(path);
            //Console.WriteLine("current directory" + jsonString);
            //deserialising a json string into C# object
            parkingSpotsList = JsonConvert.DeserializeObject<List<ParkingSpot>>(jsonString);
            return parkingSpotsList;
        }

       

        public static void SaveListInJsonFile(List<ParkingSpot> parkingSpots)
        {
            ParkingSpot[] parkingSpotArray = parkingSpots.ToArray();
            string jsonString = JsonConvert.SerializeObject(parkingSpotArray);

            //getting relative directory of the json file
            string directory = AppDomain.CurrentDomain.BaseDirectory + "//garage.json";
            string path = Path.GetFullPath("test/garage.json");

            //Console.WriteLine(jsonString);

            if (!File.Exists(directory))
            {
                Console.WriteLine("file doesn't exist");
            } else
            {
                //Console.WriteLine("file exist");
                //writing json string into json file, replacing the old file
                File.WriteAllText(directory, jsonString);
                //File.WriteAllText(path, jsonString);
            }
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

  

        }//end of PrintParkingSpots


    }//end of class program 
}//end of namespace
