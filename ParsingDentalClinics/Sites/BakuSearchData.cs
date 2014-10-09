using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;

namespace ParsingDentalClinics.Sites
{
    internal class BakuSearchData : ISiteData
    {
        public IEnumerable<InfoHolder> GetInfo()
        {
            var holdersList = new List<InfoHolder>();
            var htmlByte = new WebClient().DownloadData("http://www.bakusearch.info/russian/medical/stom_clinic.php");
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
                            infoHolder.Phone += "; " + RegExpression(value);
                        else infoHolder.Phone = RegExpression(value);
                    }
                    else if (value.Contains("e-mail"))
                        infoHolder.Mail = RegExpression(value.Replace("e-mail:", ""));
                    else if (!value.Contains("URL"))
                        infoHolder.Address = RegExpression(value);
                }
                holdersList.Add(infoHolder);
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