using System.Collections.Generic;
using System.Text.Json.Serialization;
using BeerNetApi.Models;
namespace BeerNetApi.SwaggerExamples
{
    /// <summary>
    /// Class made only for swagger demonstration purposes
    /// </summary>
    public class BeerRateExample : BeerRate
    {
        [JsonIgnore]
        public new Beer Beer { get; set; }
    }
}
