using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Congestion_tax_calculator.AppDomain
{
    public class CongestionTaxCalculator
    {
        /**
             * Calculate the total toll fee for one day
             *
             * @param vehicle - the vehicle
             * @param dates   - date and time of all passes on one day
             * @return - the total congestion tax for that day
             */

        public required ICityTollRulesReader TollFeeRulesReader { get; set; }

        List<CityTollFee> CityTollRules=[];   

        public async Task<int> GetTotalTax(VehiclePassInfo passInfo)
        {

            if (IsTollFreeVehicle(passInfo.Vtype.Trim().ToLower()))
            {
                return 0;
            }
            //read City Toll Fee Rules whether from Json file or DB
            CityTollRules = await TollFeeRulesReader.ReadRules();

            int totalTaxAmount = 0;
            List<TollFeePerDate> Fees = [];
            List<DateTime> vehiclePassdates = [];
            passInfo.Dates.ForEach(item =>vehiclePassdates.Add(DateTime.ParseExact(item.Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            
            vehiclePassdates.Sort((a, b) => a.CompareTo(b));
           
            foreach (var vehiclePassdate in vehiclePassdates)
            {
                Fees.Add(new TollFeePerDate(vehiclePassdate, GetTollFee(vehiclePassdate)));
            }
            List<DateTime> Dateswithin60Minutes=GetPassDateswithin60Minutes(Fees);

            if (Dateswithin60Minutes.Count is 0)
            {
                totalTaxAmount = Fees.Sum(i => i.Amount);

            }
            else
            {
                var HighestdateFeein60Min =Fees.Where(i => Dateswithin60Minutes.Contains(i.PassDate)).MaxBy(i => i.Amount);

                totalTaxAmount = Fees.Where(i=> !Dateswithin60Minutes.Contains(i.PassDate)||i.PassDate == HighestdateFeein60Min!.PassDate).Sum(i => i.Amount);
            }

            totalTaxAmount = totalTaxAmount > 60 ? 60 : totalTaxAmount;

            return totalTaxAmount;


        }

        private int GetTollFee(DateTime date)
        {


            if (IsTollFreeDate(date)) return 0;

            TimeSpan dateTime = date.TimeOfDay;

            var fee = CityTollRules.FirstOrDefault(item => dateTime >= TimeSpan.Parse(item.From) && dateTime <= TimeSpan.Parse(item.To))?.Amount;

            return fee ?? 0;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }
            return false;
        }

        private List<DateTime> GetPassDateswithin60Minutes(List<TollFeePerDate> Fees)
        {

            var times = Fees.Select(i=>i.PassDate).ToList();
           List<DateTime> result = new List<DateTime>();

            for (int i = 0; i < times.Count() - 1; i++)
            {
                if ((times[i + 1] - times[i]).TotalMinutes <= 60)
                {

                    result.Add(times[i]);
                    result.Add(times[i + 1]);
                }
            }
           
            return result.Distinct().ToList(); 

        }

        private bool IsTollFreeVehicle(string vehicleType)
        {

           bool result;

            if (string.IsNullOrEmpty(vehicleType))
            {
                result = false;
            }
            else
            {
                result = Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
            }

            return result;
        }


        [Flags]
        private enum TollFreeVehicles
        {
            motorcycle = 0,
            tractor = 1,
            emergency = 2,
            diplomat = 3,
            foreign = 4,
            military = 5
        }

        record TollFeePerDate(DateTime PassDate,int Amount);
    }


}
