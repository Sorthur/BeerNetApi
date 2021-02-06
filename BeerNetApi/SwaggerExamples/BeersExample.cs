using System.Collections.Generic;
using System.Text.Json.Serialization;
using BeerNetApi.Models;
namespace BeerNetApi.SwaggerExamples
{
    /// <summary>
    /// Class made only for swagger demonstration purposes
    /// </summary>
    abstract public class BeersExample : Beer
    {
        [JsonIgnore]
        public new List<BeerRate> BeerRates { get; set; }
        public new IgnoredBeersBreweryExample Brewery { get; set; }

        abstract public class IgnoredBeersBreweryExample : Brewery
        {
            [JsonIgnore]
            public new List<Beer> Beers { get; set; }
        }
    }
}
