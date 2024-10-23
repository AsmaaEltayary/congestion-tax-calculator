using Congestion_tax_calculator.AppDomain;
using Congestion_tax_calculator.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Congestion_tax_calculator.Test
{
    [TestClass]

    public class CongestionTaxCalculatorTest
    {
        public TestContext TestContext { get; set; }

        public static IEnumerable<object[]> AdditionData
        {
            get
            {
                return new[]
                {
                    //Get Tax Within 60Minutes
                    new object[] {  new VehiclePassInfo { Dates = ["2013-02-08 06:01:00", "2013-02-08 06:31:50", "2013-02-08 07:00:00"], Vtype = "other" },18},
                    //Get Tax With in different times
                    new object[] {  new VehiclePassInfo { Dates = ["2013-02-08 06:01:00", "2013-02-08 08:01:00", "2013-02-08 15:01:00"], Vtype = "other" },34 },
                    //Get Tax With in And More 60Minutes
                    new object[] {  new VehiclePassInfo { Dates = ["2013-02-08 06:30:00", "2013-02-08 07:20:00", "2013-02-08 09:00:00" ], Vtype = "other" },26 },
                    //Get Tax Above 60 SEK
                    new object[] {  new VehiclePassInfo { Dates = ["2013-02-08 07:01:00", "2013-02-08 08:20:00", "2013-02-08 14:20:00", "2013-02-08 15:25:00", "2013-02-08 16:40:00", "2013-02-08 17:50:00"], Vtype = "other" },60 },
                    //Get Tax Within For Non Taxable vehicle Type
                    new object[] {  new VehiclePassInfo { Dates = ["2013-02-08 08:20:00"], Vtype = "emergency" },0 },
                    //Get Tax In Holiday
                    new object[] {  new VehiclePassInfo { Dates = ["2013-11-01 21:00:00"], Vtype = "other" },0 },
                    //Get Tax In WeekEnd
                    new object[] {  new VehiclePassInfo { Dates = ["2013-10-06 21:00:00"], Vtype = "other" },0 },
                    //GetTaxInJuly
                    new object[] {  new VehiclePassInfo { Dates = ["2013-07-13 21:00:00"], Vtype = "other" },0 }

                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(AdditionData))]
        public async Task GetTotalTaxTest(VehiclePassInfo passInfo, int result)
        {
            int expectedValue = result;
            var tst = new CongestionTaxCalculator { TollFeeRulesReader = new GothenburgTollRulesReader() };
            int ActualValue = await tst.GetTotalTax(passInfo);


            TestContext.WriteLine($"actual value is {ActualValue}");

            Assert.AreEqual(expectedValue, ActualValue);

        }

    }
}
