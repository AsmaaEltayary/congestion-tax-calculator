using Congestion_tax_calculator.AppDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Congestion_tax_calculator.Infrastructure
{
    public class GothenburgTollRulesReader : ICityTollRulesReader
    {
        

        public async Task<List<CityTollFee>> ReadRules()
        {
            List<CityTollFee> fees ;

            string fileName = "Gothenburg-TaxRules.json";

            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            var serializationOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

             
            await using FileStream createStream = File.OpenRead(path);  //jsonLocation

            fees = await JsonSerializer.DeserializeAsync<List<CityTollFee>>(createStream, serializationOptions) ?? [];
           
            return fees;
        }
    }
}
