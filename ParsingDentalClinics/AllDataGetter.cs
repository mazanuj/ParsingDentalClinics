﻿namespace ParsingDentalClinics
{
    using System;

    using ParsingDentalClinics.Config;
    using ParsingDentalClinics.Sites;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    class AllDataGetter
    {
        public static async Task<IEnumerable<InfoHolder>> GetData()
        {
            var tasks = new List<Task<IEnumerable<InfoHolder>>>
                                {
                                    AllBizAzData.GetInfo(),
                                    AllBizKzData.GetInfo(),
                                    AstanaSpravkerData.GetInfo(),
                                    BakuSearchData.GetInfo(),
                                    BiznesinfoAz.GetInfo(),
                                    InterstomData.GetInfo(),
                                    NavigatorAzData.GetInfo(),
                                    StartsmileKzData.GetInfo(),
                                    VseKz.GetInfo(),
                                    YaMamaKzData.GetInfo()
                                };

            var allDataInArray = await Task.WhenAll(tasks);

            var resultData = new List<InfoHolder>();

            foreach (var item in allDataInArray)
            {
                resultData.AddRange(item);
            }
            return resultData;


        }
    }
}
