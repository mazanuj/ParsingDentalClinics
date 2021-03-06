﻿namespace ParsingDentalClinics.Config
{
    public class InfoHolder
    {
        public SiteEnum Site { get; set; }

        public CountryEnum Country { get; set; }

        public string ClinicName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
    }

    public enum CountryEnum
    {
        Azerbaijan,
        Kazakhstan
    }

    public enum SiteEnum
    {
        NavigatorAz,
        Interstom,
        Bakusearch,
        AllBizAz,
        BiznesinfoAz,
        YaMamaKz,
        StartsmileKz,
        AstanaSpravker,
        VseKz,
        AllBizKz
    }
}