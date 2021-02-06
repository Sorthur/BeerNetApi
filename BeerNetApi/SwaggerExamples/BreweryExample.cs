using System.Collections.Generic;
using System.Text.Json.Serialization;
using BeerNetApi.Models;
namespace BeerNetApi.SwaggerExamples
{
    /// <summary>
    /// Class made only for swagger demonstration purposes
    /// </summary>
    abstract public class BreweryExample : Brewery
    {
        public new List<BeerExample> Beers { get; set; }

        abstract public class BeerExample : Beer
        {
            [JsonIgnore]
            public new List<BeerRate> BeerRates { get; set; }
            [JsonIgnore]
            public new Brewery Brewery { get; set; }
        }
    }
}
