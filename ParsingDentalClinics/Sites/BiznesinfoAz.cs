using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Utils;

namespace ParsingDentalClinics.Sites
{
    using System;
    using System.Threading.Tasks;

    internal class BiznesinfoAz
    {
        public static async Task<IEnumerable<InfoHolder>> GetInfo()
        {
            return await Task.Run(
                () =>
                {
                    try
                    {
                        var holdersList = new List<InfoHolder>();

                        var doc =
                            new HtmlWeb().Load("http://www.biznesinfo.az/business/directory/medicine_pharmacies/dental");

                        var urlLastPage =
                            doc.DocumentNode.Descendants("a")
                                .First(
                                    x =>
                                    x.Attributes.Contains("href")
                                    && x.Attributes["href"].Value.Contains("medicine_pharmacies/dental/?page=")
                                    && x.InnerText == "&gt;&gt;")
                                .Attributes["href"].Value;
                        var numLastPage = int.Parse(Regex.Match(urlLastPage, @"\d*$").Value);

                        for (var i = numLastPage; i > 0; i--)
                        {
                            var url =
                                string.Format(
                                    "http://www.biznesinfo.az/business/directory/medicine_pharmacies/dental/?page={0}",
                                    i);
                            doc = new HtmlWeb().Load(url);

                            var clinics =
                                doc.DocumentNode.Descendants("td")
                                    .Where(
                                        x =>
                                        x.Attributes.Count == 2 && x.Attributes.Contains("height")
                                        && x.Attributes.Contains("valign") && x.Attributes["height"].Value == "50"
                                        && x.Attributes["valign"].Value == "top");

                            holdersList.AddRange(
                                clinics.Select(
                                    clinic =>
                                    new InfoHolder
                                        {
                                            Site = SiteEnum.BiznesinfoAz,
                                            Country = CountryEnum.Azerbaijan,
                                            ClinicName =
                                                RegExHelper.RegExpression(
                                                    clinic.Descendants("a")
                                                .First(
                                                    x =>
                                                    x.Attributes.Contains("href")
                                                    && x.Attributes["href"].Value == "javascript:;")
                                                .InnerText),
                                            Address =
                                                RegExHelper.RegExpression(
                                                    Regex.Match(
                                                        clinic.Descendants("div")
                                                .First(
                                                    x =>
                                                    x.Attributes.Contains("id")
                                                    && Regex.IsMatch(x.Attributes["id"].Value, @"address_\d*"))
                                                .InnerText,
                                                        @".*(?=; Tel:)").Value),
                                            Phone =
                                                RegExHelper.RegExpression(
                                                    Regex.Match(
                                                        clinic.Descendants("div")
                                                .First(
                                                    x =>
                                                    x.Attributes.Contains("id")
                                                    && Regex.IsMatch(x.Attributes["id"].Value, @"address_\d*"))
                                                .InnerText,
                                                        @"(?<=; Tel:??\s).*").Value)
                                        }));
                        }

                        return holdersList;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in BiznesinfoAz {0}", ex.Message);
                        return new List<InfoHolder>();
                    }
                });
        }
    }
}