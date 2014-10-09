using System.Collections.Generic;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;

namespace ParsingDentalClinics.Sites
{
    internal class ZoonAzData : ISiteData
    {
        public IEnumerable<InfoHolder> GetInfo()
        {
            return null;
        }

        public string RegExpression(string textInput)
        {
            return null;
        }
    }
}