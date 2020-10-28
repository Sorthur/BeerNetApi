using BeerNetApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNetApi.Models
{
    public class BreweryFilter
    {
        public string BreweryName { get; set; }
        public Country? Country { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public bool IncludeBeers { get; set; }
    }
}
