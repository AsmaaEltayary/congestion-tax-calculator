using Congestion_tax_calculator.AppDomain;
using Congestion_tax_calculator.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Congestion_tax_calculator.Infrastructure
{
    public class TollRulesDBReader(TollRulesContext dbContext) : ICityTollRulesReader
    {
        public async Task<List<CityTollFee>>ReadRules()
        {
            //move to another method then call it from actionfilter to execute before the action

            //if (dbContext.Database.GetPendingMigrations().Any())
            //{
            //    dbContext.Database.Migrate();
            //}

            List<CityTollFee> rules = [];

           var result= await dbContext.CityTollRules.Where(i => i.CityId == 1).ToListAsync();
            result.ForEach(i => rules.Add(new CityTollFee { From = i.From, To = i.To, Amount = i.Amount }));

           return rules;
        }


        
    }
}
