using BeerNetApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNetApi.Models
{
    public class BeerFilter
    {
        public int? BeerId { get; set; }
        public string BeerName { get; set; }
        public string BreweryName { get; set; }
        public Country? Country { get; set; }
        public BeerStyle? BeerStyle { get; set; }
        public float? MinExtract { get; set; }
        public float? MaxExtract { get; set; }
        public float? MinAbv { get; set; }
        public float? MaxAbv { get; set; }
        public int BeersToSkip { get; set; }
        public int BeersToTake { get; set; }
    }
}
