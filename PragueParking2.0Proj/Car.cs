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
            Type = GetVhType();
            Size = GetSize();
        }
        public Car()
        {
            RegNr = "empty";
            Type = GetVhType();
            Size = GetSize();
        }

        private int GetSize()
        {
            string sAttr = ConfigurationManager.AppSettings.Get("NrOfCarPerPSpot");
            int carSize = Int32.Parse(sAttr);
            return carSize;
        }

        private string GetVhType()
        {
            string joinedType = ConfigurationManager.AppSettings.Get("Fordonstyp");
            string[] types = joinedType.Split(',');
            return types[1];
        }
    }
}
