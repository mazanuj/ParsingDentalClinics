using System;
using System.IO;
using System.Text;

namespace ParsingDentalClinics
{
    using System.Threading.Tasks;

    internal static class Program
    {
        private static void Main()
        {
            var getAllDataTask = AllDataGetter.GetData();

            Task.WaitAll(getAllDataTask);

            var resultData = getAllDataTask.Result;

            using (var sw = new StreamWriter("data.txt", true, Encoding.GetEncoding("windows-1251")))
            {
                sw.WriteLine("СТРАНА\tКЛИНИКА\tАДРЕС\tТЕЛЕФОН\tПОЧТА");
                foreach (var i in resultData)
                    sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", i.Country, i.ClinicName, i.Address, i.Phone, i.Mail);
            }

            Console.ReadKey();
        }
    }
}