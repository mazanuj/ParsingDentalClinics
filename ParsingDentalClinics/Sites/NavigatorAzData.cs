using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;
using ParsingDentalClinics.Utils;

namespace ParsingDentalClinics.Sites
{
    using System.Threading.Tasks;

    internal class NavigatorAzData
    {
        public static async Task<IEnumerable<InfoHolder>> GetInfo()
        {
            return await Task.Run(
                () =>
                {
                    var holdersList = new List<InfoHolder>();

                    var doc = new HtmlWeb().Load("http://www.navigator.az/catalogue/16/42");

                    var pageLast = doc.DocumentNode
                        .Descendants("a")
                        .Last(x => x.Attributes.Contains("href") && x.Attributes["href"].Value.Contains("/catalogue/16/42/page"))
                        .Attributes["href"].Value;

                    var pagesSum = int.Parse(Regex.Match(pageLast, @"(?<=page)\d*(?=\.)").Value);

                    for (var i = pagesSum; i > 0; i--)
                    {
                        doc = new HtmlWeb().Load(string.Format("http://www.navigator.az/catalogue/16/42/page{0}.html", i));

                        var clinics =
                            doc.DocumentNode.Descendants("div")
                                .Where(
                                    x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "div_gray_solid_news");

                        holdersList.AddRange(clinics.Select(clinic => new InfoHolder
                        {
                            Site = SiteEnum.NavigatorAz,
                            Country = CountryEnum.Azerbaijan,
                            ClinicName =
                                clinic.Descendants("a")
                                    .First(
                                        x =>
                                            x.Attributes.Contains("href") &&
                                            Regex.IsMatch(x.Attributes["href"].Value, @"/firm/\d*/info/"))
                                    .InnerText,
                            Address = RegExHelper.RegExpression(clinic
                                .Descendants("td")
                                .Where(
                                    x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "td_commontext_11")
                                .ElementAt(1).InnerText),
                            Phone = RegExHelper.RegExpression(clinic
                                .Descendants("td")
                                .Last(
                                    x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "td_commontext_11")
                                .InnerText)
                        }));

                        #region Fake

                        //var srcValue = doc.DocumentNode
                        //    .Descendants("script")
                        //    .First(x => x.Attributes.Contains("src") && x.Attributes["src"].Value.Contains("counter.js?"))
                        //    .Attributes["src"].Value;

                        //var adds = Regex.Matches(srcValue, @"(?<=a\[\]\=)\d*(?=\&)");

                        //foreach (var add in adds)
                        //{
                        //    using (var request = new HttpRequest())
                        //    {
                        //        request.Cookies =
                        //            request.Get(string.Format("http://www.navigator.az/catalogue/16/42/page{0}.html", i))
                        //                .Cookies;
                        //        request.Referer = string.Format("http://www.navigator.az/catalogue/16/42/page{0}.html", i);
                        //        request.UserAgent = HttpHelper.ChromeUserAgent();
                        //        var respString =
                        //            request.Get(string.Format("http://www.navigator.az/firm_info_ajax.php?id={0}", add))
                        //                .ToString();
                        //        doc.LoadHtml(respString);
                        //    }

                        //    var infoHolder = new InfoHolder
                        //    {
                        //        Site = SiteEnum.NavigatorAz,
                        //        Country = CountryEnum.Azerbaijan,
                        //        ClinicName = doc.DocumentNode
                        //            .Descendants("a")
                        //            .First(
                        //                x =>
                        //                    x.Attributes.Contains("href") &&
                        //                    x.Attributes["href"].Value == string.Format("/firm/{0}/info/", add)).InnerText
                        //    };


                        //holdersList.Add(infoHolder);
                        //}

                        #endregion
                    }
                    return holdersList;
                });
        }
    }
}