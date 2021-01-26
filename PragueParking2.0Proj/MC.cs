using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration; //for the XML config file

namespace PragueParking2._0Proj
{
    class MC : Vehicle
    {
        public MC(string regNr)
        {
            RegNr = regNr;
            Type = GetVhType();
            Size = GetSize();
        }

        public MC()
        {
            RegNr = "empty";
            Type = GetVhType();
            Size = GetSize();
        }

        private int GetSize()
        {
            string sAttr = ConfigurationManager.AppSettings.Get("NrOfMCPerPSpot");
            int mcSize = Int32.Parse(sAttr);
            return mcSize;
        }

        private string GetVhType()
        {
            string joinedType = ConfigurationManager.AppSettings.Get("Fordonstyp");
            string[] types = joinedType.Split(',');
            return types[0];
        }
    }
}
