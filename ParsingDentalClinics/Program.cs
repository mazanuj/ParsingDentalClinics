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
        }
    }
}