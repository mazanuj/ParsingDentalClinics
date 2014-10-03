using System.Collections.Generic;
using ParsingDentalClinics.Config;

namespace ParsingDentalClinics.Interfaces
{
    public interface ISiteData
    {
        List<InfoHolder> GetInfo();
    }
}