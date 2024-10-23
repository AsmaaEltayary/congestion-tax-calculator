using Asp.Versioning;
using Congestion_tax_calculator.AppDomain;
using Microsoft.AspNetCore.Mvc;

namespace congestion_tax_calculator.Web.Controllers
{

    [ApiController]
    [ApiVersion(1)]
    [ApiVersion(2)]
    [Route("api/TaxFee/v{version:apiVersion}")]  

    public class TaxFeeController(CongestionTaxCalculator TaxCalculator,
                                  [FromKeyedServices("NonDBTollFeeRulesReader")] ICityTollRulesReader NoDBTollRulesReader,
                                  [FromKeyedServices("DBTollFeeRuesReader")] ICityTollRulesReader DBTollRulesReader) : ControllerBase
    {

        /// <summary>
        /// Calulate total tax for vehicles according to the pass dates and the vehicle type
        /// </summary>
        /// <param name="info">liste of dates in yyyy-MM-dd HH:mm:ss 2013-02-08 06:01:00  ,
        /// vtype:exempted(motorcycle,tractor,emergency,diplomat,foreign,military) other types are taxable (car,other)
        /// </param>
        /// <returns>total tax fees for all dates of the taxable vehicles rather than return 0 </returns>
        /// <remarks> 
        /// {
        /// "dates": [ "2013-02-08 06:01:00","2013-02-08 15:01:00" ],
        /// "vtype": "other"
        /// }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost()]
        public async Task<ActionResult<int>> CalculateTax(VehiclePassInfo info )
        {


            if (info.Dates.Count is 0) { return BadRequest("No valid dates"); }
                TaxCalculator.TollFeeRulesReader = NoDBTollRulesReader;
                int result = await TaxCalculator.GetTotalTax(info);
                return Ok(result);
            
            
        }

        /// <summary>
        /// Calulate total tax for vehicles Reading City toll Rules from DB after Migration to Local SQL DB
        /// </summary>
        /// <param name="info" >liste of dates in yyyy-MM-dd HH:mm:ss  2013-02-08 06:01:00 , 
        /// vtype:exempted(motorcycle,tractor,emergency,diplomat,foreign,military) other types are taxable (car,other)
        /// </param>
        /// <returns>total tax fees for all dates of the taxable vehicles rather than return 0 </returns>
        /// <remarks> 
        /// {
        /// "dates": [ "2013-02-08 06:01:00","2013-02-08 15:01:00" ],
        /// "vtype": "other"
        /// }
        /// </remarks>
        [ApiVersion(2)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost()]
        public async Task<ActionResult<int>> CalculateTaxWithDataStore(VehiclePassInfo info)
        {

            if (info.Dates.Count is 0) { return BadRequest("No valid dates"); }
            TaxCalculator.TollFeeRulesReader = DBTollRulesReader;
            int result = await TaxCalculator.GetTotalTax(info);
            return Ok(result);



        }

    }
}
