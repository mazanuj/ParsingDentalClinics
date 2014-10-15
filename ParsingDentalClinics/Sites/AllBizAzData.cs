using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Utils;

namespace ParsingDentalClinics.Sites
{
    using System.Threading.Tasks;

    internal static class AllBizAzData
    {
        public static async Task<IEnumerable<InfoHolder>> GetInfo()
        {
            var holdersList = new List<InfoHolder>();

            var wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.Cookie, "pager_per_page=48");
            wc.Headers.Add(HttpRequestHeader.Cookie, "test=ok");
            var html = await wc.DownloadStringTaskAsync("http://baku.all.biz/stomatologicheskie-uslugi-bsr1675");

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var ads =
                doc.DocumentNode.Descendants("div")
                    .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "b-product--list-item ");

            foreach (var url in ads.Select(ad => ad.Descendants("a")
                .First(
                    x =>
                        x.Attributes.Contains("href") &&
                        Regex.IsMatch(x.Attributes["href"].Value, @"http://\d*.az.all.biz/contacts"))
                .Attributes["href"].Value + "?show=phones"))
            {
                doc = new HtmlWeb().Load(url);
                var contacts = doc.DocumentNode.Descendants("div")
                    .First(x => x.Attributes.Contains("id") && x.Attributes["id"].Value == "contacts");

                var infoHolder = new InfoHolder
                {
                    Site = SiteEnum.AllBizAz,
                    Country = CountryEnum.Azerbaijan,
                    ClinicName = RegExHelper.RegExpression(
                        contacts.Descendants("h1")
                            .First(x => x.Attributes.Contains("itemprop") && x.Attributes["itemprop"].Value == "name")
                            .InnerText),
                    Phone =
                        RegExHelper.RegExpression(string.Join("; ",
                            contacts.Descendants("span")
                                .Where(
                                    x =>
                                        x.Attributes.Contains("itemprop") &&
                                        x.Attributes["itemprop"].Value == "telephone")
                                .Select(y => y.InnerText)
                            )),
                    Address = RegExHelper.RegExpression(string.Format("{0}, {1}, {2}",
                        contacts.Descendants("span")
                            .First(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "country-name")
                            .InnerText,
                        contacts.Descendants("span")
                            .First(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "locality")
                            .InnerText,
                        //contacts.Descendants("span")
                        //    .First(
                        //        x => x.Attributes.Contains("itemprop") && x.Attributes["itemprop"].Value == "postalCode")
                        //    .InnerText,
                        contacts.Descendants("span")
                            .First(
                                x =>
                                    x.Attributes.Contains("itemprop") &&
                                    x.Attributes["itemprop"].Value == "streetAddress")
                            .InnerText))
                };
                holdersList.Add(infoHolder);
            }
            return holdersList;
        }
    }
}