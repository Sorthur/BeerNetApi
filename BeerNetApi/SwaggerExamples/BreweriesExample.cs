using System.Collections.Generic;
using System.Text.Json.Serialization;
using BeerNetApi.Models;
namespace BeerNetApi.SwaggerExamples
{
    /// <summary>
    /// Class made only for swagger demonstration purposes
    /// </summary>
    abstract public class BreweriesExample : Brewery
    {
        [JsonIgnore]
        public new List<Beer> Beers { get; set; }
    }
}
