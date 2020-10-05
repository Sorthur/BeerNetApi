using System;
namespace BeerNetApi.Models
{
    public class BeerRate
    {
        public int Id { get; set; }
        public BeerNetUser User { get; set; }
        public Beer Beer { get; set; }
        public float Rate { get; set; }
        public string Description { get; set; }

        public BeerRate() { }
    }
}
