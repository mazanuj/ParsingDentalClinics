﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;

namespace ParsingDentalClinics.Sites
{
    internal class InterstomData : ISiteData
    {
        public List<InfoHolder> GetInfo()
        {
            var holdersList = new List<InfoHolder>();

            var doc = new HtmlWeb().Load("http://interstom.narod.ru/kliniki.html");
            var trs = doc.DocumentNode.SelectNodes("//tr");

            for (var i = 2; i < trs.Count; i++)
            {
                var tds = trs[i].ChildNodes.Where(x => x.Name == "td").ToList();
                if (tds.Count != 3) continue;

                var holder = new InfoHolder {Site = SiteEnum.Interstom, Country = CountryEnum.Azerbaijan};
                for (var j = 0; j < tds.Count; j++)
                {
                    switch (j)
                    {
                        case 0:
                            holder.ClinicName = RegExpression(tds, j);
                            break;
                        case 1:
                            var text = RegExpression(tds, j);
                            if (!text.Contains(";"))
                            {
                                holder.Address = text;
                                break;
                            }

                            var start = text.IndexOf(";");
                            holder.Address = text.Remove(start);
                            holder.Mail = text.Substring(start + 1).Replace(" ", string.Empty);
                            break;
                        case 2:
                            holder.Phone = RegExpression(tds, j);
                            break;
                    }
                }
                holdersList.Add(holder);
            }

            return holdersList;
        }

        private static string RegExpression(IReadOnlyList<HtmlNode> nodeList, int index)
        {
            var text = nodeList[index].ChildNodes
                .Where(y => y.InnerText != string.Empty)
                .Aggregate(string.Empty, (current, htmlNode) => current + htmlNode.InnerText);
            text = text.Replace("&quot;", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("&nbsp;", string.Empty);
            text = Regex.Replace(text, @"\s+", " ");
            return Regex.Replace(text, @"(^\s+)|(\s+$)", string.Empty);
        }
    }
}