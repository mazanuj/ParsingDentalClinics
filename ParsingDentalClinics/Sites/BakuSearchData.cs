using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Utils;

namespace ParsingDentalClinics.Sites
{
    using System.Threading.Tasks;

    internal static class BakuSearchData
    {
        public static async Task<IEnumerable<InfoHolder>> GetInfo()
        {
            var holdersList = new List<InfoHolder>();
            var htmlByte = await new WebClient().DownloadDataTaskAsync("http://www.bakusearch.info/russian/medical/stom_clinic.php");
            var html = Encoding.GetEncoding("windows-1251").GetString(htmlByte);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var tables =
                doc.DocumentNode.Descendants("table")
                    .Where(
                        x =>
                            x.Attributes.Contains("width") && x.Attributes["width"].Value == "96%" &&
                            !x.Attributes.Contains("height") && x.Attributes.Contains("cellpadding"));

            foreach (var table in tables)
            {
                IEnumerable<string> td;
                try
                {
                    td =
                        table.Descendants("td")
                            .First(
                                x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "black_verdana_11")
                            .InnerText.Split('\n')
                            .Where(y => !string.IsNullOrEmpty(y));
                }
                catch
                {
                    continue;
                }

                var infoHolder = new InfoHolder
                {
                    Site = SiteEnum.Bakusearch,
                    Country = CountryEnum.Azerbaijan,
                    ClinicName = table.Descendants("div")
                        .First(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "blue_verdana_12")
                        .InnerText
                };
                foreach (var value in td)
                {
                    if (Regex.IsMatch(value, @"(тел|факс|моб)"))
                    {
                        if (!string.IsNullOrEmpty(infoHolder.Phone))
                            infoHolder.Phone += "; " + RegExHelper.RegExpression(value);
                        else infoHolder.Phone = RegExHelper.RegExpression(value);
                    }
                    else if (value.Contains("e-mail"))
                        infoHolder.Mail = RegExHelper.RegExpression(value.Replace("e-mail:", ""));
                    else if (!value.Contains("URL"))
                        infoHolder.Address = RegExHelper.RegExpression(value);
                }
                holdersList.Add(infoHolder);
            }

            return holdersList;
        }
    }
}