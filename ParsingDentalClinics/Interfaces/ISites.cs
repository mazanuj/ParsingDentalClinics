using System.Collections.Generic;
using ParsingDentalClinics.Config;

namespace ParsingDentalClinics.Interfaces
{
    using System.Threading.Tasks;

    public interface ISiteData
    {
        Task<IEnumerable<InfoHolder>> GetInfo();
    }
}