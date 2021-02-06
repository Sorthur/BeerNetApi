using BeerNetApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNetApi.Models
{
    public class BeerFilter
    {
        public string BeerName { get; set; }
        public string BreweryName { get; set; }
        public Country? Country { get; set; }
        public BeerStyle? BeerStyle { get; set; }
        public float? ExtractFrom { get; set; }
        public float? ExtractTo { get; set; }
        public float? AbvFrom { get; set; }
        public float? AbvTo { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
