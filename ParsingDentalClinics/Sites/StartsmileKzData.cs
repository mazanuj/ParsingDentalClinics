﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;

namespace ParsingDentalClinics.Sites
{
    internal class StartsmileKzData : ISiteData
    {
        public IEnumerable<InfoHolder> GetInfo()
        {
            var holdersList = new List<InfoHolder>();

            var doc = new HtmlWeb().Load("http://www.startsmile.kz/search/?PAGEN_1=1");

            var pageLast = int.Parse(doc
                .DocumentNode
                .Descendants("div")
                .First(x => x.Attributes.Contains("class") &&
                            x.Attributes["class"].Value == "paginator-number")
                .Descendants("a")
                .Last(y => y.Attributes.Contains("href") &&
                           Regex.IsMatch(y.Attributes["href"].Value, @"\?PAGEN_1=\d*"))
                .InnerText);

            for (var i = pageLast; i > 0; i--)
            {
                var url = string.Format("http://www.startsmile.kz/search/?PAGEN_1={0}", i);
                doc = new HtmlWeb().Load(url);

                var clinicsUrl = doc.DocumentNode
                    .Descendants("div")
                    .First(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "clinic-list")
                    .Descendants("div")
                    .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "row")
                    .Select(x => x.Descendants("a")
                        .First(
                            y =>
                                y.Attributes.Contains("href") &&
                                Regex.IsMatch(y.Attributes["href"].Value, @"/clinic/\d*/"))
                        .Attributes["href"].Value);

                holdersList.AddRange(clinicsUrl
                    .Select(clinicUrl =>
                    {
                        doc = new HtmlWeb().Load("http://www.startsmile.kz" + clinicUrl);

                        try
                        {
                            return new InfoHolder
                            {
                                Site = SiteEnum.StartsmileKz,
                                Country = CountryEnum.Kazakhstan,
                                ClinicName = RegExpression(
                                    doc.DocumentNode
                                        .Descendants("div")
                                        .First(x => x.Attributes.Contains("class") &&
                                                    x.Attributes["class"].Value == "crumb")
                                        .ChildNodes
                                        .Last(x => x.Name == "div")
                                        .InnerText),
                                Address = RegExpression(
                                    doc.DocumentNode
                                        .Descendants("a")
                                        .First(x => x.Attributes.Contains("class") &&
                                                    x.Attributes["class"].Value == "adress")
                                        .ChildNodes
                                        .First(x => x.Name == "span")
                                        .InnerText),
                                Phone = RegExpression(
                                    doc.DocumentNode
                                        .Descendants("div")
                                        .First(x => x.Attributes.Contains("class") &&
                                                    x.Attributes["class"].Value == "tel")
                                        .ChildNodes
                                        .First(x => x.Name == "div")
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