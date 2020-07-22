using System;
using System.Collections.Generic;
using BeerNet.Models.Enums;

namespace BeerNet.Models
{
    public class Brewery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Country Country { get; set; }
        public List<Beer> Beers { get; set; }

        public Brewery()
        {
            Beers = new List<Beer>();
        }
    }
}
