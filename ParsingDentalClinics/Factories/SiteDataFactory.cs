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
                case SiteEnum.Bakusearch:
                    return new BakuSearchData();
                case SiteEnum.AllBiz:
                    return new AllBizData();
                case SiteEnum.BiznesinfoAz:
                    return new BiznesinfoAz();
                case SiteEnum.YaMamaKz:
                    return new YaMamaKzData();

                default:
                    return null;
            }
        }
    }
}