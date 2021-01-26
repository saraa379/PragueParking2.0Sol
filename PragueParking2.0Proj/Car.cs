using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration; //for the XML config file

namespace PragueParking2._0Proj
{
    class Car : Vehicle
    {
        public Car(string regNr)
        {
            RegNr = regNr;
            Type = "car";
            Size = GetSize();
        }
        public Car()
        {
            RegNr = "empty";
            Type = "car";
            Size = GetSize();
        }

        private int GetSize()
        {
            string sAttr = ConfigurationManager.AppSettings.Get("NrOfCarPerPSpot");
            int carSize = Int32.Parse(sAttr);
            return carSize;
        }
    }
}
