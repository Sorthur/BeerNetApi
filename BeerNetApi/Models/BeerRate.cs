using System;
using System.Text.Json.Serialization;

namespace BeerNetApi.Models
{
    public class BeerRate
    {
        public int Id { get; set; }
        [JsonIgnore]
        public BeerNetUser User { get; set; }
        public Beer Beer { get; set; }
        public float Rate { get; set; }
        public string Description { get; set; }

        public BeerRate() { }
    }
}
