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

    internal class AstanaSpravkerData
    {
        public static async Task<IEnumerable<InfoHolder>> GetInfo()
        {
            return await Task.Run(
                () =>
                {
                    var holdersList = new List<InfoHolder>();

                    for (var i = 1; ; i++)
                    {
                        var url = string.Format("http://astana.spravker.ru/stomatologija/page-{0}/", i);
                        var doc = new HtmlWeb().Load(url);
                        if (doc.DocumentNode.OuterHtml.Contains("page-404"))
                            break;

                        var clinics = doc.DocumentNode
                            .Descendants("div")
                            .Where(x => x.Attributes.Contains("class") &&
                                        x.Attributes["class"].Value.Contains("list-item hover"));

                        holdersList.AddRange(clinics.Select(clinic =>
                        {
                            try
                            {
                                return new InfoHolder
                                {
                                    Site = SiteEnum.AstanaSpravker,
                                    Country = CountryEnum.Kazakhstan,
                                    ClinicName = RegExHelper.RegExpression(
                                        clinic.Descendants("a")
                                            .First(x => x.Attributes.Contains("href") &&
                                                        Regex.IsMatch(x.Attributes["href"].Value,
                                                            @"/stomatologija/.*\.htm.?"))
                                            .InnerText),
                                    Address = RegExHelper.RegExpression(
                                        clinic.Descendants("div")
                                            .First(x => x.Attributes.Contains("class") &&
                                                        x.Attributes["class"].Value == "row")
                                            .ChildNodes
                                            .First(x => x.Name == "div" &&
                                                        x.Attributes.Contains("class") &&
                                                        x.Attributes["class"].Value == "right")
                                            .InnerText),
                                    Phone = RegExHelper.RegExpression(
                                        clinic.Descendants("div")
                                            .Where(x => x.Attributes.Contains("class") &&
                                                        x.Attributes["class"].Value == "row")
                                            .ElementAt(1)
                                            .ChildNodes
                                            .First(x => x.Name == "div" &&
                                                        x.Attributes.Contains("class") &&
                                                        x.Attributes["class"].Value == "right")
                                            .InnerText)
                                };
                            }
                            catch
                            {
                                return new InfoHolder();
                            }
                        }).Where(x => !string.IsNullOrEmpty(x.ClinicName)));
                    }

                    return holdersList;
                });
        }
    }
}