using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Congestion_tax_calculator.AppDomain
{

    public record CityTollFee
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public int Amount { get; set; }

    }

    public interface ICityTollRulesReader
    {


        Task<List<CityTollFee>> ReadRules();


    }


    /// <summary>
    /// dates: in yyyy-MM-dd HH:mm:ss one date or ,seperated dates 2013-02-08 06:01:00,2013-02-08 08:01:00
    /// vtype:exempted(Motorcycle,Tractor,Emergency,Diplomat,Foreign,Military) other types are taxable (car,other)
    /// </summary>

    public record VehiclePassInfo
    {
        [Required]
        public List<string> Dates { get; set; }//= [];
        [Required]
        public string Vtype { get; set; } //=string.Empty;
                                          //public int TotalFess { get; set; }


    };



}
