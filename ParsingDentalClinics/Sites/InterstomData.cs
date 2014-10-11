using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;
using ParsingDentalClinics.Utils;

namespace ParsingDentalClinics.Sites
{
    internal class InterstomData : ISiteData
    {
        public IEnumerable<InfoHolder> GetInfo()
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
                            holder.ClinicName = RegExHelper.RegExpression(tds[j].ChildNodes
                                .Where(y => y.InnerText != string.Empty)
                                .Aggregate(string.Empty, (current, htmlNode) => current + htmlNode.InnerText));
                            break;
                        case 1:
                            var text = RegExHelper.RegExpression(tds[j].ChildNodes
                                .Where(y => y.InnerText != string.Empty)
                                .Aggregate(string.Empty, (current, htmlNode) => current + htmlNode.InnerText));
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
                            holder.Phone = RegExHelper.RegExpression(tds[j].ChildNodes
                                .Where(y => y.InnerText != string.Empty)
                                .Aggregate(string.Empty, (current, htmlNode) => current + htmlNode.InnerText));
                            break;
                    }
                }
                holdersList.Add(holder);
            }

            return holdersList;
        }
    }
}