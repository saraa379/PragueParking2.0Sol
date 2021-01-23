using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0Proj
{
    class ParkingSpot
    {
        //fields
        public int parkingSpotNr { get; set; }
        public string status { get; set; }
        public string dateCheckedIn { get; set; }
        public int nrOfVehicle { get; set; }
        public Vehicle vh { get; set; }

        public ParkingSpot()
        {
            parkingSpotNr = 0;
            vh = new Vehicle();
            status = "empty";
            dateCheckedIn = "empty";
            nrOfVehicle = 0;
        }

        public ParkingSpot(int nr)
        {
            parkingSpotNr = nr;
            vh = new Vehicle();
            status = "empty";
            dateCheckedIn = "empty";
            nrOfVehicle = 0;
        }
    }
}
