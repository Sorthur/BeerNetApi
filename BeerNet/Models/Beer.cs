using System;
using System.Collections.Generic;
using BeerNet.Models.Enums;

namespace BeerNet.Models
{
    public class Beer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Brewery Brewery { get; set; }
        public BeerStyle Style { get; set; }
        public float Extract { get; set; }
        public float Abv { get; set; }
        public string Description { get; set; }
        public List<BeerRate> BeerRates { get; set; }
        public float AverageRating { get; set; }

        public Beer() { }
    }
}
