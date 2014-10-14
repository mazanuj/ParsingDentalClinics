namespace ParsingDentalClinics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal static class Program
    {
        private static void Main()
        {
            var getAllDataTask = AllDataGetter.GetData();

            Task.WaitAll(getAllDataTask);

            var resultData = getAllDataTask.Result;

            Console.ReadLine();
        }

    }
}