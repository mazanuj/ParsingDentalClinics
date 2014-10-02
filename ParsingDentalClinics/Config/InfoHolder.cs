﻿namespace ParsingDentalClinics.Config
{
    public class InfoHolder
    {
        public SiteEnum Site { get; set; }

        public CountryEnum Type { get; set; }

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
        ZoonAz
    }
}