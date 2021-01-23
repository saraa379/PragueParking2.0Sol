using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0Proj
{
    class Car : Vehicle
    {
        public Car(string regNr)
        {
            RegNr = regNr;
            Type = "car";
        }
        public Car()
        {
            RegNr = "empty";
            Type = "car";
        }
    }
}
