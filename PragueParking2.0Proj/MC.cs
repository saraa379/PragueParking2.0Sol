using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0Proj
{
    class MC : Vehicle
    {
        public MC(string regNr)
        {
            RegNr = regNr;
            Type = "mc";
        }

        public MC()
        {
            RegNr = "empty";
            Type = "mc";
        }
    }
}
