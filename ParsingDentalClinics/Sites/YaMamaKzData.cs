﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;

namespace ParsingDentalClinics.Sites
{
    internal class YaMamaKzData : ISiteData
    {
        public IEnumerable<InfoHolder> GetInfo()
        {
            var holdersList = new List<InfoHolder>();

            var doc = new HtmlWeb().Load("http://www.ya-mama.kz/catalog/zdorove/stomatologii/city-3/list/page300");

            var pageLast = int.Parse(doc
                .DocumentNode
                .Descendants("div")
                .First(x => x.Attributes.Contains("class") &&
                            x.Attributes["class"].Value == "pager_list")
                .ChildNodes.First(y => y.Name == "em").InnerText);

            for (var i = pageLast; i > 0; i--)
            {
                var url = string.Format("http://www.ya-mama.kz/catalog/zdorove/stomatologii/city-3/list/page{0}", i);
                doc = new HtmlWeb().Load(url);

                var clinics = doc.DocumentNode
                    .Descendants("tr")
                    .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "");

                holdersList.AddRange(clinics.Select(clinic => new InfoHolder
                {
                    Site = SiteEnum.YaMamaKz,
                    Country = CountryEnum.Kazakhstan,
                    ClinicName = RegExpression(
                        clinic.Descendants("a")
                            .First(x => x.Attributes.Contains("id") &&
                                        x.Attributes["id"].Value == "selected_company").InnerText),
                    Address = RegExpression(
                        clinic.Descendants("a")
                            .First(x => x.Attributes.Contains("class") &&
                                        x.Attributes["class"].Value == "showmap").InnerText),
                    Phone = RegExpression(
                        clinic.Descendants("p")
                            .First(x => x.Attributes.Contains("class") &&
                                        x.Attributes["class"].Value == "phone").InnerText),

                }));
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