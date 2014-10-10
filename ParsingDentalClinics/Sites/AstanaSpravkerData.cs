using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;

namespace ParsingDentalClinics.Sites
{
    internal class AstanaSpravkerData : ISiteData
    {
        public IEnumerable<InfoHolder> GetInfo()
        {
            var holdersList = new List<InfoHolder>();

            for (var i = 1;; i++)
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
                            ClinicName = RegExpression(
                                clinic.Descendants("a")
                                    .First(x => x.Attributes.Contains("href") &&
                                                Regex.IsMatch(x.Attributes["href"].Value,
                                                    @"/stomatologija/.*\.htm.?"))
                                    .InnerText),
                            Address = RegExpression(
                                clinic.Descendants("div")
                                    .First(x => x.Attributes.Contains("class") &&
                                                x.Attributes["class"].Value == "row")
                                    .ChildNodes
                                    .First(x => x.Name == "div" &&
                                                x.Attributes.Contains("class") &&
                                                x.Attributes["class"].Value == "right")
                                    .InnerText),
                            Phone = RegExpression(
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
        }

        public string RegExpression(string textInput)
        {
            var text = textInput.Replace("&quot;", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("&nbsp;", string.Empty);
            text = Regex.Replace(text, @"\s+", " ");
            return Regex.Replace(text, @"(^\s+)|(\s+$)", string.Empty);
        }
    }
}