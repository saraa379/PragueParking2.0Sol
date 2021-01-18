using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0Proj
{
    class ParkingSpot
    {
        //fields
        public int parkingSpotNr { get; set; }
        public string regNr { get; set; }
        public string status { get; set; }
        public string dateCheckedIn { get; set; }
        public int nrOfVehicle { get; set; }
        public string typeOfVehicle { get; set; }

        public ParkingSpot()
        {
            parkingSpotNr = 0;
            regNr = "empty";
            status = "empty";
            dateCheckedIn = "empty";
            nrOfVehicle = 0;
            typeOfVehicle = "empty";
        }

        public ParkingSpot(int nr)
        {
            parkingSpotNr = nr;
            regNr = "empty";
            status = "empty";
            dateCheckedIn = "empty";
            nrOfVehicle = 0;
            typeOfVehicle = "empty";
        }
    }
}
