using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;
using ParsingDentalClinics.Sites;

namespace ParsingDentalClinics.Factories
{
    internal static class SiteDataFactory
    {
        public static ISiteData GetSiteData(SiteEnum site)
        {
            switch (site)
            {
                case SiteEnum.Interstom:
                    return new InterstomData();

                case SiteEnum.NavigatorAz:
                    return new NavigatorAzData();

                case SiteEnum.ZoonAz:
                    return new ZoonAzData();

                default:
                    return null;
            }
        }
    }
}